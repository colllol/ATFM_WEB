# ATFM Data Tools Center

Ung dung hop nhat hai tool trong mot cua so:

- Excel Importer: doc KHH/SLOT Excel va ghi Oracle.
- FIR Tracks Logger: doc PostgreSQL `public.tracks`, phan vung FIR, doi chieu va ghi `T_TRACKS_LOG`.

## Nguyen tac an toan

- Hai panel co worker thread, trang thai, log va ket noi database rieng.
- Thu gon panel chi an giao dien; tac vu dang chay van tiep tuc.
- Dong ung dung se yeu cau Excel Importer dung o diem an toan tiep theo va dung vong Auto cua Tracks Logger.
- Cac module nghiep vu Excel Importer duoc giu nguyen tu tool goc; chi lop giao dien duoc chuyen thanh panel co the nhung.
- Tracks Logger tai su dung truc tiep ham `execute` cua `tools/TracksSyncPython/main.py`.

## Chay tu source

```powershell
python .\tools\ATFMUnified\main.py
```

## Build EXE

```powershell
.\tools\build_unified_tool.ps1
```

File dau ra: `tools\dist\ATFM-Data-Tools.exe`.

Tracks Logger van uu tien `tools\dist\TracksSync.local.json` khi chay EXE. Excel Importer nhan password Oracle tu giao dien hoac bien moi truong `ATFM_DB_PASSWORD`.
