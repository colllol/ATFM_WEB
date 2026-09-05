import os
from pathlib import Path

from docx import Document
from docx.enum.table import WD_CELL_VERTICAL_ALIGNMENT, WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Pt


ROOT = Path(__file__).resolve().parents[1]
OUTPUT = Path(os.environ.get(
    "SRS_LIVE_FIRE_OUTPUT",
    ROOT / "TaiLieu" / "SRS_Live_Fire_Message.docx",
))


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

    document.add_page_break()
    document.add_heading("PHẦN II. DAILY STATISTIC VÀ PHÊ DUYỆT DAILY STATISTIC", level=1)

    document.add_heading("11. Thông tin chung và phạm vi", level=1)
    add_table(document, ["Nội dung", "Giá trị"], [
        ("Tính năng nguồn", "Export Flight Finished"),
        ("Màn hình nguồn", "/Day_Flights/DaylyFlight.aspx?Menu_ID=89"),
        ("Tính năng thống kê", "Daily Statistic"),
        ("Màn hình thống kê", "/FinishFlights/ListFinishedFlights.aspx?Menu_ID=71"),
        ("Tính năng tra cứu đã duyệt", "Daily Statistic Accept"),
        ("Màn hình đã duyệt", "/FinishFlights/ListFinishedFlightAccepts.aspx?Menu_ID=905"),
        ("Gói dữ liệu chính", "FINISHED_STATUS_PKG; CANCELED_STATUS_PKG; MAKE_FINISHED"),
    ])
    document.add_paragraph(
        "Quy trình tiếp nhận các chuyến bay đã hoàn thành từ danh sách chuyến bay ngày, "
        "cho phép kiểm tra và hiệu chỉnh dữ liệu thống kê, xác nhận theo lô, sau đó tra cứu "
        "riêng dữ liệu đã được xác nhận. Phạm vi bao gồm chuyến bay hoàn thành và "
        "danh sách chuyến bay hủy có cùng cơ chế Accepted."
    )

    document.add_heading("12. Vai trò và luồng xử lý", level=1)
    add_table(document, ["Vai trò", "Trách nhiệm"], [
        ("Người khai thác chuyến bay ngày", "Kiểm tra danh sách và thực hiện Export Flight Finished."),
        ("Người lập thống kê", "Tra cứu dữ liệu chưa Accepted, bổ sung, sửa, xóa, xuất Excel và gửi Accepted."),
        ("Người kiểm tra/khai thác", "Tra cứu và xuất dữ liệu đã Accepted tại màn hình Daily Statistic Accept."),
    ])
    document.add_paragraph(
        "Luồng chính: DaylyFlight → Export Flight Finished → Daily Statistic (chưa Accepted) "
        "→ kiểm tra/hiệu chỉnh → Accepted → Daily Statistic Accept (đã Accepted)."
    )
    add_table(document, ["Trạng thái", "Giá trị", "Màn hình hiển thị"], [
        ("Chưa Accepted", "ISACCEPTED = 0", "Daily Statistic - Menu_ID=71"),
        ("Đã Accepted", "ISACCEPTED = 1", "Daily Statistic Accept - Menu_ID=905"),
    ])

    document.add_heading("13. Chức năng Export Flight Finished", level=1)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-DS-01", "Người dùng chọn Export Flight Finished từ màn hình DaylyFlight; hệ thống yêu cầu xác nhận trước khi xử lý."),
        ("FR-DS-02", "Hệ thống thực hiện bước MAKE_FINISHED để chuyển các chuyến đủ điều kiện sang kho dữ liệu chuyến bay hoàn thành."),
        ("FR-DS-03", "Sau khi chuyển thành công, hệ thống tải lại dữ liệu liên quan và phản ánh kết quả trên Daily Statistic."),
        ("FR-DS-04", "Nếu có lỗi ở bước tạo dữ liệu hoàn thành hoặc lấy dữ liệu export, hệ thống dừng quy trình và thông báo lỗi."),
    ])
    document.add_paragraph(
        "Dữ liệu được chuyển phải giữ được các thông tin nhận dạng và khai thác chính "
        "như số phép, hãng khai thác, callsign, đăng ký, loại tàu bay, mục đích, "
        "sân bay đi/đến, ngày bay, ETD/ETA, ATD/ATA, đường bay và ghi chú."
    )

    document.add_heading("14. Chức năng Daily Statistic", level=1)
    document.add_heading("14.1. Tra cứu", level=2)
    document.add_paragraph(
        "Màn hình mặc định truy vấn dữ liệu chưa Accepted (P_ISACCEPTED = 0). "
        "Bộ lọc gồm: số phép, đăng ký, sân bay đi/đến, mục đích, hãng khai thác, "
        "loại phép, loại tàu bay, callsign, VIA/FPL VIA, remark, ETD/ETA, ATD/ATA, "
        "khoảng ngày, khung giờ, FIR và các nhóm loại chuyến bay. Danh sách hỗ trợ phân trang."
    )
    document.add_heading("14.2. Quản lý dữ liệu", level=2)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-DS-05", "Hiển thị tổng số bản ghi và danh sách chuyến bay hoàn thành theo bộ lọc."),
        ("FR-DS-06", "Cho phép thêm, sửa, lưu hoặc xóa dữ liệu theo quyền được cấp; các dòng đang sửa phải được lưu trước khi Accepted."),
        ("FR-DS-07", "Cho phép xuất Excel theo bộ lọc và các cột người dùng lựa chọn; dữ liệu export không bị giới hạn bởi trang hiện tại."),
        ("FR-DS-08", "Hỗ trợ các danh sách nghiệp vụ: Hoàn thành, Cancel, bay quá nội, quốc tế về và chốt số liệu theo cấu hình màn hình."),
    ])
    document.add_heading("14.3. Accepted dữ liệu", level=2)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-DS-09", "Nút Accepted chỉ áp dụng cho danh sách Hoàn thành hoặc Cancel tương ứng."),
        ("FR-DS-10", "Hệ thống lấy các ID hợp lệ đang hiển thị trong kết quả, loại trùng và yêu cầu người dùng xác nhận số lượng."),
        ("FR-DS-11", "Danh sách ID được chia thành nhiều lô để không vượt giới hạn dữ liệu yêu cầu; mỗi lô gửi kèm tài khoản thực hiện."),
        ("FR-DS-12", "FINISHED_STATUS_PKG.ACCEPT_FINISHED_FLIGHTS xử lý chuyến hoàn thành; CANCELED_STATUS_PKG.ACCEPT_CANCELED_FLIGHTS xử lý chuyến hủy."),
        ("FR-DS-13", "Sau khi thành công, ISACCEPTED chuyển từ 0 sang 1, danh sách chưa Accepted được tải lại và thông báo tổng số bản ghi đã xử lý."),
        ("FR-DS-14", "Nếu một lô lỗi, dừng các lô còn lại và thông báo số bản ghi đã cập nhật trước khi lỗi."),
    ])

    document.add_heading("15. Chức năng Daily Statistic Accept", level=1)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-DS-15", "Màn hình chỉ truy vấn dữ liệu đã Accepted (P_ISACCEPTED = 1) cho danh sách Hoàn thành và Cancel."),
        ("FR-DS-16", "Cho phép tra cứu theo cùng nhóm tiêu chí nghiệp vụ của Daily Statistic và hiển thị tổng số bản ghi."),
        ("FR-DS-17", "Cho phép phân trang và xuất Excel toàn bộ kết quả đã Accepted theo bộ lọc."),
        ("FR-DS-18", "Màn hình đã Accepted không hiển thị thao tác Accepted lần nữa; dữ liệu phải tách biệt với danh sách chưa Accepted."),
    ])

    document.add_heading("16. Quy tắc nghiệp vụ và phi chức năng", level=1)
    add_table(document, ["Mã", "Quy tắc/yêu cầu"], [
        ("BR-DS-01", "Chỉ các chuyến đủ điều kiện kết thúc mới được chuyển sang dữ liệu Finished."),
        ("BR-DS-02", "Bản ghi ISACCEPTED = 0 thuộc Daily Statistic; ISACCEPTED = 1 thuộc Daily Statistic Accept."),
        ("BR-DS-03", "Không Accepted khi còn dòng đã sửa nhưng chưa lưu."),
        ("BR-DS-04", "Accepted chỉ xử lý các ID hợp lệ đang hiển thị trong kết quả người dùng đã xác nhận."),
        ("BR-DS-05", "Phải ghi tài khoản thực hiện Accepted và nhật ký hành động tại cơ sở dữ liệu."),
        ("BR-DS-06", "Không tự động Accepted bản ghi mới khi danh sách thay đổi sau lúc hiển thị."),
        ("NFR-DS-01", "Kiểm tra đăng nhập, quyền menu và quyền thao tác ở cả giao diện và máy chủ."),
        ("NFR-DS-02", "Phân trang danh sách; export dùng truy vấn riêng để lấy đủ kết quả."),
        ("NFR-DS-03", "Khi API/Oracle lỗi, không báo thành công sai và phải cho phép người dùng thử lại."),
        ("NFR-DS-04", "Thao tác lô phải thông báo chính xác tổng số bản ghi thành công trước khi gặp lỗi."),
    ])

    document.add_heading("17. Tiêu chí nghiệm thu và truy vết", level=1)
    daily_tests = [
        ("TC-DS-01", "Export Flight Finished sau khi xác nhận", "Dữ liệu đủ điều kiện xuất hiện tại Daily Statistic"),
        ("TC-DS-02", "Hủy xác nhận Export Flight Finished", "Không chuyển dữ liệu"),
        ("TC-DS-03", "MAKE_FINISHED hoặc API lỗi", "Dừng xử lý và hiển thị lỗi"),
        ("TC-DS-04", "Tra cứu Daily Statistic", "Chỉ trả bản ghi ISACCEPTED = 0 phù hợp bộ lọc"),
        ("TC-DS-05", "Thêm/sửa/lưu dữ liệu hợp lệ", "Dữ liệu cập nhật và danh sách hiển thị đúng"),
        ("TC-DS-06", "Accepted khi còn dòng chưa lưu", "Hệ thống từ chối và yêu cầu lưu trước"),
        ("TC-DS-07", "Accepted danh sách rỗng", "Hệ thống không gửi yêu cầu"),
        ("TC-DS-08", "Accepted nhiều chuyến hoàn thành", "Chia lô, cập nhật ISACCEPTED = 1 và báo đúng số lượng"),
        ("TC-DS-09", "Accepted nhiều chuyến hủy", "Gọi đúng CANCELED_STATUS_PKG và cập nhật thành công"),
        ("TC-DS-10", "Một lô Accepted bị lỗi", "Dừng lô sau và báo số bản ghi đã thành công"),
        ("TC-DS-11", "Tra cứu Daily Statistic Accept", "Chỉ trả bản ghi ISACCEPTED = 1"),
        ("TC-DS-12", "Bản ghi Accepted thành công", "Biến mất khỏi danh sách chưa duyệt và xuất hiện tại danh sách đã duyệt"),
        ("TC-DS-13", "Export Excel theo bộ lọc", "File có đủ bản ghi và đúng các cột đã chọn"),
        ("TC-DS-14", "Người không có quyền thao tác", "Giao diện/máy chủ từ chối yêu cầu"),
    ]
    add_table(document, ["Mã kiểm thử", "Nội dung", "Kết quả mong đợi"], daily_tests)
    add_table(document, ["Mã yêu cầu", "Nội dung", "Kiểm thử"], [
        ("FR-DS-01…FR-DS-04", "Export Flight Finished", "TC-DS-01…TC-DS-03"),
        ("FR-DS-05…FR-DS-08", "Tra cứu, hiệu chỉnh và export Daily Statistic", "TC-DS-04, TC-DS-05, TC-DS-13"),
        ("FR-DS-09…FR-DS-14", "Accepted dữ liệu", "TC-DS-06…TC-DS-10, TC-DS-12"),
        ("FR-DS-15…FR-DS-18", "Daily Statistic Accept", "TC-DS-11…TC-DS-13"),
        ("NFR-DS-01", "Phân quyền", "TC-DS-14"),
    ])

    document.add_page_break()
    document.add_heading("PHẦN III. QUẢN LÝ THÔNG TIN KHB QUÂN SỰ", level=1)

    document.add_heading("18. Thông tin chung và phạm vi", level=1)
    add_table(document, ["Nội dung", "Giá trị"], [
        ("Tên chức năng", "Quản lý thông tin KHB quân sự"),
        ("Màn hình quản lý", "/FinishFlights/ListFinishedFlightsMilitary.aspx?Menu_ID=843"),
        ("Màn hình khai thác báo cáo", "/FinishFlights/ListFinishedFlightsMilitaryReport.aspx?Menu_ID=863"),
        ("Bảng dữ liệu nghiệp vụ", "T_FINISHFLIGHTS_MILITARY"),
        ("Gói xử lý", "A_TEST_SEARCH"),
        ("Loại điện văn", "QS MESSAGE"),
        ("Kho điện văn chờ phát", "T_PLAN_MESSAGE"),
    ])
    document.add_paragraph(
        "Chức năng quản lý vòng đời thông tin kế hoạch bay (KHB) quân sự, "
        "từ khi nhập mới, kiểm tra và Accepted, tạo điện văn QS MESSAGE cho đến khi "
        "điện văn được phát đi. Sau khi Accepted, dữ liệu được cung cấp trên màn hình "
        "Military Report để người khai thác chỉ xem, tra cứu và lấy dữ liệu."
    )

    document.add_heading("19. Vai trò và luồng xử lý", level=1)
    add_table(document, ["Vai trò", "Trách nhiệm"], [
        ("Người nhập KHB quân sự", "Tạo mới, kiểm tra, sửa, lưu hoặc xóa KHB chưa Accepted."),
        ("Người duyệt", "Kiểm tra danh sách theo khoảng ngày và thực hiện Accepted."),
        ("Người tạo điện văn", "Chọn dữ liệu đã Accepted và Export Message thành QS MESSAGE."),
        ("Người phát điện văn", "Kiểm tra điện văn đã tạo và thực hiện phát đi theo phân hệ điện văn."),
        ("Người khai thác báo cáo", "Chỉ xem, tra cứu và xuất/lấy dữ liệu KHB quân sự đã Accepted."),
    ])
    document.add_paragraph(
        "Luồng chính: Nhập mới KHB → Chưa Accepted → Accepted → Export Message "
        "→ Tạo QS MESSAGE trong T_PLAN_MESSAGE → Phát đi. Dữ liệu sau Accepted đồng thời "
        "xuất hiện trên Military Report để khai thác theo chế độ chỉ xem."
    )
    add_table(document, ["Giai đoạn", "Trạng thái/điều kiện", "Kết quả"], [
        ("Nhập mới", "ISACCEPTED = 0", "KHB được phép hiệu chỉnh trước duyệt"),
        ("Accepted", "ISACCEPTED = 1", "KHB được chốt và hiển thị trên Military Report"),
        ("Export Message", "Chỉ lấy KHB ISACCEPTED = 1", "Tạo QS MESSAGE trong T_PLAN_MESSAGE"),
        ("Phát đi", "QS MESSAGE hợp lệ và chờ phát", "Điện văn được chuyển qua quy trình phát"),
    ])

    document.add_heading("20. Chức năng nhập và quản lý KHB quân sự", level=1)
    document.add_heading("20.1. Dữ liệu KHB", level=2)
    add_table(document, ["Nhóm", "Trường dữ liệu chính"], [
        ("Nhận dạng chuyến bay", "Hãng khai thác (OPER), ngày bay, callsign, số đăng ký"),
        ("Tàu bay và mục đích", "Loại tàu bay thực tế, loại tàu bay kế hoạch, mục đích, loại phép"),
        ("Hành trình", "Sân bay đi, sân bay đến, VIA, FPL VIA"),
        ("Thời gian", "ETD, ETA, ATD, ATA"),
        ("Thông tin bổ sung", "Remark và người tạo/cập nhật"),
    ])
    document.add_heading("20.2. Yêu cầu chức năng", level=2)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-MIL-01", "Cho phép tạo một hoặc nhiều dòng KHB quân sự mới và lưu vào dữ liệu nghiệp vụ."),
        ("FR-MIL-02", "Kiểm tra tối thiểu các trường nhận dạng, ngày bay, callsign, sân bay và thời gian theo quy tắc của biểu mẫu."),
        ("FR-MIL-03", "Cho phép tra cứu theo khoảng ngày, khung giờ, Accepted, callsign, đăng ký, sân bay, mục đích, tàu bay, VIA, remark và thời gian."),
        ("FR-MIL-04", "Cho phép sửa, lưu hàng loạt hoặc xóa KHB theo quyền và trạng thái nghiệp vụ."),
        ("FR-MIL-05", "Hiển thị tổng số bản ghi, phân trang và cho phép Export Excel theo bộ lọc."),
        ("FR-MIL-06", "KHB mới phải có ISACCEPTED = 0 và không được đưa vào QS MESSAGE trước khi Accepted."),
    ])

    document.add_heading("21. Chức năng Accepted KHB quân sự", level=1)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-MIL-07", "Người dùng phải chọn đủ Từ ngày và Đến ngày; Từ ngày không được lớn hơn Đến ngày."),
        ("FR-MIL-08", "Trước khi Accepted, hệ thống hiển thị xác nhận rõ khoảng ngày sẽ xử lý."),
        ("FR-MIL-09", "A_TEST_SEARCH.ACCEPT_FIN_FLIGHTS_MILITARY Accepted toàn bộ KHB quân sự hợp lệ chưa Accepted trong khoảng ngày và ghi nhận tài khoản thực hiện."),
        ("FR-MIL-10", "Sau khi thành công, hệ thống thông báo số chuyến đã Accepted, chuyển bộ lọc về Chưa Accepted và tải lại danh sách."),
        ("FR-MIL-11", "KHB đã Accepted phải hiển thị khi lọc Đã Accepted và trên Military Report."),
        ("FR-MIL-12", "Nếu API hoặc Oracle lỗi, không hiển thị thông báo thành công; phải mở lại nút thao tác để người dùng thử lại."),
    ])

    document.add_heading("22. Chức năng Export Message và phát đi", level=1)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-MIL-13", "Nút Export Message chỉ được bật khi người dùng đang lọc trạng thái Đã Accepted."),
        ("FR-MIL-14", "Người dùng phải chọn khoảng ngày hợp lệ và xác nhận trước khi tạo điện văn."),
        ("FR-MIL-15", "A_TEST_SEARCH.EXPORT_QS_PLAN_MESSAGE chỉ lấy KHB quân sự ISACCEPTED = 1 trong khoảng ngày đã chọn."),
        ("FR-MIL-16", "Hệ thống tạo nội dung điện văn và ghi vào T_PLAN_MESSAGE với MESS_TYPE = 'QS MESSAGE', kèm danh sách chuyến bay nguồn."),
        ("FR-MIL-17", "Sau khi export, thông báo chính xác số chuyến bay đã được tạo thành QS MESSAGE."),
        ("FR-MIL-18", "QS MESSAGE được chuyển sang phân hệ quản lý/phát điện văn; chỉ người có quyền mới được phát đi."),
        ("FR-MIL-19", "Khi phát thành công, hệ thống phải ghi nhận trạng thái, người thực hiện và thời điểm phát để truy vết."),
        ("FR-MIL-20", "Không được phát điện văn khi nội dung rỗng, dữ liệu nguồn không hợp lệ hoặc người dùng không có quyền."),
    ])

    document.add_heading("23. Chức năng Military Report", level=1)
    add_table(document, ["Mã", "Yêu cầu"], [
        ("FR-MIL-21", "Màn hình ListFinishedFlightsMilitaryReport chỉ truy vấn KHB quân sự đã Accepted (P_ISACCEPTED = 1)."),
        ("FR-MIL-22", "Người khai thác được tra cứu theo khoảng ngày, khung giờ và các trường KHB chính; danh sách hỗ trợ phân trang."),
        ("FR-MIL-23", "Màn hình chỉ cho phép xem và lấy dữ liệu; không cho phép thêm, sửa, xóa, Accepted hoặc Export Message."),
        ("FR-MIL-24", "Cho phép Export Excel toàn bộ kết quả theo bộ lọc, không chỉ các dòng của trang hiện tại."),
        ("FR-MIL-25", "Dữ liệu hiển thị trên Report phải nhất quán với bản ghi Đã Accepted trên màn hình quản lý."),
    ])

    document.add_heading("24. Quy tắc nghiệp vụ và phi chức năng", level=1)
    add_table(document, ["Mã", "Quy tắc/yêu cầu"], [
        ("BR-MIL-01", "KHB mới luôn bắt đầu ở trạng thái Chưa Accepted."),
        ("BR-MIL-02", "Accepted áp dụng cho toàn bộ KHB hợp lệ trong khoảng ngày người dùng đã xác nhận."),
        ("BR-MIL-03", "Chỉ KHB Đã Accepted mới được tạo QS MESSAGE và hiển thị tại Military Report."),
        ("BR-MIL-04", "Military Report là nguồn chỉ đọc dành cho khai thác, không làm thay đổi dữ liệu gốc."),
        ("BR-MIL-05", "Không tạo trùng QS MESSAGE chưa phát cho cùng phạm vi dữ liệu, trừ khi nghiệp vụ cho phép tạo lại."),
        ("BR-MIL-06", "Việc phát điện văn không được làm mất liên kết với danh sách KHB nguồn."),
        ("NFR-MIL-01", "Kiểm tra đăng nhập và phân quyền cho Nhập, Sửa, Xóa, Accepted, Export Message và Phát đi."),
        ("NFR-MIL-02", "Ghi nhật ký ít nhất cho Accepted, Export Message và Phát đi, gồm người thực hiện, thời gian và kết quả."),
        ("NFR-MIL-03", "Khi API/Oracle lỗi, không thay đổi sai trạng thái và phải thông báo thao tác không thành công."),
        ("NFR-MIL-04", "Danh sách hỗ trợ phân trang; truy vấn export phải lấy đủ dữ liệu theo bộ lọc."),
        ("NFR-MIL-05", "Dữ liệu đã Accepted và điện văn đã phát phải truy vết được khi kiểm tra/kiểm định."),
    ])

    document.add_heading("25. Tiêu chí nghiệm thu và truy vết", level=1)
    military_tests = [
        ("TC-MIL-01", "Nhập mới KHB hợp lệ", "Lưu thành công với ISACCEPTED = 0"),
        ("TC-MIL-02", "Thiếu trường bắt buộc hoặc ngày/giờ sai", "Không lưu; hiển thị cảnh báo"),
        ("TC-MIL-03", "Sửa và lưu nhiều dòng chưa Accepted", "Cập nhật đúng các dòng hợp lệ"),
        ("TC-MIL-04", "Accepted khi thiếu Từ ngày/Đến ngày", "Không thực hiện; yêu cầu chọn đủ ngày"),
        ("TC-MIL-05", "Accepted với Từ ngày lớn hơn Đến ngày", "Không thực hiện; cảnh báo khoảng ngày"),
        ("TC-MIL-06", "Accepted khoảng ngày hợp lệ", "Chuyển KHB phù hợp sang ISACCEPTED = 1 và báo số lượng"),
        ("TC-MIL-07", "API Accepted bị lỗi", "Không báo thành công; cho phép thử lại"),
        ("TC-MIL-08", "Export Message khi đang lọc Chưa Accepted", "Nút bị khóa hoặc thao tác bị từ chối"),
        ("TC-MIL-09", "Export Message KHB đã Accepted", "Tạo QS MESSAGE trong T_PLAN_MESSAGE và báo đúng số chuyến"),
        ("TC-MIL-10", "Export Message gặp lỗi API/Oracle", "Không báo thành công và không tạo dữ liệu dở dang"),
        ("TC-MIL-11", "Phát QS MESSAGE hợp lệ", "Ghi nhận trạng thái, người và thời điểm phát"),
        ("TC-MIL-12", "Người không có quyền phát", "Hệ thống từ chối thao tác"),
        ("TC-MIL-13", "Tra cứu Military Report", "Chỉ hiển thị KHB ISACCEPTED = 1 phù hợp bộ lọc"),
        ("TC-MIL-14", "Thử thêm/sửa/xóa tại Military Report", "Không có thao tác hoặc máy chủ từ chối"),
        ("TC-MIL-15", "Export Excel từ Military Report", "File chứa đủ dữ liệu theo bộ lọc"),
        ("TC-MIL-16", "Đối chiếu màn hình quản lý và Report", "Dữ liệu Đã Accepted nhất quán"),
    ]
    add_table(document, ["Mã kiểm thử", "Nội dung", "Kết quả mong đợi"], military_tests)
    add_table(document, ["Mã yêu cầu", "Nội dung", "Kiểm thử"], [
        ("FR-MIL-01…FR-MIL-06", "Nhập và quản lý KHB", "TC-MIL-01…TC-MIL-03"),
        ("FR-MIL-07…FR-MIL-12", "Accepted KHB quân sự", "TC-MIL-04…TC-MIL-07"),
        ("FR-MIL-13…FR-MIL-20", "Export Message và Phát đi", "TC-MIL-08…TC-MIL-12"),
        ("FR-MIL-21…FR-MIL-25", "Military Report chỉ xem", "TC-MIL-13…TC-MIL-16"),
        ("NFR-MIL-01…NFR-MIL-05", "Bảo mật, nhật ký, hiệu năng và truy vết", "TC-MIL-07, TC-MIL-10…TC-MIL-16"),
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
