#!/usr/bin/env python3
"""Create the ADS-B tools operating guide using the supplied AMHS guide as a layout template."""

from __future__ import annotations

import argparse
import tempfile
from pathlib import Path
from zipfile import ZipFile

from docx import Document
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.shared import Cm, Pt


TODAY = "19/09/2026"


def clear_document_body(document: Document) -> None:
    """Keep the template styles and section setup, but replace its body content."""
    body = document._element.body
    for child in list(body):
        if not child.tag.endswith("}sectPr"):
            body.remove(child)


def set_cell_shading(cell, fill: str) -> None:
    properties = cell._tc.get_or_add_tcPr()
    shading = OxmlElement("w:shd")
    shading.set("{http://schemas.openxmlformats.org/wordprocessingml/2006/main}fill", fill)
    properties.append(shading)


def set_cell_text(cell, value: str, bold: bool = False) -> None:
    paragraph = cell.paragraphs[0]
    paragraph.alignment = WD_ALIGN_PARAGRAPH.LEFT
    run = paragraph.add_run(value)
    run.bold = bold


def add_table(document: Document, headers: list[str], rows: list[list[str]]) -> None:
    table = document.add_table(rows=1, cols=len(headers))
    table.style = "Table Grid"
    table.autofit = True
    for index, header in enumerate(headers):
        cell = table.rows[0].cells[index]
        set_cell_text(cell, header, bold=True)
        set_cell_shading(cell, "D9EAF7")
    for values in rows:
        cells = table.add_row().cells
        for index, value in enumerate(values):
            set_cell_text(cells[index], value)


def add_bullets(document: Document, values: list[str]) -> None:
    for value in values:
        document.add_paragraph(value, style="List Bullet")


def add_caption(document: Document, value: str) -> None:
    paragraph = document.add_paragraph(value)
    paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
    for run in paragraph.runs:
        run.italic = True


def add_picture(document: Document, path: Path, caption: str) -> None:
    paragraph = document.add_paragraph()
    paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
    paragraph.add_run().add_picture(str(path), width=Cm(15.8))
    add_caption(document, caption)


def add_toc_field(paragraph) -> None:
    begin = OxmlElement("w:fldChar")
    begin.set("{http://schemas.openxmlformats.org/wordprocessingml/2006/main}fldCharType", "begin")
    instruction = OxmlElement("w:instrText")
    instruction.set("{http://schemas.openxmlformats.org/wordprocessingml/2006/main}space", "preserve")
    instruction.text = ' TOC \\o "1-2" \\h \\z \\u '
    separate = OxmlElement("w:fldChar")
    separate.set("{http://schemas.openxmlformats.org/wordprocessingml/2006/main}fldCharType", "separate")
    text = OxmlElement("w:t")
    text.text = "Nhấn Ctrl+A rồi F9 trong Word để cập nhật mục lục."
    end = OxmlElement("w:fldChar")
    end.set("{http://schemas.openxmlformats.org/wordprocessingml/2006/main}fldCharType", "end")
    paragraph._p.append(begin)
    paragraph._p.append(instruction)
    paragraph._p.append(separate)
    paragraph._p.append(text)
    paragraph._p.append(end)


def extract_source_images(source_document: Path, destination: Path) -> tuple[Path, Path]:
    """Reuse the two real ADS-B screenshots already embedded in the project SRS."""
    with ZipFile(source_document) as archive:
        media = sorted(name for name in archive.namelist() if name.startswith("word/media/"))
        if len(media) < 2:
            raise ValueError("SRS ADS-B không có đủ hai ảnh giao diện.")
        paths = []
        for index, name in enumerate(media[:2], start=1):
            suffix = Path(name).suffix or ".png"
            image_path = destination / f"adsb-screen-{index}{suffix}"
            image_path.write_bytes(archive.read(name))
            paths.append(image_path)
    return paths[0], paths[1]


def add_cover(document: Document) -> None:
    title = document.add_paragraph(style="Title")
    title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    title.paragraph_format.space_before = Pt(130)
    run = title.add_run("TÀI LIỆU HƯỚNG DẪN SỬ DỤNG")
    run.bold = True
    run.font.name = "Arial"
    run.font.size = Pt(23)

    subtitle = document.add_paragraph(style="Title")
    subtitle.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = subtitle.add_run("CÔNG CỤ ĐỒNG BỘ VÀ THEO DÕI ADS-B")
    run.bold = True
    run.font.name = "Arial"
    run.font.size = Pt(20)

    details = document.add_paragraph()
    details.alignment = WD_ALIGN_PARAGRAPH.CENTER
    details.paragraph_format.space_before = Pt(46)
    details.add_run(
        "Hệ thống Quản lý luồng không lưu Việt Nam - ATFM Web\n"
        "Thành phần: TracksSync và Flight Tracking API\n"
        f"Phiên bản tài liệu: 1.0\nNgày cập nhật: {TODAY}"
    )
    document.add_page_break()


def build(template: Path, source_srs: Path, output: Path) -> None:
    document = Document(template)
    clear_document_body(document)
    document.core_properties.title = "Hướng dẫn sử dụng công cụ ADS-B"
    document.core_properties.subject = "TracksSync và Flight Tracking API"
    document.core_properties.author = "VATM ATFM"

    add_cover(document)
    document.add_heading("MỤC LỤC", level=1)
    toc = document.add_paragraph()
    add_toc_field(toc)
    document.add_page_break()

    document.add_heading("Giới thiệu", level=1)
    document.add_paragraph(
        "Bộ công cụ ADS-B phục vụ hai nhu cầu bổ trợ trong ATFM: lưu dữ liệu giám sát để báo cáo, "
        "và cung cấp vị trí tàu bay gần thời gian thực cho bản đồ theo dõi. Tài liệu này hướng dẫn "
        "cán bộ vận hành sử dụng TracksSync và kiểm tra Flight Tracking API; không thay thế tài liệu "
        "quản trị cơ sở dữ liệu hoặc hướng dẫn triển khai hạ tầng."
    )
    add_table(
        document,
        ["Thành phần", "Mục đích", "Dữ liệu xử lý"],
        [
            ["TracksSync", "Đồng bộ, đối chiếu và xác minh dữ liệu ADS-B theo chu kỳ.", "PostgreSQL public.tracks -> Oracle T_TRACKS_LOG."],
            ["Flight Tracking API", "Cung cấp vị trí mới nhất cho FlightTrackingMap của ATFM Web.", "Đọc PostgreSQL public.tracks qua HTTP nội bộ."],
            ["ATFM Web", "Hiển thị bản đồ và báo cáo ADS-B cho người dùng nghiệp vụ.", "T_TRACKS_LOG, Flight Tracking API và metadata Oracle."],
        ],
    )
    document.add_paragraph("Luồng dữ liệu tổng quát: public.tracks -> TracksSync -> T_TRACKS_LOG -> báo cáo ADS-B; đồng thời public.tracks -> Flight Tracking API -> FlightTrackingMap.")

    document.add_heading("1. Chuẩn bị và cài đặt", level=1)
    add_bullets(document, [
        "Máy chạy công cụ là Windows 10/11 64-bit và kết nối được tới PostgreSQL, Oracle, cũng như file GeoJSON vùng FIR VVHN/VVHM.",
        "Dùng bản EXE đã đóng gói khi vận hành thông thường: ATFM-TracksSync.exe cho giao diện, ATFM-TracksSync-CLI.exe cho Task Scheduler và ATFM-FlightTrackingApi.exe cho dịch vụ API.",
        "Đặt TracksSync.local.json cạnh EXE khi triển khai riêng; file cấu hình và watermark chứa thông tin vận hành nên không đưa lên Git hoặc gửi qua kênh không được phép.",
        "Không chạy hai phiên TracksSync cùng dùng một bảng T_TRACKS_LOG hoặc một file watermark.",
    ])

    document.add_heading("2. Cấu hình kết nối", level=1)
    document.add_paragraph(
        "TracksSync ưu tiên cấu hình theo thứ tự: biến môi trường TRACKS_CONFIG, file TracksSync.local.json "
        "cạnh chương trình, sau đó tới prjApplication\\App_Data\\TracksSync.local.json. Flight Tracking API dùng "
        "FlightTrackingApi.local.json cạnh EXE hoặc các biến môi trường FLIGHT_API_PG_* và FLIGHT_API_HOST/PORT."
    )
    add_table(document, ["Nhóm cấu hình", "Nội dung cần kiểm tra"], [
        ["PostgreSQL", "Host, port, database, tài khoản chỉ đọc và thời gian chờ kết nối."],
        ["Oracle", "Host, port, service, quyền đọc T_DAY_FLIGHTS_GOINGON và ghi T_TRACKS_LOG cho TracksSync."],
        ["FIR", "Đường dẫn tuyệt đối tới file GeoJSON chứa vùng VVHN và VVHM, tài khoản chạy tool có quyền đọc."],
        ["Watermark", "Đường dẫn file mốc đồng bộ; để trống để tool tự đặt cạnh chương trình."],
        ["API", "Host/port dịch vụ; port mặc định 5088. Chỉ cho phép mạng nội bộ truy cập qua firewall."],
    ])
    document.add_paragraph("Không ghi mật khẩu thật trong file hướng dẫn, ảnh chụp màn hình, log gửi hỗ trợ hoặc package ZIP. Sau khi thay đổi cấu hình, luôn chạy kiểm tra trước khi bật đồng bộ tự động.")

    document.add_heading("3. Màn hình TracksSync", level=1)
    document.add_paragraph("Mở ATFM-TracksSync.exe. Giao diện có vùng log và năm thao tác: Kiểm tra đối chiếu, Ghi T_TRACKS_LOG, Xác minh kết quả, Auto và Stop. Chu kỳ là khoảng nghỉ sau khi một lượt hoàn tất, phải là số nguyên dương.")
    document.add_heading("3.1 Kiểm tra đối chiếu và ghi dữ liệu", level=2)
    add_bullets(document, [
        "Bấm Kiểm tra đối chiếu trước. Tool đọc public.tracks, loại điểm ngoài FIR, chuẩn hóa callsign và đối chiếu chuyến bay Oracle nhưng chưa ghi dữ liệu nghiệp vụ.",
        "Đọc log để xác nhận đúng file cấu hình, kết nối PostgreSQL/Oracle, số callsign/ngày đối chiếu và các trường hợp bị bỏ qua.",
        "Bấm Ghi T_TRACKS_LOG khi kết quả kiểm tra hợp lệ. Tool MERGE theo khóa CALLSIGN + DATE, cập nhật vị trí mới nhất và chỉ ghi watermark sau khi đồng bộ thành công.",
        "Bấm Xác minh kết quả để nhận thống kê đúng, sai, thiếu, thừa và trùng khóa. Một lượt bình thường không có traceback; sai, thiếu và trùng khóa phải bằng 0.",
    ])
    document.add_heading("3.2 Quy tắc dữ liệu chính", level=2)
    add_table(document, ["Quy tắc", "Cách xử lý của tool"], [
        ["Callsign", "Lấy phần trước dấu gạch nối của flight_id_current, bỏ khoảng trắng và chuyển sang chữ hoa."],
        ["FIR", "VVHN hoặc biên chung có STATUS=1; VVHM có STATUS=2; ngoài hai FIR bị loại."],
        ["Đối chiếu chuyến bay", "Tìm CALLSIGN + ngày trong T_DAY_FLIGHTS_GOINGON; trường hợp nhiều dòng dùng khoảng ETD-ETA, kể cả qua nửa đêm."],
        ["Không match", "Vẫn ghi bản ghi đủ điều kiện với PERMTYPE=OTHER, thông tin hành trình để trống."],
        ["Thời điểm FIR", "Giữ TIME_IN sớm nhất; TIME_OUT là lần đầu đổi FIR sau đó, nếu có."],
        ["Bản ghi cũ", "Cập nhật tọa độ/thời gian mới nhất; có thể bổ sung metadata khi bản ghi OTHER được match ở lượt sau."],
    ])

    document.add_heading("4. Chạy tự động và dòng lệnh", level=1)
    document.add_paragraph("Auto thực hiện check -> sync -> verify trên cùng tập dữ liệu, sau đó mới đếm ngược tới lượt tiếp theo. Nút Stop yêu cầu dừng vòng Auto; lượt đang thao tác cơ sở dữ liệu sẽ hoàn tất an toàn trước khi dừng.")
    add_table(document, ["Lệnh/chế độ", "Công dụng"], [
        ["--mode check", "Đọc và đối chiếu, không ghi log nghiệp vụ hoặc watermark."],
        ["--mode sync", "Ghi/cập nhật T_TRACKS_LOG và cập nhật watermark."],
        ["--mode verify", "Đối chiếu dữ liệu dự kiến với Oracle, không cập nhật watermark."],
        ["--mode all", "Chạy tuần tự check, sync và verify."],
        ["--full", "Bỏ qua watermark để quét lại toàn bộ; chỉ dùng khi chạy lần đầu, backfill hoặc xử lý watermark sai."],
    ])
    document.add_paragraph("Ví dụ chạy qua Task Scheduler: Program/script là ATFM-TracksSync-CLI.exe, Arguments là --mode all, Start in là thư mục chứa EXE và TracksSync.local.json.")

    document.add_heading("5. Flight Tracking API", level=1)
    document.add_paragraph("Flight Tracking API chạy độc lập trên máy có quyền đọc PostgreSQL, giúp IIS chỉ cần gọi HTTP nội bộ. Sau khi khởi động bản EXE nền, dùng các địa chỉ kiểm tra bên dưới; thay 127.0.0.1 bằng IP máy API khi kiểm tra từ máy được phép khác.")
    add_table(document, ["Địa chỉ", "Kết quả mong đợi"], [
        ["/health/live", "HTTP 200 và trạng thái ok: tiến trình API đang hoạt động."],
        ["/health/ready", "HTTP 200 khi kết nối PostgreSQL sẵn sàng; HTTP 503 khi chưa sẵn sàng."],
        ["/swagger/", "Mở giao diện Swagger để xem và thử các endpoint."],
        ["/openapi.json", "Trả đặc tả OpenAPI dạng JSON."],
        ["/api/v1/tracks?date=yyyy-MM-dd", "Trả vị trí mới nhất của mỗi callsign trong ngày hợp lệ."],
    ])
    document.add_paragraph("API không dùng X-API-Key. Vì vậy chỉ mở cổng 5088 cho IP máy IIS hoặc mạng vận hành đã được phép; không công bố endpoint trực tiếp ra Internet. Khi thay đổi địa chỉ API, cập nhật FlightTrackingApiBaseUrl và FlightTrackingApiTimeoutSeconds trong cấu hình ATFM Web theo quy trình quản trị hệ thống.")

    with tempfile.TemporaryDirectory(prefix="adsb-guide-") as temporary_directory:
        report_image, map_image = extract_source_images(source_srs, Path(temporary_directory))
        document.add_heading("6. Kết quả khai thác ADS-B", level=1)
        document.add_paragraph("Dữ liệu do hai tool xử lý được khai thác trên ATFM Web. Ảnh dưới đây là giao diện thực tế đã lưu trong tài liệu kỹ thuật ADS-B của dự án, dùng để nhận biết đầu ra nghiệp vụ của quy trình.")
        add_picture(document, report_image, "Hình 1. Báo cáo hiệu suất ADS-B sử dụng dữ liệu T_TRACKS_LOG.")
        add_picture(document, map_image, "Hình 2. Bản đồ theo dõi chuyến bay sử dụng Flight Tracking API.")

    document.add_heading("7. Xử lý sự cố nhanh", level=1)
    add_table(document, ["Hiện tượng", "Cách xử lý"], [
        ["Không kết nối PostgreSQL/Oracle", "Kiểm tra host, port, service, tài khoản, firewall và quyền database; chạy lại Kiểm tra đối chiếu."],
        ["Không có dòng mới", "Kiểm tra watermark, updated_at_utc của nguồn và vị trí có nằm trong FIR; thử check --full trước khi ghi lại."],
        ["Nhiều dòng bị bỏ qua", "Rà soát chuyến bay trùng CALLSIGN + ngày và bảo đảm ETD/ETA hợp lệ để chọn được một khoảng duy nhất."],
        ["API ready trả 503", "Kiểm tra cấu hình PostgreSQL và đường mạng từ máy API; live=200 chỉ xác nhận tiến trình còn chạy."],
        ["Bản đồ không có dữ liệu", "Kiểm tra health/live, health/ready, firewall cổng 5088 và cấu hình FlightTrackingApiBaseUrl của ATFM Web."],
        ["Cần chạy lại toàn bộ", "Dừng Auto, sao lưu/đánh giá dữ liệu, chạy check --full trước; chỉ sync --full sau khi xác nhận kết quả."],
    ])

    document.add_heading("8. Vận hành an toàn", level=1)
    add_bullets(document, [
        "Luôn kiểm tra log sau mỗi thay đổi cấu hình; không bỏ qua traceback, lỗi sai/thiếu hoặc trùng khóa.",
        "Không sửa hoặc xóa watermark trong khi Auto đang chạy. Sao lưu dữ liệu trước khi dùng --full trên môi trường thật.",
        "Giữ quyền tối thiểu: Flight Tracking API chỉ cần đọc PostgreSQL; TracksSync chỉ dùng quyền Oracle cần thiết cho nhiệm vụ đồng bộ.",
        "Theo dõi tuổi watermark, kết quả verify, dung lượng T_TRACKS_LOG, trạng thái health/ready và tần suất lỗi Auto.",
        "Dừng các tiến trình liên quan trước khi nâng cấp; giữ lại file cấu hình local và log cần thiết để có thể khôi phục vận hành.",
    ])

    output.parent.mkdir(parents=True, exist_ok=True)
    document.save(output)


def main() -> None:
    parser = argparse.ArgumentParser(description="Tạo hướng dẫn sử dụng công cụ ADS-B.")
    parser.add_argument("--template", type=Path, required=True)
    parser.add_argument("--source-srs", type=Path, required=True)
    parser.add_argument("--output", type=Path, required=True)
    args = parser.parse_args()
    for path in (args.template, args.source_srs):
        if not path.is_file():
            raise FileNotFoundError(path)
    build(args.template, args.source_srs, args.output)


if __name__ == "__main__":
    main()
