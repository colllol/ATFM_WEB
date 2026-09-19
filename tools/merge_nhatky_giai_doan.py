from copy import deepcopy
from pathlib import Path

from docx import Document


BASE = Path(__file__).resolve().parents[1] / "TaiLieu" / "NhatKyTrienKhai_GiaiDoan"
FILES = [
    "Mau_1_Bia_Quyen_NhatKy_CongTac_TrienKhai.docx",
    "Mau_1_1_GD01_KhaoSat.docx",
    "Mau_1_1_GD02_PhanTich_ThietKe.docx",
    "Mau_1_1_GD03_LapTrinh_HieuChinh.docx",
    "Mau_1_1_GD04_CaiDat_KiemThuNoiBo.docx",
    "Mau_1_1_GD05_KiemThuHeThong_HoanThien.docx",
    "Mau_1_1_GD06_DaoTao_ChuyenGiao.docx",
    "Mau_1_1_GD07_NghiemThu_BanGiao.docx",
]
OUTPUT = BASE / "Mau_1_NhatKyTrienKhai_GiaiDoan.docx"


def append_document(target, source):
    target.add_page_break()
    body = target.element.body
    for child in source.element.body:
        if child.tag.endswith("sectPr"):
            continue
        body.append(deepcopy(child))


def main():
    missing = [name for name in FILES if not (BASE / name).exists()]
    if missing:
        raise FileNotFoundError("Thiếu file: " + ", ".join(missing))
    merged = Document(BASE / FILES[0])
    for name in FILES[1:]:
        append_document(merged, Document(BASE / name))
    merged.save(OUTPUT)
    print(OUTPUT)


if __name__ == "__main__":
    main()
