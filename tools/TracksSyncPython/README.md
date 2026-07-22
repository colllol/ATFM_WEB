# ATFM FIR Tracks Logger

Tool Python doc `public.tracks`, phan vung theo `vietnam-fir-VVHM-VVHN.geojson`,
doi chieu voi `ATFM.T_DAY_FLIGHTS_GOINGON` va ghi ket qua vao `ATFM.T_TRACKS_LOG`.

## Luong xu ly

1. Doc `public.tracks` gom `flight_id_current`, `updated_at_utc`, `last_lat`, `last_lon`.
2. Chi xu ly cac dong co toa do nam trong hoac tren bien VVHN/VVHM.
3. `flight_id_current` duoc cat tu dau chuoi den dau `-`; vi du `CES6380-2026-07-07` thanh `CES6380`.
4. Ngay doi chieu lay tu `updated_at_utc` theo `dd-mm-yyyy`, so voi `FLIGHTDATE`.
5. Chi ghi khi `CALLSIGN + DATE` match dung 1 dong trong `T_DAY_FLIGHTS_GOINGON`.
6. `PERMTYPE = O/F` duoc phep thieu `FROM_AIRP` hoac `TO_AIRP`; `LD` va cac loai khac phai co du ca hai san bay.
7. Neu nam VVHN hoac bien chung VVHN/VVHM thi `STATUS = 1`; neu nam VVHM thi `STATUS = 2`.
8. Sau moi lan sync thanh cong, tool ghi watermark `yyyy-mm-dd hh:mm:ss` de lan sau chi doc ban ghi moi hon.

Neu `T_DAY_FLIGHTS_GOINGON` co nhieu dong cung `FLIGHTNBR + FLIGHTDATE`, tool se bo qua dong do de tranh gan sai route.
Bang log duoc bao ve bang unique index `CALLSIGN + DATE`; che do verify kiem tra ca dong sai, thieu, thua va khoa trung.

## Chay bang Python

```powershell
python .\tools\TracksSyncPython\main.py --mode check
python .\tools\TracksSyncPython\main.py --mode sync
python .\tools\TracksSyncPython\main.py --mode verify
python .\tools\TracksSyncPython\main.py --mode all
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

Build ca hai ban bang lenh:

```powershell
.\tools\build_tracks_sync.ps1
```

- `tools\dist\ATFM-TracksSync.exe`: giao dien Windows, dung cac nut Kiem tra/Ghi/Xac minh va Auto theo chu ky giay.
- `tools\dist\ATFM-TracksSync-CLI.exe`: ban dong lenh, ho tro `--mode check|sync|verify|all` va `--full`.
- Toan bo file trung gian cua PyInstaller nam trong `tools\build`.

Che do Auto chay lien tuc theo thu tu Kiem tra -> Ghi -> Xac minh tren cung tap du lieu moi.
Sau khi mot luot hoan tat, tool moi dem nguoc so giay da nhap va bat dau luot ke tiep.
Auto chi dung khi dong ung dung.

Nen chay `check` truoc. Chi `sync` moi ghi `T_TRACKS_LOG` va cap nhat watermark; `check` va `verify` khong ghi du lieu nghiep vu.

## Cau hinh

Tool uu tien doc config theo thu tu:

1. Bien moi truong `TRACKS_CONFIG`.
2. File `TracksSync.local.json` cung thu muc voi exe/source.
3. File `prjApplication/App_Data/TracksSync.local.json`.

Với bản EXE trong `tools\dist`, tool tự tìm thư mục gốc project để đọc
`prjApplication\App_Data\TracksSync.local.json`. Khi triển khai riêng file EXE, cần đặt
`TracksSync.local.json` cùng thư mục với EXE.

Xem mau tai `TracksSync.sample.json`. File `*.local.json` va `*.watermark.log` khong nen dua len Git.
