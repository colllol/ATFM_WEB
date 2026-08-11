from pathlib import Path

from docx import Document
from docx.enum.table import WD_CELL_VERTICAL_ALIGNMENT, WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Pt


ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / "TaiLieu" / "SRS_Live_Fire_Message.docx"


def set_font(style, size, bold=False):
    style.font.name = "Times New Roman"
    style._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    style.font.size = Pt(size)
    style.font.bold = bold


def shade(cell, fill="D9EAF7"):
    properties = cell._tc.get_or_add_tcPr()
    element = OxmlElement("w:shd")
    element.set(qn("w:fill"), fill)
    properties.append(element)


def repeat_header(row):
    properties = row._tr.get_or_add_trPr()
    element = OxmlElement("w:tblHeader")
    element.set(qn("w:val"), "true")
    properties.append(element)


def add_table(document, headers, rows):
    result = document.add_table(rows=1, cols=len(headers))
    result.style = "Table Grid"
    result.alignment = WD_TABLE_ALIGNMENT.CENTER
    repeat_header(result.rows[0])
    for index, value in enumerate(headers):
        cell = result.rows[0].cells[index]
        cell.text = value
        shade(cell)
        for run in cell.paragraphs[0].runs:
            run.bold = True
    for row in rows:
        cells = result.add_row().cells
        for index, value in enumerate(row):
            cells[index].text = str(value)
            cells[index].vertical_alignment = WD_CELL_VERTICAL_ALIGNMENT.CENTER
    document.add_paragraph()


def add_bullets(document, items):
    for item in items:
        document.add_paragraph(item, style="List Bullet")


def build_document():
    document = Document()
    section = document.sections[0]
    section.top_margin = Cm(2)
    section.bottom_margin = Cm(2)
    section.left_margin = Cm(2.5)
    section.right_margin = Cm(2)

    set_font(document.styles["Normal"], 12)
    set_font(document.styles["Title"], 20, True)
    set_font(document.styles["Heading 1"], 15, True)
    set_font(document.styles["Heading 2"], 13, True)

    title = document.add_paragraph()
    title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    title.paragraph_format.space_before = Pt(80)
    run = title.add_run("TÀI LIỆU ĐẶC TẢ YÊU CẦU PHẦN MỀM")
    run.bold = True
    run.font.name = "Times New Roman"
    run.font.size = Pt(20)
    subtitle = document.add_paragraph()
    subtitle.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = subtitle.add_run("TẠO VÀ PHÊ DUYỆT LIVE FIRE MESSAGE")
    run.bold = True
    run.font.name = "Times New Roman"
    run.font.size = Pt(18)
    info = document.add_paragraph()
    info.alignment = WD_ALIGN_PARAGRAPH.CENTER
    info.paragraph_format.space_before = Pt(24)
    info.add_run(
        "Hệ thống: ATFM_WEB\nPhân hệ: MessManagement\n"
        "Phiên bản tài liệu: 1.1\nNgày cập nhật: 11/08/2026"
    )
    document.add_page_break()

    document.add_heading("1. Thông tin chung", level=1)
    add_table(document, ["Nội dung", "Giá trị"], [
        ("Tên tính năng", "Quản lý điện văn bắn đạn thật"),
        ("Màn hình tạo điện văn", "/MessManagement/LiveFireMessage.aspx?Menu_ID=989"),
        ("Màn hình phê duyệt", "/MessManagement/LiveFireMessageAccepted.aspx?Menu_ID=990"),
        ("Đối tượng sử dụng", "Người nhập điện văn; người phê duyệt"),
        ("Hệ thống", "ATFM_WEB"),
        ("Phân hệ", "MessManagement"),
    ])

    document.add_heading("2. Mục đích và phạm vi", level=1)
    document.add_paragraph(
        "Tính năng hỗ trợ lập, lưu nháp, gửi duyệt, phê duyệt và từ chối điện văn "
        "bắn đạn thật; đồng thời cho phép tra cứu điện văn theo ngày, trạng thái và từ khóa."
    )
    add_bullets(document, [
        "Tạo mới và lưu điện văn ở trạng thái Nháp.",
        "Chỉnh sửa điện văn Nháp hoặc điện văn bị Từ chối.",
        "Gửi điện văn sang bộ phận phê duyệt.",
        "Xem, duyệt hoặc từ chối điện văn kèm lý do.",
        "Tra cứu và phân trang danh sách điện văn.",
    ])
    document.add_paragraph(
        "Luồng export điện văn đã duyệt sang T_PLAN_MESSAGE được hiển thị trên màn hình "
        "phê duyệt nhưng không thuộc phạm vi kiểm định chính của tài liệu này."
    )

    document.add_heading("3. Vai trò và phân quyền", level=1)
    add_table(document, ["Vai trò", "Quyền", "Chức năng"], [
        ("Người nhập điện văn", "R_Add", "Tạo điện văn mới"),
        ("Người cập nhật điện văn", "R_Edit", "Sửa, lưu nháp và gửi duyệt"),
        ("Người phê duyệt", "R_Pub", "Duyệt hoặc từ chối điện văn"),
        ("Người xem", "Quyền truy cập menu", "Tra cứu và xem chi tiết"),
    ])
    document.add_paragraph(
        "Hệ thống phải kiểm tra quyền tại phía máy chủ. Người dùng không có quyền tương ứng "
        "không được thực hiện thao tác bằng cách gọi trực tiếp API."
    )

    document.add_heading("4. Trạng thái và luồng xử lý", level=1)
    add_table(document, ["Giá trị", "Trạng thái", "Ý nghĩa"], [
        ("0", "Nháp", "Điện văn đang được nhập hoặc chỉnh sửa"),
        ("1", "Đã duyệt", "Điện văn đã được người có thẩm quyền duyệt"),
        ("2", "Chờ duyệt", "Điện văn đã gửi sang màn hình phê duyệt"),
        ("3", "Từ chối", "Điện văn không được duyệt và có lý do từ chối"),
        ("4", "Đã export", "Điện văn đã được chuyển sang T_PLAN_MESSAGE"),
    ])
    document.add_paragraph(
        "Luồng chính: Tạo mới → Nháp → Chờ duyệt → Đã duyệt. Nếu bị từ chối: "
        "Chờ duyệt → Từ chối → chỉnh sửa → Chờ duyệt. Điện văn chỉ được chỉnh sửa "
        "khi ở trạng thái Nháp hoặc Từ chối."
    )

    document.add_heading("5. Chức năng tạo Live Fire Message", level=1)
    document.add_heading("5.1. Thông tin chức năng", level=2)
    add_table(document, ["Nội dung", "Mô tả"], [
        ("Mã chức năng", "LF-CREATE"),
        ("Tác nhân", "Người nhập điện văn"),
        ("Tiền điều kiện", "Đã đăng nhập và có quyền truy cập menu"),
        ("Kết quả", "Điện văn được lưu nháp hoặc chuyển sang Chờ duyệt"),
    ])
    document.add_heading("5.2. Tra cứu điện văn", level=2)
    document.add_paragraph(
        "Cho phép tìm kiếm theo Từ ngày, Đến ngày, trạng thái Nháp/Từ chối, từ khóa "
        "và số dòng mỗi trang (20, 50 hoặc 100). Từ khóa áp dụng cho mã điện văn, "
        "tiêu đề, địa điểm hoặc người nhập. Không tìm kiếm nếu Từ ngày lớn hơn Đến ngày."
    )
    document.add_heading("5.3. Dữ liệu điện văn", level=2)
    add_table(document, ["Nhóm", "Trường dữ liệu", "Yêu cầu"], [
        ("Thông tin chung", "Ngày điện văn", "Bắt buộc; định dạng ngày"),
        ("Thông tin chung", "Mã điện văn", "Không bắt buộc; tối đa 100 ký tự"),
        ("Thông tin chung", "Tiêu đề", "Bắt buộc; tối đa 300 ký tự"),
        ("Thông tin chung", "Nội dung mở đầu", "Không bắt buộc; tối đa 2.000 ký tự"),
        ("Thông tin chung", "Địa điểm bắn", "Bắt buộc; tối đa 1.000 ký tự"),
        ("Tọa độ", "Tên điểm, vĩ độ, kinh độ", "Cho phép nhiều dòng; thêm hoặc xóa"),
        ("Thông số bắn", "Phương vị, độ cao, cự ly", "Không bắt buộc"),
        ("Thời gian bắn", "Từ giờ, đến giờ, ngày, độ cao", "Nhiều dòng; giờ HH:mm"),
        ("Chỉ huy", "Cấp bậc, họ tên, số điện thoại", "Không bắt buộc"),
        ("Người thay thế", "Cấp bậc, họ tên, số điện thoại", "Không bắt buộc"),
        ("Khác", "Hạn chế/cấm bay; người ký", "Không bắt buộc"),
    ])
    document.add_paragraph(
        "Tiêu đề mặc định là “THONG BAO BAN DAN THAT”. Hệ thống khởi tạo bốn điểm "
        "tọa độ D1, D2, D3, D4 và một dòng thời gian bắn."
    )
    document.add_heading("5.4. Xem trước nội dung", level=2)
    document.add_paragraph(
        "Hệ thống tự động tạo nội dung xem trước khi dữ liệu biểu mẫu thay đổi. "
        "Người dùng không được sửa trực tiếp ô xem trước."
    )
    document.add_heading("5.5. Lưu nháp và gửi duyệt", level=2)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-LF-01", "Có R_Add khi tạo hoặc R_Edit khi sửa; trạng thái Nháp/Từ chối; có ngày, tiêu đề, địa điểm và khung giờ hợp lệ. Lưu thành công ở trạng thái Nháp."),
        ("FR-LF-02", "Có R_Edit; điện văn đã lưu, không còn thay đổi chưa lưu và đang Nháp/Từ chối. Sau xác nhận, chuyển thành Chờ duyệt và khóa chỉnh sửa."),
    ])

    document.add_heading("6. Chức năng Approve Live Fire Message", level=1)
    document.add_heading("6.1. Tra cứu và xem chi tiết", level=2)
    document.add_paragraph(
        "Cho phép tìm theo ngày, trạng thái Chờ duyệt/Đã duyệt/Đã export/Tất cả, từ khóa "
        "và số dòng. Trạng thái mặc định là Chờ duyệt. Chi tiết điện văn tại màn hình "
        "phê duyệt chỉ được xem, không được chỉnh sửa."
    )
    document.add_heading("6.2. Duyệt và từ chối", level=2)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-LF-03", "Chỉ người có R_Pub được duyệt điện văn Chờ duyệt. Sau xác nhận, chuyển thành Đã duyệt; ghi nhận người, thời điểm và nội dung duyệt; khóa chỉnh sửa."),
        ("FR-LF-04", "Chỉ người có R_Pub được từ chối điện văn Chờ duyệt. Lý do là bắt buộc. Điện văn chuyển thành Từ chối để chỉnh sửa và gửi lại."),
    ])

    document.add_heading("7. Quy tắc nghiệp vụ", level=1)
    add_table(document, ["Mã", "Quy tắc"], [
        ("BR-LF-01", "Chỉ điện văn Nháp hoặc Từ chối mới được chỉnh sửa."),
        ("BR-LF-02", "Phải lưu các thay đổi trước khi gửi duyệt."),
        ("BR-LF-03", "Chỉ điện văn Chờ duyệt mới được duyệt hoặc từ chối."),
        ("BR-LF-04", "Từ chối điện văn bắt buộc phải có lý do."),
        ("BR-LF-05", "Điện văn Đã duyệt không được sửa nội dung."),
        ("BR-LF-06", "Mọi thao tác phải kiểm tra quyền ở phía máy chủ."),
        ("BR-LF-07", "Phải kiểm tra VERSION_NO trước khi cập nhật hoặc đổi trạng thái."),
        ("BR-LF-08", "Nếu dữ liệu đã thay đổi, từ chối thao tác trên phiên bản cũ và yêu cầu tải lại."),
        ("BR-LF-09", "Từ ngày tìm kiếm không được lớn hơn Đến ngày."),
        ("BR-LF-10", "Giờ bắn đúng HH:mm và giờ bắt đầu nhỏ hơn giờ kết thúc."),
    ])

    document.add_heading("8. Yêu cầu phi chức năng", level=1)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("NFR-LF-01", "Kiểm tra đăng nhập và phân quyền khi thay đổi dữ liệu."),
        ("NFR-LF-02", "Mã hóa dữ liệu hiển thị để hạn chế chèn HTML hoặc script."),
        ("NFR-LF-03", "Ghi người thực hiện và thời điểm khi đổi trạng thái."),
        ("NFR-LF-04", "Khi API hoặc Oracle lỗi, thông báo thất bại và không làm sai trạng thái."),
        ("NFR-LF-05", "Danh sách hỗ trợ phân trang."),
        ("NFR-LF-06", "Giữ nguyên nội dung đã duyệt để đối chiếu và kiểm tra."),
        ("NFR-LF-07", "Phát hiện xung đột phiên bản khi nhiều người cùng xử lý."),
    ])

    document.add_heading("9. Tiêu chí nghiệm thu", level=1)
    tests = [
        ("TC-LF-01", "Tạo đủ trường bắt buộc", "Lưu thành công ở trạng thái Nháp"),
        ("TC-LF-02", "Bỏ trống ngày, tiêu đề hoặc địa điểm", "Không cho lưu; cảnh báo"),
        ("TC-LF-03", "Nhập giờ không đúng HH:mm", "Không cho lưu"),
        ("TC-LF-04", "Từ giờ lớn hơn hoặc bằng Đến giờ", "Không cho lưu"),
        ("TC-LF-05", "Có thay đổi chưa lưu rồi Gửi duyệt", "Yêu cầu lưu nháp trước"),
        ("TC-LF-06", "Gửi điện văn Nháp", "Chuyển sang Chờ duyệt"),
        ("TC-LF-07", "Không có R_Add nhưng thêm mới", "Hệ thống từ chối"),
        ("TC-LF-08", "Không có R_Edit nhưng sửa/gửi duyệt", "Hệ thống từ chối"),
        ("TC-LF-09", "Có R_Pub duyệt điện văn Chờ duyệt", "Chuyển sang Đã duyệt"),
        ("TC-LF-10", "Không có R_Pub nhưng thực hiện duyệt", "Hệ thống từ chối"),
        ("TC-LF-11", "Từ chối không nhập lý do", "Không cho thực hiện"),
        ("TC-LF-12", "Từ chối với lý do hợp lệ", "Chuyển sang Từ chối và lưu lý do"),
        ("TC-LF-13", "Sửa điện văn bị từ chối và gửi lại", "Chuyển lại Chờ duyệt"),
        ("TC-LF-14", "Sửa điện văn đã duyệt", "Không cho phép"),
        ("TC-LF-15", "Hai người xử lý cùng phiên bản", "Từ chối thao tác trên phiên bản cũ"),
        ("TC-LF-16", "Từ ngày lớn hơn Đến ngày", "Không tìm kiếm; cảnh báo"),
        ("TC-LF-17", "API hoặc Oracle lỗi khi duyệt", "Không đổi trạng thái; báo lỗi"),
        ("TC-LF-18", "Xem lại điện văn đã duyệt", "Đúng nội dung, người và thời điểm duyệt"),
    ]
    add_table(document, ["Mã kiểm thử", "Nội dung", "Kết quả mong đợi"], tests)

    document.add_heading("10. Ma trận truy vết", level=1)
    add_table(document, ["Mã yêu cầu", "Nội dung", "Kiểm thử"], [
        ("FR-LF-01", "Tạo và lưu nháp", "TC-LF-01 đến TC-LF-04"),
        ("FR-LF-02", "Gửi duyệt", "TC-LF-05 đến TC-LF-08"),
        ("FR-LF-03", "Duyệt điện văn", "TC-LF-09, TC-LF-10, TC-LF-14"),
        ("FR-LF-04", "Từ chối điện văn", "TC-LF-11 đến TC-LF-13"),
        ("BR-LF-07", "Kiểm soát phiên bản", "TC-LF-15"),
        ("NFR-LF-04", "Xử lý lỗi API/Oracle", "TC-LF-17"),
        ("NFR-LF-06", "Lưu nội dung đã duyệt", "TC-LF-18"),
    ])

    for item in document.sections:
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
    document.save(OUTPUT)


if __name__ == "__main__":
    build_document()
    print(OUTPUT)
