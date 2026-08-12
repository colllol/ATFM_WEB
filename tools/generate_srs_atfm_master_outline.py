from pathlib import Path

from docx import Document
from docx.enum.table import WD_CELL_VERTICAL_ALIGNMENT, WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Pt


ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / "TaiLieu" / "SRS_ATFM_WEB_Khung_Suon.docx"


SECTIONS = [
    ("I", "PHÂN HỆ TÍCH HỢP VÀ TỰ ĐỘNG HÓA DỮ LIỆU", [
        "Tự động cập nhật dữ liệu từ email và thư mục lưu trữ",
        "Thu thập và xử lý dữ liệu ADS-B cho khai thác O/F",
        "Tích hợp lọc SLOT vào hệ thống SLB",
        "Kết nối trực tiếp AMHS để gửi/nhận điện văn",
        "Chuẩn hóa API chia sẻ dữ liệu",
    ]),
    ("II", "PHÂN HỆ CẢNH BÁO VÀ TIỆN ÍCH HỖ TRỢ", [
        "Hệ thống cảnh báo thông minh đa kịch bản",
        "Quản lý thông tin KHB quân sự và sử dụng vùng trời",
    ]),
    ("III", "PHÂN HỆ NÂNG CẤP VÀ TỐI ƯU CHỨC NĂNG HTSLB", [
        "Mở rộng thời gian lưu trữ điện văn INBOX",
        "Cảnh báo NOPERM cho điện văn không có trong KHBN",
        "Áp dụng quy tắc 120 giờ ICAO cho hiển thị FPL",
        "Tìm kiếm chuyến bay đa trường và theo từ khóa",
        "Tìm kiếm nâng cao phục vụ báo cáo",
        "Phân loại chuyến bay Quốc nội/Quốc tế tự động",
        "Export dữ liệu chia tách ba loại file ALL, LD, OF",
        "Chức năng Edit info chuyến bay",
        "Hiển thị lịch sử thao tác của người xử lý chuyến bay",
        "Đồng bộ dữ liệu điện văn liên ngày",
        "Thống kê theo khung giờ, chặng và đường bay",
        "Cảnh báo chuyến bay không có trong phép bay/KHB ngày",
        "Hỗ trợ hủy phép bay quốc tế/quốc nội",
        "Tối ưu hiệu năng ExportBravo dưới 3 phút",
        "Chức năng F8 ép dòng trong Calendar Accepted",
        "Tối ưu Gen KHB ngày hôm sau ra AFTN dưới 3 phút",
        "Mở rộng phạm vi tra cứu INBOX trên 7 ngày",
        "Tự động cập nhật khi sửa KHB đã build",
        "Cảnh báo chuyến bay cấp sai ngày bay so với thực tế",
        "Bổ sung trường Mục đích chuyến bay trong KHB ngày",
        "Logic cảnh báo đỏ chính xác cho chuyến bay hết hiệu lực",
        "Tự động cập nhật đường bay theo đoạn từ FPL thực tế",
        "Tự động nhận diện và chuyển ngày cho chuyến bay quốc tế hạ cánh hôm sau",
        "Theo dõi kế hoạch bay hằng ngày với cập nhật liên tục",
        "Thông báo thời điểm nhận số liệu bay Đi/Đến từ Trung tâm TBHĐB",
        "Cập nhật số liệu bay từ HTSLB sang Bravo",
        "Khắc phục lỗi nhầm ngày so với thực tế",
        "Sửa nhiều chuyến bay cùng nội dung trong một thao tác",
        "Đối chiếu số liệu bay giữa HTSLB và Bravo",
        "Mở rộng tìm kiếm trên giao diện Daily Military Report",
        "Thêm lựa chọn theo tất cả tiêu chí thống kê trong báo cáo",
        "Đánh giá, so sánh số liệu của KHBHĐBN",
    ]),
    ("IV", "PHÂN HỆ PHÂN TÍCH VÀ BÁO CÁO THÔNG MINH", [
        "Biểu đồ thông tin tổng quan khai thác bay",
        "Phân tích xu hướng khai thác theo thời gian",
        "Phát hiện và cảnh báo dữ liệu bất thường",
        "Tích hợp ADS-B cho báo cáo khai thác thực tế",
        "Tổng hợp chỉ số hiệu suất bay từ ADS-B O/F",
        "Biểu đồ thống kê trạng thái chuyến bay theo tỷ lệ phần trăm",
        "Biểu đồ so sánh hoạt động bay giữa các sân bay",
        "Báo cáo chuyến bay quân sự theo tiêu chí mở rộng",
        "Báo cáo tổng hợp hoạt động bay tại tất cả sân bay dân dụng",
        "Báo cáo cất, hạ cánh tại các sân bay toàn quốc",
    ]),
    ("V", "PHÂN HỆ AI HỖ TRỢ THỐNG KÊ, TÌM KIẾM VÀ TỔNG HỢP", [
        "Xử lý ngôn ngữ tự nhiên cho truy vấn hàng không",
        "Truy vấn và tổng hợp dữ liệu tự động theo yêu cầu",
        "Tích hợp Trợ lý ảo (Chatbot) vào giao diện HTSLB",
        "Báo cáo và giám sát hiệu năng của Trợ lý ảo",
    ]),
]


def font(style, size, bold=False):
    style.font.name = "Times New Roman"
    style._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    style.font.size = Pt(size)
    style.font.bold = bold


def shade(cell, fill):
    props = cell._tc.get_or_add_tcPr()
    item = OxmlElement("w:shd")
    item.set(qn("w:fill"), fill)
    props.append(item)


def repeat_header(row):
    props = row._tr.get_or_add_trPr()
    item = OxmlElement("w:tblHeader")
    item.set(qn("w:val"), "true")
    props.append(item)


def table(doc, headers, rows):
    result = doc.add_table(rows=1, cols=len(headers))
    result.style = "Table Grid"
    result.alignment = WD_TABLE_ALIGNMENT.CENTER
    repeat_header(result.rows[0])
    for index, value in enumerate(headers):
        cell = result.rows[0].cells[index]
        cell.text = value
        shade(cell, "D9EAF7")
        for run in cell.paragraphs[0].runs:
            run.bold = True
    for values in rows:
        cells = result.add_row().cells
        for index, value in enumerate(values):
            cells[index].text = str(value)
            cells[index].vertical_alignment = WD_CELL_VERTICAL_ALIGNMENT.CENTER
    doc.add_paragraph()
    return result


def placeholder(doc, text="[Sẽ đặc tả tại bước tiếp theo]"):
    paragraph = doc.add_paragraph(text)
    paragraph.runs[0].italic = True
    paragraph.runs[0].font.color.rgb = None


def build():
    doc = Document()
    section = doc.sections[0]
    section.top_margin = Cm(2)
    section.bottom_margin = Cm(2)
    section.left_margin = Cm(2.5)
    section.right_margin = Cm(2)
    font(doc.styles["Normal"], 12)
    font(doc.styles["Title"], 20, True)
    font(doc.styles["Heading 1"], 15, True)
    font(doc.styles["Heading 2"], 13, True)
    font(doc.styles["Heading 3"], 12, True)

    cover = doc.add_paragraph()
    cover.alignment = WD_ALIGN_PARAGRAPH.CENTER
    cover.paragraph_format.space_before = Pt(55)
    run = cover.add_run("TÀI LIỆU ĐẶC TẢ YÊU CẦU PHẦN MỀM")
    run.bold = True
    run.font.name = "Times New Roman"
    run.font.size = Pt(20)
    sub = doc.add_paragraph()
    sub.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = sub.add_run("HỆ THỐNG QUẢN LÝ, ĐIỀU HÀNH BAY – ATFM_WEB")
    run.bold = True
    run.font.name = "Times New Roman"
    run.font.size = Pt(17)
    line = doc.add_paragraph()
    line.alignment = WD_ALIGN_PARAGRAPH.CENTER
    line.paragraph_format.space_before = Pt(30)
    line.add_run("KHUNG SƯỜN TÀI LIỆU – BƯỚC 1").bold = True
    info = doc.add_paragraph()
    info.alignment = WD_ALIGN_PARAGRAPH.CENTER
    info.paragraph_format.space_before = Pt(50)
    info.add_run(
        "Mã tài liệu: SRS-ATFM-WEB\n"
        "Phiên bản: 0.1 – Khung sườn\n"
        "Ngày lập: 12/08/2026\n"
        "Đơn vị: ........................................................\n"
        "Người lập: ...................................................."
    )
    doc.add_page_break()

    doc.add_heading("KIỂM SOÁT TÀI LIỆU", level=1)
    table(doc, ["Thuộc tính", "Nội dung"], [
        ("Tên tài liệu", "Đặc tả yêu cầu phần mềm ATFM_WEB"),
        ("Mã tài liệu", "SRS-ATFM-WEB"),
        ("Phiên bản", "0.1"),
        ("Trạng thái", "Khung sườn – chờ bổ sung đặc tả chi tiết"),
        ("Tài liệu đầu vào", "Plan_20-25.7.2026.docx; NHAT_KY_THAY_DOI.txt; mã nguồn ATFM_WEB"),
    ])
    doc.add_heading("Lịch sử thay đổi", level=2)
    table(doc, ["Phiên bản", "Ngày", "Nội dung", "Người thực hiện"], [
        ("0.1", "12/08/2026", "Tạo bìa và khung sườn SRS", "Codex"),
    ])
    doc.add_heading("Phê duyệt tài liệu", level=2)
    table(doc, ["Vai trò", "Họ tên", "Chữ ký", "Ngày"], [
        ("Người lập", "", "", ""),
        ("Người kiểm tra", "", "", ""),
        ("Người phê duyệt", "", "", ""),
    ])

    doc.add_heading("MỤC LỤC DỰ KIẾN", level=1)
    for item in [
        "1. Giới thiệu", "2. Tổng quan hệ thống", "3. Phạm vi và danh mục chức năng",
        "4. Yêu cầu chung", "5. Phân hệ tích hợp và tự động hóa dữ liệu",
        "6. Phân hệ cảnh báo và tiện ích hỗ trợ", "7. Phân hệ nâng cấp và tối ưu HTSLB",
        "8. Phân hệ phân tích và báo cáo thông minh", "9. Phân hệ ứng dụng AI",
        "10. Yêu cầu dữ liệu và tích hợp", "11. Yêu cầu phi chức năng",
        "12. Yêu cầu triển khai và vận hành", "13. Kiểm thử và nghiệm thu",
        "14. Ma trận truy vết", "15. Phụ lục",
    ]:
        doc.add_paragraph(item, style="List Number")
    doc.add_page_break()

    doc.add_heading("1. Giới thiệu", level=1)
    for title in ["1.1. Mục đích", "1.2. Phạm vi", "1.3. Đối tượng sử dụng tài liệu", "1.4. Thuật ngữ và từ viết tắt", "1.5. Tài liệu tham chiếu"]:
        doc.add_heading(title, level=2)
        placeholder(doc)

    doc.add_heading("2. Tổng quan hệ thống", level=1)
    for title in ["2.1. Bối cảnh nghiệp vụ", "2.2. Mục tiêu nâng cấp", "2.3. Các bên liên quan", "2.4. Nhóm người dùng", "2.5. Kiến trúc và sơ đồ ngữ cảnh", "2.6. Giả định và phụ thuộc"]:
        doc.add_heading(title, level=2)
        placeholder(doc)

    doc.add_heading("3. Phạm vi và danh mục chức năng", level=1)
    rows = []
    for roman, name, items in SECTIONS:
        for index, item in enumerate(items, 1):
            rows.append((f"{roman}.{index:02d}", name, item, "Chờ đặc tả"))
    table(doc, ["Mã", "Phân hệ", "Chức năng/hạng mục", "Trạng thái SRS"], rows)

    doc.add_heading("4. Yêu cầu chung", level=1)
    for title in ["4.1. Đăng nhập và quản lý phiên", "4.2. Phân quyền", "4.3. Tra cứu và phân trang", "4.4. Nhập liệu và kiểm tra dữ liệu", "4.5. Nhật ký và truy vết", "4.6. Xuất báo cáo/điện văn"]:
        doc.add_heading(title, level=2)
        placeholder(doc)

    chapter = 5
    for roman, name, items in SECTIONS:
        doc.add_heading(f"{chapter}. {name.title()}", level=1)
        doc.add_heading(f"{chapter}.1. Mục tiêu và phạm vi phân hệ", level=2)
        placeholder(doc)
        doc.add_heading(f"{chapter}.2. Danh mục yêu cầu chức năng", level=2)
        function_rows = []
        prefix = ["INT", "ALT", "OPT", "RPT", "AI"][chapter - 5]
        for index, item in enumerate(items, 1):
            function_rows.append((f"FR-{prefix}-{index:03d}", item, "Chờ đặc tả", "Chờ xây dựng"))
        table(doc, ["Mã yêu cầu", "Tên yêu cầu", "Nội dung", "Tiêu chí nghiệm thu"], function_rows)
        doc.add_heading(f"{chapter}.3. Quy tắc nghiệp vụ", level=2)
        placeholder(doc)
        doc.add_heading(f"{chapter}.4. Luồng xử lý và ngoại lệ", level=2)
        placeholder(doc)
        chapter += 1

    doc.add_heading("10. Yêu cầu dữ liệu và tích hợp", level=1)
    for title in ["10.1. Mô hình và từ điển dữ liệu", "10.2. Oracle và các kho dữ liệu", "10.3. ADS-B", "10.4. Email và thư mục lưu trữ", "10.5. AMHS/AFTN", "10.6. Bravo", "10.7. API chia sẻ dữ liệu", "10.8. Chuyển đổi, đối soát và lưu trữ dữ liệu"]:
        doc.add_heading(title, level=2)
        placeholder(doc)

    doc.add_heading("11. Yêu cầu phi chức năng", level=1)
    table(doc, ["Mã nhóm", "Nhóm yêu cầu", "Chỉ tiêu cần đặc tả"], [
        ("NFR-PERF", "Hiệu năng", "Thời gian phản hồi, số người dùng, thời gian export/build"),
        ("NFR-SEC", "An toàn thông tin", "Xác thực, phân quyền, bảo vệ dữ liệu, quản lý bí mật"),
        ("NFR-AVAIL", "Sẵn sàng và tin cậy", "Tỷ lệ sẵn sàng, xử lý lỗi, retry"),
        ("NFR-AUDIT", "Nhật ký và kiểm toán", "Sự kiện, nội dung, thời hạn lưu"),
        ("NFR-BACKUP", "Sao lưu và khôi phục", "RTO, RPO, chu kỳ sao lưu"),
        ("NFR-COMPAT", "Tương thích", "Trình duyệt, hệ điều hành, phiên bản Oracle"),
        ("NFR-USABLE", "Khả năng sử dụng", "Giao diện, cảnh báo, khả năng tiếp cận"),
        ("NFR-MAINT", "Khả năng bảo trì", "Cấu hình, giám sát, triển khai và rollback"),
    ])

    doc.add_heading("12. Yêu cầu triển khai và vận hành", level=1)
    for title in ["12.1. Môi trường triển khai", "12.2. Cấu hình", "12.3. Quy trình cài đặt", "12.4. Giám sát và cảnh báo", "12.5. Rollback", "12.6. Hướng dẫn vận hành"]:
        doc.add_heading(title, level=2)
        placeholder(doc)

    doc.add_heading("13. Kiểm thử và nghiệm thu", level=1)
    table(doc, ["Nhóm", "Phạm vi dự kiến", "Tiêu chí đạt"], [
        ("Chức năng", "Luồng chính, ngoại lệ và phân quyền", "Mỗi FR có tối thiểu một test case"),
        ("Tích hợp", "Oracle, ADS-B, Email, AMHS, Bravo, API", "Dữ liệu đúng và có đối soát"),
        ("Hiệu năng", "Tra cứu, export, build, đồng bộ", "Đạt chỉ tiêu NFR đã phê duyệt"),
        ("An toàn thông tin", "Xác thực, phân quyền và dữ liệu đầu vào", "Không còn lỗi nghiêm trọng"),
        ("Nghiệm thu người dùng", "Các quy trình nghiệp vụ chính", "Có biên bản và bằng chứng"),
    ])

    doc.add_heading("14. Ma trận truy vết", level=1)
    table(doc, ["Mã yêu cầu", "Thiết kế/thành phần", "Mã kiểm thử", "Bằng chứng", "Kết quả"], [
        ("[FR-...]", "[Màn hình/API/Package]", "[TC-...]", "[Ảnh/Log/Biên bản]", "Chờ kiểm thử"),
    ])

    doc.add_heading("15. Phụ lục", level=1)
    for title in ["15.1. Danh mục màn hình và URL", "15.2. Danh mục API/Package", "15.3. Danh mục bảng dữ liệu", "15.4. Ma trận phân quyền", "15.5. Danh mục báo cáo/điện văn", "15.6. Biểu mẫu bằng chứng kiểm thử"]:
        doc.add_heading(title, level=2)
        placeholder(doc)

    for item in doc.sections:
        footer = item.footer.paragraphs[0]
        footer.alignment = WD_ALIGN_PARAGRAPH.CENTER
        run = footer.add_run("Trang ")
        begin = OxmlElement("w:fldChar")
        begin.set(qn("w:fldCharType"), "begin")
        instruction = OxmlElement("w:instrText")
        instruction.set(qn("xml:space"), "preserve")
        instruction.text = "PAGE"
        end = OxmlElement("w:fldChar")
        end.set(qn("w:fldCharType"), "end")
        run._r.extend([begin, instruction, end])

    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    doc.save(OUTPUT)
    print(OUTPUT)


if __name__ == "__main__":
    build()
