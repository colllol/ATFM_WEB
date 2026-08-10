# ATFM Flight Tracking API (Python)

Chương trình này chạy trên máy trung gian có thể truy cập PostgreSQL. Nó đọc bảng
`public.tracks` và cung cấp HTTP API cho `SLOTS/FlightTrackingMap.aspx`. Máy IIS chỉ cần kết
nối được tới cổng HTTP của máy trung gian, không cần cùng dải mạng với PostgreSQL và không cần
cài Npgsql/PostgreSQL driver.

API không dùng `X-API-Key`. Chỉ nên mở cổng API cho đúng địa chỉ IP của máy IIS bằng Windows
Firewall hoặc firewall mạng; không công khai trực tiếp ra Internet.

## Cấu hình và chạy source

```powershell
cd tools\FlightTrackingApiPython
python -m pip install -r requirements.txt
Copy-Item FlightTrackingApi.example.json FlightTrackingApi.local.json
# Sửa PostgreSQL host/database/username/password trong FlightTrackingApi.local.json
python main.py --check
python main.py
```

Các endpoint:

- `GET /health/live`: kiểm tra tiến trình đang chạy.
- `GET /health/ready`: kiểm tra PostgreSQL có kết nối được hay không.
- `GET /api/v1/tracks?date=yyyy-MM-dd`: lấy vị trí mới nhất của từng callsign.

Có thể dùng biến môi trường thay cho file JSON: `FLIGHT_API_PG_HOST`, `FLIGHT_API_PG_PORT`,
`FLIGHT_API_PG_DATABASE`, `FLIGHT_API_PG_USERNAME`, `FLIGHT_API_PG_PASSWORD`,
`FLIGHT_API_PG_SSLMODE`, `FLIGHT_API_HOST`, `FLIGHT_API_PORT`.

## Build EXE để chuyển sang máy khác

Trên máy Windows đã cài Python, chạy từ thư mục gốc project:

```powershell
.\tools\build_flight_tracking_api.ps1 -InstallDependencies
```

Kết quả:

- `tools\dist\ATFM-FlightTrackingApi.exe`: file chạy độc lập.
- `tools\dist\ATFM-FlightTrackingApi-package.zip`: gói chuyển máy gồm EXE, file cấu hình
  `FlightTrackingApi.local.json` có thể sửa trực tiếp và README.

Trên máy đích, giải nén ZIP, sửa các giá trị trong `FlightTrackingApi.local.json` rồi chạy:

```powershell
.\ATFM-FlightTrackingApi.exe --check
.\ATFM-FlightTrackingApi.exe
```

Nếu chỉ sao chép riêng EXE mà chưa có file cấu hình, chạy
`.\ATFM-FlightTrackingApi.exe --init-config` để tạo file mới cạnh EXE.

PyInstaller đóng kèm Python và thư viện nên máy đích không cần cài Python. EXE build trên
Windows dùng cho Windows; nếu máy trung gian chạy Linux thì build lại trên Linux.

## Cấu hình ATFM Web

Trong `prjApplication/Web.config`, đặt địa chỉ máy trung gian:

```xml
<add key="FlightTrackingApiBaseUrl" value="http://IP_MAY_TRUNG_GIAN:5088" />
<add key="FlightTrackingApiTimeoutSeconds" value="10" />
```

Trang Web Forms gọi API ở phía server. Vì vậy firewall máy API phải cho phép IP của máy IIS,
không phải IP trình duyệt người dùng.
