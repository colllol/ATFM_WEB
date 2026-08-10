# FlightTrackingApi

Dịch vụ độc lập đọc vị trí mới nhất theo callsign từ PostgreSQL và trả JSON cho trang
`SLOTS/FlightTrackingMap.aspx`. Ứng dụng Web Forms gọi API ở phía server, vì vậy trình duyệt
không cần truy cập PostgreSQL và không gặp vấn đề CORS.

## Cấu hình

Không đưa mật khẩu thật vào source. Trên máy chạy API, đặt các biến môi trường:

```powershell
$env:ConnectionStrings__TracksPostgres = "Host=DB_HOST;Port=5432;Database=postgres;Username=USER;Password=PASSWORD;Timeout=15;Command Timeout=30;Pooling=true"
$env:FlightTracking__ApiKey = "MOT_CHUOI_BI_MAT_IT_NHAT_16_KY_TU"
$env:ASPNETCORE_URLS = "http://0.0.0.0:5088"
```

Có thể sửa `appsettings.json` cạnh file chạy thay cho biến môi trường. Biến môi trường được
ưu tiên hơn file JSON. Service sẽ từ chối khởi động nếu API key vẫn là giá trị mẫu.

## Chạy khi phát triển

```powershell
dotnet restore
dotnet run
```

Kiểm tra:

```powershell
Invoke-RestMethod http://localhost:5088/health/live
Invoke-RestMethod http://localhost:5088/health/ready
Invoke-RestMethod "http://localhost:5088/api/v1/tracks?date=2026-08-10" -Headers @{ "X-API-Key" = $env:FlightTracking__ApiKey }
```

`/health/live` chỉ kiểm tra tiến trình. `/health/ready` kiểm tra kết nối PostgreSQL. Endpoint
dữ liệu yêu cầu header `X-API-Key` và mặc định chỉ cho truy vấn hôm nay cùng 7 ngày trước đó.

## Đóng gói mang sang máy khác

Máy build cần .NET 8 SDK. Từ thư mục này chạy:

```powershell
.\build-package.ps1 -Runtime win-x64
```

File `artifacts/FlightTrackingApi-win-x64.zip` là gói self-contained; máy Windows đích không
cần cài .NET. Giải nén, cấu hình biến môi trường hoặc sửa `appsettings.json`, rồi chạy
`FlightTrackingApi.exe`. Để tạo gói Linux dùng `-Runtime linux-x64`. Nếu máy đích đã có .NET 8
Runtime, thêm `-FrameworkDependent` để giảm kích thước gói.

Build bằng Docker:

```powershell
docker build -t atfm-flight-tracking-api .
docker run --rm -p 5088:8080 `
  -e ConnectionStrings__TracksPostgres="Host=DB_HOST;..." `
  -e FlightTracking__ApiKey="MOT_CHUOI_BI_MAT_IT_NHAT_16_KY_TU" `
  atfm-flight-tracking-api
```

Chỉ mở cổng API trong mạng nội bộ/firewall cho máy IIS cần sử dụng. Nên đặt API sau HTTPS
reverse proxy khi lưu lượng đi qua mạng không tin cậy.

## Cấu hình ứng dụng ATFM Web

Trong `prjApplication/Web.config`:

```xml
<add key="FlightTrackingApiBaseUrl" value="http://MAY_API:5088" />
<add key="FlightTrackingApiKey" value="MOT_CHUOI_BI_MAT_IT_NHAT_16_KY_TU" />
<add key="FlightTrackingApiTimeoutSeconds" value="10" />
```

API key ở hai phía phải giống nhau. Tài khoản PostgreSQL nên là tài khoản chỉ có quyền
`SELECT` trên `public.tracks`.
