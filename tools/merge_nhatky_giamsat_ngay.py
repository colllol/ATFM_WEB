from copy import deepcopy
from pathlib import Path
import re

from docx import Document


BASE = Path(__file__).resolve().parents[1] / "TaiLieu" / "NhatKyGiamSat_Ngay"
COVER = BASE / "Mau_2_Bia_Quyen_NhatKy_GiamSat_CongTac_TrienKhai.docx"
OUTPUT = BASE / "Mau_2_1_NhatKyGiamSat_Ngay.docx"
DAY_PATTERN = re.compile(r"^Mau_2_1_Ngay_(\d{2})_\d{8}\.docx$", re.IGNORECASE)


def append_document(target, source):
    target.add_page_break()
    body = target.element.body
    for child in source.element.body:
        if child.tag.endswith("sectPr"):
            continue
        body.append(deepcopy(child))


def day_files():
    items = []
    for path in BASE.glob("Mau_2_1_Ngay_*.docx"):
        match = DAY_PATTERN.match(path.name)
        if match:
            items.append((int(match.group(1)), path))
    items.sort(key=lambda item: item[0])
    expected = list(range(1, 61))
    actual = [number for number, _ in items]
    if actual != expected:
        raise ValueError(f"Danh sách ngày không đủ hoặc sai thứ tự: {actual}")
    return [path for _, path in items]


def main():
    if not COVER.exists():
        raise FileNotFoundError(f"Thiếu file bìa: {COVER}")
    merged = Document(COVER)
    for path in day_files():
        append_document(merged, Document(path))
    merged.save(OUTPUT)
    print(OUTPUT)


if __name__ == "__main__":
    main()
