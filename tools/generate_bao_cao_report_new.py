from pathlib import Path

from docx import Document
from docx.enum.section import WD_SECTION
from docx.enum.table import WD_CELL_VERTICAL_ALIGNMENT, WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Inches, Pt, RGBColor


ROOT = Path(__file__).resolve().parents[1]
IMAGE_DIR = Path(r"C:\Users\Admin\Pictures\Saved Pictures")
OUTPUT = ROOT / "TaiLieu" / "BAO_CAO_HE_THONG_REPORT_NEW_NGHIEP_VU.docx"

BLUE = "1F4E78"
LIGHT_BLUE = "D9EAF7"
GRAY = "E7E6E6"


def set_cell_shading(cell, fill):
    tc_pr = cell._tc.get_or_add_tcPr()
    shd = tc_pr.find(qn("w:shd"))
    if shd is None:
        shd = OxmlElement("w:shd")
        tc_pr.append(shd)
    shd.set(qn("w:fill"), fill)


def set_cell_text(cell, text, bold=False, color=None, size=12):
    cell.text = ""
    paragraph = cell.paragraphs[0]
    paragraph.alignment = WD_ALIGN_PARAGRAPH.LEFT
    paragraph.paragraph_format.first_line_indent = Cm(0)
    paragraph.paragraph_format.line_spacing = 1.15
    run = paragraph.add_run(str(text))
    run.bold = bold
    run.font.name = "Times New Roman"
    run.font.size = Pt(size)
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    if color:
        run.font.color.rgb = RGBColor.from_string(color)
    cell.vertical_alignment = WD_CELL_VERTICAL_ALIGNMENT.CENTER


def add_table(document, headers, rows, widths=None):
    table = document.add_table(rows=1, cols=len(headers))
    table.alignment = WD_TABLE_ALIGNMENT.CENTER
    table.style = "Table Grid"
    for index, header in enumerate(headers):
        set_cell_text(table.rows[0].cells[index], header, bold=True, color="FFFFFF")
        set_cell_shading(table.rows[0].cells[index], BLUE)
        if widths:
            table.rows[0].cells[index].width = Cm(widths[index])
    for row_index, row in enumerate(rows):
        cells = table.add_row().cells
        for index, value in enumerate(row):
            set_cell_text(cells[index], value)
            if row_index % 2:
                set_cell_shading(cells[index], "F5F9FC")
            if widths:
                cells[index].width = Cm(widths[index])
    document.add_paragraph()
    return table


def add_body(document, text, bold_lead=None):
    paragraph = document.add_paragraph()
    paragraph.alignment = WD_ALIGN_PARAGRAPH.JUSTIFY
    paragraph.paragraph_format.first_line_indent = Cm(1.27)
    paragraph.paragraph_format.line_spacing = 1.5
    paragraph.paragraph_format.space_after = Pt(6)
    if bold_lead and text.startswith(bold_lead):
        lead = paragraph.add_run(bold_lead)
        lead.bold = True
        paragraph.add_run(text[len(bold_lead):])
    else:
        paragraph.add_run(text)
    return paragraph


def add_bullet(document, text):
    paragraph = document.add_paragraph(style="List Bullet")
    paragraph.alignment = WD_ALIGN_PARAGRAPH.JUSTIFY
    paragraph.paragraph_format.left_indent = Cm(1.27)
    paragraph.paragraph_format.first_line_indent = Cm(-0.63)
    paragraph.paragraph_format.line_spacing = 1.5
    paragraph.paragraph_format.space_after = Pt(3)
    paragraph.add_run(text)
    return paragraph


def add_picture_page(document, image_name, caption):
    document.add_page_break()
    paragraph = document.add_paragraph()
    paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
    paragraph.paragraph_format.first_line_indent = Cm(0)
    paragraph.add_run().add_picture(str(IMAGE_DIR / image_name), width=Cm(16))
    caption_paragraph = document.add_paragraph()
    caption_paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
    caption_paragraph.paragraph_format.first_line_indent = Cm(0)
    caption_paragraph.paragraph_format.space_after = Pt(6)
    run = caption_paragraph.add_run(caption)
    run.italic = True
    run.font.size = Pt(11)


def add_report_section(document, number, title, path, image_name, observed, functions, notes):
    document.add_heading(f"{number}. {title}", level=1)
    add_body(document, f"Đường dẫn: {path}", "Đường dẫn:")
    document.add_heading("Số liệu quan sát", level=2)
    for item in observed:
        add_bullet(document, item)
    document.add_heading("Chức năng và cách sử dụng", level=2)
    for item in functions:
        add_bullet(document, item)
    document.add_heading("Nhận xét và lưu ý", level=2)
    for item in notes:
        add_bullet(document, item)
    add_picture_page(document, image_name, f"Hình {number}. Ảnh toàn trang {title} (GoFullPage)")


def configure_styles(document):
    normal = document.styles["Normal"]
    normal.font.name = "Times New Roman"
    normal.font.size = Pt(12)
    normal._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    normal.paragraph_format.alignment = WD_ALIGN_PARAGRAPH.JUSTIFY
    normal.paragraph_format.first_line_indent = Cm(1.27)
    normal.paragraph_format.line_spacing = 1.5
    normal.paragraph_format.space_after = Pt(6)

    for style_name, size, color in [
        ("Title", 18, BLUE),
        ("Heading 1", 16, BLUE),
        ("Heading 2", 13, BLUE),
        ("Heading 3", 12, BLUE),
    ]:
        style = document.styles[style_name]
        style.font.name = "Times New Roman"
        style.font.size = Pt(size)
        style.font.bold = True
        style.font.color.rgb = RGBColor.from_string(color)
        style._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
        style.paragraph_format.first_line_indent = Cm(0)
        style.paragraph_format.space_before = Pt(8)
        style.paragraph_format.space_after = Pt(6)


def add_header_footer(section):
    header = section.header.paragraphs[0]
    header.alignment = WD_ALIGN_PARAGRAPH.CENTER
    header.paragraph_format.first_line_indent = Cm(0)
    run = header.add_run("BÁO CÁO HỆ THỐNG REPORT NEW - VATM FLIGHT ANALYTICS")
    run.font.name = "Times New Roman"
    run.font.size = Pt(10)
    run.font.bold = True
    run.font.color.rgb = RGBColor.from_string(BLUE)

    footer = section.footer.paragraphs[0]
    footer.alignment = WD_ALIGN_PARAGRAPH.CENTER
    footer.paragraph_format.first_line_indent = Cm(0)
    run = footer.add_run("Tài liệu phân tích giao diện và chức năng - Ngày 13/08/2026")
    run.font.name = "Times New Roman"
    run.font.size = Pt(10)


def add_cover(document):
    for _ in range(3):
        document.add_paragraph()
    title = document.add_paragraph()
    title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    title.paragraph_format.first_line_indent = Cm(0)
    run = title.add_run("BÁO CÁO CHI TIẾT NGHIỆP VỤ\nHỆ THỐNG REPORT NEW")
    run.bold = True
    run.font.name = "Times New Roman"
    run.font.size = Pt(22)
    run.font.color.rgb = RGBColor.from_string(BLUE)
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")

    subtitle = document.add_paragraph()
    subtitle.alignment = WD_ALIGN_PARAGRAPH.CENTER
    subtitle.paragraph_format.first_line_indent = Cm(0)
    subtitle.paragraph_format.space_before = Pt(18)
    run = subtitle.add_run("VATM Flight Analytics\nPhân tích 09 trang báo cáo và ảnh toàn trang GoFullPage")
    run.font.name = "Times New Roman"
    run.font.size = Pt(16)
    run.bold = True

    for _ in range(7):
        document.add_paragraph()
    info = document.add_paragraph()
    info.alignment = WD_ALIGN_PARAGRAPH.CENTER
    info.paragraph_format.first_line_indent = Cm(0)
    run = info.add_run("Ngày lập báo cáo: 13/08/2026\nNguồn: giao diện hệ thống tại localhost/atfm và mã nguồn dự án ATFM_WEB")
    run.font.name = "Times New Roman"
    run.font.size = Pt(12)
    document.add_page_break()


def build_document():
    for index in range(1, 10):
        image = IMAGE_DIR / f"{index}.png"
        if not image.exists():
            raise FileNotFoundError(f"Thiếu ảnh nguồn: {image}")

    document = Document()
    configure_styles(document)
    section = document.sections[0]
    section.page_width = Cm(21)
    section.page_height = Cm(29.7)
    section.top_margin = Cm(2)
    section.bottom_margin = Cm(2)
    section.left_margin = Cm(3)
    section.right_margin = Cm(2)
    add_header_footer(section)
    add_cover(document)

    document.add_heading("1. Mục đích và phạm vi", level=1)
    add_body(document, "Báo cáo mô tả giao diện, số liệu quan sát, bộ lọc, chỉ số, biểu đồ, bảng chi tiết, nguồn dữ liệu và các điểm cần lưu ý của chín trang thuộc phân hệ Report New. Ảnh được chụp toàn trang bằng GoFullPage trên Chrome vào ngày 13/08/2026, độ rộng 1.366 pixel.")
    add_body(document, "Số liệu trong tài liệu phản ánh đúng trạng thái tại thời điểm chụp. Giá trị bằng 0 chỉ thể hiện không có dữ liệu phù hợp với bộ lọc đang chọn; không được xem là bằng chứng duy nhất để kết luận dịch vụ hoặc cơ sở dữ liệu bị lỗi.")

    document.add_heading("2. Tổng hợp nhanh", level=1)
    add_table(
        document,
        ["STT", "Trang", "Kỳ quan sát", "Kết quả nổi bật"],
        [
            ["1", "Dashboard", "13/08/2026", "0 chuyến; màn hình điều hành tổng hợp"],
            ["2", "Tỷ lệ trạng thái", "13/08/2026", "0 chuyến; đủ 4 nhóm trạng thái"],
            ["3", "Tổng quan khai thác", "01-13/08/2026", "1 chuyến hoàn thành tại VVNB"],
            ["4", "Phân tích xu hướng", "13/08/2026", "Hiện tại 0; cùng kỳ 2025 là 1.448"],
            ["5", "Cảnh báo bất thường", "13/08/2026", "0 cảnh báo delay từ 15 phút"],
            ["6", "Chuyến bay quân sự", "13/08/2026", "0 bản ghi phù hợp"],
            ["7", "Tổng hợp dân dụng", "14/07-13/08/2026", "2 chuyến; OTP 100%"],
            ["8", "Cất/hạ cánh toàn quốc", "13/08/2026", "0 lượt"],
            ["9", "So sánh sân bay", "13/08/2026", "VVBM: 0/0 ở hai mốc so sánh"],
        ],
        [1.2, 5.1, 4.0, 6.0],
    )

    document.add_heading("3. Đánh giá tổng thể", level=1)
    add_body(document, "Phân hệ có cấu trúc giao diện thống nhất: nhận diện VATM, vùng tiêu đề màu xanh, bộ lọc tập trung, thẻ KPI, khu vực trực quan hóa và bảng dữ liệu. Các màu xanh lá, đỏ, cam và xanh dương được dùng nhất quán cho Finished, Cancel, Delay và Wait. Cách tổ chức này phù hợp với màn hình nghiệp vụ cần quét nhanh.")
    add_body(document, "Phần lớn trang hỗ trợ tương tác trực tiếp trên biểu đồ hoặc thẻ trạng thái để lọc dữ liệu. Hai báo cáo FlightStatusRate và FlightOperationOverview có xuất Excel; MilitaryFlightReport cũng cung cấp xuất Excel. Dashboard đóng vai trò điều hướng sang ba báo cáo chi tiết.")
    add_body(document, "Dữ liệu trong ảnh cho thấy báo cáo quá khứ hoạt động: trang tổng quan khai thác trả một chuyến và trang dân dụng trả hai chuyến. Trong khi đó, các trang chọn riêng ngày 13/08/2026 chủ yếu trả 0. Cần đánh giá theo từng nguồn dữ liệu và điều kiện ngày, không nên suy rộng thành lỗi chung của toàn phân hệ.")

    add_report_section(
        document, "4.1", "Thống kê trạng thái chuyến bay", "ReportNew/FlightStatusRate.aspx", "2.png",
        [
            "Bộ lọc đang chọn: tất cả sân bay, tất cả hãng bay, từ 13/08/2026 đến 13/08/2026.",
            "Tổng chuyến, hoàn thành, hủy và delay đều bằng 0; donut và bốn nhóm trạng thái đều hiển thị 0%.",
            "Danh sách chuyến bay có 0 dòng; giao diện thông báo không có chuyến bay phù hợp.",
        ],
        [
            "Cho phép lọc theo sân bay, hãng bay và khoảng ngày; hỗ trợ xuất Excel.",
            "Phân loại bốn trạng thái Finished, Cancel, Delay và Wait. Người dùng có thể chọn vùng trạng thái để lọc bảng chi tiết.",
            "Bảng chi tiết gồm callsign, hãng, đăng ký, loại phép, sân bay đi/đến, thời gian thực tế/kế hoạch và trạng thái; có phân trang phía trình duyệt.",
            "Ngày hiện tại lấy dữ liệu từ T_DAY_FLIGHTS_GOINGON; kỳ lịch sử lấy T_FINISHED_FLIGHTS và ghép chuyến hủy.",
        ],
        [
            "Kết quả 0 phù hợp với bộ lọc trong ảnh, nhưng cần thử thêm ngày có dữ liệu để nghiệm thu biểu đồ, lọc chéo và Excel.",
            "Theo mã nguồn, nhánh bổ sung chuyến hủy lịch sử chưa áp dụng đầy đủ bộ lọc sân bay/hãng; cần kiểm thử để tránh sai lệch số liệu khi chọn bộ lọc cụ thể.",
        ],
    )

    add_report_section(
        document, "4.2", "Thông tin tổng quan khai thác bay", "ReportNew/FlightOperationOverview.aspx", "3.png",
        [
            "Bộ lọc: tất cả sân bay, từ 01/08/2026 đến 13/08/2026; nguồn hiển thị T_FINISHED_FLIGHTS.",
            "Có 1 chuyến bay, 1 chuyến hoàn thành, 0 chuyến delay và 1 sân bay khai thác.",
            "Biểu đồ sân bay ghi nhận VVNB có 1 lượt cất cánh và 0 lượt hạ cánh.",
            "Bảng hiển thị AAR730, hãng AAR, loại phép LD, hành trình VVNB-RKSI, ATD 030625 và ATA 031035.",
        ],
        [
            "Tổng hợp riêng các chuyến Finished và Delay; lọc theo sân bay và khoảng ngày; hỗ trợ xuất Excel.",
            "Donut trình bày cơ cấu trạng thái và biểu đồ cột ghép trình bày cất/hạ cánh theo sân bay.",
            "Có thể chọn trạng thái hoặc cột sân bay để lọc chéo danh sách chuyến bay.",
        ],
        [
            "Ảnh xác nhận luồng dữ liệu lịch sử trả kết quả và mối liên hệ giữa KPI, donut, biểu đồ sân bay và bảng là nhất quán.",
            "Danh sách đầy đủ được trả về trình duyệt trước khi phân trang; khoảng ngày lớn có thể làm tăng thời gian tải và dung lượng phản hồi.",
        ],
    )

    add_report_section(
        document, "4.3", "Phân tích xu hướng theo thời gian", "ReportNew/FlightTrendAnalysis.aspx", "4.png",
        [
            "Bộ lọc: tất cả sân bay, tất cả hãng bay, ngày 13/08/2026, chế độ xem theo ngày.",
            "Kỳ hiện tại có 0 chuyến; cùng kỳ 13/08/2025 có 1.448 chuyến; chênh lệch -1.448 chuyến và tỷ lệ thay đổi -100%.",
            "Biểu đồ thể hiện điểm kỳ hiện tại ở 0 và điểm cùng kỳ năm trước ở khoảng 1.448.",
        ],
        [
            "So sánh tổng Finished + Delay của kỳ được chọn với đúng cùng kỳ năm trước.",
            "Hỗ trợ lọc sân bay, hãng bay, khoảng ngày và chuyển chế độ theo ngày hoặc theo tháng.",
            "Biểu đồ đường trên canvas có tooltip khi di chuột; KPI cho biết tổng hai kỳ, chênh lệch tuyệt đối và phần trăm thay đổi.",
        ],
        [
            "Mức giảm 100% là kết quả toán học từ dữ liệu quan sát, chưa đủ để kết luận hoạt động thực tế giảm 100%; cần xác minh dữ liệu ngày hiện tại đã được nạp đầy đủ hay chưa.",
            "Mã nguồn giới hạn chế độ ngày tối đa 367 ngày và không cho ngày kết thúc vượt ngày hiện tại.",
        ],
    )

    add_report_section(
        document, "4.4", "Phát hiện và cảnh báo dữ liệu bất thường", "ReportNew/AnomalyWarning.aspx", "5.png",
        [
            "Nguồn dữ liệu hiện tại là ngày 13/08/2026.",
            "Tổng cảnh báo, mức 1, mức 2 và mức 3 đều bằng 0; giao diện ghi nhận không có chuyến cần cảnh báo.",
            "Ba ngưỡng lần lượt là 15-29 phút, 30-59 phút và từ 60 phút.",
        ],
        [
            "Theo dõi chuyến cất cánh chậm trong ngày dựa trên chênh lệch ATD và ETD.",
            "Nút Xem cảnh báo delay mở danh sách chi tiết; người dùng có thể lọc theo cấp độ cảnh báo.",
            "Dữ liệu lấy từ T_DAY_FLIGHTS_GOINGON và chỉ xử lý ETD 4 chữ số, ATD 6 chữ số.",
        ],
        [
            "Tên trang mô tả phạm vi bất thường rộng, nhưng chức năng hiện tại tập trung vào delay cất cánh.",
            "Bản ghi có định dạng giờ không hợp lệ bị bỏ qua; nên có thống kê dữ liệu bị loại để người vận hành phân biệt không có cảnh báo với dữ liệu đầu vào không đạt chuẩn.",
        ],
    )

    add_report_section(
        document, "4.5", "Báo cáo chuyến bay quân sự", "ReportNew/MilitaryFlightReport.aspx", "6.png",
        [
            "Bộ lọc: tất cả sân bay, tất cả mục đích, từ 13/08/2026 đến 13/08/2026.",
            "Danh sách chi tiết có 0 bản ghi và thông báo không có chuyến bay phù hợp.",
            "Giao diện ghi nguồn API T_FINISHFLIGHTS_MILITARY.",
        ],
        [
            "Tra cứu theo sân bay, mục đích và khoảng ngày; hỗ trợ xuất Excel.",
            "Bảng gồm loại phép P_TYPE, sân bay đi/đến, ETD, ETA, ATD, ATA, mục đích và ngày bay; có phân trang phía trình duyệt.",
            "Frontend gọi API MilitaryFlightReport/GetData; phần controller và truy vấn nguồn không nằm trong repository web hiện tại.",
        ],
        [
            "Cần kiểm thử với kỳ có dữ liệu quân sự để xác nhận bộ lọc mục đích, định dạng thời gian và nội dung Excel.",
            "Tên nguồn trên giao diện là T_FINISHFLIGHTS_MILITARY; cần đối chiếu hệ thống dữ liệu để xác nhận đây là tên chính thức, không phải T_FINISHED_FLIGHTS_MILITARY.",
        ],
    )

    add_report_section(
        document, "4.6", "Tổng hợp chuyến bay dân dụng", "ReportNew/CivilFlightSummary.aspx", "7.png",
        [
            "Bộ lọc: tất cả sân bay, từ 14/07/2026 đến 13/08/2026; nguồn hiển thị T_FINISHED_FLIGHTS.",
            "Có 2 chuyến dân dụng: 1 nội địa, 1 quốc tế; hệ số đúng giờ OTP là 100%.",
            "VVNB có 1 lượt đi, 1 lượt đến, tổng lưu lượng 2; VVTS có 1 lượt đi, 0 lượt đến, tổng 1.",
            "Hai chuyến là HVN258 ngày 28/07/2026, VVTS-VVNB, trạng thái FINISHED; và AAR730 ngày 03/08/2026, VVNB-RKSI, trạng thái FINISHED.",
        ],
        [
            "Tổng hợp KPI tổng dân dụng, nội địa, quốc tế và OTP theo sân bay/khoảng ngày.",
            "Bảng lưu lượng sân bay hiển thị tối đa các đầu mối có lưu lượng cao; bảng chuyến bay trình bày ngày, chuyến, hãng, hành trình và trạng thái.",
            "Frontend gọi API CivilFlightSummary/GetData; quy tắc phân loại nội địa/quốc tế và OTP nằm ở backend ngoài repository này.",
        ],
        [
            "Tổng lưu lượng qua các sân bay có thể lớn hơn số chuyến vì một chuyến được tính tại cả đầu đi và đầu đến; ảnh đang thể hiện đúng nguyên tắc này.",
            "Bảng chuyến bay chưa có phân trang hoặc xuất Excel trên giao diện; cần cân nhắc khi truy vấn kỳ dài.",
        ],
    )

    add_report_section(
        document, "4.7", "Báo cáo cất, hạ cánh toàn quốc", "ReportNew/AirportTakeoffLanding.aspx", "8.png",
        [
            "Bộ lọc: tất cả sân bay, từ 13/08/2026 đến 13/08/2026.",
            "Tổng cất cánh, tổng hạ cánh và số sân bay khai thác đều bằng 0; không xác định sân bay có lưu lượng cao nhất.",
            "Biểu đồ thông báo không có chuyến Finished hoặc Delay; vùng chi tiết yêu cầu chọn một cột cất/hạ cánh.",
        ],
        [
            "Thống kê Finished + Delay tại các sân bay Việt Nam, nhận diện theo mã bắt đầu bằng VV.",
            "Biểu đồ cột ghép trình bày lượt cất cánh và hạ cánh. Khi chọn cột, hệ thống tải danh sách chuyến bay tương ứng.",
            "Danh sách chi tiết dùng phân trang server, cỡ trang từ 25 đến 500 dòng.",
        ],
        [
            "Theo mã nguồn, điều kiện truy vấn dùng thời gian nhỏ hơn ngày kết thúc nhưng WebMethod truyền trực tiếp ngày kết thúc, có nguy cơ loại toàn bộ ngày Đến ngày. Việc chọn cùng một ngày và nhận 0 trong ảnh phù hợp với rủi ro này, dù vẫn cần truy vấn đối chứng để kết luận nguyên nhân.",
            "Nên sửa hoặc kiểm thử quy ước khoảng ngày theo dạng [Từ ngày, Đến ngày + 1) để bảo đảm bao gồm trọn ngày kết thúc.",
        ],
    )

    add_report_section(
        document, "4.8", "So sánh hoạt động sân bay", "Common/ChartReportAirport.aspx", "9.png",
        [
            "Chế độ 1 sân bay, sân bay VVBM, hai mốc cùng là 13/08/2026.",
            "Cả hai chuỗi so sánh đều có tổng chuyến, Finished, Cancel, Delay và Wait bằng 0.",
            "Biểu đồ vẫn hiển thị đầy đủ năm nhóm chỉ số và chú thích hai chuỗi theo ngày.",
        ],
        [
            "Cho phép chọn từ 1 đến 5 sân bay. Với một sân bay, hệ thống so sánh hai ngày; với nhiều sân bay, hệ thống cộng dữ liệu trong khoảng giữa hai mốc.",
            "Biểu đồ cột ghép HTML/CSS trình bày tổng chuyến và bốn trạng thái; không có bảng chi tiết hoặc xuất Excel.",
            "Frontend nhận danh sách chuyến thô rồi tự tổng hợp theo sân bay và trạng thái.",
        ],
        [
            "Khi hai ngày giống nhau, hai chuỗi có cùng nhãn và màu khác nhau nhưng không tạo thêm giá trị phân tích. Nên cảnh báo hoặc tự gộp khi người dùng chọn hai mốc trùng nhau.",
            "Nhánh lịch sử có cùng rủi ro loại ngày kết thúc do dùng điều kiện nhỏ hơn toDate mà không cộng thêm một ngày.",
        ],
    )

    add_report_section(
        document, "4.9", "Trung tâm điều hành bay", "Common/ChartReport.aspx", "1.png",
        [
            "Bộ lọc: tất cả sân bay, từ 13/08/2026 đến 13/08/2026.",
            "Tổng chuyến, hoàn thành, delay và cần chú ý đều bằng 0; donut bốn trạng thái đều 0%.",
            "Biểu đồ cất/hạ cánh không có chuyến Finished hoặc Delay. Biểu đồ xu hướng có điểm kỳ hiện tại ở 0 và một điểm cùng kỳ năm trước ở mức cao hơn.",
        ],
        [
            "Dashboard ghép dữ liệu của FlightStatusRate, FlightOperationOverview và FlightTrendAnalysis để tạo góc nhìn điều hành tổng hợp.",
            "Có bốn KPI, donut trạng thái, biểu đồ cất/hạ cánh, biểu đồ xu hướng và ba liên kết nhanh sang báo cáo chi tiết.",
            "Người dùng có thể chọn trạng thái hoặc cụm sân bay để lọc chéo; biểu đồ xu hướng có tooltip khi di chuột.",
        ],
        [
            "Dashboard phụ thuộc đồng thời ba nguồn dữ liệu; một request lỗi có thể khiến toàn bộ màn hình chuyển sang trạng thái lỗi.",
            "Trong ảnh, điểm cùng kỳ năm trước vẫn xuất hiện trong khi KPI hiện tại bằng 0. Cần dùng trang xu hướng để xem con số cụ thể và kiểm tra tính đầy đủ của dữ liệu ngày hiện tại.",
        ],
    )

    document.add_heading("5. Phân tích nghiệp vụ theo mã nguồn", level=1)
    add_body(document, "Chương này mô tả quy tắc xử lý được đọc trực tiếp từ JavaScript, WebMethod và code-behind trong repository. FlightStatusRate, AnomalyWarning và ChartReportAirport gọi WebMethod nội bộ. Các trang Overview, Trend, Military, Civil và AirportTakeoffLanding gọi API ngoài qua reportApiBase; do repository không có controller của API này, code-behind cùng tên chỉ được xem là thuật toán tham chiếu cho đến khi đối chiếu bản triển khai backend.")

    document.add_heading("5.1. Nghiệp vụ phân loại trạng thái chuyến bay", level=2)
    add_body(document, "Phạm vi đầu vào của thuật toán gồm chuyến có PERMTYPE bằng LD, hoặc O/F có ít nhất một đầu sân bay mang tiền tố VV. Chuyến phải có OPER_ID và nằm trong khoảng ngày. Hãng bay và sân bay được chuẩn hóa bằng cách bỏ khoảng trắng, chuyển chữ hoa; giá trị rỗng hoặc ALL được hiểu là không lọc.")
    add_body(document, "Nếu Từ ngày và Đến ngày đều là ngày hệ thống, nguồn chính là T_DAY_FLIGHTS_GOINGON. Các kỳ còn lại dùng T_FINISHED_FLIGHTS. Do bảng lịch sử không lưu EOBT, hệ thống tìm lại kế hoạch bay theo ngày bay và registration từ T_DAY_FLIGHTS_GOINGON, T_DAY_FLIGHTS_GOINGON032024 và T_T_DAY_FLIGHTS_GOINGON; sau đó ưu tiên bản ghi trùng callsign và hành trình, rồi trùng callsign, rồi trùng hành trình.")
    add_body(document, "Thời gian kế hoạch ưu tiên EOBTDATE, sau đó EOBT. Chuỗi thời gian được chấp nhận ở dạng DDHHMM hoặc HHMM. Với DDHHMM, ngày chỉ hợp lệ khi bằng ngày bay, ngày liền trước hoặc ngày liền sau. Số phút delay được làm tròn từ hiệu ATD trừ EOBT và chỉ được tính khi ATD nằm trong cửa sổ từ EOBT trừ 6 giờ đến EOBT cộng 24 giờ.")
    add_table(
        document,
        ["Thứ tự", "Trạng thái", "Điều kiện nghiệp vụ"],
        [
            ["1", "DELAY_60_PLUS", "Khởi hành từ sân bay VV, có sân bay đến, có ATD và trễ từ 60 phút"],
            ["2", "DELAY_30_59", "Cùng điều kiện trên, trễ từ 30 đến 59 phút"],
            ["3", "DELAY_15_29", "Cùng điều kiện trên, trễ từ 15 đến 29 phút"],
            ["4", "FINISHED", "Nội địa có đủ FROM/TO/ATD/ATA; quốc tế có đủ dữ liệu thực tế theo chiều đến hoặc đi Việt Nam"],
            ["5", "CANCEL", "Quốc tế thiếu dữ liệu bắt buộc theo chiều khai thác; nội địa thiếu dữ liệu; hoặc không khớp quy tắc khác"],
            ["6", "WAIT", "Chỉ ngày hiện tại: nội địa khởi hành Việt Nam, có đích và giờ kế hoạch nhưng chưa có ATD"],
        ],
        [1.4, 3.7, 11.2],
    )
    add_body(document, "Quy tắc delay được xét trước Finished. Vì vậy chuyến đã có đủ dữ liệu hoàn tất nhưng ATD trễ từ 15 phút vẫn thuộc nhóm Delay; chuyến trễ dưới 15 phút được xếp Finished nếu đủ điều kiện hoàn tất. Server trả bốn nhóm loại trừ nhau, còn frontend gom mọi trạng thái DELAY_* thành một nhóm Delay.")
    add_bullet(document, "Khoảng ngày của FlightStatusRate và các luồng dùng đúng code này có dạng nửa mở [Từ ngày, Đến ngày + 1), vì vậy bao gồm đầy đủ ngày kết thúc.")
    add_bullet(document, "Nhánh bổ sung T_DAY_FLIGHTS_CANCEL cho kỳ lịch sử không áp dụng bộ lọc hãng và sân bay. Đây là sai lệch nghiệp vụ mức cao vì KPI, donut, bảng và Excel có thể chứa chuyến hủy ngoài phạm vi đã chọn.")
    add_bullet(document, "Excel của FlightStatusRate xuất toàn bộ tập kết quả tải từ server, không chỉ trạng thái đang được người dùng chọn trên donut.")

    document.add_heading("5.2. Nghiệp vụ tổng quan khai thác", level=2)
    add_body(document, "FlightOperationOverview chỉ giữ Finished và Delay; Cancel và Wait không tham gia. Tổng chuyến bằng Finished cộng Delay. Mỗi chuyến có FROM_AIRP bắt đầu VV cộng một lượt cất cánh cho sân bay đi; TO_AIRP bắt đầu VV cộng một lượt hạ cánh cho sân bay đến. Vì một chuyến nội địa tạo hai movement nhưng chỉ là một chuyến, tổng cất cánh cộng tổng hạ cánh không được kỳ vọng bằng KPI tổng chuyến.")
    add_bullet(document, "Số sân bay khai thác là số mã VV có ít nhất một movement, không phải tổng số sân bay trong danh mục.")
    add_bullet(document, "Danh sách sân bay được xếp theo tổng cất + hạ cánh giảm dần, sau đó theo mã sân bay.")
    add_bullet(document, "Bấm cột cất/hạ cánh lọc bảng và Excel theo đúng chiều movement; nhưng donut lại lấy cả chuyến đi lẫn đến sân bay đó. Do đó donut có thể không khớp bảng chi tiết.")
    add_bullet(document, "Khoảng nhiều ngày có chứa hôm nay nhưng không chỉ riêng hôm nay dùng nguồn lịch sử; chuyến đang khai thác trong ngày có thể chưa xuất hiện.")

    document.add_heading("5.3. Nghiệp vụ phân tích xu hướng", level=2)
    add_body(document, "Xu hướng chỉ tính Finished và Delay. Kỳ so sánh được tạo bằng cách lùi chính xác Từ ngày và Đến ngày một năm. Dữ liệu được gom theo khóa yyyy-MM-dd ở chế độ ngày hoặc yyyy-MM ở chế độ tháng; các mốc không có dữ liệu vẫn được điền 0 để chuỗi thời gian liên tục.")
    add_table(
        document,
        ["Chỉ số", "Công thức"],
        [
            ["Tổng kỳ hiện tại", "Finished hiện tại + Delay hiện tại"],
            ["Tổng cùng kỳ", "Finished cùng kỳ + Delay cùng kỳ"],
            ["Chênh lệch", "Tổng hiện tại - Tổng cùng kỳ"],
            ["Tỷ lệ thay đổi", "Chênh lệch x 100 / Tổng cùng kỳ, làm tròn 1 chữ số"],
            ["Ngoại lệ nền 0", "Hai kỳ cùng 0 thì 0%; kỳ trước 0 và kỳ này dương thì code quy ước 100%"],
        ],
        [5.1, 11.2],
    )
    add_bullet(document, "Chế độ ngày hỗ trợ tối đa 367 ngày tính cả hai đầu; Dashboard tự chuyển sang tháng khi khoảng chọn lớn hơn 62 ngày.")
    add_bullet(document, "Quy ước tăng 100% từ nền 0 chưa chuẩn về thống kê; nên hiển thị N/A hoặc không xác định.")
    add_bullet(document, "Kỳ có ngày 29/02 có thể ánh xạ trùng 28/02 khi lùi một năm, làm tổng điểm biểu đồ cùng kỳ không khớp KPI.")

    document.add_heading("5.4. Nghiệp vụ cảnh báo delay", level=2)
    add_body(document, "AnomalyWarning chỉ truy vấn các chuyến trong ngày Oracle hiện tại từ T_DAY_FLIGHTS_GOINGON, có cả ETD và ATD. ETD phải là bốn chữ số HHmm; ATD phải là sáu chữ số ddHHmm. Ngày trong ATD chỉ được phép là ngày bay, ngày trước hoặc ngày sau, qua đó hỗ trợ chuyến bay qua nửa đêm.")
    add_body(document, "DelayMinutes bằng ATD timestamp trừ ETD timestamp. Dưới 15 phút hoặc cất sớm không tạo cảnh báo; 15-29 phút là mức 1; 30-59 phút là mức 2; từ 60 phút là mức 3. Danh sách được xếp theo mức giảm dần, số phút chậm giảm dần và callsign tăng dần.")
    add_bullet(document, "Bản ghi giờ sai định dạng bị loại âm thầm, không có KPI dữ liệu lỗi. Tổng cảnh báo vì vậy có thể thấp hơn số bản ghi nguồn cần kiểm tra.")
    add_bullet(document, "Tên trang là cảnh báo dữ liệu bất thường nhưng thuật toán hiện chỉ phát hiện chậm khởi hành.")

    document.add_heading("5.5. Nghiệp vụ chuyến bay quân sự", level=2)
    add_body(document, "Frontend gửi fromDate, toDate, airport và purpose tới api/MilitaryFlightReport/GetData. Client chỉ kiểm tra ngày không rỗng và Từ ngày không lớn hơn Đến ngày. Khi đổi ngày, lựa chọn sân bay và mục đích được đặt lại ALL. Danh mục sân bay/mục đích được cập nhật từ response của API.")
    add_bullet(document, "Bảng tải toàn bộ data.flights rồi phân trang client với cỡ 25/50/100/200/500; Excel xuất toàn bộ kết quả, không chỉ trang hiện tại.")
    add_bullet(document, "Repository không có source controller/query. Chưa thể xác nhận tiêu chí quân sự, ý nghĩa P_TYPE/PURPOSE, quy tắc biên ngày, loại trùng, xử lý null hoặc tên bảng T_FINISHFLIGHTS_MILITARY.")
    add_bullet(document, "Nhánh Excel chuẩn có STT; nhánh dự phòng khi ReportControls không tải thành công không có STT, dẫn đến đầu ra không hoàn toàn nhất quán.")

    document.add_heading("5.6. Nghiệp vụ tổng hợp dân dụng", level=2)
    add_body(document, "Frontend gửi FromDate, ToDate, Airport, Oper=ALL và CurrentDay=false tới api/CivilFlightSummary/GetData. Kỳ mặc định là 30 ngày gần nhất. Giao diện hiển thị trực tiếp Total, Domestic, International, OnTimePercent, Airports và Flights từ backend.")
    add_bullet(document, "Tỷ lệ nội địa = Domestic x 100 / Total; tỷ lệ quốc tế = International x 100 / Total; khi Total bằng 0 thì tỷ lệ bằng 0. OTP được hiển thị nguyên trạng từ API.")
    add_bullet(document, "Bảng lưu lượng chỉ lấy 12 phần tử đầu response và không tự sắp xếp; nhãn lưu lượng cao nhất chỉ đúng nếu backend đã sắp giảm dần.")
    add_bullet(document, "Không có validation ngày, phân trang hay xuất Excel phía client. Nút Áp dụng không khóa khi tải nên response cũ có thể về sau và ghi đè response mới.")
    add_bullet(document, "Repository không có source API nên chưa thể xác nhận quy tắc nội địa/quốc tế, ngưỡng OTP, đơn vị đếm chuyến, cách tính movement và nguồn Oracle.")

    document.add_heading("5.7. Nghiệp vụ cất/hạ cánh toàn quốc", level=2)
    add_body(document, "Thuật toán tham chiếu chỉ giữ Finished và Delay. Một chuyến có sân bay đi VV cộng một lượt cất cánh; sân bay đến VV cộng một lượt hạ cánh. Sân bay khai thác là số mã có movement. Sân bay lưu lượng cao nhất được chọn theo departures + arrivals giảm dần, nếu hòa thì theo mã tăng dần.")
    add_bullet(document, "Chi tiết chỉ chấp nhận mã sân bay VV và movement departure hoặc arrival. page tối thiểu 1; pageSize bị giới hạn từ 25 đến 500.")
    add_bullet(document, "Lỗi cận ngày: SQL dùng FLIGHTDATE < toDate nhưng code truyền nguyên ngày kết thúc. Chọn cùng một ngày tạo khoảng rỗng; chọn 01-03 chỉ lấy ngày 01 và 02.")
    add_bullet(document, "Phân trang vẫn đọc toàn bộ result set Oracle mỗi lần chuyển trang. Nếu yêu cầu trang vượt cuối, metadata page bị chặn nhưng rows có thể rỗng do offset đã tính theo trang cũ.")

    document.add_heading("5.8. Nghiệp vụ so sánh hoạt động sân bay", level=2)
    add_body(document, "Trang tải danh sách chuyến thô từ WebMethod rồi tổng hợp tại trình duyệt. Với một sân bay, hệ thống so cùng sân bay tại chính xác ngày 1 và ngày 2. Với từ hai đến năm sân bay, hệ thống không tạo hai kỳ mà cộng từng sân bay trong toàn khoảng từ ngày nhỏ hơn đến ngày lớn hơn.")
    add_bullet(document, "Một bản ghi được tính một chuyến liên quan nếu sân bay là đầu đi hoặc đầu đến; không tách cất cánh và hạ cánh. Nếu FROM và TO cùng mã, bản ghi vẫn chỉ cộng một lần.")
    add_bullet(document, "Total tăng cho mọi bản ghi, còn Finished/Cancel/Delay/Wait chỉ tăng khi trạng thái được nhận diện. Trạng thái lạ có thể làm Total lớn hơn tổng bốn nhóm.")
    add_bullet(document, "Không chặn chọn trùng sân bay hoặc hai ngày giống nhau; khi đó các chuỗi có thể hoàn toàn trùng lặp.")
    add_bullet(document, "Lỗi cận ngày lịch sử giống AirportTakeoffLanding: ngày kết thúc bị loại. Riêng đúng hôm nay mới truyền toDate + 1 và có dữ liệu hiện hành.")

    document.add_heading("5.9. Nghiệp vụ Dashboard điều hành", level=2)
    add_body(document, "Mỗi lần Áp dụng, Dashboard chạy song song ba request: trạng thái chuyến bay, tổng quan khai thác và xu hướng. Cùng bộ lọc sân bay/ngày được dùng; hãng luôn là ALL. Xu hướng theo ngày khi kỳ không quá 62 ngày, ngược lại theo tháng. Cơ chế jQuery.when yêu cầu cả ba request thành công; một request lỗi làm toàn Dashboard chuyển sang trạng thái lỗi.")
    add_bullet(document, "KPI tổng và bốn trạng thái lấy từ FlightStatusRate. Cần chú ý bằng Cancel + Wait.")
    add_bullet(document, "Biểu đồ cất/hạ cánh lấy từ Overview và chỉ tính Finished + Delay. Biểu đồ xu hướng cũng chỉ tính Finished + Delay và so với cùng kỳ năm trước.")
    add_bullet(document, "Click cụm sân bay chỉ lọc donut tại client; không đổi KPI, biểu đồ cột hoặc trend. Click trạng thái chỉ nhấn mạnh donut; không phải lọc chéo toàn Dashboard.")
    add_bullet(document, "Dashboard thừa hưởng lỗi chuyến hủy lịch sử không tuân bộ lọc sân bay/hãng từ FlightStatusRate.")

    document.add_heading("5.10. Ma trận luồng dữ liệu", level=2)
    add_table(
        document,
        ["Trang", "Endpoint frontend", "Nguồn/quy tắc có thể xác nhận"],
        [
            ["FlightStatusRate", "WebMethod cùng trang", "Oracle và toàn bộ quy tắc phân loại có source"],
            ["FlightOperationOverview", "API reportApiBase", "Có code-behind tham chiếu; cần đối chiếu controller triển khai"],
            ["FlightTrendAnalysis", "API reportApiBase", "Có code-behind tham chiếu; cần đối chiếu controller triển khai"],
            ["AnomalyWarning", "WebMethod cùng trang", "Oracle và công thức cảnh báo có source"],
            ["MilitaryFlightReport", "API reportApiBase", "Chỉ xác nhận payload/UI; backend ngoài repo"],
            ["CivilFlightSummary", "API reportApiBase", "Chỉ xác nhận payload/UI; backend ngoài repo"],
            ["AirportTakeoffLanding", "API reportApiBase", "Có code-behind tham chiếu; cần đối chiếu controller triển khai"],
            ["ChartReportAirport", "WebMethod cùng trang", "Oracle + tổng hợp client có source"],
            ["ChartReport", "Ba WebMethod/API phối hợp", "Tổng hợp từ Status, Overview và Trend"],
        ],
        [4.5, 4.2, 7.6],
    )

    document.add_heading("6. Các vấn đề ưu tiên", level=1)
    add_table(
        document,
        ["Mức", "Nội dung", "Ảnh hưởng", "Khuyến nghị"],
        [
            ["Cao", "Ngày kết thúc có nguy cơ bị loại ở AirportTakeoffLanding và ChartReportAirport", "Sai hoặc rỗng dữ liệu khi chọn cùng ngày", "Chuẩn hóa khoảng ngày [from, to + 1 ngày) và thêm test biên"],
            ["Cao", "Chuyến hủy lịch sử có thể không tuân bộ lọc sân bay/hãng", "KPI và bảng FlightStatusRate sai phạm vi", "Bổ sung điều kiện lọc vào truy vấn cancel và test đối chiếu"],
            ["Trung bình", "Một lỗi request có thể làm hỏng toàn Dashboard", "Mất toàn bộ góc nhìn dù còn nguồn hợp lệ", "Hiển thị từng widget độc lập và cảnh báo nguồn lỗi"],
            ["Trung bình", "Một số bảng tải toàn bộ dữ liệu về trình duyệt", "Chậm, tốn bộ nhớ ở kỳ dài", "Phân trang/lọc server và giới hạn khoảng ngày"],
            ["Thấp", "Tên cảnh báo bất thường rộng hơn chức năng delay", "Người dùng hiểu sai phạm vi", "Đổi tên hoặc mở rộng thêm quy tắc bất thường"],
        ],
        [1.5, 5.2, 4.7, 5.0],
    )

    document.add_heading("7. Kịch bản nghiệm thu đề xuất", level=1)
    for item in [
        "Chọn một ngày chắc chắn có dữ liệu và đối chiếu tổng KPI với số dòng chi tiết sau khi lọc từng trạng thái.",
        "Kiểm tra ngày đầu, ngày cuối và trường hợp Từ ngày bằng Đến ngày trên tất cả báo cáo.",
        "Đối chiếu bộ lọc sân bay/hãng với chuyến Cancel lịch sử và truy vấn nguồn.",
        "Kiểm tra click donut, thẻ trạng thái, cột sân bay và tooltip xu hướng; xác nhận bộ lọc chéo không làm sai tổng.",
        "Xuất Excel từ FlightStatusRate, FlightOperationOverview và MilitaryFlightReport; đối chiếu số dòng, cột, tiếng Việt và thời gian.",
        "Kiểm tra kỳ dài, dữ liệu rỗng, API timeout, Oracle lỗi và phản hồi sai định dạng; từng trang phải có thông báo rõ ràng.",
        "Kiểm tra giao diện ở độ phân giải desktop và mobile, đặc biệt bảng nhiều cột và vùng biểu đồ canvas.",
    ]:
        add_bullet(document, item)

    document.add_heading("8. Tệp mã nguồn đã đối chiếu", level=1)
    source_rows = [
        ("FlightStatusRate", "prjApplication/ReportNew/FlightStatusRate.js; FlightStatusRate.aspx.cs"),
        ("FlightOperationOverview", "prjApplication/ReportNew/FlightOperationOverview.js; FlightOperationOverview.aspx.cs"),
        ("FlightTrendAnalysis", "prjApplication/ReportNew/FlightTrendAnalysis.js; FlightTrendAnalysis.aspx.cs"),
        ("AnomalyWarning", "prjApplication/ReportNew/AnomalyWarning.js; AnomalyWarning.aspx.cs"),
        ("MilitaryFlightReport", "prjApplication/ReportNew/MilitaryFlightReport.js"),
        ("CivilFlightSummary", "prjApplication/ReportNew/FlightSummaryReports.js"),
        ("AirportTakeoffLanding", "prjApplication/ReportNew/AirportTakeoffLanding.js; AirportTakeoffLanding.aspx.cs"),
        ("ChartReportAirport", "prjApplication/Common/ChartReportAirport.js; ChartReportAirport.aspx.cs"),
        ("Dashboard", "prjApplication/ReportNew/Dashboard.js; ReportNew.js"),
    ]
    add_table(document, ["Trang", "Tệp đối chiếu"], source_rows, [5.0, 11.5])

    document.add_heading("9. Kết luận", level=1)
    add_body(document, "Chín trang đã hình thành một bộ báo cáo điều hành tương đối đầy đủ, bao phủ trạng thái chuyến bay, khai thác sân bay, xu hướng, cảnh báo delay, chuyến quân sự, chuyến dân dụng và so sánh sân bay. Giao diện nhất quán, dễ quét và có các tương tác phù hợp với nghiệp vụ.")
    add_body(document, "Dữ liệu ảnh xác nhận các báo cáo lịch sử có thể trả kết quả, nổi bật là một chuyến hoàn thành trong tổng quan khai thác và hai chuyến dân dụng với OTP 100%. Các màn hình ngày 13/08/2026 chủ yếu không có dữ liệu. Trước khi đưa vào nghiệm thu chính thức, cần ưu tiên kiểm tra quy ước ngày kết thúc, bộ lọc chuyến hủy lịch sử và khả năng chịu lỗi của Dashboard.")

    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    document.save(OUTPUT)
    return OUTPUT


if __name__ == "__main__":
    print(build_document())
