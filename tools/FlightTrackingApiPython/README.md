# ATFM Flight Tracking API (Python)

Chuong trinh doc `public.tracks` trong PostgreSQL va cap HTTP API cho
`SLOTS/FlightTrackingMap.aspx`. API khong dung `X-API-Key`.

## Cau hinh chung

Flight Tracking API va TracksSync dung chung mot file `TracksSync.local.json`. Thu tu tim file:

1. Duong dan trong bien moi truong `TRACKS_CONFIG`.
2. `TracksSync.local.json` nam cung thu muc voi EXE/source.
3. `prjApplication/App_Data/TracksSync.local.json` khi chay trong project.

File can co section `postgres` va `flight_tracking_api`. Xem mau tai
`tools/TracksSyncPython/TracksSync.sample.json`.

## Chay source

```powershell
python -m pip install -r .\tools\TracksSyncPython\requirements.txt
python .\tools\FlightTrackingApiPython\main.py --check
python .\tools\FlightTrackingApiPython\main.py
```

Co the truyen file cu the bang `--config C:\duong-dan\TracksSync.local.json`.

Endpoint:

- `GET /health/live`: tien trinh API dang chay.
- `GET /health/ready`: API ket noi duoc PostgreSQL.
- `GET /api/v1/tracks?date=yyyy-MM-dd`: vi tri moi nhat cua tung callsign.

## Build va trien khai

Build chung voi TracksSync:

```powershell
.\tools\build_tracks_sync.ps1 -InstallDependencies
```

Giai nen `tools\dist\ATFM-TracksSync-package.zip`, sua mot file
`TracksSync.local.json`, sau do chay `ATFM-TracksSync.exe`. TracksSync se khoi dong API ngam, khong hien
cua so console. API la tien trinh doc lap va khong bi dung khi dong TracksSync.

Van co the build/chay rieng API:

```powershell
.\tools\build_flight_tracking_api.ps1 -InstallDependencies
.\ATFM-FlightTrackingApi.exe --check
.\ATFM-FlightTrackingApi.exe
```

Neu chua co config, `ATFM-FlightTrackingApi.exe --init-config` tao `TracksSync.local.json` canh EXE.

Chi mo cong 5088 cho dia chi IP cua may IIS bang firewall; khong cong khai API truc tiep ra Internet.
Trong `prjApplication/Web.config`, dat `FlightTrackingApiBaseUrl` thanh dia chi may chay API.
