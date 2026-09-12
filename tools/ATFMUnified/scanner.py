from __future__ import annotations

import re
from pathlib import Path


EXCEL_SUFFIXES = {".xlsx", ".xlsm"}
DEFAULT_FOLDER_NAMES = {"KHH", "SLOT CHK"}


def natural_key(value: str) -> list[object]:
    return [int(part) if part.isdigit() else part.casefold() for part in re.split(r"(\d+)", value)]


def scan_folders(root: Path) -> list[tuple[Path, list[Path]]]:
    if not root.is_dir():
        raise ValueError(f"Thư mục không tồn tại: {root}")
    result: list[tuple[Path, list[Path]]] = []
    folders = (
        p
        for p in root.iterdir()
        if p.is_dir() and p.name.strip().upper() in DEFAULT_FOLDER_NAMES
    )
    for folder in sorted(folders, key=lambda p: natural_key(p.name)):
        files = sorted(
            (
                p
                for p in folder.rglob("*")
                if p.is_file()
                and p.suffix.casefold() in EXCEL_SUFFIXES
                and not p.name.startswith("~$")
            ),
            key=lambda p: natural_key(str(p.relative_to(folder))),
        )
        result.append((folder, files))
    return result

