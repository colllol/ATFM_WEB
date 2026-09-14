# Thông báo ATFM và tích hợp AI

Cập nhật: 2026-09-14.

Backend của luồng thông báo là repo `C:\Users\Admin\Documents\GitHub\API`, ứng dụng `QLB.API`. Trình duyệt gọi handler cùng nguồn của ATFM Web; handler lấy USER_ID từ phiên đăng nhập rồi chuyển yêu cầu sang QLB.API. QLB.API đồng bộ dữ liệu nguồn vào Oracle và gọi NL2SQL để lấy chi tiết/kết quả job.

## Thành phần và ranh giới quyền

- `QLB.API/Controllers/System/NotificationsController.cs`: các action `GetState`, `GetPage`, `MarkRead`, `MarkAllRead`, `GetAiJob`, `GetAiResult`, `Create`, đều bảo vệ bằng `ApiKeyAuthorize`.
- `QLB.API/Data/System/NotificationsRepository.cs` và `QLB.BusinessLogic/System/NotificationsDAL.cs`: đọc/đánh dấu thông báo qua package Oracle; trả tổng unread toàn hệ thống và unread AI theo người dùng.
- `QLB.API/Data/System/AiNotificationService.cs`: đồng bộ NL2SQL, cursor bền vững, target, kiểm tra quyền trước khi lấy job/kết quả.
- `QLB.API/Data/System/EmailNotificationService.cs`: đồng bộ email từ endpoint `APIEmail`, giữ nguồn email trong chuông chung.
- Web repo `prjApplication/Handlers/Notification.ashx.cs`: proxy mỏng, dùng `ATFM_CURRENT_USER` của phiên đã xác thực; không lấy USER_ID từ tham số trình duyệt và không kết nối Oracle/NL2SQL trực tiếp.

API nhận `userId` từ một caller backend đã có API key. `ApiKeyAuthorize` xác thực dịch vụ gọi, không tự xác thực phiên người dùng trình duyệt. Vì thế key web→QLB.API phải nằm ở server. Action job nhận ID thông báo ATFM, tìm `job_id` đã lưu và kiểm tra target cùng `AI_NOTIFICATION_PKG.CAN_RECEIVE` trước khi gọi nguồn; không cho trình duyệt chọn URL job tùy ý.

## Cấu hình đúng nơi

Hai key có mục đích riêng:

1. **ATFM Web → QLB.API**: Web.config của web có `ApplicationPath.API` và `APIKey`. Giá trị base URL hiện tại là `http://172.29.187.90:8888/ATFM_API/`. Web gửi key bằng `X-API-Key`. Bộ lọc API ưu tiên biến môi trường `ATFM_API_KEY`, sau đó appSettings `APIKey`; key hiệu lực phải khớp key của web.
2. **QLB.API → NL2SQL**: Web.config/môi trường của QLB.API có `APIAINotifications`, hiện là `http://192.168.100.136:8000/api/integration/notifications`; API key ưu tiên biến môi trường `QUERY_JOB_API_KEY`, sau đó appSettings `APIAIKey` hiện để trống. Endpoint job/kết quả suy ra từ cùng địa chỉ dịch vụ tin cậy, không cấu hình riêng. Không dùng key báo cáo AI cũ hoặc key web→QLB.API để thay integration key.

Oracle của notification dùng **appSettings `ConnectDB` trong QLB.API/Web.config**. Đã đối chiếu `ConnectDB` trỏ cùng DB/user với `SlotsOracle` của web, nơi migration đã triển khai. connectionStrings `ConnectionString` trong API hiện trỏ DB khác; không dùng thay `ConnectDB` cho luồng này. `APIEmail` là endpoint email đã chuyển từ cấu hình web sang backend API.

Không đưa key/connection string vào JavaScript, output kiểm tra, log hoặc tài liệu. Cấu hình AI nằm ở QLB.API sau khi chuyển luồng; thay cấu hình AI trong web sẽ không cấu hình dịch vụ đồng bộ API.

## Đồng bộ và trạng thái đọc

Theo yêu cầu hiện tại, `T_AI_NOTIFICATION_SYNC.AUDIENCE_MODE='ALL_USERS'`: mọi tài khoản ATFM hoạt động được xem thông báo AI và kết quả job. Đã kiểm tra có 32 tài khoản hoạt động. Mỗi thông báo AI vẫn dùng `TARGET_TYPE=1` với target cụ thể; `T_NOTIFICATION_READ` giữ trạng thái riêng từng tài khoản. Không gọi endpoint đánh dấu đọc NL2SQL vì trạng thái ở đó là toàn cục.

Request `GetState`/`GetPage` kích hoạt tác vụ nền tối đa mỗi 15 giây; UI đang hiển thị polling 15 giây. Mỗi lượt đồng bộ AI xử lý tối đa 5 trang, 100 sự kiện/trang. Đây là tác vụ theo hoạt động request, không bảo đảm chạy khi IIS idle. Khi thiếu integration key, backend vẫn reconciliation target nhưng chưa nhập được sự kiện mới từ NL2SQL.

Cursor `LAST_ID`, sự kiện `SOURCE_HASH='AI_QUERY:<id>'` và target commit trong cùng transaction. `SOURCE_KEY` lưu job ID, `AI_RECIPIENT_ID` giữ recipient gốc. Không dùng `unread=true` cho nguồn đồng bộ bền vững. Reconciliation giữ lịch sử đọc; quyền bị thu hồi có hiệu lực ngay qua `CAN_RECEIVE` dù target cũ chưa được dọn.

`MAPPED` vẫn có sẵn khi cần giới hạn recipient: explicit mapping được ưu tiên, email chỉ tự khớp khi duy nhất trong tài khoản hoạt động, guest ID phân biệt hoa/thường. Xem hướng dẫn schema trong web repo để chuyển chế độ; hiện không cần bổ sung mapping cho `ALL_USERS`.

## Triển khai

1. Schema chuẩn nằm trong **web repo**, không tạo bản SQL thứ hai trong API: `C:\Users\Admin\Documents\APP_V2\APP\Database\Oracle\20260914_AI_notification_integration.sql`, `_verify.sql`, `_rollback.sql` và file `.md` cùng tên. Migration 14 block đã chạy; 6 object VALID. Việc chuyển code sang QLB.API không cần migration mới.
2. Cấu hình backend API đúng `ConnectDB`, key web→API, `APIAINotifications`, integration key và `APIEmail`; build QLB.API .NET Framework 4.8. Project hiện dùng ODAC x64; chạy application pool 64-bit phù hợp.
3. **Triển khai QLB.API trước, sau đó ATFM Web.** Web đang trỏ API tại `172.29.187.90:8888/ATFM_API/`; build repo API trên máy phát triển chưa cập nhật server này. Phiên làm việc chưa triển khai server API từ xa.
4. Sau khi API server có bản mới, kiểm tra request có key từ web, danh sách tất cả/AI, bộ đếm và đọc riêng bằng hai tài khoản. Bấm thông báo AI phải mở đúng job; chỉ tải kết quả khi người dùng yêu cầu.

IIS local của web trỏ source nên build web local có hiệu lực ngay. Cấu hình phát triển API có IISUrl `http://localhost/QLB.API`; cổng dev 50590 chưa có listener ở lần kiểm tra. Không đổi URL web sang API local chỉ dựa trên cấu hình dev.

## Bằng chứng kiểm tra và giới hạn

Từ thư mục gốc repo API:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tests/test_ai_notification_service.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File tests/test_ai_notification_upsert.ps1 -RunLive
```

Đã đạt 57 kiểm tra service/HTTP loopback và 24 assertion production Upsert/Oracle với rollback; phiên DB mới xác nhận hết fixture, cursor/policy không đổi. Script schema đạt 17 assertion rollback. Kiểm tra thêm 101 thông báo tạm đạt 7 assertion phân trang/lọc nguồn/badge vượt 50 dòng; fixture đã rollback. Kiểm thử proxy web mới được thực hiện riêng trong web repo.

Endpoint NL2SQL truy cập được nhưng trả HTTP 401 khi chưa có integration key. Những kiểm tra trên chưa xác nhận đồng bộ thông báo thật hoặc mở kết quả thật từ NL2SQL. Cần key hợp lệ trên **server QLB.API** và triển khai API remote trước khi nghiệm thu toàn luồng qua web hiện tại.

## Quay lui

Phối hợp hai ứng dụng: dừng/vô hiệu hóa polling ở API, dùng script rollback schema trong web repo nếu cần thu hồi hiển thị AI, rồi khôi phục các bản build tương thích. Rollback schema giữ sự kiện, cursor, mapping và lịch sử đọc; không chạy rollback `SOURCE_HASH` cũ. Khi bật lại bản sửa, chọn audience, reconciliation và bật cấu hình API.
