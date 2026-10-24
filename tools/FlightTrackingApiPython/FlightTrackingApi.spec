# -*- mode: python ; coding: utf-8 -*-
from PyInstaller.utils.hooks import collect_all

flasgger_datas, flasgger_binaries, flasgger_hiddenimports = collect_all("flasgger")

a = Analysis(
    ["main.py"],
    pathex=[],
    binaries=flasgger_binaries,
    datas=[("FlightTrackingApi.example.json", ".")] + flasgger_datas,
    hiddenimports=["psycopg_binary"] + flasgger_hiddenimports,
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
    name="ATFM-FlightTrackingApi",
    icon="..\\assets\\ATFM-Tools.ico",
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=False,
    disable_windowed_traceback=True,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
)
