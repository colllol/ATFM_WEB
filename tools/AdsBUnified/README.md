# ATFM ADS-B Tools (bản hợp nhất)

`ATFM-ADS-B-Tools.exe` mở giao diện **FIR Tracks Logger** và tự chạy **Flight Tracking API** nền trong cùng EXE. Khi đóng giao diện, API cũng được dừng; không cần chạy `ATFM-FlightTrackingApi.exe` riêng.

## Cấu hình

Đặt `TracksSync.local.json` cạnh EXE để dùng chung cấu hình PostgreSQL/Oracle của Tracks Logger. Nếu cần cấu hình HTTP API riêng (host, port, threads hoặc PostgreSQL khác), đặt thêm `FlightTrackingApi.local.json` cạnh EXE theo mẫu trong `tools/FlightTrackingApiPython/FlightTrackingApi.example.json`.

Mặc định API lắng nghe cổng `5088`. Có thể kiểm tra khi ứng dụng đang mở:

```powershell
Invoke-WebRequest http://127.0.0.1:5088/health/live -UseBasicParsing
```

## Build

```powershell
.\tools\build_adsb_unified_tool.ps1
```

Kết quả: `tools\dist\ATFM-ADS-B-Tools.exe`.

Có thể chạy chỉ API khi cần vận hành dạng dịch vụ:

```powershell
.\ATFM-ADS-B-Tools.exe --api-only
```
