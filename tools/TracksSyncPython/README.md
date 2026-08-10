# ATFM FIR Tracks Logger

Tool Python doc `public.tracks`, phan vung theo `vietnam-fir-VVHM-VVHN.geojson`,
doi chieu voi `ATFM.T_DAY_FLIGHTS_GOINGON` va ghi ket qua vao `ATFM.T_TRACKS_LOG`.

## Luong xu ly

1. Doc `public.tracks` gom `flight_id_current`, `updated_at_utc`, `last_lat`, `last_lon`.
2. Chi xu ly cac dong co toa do nam trong hoac tren bien VVHN/VVHM.
3. `flight_id_current` duoc cat tu dau chuoi den dau `-`; vi du `CES6380-2026-07-07` thanh `CES6380`.
4. Ngay doi chieu lay tu `updated_at_utc` theo `dd-mm-yyyy`, so voi `FLIGHTDATE`.
5. Neu khong tim thay `CALLSIGN + DATE` trong `T_DAY_FLIGHTS_GOINGON`, van ghi dong voi `PERMTYPE = OTHER` (chuyen bay khac), giu callsign, ngay, status, thoi gian cap nhat va toa do; thong tin chuyen bay de trong.
6. Neu co nhieu dong cung `CALLSIGN + DATE`, tool dung gio cua `updated_at_utc` de chon dong co khoang `ETD-ETA` bao gom thoi diem do. Neu khong co dung mot khoang phu hop thi bo qua; khoang qua nua dem duoc ho tro.
7. Moi dong thieu `FROM_AIRP` hoac `TO_AIRP` (ke ca `PERMTYPE = O/F`) van duoc ghi va gan `PERMTYPE = OTHER` (chuyen bay khac); dong day du thong tin giu nguyen `PERMTYPE` goc.
8. Neu nam VVHN hoac bien chung VVHN/VVHM thi `STATUS = 1`; neu nam VVHM thi `STATUS = 2`.
9. `TIME_IN` la thoi diem dau tien callsign cham FIR; `TIME_OUT` la thoi diem dau tien sau do cham FIR con lai (STATUS khac STATUS dau), neu khong chuyen FIR thi de trong. Neu `CALLSIGN + DATE` da co trong `T_TRACKS_LOG`, tool cap nhat toa do/thoi diem moi nhat va giu cac moc TIME_IN/TIME_OUT da ghi; neu dong cu la `OTHER` va lan sau tim thay chuyen bay day du, tool bo sung lai thong tin chuyen bay.
10. Sau moi lan sync thanh cong, tool ghi watermark `yyyy-mm-dd hh:mm:ss` de lan sau chi doc ban ghi moi hon.

Neu `T_DAY_FLIGHTS_GOINGON` co nhieu dong cung `FLIGHTNBR + FLIGHTDATE`, tool chi bo qua khi khong chon duoc duy nhat theo khoang `ETD-ETA`.
Bang log duoc bao ve bang unique index `CALLSIGN + DATE`. Trong mot luot `all`, verify kiem tra toan bo
du lieu cua dong vua insert va chi kiem tra toa do cua dong da ton tai; dong thieu, dong thua va khoa trung
van duoc thong ke rieng.
Lan chay dau sau khi nang cap se tu them cac cot `LAT NUMBER`, `LON NUMBER`, `TIME_IN VARCHAR2(19)`, `TIME_OUT VARCHAR2(19)`. Co the chay script
`Database/Oracle/20260723_T_TRACKS_LOG_add_lat_lon.sql` neu can nang cap schema rieng truoc khi mo tool.

De dien toa do cho cac dong log cu, chay `sync --full` hoac `all --full` mot lan sau khi nang cap. Tool backfill
cac khoa con du dieu kien va dat hai cot thanh `NOT NULL` khi khong con dong thieu toa do. Neu van con log lich
su khong the backfill, tool bao so luong va giu nguyen de tranh tu dong xoa du lieu. Cac lan chay thuong tiep
tuc dung watermark.

## Chay bang Python

```powershell
python .\tools\TracksSyncPython\main.py --mode check
python .\tools\TracksSyncPython\main.py --mode sync
python .\tools\TracksSyncPython\main.py --mode verify
python .\tools\TracksSyncPython\main.py --mode all
python .\tools\TracksSyncPython\main.py --mode config
```

Chay lai toan bo, bo qua watermark:

```powershell
python .\tools\TracksSyncPython\main.py --mode sync --full
```

Mo giao dien:

```powershell
python .\tools\TracksSyncPython\main.py
```

## Build va ban EXE

Build TracksSync, Flight Tracking API va goi chuyen may bang lenh:

```powershell
.\tools\build_tracks_sync.ps1
```

- `tools\dist\ATFM-TracksSync.exe`: giao dien Windows, co nut Test cau hinh, Kiem tra/Ghi/Xac minh, Auto va Stop.
- `tools\dist\ATFM-TracksSync-CLI.exe`: ban dong lenh, ho tro `--mode config|check|sync|verify|all` va `--full`.
- `tools\dist\ATFM-FlightTrackingApi.exe`: API doc PostgreSQL, chay doc lap voi TracksSync.
- `tools\dist\ATFM-TracksSync-package.zip`: goi chuyen may gom TracksSync GUI, API va mot file `TracksSync.local.json` dung chung.
- Toan bo file trung gian cua PyInstaller nam trong `tools\build`.

Khi mo `ATFM-TracksSync.exe`, chuong trinh tu kiem tra va khoi dong
`ATFM-FlightTrackingApi.exe` o che do an cua so console. API la tien trinh doc lap, tiep tuc chay khi dong
giao dien TracksSync. Hai file EXE phai nam cung thu muc.

Che do Auto chay lien tuc theo thu tu Kiem tra -> Ghi -> Xac minh tren cung tap du lieu moi.
Sau khi mot luot hoan tat, tool moi dem nguoc so giay da nhap va bat dau luot ke tiep.
Nut Stop dung vong Auto va thoi gian dem nguoc. Neu mot luot dang thao tac database, tool cho luot do hoan tat
an toan roi moi dung; sau do co the bam Auto de chay lai ma khong can dong ung dung.

Nen chay `check` truoc. `check` va `verify` khong ghi du lieu nghiep vu nhung van co the tao/cap nhat schema `T_TRACKS_LOG`; chi `sync` va `all` ghi log va cap nhat watermark.

## Cau hinh

Tool uu tien doc config theo thu tu:

1. Bien moi truong `TRACKS_CONFIG`.
2. File `TracksSync.local.json` cung thu muc voi exe/source.
3. File `prjApplication/App_Data/TracksSync.local.json`.

Với bản EXE trong `tools\dist`, tool tự tìm thư mục gốc project để đọc
`prjApplication\App_Data\TracksSync.local.json`. Khi triển khai riêng file EXE, cần đặt
`TracksSync.local.json` cùng thư mục với EXE.

Xem mau tai `TracksSync.sample.json`. File `*.local.json` va `*.watermark.log` khong nen dua len Git.

Section API trong cung file cau hinh:

```json
"flight_tracking_api": {
  "host": "0.0.0.0",
  "port": 5088,
  "threads": 8,
  "max_lookback_days": 7
}
```

`host = 0.0.0.0` cho phep API lang nghe tren cac card mang cua may. TracksSync tu dung
`127.0.0.1` khi goi health check noi bo. Nut **Test cau hinh** kiem tra PostgreSQL, Oracle, GeoJSON FIR
va trang thai san sang cua API, khong ghi du lieu nghiep vu.
