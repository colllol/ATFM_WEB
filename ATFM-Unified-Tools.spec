# -*- mode: python ; coding: utf-8 -*-
from PyInstaller.utils.hooks import collect_all

datas = []
binaries = []
hiddenimports = ["getpass", "cryptography", "_cffi_backend"]

for package in ("openpyxl", "oracledb", "psycopg", "cryptography", "cffi"):
    try:
        package_datas, package_binaries, package_hidden = collect_all(package)
        datas += package_datas
        binaries += package_binaries
        hiddenimports += package_hidden
    except Exception:
        # PyInstaller van se phat hien package import truc tiep; mot so ban
        # dependency khong cung cap metadata cho collect_all.
        pass

a = Analysis(
    ["tools\\ATFMUnified\\main.py"],
    pathex=["tools", "tools\\ATFMUnified"],
    binaries=binaries,
    datas=datas,
    hiddenimports=hiddenimports,
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
    optimize=0,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.datas,
    [],
    name="ATFM-Data-Tools",
    icon="tools\\assets\\ATFM-Tools.ico",
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=False,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
)
