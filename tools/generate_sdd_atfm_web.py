import os
import re
from pathlib import Path

from docx import Document
from docx.enum.section import WD_SECTION
from docx.enum.table import WD_CELL_VERTICAL_ALIGNMENT, WD_TABLE_ALIGNMENT
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Cm, Pt, RGBColor


ROOT = Path(__file__).resolve().parents[1]
OUTPUT = Path(os.environ.get(
    "SDD_ATFM_OUTPUT",
    ROOT / "TaiLieu" / "SDD_ATFM_WEB_TONG_THE_V1_082026.docx",
))
SRS_SOURCE = ROOT / "TaiLieu" / "SRS_ATFM_TLKT_V1_082026.docx"


def set_cell_text(cell, value, bold=False, color=None):
    cell.text = ""
    paragraph = cell.paragraphs[0]
    paragraph.paragraph_format.space_after = Pt(0)
    for index, line in enumerate(str(value).split("\n")):
        if index:
            paragraph.add_run().add_break()
        run = paragraph.add_run(line)
        run.font.name = "Times New Roman"
        run._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
        run.font.size = Pt(9)
        run.bold = bold
        if color:
            run.font.color.rgb = RGBColor(*color)
    cell.vertical_alignment = WD_CELL_VERTICAL_ALIGNMENT.CENTER


def table(doc, headers, rows, widths=None):
    result = doc.add_table(rows=1, cols=len(headers))
    result.style = "Table Grid"
    result.alignment = WD_TABLE_ALIGNMENT.CENTER
    for index, header in enumerate(headers):
        set_cell_text(result.rows[0].cells[index], header, bold=True, color=(31, 78, 121))
    for values in rows:
        cells = result.add_row().cells
        for index, value in enumerate(values):
            set_cell_text(cells[index], value)
    if widths:
        for row in result.rows:
            for index, width in enumerate(widths):
                if index < len(row.cells):
                    row.cells[index].width = Cm(width)
    doc.add_paragraph()
    return result


def paragraph(doc, text, bold=False, italic=False, align=None):
    p = doc.add_paragraph()
    p.paragraph_format.space_after = Pt(6)
    p.paragraph_format.line_spacing = 1.12
    if align is not None:
        p.alignment = align
    run = p.add_run(text)
    run.font.name = "Times New Roman"
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    run.font.size = Pt(10.5)
    run.bold = bold
    run.italic = italic
    return p


def bullet(doc, text, level=0):
    p = doc.add_paragraph(style="List Bullet" if level == 0 else "List Bullet 2")
    p.paragraph_format.space_after = Pt(3)
    run = p.add_run(text)
    run.font.name = "Times New Roman"
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    run.font.size = Pt(10.5)
    return p


def code_block(doc, text):
    p = doc.add_paragraph()
    p.paragraph_format.left_indent = Cm(0.6)
    p.paragraph_format.right_indent = Cm(0.6)
    p.paragraph_format.space_before = Pt(5)
    p.paragraph_format.space_after = Pt(8)
    p.paragraph_format.line_spacing = 1.0
    run = p.add_run(text)
    run.font.name = "Consolas"
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Consolas")
    run.font.size = Pt(8.5)
    run.font.color.rgb = RGBColor(50, 50, 50)
    return p


def heading(doc, text, level):
    result = doc.add_heading(text, level=level)
    result.paragraph_format.keep_with_next = True
    return result


def page_break(doc):
    doc.add_page_break()


def add_toc(doc):
    title = doc.add_paragraph()
    title.alignment = WD_ALIGN_PARAGRAPH.CENTER
    title.paragraph_format.space_after = Pt(12)
    run = title.add_run("MỤC LỤC")
    run.bold = True
    run.font.name = "Times New Roman"
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    run.font.size = Pt(15)

    paragraph_node = doc.add_paragraph()
    begin_run = OxmlElement("w:r")
    begin = OxmlElement("w:fldChar")
    begin.set(qn("w:fldCharType"), "begin")
    begin.set(qn("w:dirty"), "true")
    begin_run.append(begin)
    instruction_run = OxmlElement("w:r")
    instruction = OxmlElement("w:instrText")
    instruction.set(qn("xml:space"), "preserve")
    instruction.text = ' TOC \\o "1-3" \\h \\z \\u '
    instruction_run.append(instruction)
    separate_run = OxmlElement("w:r")
    separate = OxmlElement("w:fldChar")
    separate.set(qn("w:fldCharType"), "separate")
    separate_run.append(separate)
    placeholder_run = OxmlElement("w:r")
    placeholder = OxmlElement("w:t")
    placeholder.text = "Mở tài liệu bằng Microsoft Word và cập nhật trường để hiển thị mục lục."
    placeholder_run.append(placeholder)
    end_run = OxmlElement("w:r")
    end = OxmlElement("w:fldChar")
    end.set(qn("w:fldCharType"), "end")
    end_run.append(end)
    paragraph_node._p.extend([begin_run, instruction_run, separate_run, placeholder_run, end_run])

    settings = doc.settings._element
    update_fields = settings.find(qn("w:updateFields"))
    if update_fields is None:
        update_fields = OxmlElement("w:updateFields")
        settings.append(update_fields)
    update_fields.set(qn("w:val"), "true")


def add_page_number(section):
    footer = section.footer.paragraphs[0]
    footer.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = footer.add_run("Trang ")
    run.font.name = "Times New Roman"
    run.font.size = Pt(9)
    begin = OxmlElement("w:fldChar")
    begin.set(qn("w:fldCharType"), "begin")
    instruction = OxmlElement("w:instrText")
    instruction.set(qn("xml:space"), "preserve")
    instruction.text = "PAGE"
    end = OxmlElement("w:fldChar")
    end.set(qn("w:fldCharType"), "end")
    run._r.extend([begin, instruction, end])


def add_cover(doc):
    for _ in range(3):
        paragraph(doc, "", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "TÀI LIỆU THIẾT KẾ PHẦN MỀM", bold=True, align=WD_ALIGN_PARAGRAPH.CENTER)
    p = paragraph(doc, "ATFM_WEB", bold=True, align=WD_ALIGN_PARAGRAPH.CENTER)
    p.runs[0].font.size = Pt(24)
    p.runs[0].font.color.rgb = RGBColor(31, 78, 121)
    paragraph(doc, "SDD_ATFM_WEB_TONG_THE_V1_082026", bold=True, align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Thiết kế kiến trúc và thiết kế chi tiết tổng thể", italic=True, align=WD_ALIGN_PARAGRAPH.CENTER)
    for _ in range(5):
        paragraph(doc, "", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Phiên bản: 1.0", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Ngày lập: 24/08/2026", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Tài liệu nguồn: SRS_ATFM_TLKT_V1_082026.docx", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Đơn vị lập: ........................................................", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Người kiểm tra: ....................................................", align=WD_ALIGN_PARAGRAPH.CENTER)
    paragraph(doc, "Người phê duyệt: ..................................................", align=WD_ALIGN_PARAGRAPH.CENTER)
    page_break(doc)


def add_control(doc):
    heading(doc, "KIỂM SOÁT TÀI LIỆU", 1)
    table(doc, ["Thuộc tính", "Nội dung"], [
        ("Tên tài liệu", "Thiết kế kiến trúc và thiết kế chi tiết tổng thể ATFM_WEB"),
        ("Mã tài liệu", "SDD-ATFM-WEB"),
        ("Phiên bản", "1.0"),
        ("Ngày ban hành dự kiến", "24/08/2026"),
        ("Tài liệu nguồn", "SRS_ATFM_TLKT_V1_082026.docx; mã nguồn ATFM_WEB; script Oracle; cấu hình tích hợp"),
        ("Trạng thái", "Bản thiết kế cơ sở – chờ rà soát và phê duyệt"),
    ], [4.2, 12.3])
    heading(doc, "Lịch sử thay đổi", 2)
    table(doc, ["Phiên bản", "Ngày", "Nội dung", "Người thực hiện"], [
        ("0.1", "24/08/2026", "Tạo khung thiết kế tổng thể và phân rã 5 phân hệ", "Codex"),
        ("1.0", "24/08/2026", "Bổ sung kiến trúc, dữ liệu, tích hợp, bảo mật, vận hành và thiết kế chi tiết", "Codex"),
    ])
    heading(doc, "Nguyên tắc sử dụng tài liệu", 2)
    paragraph(doc, "SDD là cơ sở để thiết kế chi tiết, phát triển, cấu hình tích hợp và bàn giao vận hành. Mọi thay đổi làm ảnh hưởng đến mã FR, giao diện công khai, cấu trúc dữ liệu hoặc hợp đồng tích hợp phải cập nhật đồng thời SDD, sơ đồ liên quan và bảng ánh xạ yêu cầu.")
    paragraph(doc, "Các tên package/bảng/API chưa có trong mã nguồn hiện tại được đánh dấu là tên thiết kế đề xuất; trước khi lập trình phải chốt tên vật lý, schema, phiên bản và chủ sở hữu.")
    page_break(doc)


def add_scope(doc):
    heading(doc, "1. Mục đích, phạm vi và quy ước thiết kế", 1)
    heading(doc, "1.1. Mục đích", 2)
    paragraph(doc, "Tài liệu mô tả cách chuyển các yêu cầu trong SRS thành kiến trúc, thành phần phần mềm, luồng xử lý, mô hình dữ liệu, hợp đồng tích hợp và kế hoạch triển khai cho ATFM_WEB. Thiết kế ưu tiên khả năng nâng cấp từng phần, giữ tương thích với các màn hình WebForms hiện hữu và kiểm soát rủi ro khi tích hợp hệ thống ngoài.")
    heading(doc, "1.2. Phạm vi thiết kế", 2)
    table(doc, ["Nhóm", "Phạm vi", "Đầu ra thiết kế"], [
        ("INT", "Email/File, ADS-B, SLOT, AMHS/AFTN, API Gateway và Bravo", "Adapter, queue, mapping, retry, quarantine, audit"),
        ("ALT", "Cảnh báo runtime, Live Fire Message, KHB quân sự", "State machine, notification, approval, message dispatch"),
        ("OPT", "Tra cứu, cảnh báo, KHB, export, đồng bộ, đối soát và tối ưu", "Module service, package contract, batch job, index và cache"),
        ("RPT", "Dashboard, biểu đồ, báo cáo ADS-B, quân sự, sân bay", "Query model, snapshot, drill-down, export và phân trang"),
        ("AI", "NL2SQL/Vanna, Chatbot, Oracle read-only", "AI service, SQL guard, session, training review và API"),
        ("COM", "Đăng nhập, RBAC, input/output, audit, cấu hình, logging", "Cross-cutting components và policy dùng chung"),
    ])
    heading(doc, "1.3. Quy ước mã thiết kế", 2)
    table(doc, ["Mã", "Ý nghĩa", "Ví dụ"], [
        ("HLD-ARCH", "Quyết định kiến trúc cấp cao", "HLD-ARCH-001: Oracle là nguồn dữ liệu nghiệp vụ chính"),
        ("CMP", "Thành phần phần mềm", "CMP-MSG-001: MessageApplicationService"),
        ("INT", "Hợp đồng tích hợp", "INT-BRV-001: Bravo sync API"),
        ("DAT", "Thiết kế dữ liệu", "DAT-FLT-001: Flight identity và phiên bản"),
        ("SEC", "Thiết kế bảo mật", "SEC-RBAC-001: kiểm tra quyền phía máy chủ"),
        ("OPS", "Thiết kế triển khai/vận hành", "OPS-JOB-001: hàng chờ đồng bộ nền"),
    ])
    heading(doc, "1.4. Nguyên tắc quyết định", 2)
    for item in [
        "Giữ nguyên URL/Menu_ID và hành vi hợp lệ của màn hình hiện hữu; thay đổi phải có compatibility layer khi cần.",
        "Không để code-behind chứa truy vấn nghiệp vụ phức tạp hoặc gọi trực tiếp nhiều package ngoài phạm vi service.",
        "Không dùng distributed transaction giữa Oracle và hệ thống ngoài; dùng outbox, idempotency và reconciliation.",
        "Ngày giờ lưu theo UTC hoặc timezone nguồn; ngày nghiệp vụ được xác định riêng bằng BusinessDateService.",
        "Mọi lỗi phải có mã ổn định, thông báo an toàn cho người dùng và chi tiết kỹ thuật trong log có kiểm soát.",
    ]:
        bullet(doc, item)
    page_break(doc)


def add_context(doc):
    heading(doc, "2. Kiến trúc ngữ cảnh và kiến trúc triển khai", 1)
    heading(doc, "2.1. Các tác nhân và hệ thống liên quan", 2)
    table(doc, ["Tác nhân/hệ thống", "Kênh", "Dữ liệu hoặc trách nhiệm"], [
        ("Khai thác viên", "Browser/HTTPS", "Tra cứu, nhập, sửa, duyệt, phát điện văn và xem báo cáo theo phạm vi quyền"),
        ("Người duyệt", "Browser/HTTPS", "Duyệt Live Fire, KHB quân sự, Daily Statistic và các luồng yêu cầu phê duyệt"),
        ("Quản trị", "Browser/HTTPS/Management", "Danh mục, cấu hình, user/role/menu, giám sát và xử lý hàng chờ"),
        ("Email/File source", "IMAP/SMTP/File share", "Điện văn, file SLOT/KHH và tệp dữ liệu đầu vào"),
        ("ADS-B source", "HTTPS/SFTP/DB service", "Vệt bay, thời gian, vị trí, chất lượng và trạng thái nguồn"),
        ("AMHS/AFTN", "Adapter/queue", "Gửi/nhận điện văn hàng không và trạng thái delivery"),
        ("Bravo 10", "REST/JSON", "Đồng bộ số liệu chuyến, actual time, purpose và đối soát"),
        ("API consumers", "API Gateway", "Ứng dụng/hệ thống tiêu thụ dữ liệu được cấp scope"),
    ])
    heading(doc, "2.2. Sơ đồ ngữ cảnh", 2)
    code_block(doc, """[Khai thác viên] ──HTTPS──┐
[Người duyệt/Quản trị] ────┼──> [DMZ: Reverse Proxy/API Gateway]
[API Consumer] ────────────┘                 │
                                             ▼
                         [ATFM_WEB Application Zone - IIS]
                              │        │             │
                              │        │             └──> [AI Service]
                              │        └────────────────> [Integration Workers]
                              ▼                         │
                     [Oracle ATFM / Audit]              ├── Email/File
                              │                         ├── ADS-B/SLOT
                              └── Reports/Exports        ├── AMHS/AFTN
                                                        └── Bravo 10""")
    paragraph(doc, "DMZ chỉ tiếp nhận và kiểm soát kết nối. Application Zone là nơi thực hiện xác thực, phân quyền, điều phối nghiệp vụ và gọi DAL. Data Zone không cho phép hệ thống ngoài kết nối trực tiếp vào Oracle ATFM.")
    heading(doc, "2.3. Kiến trúc triển khai đề xuất", 2)
    table(doc, ["Vùng", "Máy chủ/dịch vụ", "Luồng cho phép", "Kiểm soát"], [
        ("DMZ", "Reverse Proxy, API Gateway, WAF nếu có", "HTTPS từ người dùng/API consumer đến endpoint công khai", "TLS, rate limit, IP allowlist, request ID"),
        ("Application", "IIS/ASP.NET ATFM_WEB; Worker Services; Scheduler", "Oracle, queue, external adapters", "Service account, firewall egress, timeout"),
        ("Integration", "ADS-B sync, AeroSync, AMHS adapter, Bravo adapter", "Nguồn ngoài qua endpoint đã phê duyệt", "Retry, circuit breaker, quarantine, secret store"),
        ("Data", "Oracle ATFM; PostgreSQL ADS-B theo dịch vụ; Redis/RabbitMQ nếu triển khai", "Chỉ Application/Integration qua cổng được duyệt", "Network ACL, read/write account, backup"),
    ])
    heading(doc, "2.4. Các quyết định kiến trúc bắt buộc", 2)
    table(doc, ["Mã", "Quyết định", "Lý do"], [
        ("HLD-ARCH-001", "Oracle ATFM là nguồn dữ liệu nghiệp vụ chuẩn; mọi cache/report phải chỉ rõ snapshot và thời điểm", "Tránh nhiều nguồn sự thật và sai lệch báo cáo"),
        ("HLD-ARCH-002", "Bravo chỉ tích hợp qua API, không ghi trực tiếp vào DB Bravo", "Giảm phụ thuộc schema và kiểm soát xung đột"),
        ("HLD-ARCH-003", "Tác vụ lớn chạy nền, giao diện nhận batch status", "Không giữ request HTTP quá lâu và đáp ứng mục tiêu dưới 3 phút"),
        ("HLD-ARCH-004", "AI chạy tách process/service với Oracle user chỉ đọc", "Cách ly dependency Python/LLM và giảm rủi ro DML"),
        ("HLD-ARCH-005", "Tất cả luồng ghi có audit và correlation ID", "Đối soát, vận hành và xử lý sự cố nhất quán"),
    ])
    page_break(doc)


def add_logical_architecture(doc):
    heading(doc, "3. Kiến trúc logic và phân rã thành phần", 1)
    heading(doc, "3.1. Các lớp phần mềm", 2)
    table(doc, ["Lớp", "Trách nhiệm", "Thành phần hiện hữu/đề xuất"], [
        ("Presentation", "Render ASPX, callback, validate hiển thị, giữ trạng thái bộ lọc", "prjApplication, Masters, JavaScript, WebMethod"),
        ("Application", "Điều phối use case, transaction boundary, authorization context", "Service mới trong prjBusinessLogic/Application"),
        ("Domain", "Quy tắc ngày bay, trạng thái điện văn, matching, conflict policy", "FlightDomain, MessageDomain, ReportDomain"),
        ("Data Access", "Bind parameter, gọi package, paging, mapping DataTable/DTO", "DAL hiện hữu; Repository/PackageGateway đề xuất"),
        ("Integration", "Adapter giao tiếp ngoài, retry, rate limit, mapping schema", "EmailAdapter, AdsBAdapter, BravoAdapter, AmhsAdapter"),
        ("Cross-cutting", "Auth, RBAC, audit, log, config, result, error, metrics", "PageBase, shared service và middleware đề xuất"),
    ])
    heading(doc, "3.2. Thành phần lõi", 2)
    table(doc, ["Mã", "Thành phần", "Trách nhiệm chính", "Phụ thuộc"], [
        ("CMP-CORE-001", "FlightIdentityService", "Chuẩn hóa callsign, flight date, route, khóa match và phiên bản", "Oracle, BusinessDateService"),
        ("CMP-CORE-002", "BusinessDateService", "Quy đổi UTC/local, cross-midnight, ngày nghiệp vụ và ngưỡng bất thường", "Timezone configuration"),
        ("CMP-CORE-003", "PermissionService", "Tính quyền theo user-role-menu-action và phạm vi dữ liệu", "T_MENUS, user/role tables"),
        ("CMP-CORE-004", "AuditService", "Ghi before/after, actor, source, correlation ID và result", "Audit tables, log sink"),
        ("CMP-CORE-005", "JobOrchestrator", "Tạo batch, retry, lock, progress và hoàn thành job", "Queue, scheduler, Oracle"),
        ("CMP-CORE-006", "StandardResult", "Chuẩn hóa response success/error/warning/data/batch ID", "Tất cả service/API"),
    ])
    heading(doc, "3.3. Quy tắc phụ thuộc", 2)
    code_block(doc, "Presentation → Application → Domain → Data/Integration\nPresentation -X-> Oracle/package trực tiếp\nDomain -X-> UI control/session\nIntegration -X-> hiển thị; chỉ trả DTO/result\nAudit/Correlation phải được truyền xuyên suốt toàn bộ luồng")
    paragraph(doc, "Trong giai đoạn chuyển tiếp, các màn hình cũ vẫn có thể gọi DAL trực tiếp. Với chức năng sửa đổi mới, phải dùng service façade; các DAL cũ chỉ được bọc lại để giảm rủi ro thay đổi đồng loạt.")
    heading(doc, "3.4. Mẫu xử lý dùng chung", 2)
    table(doc, ["Mẫu", "Áp dụng", "Quy tắc"], [
        ("Query", "Tra cứu/báo cáo", "Filter DTO → bind parameter → page DTO → metadata snapshot"),
        ("Command", "Nhập/sửa/duyệt", "Validate → authorize → transaction → audit → result"),
        ("Integration job", "ADS-B/Bravo/AMHS", "Receive → normalize → idempotency → persist → acknowledge"),
        ("Batch command", "Export/Gen/Bulk edit", "Create batch → process chunks → progress → finalize/reconcile"),
        ("Notification", "Header/runtime warning", "Create event → target user/scope → unread/read → deep link"),
    ])
    page_break(doc)


def add_integration_design(doc):
    heading(doc, "4. Thiết kế phân hệ tích hợp và tự động hóa dữ liệu", 1)
    paragraph(doc, "Phân hệ tích hợp bao gồm FR-INT-001 đến FR-INT-005. Mỗi adapter phải độc lập với giao diện WebForms, có schema version, timeout, retry, quarantine và nhật ký correlation.")
    common_rows = [
        ("Receive", "Nhận payload/file và tạo ReceiveId", "Không ghi nghiệp vụ trước khi kiểm tra kích thước/hash"),
        ("Validate", "Kiểm tra schema, encoding, bắt buộc và timestamp", "Lỗi đưa vào quarantine với mã nguyên nhân"),
        ("Normalize", "Chuẩn hóa ngày giờ, mã sân bay, callsign, loại dữ liệu", "Lưu raw và normalized độc lập"),
        ("Match", "Ghép flight/permission/KHB theo khóa ưu tiên", "Nhiều ứng viên phải chuyển Chưa xác định"),
        ("Persist", "Ghi Oracle hoặc kho tích hợp", "Idempotency theo source key + version"),
        ("Publish", "Kích hoạt báo cáo/cảnh báo downstream", "Không publish nếu trạng thái chưa đạt"),
    ]
    for title, code, source, components in [
        ("4.1. FR-INT-001 – Email/File và AeroSync", "FR-INT-001", "Email, thư mục lưu trữ, AeroSync và Oracle ATFM", "IMAP/FileReceiver, Parser, Normalizer, OracleWriter"),
        ("4.2. FR-INT-002 – ADS-B O/F", "FR-INT-002", "Flight Tracking API/PostgreSQL tracks và T_TRACKS_LOG", "AdsBReceiver, TrackQuality, FlightMatcher, TracksRepository"),
        ("4.3. FR-INT-003 – SLOT/KHH", "FR-INT-003", "Excel KHH/SLOT và các bảng T_KHH/T_SLOT_AERO", "ExcelScanner, ImportStaging, SlotComparator, ResultRepository"),
        ("4.4. FR-INT-004 – AMHS/AFTN", "FR-INT-004", "AMHS connector, AFTN queue và Inbox/Outbox", "AmhsClient, MessageParser, MessageStore, DeliveryTracker"),
        ("4.5. FR-INT-005 – API Gateway", "FR-INT-005", "REST/JSON, OAuth/JWT, API key, scope và rate limit", "RouteRegistry, AuthFilter, RateLimiter, ApiAudit"),
    ]:
        heading(doc, title, 2)
        table(doc, ["Thuộc tính", "Thiết kế"], [
            ("Mã", code),
            ("Nguồn/đích", source),
            ("Thành phần", components),
            ("Kiểu xử lý", "Receive → Validate → Normalize → Match → Persist → Publish"),
            ("Chế độ lỗi", "Retry lỗi tạm thời; quarantine lỗi dữ liệu; alert khi quá ngưỡng"),
        ])
        table(doc, ["Bước", "Xử lý", "Dữ liệu/kiểm soát"], common_rows)
        paragraph(doc, "Thiết kế riêng cho " + code + ":")
        for item in [
            "Mỗi bản tin hoặc file phải có SourceId, SourceTimestamp, ReceivedAt, SchemaVersion và ContentHash.",
            "Không tạo bản ghi nghiệp vụ trùng khi nhận lại cùng SourceId/ContentHash; bản mới hơn phải tạo revision.",
            "Khi downstream chưa sẵn sàng, dữ liệu được giữ trong hàng chờ và có thể replay theo ReceiveId.",
            "Thông tin nhạy cảm không xuất hiện trong log mức INFO; payload đầy đủ chỉ lưu theo retention được phê duyệt.",
        ]:
            bullet(doc, item)
    heading(doc, "4.6. Thiết kế hàng chờ và retry", 2)
    table(doc, ["Trạng thái", "Ý nghĩa", "Chuyển tiếp"], [
        ("RECEIVED", "Đã tiếp nhận, chưa xử lý", "VALIDATING hoặc REJECTED"),
        ("VALIDATED", "Đã qua kiểm tra schema", "NORMALIZED hoặc QUARANTINED"),
        ("PROCESSING", "Đang ghi/match/publish", "COMPLETED hoặc FAILED"),
        ("RETRY_WAIT", "Lỗi tạm thời, chờ retry", "PROCESSING hoặc DEAD_LETTER"),
        ("QUARANTINED", "Lỗi dữ liệu cần xử lý thủ công", "REPLAY hoặc CLOSED"),
        ("COMPLETED", "Đã ghi và phát downstream", "ARCHIVED"),
    ])
    page_break(doc)


def add_message_design(doc):
    heading(doc, "5. Thiết kế cảnh báo, điện văn và phê duyệt", 1)
    heading(doc, "5.1. Kiến trúc cảnh báo runtime", 2)
    table(doc, ["Thành phần", "Trách nhiệm", "Nguồn"], [
        ("NotificationRuleEngine", "Đánh giá NOPERM, sai ngày, hết hạn, batch lỗi và bất thường", "Oracle/query service, integration events"),
        ("NotificationRepository", "Lưu notification, trạng thái đọc và phạm vi user/role", "T_NOTIFICATION/notification package"),
        ("NotificationPublisher", "Đưa thông báo vào header/dashboard hoặc kênh email", "Application event bus"),
        ("NotificationQuery", "Phân trang, lọc mức độ, mở deep link", "WebMethod/API"),
        ("NotificationAudit", "Ghi acknowledge, dismiss, open và escalation", "AuditService"),
    ])
    heading(doc, "5.2. Live Fire Message và Approve", 2)
    table(doc, ["Bước", "Tác nhân", "Kết quả"], [
        ("Create draft", "Người lập", "Tạo MessageId, nội dung, vùng trời, thời gian hiệu lực và trạng thái DRAFT"),
        ("Validate", "Application service", "Kiểm tra bắt buộc, xung đột thời gian, địa chỉ và format điện văn"),
        ("Submit", "Người lập", "Chuyển PENDING_APPROVAL và ghi audit"),
        ("Review", "Người duyệt", "Xem raw/normalized, lịch sử và dữ liệu liên quan"),
        ("Accept/Reject", "Người duyệt", "ACCEPTED hoặc REJECTED; bắt buộc lý do khi từ chối"),
        ("Export/Dispatch", "Hệ thống/người phát", "Sinh điện văn, gửi AMHS/AFTN, theo dõi delivery"),
        ("Close", "Hệ thống", "EXPIRED/CANCELLED khi hết hiệu lực hoặc bị hủy có quyền"),
    ])
    code_block(doc, "DRAFT → VALIDATED → PENDING_APPROVAL → ACCEPTED → QUEUED → SENDING\n                                               ├──────────────→ REJECTED\n                                               └──────────────→ CANCELLED\nSENDING → SENT → DELIVERED\nSENDING → FAILED → RETRY_WAIT → SENDING")
    heading(doc, "5.3. Thiết kế Message Management hiện hữu", 2)
    paragraph(doc, "Màn hình MessManagement.aspx hiện đang nạp dữ liệu qua DayFlightsDAL, nhận callback bằng chuỗi tham số và gọi MESSAGE_PKG/QlbOutBoxDAL trực tiếp. Thiết kế chuyển tiếp đề xuất giữ callback để tương thích nhưng đưa toàn bộ xử lý vào MessageApplicationService.")
    table(doc, ["Thành phần", "Thiết kế đề xuất", "Mã nguồn liên quan"], [
        ("MessagePageAdapter", "Chuyển callback/UI input thành Command DTO", "MessManagement.aspx.cs"),
        ("MessageApplicationService", "Kiểm tra quyền, validate, transaction và điều phối gửi", "prjBusinessLogic/Application"),
        ("DayFlightMessageRepository", "Tra cứu PART_NO, CONTENT, ADDRESS theo ngày/loại", "DayFlightsDAL.cs"),
        ("MessagePackageGateway", "Đóng gói gọi MESSAGE_PKG và chuẩn hóa result", "clsResuftAPI / Oracle package"),
        ("OutboxStatusRepository", "Quản lý QUEUED/SENDING/SENT/FAILED", "QlbOutBoxDAL.cs"),
        ("MessageAuditService", "Ghi actor, before/after, correlation và lỗi", "WriteLogHistory2Database"),
    ])
    heading(doc, "5.4. Quy tắc gửi an toàn", 2)
    for item in [
        "Không chuyển trạng thái SENT trước khi package/AMHS trả kết quả thành công.",
        "Mỗi lần gửi có MessageId và IdempotencyKey; gửi lại không tạo bản tin trùng.",
        "Ngày phải parse theo một định dạng nghiệp vụ cố định, không phụ thuộc CultureInfo của máy chủ.",
        "Kết quả trả về dùng StandardResult gồm Code, Message, Status, BatchId và Retryable.",
        "Gửi hàng loạt phải chạy nền; giao diện chỉ hiển thị progress và kết quả từng dòng.",
    ]:
        bullet(doc, item)
    heading(doc, "5.5. Thiết kế dữ liệu điện văn", 2)
    table(doc, ["Trường", "Kiểu đề xuất", "Quy tắc"], [
        ("MESSAGE_ID", "VARCHAR2/NUMBER", "Khóa nghiệp vụ duy nhất"),
        ("PART_NO", "VARCHAR2", "Mã nhóm điện văn/chuyến"),
        ("FLIGHT_DATE", "DATE", "Ngày nghiệp vụ sau BusinessDateService"),
        ("MESSAGE_TYPE", "VARCHAR2", "FPL/DEP/ARR/DLA/LiveFire/..."),
        ("CONTENT_HASH", "VARCHAR2(64)", "SHA-256 nội dung canonical"),
        ("STATUS", "VARCHAR2", "State machine ở mục 5.2"),
        ("CORRELATION_ID", "VARCHAR2", "Liên kết request, audit, package và delivery"),
        ("RETRY_COUNT/ERROR_CODE", "NUMBER/VARCHAR2", "Điều khiển retry và hiển thị lỗi an toàn"),
    ])
    page_break(doc)


def add_opt_design(doc):
    heading(doc, "6. Thiết kế phân hệ nâng cấp và tối ưu HTSLB", 1)
    paragraph(doc, "Phân hệ OPT gồm các thay đổi trên màn hình hiện hữu, Oracle package, job nền và cơ chế cảnh báo. Mỗi thay đổi phải có feature flag hoặc khả năng rollback khi cần.")
    groups = [
        ("6.1. Tra cứu và cảnh báo", "FR-OPT-001…013, FR-OPT-021, FR-OPT-024, FR-OPT-030", "SearchExtension, SearchPermissionAdv, Inbox, Permission lists, ListFlightOnMess, Notification"),
        ("6.2. Hiệu năng và batch", "FR-OPT-014…018", "ExportBravo, Calendar Accepted, Gen KHB/AFTN, Inbox archive, build/version"),
        ("6.3. Ngày bay và nội dung KHB", "FR-OPT-019, FR-OPT-020, FR-OPT-022, FR-OPT-023, FR-OPT-027", "BusinessDateService, FPL matcher, purpose catalog, cross-midnight"),
        ("6.4. Bravo và đối soát", "FR-OPT-026, FR-OPT-028, FR-OPT-029", "Bravo Adapter, batch edit, conflict queue, reconciliation snapshot"),
        ("6.5. Báo cáo mở rộng", "FR-OPT-031, FR-OPT-032", "Report metadata, preset, compare KHBHĐBN, anomaly rules"),
    ]
    for title, scope, components in groups:
        heading(doc, title, 2)
        table(doc, ["Phạm vi", "Mã SRS", "Thành phần thiết kế"], [(title, scope, components)])
        table(doc, ["Thiết kế", "Nội dung"], [
            ("Input", "Filter/command DTO có schema, kiểu, default, bắt buộc và giới hạn kích thước"),
            ("Processing", "Application service → domain rule → DAL/package hoặc adapter"),
            ("Concurrency", "Optimistic version, lock theo batch và chống double submit"),
            ("Output", "Paged result, KPI, progress, warning và deep link"),
            ("Failure", "Error code, retryable, giữ dữ liệu cũ không được coi là kết quả mới"),
        ])
    heading(doc, "6.6. Thiết kế batch ExportBravo và Gen KHB", 2)
    code_block(doc, "CreateBatch → ValidateScope → SnapshotSource → ProcessChunks → PersistResult\n          │             │                  │                │\n          └─ reject      └─ no data         └─ snapshot ID   └─ progress 0..100\nFinalize → Reconcile → PublishNotification → ArchiveBatch")
    table(doc, ["Chỉ tiêu", "Thiết kế đáp ứng"], [
        ("Thời gian", "Đo từng phase; cảnh báo khi vượt 80% SLA; mục tiêu ExportBravo/Gen KHB <180 giây"),
        ("Kích thước", "Chunk theo số dòng; không giữ toàn bộ DataTable trong một request"),
        ("An toàn", "IdempotencyKey theo ngày/batch; không chạy trùng khi job đang LOCKED"),
        ("Đối soát", "Đếm source, inserted, updated, skipped, failed và discrepancy"),
        ("Rollback", "Rollback package/script; dữ liệu batch có trạng thái và snapshot để chạy bù"),
    ])
    heading(doc, "6.7. Thiết kế tìm kiếm đa trường", 2)
    table(doc, ["Lớp", "Quy tắc"], [
        ("UI", "Hiển thị filter theo metadata, định dạng ngày/giờ thống nhất, giữ filter khi phân trang"),
        ("API", "Whitelist field/operator; bind parameter; trả total, page, pageSize, sort"),
        ("Oracle", "Predicate động an toàn, index theo PERMDATE/FLIGHTDATE/PERM_ID và execution plan"),
        ("Detail", "Mở View_PermSC/View_PermNo theo ID đã kiểm tra quyền; không truyền SQL/raw input"),
    ])
    page_break(doc)


def add_report_design(doc):
    heading(doc, "7. Thiết kế phân hệ báo cáo và phân tích", 1)
    heading(doc, "7.1. Mô hình truy vấn báo cáo", 2)
    code_block(doc, "ReportFilterDTO → ReportQueryService → SnapshotProvider → Oracle Query\n                                      ├─ KPI Calculator\n                                      ├─ Chart Dataset\n                                      ├─ Drill-down Query\n                                      └─ Export Worker")
    table(doc, ["Thành phần", "Thiết kế"], [
        ("ReportMetadata", "Khai báo field, operator, type, danh mục, default, dependency và column visibility"),
        ("ReportFilterValidator", "Kiểm tra range ngày, timezone, quyền dữ liệu và giới hạn kết quả"),
        ("SnapshotProvider", "Chọn T_DAY_FLIGHTS/T_FINISHED_FLIGHTS/T_TRACKS_LOG theo kỳ và lưu SnapshotId"),
        ("KpiCalculator", "Tính tổng, tỷ lệ, delay, airport count và các chỉ số ADS-B"),
        ("DrillDownService", "Dùng cùng filter/snapshot với KPI, tránh lệch tổng và chi tiết"),
        ("ExportWorker", "Sinh Excel/CSV toàn tập kết quả; ghi filter, nguồn và thời điểm"),
    ])
    heading(doc, "7.2. Danh mục báo cáo", 2)
    table(doc, ["Nhóm", "Màn hình đại diện", "Nguồn dữ liệu"], [
        ("Tổng quan", "FlightOperationOverview.aspx, FlightStatusRate.aspx", "T_DAY_FLIGHTS_GOINGON/T_FINISHED_FLIGHTS"),
        ("Xu hướng/bất thường", "FlightTrendAnalysis.aspx, AnomalyWarning.aspx", "Finished flights, notification/anomaly store"),
        ("ADS-B", "AdsBPerformanceReport.aspx, bản đồ tracking", "T_TRACKS_LOG, ADS-B snapshot, KHB/FPL"),
        ("Sân bay", "ChartReportAirport.aspx, CivilFlightSummary.aspx", "Airport dimension, finished flights"),
        ("Quân sự", "MilitaryFlightReport.aspx, Daily Military Report", "Military KHB/finished flights Accepted"),
        ("KHBHĐBN", "Report metadata/compare", "KHB version snapshots và business date"),
    ])
    heading(doc, "7.3. Quy tắc nhất quán số liệu", 2)
    for item in [
        "KPI, biểu đồ, bảng và drill-down trong cùng một lần xem phải dùng cùng SnapshotId.",
        "Tổng chuyến phải đối soát được với tổng các dòng chi tiết sau khi áp dụng cùng filter.",
        "Dữ liệu thiếu hoặc nguồn trễ phải hiển thị trạng thái nguồn; không dùng âm thầm snapshot cũ.",
        "Export phải ghi bộ lọc, thời điểm sinh, nguồn dữ liệu, phiên bản công thức và user thực hiện.",
        "Báo cáo chỉ đọc; mọi thao tác sửa phải chuyển về màn hình nghiệp vụ có phân quyền riêng.",
    ]:
        bullet(doc, item)
    heading(doc, "7.4. Thiết kế hiệu năng báo cáo", 2)
    table(doc, ["Rủi ro", "Biện pháp"], [
        ("Khoảng ngày lớn", "Giới hạn range, phân trang DB, chạy nền và cảnh báo khi query quá ngưỡng"),
        ("Nhiều filter", "Metadata whitelist, bind parameter, index theo nhóm phổ biến"),
        ("Drill-down lặp", "Cache theo SnapshotId + FilterHash + Page"),
        ("Export lớn", "Worker streaming, file tạm có checksum và tự hết hạn"),
        ("ADS-B raw lớn", "Tổng hợp theo ngày/airport/FIR, chỉ truy raw khi drill-down"),
    ])
    heading(doc, "7.5. Thiết kế cảnh báo bất thường", 2)
    table(doc, ["Bước", "Thành phần", "Đầu ra"], [
        ("Collect", "AnomalyDataProvider", "Metric theo kỳ và dimension"),
        ("Evaluate", "Rule/Threshold Engine", "NORMAL/WARN/CRITICAL"),
        ("Correlate", "Flight/Message/Batch context", "Danh sách chuyến/bản ghi liên quan"),
        ("Notify", "NotificationPublisher", "Header badge, deep link, mức độ"),
        ("Resolve", "User action + audit", "Acknowledged/Resolved/False positive"),
    ])
    page_break(doc)


def add_ai_design(doc):
    heading(doc, "8. Thiết kế phân hệ AI hỗ trợ thống kê, tìm kiếm và tổng hợp", 1)
    paragraph(doc, "AI được thiết kế như một dịch vụ độc lập, không nhúng trực tiếp runtime Python/LLM vào tiến trình ASP.NET. ATFM_WEB chỉ gọi API theo session, scope và timeout.")
    heading(doc, "8.1. Thành phần AI", 2)
    table(doc, ["Thành phần", "Vai trò", "Bảo vệ"], [
        ("Chat API", "Nhận câu hỏi, session, user context và trả kết quả", "HTTPS, CSRF/session token, rate limit"),
        ("Intent/Context Builder", "Nhận diện ngày, hãng, sân bay, loại chuyến và schema", "Không cho LLM tự chọn bảng ngoài allowlist"),
        ("Prompt/Training Store", "Quản lý system prompt, mẫu câu hỏi và version", "Review workflow, audit, không chứa secret"),
        ("SQL Guard", "Parse Oracle SQL, chặn DML/DDL/multi-statement", "Allowlist bảng/cột, row limit, timeout"),
        ("OracleRunner", "Thực thi SELECT bằng read-only account", "Bind parameter, max rows, circuit breaker"),
        ("Result Renderer", "Chuẩn hóa JSON/table/chart và giải thích kết quả", "Che dữ liệu nhạy cảm theo scope"),
    ])
    heading(doc, "8.2. Luồng xử lý NL2SQL", 2)
    code_block(doc, "Question → Session/Auth → Intent → Schema Context → LLM Draft SQL\n    → SQL Parser/Allowlist → User Preview (nếu rủi ro) → OracleRunner\n    → Result Normalizer → Chart/Table → Audit + Feedback")
    heading(doc, "8.3. Quy tắc SQL bắt buộc", 2)
    for item in [
        "Chỉ cho phép một truy vấn SELECT hợp lệ theo dialect Oracle; cấm DML, DDL, transaction, lock và INTO.",
        "Tên bảng/cột phải thuộc metadata allowlist; không cho phép SELECT * trong truy vấn sản xuất.",
        "Tham số ngày, mã sân bay, hãng và trạng thái phải bind parameter; không ghép chuỗi trực tiếp.",
        "Giới hạn số dòng, thời gian chạy và số lần retry; kết quả quá lớn phải chuyển sang export nền.",
        "Lưu SQL, schema version, model, prompt version và user trong metadata nhưng che secret.",
    ]:
        bullet(doc, item)
    heading(doc, "8.4. API AI đề xuất", 2)
    table(doc, ["Endpoint", "Method", "Mục đích", "Kết quả"], [
        ("/api/ai/session", "POST", "Tạo phiên hội thoại", "SessionId, expiry, scope"),
        ("/api/ai/query/preview", "POST", "Sinh SQL và kiểm tra an toàn", "Intent, SQL masked, warnings"),
        ("/api/ai/query/execute", "POST", "Thực thi truy vấn đã duyệt", "Columns, rows, rowCount, snapshot"),
        ("/api/ai/feedback", "POST", "Gửi phản hồi/đề xuất huấn luyện", "FeedbackId, review status"),
        ("/api/ai/admin/training", "GET/POST", "Quản lý mẫu đã duyệt", "Version, approval state"),
    ])
    page_break(doc)


def add_data_design(doc):
    heading(doc, "9. Thiết kế dữ liệu và mô hình lưu trữ", 1)
    heading(doc, "9.1. Nguyên tắc dữ liệu", 2)
    table(doc, ["Nguyên tắc", "Thiết kế"], [
        ("Nguồn chuẩn", "Oracle ATFM lưu master/detail nghiệp vụ; kho phụ chỉ lưu dữ liệu tích hợp hoặc cache"),
        ("Định danh", "FlightId/PermissionId/MessageId ổn định; không match chỉ bằng ngày hoặc callsign"),
        ("Phiên bản", "Raw, normalized, accepted, build và source revision phải phân biệt"),
        ("Ngày giờ", "Lưu UTC/source timezone và BusinessDate; không cộng ngày ở nhiều tầng"),
        ("Truy vết vận hành", "BatchId, CorrelationId, SourceId và audit trước–sau"),
        ("Archive", "Current/history tách theo retention; query phải chỉ rõ phạm vi"),
    ])
    heading(doc, "9.2. Nhóm bảng chính", 2)
    table(doc, ["Nhóm", "Bảng đại diện", "Quan hệ"], [
        ("Permission", "T_PERMMASTER_SC/NO, T_PERMDETAIL_SC/NO", "Permission → nhiều flight segment/detail"),
        ("Daily flight", "T_DAY_FLIGHTS, T_DAY_FLIGHTS_GOINGON", "Flight identity, KHB, actual event"),
        ("Finished/Military", "T_FINISHED_FLIGHTS, T_FINISHFLIGHTS_MILITARY", "Finished/Accepted snapshot"),
        ("Message", "T_PLAN_MESSAGE, Inbox/Outbox, message log", "Message → flight/part/address/status"),
        ("ADS-B", "T_TRACKS_LOG, quality/match metadata", "Track batch → flight/snapshot"),
        ("Report", "Snapshot, aggregate, export metadata", "Snapshot → filter/hash → report result"),
        ("Admin/Audit", "T_MENUS, user/role, audit/error/notification", "Actor → action → object → result"),
    ])
    heading(doc, "9.3. Từ điển dữ liệu tối thiểu", 2)
    table(doc, ["Đối tượng", "Trường thiết kế", "Quy tắc"], [
        ("Flight", "FLIGHT_ID, CALLSIGN, FLIGHTDATE, FROM_AIRP, TO_AIRP", "Khóa match và ngày nghiệp vụ phải ổn định"),
        ("Permission", "PERM_ID, PERM_NUMBER, PERMDATE, ENDDATE, VALIDHOURS", "Phân biệt SC/NO và trạng thái hiệu lực"),
        ("Message", "MESSAGE_ID, PART_NO, MESS_TYPE, CONTENT_HASH, STATUS", "Canonical content và state machine"),
        ("Batch", "BATCH_ID, SOURCE, STARTED_AT, FINISHED_AT, STATUS", "Idempotency, progress, reconciliation"),
        ("Notification", "NOTIFICATION_ID, USER_ID/SCOPE, LEVEL, READ_AT, DEEP_LINK", "Phân phối theo quyền dữ liệu"),
        ("Audit", "ACTOR, ACTION, OBJECT_TYPE/ID, BEFORE, AFTER, RESULT", "Che secret và giữ correlation"),
    ])
    heading(doc, "9.4. Index và phân vùng dữ liệu", 2)
    table(doc, ["Khu vực", "Index/partition đề xuất", "Mục đích"], [
        ("Flight", "FLIGHTDATE, FLIGHT_ID, CALLSIGN, FROM/TO", "Tra cứu và match chuyến"),
        ("Permission", "PERM_ID, PERMDATE/ENDDATE, PERM_NUMBER", "Danh sách phép và hết hạn"),
        ("Message", "FLIGHTDATE, MESS_TYPE, PART_NO, STATUS", "Inbox/Outbox/Message Management"),
        ("ADS-B", "TRACK_TIME, FLIGHT_ID, FIR, quality", "Báo cáo và truy vết vệt bay"),
        ("Audit", "CREATED_AT, ACTOR, OBJECT_ID", "Truy vấn vận hành theo thời gian"),
    ])
    heading(doc, "9.5. Transaction và đồng thời", 2)
    paragraph(doc, "Một transaction Oracle chỉ bao phủ dữ liệu trong cùng schema. Với API ngoài, hệ thống ghi trạng thái và outbox trước, thực hiện gọi ngoài sau đó cập nhật kết quả. Lost update được ngăn bằng version/timestamp; batch edit phải kiểm tra lại giá trị sau preview.")
    page_break(doc)


def add_api_design(doc):
    heading(doc, "10. Thiết kế API, package và hợp đồng tích hợp", 1)
    heading(doc, "10.1. Chuẩn response dùng chung", 2)
    code_block(doc, "{\n  \"success\": true,\n  \"code\": \"OK\",\n  \"message\": \"...\",\n  \"correlationId\": \"...\",\n  \"batchId\": \"...\",\n  \"data\": {},\n  \"warnings\": [],\n  \"retryable\": false\n}")
    table(doc, ["Mã", "Ý nghĩa", "HTTP/ứng xử"], [
        ("OK", "Thành công", "200"),
        ("VALIDATION_ERROR", "Dữ liệu đầu vào không hợp lệ", "400, hiển thị trường lỗi"),
        ("FORBIDDEN", "Không đủ quyền", "403, không tiết lộ dữ liệu"),
        ("NOT_FOUND", "Không tìm thấy bản ghi", "404"),
        ("CONFLICT", "Version/idempotency/state conflict", "409, yêu cầu tải lại"),
        ("UPSTREAM_TIMEOUT", "Hệ thống ngoài timeout", "502/504, retryable"),
        ("DB_ERROR", "Oracle/package lỗi", "500, log correlation"),
        ("BATCH_ACCEPTED", "Đã nhận xử lý nền", "202, trả BatchId"),
    ])
    heading(doc, "10.2. API nội bộ đề xuất", 2)
    table(doc, ["Nhóm", "Endpoint/service", "Mục đích"], [
        ("Flight", "GET /api/flights; GET /api/flights/{id}", "Tra cứu flight, detail, status và version"),
        ("Message", "POST /api/messages/send; GET /api/messages/{id}", "Gửi một/batch điện văn và theo dõi trạng thái"),
        ("Notification", "GET /api/notifications; POST /read", "Header badge và deep link"),
        ("Report", "POST /api/reports/{code}/query; /export", "Query snapshot và export nền"),
        ("Integration", "POST /api/integration/{source}/replay", "Replay quarantined batch theo quyền"),
        ("Job", "GET /api/jobs/{batchId}", "Progress, lỗi, kết quả và reconciliation"),
    ])
    heading(doc, "10.3. Package Gateway", 2)
    table(doc, ["Gateway", "Quy tắc"], [
        ("Package name", "Một gateway cho mỗi nhóm package, không rải chuỗi tên package trong UI"),
        ("Parameters", "Bind parameter, kiểu dữ liệu rõ, DATE/TIMESTAMP không parse ngầm"),
        ("Result", "Chuẩn hóa return code, out parameter, cursor/DataTable thành DTO"),
        ("Timeout", "Cấu hình theo thao tác; không dùng timeout vô hạn"),
        ("Logging", "Log package, procedure, elapsed và correlation; không log credential/content nhạy cảm"),
        ("Rollback", "Script deploy/rollback đi cùng thay đổi package/index"),
    ])
    heading(doc, "10.4. Hợp đồng Bravo", 2)
    table(doc, ["Trường", "Chiều", "Quy tắc"], [
        ("idempotencyKey", "ATFM → Bravo", "Ổn định theo batch + flight + version"),
        ("flightIdentity", "Hai chiều", "CallsSign/flight date/route theo mapping đã phê duyệt"),
        ("actualTime", "ATFM → Bravo", "KEEP ORIGINAL nếu manual override"),
        ("purpose", "ATFM → Bravo", "Chỉ update trường được phép"),
        ("result/conflict", "Bravo → ATFM", "INSERT/UPDATE/NO_CHANGE/CONFLICT/ERROR"),
    ])
    page_break(doc)


def add_ui_workflow(doc):
    heading(doc, "11. Thiết kế giao diện và luồng nghiệp vụ", 1)
    heading(doc, "11.1. Quy chuẩn giao diện", 2)
    table(doc, ["Khu vực", "Quy chuẩn"], [
        ("Filter", "Label rõ, định dạng ngày/giờ thống nhất, nút Reset, giữ điều kiện khi chuyển trang"),
        ("Grid", "Cột cố định theo nghiệp vụ, cuộn ngang có kiểm soát, tổng số dòng và trạng thái tải"),
        ("Command", "Nút theo quyền; confirm cho thao tác ghi/xóa/duyệt/phát đi"),
        ("Error", "Thông báo thân thiện; có correlation để hỗ trợ; không hiển thị stack trace"),
        ("Runtime", "Header notification có badge, level, timestamp và deep link"),
        ("Export", "Hiển thị batch progress, file expiry và tiêu chí đã sử dụng"),
    ])
    heading(doc, "11.2. Luồng tra cứu điện văn", 2)
    code_block(doc, "Chọn ngày/loại → Validate filter → Query package/DAL → Bind grid\n     ├─ View content → lấy CONTENT theo PartNo + MessType\n     ├─ View address → lấy danh sách địa chỉ theo quyền\n     └─ Send → Command service → Outbox → Message adapter → Status")
    heading(doc, "11.3. Luồng Daily Statistic", 2)
    table(doc, ["Bước", "Màn hình/thành phần", "Thiết kế"], [
        ("Export Flight Finished", "DaylyFlight.aspx", "Chọn ngày → snapshot flight → tạo batch finished"),
        ("Daily Statistic", "ListFinishedFlights.aspx", "Query theo batch/ngày, lọc, phân trang, chỉ đọc/sửa theo quyền"),
        ("Accept", "ListFinishedFlightAccepts.aspx", "Review, accept/reject, lý do, audit và khóa trạng thái"),
        ("Report", "Daily report/export", "Chỉ lấy bản ghi Accepted, hiển thị nguồn và thời điểm snapshot"),
    ])
    heading(doc, "11.4. Luồng KHB quân sự", 2)
    code_block(doc, "Nhập mới KHB → Validate/Save Draft → Submit → Accepted/Rejected\n       → Export Message → AMHS/AFTN → Delivery status → Military Report (read-only)")
    heading(doc, "11.5. Phân quyền màn hình", 2)
    table(doc, ["Vai trò", "Xem", "Nhập/Sửa", "Duyệt", "Phát đi", "Export"], [
        ("Khai thác viên", "Theo đơn vị", "Theo nhiệm vụ", "Không mặc định", "Không mặc định", "Theo quyền"),
        ("Người duyệt", "Theo phạm vi", "Điều chỉnh được phép", "Có", "Theo nghiệp vụ", "Có"),
        ("Quản lý/báo cáo", "Báo cáo được cấp", "Không mặc định", "Theo quy trình", "Không", "Có"),
        ("Quản trị ứng dụng", "Có kiểm soát", "Danh mục/cấu hình", "Không thay nghiệp vụ", "Không", "Theo quyền"),
        ("API client", "Theo scope", "Theo endpoint", "Không mặc định", "Theo scope", "Không mặc định"),
    ])
    heading(doc, "11.6. Quy tắc callback WebForms", 2)
    paragraph(doc, "Callback hiện hữu phải được giới hạn bằng command allowlist. Tham số callback được parse vào DTO typed, không dùng chuỗi delimiter làm hợp đồng dài hạn. Mỗi command phải gọi PermissionService trước khi gọi DAL/package; kết quả trả JSON chuẩn để JavaScript xử lý.")
    page_break(doc)


def add_security_nfr(doc):
    heading(doc, "12. Thiết kế bảo mật và yêu cầu phi chức năng", 1)
    heading(doc, "12.1. Xác thực và phiên", 2)
    table(doc, ["Mã", "Thiết kế"], [
        ("SEC-AUTH-001", "Mọi page/API kiểm tra authenticated session; hết phiên trả về login/401, không dùng session cũ"),
        ("SEC-AUTH-002", "Cookie Secure/HttpOnly/SameSite phù hợp; logout hủy session phía server"),
        ("SEC-AUTH-003", "CSRF token cho command POST/callback; không tin user ID từ client"),
        ("SEC-AUTH-004", "Service/API credential lưu trong secret store hoặc cấu hình bảo vệ, không commit .env"),
    ])
    heading(doc, "12.2. RBAC và phạm vi dữ liệu", 2)
    table(doc, ["Lớp kiểm tra", "Nội dung"], [
        ("Menu", "User có quyền truy cập Menu_ID/page hay không"),
        ("Action", "Xem/Thêm/Sửa/Xóa/Duyệt/Export/Phát đi"),
        ("Data scope", "Đơn vị, sân bay, vùng trời, loại chuyến và phân loại quân sự/dân dụng"),
        ("Object state", "Không sửa bản ghi Accepted/Sent/Locked nếu không có quyền đặc biệt"),
        ("API scope", "Client, route, method, rate limit và IP bypass"),
    ])
    heading(doc, "12.3. NFR đo lường", 2)
    table(doc, ["Mã", "Chỉ tiêu", "Cách đo"], [
        ("NFR-PERF-001", "ExportBravo <180 giây cho phạm vi đại diện", "Batch timer từ snapshot đến reconcile; tối thiểu 3 lần"),
        ("NFR-PERF-002", "Gen KHB D+1 ra AFTN <180 giây", "Đo receive → message ready; ghi phase timing"),
        ("NFR-PERF-003", "Tra cứu trang đầu ≤3 giây ở tải chuẩn", "Server elapsed + DB elapsed + payload size"),
        ("NFR-AVAIL-001", "Availability theo giờ vận hành được phê duyệt", "Monitor uptime và incident"),
        ("NFR-DATA-001", "Không duplicate khi retry/replay", "So sánh source key, batch và target count"),
        ("NFR-SEC-001", "Không phát hiện secret/SQL nhạy cảm trong log", "Log review và secret scan"),
        ("NFR-OPS-001", "Backup/restore đạt RPO/RTO đã phê duyệt", "Diễn tập restore theo runbook"),
    ])
    heading(doc, "12.4. Logging và audit", 2)
    table(doc, ["Event", "Trường tối thiểu"], [
        ("User command", "Actor, role, menu, action, object, before/after, result, IP, correlation"),
        ("Integration", "Source, receive/batch ID, schema, hash, elapsed, retry, upstream status"),
        ("Job", "Job type, scope, progress, start/end, counts, error summary"),
        ("Security", "Login/logout/failure, permission denied, token/client, source IP"),
    ])
    page_break(doc)


def add_operations(doc):
    heading(doc, "13. Thiết kế triển khai, giám sát và phục hồi", 1)
    heading(doc, "13.1. Môi trường", 2)
    table(doc, ["Môi trường", "Mục đích", "Dữ liệu/cấu hình"], [
        ("DEV", "Phát triển và unit/integration local", "Dữ liệu giả lập/masked, endpoint sandbox"),
        ("TEST/UAT", "Tích hợp và xác nhận nghiệp vụ", "Snapshot đại diện, endpoint test, tài khoản theo vai trò"),
        ("PROD", "Khai thác chính thức", "Secret riêng, dữ liệu thật, change approval và monitoring"),
    ])
    heading(doc, "13.2. Gói phát hành", 2)
    table(doc, ["Thành phần", "Yêu cầu bàn giao"], [
        ("Web application", "PublishOutput, version, checksum, web.config mẫu và rollback package"),
        ("Business/DAL", "DLL, dependency, binding, cấu hình timeout và package map"),
        ("Oracle", "Deploy/rollback script, precheck/postcheck, thứ tự chạy và quyền schema"),
        ("Integration worker", "Service binary/container, endpoint, retry/queue configuration"),
        ("AI", "Python package lock, model/config, SQL allowlist, read-only DSN"),
        ("Documentation", "SDD, API/OpenAPI, data dictionary, runbook và release note"),
    ])
    heading(doc, "13.3. Giám sát", 2)
    table(doc, ["Đối tượng", "Metric/log", "Cảnh báo"], [
        ("IIS/Web", "Request count, error rate, latency, worker health", "5xx tăng, pool recycle, latency vượt ngưỡng"),
        ("Oracle", "Session, CPU, IO, lock, slow query", "Timeout, deadlock, tablespace, plan regress"),
        ("Integration", "Received, processed, failed, retry, quarantine", "Không nhận batch, lỗi liên tiếp, queue tăng"),
        ("Jobs", "Duration, progress, throughput, result count", "Vượt SLA, stuck, duplicate lock"),
        ("External", "AMHS/Bravo/ADS-B availability, response code", "Timeout, auth failure, schema mismatch"),
    ])
    heading(doc, "13.4. Backup và phục hồi", 2)
    for item in [
        "Oracle backup theo chính sách DBA; backup ngoài máy chủ chính và kiểm tra khả năng đọc file.",
        "Cấu hình endpoint/secret lưu riêng theo môi trường; không khôi phục secret DEV vào PROD.",
        "Restore phải có runbook, owner, thời gian bắt đầu/kết thúc và kết quả đối soát mẫu.",
        "Thay đổi package/index phải có rollback hoặc phương án bù dữ liệu đã được xem xét.",
    ]:
        bullet(doc, item)
    heading(doc, "13.5. Xử lý sự cố", 2)
    table(doc, ["Sự cố", "Phân tích nhanh", "Khôi phục"], [
        ("API timeout", "Kiểm tra endpoint, DNS, TLS, correlation và retry", "Tạm dừng batch, retry có giới hạn, chuyển queue"),
        ("Oracle chậm", "Slow query/lock/session/plan", "Giảm scope, kill session theo quy trình DBA, chạy bù"),
        ("Message gửi lỗi", "Outbox state, AMHS response, payload hash", "Giữ FAILED, retry/replay, không đánh dấu SENT"),
        ("Sai ngày bay", "Nguồn timestamp, timezone, rule revision", "Đóng cảnh báo, sửa có quyền, tính lại downstream"),
        ("Report lệch KPI", "Snapshot/filter hash và query version", "Khóa snapshot, rebuild aggregate, ghi chú nguồn"),
    ])
    page_break(doc)


def add_implementation(doc):
    heading(doc, "14. Lộ trình triển khai và chuyển đổi", 1)
    heading(doc, "14.1. Giai đoạn thực hiện", 2)
    table(doc, ["Giai đoạn", "Phạm vi", "Điều kiện chuyển bước"], [
        ("P0 – Chuẩn hóa", "Chốt SDD, naming, schema, API contract, môi trường và quyền", "Được phê duyệt baseline thiết kế"),
        ("P1 – Cross-cutting", "StandardResult, PermissionService, AuditService, BusinessDateService, logging", "Màn hình mẫu dùng thành công"),
        ("P2 – Integration", "Email/ADS-B/SLOT/AMHS/Bravo adapters và queue", "Replay/idempotency/monitoring hoạt động"),
        ("P3 – Core/ALT/OPT", "Message, KHB, cảnh báo, batch và đối soát", "Luồng nghiệp vụ end-to-end ổn định"),
        ("P4 – Report/AI", "Snapshot report, export nền, AI read-only", "Nguồn dữ liệu và scope được phê duyệt"),
        ("P5 – Cutover", "Triển khai production, migration, vận hành song song", "Runbook/rollback/owner sẵn sàng"),
    ])
    heading(doc, "14.2. Chiến lược tương thích mã nguồn", 2)
    table(doc, ["Khu vực hiện hữu", "Cách chuyển đổi"], [
        ("ASPX/code-behind", "Giữ event/callback công khai; gọi service façade mới"),
        ("DAL", "Giữ method cũ, thêm Repository/PackageGateway cho chức năng mới"),
        ("Oracle package", "Thêm procedure versioned; deploy/rollback tách biệt"),
        ("JavaScript", "Giữ contract response cũ khi cần; thêm schema version và StandardResult"),
        ("Export", "Giữ định dạng file hiện hành; bổ sung metadata/filter/batch status"),
    ])
    heading(doc, "14.3. Chuyển đổi dữ liệu", 2)
    table(doc, ["Bước", "Nội dung"], [
        ("Inventory", "Liệt kê bảng/package/index hiện có, dữ liệu current/history và owner"),
        ("Profile", "Đo NULL, duplicate, sai ngày, orphan, mã sân bay/hãng không chuẩn"),
        ("Backfill", "Tạo snapshot/version/correlation cho dữ liệu cần đối soát"),
        ("Dual read", "So sánh query mới với query cũ theo phạm vi nhỏ"),
        ("Cutover", "Chuyển menu/job theo feature flag, theo dõi và có rollback"),
        ("Cleanup", "Đóng luồng cũ sau thời gian vận hành ổn định và phê duyệt"),
    ])
    heading(doc, "14.4. Rủi ro thiết kế", 2)
    table(doc, ["Rủi ro", "Mức", "Biện pháp"], [
        ("Package hiện hữu có side effect không ghi trong tài liệu", "Cao", "Đọc source/package, chạy sandbox, snapshot trước/sau"),
        ("Ngày/múi giờ không nhất quán", "Cao", "BusinessDateService, test dữ liệu qua ngày và lưu source timezone"),
        ("Tích hợp ngoài thay đổi schema", "Cao", "Schema version, contract validation, quarantine và adapter"),
        ("Export/job vượt SLA", "Trung bình", "Batch, chunk, index, execution plan và monitor phase"),
        ("AI sinh SQL không an toàn", "Cao", "Read-only account, parser, allowlist, preview và row limit"),
    ])
    page_break(doc)


def add_traceability_appendix(doc):
    heading(doc, "15. Ánh xạ yêu cầu sang thiết kế", 1)
    paragraph(doc, "Bảng dưới đây dùng để đảm bảo mỗi nhóm yêu cầu trong SRS có nơi triển khai trong thiết kế. Đây là bảng quản lý phạm vi thiết kế, không thay thế tài liệu yêu cầu hoặc tài liệu vận hành.")
    table(doc, ["Nhóm SRS", "Phân hệ thiết kế", "Thành phần chính", "Dữ liệu/tích hợp"], [
        ("FR-INT-001", "AeroSync/Email/File", "Receiver, Parser, Normalizer, OracleWriter", "Inbox, T_PERM*, source archive"),
        ("FR-INT-002", "ADS-B", "AdsBReceiver, Quality, Matcher, TracksSync", "PostgreSQL tracks, T_TRACKS_LOG"),
        ("FR-INT-003", "SLOT", "ExcelImporter, Comparator, ResultRepository", "T_SLOT_AERO, T_KHH, KQ1–KQ4"),
        ("FR-INT-004", "AMHS/AFTN", "AmhsClient, MessageParser, DeliveryTracker", "Inbox/Outbox, T_PLAN_MESSAGE"),
        ("FR-INT-005", "API Gateway", "RouteRegistry, Auth, RateLimiter", "API client/scope/config/audit"),
        ("FR-ALT-001/002", "Alert and Message", "Notification, LiveFire, MilitaryKHB", "Notification, message, KHB military"),
        ("FR-OPT-001…032", "HTSLB Optimization", "Search, Batch, BusinessDate, Bravo, Reconcile", "Permission, flight, message, batch"),
        ("FR-RPT-001…010", "Reporting", "Metadata, Snapshot, KPI, DrillDown, Export", "Finished, ADS-B, airport, military"),
        ("FR-AI-001…004", "AI Service", "NL2SQL, SQLGuard, OracleRunner, Chat API", "Oracle read-only, training/session store"),
        ("COM/DATA/INT/OPS", "Shared platform", "Auth, RBAC, Audit, Config, Job, Monitor", "User/role, audit, job, error, metrics"),
    ])
    heading(doc, "15.1. Danh mục sơ đồ cần tạo khi thiết kế chi tiết", 2)
    table(doc, ["Mã sơ đồ", "Tên sơ đồ", "Phạm vi"], [
        ("DIA-01", "Context Diagram", "Người dùng, ATFM_WEB, Oracle và hệ thống ngoài"),
        ("DIA-02", "Deployment Diagram", "DMZ/Application/Integration/Data"),
        ("DIA-03", "Component Diagram", "WebForms, Business, DAL, adapters, workers"),
        ("DIA-04", "Sequence – Message Send", "Create/validate/outbox/send/status"),
        ("DIA-05", "Sequence – ADS-B Sync", "Receive/normalize/match/persist/report"),
        ("DIA-06", "Sequence – Bravo Sync", "Snapshot/payload/conflict/reconcile"),
        ("DIA-07", "State – KHB/Message", "Draft/Accepted/Sent/Failed/Expired"),
        ("DIA-08", "ERD nghiệp vụ", "Flight, permission, message, notification, audit"),
        ("DIA-09", "Data Flow – Report", "Filter/snapshot/KPI/drill-down/export"),
        ("DIA-10", "AI Guard Flow", "Question/SQL guard/read-only Oracle/result"),
    ])
    heading(doc, "15.2. Danh mục quyết định cần phê duyệt", 2)
    table(doc, ["Quyết định", "Chủ sở hữu đề xuất", "Thời điểm"], [
        ("Tên vật lý bảng/package mới", "DBA + kiến trúc sư", "Trước thiết kế LLD"),
        ("Endpoint/schema Bravo và AMHS", "Chủ hệ thống ngoài", "Trước tích hợp TEST"),
        ("Timezone/ngày nghiệp vụ", "Nghiệp vụ + kiến trúc sư", "Trước xử lý FR-OPT-023/027"),
        ("SLA ExportBravo/Gen KHB", "Vận hành + nghiệp vụ", "Trước cấu hình monitor"),
        ("Retention raw message/ADS-B/audit", "ATTT + DBA", "Trước production"),
        ("Scope AI và dữ liệu cho phép", "Nghiệp vụ + ATTT", "Trước mở Chatbot"),
    ])
    page_break(doc)


def add_appendices(doc):
    heading(doc, "16. Phụ lục thiết kế", 1)
    heading(doc, "16.1. Danh mục màn hình chính", 2)
    table(doc, ["Nhóm", "Màn hình/URL đại diện", "Vai trò"], [
        ("Điện văn", "MessManagement.aspx; Inbox.aspx; ShowMessageFullContent.aspx", "Khai thác, duyệt, phát điện"),
        ("Live Fire", "LiveFireMessage.aspx; LiveFireMessageAccepted.aspx", "Người lập/người duyệt"),
        ("KHB", "DaylyFlight.aspx; ListFlightOnMess.aspx; Military Report", "Kế hoạch, theo dõi, báo cáo"),
        ("Phép bay", "ListPermissionSC/NO; SearchExtension; SearchPermissionAdv", "Tra cứu/quản lý phép"),
        ("Báo cáo", "ReportNew/*; Common/ChartReport*", "Quản lý, báo cáo, khai thác"),
        ("Công cụ", "ImportsPermSC_LD_V2.aspx; ADS-B/SLOT/API tools", "Tích hợp/quản trị"),
    ])
    heading(doc, "16.2. Danh mục package và service", 2)
    table(doc, ["Nhóm", "Thành phần đại diện", "Mục đích"], [
        ("Message", "MESSAGE_PKG, MESSAGE_FLIGHT_PKG, P_FLY", "Tạo/gửi/tra cứu/đối soát điện văn"),
        ("Permission", "A_TEST_SEARCH, PERM_IMP_V2_PKG", "Tra cứu/import/hủy phép"),
        ("Bravo", "BRAVO_EXPORT_PKG, Bravo Adapter", "Export/sync/reconcile"),
        ("Notification", "NOTIFICATION_PKG, Notification service", "Cảnh báo/header/read state"),
        ("Gateway", "REST routes, OAuth/JWT, API key", "Chia sẻ dữ liệu và kiểm soát API"),
        ("AI", "FastAPI, Vanna, Ollama, OracleRunner", "NL2SQL/Chatbot/report assistant"),
    ])
    heading(doc, "16.3. Mẫu đặc tả component", 2)
    table(doc, ["Trường", "Nội dung bắt buộc"], [
        ("Mã/tên", "Mã CMP, tên class/service/package, owner"),
        ("Mục tiêu", "FR/BR/NFR mà component thực hiện"),
        ("Input/output", "DTO/schema, kiểu dữ liệu, nullability, mã lỗi"),
        ("Dependencies", "Component, bảng, package, endpoint và secret reference"),
        ("State/transaction", "State machine, transaction boundary, retry/idempotency"),
        ("Security", "Authentication, authorization, data scope, masking"),
        ("Operations", "Metric, log, alert, health check, runbook"),
        ("Versioning", "Compatibility, migration, rollback và feature flag"),
    ])
    heading(doc, "16.4. Mẫu đặc tả API", 2)
    table(doc, ["Trường", "Ví dụ"], [
        ("Endpoint/method", "POST /api/messages/send"),
        ("Authorization", "Session + action SEND_MESSAGE hoặc OAuth scope message:send"),
        ("Request", "messageId/partNo/flightDate/messType/recipients/content/idempotencyKey"),
        ("Response", "StandardResult + status/correlationId/batchId"),
        ("Error", "VALIDATION_ERROR/CONFLICT/UPSTREAM_TIMEOUT/DB_ERROR"),
        ("SLA", "Request nhỏ ≤3 giây; batch trả 202 và xử lý nền"),
        ("Audit", "Actor, action, object, before/after, result, correlation"),
    ])
    heading(doc, "16.5. Kết luận thiết kế", 2)
    paragraph(doc, "Thiết kế đề xuất tạo một nền tảng nhất quán cho toàn bộ SRS: UI WebForms vẫn được bảo toàn, nghiệp vụ được phân lớp, tích hợp được cô lập, Oracle giữ vai trò nguồn chuẩn, báo cáo dùng snapshot, batch có idempotency và AI bị giới hạn bởi SQL Guard/read-only account. Sau khi phê duyệt SDD, mỗi phân hệ có thể tiếp tục lập LLD, API contract và kế hoạch thay đổi package độc lập.")


def add_design_annexes(doc):
    heading(doc, "PHỤ LỤC A. Các luồng tuần tự tham chiếu", 1)
    paragraph(doc, "Các luồng dưới đây là mẫu chuẩn để nhóm phát triển chuyển thành sequence diagram UML hoặc BPMN trong LLD. Mỗi bước phải gắn component, transaction boundary, mã lỗi và audit event.")
    sequences = [
        ("A.1. Nhận và phân tích điện văn", [
            ("1", "Receiver", "Nhận message/file, tạo ReceiveId và hash"),
            ("2", "Parser", "Kiểm tra encoding/header, nhận diện loại điện văn"),
            ("3", "Normalizer", "Chuẩn hóa ngày, callsign, sân bay, nội dung"),
            ("4", "Matcher", "Ghép flight/permission/KHB theo khóa ưu tiên"),
            ("5", "OracleWriter", "Ghi raw/normalized/status trong transaction"),
            ("6", "Publisher", "Phát sự kiện notification/report nếu đủ điều kiện"),
        ]),
        ("A.2. Gửi một điện văn", [
            ("1", "PageAdapter", "Nhận command, kiểm tra CSRF và quyền"),
            ("2", "MessageService", "Validate recipient/content/date và tạo idempotency key"),
            ("3", "Outbox", "Ghi QUEUED trong Oracle"),
            ("4", "MessageAdapter", "Gọi MESSAGE_PKG/AMHS với timeout"),
            ("5", "StatusRepository", "Cập nhật SENT/FAILED, retry metadata"),
            ("6", "Audit/Notify", "Ghi audit, thông báo kết quả và deep link"),
        ]),
        ("A.3. Đồng bộ Bravo", [
            ("1", "JobOrchestrator", "Tạo batch và khóa phạm vi ngày"),
            ("2", "SnapshotProvider", "Chụp dữ liệu HTSLB đã được duyệt"),
            ("3", "BravoAdapter", "Sinh payload theo mapping/version"),
            ("4", "Bravo API", "INSERT/UPDATE/NO_CHANGE/CONFLICT"),
            ("5", "Reconcile", "Đối soát khóa, actual time, purpose và count"),
            ("6", "Notification", "Thông báo hoàn thành hoặc hàng chờ lỗi"),
        ]),
        ("A.4. Sinh báo cáo", [
            ("1", "ReportPage", "Nhận filter và quyền dữ liệu"),
            ("2", "Metadata", "Kiểm tra field/operator và default"),
            ("3", "Snapshot", "Chọn nguồn và ghi SnapshotId"),
            ("4", "QueryService", "Truy vấn KPI/chart/detail cùng filter"),
            ("5", "Renderer", "Trả bảng, biểu đồ, tổng và warning nguồn"),
            ("6", "ExportWorker", "Sinh file nền nếu người dùng yêu cầu"),
        ]),
        ("A.5. NL2SQL", [
            ("1", "Chat API", "Nhận câu hỏi và session scope"),
            ("2", "Context Builder", "Bổ sung schema, date và nghiệp vụ"),
            ("3", "LLM", "Sinh SQL nháp và giải thích"),
            ("4", "SQL Guard", "Parse, allowlist, chặn DML/DDL và giới hạn"),
            ("5", "OracleRunner", "Execute bằng tài khoản read-only"),
            ("6", "Result", "Chuẩn hóa JSON/chart, lưu audit và feedback"),
        ]),
    ]
    for title, rows in sequences:
        heading(doc, title, 2)
        table(doc, ["Bước", "Thành phần", "Xử lý và dữ liệu trao đổi"], rows)
        table(doc, ["Điểm kiểm soát", "Thiết kế bắt buộc"], [
            ("Authorization", "Kiểm tra user/client, action và data scope trước bước ghi hoặc đọc dữ liệu nhạy cảm"),
            ("Idempotency", "Khóa theo source/batch/message; retry không nhân đôi kết quả"),
            ("Failure", "Mã lỗi, retryable, trạng thái trung gian và correlation ID"),
            ("Audit", "Actor/system, action, object, result, elapsed và dữ liệu before/after phù hợp"),
        ])
        page_break(doc)

    heading(doc, "PHỤ LỤC B. Từ điển dữ liệu mở rộng", 1)
    paragraph(doc, "Bảng là danh mục thiết kế tham chiếu. Khi lập LLD, DBA cần bổ sung kiểu vật lý, độ dài, index, partition, constraint và script migration theo schema thật.")
    rows = [
        ("FLIGHT_ID", "Flight", "Khóa chuyến chuẩn", "Oracle ATFM", "Không đổi sau khi tạo"),
        ("CALLSIGN", "Flight", "Số hiệu chuyến bay", "Oracle/ADS-B", "Trim/uppercase theo rule"),
        ("FLIGHTDATE", "Flight", "Ngày nghiệp vụ", "Oracle", "BusinessDateService"),
        ("FROM_AIRP", "Flight", "Sân bay đi", "Oracle/ADS-B", "Mã ICAO"),
        ("TO_AIRP", "Flight", "Sân bay đến", "Oracle/ADS-B", "Mã ICAO"),
        ("PERM_ID", "Permission", "Khóa phép bay", "Oracle", "FK tới master"),
        ("PERM_NUMBER", "Permission", "Số phép", "Oracle", "Tra cứu/index"),
        ("PERMDATE", "Permission", "Ngày hiệu lực", "Oracle", "DATE nghiệp vụ"),
        ("ENDDATE", "Permission", "Ngày kết thúc SC", "Oracle", "Cảnh báo hết hạn"),
        ("VALIDHOURS", "Permission", "Thời hạn NO", "Oracle", "Không âm/không sai format"),
        ("PART_NO", "Message", "Mã nhóm điện văn", "Message package", "Khóa ghép nội dung"),
        ("MESS_TYPE", "Message", "Loại điện văn", "Message package", "Danh mục kiểm soát"),
        ("CONTENT", "Message", "Nội dung raw/canonical", "Inbox/Outbox", "Mask khi log"),
        ("CONTENT_HASH", "Message", "Hash nội dung", "Integration store", "SHA-256"),
        ("STATUS", "Message", "Trạng thái state machine", "Message/Job", "Enum quản lý"),
        ("BATCH_ID", "Batch", "Định danh xử lý nền", "Job store", "Idempotency/progress"),
        ("SOURCE_ID", "Integration", "Định danh nguồn", "Raw store", "Không trùng trong scope"),
        ("SCHEMA_VERSION", "Integration", "Version schema", "Raw store", "Quarantine nếu không hỗ trợ"),
        ("RECEIVED_AT", "Integration", "Thời điểm nhận", "All adapters", "UTC"),
        ("SOURCE_TIMEZONE", "Integration", "Múi giờ nguồn", "All adapters", "Không suy diễn ngầm"),
        ("TRACK_ID", "ADS-B", "Khóa vệt bay", "T_TRACKS_LOG", "Unique theo nguồn"),
        ("LAT/LON", "ADS-B", "Vị trí", "T_TRACKS_LOG", "Range hợp lệ"),
        ("QUALITY_STATUS", "ADS-B", "Chất lượng bản ghi", "T_TRACKS_LOG", "ACCEPT/REVIEW/REJECT"),
        ("SNAPSHOT_ID", "Report", "Phiên dữ liệu báo cáo", "Report store", "KPI/detail cùng snapshot"),
        ("FILTER_HASH", "Report", "Hash bộ lọc", "Report store", "Chuẩn hóa JSON"),
        ("NOTIFICATION_ID", "Notification", "Khóa thông báo", "Notification", "User/scope"),
        ("READ_AT", "Notification", "Thời điểm đã đọc", "Notification", "Theo user"),
        ("CORRELATION_ID", "Cross-cutting", "Liên kết một request", "All log/audit", "UUID"),
        ("ACTOR_ID", "Audit", "Người/client thực hiện", "Audit", "Không tin từ payload"),
        ("ERROR_CODE", "Error", "Mã lỗi chuẩn", "Error/Job", "Ổn định theo API"),
    ]
    table(doc, ["Trường", "Đối tượng", "Ý nghĩa", "Nguồn", "Quy tắc"], rows)
    heading(doc, "PHỤ LỤC C. Danh mục API và package", 1)
    api_rows = [
        ("API-001", "/api/flights/search", "POST", "Tra cứu chuyến đa trường", "FlightQueryService"),
        ("API-002", "/api/flights/{id}", "GET", "Chi tiết chuyến", "FlightDetailService"),
        ("API-003", "/api/messages/send", "POST", "Gửi một điện văn", "MessageApplicationService"),
        ("API-004", "/api/messages/batch", "POST", "Gửi hàng loạt", "MessageBatchService"),
        ("API-005", "/api/messages/{id}/status", "GET", "Trạng thái delivery", "DeliveryTracker"),
        ("API-006", "/api/notifications", "GET", "Thông báo header", "NotificationQuery"),
        ("API-007", "/api/notifications/{id}/read", "POST", "Đánh dấu đã đọc", "NotificationCommand"),
        ("API-008", "/api/reports/{code}/query", "POST", "KPI/chart/detail", "ReportQueryService"),
        ("API-009", "/api/reports/{code}/export", "POST", "Export nền", "ExportWorker"),
        ("API-010", "/api/jobs/{batchId}", "GET", "Progress job", "JobQueryService"),
        ("API-011", "/api/integration/{source}/replay", "POST", "Replay quarantine", "ReplayService"),
        ("API-012", "/api/bravo/sync", "POST", "Đồng bộ Bravo", "BravoAdapter"),
        ("API-013", "/api/bravo/reconcile", "POST", "Đối soát Bravo", "ReconcileService"),
        ("API-014", "/api/adsb/sync", "POST", "Đồng bộ ADS-B", "AdsBAdapter"),
        ("API-015", "/api/ai/session", "POST", "Tạo phiên AI", "AI Chat API"),
        ("API-016", "/api/ai/query/preview", "POST", "Preview SQL", "SQL Guard"),
        ("API-017", "/api/ai/query/execute", "POST", "Execute SQL read-only", "OracleRunner"),
        ("API-018", "/api/health", "GET", "Health check", "HealthController"),
        ("PKG-001", "MESSAGE_PKG", "Oracle", "Tạo/gửi điện văn", "MessagePackageGateway"),
        ("PKG-002", "MESSAGE_FLIGHT_PKG", "Oracle", "Tra cứu flight theo điện văn", "MessageFlightRepository"),
        ("PKG-003", "A_TEST_SEARCH", "Oracle", "Tra cứu/export flight", "FlightQueryRepository"),
        ("PKG-004", "BRAVO_EXPORT_PKG", "Oracle", "Chuẩn bị dữ liệu Bravo", "BravoBatchRepository"),
        ("PKG-005", "NOTIFICATION_PKG", "Oracle", "Lưu/đọc notification", "NotificationRepository"),
    ]
    table(doc, ["Mã", "Endpoint/package", "Loại", "Mục đích", "Component gọi"], api_rows)
    page_break(doc)

    heading(doc, "PHỤ LỤC D. Mã lỗi, retry và xử lý vận hành", 1)
    errors = [
        ("VAL-001", "Ngày không hợp lệ", "Không", "Sửa input"),
        ("VAL-002", "Thiếu trường bắt buộc", "Không", "Bổ sung dữ liệu"),
        ("AUTH-001", "Phiên hết hạn", "Không", "Đăng nhập lại"),
        ("AUTH-002", "Không đủ quyền thao tác", "Không", "Liên hệ quản trị"),
        ("MATCH-001", "Không match được chuyến", "Có điều kiện", "Đưa review"),
        ("MATCH-002", "Nhiều ứng viên match", "Không tự retry", "Chọn thủ công"),
        ("DUP-001", "Nguồn đã xử lý", "Không", "Trả kết quả cũ"),
        ("DUP-002", "Idempotency conflict", "Không", "Tải lại version"),
        ("DB-001", "Oracle timeout", "Có", "Retry backoff"),
        ("DB-002", "Oracle constraint", "Không", "Quarantine/rollback"),
        ("DB-003", "Deadlock", "Có", "Retry giới hạn"),
        ("PKG-001", "Package trả lỗi nghiệp vụ", "Không", "Hiển thị mã package"),
        ("UP-001", "Upstream timeout", "Có", "Circuit breaker"),
        ("UP-002", "Upstream 401/403", "Không", "Cảnh báo credential"),
        ("UP-003", "Upstream schema mismatch", "Không", "Quarantine"),
        ("MSG-001", "Content format invalid", "Không", "Không gửi"),
        ("MSG-002", "Delivery failed", "Có", "Retry/replay"),
        ("MSG-003", "Recipient invalid", "Không", "Sửa địa chỉ"),
        ("JOB-001", "Batch lock tồn tại", "Không", "Trả batch đang chạy"),
        ("JOB-002", "Batch quá SLA", "Có", "Alert/operator"),
        ("JOB-003", "Chunk rollback", "Có điều kiện", "Chạy lại chunk"),
        ("RPT-001", "Snapshot expired", "Không", "Tạo snapshot mới"),
        ("RPT-002", "Filter không được phép", "Không", "Báo quyền"),
        ("RPT-003", "Export quá lớn", "Có", "Export nền/chia file"),
        ("AI-001", "SQL ngoài allowlist", "Không", "Reject/preview"),
        ("AI-002", "LLM timeout", "Có", "Retry một lần"),
        ("AI-003", "Oracle row limit", "Không", "Gợi ý filter"),
        ("SEC-001", "CSRF/invalid token", "Không", "Từ chối và audit"),
        ("SEC-002", "Secret/config thiếu", "Không", "Không khởi động service"),
        ("OPS-001", "Disk/queue đầy", "Không", "Alert và throttle"),
    ]
    table(doc, ["Mã", "Điều kiện", "Retry", "Xử lý người vận hành"], errors)
    heading(doc, "PHỤ LỤC E. Cấu hình và baseline môi trường", 1)
    configs = [
        ("DB_ATFM_DSN", "Application", "DSN Oracle ATFM", "Secret/config protected"),
        ("DB_ATFM_TIMEOUT", "Application", "Timeout Oracle", "Theo thao tác"),
        ("IMAP_HOST/PORT", "AeroSync", "Nguồn email", "Không log password"),
        ("AMHS_ENDPOINT", "AMHS", "Endpoint gửi/nhận", "TLS/allowlist"),
        ("BRAVO_BASE_URL", "Bravo", "API Bravo", "TLS + client credential"),
        ("BRAVO_TIMEOUT", "Bravo", "Timeout API", "Có circuit breaker"),
        ("ADSB_SOURCE_URL", "ADS-B", "Nguồn tracking", "Schema version"),
        ("QUEUE_CONNECTION", "Worker", "RabbitMQ/queue", "Service account"),
        ("REDIS_CONNECTION", "Cache", "Cache/idempotency", "TTL và secret"),
        ("REPORT_MAX_RANGE", "Report", "Khoảng ngày tối đa", "Theo role/report"),
        ("EXPORT_RETENTION_HOURS", "Export", "Thời hạn file", "Tự động xóa"),
        ("NOTIFICATION_POLL_SECONDS", "Notification", "Chu kỳ cập nhật", "Không request chồng"),
        ("AI_OLLAMA_HOST", "AI", "LLM endpoint", "Không public trực tiếp"),
        ("AI_MODEL", "AI", "Tên model/version", "Pin version"),
        ("AI_MAX_ROWS", "AI", "Giới hạn kết quả", "Read-only"),
        ("AI_SQL_TIMEOUT", "AI", "Timeout query", "Kill/stop theo policy"),
        ("AUDIT_RETENTION_DAYS", "Audit", "Lưu audit", "Theo ATTT"),
        ("LOG_LEVEL", "All", "Mức log", "INFO production, DEBUG có kiểm soát"),
        ("TIMEZONE_BUSINESS", "All", "Múi giờ nghiệp vụ", "Chốt trước triển khai"),
        ("FEATURE_BRAVO_SYNC", "Feature flag", "Bật/tắt sync", "Rollback an toàn"),
        ("FEATURE_NEW_SEARCH", "Feature flag", "Bật search mới", "Dual read/cutover"),
        ("HEALTH_INTERVAL", "Ops", "Health check interval", "Monitor"),
        ("RETRY_MAX_ATTEMPTS", "Integration", "Số lần retry", "Theo nguồn"),
        ("RETRY_BACKOFF_MS", "Integration", "Backoff", "Exponential + jitter"),
    ]
    table(doc, ["Tên cấu hình", "Phạm vi", "Ý nghĩa", "Yêu cầu"], configs)
    page_break(doc)

    heading(doc, "PHỤ LỤC F. Checklist bàn giao thiết kế", 1)
    checklist = [
        ("F-01", "Đã chốt sơ đồ context/deployment/component", "Kiến trúc sư"),
        ("F-02", "Đã chốt owner và phiên bản của mọi endpoint ngoài", "Tích hợp"),
        ("F-03", "Đã chốt bảng/package/index vật lý", "DBA"),
        ("F-04", "Đã chốt business date/timezone", "Nghiệp vụ"),
        ("F-05", "Đã chốt state machine điện văn/KHB/batch", "Nghiệp vụ"),
        ("F-06", "Đã có API contract và error catalog", "Phát triển"),
        ("F-07", "Đã có data dictionary và mapping", "Dữ liệu"),
        ("F-08", "Đã có quyền user/role/menu/action", "ATTT"),
        ("F-09", "Đã có retry/idempotency/quarantine", "Tích hợp"),
        ("F-10", "Đã có health check/metric/alert", "Vận hành"),
        ("F-11", "Đã có backup/restore/rollback runbook", "DBA/Vận hành"),
        ("F-12", "Đã có feature flag và dual-read plan", "Phát triển"),
        ("F-13", "Đã có mapping FR → component → data/API", "Quản lý thiết kế"),
        ("F-14", "Đã rà soát side effect của package hiện hữu", "DBA/Phát triển"),
        ("F-15", "Đã xác định dữ liệu mẫu và snapshot đại diện", "Nghiệp vụ"),
        ("F-16", "Đã chốt retention và masking", "ATTT/DBA"),
        ("F-17", "Đã chốt chủ sở hữu xử lý lỗi từng hệ thống ngoài", "Vận hành"),
        ("F-18", "Đã chốt giới hạn AI và Oracle read-only account", "ATTT/AI"),
        ("F-19", "Đã chốt thứ tự deploy/rollback script", "DevOps/DBA"),
        ("F-20", "Đã cập nhật SDD khi thay đổi SRS hoặc mã nguồn", "Quản lý cấu hình"),
    ]
    table(doc, ["Mã", "Điều kiện bàn giao", "Chủ trì"], checklist)
    heading(doc, "PHỤ LỤC G. Mẫu trang thiết kế component", 1)
    table(doc, ["Mục", "Nội dung mẫu"], [
        ("Tên/Mã", "CMP-MSG-001 – MessageApplicationService"),
        ("Mục tiêu", "Điều phối gửi một và hàng loạt điện văn theo quyền"),
        ("FR liên quan", "FR-ALT-002, FR-INT-004, FR-OPT-024"),
        ("Input", "SendMessageCommand với flightDate, partNo, recipient, content, idempotencyKey"),
        ("Output", "StandardResult với status, messageId, correlationId, retryable"),
        ("Dependencies", "PermissionService, MessageRepository, AmhsAdapter, AuditService"),
        ("Transaction", "Oracle outbox; adapter ngoài ngoài transaction; cập nhật trạng thái sau response"),
        ("Security", "Action SEND_MESSAGE, data scope theo đơn vị/sân bay, masking content log"),
        ("Metrics", "send_success, send_failed, send_latency, queue_depth, retry_count"),
        ("Rollback", "Đưa FAILED về RETRY_WAIT hoặc replay theo MessageId; không sửa SENT"),
    ])
    paragraph(doc, "Kết thúc tài liệu thiết kế tổng thể. Các trang LLD tiếp theo phải giữ mã thành phần, mã quyết định, hợp đồng dữ liệu và nguyên tắc trạng thái đã nêu trong SDD này.")


def add_change_annex(doc):
    heading(doc, "PHỤ LỤC H. Thiết kế quản lý phiên bản và thay đổi", 1)
    paragraph(doc, "Phụ lục này quy định cách giữ tính nhất quán giữa SRS, SDD, mã nguồn WebForms, package Oracle, adapter tích hợp và cấu hình triển khai. Mục tiêu là có thể nâng cấp từng phần mà không làm mất dữ liệu hoặc phá vỡ URL/hợp đồng hiện hữu.")
    heading(doc, "H.1. Ma trận phiên bản thành phần", 2)
    table(doc, ["Thành phần", "Phiên bản theo dõi", "Tương thích", "Nơi lưu"], [
        ("WebForms page/JS/CSS", "Build number + asset version", "Menu_ID/URL và response contract", "Source control/PublishOutput"),
        ("Business/DAL", "Assembly version", "DTO/service interface", "prjBusinessLogic/prjComponents"),
        ("Oracle package", "Package revision + deploy ID", "Procedure/parameter/result", "Database/Oracle deploy script"),
        ("API Gateway", "Route/API version", "OpenAPI/schema/error catalog", "Gateway configuration"),
        ("External adapter", "Adapter/schema version", "Upstream contract", "Worker/service package"),
        ("AI prompt/model", "Prompt/model/training version", "SQL guard/schema metadata", "AI service configuration"),
    ])
    page_break(doc)
    heading(doc, "H.2. Luồng phê duyệt thay đổi", 2)
    table(doc, ["Bước", "Chủ trì", "Đầu ra"], [
        ("Request", "Nghiệp vụ/Phát triển", "Mã thay đổi, lý do, FR/BR bị ảnh hưởng"),
        ("Impact", "Kiến trúc sư/DBA/ATTT", "Ảnh hưởng component, data, API, downtime và rollback"),
        ("Design", "Kiến trúc sư", "Cập nhật SDD/LLD/sơ đồ/hợp đồng"),
        ("Build", "Phát triển/DBA", "Bản build, script deploy/rollback, checksum"),
        ("Deploy", "Vận hành", "Release note, precheck/postcheck, monitor"),
        ("Close", "Chủ sở hữu nghiệp vụ", "Xác nhận trạng thái và cập nhật nhật ký thay đổi"),
    ])
    paragraph(doc, "Mọi thay đổi package hoặc index phải đi cùng script rollback. Thay đổi API phải có version hoặc compatibility layer; không thay đổi âm thầm trường response đang được màn hình hiện hữu sử dụng.")
    page_break(doc)
    heading(doc, "H.3. Ma trận tương thích", 2)
    table(doc, ["Client/consumer", "Hợp đồng cần giữ", "Cách tương thích"], [
        ("ASPX/JavaScript cũ", "Callback argument/response, Menu_ID", "Adapter response và feature flag"),
        ("Report export", "Tên cột, định dạng file, filter", "Thêm metadata, giữ column alias"),
        ("Oracle package caller", "Procedure/parameter/result", "Procedure versioned hoặc overload"),
        ("Bravo", "Payload/mapping/idempotency", "Schema version và mapping registry"),
        ("AMHS/AFTN", "Message format/address/status", "Adapter parser và delivery state"),
        ("AI client", "Chat/query result schema", "API version, SQL guard policy"),
    ])
    page_break(doc)
    heading(doc, "H.4. Kế hoạch chuyển đổi dữ liệu", 2)
    table(doc, ["Đối tượng", "Bước chuyển đổi", "Điều kiện dừng"], [
        ("Message/Outbox", "Bổ sung hash, status, correlation; backfill theo batch", "Duplicate hoặc content hash không xác định"),
        ("Flight date", "Tính BusinessDate từ timestamp/timezone và ghi cờ uncertain", "Không đủ timezone/source"),
        ("ADS-B", "Chuẩn hóa quality/match và tạo snapshot", "Sai schema hoặc duplicate track"),
        ("Bravo", "Lấy snapshot, match key, đẩy test batch rồi mới bật flag", "Conflict vượt ngưỡng"),
        ("Report", "Tạo metadata/filter hash và so sánh query cũ/mới", "KPI lệch ngoài ngưỡng được phê duyệt"),
    ])
    page_break(doc)
    heading(doc, "H.5. Ma trận sở hữu vận hành", 2)
    table(doc, ["Đối tượng", "Owner chính", "Escalation", "SLA phản hồi"], [
        ("Oracle/package/index", "DBA ATFM", "Kiến trúc sư/nhà thầu", "Theo mức sự cố"),
        ("ATFM_WEB/IIS", "Quản trị ứng dụng", "Đội phát triển", "Theo giờ vận hành"),
        ("ADS-B/SLOT", "Đơn vị cung cấp dữ liệu", "Tích hợp/ATFM", "Theo batch dự kiến"),
        ("AMHS/AFTN", "Đơn vị thông tin hàng không", "Quản trị/nhà cung cấp", "Theo delivery SLA"),
        ("Bravo API", "Chủ hệ thống Bravo", "Tích hợp/ATFM", "Theo sync SLA"),
        ("AI service", "Đội AI/ứng dụng", "ATTT/DBA", "Theo query/API SLA"),
        ("Backup/restore", "Vận hành/DBA", "Quản lý hệ thống", "Theo RPO/RTO"),
    ])
    page_break(doc)
    heading(doc, "H.6. Nhật ký quyết định thiết kế", 2)
    table(doc, ["Mã", "Quyết định", "Trạng thái"], [
        ("ADR-001", "Giữ WebForms làm lớp trình bày, bổ sung service façade", "Đề xuất"),
        ("ADR-002", "Oracle là nguồn nghiệp vụ chuẩn; cache/report phải ghi snapshot", "Đề xuất"),
        ("ADR-003", "Bravo chỉ qua API, không ghi DB trực tiếp", "Bắt buộc"),
        ("ADR-004", "Tích hợp ngoài dùng idempotency/retry/quarantine", "Bắt buộc"),
        ("ADR-005", "AI dùng Oracle read-only và SQL Guard", "Bắt buộc"),
        ("ADR-006", "Job lớn chạy nền và trả BatchId", "Đề xuất"),
        ("ADR-007", "BusinessDateService là nơi duy nhất xác định ngày nghiệp vụ", "Bắt buộc"),
        ("ADR-008", "Thay đổi API/package phải có version/compatibility", "Bắt buộc"),
    ])
    paragraph(doc, "Khi một quyết định được phê duyệt, trạng thái trong nhật ký phải đổi từ Đề xuất sang Đã chốt, đồng thời cập nhật các chương kiến trúc, dữ liệu, API và vận hành có liên quan.")


def configure_styles(doc):
    normal = doc.styles["Normal"]
    normal.font.name = "Times New Roman"
    normal._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
    normal.font.size = Pt(10.5)
    normal.paragraph_format.line_spacing = 1.12
    normal.paragraph_format.space_after = Pt(5)
    for name, size, color in [("Heading 1", 15, (31, 78, 121)), ("Heading 2", 13, (47, 84, 150)), ("Heading 3", 11.5, (68, 68, 68))]:
        style = doc.styles[name]
        style.font.name = "Times New Roman"
        style._element.rPr.rFonts.set(qn("w:eastAsia"), "Times New Roman")
        style.font.size = Pt(size)
        style.font.bold = True
        style.font.color.rgb = RGBColor(*color)


def build():
    doc = Document()
    section = doc.sections[0]
    section.top_margin = Cm(2.0)
    section.bottom_margin = Cm(1.8)
    section.left_margin = Cm(2.3)
    section.right_margin = Cm(1.8)
    configure_styles(doc)
    add_cover(doc)
    add_control(doc)
    add_toc(doc)
    page_break(doc)
    add_scope(doc)
    add_context(doc)
    add_logical_architecture(doc)
    add_integration_design(doc)
    add_message_design(doc)
    add_opt_design(doc)
    add_report_design(doc)
    add_ai_design(doc)
    add_data_design(doc)
    add_api_design(doc)
    add_ui_workflow(doc)
    add_security_nfr(doc)
    add_operations(doc)
    add_implementation(doc)
    add_traceability_appendix(doc)
    add_appendices(doc)
    add_design_annexes(doc)
    add_change_annex(doc)
    for item in doc.sections:
        add_page_number(item)
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    doc.save(OUTPUT)
    print(OUTPUT)


if __name__ == "__main__":
    build()
