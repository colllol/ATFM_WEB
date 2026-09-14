# Triển khai thông báo AI — 2026-09-14

ATFM gọi API NL2SQL từ backend, lưu thông báo vào `T_NOTIFICATION` và dùng `T_NOTIFICATION_READ` để giữ trạng thái đọc riêng cho từng tài khoản. Không gửi thao tác đọc về API NL2SQL vì trạng thái tại đó là toàn cục.

Theo yêu cầu hiện tại, `AUDIENCE_MODE=ALL_USERS`: mọi tài khoản ATFM đang hoạt động được xem thông báo AI và chi tiết/kết quả job. Quyền này bao gồm nội dung kết quả. Mỗi bản ghi AI vẫn dùng `TARGET_TYPE=1` và có target cụ thể, không giả lập ánh xạ email.

## Thứ tự triển khai

1. Chạy `20260914_AI_notification_integration.sql` trước khi build/chạy phiên bản web mới. Script tự bổ sung `SOURCE_HASH` và cột nguồn nếu thiếu; giữ hash email cũ, thay index nguồn bằng index hash duy nhất. Không cần quyền CREATE VIEW: kiểm tra quyền nằm trong hàm `AI_NOTIFICATION_PKG.CAN_RECEIVE(recipient_id, user_id)`. Script giữ package cũ qua wrapper để các caller cũ cũng dùng kiểm tra quyền hiện tại.
2. Chạy `20260914_AI_notification_integration_verify.sql`. Phần cuối kiểm tra 17 tình huống trong một transaction và rollback cả khi thành công hoặc lỗi; cần ít nhất hai tài khoản hoạt động. Không gọi package wrapper cũ trong transaction kiểm thử vì wrapper giữ hành vi `COMMIT` cũ.
3. Cấu hình appSettings `APIAINotifications` bằng endpoint đầy đủ, hiện là `http://192.168.100.136:8000/api/integration/notifications`. Endpoint job/kết quả được suy ra từ cùng địa chỉ dịch vụ tin cậy, không có cấu hình query-jobs riêng. API key ưu tiên biến môi trường `QUERY_JOB_API_KEY`, sau đó appSettings `APIAIKey` (hiện để trống). Không dùng key của báo cáo AI cũ; không đưa key vào JavaScript hoặc tài liệu triển khai.
4. Build ứng dụng bằng MSBuild .NET Framework 4.8, đăng nhập và mở chuông/danh sách AI. Backend tải các trang theo cursor tăng dần; kiểm tra `LAST_ID` tăng sau khi commit, không tăng khi import lỗi. IIS local trỏ thẳng thư mục source nên build local có hiệu lực ngay.
5. Kiểm tra bằng hai tài khoản: cùng thấy AI, tài khoản thứ nhất đọc không làm tài khoản thứ hai hết unread; logo AI mở danh sách AI; nhấn thông báo mở đúng job/kết quả.

`tools/db_config.py` đọc kết nối từ biến môi trường `ATFM_DB_USER`, `ATFM_DB_PASSWORD`, `ATFM_DB_DSN` hoặc `SlotsOracle` trong Web.config. Không in thông tin kết nối khi kiểm tra.

## Cursor và quyền truy cập

- `T_AI_NOTIFICATION_SYNC` có một dòng `SOURCE_NAME='NL2SQL'`, `LAST_ID`, `LAST_POLL_AT`, `AUDIENCE_MODE`. Cursor ban đầu bằng 0 để phát lại sự kiện cũ; migration chạy lại không reset cursor đã có.
- Backend khóa dòng cursor, upsert mỗi sự kiện bằng `SOURCE_HASH='AI_QUERY:<id>'`, lưu `job_id` tại `SOURCE_KEY`, giữ `recipient_id` tại `AI_RECIPIENT_ID`, cập nhật target và cursor trong cùng transaction. Không dùng `unread=true` để làm nguồn đồng bộ bền vững.
- Request state/list xếp lịch đồng bộ nền tối đa mỗi 15 giây; giao diện đang hiển thị polling mỗi 15 giây. Mỗi lượt nền nhập tối đa 5 trang, 100 sự kiện/trang. Đây là tác vụ theo hoạt động web, không phải daemon đảm bảo chạy khi IIS idle. Khi thiếu API key, backend vẫn reconciliation target; chưa tải được sự kiện mới từ NL2SQL.
- Cập nhật mapping/quyền không xóa lịch sử đọc. `RECONCILE_TARGETS` thêm target mới và xóa target đã mất quyền; package đọc và truy cập job còn gọi `CAN_RECEIVE` kiểm tra quyền hiện tại để chặn quyền cũ trước khi reconciliation chạy.
- `AI_NOTIFICATION_PKG` không commit: caller phải commit/rollback. `NOTIFICATION_PKG` giữ signature và commit cũ cho caller tương thích.
- Trong `ALL_USERS`, tài khoản mới hoạt động được cấp target ở lần reconciliation tiếp theo; tài khoản bị khóa mất quyền đọc ngay.

## Chuyển sang ánh xạ người nhận khi cần

Ở thời điểm kiểm tra DB ngày 2026-09-14 có 50 tài khoản, 32 hoạt động, chưa tài khoản nào có `USEREMAIL`. Logo chatbot cũ chỉ mở trang ngoài, không truyền email/guest ID từ ATFM. Vì vậy không thể suy ra mapping đúng từ phiên ATFM hiện tại.

Chế độ `MAPPED` dùng mapping quản trị trong `T_AI_RECIPIENT_MAP`; các hàng explicit, kể cả `IS_ACTIVE=0`, luôn chặn fallback email tự động. Có thể cấp cùng recipient cho nhiều USER_ID một cách có chủ đích. Nếu chưa có bất kỳ mapping explicit nào, email chỉ khớp khi duy nhất trong các tài khoản hoạt động. Email được chuẩn hóa `LOWER(TRIM(...))`; guest ID phân biệt hoa/thường và giữ nguyên chuỗi. Không đoán USER_ID từ tên tài khoản, recipient trống hoặc nội dung thông báo.

Ví dụ SQL dùng bind parameters đã được quản trị viên xác minh, không thay bằng ID/email phỏng đoán:

```sql
MERGE INTO T_AI_RECIPIENT_MAP D
USING (SELECT CASE WHEN INSTR(:recipient_id, '@') > 0
                   THEN LOWER(TRIM(:recipient_id)) ELSE :recipient_id END RECIPIENT_ID,
              :user_id USER_ID FROM DUAL) S
ON (D.RECIPIENT_ID = S.RECIPIENT_ID AND D.USER_ID = S.USER_ID)
WHEN MATCHED THEN UPDATE SET D.IS_ACTIVE = 1
WHEN NOT MATCHED THEN INSERT (RECIPIENT_ID, USER_ID, IS_ACTIVE)
VALUES (S.RECIPIENT_ID, S.USER_ID, 1);

UPDATE T_AI_NOTIFICATION_SYNC SET AUDIENCE_MODE = 'MAPPED' WHERE SOURCE_NAME = 'NL2SQL';
BEGIN AI_NOTIFICATION_PKG.RECONCILE_TARGETS; END;
/
COMMIT;
```

`T_USERS.USERID` chưa có unique constraint trong DB hiện tại nên không thêm FK từ mapping, tránh thay đổi schema tài khoản cũ. `AI_NOTIFICATION_PKG.CAN_RECEIVE` luôn đối chiếu USERID hoạt động thực tế; hàng mapping tới ID không tồn tại không cấp quyền.

## Trạng thái kết nối thực tế

Đã triển khai migration 14 block, xác nhận 6 object VALID và build Debug thành công. Script verify đạt 17 assertion và rollback fixture. Kiểm tra bổ sung với 101 thông báo tạm đạt 7 assertion phân trang, bộ lọc nguồn và badge vượt giới hạn 50 dòng popup; dữ liệu thử đã rollback.

Các lệnh kiểm tra có thể chạy lại:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tests/test_ai_notification_service.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File tests/test_ai_notification_upsert.ps1 -RunLive
```

Lệnh đầu đạt 57 kiểm tra service/HTTP loopback, không dùng DB/khóa thật. Lệnh thứ hai là opt-in dùng kết nối Oracle hiện tại: 24 assertion trên production Upsert, trạng thái đọc riêng và rollback; phiên DB mới xác nhận không còn fixture, cursor/policy không đổi. UI có 34 kiểm tra mô phỏng chuông/danh sách và 24 kiểm tra mô phỏng job/kết quả; chưa kiểm chứng thao tác trên phiên trình duyệt đã đăng nhập.

Endpoint NL2SQL hiện truy cập được nhưng trả HTTP 401 khi chưa có integration key. Cần cấu hình key backend hợp lệ để đồng bộ thông báo thật và mở job/kết quả. HTTP 401 không chứng minh luồng nhập sự kiện đã thành công.

## Quay lui

Dừng application pool, vô hiệu hóa cấu hình đồng bộ NL2SQL của bản web cũ, chạy `20260914_AI_notification_integration_rollback.sql`, sau đó khôi phục bản build trước. Script quay lui ẩn thông báo AI bằng cách thu hồi target, giữ sự kiện, cursor, mapping, lịch sử đọc và schema bổ sung. Không dùng script rollback `SOURCE_HASH` cũ. Khi triển khai lại bản sửa, chọn audience rồi reconciliation và bật cấu hình backend.
