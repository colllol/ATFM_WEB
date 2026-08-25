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
    heading(doc, "5. Phân Hệ Cảnh Báo Và Tiện Ích Hỗ Trợ", 1)
    paragraph(doc, "Phân hệ này tập trung vào cảnh báo vận hành theo thời gian thực và các tiện ích hỗ trợ quản lý sử dụng vùng trời, KHB quân sự. Thiết kế tách riêng cơ chế phát hiện/thông báo khỏi quy trình nghiệp vụ có phê duyệt, đồng thời bảo đảm mọi thay đổi đều có trạng thái và nhật ký.")
    heading(doc, "5.1. Hệ thống cảnh báo thông minh đa kịch bản", 2)
    heading(doc, "5.1.1. Kiến trúc cảnh báo runtime", 3)
    table(doc, ["Thành phần", "Trách nhiệm", "Nguồn"], [
        ("NotificationRuleEngine", "Đánh giá NOPERM, sai ngày, hết hạn, batch lỗi và bất thường", "Oracle/query service, integration events"),
        ("NotificationRepository", "Lưu notification, trạng thái đọc và phạm vi user/role", "T_NOTIFICATION/notification package"),
        ("NotificationPublisher", "Đưa thông báo vào header/dashboard hoặc kênh email", "Application event bus"),
        ("NotificationQuery", "Phân trang, lọc mức độ, mở deep link", "WebMethod/API"),
        ("NotificationAudit", "Ghi acknowledge, dismiss, open và escalation", "AuditService"),
    ])
    heading(doc, "5.1.2. Kịch bản cảnh báo", 3)
    table(doc, ["Kịch bản", "Điều kiện phát hiện", "Hiển thị và xử lý"], [
        ("Không có phép/KHB", "Điện văn hoặc chuyến bay không tìm thấy phép bay/KHB ngày tương ứng", "Badge tại header, mức cảnh báo, deep link đến bản ghi; cho phép acknowledge và ghi audit"),
        ("Phép bay hết hiệu lực", "PERMDATE, VALIDHOURS hoặc ENDDATE đã quá thời điểm nghiệp vụ", "Mức cao, liên kết danh sách phép; không tự động sửa dữ liệu nguồn"),
        ("Sai ngày bay", "Ngày trên kế hoạch không khớp ngày thực tế sau khi chuẩn hóa múi giờ", "Hiển thị ngày nguồn/ngày tính được, yêu cầu người có quyền rà soát"),
        ("Batch/integration lỗi", "ADS-B, Bravo, AMHS, Gen KHB hoặc job nền không đạt SLA/hoàn tất một phần", "Hiển thị số thành công/lỗi, chi tiết lỗi, retry có kiểm soát và correlation id"),
        ("Bất thường báo cáo", "KPI hoặc tỷ lệ vượt ngưỡng cấu hình theo sân bay, ngày hoặc loại chuyến", "Mức độ, nguyên nhân dự kiến, drill-down, trạng thái xử lý và lịch sử"),
    ])
    heading(doc, "5.1.3. Luồng cập nhật tại header", 3)
    code_block(doc, "Rule/Event → NotificationRepository → ScopeResolver → Header badge/list\n                                  ├─ acknowledge/dismiss → NotificationAudit\n                                  └─ deep link → màn hình nghiệp vụ có kiểm tra quyền")
    paragraph(doc, "Header polling hoặc push phải có khoảng thời gian cấu hình, chống gọi chồng, chỉ trả thông báo thuộc user/role/đơn vị và phạm vi dữ liệu hiện hành. Khi mất kết nối, giao diện giữ thông báo cuối cùng, hiển thị thời điểm đồng bộ gần nhất và tự đồng bộ bù khi kết nối trở lại.")
    heading(doc, "5.2. Quản lý thông tin KHB quân sự và sử dụng vùng trời", 2)
    paragraph(doc, "Nội dung được tách thành hai phần nhỏ: (1) quản lý thông tin sử dụng vùng trời, bao gồm Live Fire Message và phê duyệt; (2) quản lý KHB quân sự từ nhập mới đến phát điện văn và tra cứu báo cáo sau duyệt.")
    heading(doc, "5.2.1. Quản lý sử dụng vùng trời", 3)
    paragraph(doc, "Live Fire Message ghi nhận khu vực, thời gian hiệu lực, điều kiện sử dụng, đơn vị đề nghị và nội dung điện văn. Hệ thống phải kiểm tra trường bắt buộc, giao thoa thời gian/khu vực, đối tượng nhận và quyền phê duyệt trước khi cho phép phát đi.")
    table(doc, ["Bước", "Tác nhân", "Kết quả"], [
        ("Create draft", "Người lập", "Tạo MessageId, nội dung, vùng trời, thời gian hiệu lực và trạng thái DRAFT"),
        ("Validate", "Application service", "Kiểm tra bắt buộc, xung đột thời gian, địa chỉ và format điện văn"),
        ("Submit", "Người lập", "Chuyển PENDING_APPROVAL và ghi audit"),
        ("Review", "Người duyệt", "Xem raw/normalized, lịch sử và dữ liệu liên quan"),
        ("Accept/Reject", "Người duyệt", "ACCEPTED hoặc REJECTED; bắt buộc lý do khi từ chối"),
        ("Export/Dispatch", "Hệ thống/người phát", "Sinh điện văn, gửi AMHS/AFTN, theo dõi delivery"),
        ("Close", "Hệ thống", "EXPIRED/CANCELLED khi hết hiệu lực hoặc bị hủy có quyền"),
    ])
    table(doc, ["Nhóm thông tin", "Dữ liệu chính", "Kiểm soát"], [
        ("Vùng trời", "Mã/khu vực, tọa độ hoặc mô tả, độ cao, đơn vị quản lý", "Chuẩn hóa định dạng, kiểm tra vùng hợp lệ và quyền cập nhật"),
        ("Điều kiện sử dụng", "Ngày/giờ bắt đầu-kết thúc, mục đích, hạn chế và liên hệ", "Không cho thời gian kết thúc trước bắt đầu; kiểm tra giao thoa"),
        ("Xung đột", "Chuyến bay/KHB/điện văn bị ảnh hưởng và mức độ", "Cảnh báo trước submit; người duyệt xác nhận hoặc từ chối"),
    ])
    code_block(doc, "DRAFT → VALIDATED → PENDING_APPROVAL → ACCEPTED → QUEUED → SENDING\n                                               ├──────────────→ REJECTED\n                                               └──────────────→ CANCELLED\nSENDING → SENT → DELIVERED\nSENDING → FAILED → RETRY_WAIT → SENDING")
    heading(doc, "5.2.1.1. Thiết kế Message Management hiện hữu", 3)
    paragraph(doc, "Màn hình MessManagement.aspx hiện đang nạp dữ liệu qua DayFlightsDAL, nhận callback bằng chuỗi tham số và gọi MESSAGE_PKG/QlbOutBoxDAL trực tiếp. Thiết kế chuyển tiếp đề xuất giữ callback để tương thích nhưng đưa toàn bộ xử lý vào MessageApplicationService.")
    table(doc, ["Thành phần", "Thiết kế đề xuất", "Mã nguồn liên quan"], [
        ("MessagePageAdapter", "Chuyển callback/UI input thành Command DTO", "MessManagement.aspx.cs"),
        ("MessageApplicationService", "Kiểm tra quyền, validate, transaction và điều phối gửi", "prjBusinessLogic/Application"),
        ("DayFlightMessageRepository", "Tra cứu PART_NO, CONTENT, ADDRESS theo ngày/loại", "DayFlightsDAL.cs"),
        ("MessagePackageGateway", "Đóng gói gọi MESSAGE_PKG và chuẩn hóa result", "clsResuftAPI / Oracle package"),
        ("OutboxStatusRepository", "Quản lý QUEUED/SENDING/SENT/FAILED", "QlbOutBoxDAL.cs"),
        ("MessageAuditService", "Ghi actor, before/after, correlation và lỗi", "WriteLogHistory2Database"),
    ])
    heading(doc, "5.2.1.2. Quy tắc gửi an toàn", 3)
    for item in [
        "Không chuyển trạng thái SENT trước khi package/AMHS trả kết quả thành công.",
        "Mỗi lần gửi có MessageId và IdempotencyKey; gửi lại không tạo bản tin trùng.",
        "Ngày phải parse theo một định dạng nghiệp vụ cố định, không phụ thuộc CultureInfo của máy chủ.",
        "Kết quả trả về dùng StandardResult gồm Code, Message, Status, BatchId và Retryable.",
        "Gửi hàng loạt phải chạy nền; giao diện chỉ hiển thị progress và kết quả từng dòng.",
    ]:
        bullet(doc, item)
    heading(doc, "5.2.2. Quản lý KHB quân sự", 3)
    paragraph(doc, "Quy trình KHB quân sự gồm: Nhập mới KHB → Duyệt (Accepted) → Tạo điện văn (Export Message) → Phát đi. Sau khi duyệt, dữ liệu được cung cấp tại Daily Military Report ở chế độ chỉ xem và lấy dữ liệu cho người khai thác.")
    table(doc, ["Giai đoạn", "Màn hình/thành phần", "Yêu cầu thiết kế"], [
        ("Nhập mới", "ListFinishedFlightsMilitary.aspx", "Nhập và kiểm tra ngày bay, callsign, sân bay, mục đích, vùng trời; lưu DRAFT; không hiển thị như dữ liệu chính thức"),
        ("Duyệt", "Military Accepted", "So sánh trước/sau, kiểm tra quyền và xung đột; ACCEPT hoặc REJECT kèm lý do; ghi audit"),
        ("Export Message", "Message package/worker", "Chỉ tạo điện văn từ bản ghi ACCEPTED; sinh MessageId, batch và correlation id"),
        ("Phát đi", "AMHS/AFTN adapter", "Đưa vào outbox, theo dõi QUEUED/SENDING/SENT/FAILED, retry và đối soát delivery"),
        ("Báo cáo", "ListFinishedFlightsMilitaryReport.aspx", "Chỉ đọc dữ liệu đã duyệt; lọc theo ngày, đơn vị, khu vực, trạng thái; không cập nhật trực tiếp"),
    ])
    code_block(doc, "NEW/DRAFT → SUBMITTED → ACCEPTED → MESSAGE_QUEUED → SENT\n                         ├────────────────→ REJECTED\n                         └────────────────→ CANCELLED\nSENT → DELIVERED / FAILED → RETRY_WAIT")
    heading(doc, "5.2.2.1. Quy tắc dữ liệu và phân quyền KHB quân sự", 3)
    table(doc, ["Trạng thái", "Tác nhân", "Quyền và kiểm soát"], [
        ("Bản nháp", "Người lập/đơn vị", "Được sửa trong phạm vi đơn vị; chưa phải dữ liệu báo cáo chính thức"),
        ("Chờ duyệt", "Người lập/người duyệt", "Không sửa đồng thời; người duyệt xem bản trước/sau và lý do từ chối"),
        ("Accepted", "Người duyệt/quản lý", "Khóa phiên bản được duyệt; sửa phải tạo revision và chạy lại quy trình"),
        ("Đã phát", "Khai thác/tra cứu", "Chỉ đọc trạng thái phát và delivery; không chỉnh nội dung điện văn đã gửi"),
        ("Military Report", "Người khai thác", "Chỉ đọc, lọc/xuất dữ liệu theo quyền; không cập nhật trực tiếp vào KHB"),
    ])
    heading(doc, "5.2.2.2. Thiết kế dữ liệu điện văn dùng chung", 3)
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


def add_opt_detailed_appendix(doc):
    heading(doc, "PHỤ LỤC I. Thiết kế chi tiết phân hệ nâng cấp và tối ưu HTSLB", 1)
    paragraph(doc, "Phụ lục này cụ thể hóa Mục 6 thành các luồng, component, dữ liệu, hợp đồng xử lý và điểm kiểm soát để lập LLD, script Oracle và kế hoạch triển khai. Phạm vi bao phủ FR-OPT-001 đến FR-OPT-032; URL và Menu_ID hiện hữu phải được giữ tương thích.")
    heading(doc, "I.1. Phạm vi và ma trận component", 2)
    table(doc, ["Nhóm", "FR-OPT", "Màn hình/nguồn chính", "Component thiết kế"], [
        ("Tra cứu, cảnh báo", "001–013, 021, 024, 030", "SearchExtension, SearchPermissionAdv, Inbox, Permission lists, ListFlightOnMess, header", "SearchPageAdapter, FlightQueryService, NotificationRuleEngine, PermissionStatusService"),
        ("Hiệu năng và batch", "014–018", "ExportBravo, Calendar Accepted, Gen KHB/AFTN, Inbox archive", "BatchOrchestrator, ChunkProcessor, ProgressRepository, RetryPolicy"),
        ("Ngày bay và KHB", "019, 020, 022, 023, 025, 027", "DaylyFlight, Calendar, Daily Statistic, KHB quân sự", "BusinessDateService, FplMatcher, PurposeCatalog, RevisionService"),
        ("Bravo và đối soát", "026, 028, 029", "Bravo export/sync, batch edit, conflict/reconcile", "BravoAdapter, MappingRegistry, ConflictQueue, ReconcileService"),
        ("Báo cáo mở rộng", "031, 032", "ReportNew, preset/filter, compare KHBHĐBN, anomaly", "ReportMetadata, SnapshotProvider, KPI/AnomalyService"),
    ])
    heading(doc, "I.2. Kiến trúc xử lý dùng chung", 2)
    code_block(doc, "WebForms/JS → PageAdapter → DTO → Application Service → Domain Rule → Oracle DAL/Package or External Adapter → Audit/Notification/Batch")
    table(doc, ["Lớp", "Trách nhiệm", "Nguyên tắc"], [
        ("PageAdapter", "Giữ callback, Menu_ID, filter và format hiện hữu", "Không để UI gọi SQL/package trực tiếp; kiểm tra CSRF và input size"),
        ("Application Service", "Điều phối use case, transaction boundary và quyền", "Một use case một service; trả StandardResult/QueryResult"),
        ("Domain Rule", "Ngày nghiệp vụ, match chuyến, cảnh báo, trạng thái batch", "Deterministic, có mã rule và version cấu hình"),
        ("DAL/Package", "Truy vấn, ghi dữ liệu và gọi Oracle", "Bind parameter, timeout, execution plan và optimistic version"),
        ("Worker/Adapter", "Batch lớn và tích hợp Bravo/ADS-B/AMHS", "Idempotency, retry, quarantine, correlation ID"),
    ])
    heading(doc, "I.3. Thiết kế tìm kiếm đa trường và tra cứu nâng cao", 2)
    table(doc, ["Bước", "Xử lý", "Kết quả/kiểm soát"], [
        ("Build filter", "Field/operator/value, ngày, page, sort và data scope", "Filter DTO có schema; bỏ field ngoài allowlist"),
        ("Validate", "Kiểu ngày/giờ, khoảng tối đa, ký tự và quyền field", "VAL-001/VAL-002 hoặc AUTH-002; không query khi lỗi"),
        ("Compile predicate", "Map field → cột/index; bind toàn bộ giá trị", "Không nối chuỗi SQL từ input; ghi FilterHash"),
        ("Query", "Lọc phép bay, chuyến, KHB, điện văn theo snapshot/ngày", "Phân trang ổn định; trả total có giới hạn"),
        ("Drill-down", "Mở View_PermSC/View_PermNo/Inbox/Military Report", "Kiểm tra lại ID và data scope trước khi đọc"),
        ("Export", "Đưa truy vấn lớn vào ExportWorker", "Không giữ DataTable trong request; ghi BatchId/progress"),
    ])
    table(doc, ["Nhóm field", "Index/điều kiện đề xuất", "Ghi chú"], [
        ("Phép bay", "PERMDATE, ENDDATE, PERM_ID, PERM_NUMBER", "Hết hiệu lực, số phép và khoảng ngày"),
        ("Chuyến/KHB", "FLIGHTDATE, CALLSIGN, FROM_AIRP, TO_AIRP, STATUS", "Tách ngày nghiệp vụ và timestamp nguồn"),
        ("Điện văn", "PART_NO, MESS_TYPE, RECEIVED_AT, CONTENT_HASH", "Tra cứu theo điện văn, chống trùng"),
        ("Quân sự", "UNIT_ID, PURPOSE_CODE, AREA_CODE, ACCEPTED_AT", "Bảo vệ phạm vi theo đơn vị"),
    ])
    heading(doc, "I.4. Thiết kế cảnh báo và cập nhật runtime", 2)
    table(doc, ["Rule", "Khóa chống trùng", "Mức độ", "Hành động"], [
        ("Không có phép/KHB", "SOURCE_TYPE + FLIGHT_ID + BUSINESS_DATE", "Cao", "Deep link tra cứu, acknowledge"),
        ("Phép hết hiệu lực", "PERM_ID + rule_version + date", "Cao", "Đánh dấu đỏ, không tự sửa"),
        ("Sai ngày bay", "FLIGHT_ID + source_date + calculated_date", "Cao", "Rà soát và ghi before/after"),
        ("Batch quá SLA/lỗi", "BATCH_ID + phase + error_code", "TB/Cao", "Progress, retry hoặc quarantine"),
        ("Bất thường", "REPORT_CODE + FILTER_HASH + window", "Cấu hình", "Drill-down và trạng thái xử lý"),
    ])
    paragraph(doc, "Notification polling phải chống request chồng, lọc theo user/role/đơn vị, hỗ trợ chưa đọc/đã đọc và hiển thị thời điểm đồng bộ cuối. Dedupe key ổn định giữa retry nhưng thay đổi khi rule_version hoặc source event thay đổi.")
    heading(doc, "I.5. Thiết kế batch và chỉ tiêu hiệu năng", 2)
    code_block(doc, "CreateBatch → LockScope → SnapshotSource → ValidateChunks → ProcessChunks → PersistCounters → Reconcile → PublishNotification → Finalize")
    table(doc, ["Batch", "Phân đoạn xử lý", "Chỉ tiêu/điều kiện đạt"], [
        ("ExportBravo", "snapshot → mapping → chunk export → response → reconcile", "Mục tiêu dưới 180 giây; cảnh báo ở 80% SLA"),
        ("Gen KHB/AFTN", "load KHB → validate → generate message → outbox → delivery", "Mục tiêu dưới 180 giây; không phát khi còn invalid"),
        ("F8 Calendar Accepted", "chọn dòng → kiểm tra version → ép dòng → audit", "Không mất dữ liệu; conflict yêu cầu reload"),
        ("Inbox mở rộng >7 ngày", "range query → archive index → page/export", "Không full scan; giới hạn theo quyền"),
        ("Replay lỗi", "quarantine → sửa/duyệt → replay chunk", "Không duplicate; giữ source/correlation"),
    ])
    table(doc, ["Chỉ số", "Cách đo", "Ngưỡng vận hành"], [
        ("Elapsed", "created_at đến finalized_at theo batch/phase", "<180 giây cho ExportBravo và Gen KHB"),
        ("Throughput", "rows_processed / elapsed_seconds", "Theo baseline, theo dõi p50/p95"),
        ("Error rate", "failed / total theo chunk", "Cảnh báo khi vượt ngưỡng cấu hình"),
        ("Queue depth", "queued + retry_wait", "Throttle khi vượt capacity"),
        ("Reconcile gap", "source - inserted - updated - skipped - failed", "Bằng 0 hoặc có discrepancy record"),
    ])
    heading(doc, "I.6. Thiết kế ngày bay, KHB và revision", 2)
    table(doc, ["Quy tắc", "Thiết kế", "Lưu vết"], [
        ("Ngày nghiệp vụ", "BusinessDateService nhận timestamp, source timezone và airport rule", "source_date, calculated_date, timezone, rule_version"),
        ("Sai ngày thực tế", "FplMatcher so sánh KHB/permission với actual flight", "Không tự đổi ngày; yêu cầu xác nhận"),
        ("KHB đã build", "RevisionService tạo revision mới và phát notification", "build_id, revision_no, actor, before/after"),
        ("Mục đích bay", "PurposeCatalog chuẩn hóa mã/tên theo loại KHB", "purpose_code và label version"),
        ("KHB quân sự", "Nhập mới → Accepted → Export Message → Dispatch → Military Report", "Accepted khóa phiên bản; báo cáo chỉ đọc"),
    ])
    heading(doc, "I.7. Thiết kế Bravo, conflict và đối soát", 2)
    table(doc, ["Thành phần", "Trách nhiệm", "Dữ liệu bắt buộc"], [
        ("MappingRegistry", "Quản lý mapping field/phiên bản payload Bravo", "schema_version, field_map, effective_from/to"),
        ("BravoAdapter", "Gửi payload, timeout/circuit breaker, chuẩn hóa response", "request_id, idempotency_key, upstream_status"),
        ("ConflictQueue", "Lưu bản ghi không match hoặc khác actual time/purpose", "conflict_type, source_value, target_value, owner"),
        ("ReconcileService", "Đối chiếu count, key, trạng thái và thời điểm", "batch_id, source_count, result_count, discrepancy"),
        ("RetryWorker", "Retry lỗi tạm thời, không retry lỗi nghiệp vụ", "attempt, next_retry_at, error_code"),
    ])
    heading(doc, "I.8. Bảo mật, rollback và kiểm thử thiết kế", 2)
    table(doc, ["Kiểm soát", "Yêu cầu thiết kế"], [
        ("Phân quyền", "Kiểm tra action, role, đơn vị và data scope ở mọi command/query"),
        ("Audit", "Ghi actor, action, object, before/after, result, elapsed, correlation_id"),
        ("Concurrency", "Optimistic version cho phép/KHB; lock theo batch; trả CONFLICT khi đổi version"),
        ("Rollback", "Có script rollback package/index/config; snapshot để chạy bù"),
        ("Kiểm thử", "Functional, boundary ngày, duplicate/retry, quyền, p95 và đối soát"),
        ("Nghiệm thu", "Không mất/duplicate dữ liệu, cảnh báo đúng rule, batch đạt SLA, report khớp nguồn"),
    ])
    heading(doc, "I.9. Danh mục đầu ra LLD cần hoàn thiện", 2)
    table(doc, ["Mã đầu ra", "Nội dung", "FR liên quan"], [
        ("LLD-OPT-01", "Component/sequence cho Search và Notification", "001–013, 021, 024, 030"),
        ("LLD-OPT-02", "Batch, chunk, progress, retry và benchmark", "014–018"),
        ("LLD-OPT-03", "BusinessDate/FPL/KHB revision/purpose", "019, 020, 022, 023, 025, 027"),
        ("LLD-OPT-04", "Bravo mapping, adapter, conflict, reconcile", "026, 028, 029"),
        ("LLD-OPT-05", "Report metadata, snapshot, compare, anomaly", "031, 032"),
        ("LLD-OPT-06", "Oracle DDL/index/package và rollback", "Toàn bộ FR-OPT"),
    ])
    heading(doc, "I.10. Phiếu thiết kế riêng theo từng mã FR-OPT", 2)
    paragraph(doc, "Mỗi phiếu dưới đây là baseline để lập LLD, API contract, script Oracle và test case riêng cho từng mã. Các nguyên tắc phân quyền, audit, ngày nghiệp vụ, retry và rollback dùng chung tại I.1–I.9 nhưng phải được kiểm tra lại trong từng phiếu.")
    profiles = {
        "retention": {
            "actor": "Quản trị cấu hình và người khai thác INBOX; có phiên hợp lệ và phạm vi dữ liệu được cấp.",
            "flow": "Nhận/lưu raw → áp dụng retention cấu hình → archive hoặc dọn theo batch → ghi thống kê → cho phép tra cứu/khôi phục theo chính sách.",
            "components": "InboxRepository, RetentionPolicyService, ArchiveWorker, AuditService, SearchPageAdapter.",
            "data": "Inbox raw/normalized, RECEIVED_AT, RETENTION_DAYS, archive batch, cleanup log; index theo ngày nhận và nguồn.",
            "rules": "Không xóa bản ghi còn hạn; lỗi dọn dữ liệu chuyển retry/quarantine; không để mất liên kết raw–normalized–audit.",
            "output": "Kết quả tra cứu và báo cáo số dòng đã lưu/dọn; metric retention lag; rollback bằng backup/snapshot trước cleanup.",
        },
        "notification": {
            "actor": "Người khai thác, người duyệt hoặc operator thuộc user/role/đơn vị nhận cảnh báo.",
            "flow": "Rule/Event → NotificationRepository → ScopeResolver → header/list → acknowledge/deep-link → audit; retry không tạo thông báo trùng.",
            "components": "NotificationRuleEngine, NotificationRepository, NotificationQuery, NotificationPublisher, NotificationAudit.",
            "data": "T_NOTIFICATION, NOTIFICATION_PKG, SOURCE_TYPE/SOURCE_KEY, severity, READ_AT, correlation_id; unique dedupe key.",
            "rules": "Thiếu dữ liệu phải là Chưa xác định; polling chống request chồng; chỉ hiển thị đúng phạm vi quyền và giữ thời điểm đồng bộ cuối.",
            "output": "Badge/header, danh sách cảnh báo, trạng thái chưa đọc/đã đọc và deep-link; rollback bằng tắt rule/feature flag.",
        },
        "fpl": {
            "actor": "Hệ thống phân tích điện văn/FPL và người khai thác kiểm tra kết quả match.",
            "flow": "Parse raw → chuẩn hóa callsign/date/airport → tính cửa sổ hoặc ngày → match FPL/KHB/permission → lưu kết quả và phát cảnh báo nếu cần.",
            "components": "MessageParser, BusinessDateService, FplMatcher, PermissionQueryService, NotificationPublisher.",
            "data": "Inbox raw/normalized, FPL EOBT/ETA, FLIGHTDATE, callsign, FROM/TO, match status, rule_version.",
            "rules": "Chuẩn hóa cùng múi giờ; xử lý đúng biên 120 giờ, qua ngày và dữ liệu thiếu; không tự kết luận khi nguồn mâu thuẫn.",
            "output": "FPL được ghép/loại trừ có lý do, trạng thái NOPERM/REVIEW và log bằng chứng; rollback bằng reprocess theo ReceiveId.",
        },
        "search": {
            "actor": "Người có quyền tra cứu/export; filter phải thuộc data scope của user/đơn vị.",
            "flow": "Metadata → Filter DTO → validate range/operator → query count/page bằng bind parameter → result/export/drill-down.",
            "components": "SearchPageAdapter, FilterMetadata, QueryService, PermissionRepository, ExportWorker, DeepLinkService.",
            "data": "PERM*, T_DAY_FLIGHTS/T_FINISHED_FLIGHTS, message/airport catalog; index ngày, callsign, FROM/TO, PERM_ID và trạng thái.",
            "rules": "Whitelist field/operator; ngày bắt đầu không lớn hơn ngày kết thúc; phân trang ổn định; phạm vi lớn chuyển job nền, không SQL động từ input.",
            "output": "PagedResult, tổng số, cảnh báo, filter hash, Excel/CSV và deep-link chi tiết; rollback bằng feature flag/dual-read.",
        },
        "edit": {
            "actor": "Người có action Edit/Cancel/History và quyền trên đơn vị, chuyến hoặc phép tương ứng.",
            "flow": "Load bản mới nhất → validate input/quyền/version → hiển thị diff → transaction update hoặc staging → tính lại dữ liệu dẫn xuất → audit.",
            "components": "PageAdapter, Flight/KHBApplicationService, OptimisticVersionGuard, AuditService, NotificationPublisher.",
            "data": "Flight/KHB/permission staging, expected_version, before/after, actor, reason, status; transaction nguyên tử.",
            "rules": "Không sửa khóa/trạng thái bị khóa; conflict phải yêu cầu reload; nội dung điện văn nguồn không sửa trực tiếp; thao tác hủy phải idempotent.",
            "output": "Bản ghi mới/version, lịch sử thao tác, cảnh báo downstream và mã lỗi CONFLICT/VALIDATION; rollback theo revision hoặc staging.",
        },
        "batch": {
            "actor": "Người có quyền khởi chạy batch và worker/service account thực hiện nền.",
            "flow": "CreateBatch → lock scope → snapshot → validate chunks → process → counters → reconcile → publish → finalize; lỗi chuyển retry/quarantine.",
            "components": "BatchOrchestrator, ChunkProcessor, ProgressRepository, RetryWorker, LockService, AuditService.",
            "data": "BATCH_ID, scope_hash, snapshot_id, phase, progress, source/target counters, retry/error; lock theo ngày/phạm vi.",
            "rules": "IdempotencyKey bắt buộc; chống chạy đồng thời; chunk rollback độc lập; không báo thành công một phần nếu policy yêu cầu all-or-nothing.",
            "output": "202 + BatchId, progress, counters, error detail và summary; mục tiêu SLA theo FR; rollback bằng snapshot/replay.",
        },
        "date": {
            "actor": "Hệ thống nghiệp vụ và người duyệt xử lý ngoại lệ ngày bay.",
            "flow": "Nhận timestamp/source timezone/airport → BusinessDateService → so sánh KHB/permission/FPL/actual → gắn cờ hoặc tạo revision → thông báo.",
            "components": "BusinessDateService, FlightDateRule, FplMatcher, RevisionService, KHBValidator, NotificationRuleEngine.",
            "data": "FLIGHTDATE, EOBT/ETA/actual, timezone, date_source, calculated_date, rule_version, revision_no.",
            "rules": "Xử lý qua ngày, dấu '+', tháng/năm và cửa sổ cho phép; thiếu nguồn là Chưa xác định; không tự sửa ngày khi chưa xác nhận.",
            "output": "Ngày chuẩn, cờ sai ngày/uncertain, before-after và deep-link xử lý; rollback bằng revision/reprocess.",
        },
        "bravo": {
            "actor": "Operator/worker tích hợp HTSLB–Bravo và người xử lý conflict.",
            "flow": "Snapshot HTSLB → mapping schema → gọi Bravo API/package → nhận response → đối soát count/key → conflict queue → retry/replay.",
            "components": "BravoAdapter, MappingRegistry, ReconcileService, ConflictQueue, RetryWorker, NotificationPublisher.",
            "data": "Bravo payload/response, schema_version, idempotency_key, BATCH_ID, business key, source/target values, discrepancy.",
            "rules": "Không ghi DB Bravo trực tiếp; timeout/circuit breaker; retry chỉ lỗi tạm thời; không ghi đè Accepted ngoài policy; mọi conflict có owner.",
            "output": "INSERTED/UPDATED/SKIPPED/CONFLICT/FAILED, reconciliation snapshot và cảnh báo SLA; rollback bằng replay conflict hoặc batch snapshot.",
        },
        "report": {
            "actor": "Người khai thác báo cáo và quản trị metadata/filter theo phân quyền.",
            "flow": "Chọn metadata/filter → validate data scope → tạo snapshot → tính KPI/chart/detail → drill-down/export; anomaly phát cảnh báo.",
            "components": "ReportMetadata, ReportFilterValidator, SnapshotProvider, KPI/AnomalyService, CompareService, ExportWorker.",
            "data": "Report filter/hash, snapshot_id, T_FINISHED_FLIGHTS, KHB, ADS-B/Bravo summary, KPI và export job.",
            "rules": "KPI và chi tiết dùng cùng snapshot/filter; giới hạn khoảng ngày/số dòng; report quân sự chỉ đọc; export lớn chạy nền.",
            "output": "Bảng/biểu đồ/KPI, discrepancy/anomaly, exportId và source timestamp; rollback bằng đổi metadata/feature flag.",
        },
    }
    cards = [
        ("FR-OPT-001", "Mở rộng thời gian lưu trữ điện văn INBOX", "retention", "Inbox.aspx / chính sách retention", "Giữ và tra cứu điện văn quá phạm vi mặc định nhưng vẫn tuân retention, archive và audit."),
        ("FR-OPT-002", "Cảnh báo NOPERM cho điện văn không có trong KHBN", "notification", "Inbox/FPL analyzer và header notification", "Đối chiếu điện văn với KHBN/phép bay; chỉ cảnh báo NOPERM sau khi chuẩn hóa và không có bản ghi phù hợp."),
        ("FR-OPT-003", "Áp dụng quy tắc 120 giờ ICAO cho hiển thị FPL", "fpl", "Inbox/FPL display", "Ghép và hiển thị FPL trong cửa sổ 120 giờ, kể cả khác ngày và đúng tại biên 120 giờ."),
        ("FR-OPT-004", "Tìm kiếm chuyến bay đa trường và theo từ khóa", "search", "SearchExtension.aspx?Menu_ID=381", "Tìm theo nhiều trường/chủ đề, giữ filter khi phân trang và mở đúng chi tiết chuyến bay."),
        ("FR-OPT-005", "Tìm kiếm nâng cao phục vụ báo cáo", "search", "SearchPermissionAdv.aspx?Menu_ID=988", "Lọc SC/NO theo ngày cấp phép, giờ, FROM/TO/VIA, cột nhanh và export đủ chi tiết."),
        ("FR-OPT-006", "Phân loại chuyến bay Quốc nội/Quốc tế tự động", "search", "SearchPermissionAdv.aspx?Menu_ID=988", "Tính Domestic/International/Unknown từ danh mục sân bay và áp dụng nhất quán cho SC, NO, màn hình và Excel."),
        ("FR-OPT-007", "Export dữ liệu chia tách ba loại file ALL, LD, OF", "batch", "Màn hình export dữ liệu chuyến bay", "Tạo đồng thời ALL, LD, OF cùng snapshot, đếm dòng và checksum, không nhân đôi hoặc xếp sai nhóm."),
        ("FR-OPT-008", "Chức năng Edit info chuyến bay", "edit", "Màn hình chi tiết chuyến bay/KHB", "Cho phép sửa trường được cấp quyền, kiểm tra version và tính lại phân loại/cảnh báo/match liên quan."),
        ("FR-OPT-009", "Hiển thị lịch sử thao tác của người xử lý chuyến bay", "edit", "Chi tiết chuyến bay và audit history", "Tra cứu lịch sử theo ID ổn định, hiển thị actor, thời gian, thao tác, before/after và kết quả."),
        ("FR-OPT-010", "Đồng bộ dữ liệu điện văn liên ngày", "fpl", "Inbox/MessageFlight và job đồng bộ", "Đối chiếu điện văn qua ngày nghiệp vụ, không bỏ sót hoặc nhân đôi khi chuyển ngày."),
        ("FR-OPT-011", "Thống kê theo khung giờ, chặng và đường bay", "search", "SearchPermissionAdv.aspx?Menu_ID=988 / report query", "Tổng hợp theo giờ, chặng, FROM–TO/VIA với cùng tập dữ liệu và bộ lọc nguồn."),
        ("FR-OPT-012", "Cảnh báo chuyến bay không có trong phép bay/KHB ngày", "notification", "Header notification và danh sách chuyến", "Phát cảnh báo runtime, deep-link đến phép/KHB và không kết luận sai khi dữ liệu đối chiếu chưa đủ."),
        ("FR-OPT-013", "Hỗ trợ hủy phép bay quốc tế/quốc nội", "edit", "ImportsPermSC_LD_V2.aspx?Menu_ID=991", "Nhập/paste, chuẩn hóa, staging, kiểm tra từng dòng và hủy đúng chuyến/đoạn với trạng thái idempotent."),
        ("FR-OPT-014", "Tối ưu hiệu năng ExportBravo dưới 3 phút", "batch", "ExportBravo/BRAVO_EXPORT_PKG", "Xử lý theo tập/chunk, lock theo ngày và đạt dưới 180 giây trên bộ tải nghiệm thu."),
        ("FR-OPT-015", "Chức năng F8 ép dòng trong Calendar Accepted", "batch", "Calendar Accepted / CanlendarFlights.aspx", "Bắt phím F8, sao chép dòng tạm có kiểm soát, không ghi DB trước xác nhận và chống conflict."),
        ("FR-OPT-016", "Tối ưu Gen KHB ngày hôm sau ra AFTN dưới 3 phút", "batch", "Gen KHB ngày hôm sau/AFTN", "Sinh message D+1 theo ngày nghiệp vụ, kiểm tra KHB duyệt, outbox và delivery trong SLA."),
        ("FR-OPT-017", "Mở rộng phạm vi tra cứu INBOX trên 7 ngày", "search", "Inbox.aspx", "Cho phép khoảng ngày cấu hình lớn hơn 7 ngày, dùng index/range query, phân trang và export nền."),
        ("FR-OPT-018", "Tự động cập nhật khi sửa KHB đã build", "date", "DaylyFlight/KHB build và downstream", "Phát hiện trường ảnh hưởng, tạo revision/build lại hoặc chờ duyệt tùy trạng thái đã phát."),
        ("FR-OPT-019", "Cảnh báo chuyến bay cấp sai ngày bay so với thực tế", "notification", "Dòng chuyến và header notification", "So sánh ngày cấp với actual/FPL/điện văn, xử lý qua đêm và yêu cầu xác nhận ngoại lệ."),
        ("FR-OPT-020", "Bổ sung trường Mục đích chuyến bay trong KHB ngày", "date", "DaylyFlight.aspx/KHB ngày", "Bổ sung PURPOSE_CODE từ danh mục, truyền qua Accepted/build/export/report và audit thay đổi."),
        ("FR-OPT-021", "Logic cảnh báo đỏ chính xác cho chuyến bay hết hiệu lực", "notification", "ListPermissionNo, ListPermissionSC, Edit_PermNo, Edit_PermSC", "Tính hiệu lực theo PERMDATE/ENDDATE/VALIDHOURS và hiển thị đỏ đúng thời điểm nghiệp vụ."),
        ("FR-OPT-022", "Tự động cập nhật đường bay theo đoạn từ FPL thực tế", "date", "FPL analyzer/KHB và flight detail", "Lấy segment FROM/TO/VIA từ FPL thực tế, đối chiếu version và cập nhật dữ liệu dẫn xuất có audit."),
        ("FR-OPT-023", "Tự động nhận diện và chuyển ngày cho chuyến bay quốc tế hạ cánh hôm sau", "date", "FPL/actual flight và KHB ngày", "Nhận diện ETA dấu '+', chuyển ngày hạ cánh đúng timezone và không đổi ngày cấp khi thiếu bằng chứng."),
        ("FR-OPT-024", "Theo dõi kế hoạch bay hằng ngày với cập nhật liên tục", "notification", "ListFlightOnMess.aspx?Menu_ID=948", "Lọc chuyến bay theo điện văn, tạo change feed và hiển thị danh sách thay đổi liên tục."),
        ("FR-OPT-025", "Thông báo thời điểm nhận số liệu bay Đi/Đến từ Trung tâm TBHĐB", "notification", "Header notification/ATFM-DPLKL", "Ghi nhận thời điểm nhận số liệu Đi/Đến, phát thông báo runtime và chống trùng theo source event."),
        ("FR-OPT-026", "Cập nhật số liệu bay từ HTSLB sang Bravo", "bravo", "Bravo export/sync", "Mapping dữ liệu HTSLB sang Bravo theo schema/version, gửi theo batch và trả counters."),
        ("FR-OPT-027", "Khắc phục lỗi nhầm ngày so với thực tế", "date", "Flight/KHB/Bravo reconciliation", "Chuẩn hóa ngày nguồn, sửa logic qua ngày và tạo cờ/đề xuất điều chỉnh thay vì sửa ngầm."),
        ("FR-OPT-028", "Sửa nhiều chuyến bay cùng nội dung trong một thao tác", "edit", "Batch edit flight/KHB", "Chọn tập chuyến cùng điều kiện, preview diff, optimistic version từng dòng và commit theo policy."),
        ("FR-OPT-029", "Đối chiếu số liệu bay giữa HTSLB và Bravo", "bravo", "Bravo reconcile/report", "So sánh count, khóa chuyến, thời gian, purpose và trạng thái; đưa sai khác vào conflict queue."),
        ("FR-OPT-030", "Mở rộng tìm kiếm trên giao diện Daily Military Report", "report", "ListFinishedFlightsMilitaryReport.aspx?Menu_ID=863", "Bổ sung filter ngày, đơn vị, khu vực, trạng thái và export chỉ đọc theo data scope."),
        ("FR-OPT-031", "Thêm lựa chọn theo tất cả tiêu chí thống kê trong báo cáo", "report", "ReportNew và metadata filter", "Khai báo đầy đủ tiêu chí thống kê, kết hợp AND/OR theo metadata và bảo đảm KPI/detail/export cùng filter."),
        ("FR-OPT-032", "Đánh giá, so sánh số liệu của KHBHĐBN", "report", "Report compare KHBHĐBN", "So sánh số liệu KHBHĐBN theo kỳ/sân bay/loại chuyến, nêu chênh lệch, nguyên nhân và nguồn snapshot."),
    ]
    for index, (code, title, profile_key, screen, focus) in enumerate(cards, 1):
        profile = profiles[profile_key]
        heading(doc, f"I.10.{index}. {code} – {title}", 3)
        table(doc, ["Trường thiết kế", "Đặc tả riêng cho mã FR"], [
            ("Mục tiêu/phạm vi", focus),
            ("Màn hình/nguồn", screen),
            ("Tác nhân/điều kiện", profile["actor"]),
            ("Luồng xử lý", profile["flow"]),
            ("Component", profile["components"]),
            ("Dữ liệu/package/index", profile["data"]),
            ("Quy tắc/ngoại lệ", profile["rules"]),
            ("Đầu ra/NFR/rollback", profile["output"]),
        ])
    page_break(doc)


def add_report_detailed_appendix(doc):
    heading(doc, "PHỤ LỤC J. Thiết kế chi tiết phân hệ báo cáo và phân tích", 1)
    paragraph(doc, "Phụ lục này cụ thể hóa Mục 7 theo SRS thành một kiến trúc báo cáo dùng chung và 10 phiếu thiết kế riêng cho FR-RPT-001 đến FR-RPT-010. Mỗi báo cáo phải dùng cùng SnapshotId cho KPI, biểu đồ, bảng và drill-down; báo cáo chỉ đọc, còn thao tác sửa chuyển về màn hình nghiệp vụ có phân quyền riêng.")
    heading(doc, "J.1. Kiến trúc báo cáo dùng chung", 2)
    code_block(doc, "ReportPage → ReportMetadata → FilterValidator → SnapshotProvider → Query/KPI/Chart\n                                                ├─ DrillDownService\n                                                ├─ ExportWorker\n                                                └─ Audit + SourceStatus + Notification")
    table(doc, ["Thành phần", "Thiết kế bắt buộc", "Đầu ra"], [
        ("ReportMetadata", "Khai báo field/operator/type/default, dependency, column, công thức và quyền field", "Filter schema/version"),
        ("FilterValidator", "Kiểm tra range ngày, timezone, data scope, giới hạn dòng và tổ hợp điều kiện", "VALIDATION_ERROR hoặc FilterDTO chuẩn hóa"),
        ("SnapshotProvider", "Chọn nguồn theo kỳ, tạo SnapshotId, ghi source timestamp và schema/formula version", "SnapshotId dùng chung KPI/detail/export"),
        ("KpiCalculator", "Tính tổng, tỷ lệ, delay, airport count, ADS-B O/F và discrepancy theo snapshot", "KPI dataset + formula version"),
        ("Chart/DrillDown", "Sinh series/category, liên kết chi tiết theo cùng filter và snapshot", "Chart dataset, rows, deep-link"),
        ("ExportWorker", "Stream Excel/CSV/PDF, ghi filter/source/time/checksum và tự hết hạn file", "ExportId, progress, file"),
        ("Audit/SourceStatus", "Ghi user, query, export, nguồn trễ/thiếu và trạng thái xử lý", "Audit event, warning, source badge"),
    ])
    heading(doc, "J.2. Quy tắc dữ liệu, hiệu năng và nghiệm thu dùng chung", 2)
    table(doc, ["Nhóm", "Yêu cầu thiết kế", "Bằng chứng"], [
        ("Nhất quán", "KPI, chart, bảng và drill-down cùng SnapshotId/FilterHash; tổng chi tiết đối soát được", "Snapshot log, reconcile count"),
        ("Ngày/múi giờ", "Chuẩn hóa BusinessDate và source timezone; hiển thị rõ kỳ dữ liệu và thời điểm cập nhật", "Source timestamp, timezone test"),
        ("Hiệu năng", "P95 truy vấn thông thường ≤3 giây; khoảng lớn/export chuyển worker nền", "query_latency, export_elapsed"),
        ("Nguồn thiếu/trễ", "Hiển thị SourceStatus; không âm thầm dùng snapshot cũ hoặc gộp NULL thành 0", "Warning/source badge"),
        ("Phân quyền", "Role/action/data scope ở query, drill-down và export; báo cáo quân sự chỉ đọc", "Denied action/audit"),
        ("Công thức", "Version hóa công thức KPI/anomaly; export ghi formula_version", "Metadata/config history"),
    ])
    heading(doc, "J.3. Phiếu thiết kế riêng theo từng mã FR-RPT", 2)
    report_cards = [
        ("FR-RPT-001", "Biểu đồ thông tin tổng quan khai thác bay", "http://localhost/ATFM_WEB/ReportNew/FlightOperationOverview.aspx?Menu_ID=908", "Quản lý/khai thác theo ngày, sân bay, hãng, loại chuyến và trạng thái.", "T_DAY_FLIGHTS_GOINGON/T_FINISHED_FLIGHTS, airport/airline dimension; snapshot theo kỳ.", "Tổng chuyến, đi/đến, quốc nội/quốc tế, theo sân bay/hãng; tỷ lệ và biến động so với kỳ trước.", "KPI cards + line/bar/donut; drill-down vào danh sách finished flight; export Excel/CSV.", "Refresh theo kỳ cấu hình; thiếu nguồn hiển thị badge; P95 ≤3 giây; chỉ đọc và audit export."),
        ("FR-RPT-002", "Phân tích xu hướng khai thác theo thời gian", "http://localhost/ATFM_WEB/ReportNew/FlightTrendAnalysis.aspx?Menu_ID=909", "Phân tích xu hướng theo ngày/tuần/tháng, sân bay, hãng, loại chuyến và khung giờ.", "Finished flight snapshot, BusinessDate, airport/airline/status dimensions.", "Chuỗi tổng chuyến, moving/period comparison, peak/off-peak và tỷ lệ thay đổi; công thức version hóa.", "Line/area chart, chọn khoảng và granularity, drill-down ngày; export nền theo FilterHash.", "Không trộn timezone; khoảng lớn chạy nền; snapshot hết hạn yêu cầu tạo lại; audit bộ lọc."),
        ("FR-RPT-003", "Phát hiện và cảnh báo dữ liệu bất thường", "http://localhost/ATFM_WEB/ReportNew/AnomalyWarning.aspx?Menu_ID=910", "Phát hiện KPI, tỷ lệ hoặc bản ghi vượt ngưỡng cấu hình và hỗ trợ xử lý cảnh báo.", "KPI snapshot, notification/anomaly store, flight/message/batch context.", "Rule/threshold theo sân bay, ngày, loại chuyến; NORMAL/WARN/CRITICAL; xác định nguyên nhân ứng viên.", "Bảng cảnh báo + severity/filter + deep-link chuyến/batch; acknowledge/resolve; export evidence.", "Dedupe theo rule/window/source; dữ liệu thiếu là REVIEW; threshold và thao tác phải audit."),
        ("FR-RPT-004", "Tích hợp ADS-B cho báo cáo khai thác thực tế", "ADS-B snapshot/API và báo cáo khai thác thực tế; liên kết AdsBPerformanceReport.aspx?Menu_ID=923", "Bổ sung actual flight/tracks vào báo cáo, đối chiếu KHB/FPL và phân biệt chất lượng nguồn.", "T_TRACKS_LOG, ADS-B snapshot, matcher với KHB/FPL/finished flight, source quality.", "Actual departure/arrival, track coverage, match rate, delay và sai khác kế hoạch–thực tế.", "Bản đồ/series/KPI + drill-down track/flight; hiển thị SourceStatus; export snapshot có schema version.", "Không dùng raw full scan; tổng hợp theo ngày/sân bay/FIR; ADS-B trễ hoặc lỗi chuyển REVIEW, không tự coi là zero."),
        ("FR-RPT-005", "Tổng hợp chỉ số hiệu suất bay từ ADS-B O/F", "http://localhost/ATFM_WEB/ReportNew/AdsBPerformanceReport.aspx?Menu_ID=923", "Tính chỉ số hiệu suất từ cất/hạ cánh ADS-B O/F theo kỳ và chiều khai thác.", "ADS-B O/F, T_TRACKS_LOG, KHB/FPL và airport dimension; snapshot actual.", "Count O/F, on-time/delay, thời gian thực tế, coverage, match rate và tỷ lệ thiếu bản ghi.", "KPI + bar/line theo sân bay/hãng/khung giờ; drill-down O/F; export ghi nguồn ADS-B và công thức.", "Chỉ tính bản ghi QUALITY_STATUS hợp lệ; không chia cho mẫu rỗng; timeout/export nền và audit."),
        ("FR-RPT-006", "Biểu đồ thống kê trạng thái chuyến bay theo tỷ lệ phần trăm", "http://localhost/ATFM_WEB/ReportNew/FlightStatusRate.aspx?Menu_ID=907", "Hiển thị tỷ lệ từng trạng thái chuyến bay trên cùng tập lọc.", "Finished flight snapshot, status catalog, airport/airline/date filters.", "Tỷ lệ = số chuyến trạng thái / tổng chuyến hợp lệ; tổng tỷ lệ phải kiểm tra sai số làm tròn.", "Donut/stacked bar + bảng số tuyệt đối/tỷ lệ; click status mở danh sách chi tiết; export.", "Không tính bản ghi NULL vào mẫu nếu policy; ghi rõ denominator; filter/detail dùng cùng snapshot."),
        ("FR-RPT-007", "Biểu đồ so sánh hoạt động bay giữa các sân bay", "http://localhost/ATFM_WEB/Common/ChartReportAirport.aspx?Menu_ID=906", "So sánh lưu lượng và chỉ số khai thác giữa nhiều sân bay.", "Airport dimension, finished flights, ADS-B/Bravo summary nếu chọn; snapshot kỳ.", "Tổng đi/đến, quốc nội/quốc tế, delay và chênh lệch theo sân bay; chuẩn hóa mã ICAO.", "Grouped bar/heatmap/ranking; drill-down từng sân bay; export giữ thứ tự/ranking.", "Không so sánh khác kỳ nếu chưa cảnh báo; sân bay thiếu dữ liệu hiển thị Unknown; giới hạn số sân bay."),
        ("FR-RPT-008", "Báo cáo chuyến bay quân sự theo tiêu chí mở rộng", "http://localhost/ATFM_WEB/ReportNew/MilitaryFlightReport.aspx?Menu_ID=911", "Tra cứu và tổng hợp KHB quân sự/flight đã Accepted theo tiêu chí mở rộng.", "Military KHB, finished flights Accepted, unit/area/purpose/status dimension.", "Tổng theo đơn vị, khu vực, mục đích, ngày, trạng thái; phân biệt draft/Accepted/Dispatched.", "Bảng + biểu đồ + filter ngày/đơn vị/khu vực/purpose; drill-down readonly; export theo scope.", "Không hiển thị bản nháp ngoài quyền; dữ liệu Accepted khóa; audit truy vấn/export và masking trường nhạy cảm."),
        ("FR-RPT-009", "Báo cáo tổng hợp hoạt động bay tại tất cả sân bay dân dụng", "http://localhost/ATFM_WEB/ReportNew/CivilFlightSummary.aspx?Menu_ID=912", "Tổng hợp hoạt động khai thác dân dụng toàn mạng lưới sân bay.", "Civil airport dimension, finished flights, status/airline/route and optional ADS-B summary.", "Tổng chuyến, đi/đến, hãng, route, quốc nội/quốc tế và biến động theo sân bay.", "Bảng tổng hợp + ranking/chart; drill-down sân bay/route; export toàn quốc chạy nền.", "Chuẩn hóa loại civil; không nhân đôi multi-leg; kiểm tra count toàn quốc và từng sân bay; snapshot bắt buộc."),
        ("FR-RPT-010", "Báo cáo cất, hạ cánh tại các sân bay toàn quốc", "http://localhost/ATFM_WEB/ReportNew/AirportTakeoffLanding.aspx?Menu_ID=913", "Theo dõi số liệu takeoff/landing toàn quốc theo ngày, giờ và sân bay.", "Finished actual, ADS-B O/F, airport dimension, event/status source; snapshot actual.", "Số cất cánh/hạ cánh, tỷ lệ theo giờ/sân bay, sai khác O/F và tổng đối soát.", "Map/table/time-series; drill-down event/flight; export theo kỳ và source status.", "Phân biệt takeoff/landing và O/F; timezone sân bay; sự kiện thiếu gắn REVIEW; SLA query/export và audit."),
    ]
    for index, (code, title, screen, actor_scope, source, kpi, visual, controls) in enumerate(report_cards, 1):
        heading(doc, f"J.3.{index}. {code} – {title}", 3)
        table(doc, ["Trường thiết kế", "Đặc tả riêng cho báo cáo"], [
            ("Mục tiêu/phạm vi", actor_scope),
            ("Màn hình/URL", screen),
            ("Nguồn dữ liệu/snapshot", source),
            ("KPI/công thức", kpi),
            ("Biểu đồ/bảng/drill-down/export", visual),
            ("Làm mới, quy tắc, phân quyền và NFR", controls),
        ])
    page_break(doc)


def add_ai_detailed_appendix(doc):
    heading(doc, "PHỤ LỤC K. Thiết kế chi tiết phân hệ AI hỗ trợ thống kê, tìm kiếm và tổng hợp", 1)
    paragraph(doc, "Phụ lục này cụ thể hóa Mục 8 theo SRS cho FR-AI-001 đến FR-AI-004. AI được triển khai như dịch vụ độc lập, chỉ truy vấn dữ liệu được cấp phép bằng tài khoản Oracle read-only; mọi câu hỏi, SQL, kết quả, phiên bản model/prompt và thao tác người dùng phải có thể truy vết.")
    heading(doc, "K.1. Kiến trúc AI dùng chung", 2)
    code_block(doc, "Question/Voice → Session/Auth → Intent/Context Builder → LLM Draft\n                              → SQL Parser/Allowlist/Policy → Preview/Approval\n                              → OracleRunner(read-only) → Result Normalizer\n                              → Table/Chart/Summary → Audit/Feedback/Monitoring")
    table(doc, ["Thành phần", "Thiết kế", "Kiểm soát"], [
        ("Chat API", "Nhận câu hỏi, session, scope và trả intent/SQL/kết quả", "HTTPS, token/CSRF, rate limit, timeout"),
        ("Intent/Context Builder", "Nhận diện ngày, hãng, sân bay, loại chuyến, metric và bổ sung schema nghiệp vụ", "Không cho model tự chọn bảng/cột ngoài metadata"),
        ("LLM/Prompt Store", "Quản lý model, system prompt, few-shot, training sample và version", "Review/approval, không chứa secret, rollback version"),
        ("SQL Guard", "Parse Oracle SQL, chặn DML/DDL/multi-statement/SELECT ngoài allowlist", "Bind parameter, row/time limit, policy decision"),
        ("OracleRunner", "Thực thi SELECT bằng read-only account và trả snapshot/result metadata", "Circuit breaker, kill timeout, data scope"),
        ("Result Renderer", "Chuẩn hóa bảng, biểu đồ, tổng hợp và giải thích nguồn", "Mask dữ liệu nhạy cảm, không suy diễn khi NULL"),
        ("AI Monitor", "Đo latency, token/cost, lỗi, chất lượng, usage và health", "Dashboard, alert, audit và retention"),
    ])
    heading(doc, "K.2. Vòng đời yêu cầu AI và chính sách an toàn", 2)
    table(doc, ["Bước", "Xử lý", "Trạng thái/bằng chứng"], [
        ("Receive", "Nhận câu hỏi tiếng Việt/giọng nói, user/session/scope", "RECEIVED, request_id"),
        ("Understand", "Nhận diện intent, thực thể, kỳ dữ liệu và câu hỏi thiếu", "UNDERSTOOD/CLARIFY"),
        ("Draft", "Sinh SQL hoặc kế hoạch tổng hợp theo schema version", "DRAFT, prompt/model version"),
        ("Guard", "Parse, allowlist, bind, giới hạn dòng/thời gian và kiểm tra scope", "APPROVED/REJECTED, policy reason"),
        ("Execute", "Oracle read-only hoặc chuyển export nền nếu kết quả lớn", "RUNNING/COMPLETED/FAILED"),
        ("Present", "Trình bày bảng/biểu đồ/tóm tắt và nguồn dữ liệu", "PRESENTED, snapshot_id"),
        ("Learn/Monitor", "Phản hồi người dùng, đánh giá chất lượng và metric vận hành", "FEEDBACK/REVIEWED/AUDITED"),
    ])
    heading(doc, "K.3. NFR và kiểm soát dùng chung", 2)
    table(doc, ["Nhóm", "Yêu cầu", "Bằng chứng"], [
        ("Bảo mật", "RBAC/data scope ở API và Oracle; không nhận CREATED_BY/scope từ payload không tin cậy", "Denied action, read-only account, audit"),
        ("An toàn SQL", "Một SELECT hợp lệ; cấm DML/DDL/lock/INTO/SELECT * sản xuất; bind tham số", "SQL Guard decision, blocked query log"),
        ("Đúng dữ liệu", "Hiển thị snapshot, source timestamp, công thức và cảnh báo dữ liệu thiếu/trễ", "Result metadata, formula version"),
        ("Hiệu năng", "P95 câu hỏi thông thường theo SLA; query lớn chuyển export nền; LLM timeout có giới hạn retry", "Latency/token/timeout metrics"),
        ("Riêng tư", "Mask nội dung nhạy cảm, không log secret/prompt chứa dữ liệu cá nhân ngoài chính sách", "Log review, retention/masking test"),
        ("Rollback", "Có thể tắt model/prompt/route bằng feature flag và quay về bản đã phê duyệt", "Version registry, rollback runbook"),
    ])
    heading(doc, "K.4. Phiếu thiết kế riêng theo từng mã FR-AI", 2)
    ai_cards = [
        ("FR-AI-001", "Xử lý ngôn ngữ tự nhiên cho truy vấn hàng không", "Nhận câu hỏi tiếng Việt/giọng nói, nhận diện ý định, thực thể, kỳ dữ liệu và sinh Oracle SQL có ngữ cảnh schema.", "Chat API, Intent/Context Builder, Prompt Store, LLM, SQL Guard.", "Question, user scope, schema_version, intent, entities, draft_sql, confidence, clarification.", "Câu hỏi mơ hồ phải hỏi lại; tên bảng/cột ngoài allowlist bị từ chối; SQL không được thực thi trước Guard/Preview; lưu model/prompt/version."),
        ("FR-AI-002", "Truy vấn và tổng hợp dữ liệu tự động theo yêu cầu", "Thực thi truy vấn đọc an toàn, chuẩn hóa dữ liệu, tính tổng hợp và trình bày bảng/biểu đồ theo câu hỏi.", "SQL Guard, OracleRunner, SnapshotProvider, ResultNormalizer, KPI/Chart Renderer.", "Approved SQL, bind params, snapshot_id, columns/rows, row_count, KPI formula, source timestamp.", "Giới hạn dòng/thời gian; kết quả lớn chuyển export; NULL/thiếu nguồn phải hiển thị rõ; KPI và chi tiết cùng snapshot."),
        ("FR-AI-003", "Tích hợp Trợ lý ảo (Chatbot) vào giao diện HTSLB", "Cung cấp giao diện chat/API thời gian thực, quản lý phiên hội thoại, phản hồi và training sample có kiểm duyệt.", "Chat UI/handler, Chat API, SessionStore, FeedbackStore, TrainingApprovalService, Notification/RateLimiter.", "SessionId, user/role/scope, conversation turns, message status, feedback, prompt/model version.", "Phiên hết hạn phải yêu cầu đăng nhập lại; không lộ SQL/secret; feedback chưa duyệt không đưa vào training; streaming phải có cancel/timeout."),
        ("FR-AI-004", "Báo cáo và giám sát hiệu năng của Trợ lý ảo", "Theo dõi hoạt động, audit, chất lượng, sức khỏe dịch vụ, hiệu năng truy vấn và chi phí sử dụng AI.", "AI Monitor, UsageRepository, AuditService, HealthProbe, Dashboard/Report API, AlertPublisher.", "Request/response latency, token/cost, model, prompt, SQL policy, error, row_count, user, feedback score, health state.", "Tách metric kỹ thuật và chất lượng; cảnh báo LLM/Oracle timeout, SQL reject, usage tăng bất thường; dữ liệu giám sát phải mask và có retention."),
    ]
    for index, (code, title, objective, components, data, rules) in enumerate(ai_cards, 1):
        heading(doc, f"K.4.{index}. {code} – {title}", 3)
        table(doc, ["Trường thiết kế", "Đặc tả riêng cho mã FR"], [
            ("Mục tiêu/phạm vi", objective),
            ("Thành phần", components),
            ("Dữ liệu/API", data),
            ("Luồng chính", "Receive → Understand → Draft/Guard → Execute → Present; với FR-AI-003 bổ sung Session/Chat/Feedback, với FR-AI-004 bổ sung Collect → Evaluate → Alert."),
            ("Quy tắc/ngoại lệ", rules),
            ("Đầu ra/NFR/rollback", "Kết quả có request/correlation/snapshot hoặc health/metric tương ứng; audit đầy đủ; retry có giới hạn; tắt feature/model và quay về version trước khi phát hiện lỗi."),
        ])
    heading(doc, "K.5. API, dữ liệu và kiểm thử AI", 2)
    table(doc, ["Mã", "Endpoint/bằng chứng", "Kiểm tra tối thiểu"], [
        ("AI-API-01", "/api/ai/session, /api/ai/query/preview", "Scope, intent, SQL masked, clarification, policy decision"),
        ("AI-API-02", "/api/ai/query/execute, /api/ai/export", "Read-only, bind parameter, row/time limit, snapshot và export nền"),
        ("AI-API-03", "/api/ai/feedback, /api/ai/admin/training", "Approval, version, rollback và không training từ mẫu chưa duyệt"),
        ("AI-OPS-01", "Dashboard/health/audit/alert", "Latency, timeout, reject, quality score, usage/cost và source status"),
        ("AI-TC-01", "Bộ câu hỏi tiếng Việt/giọng nói và dữ liệu biên", "Đúng intent, hỏi lại khi mơ hồ, không hallucinate dữ liệu"),
        ("AI-TC-02", "SQL độc hại, ngoài allowlist, query quá lớn", "Reject/preview, audit, không ghi dữ liệu, không lộ secret"),
        ("AI-TC-03", "Oracle/LLM timeout, mất session, nguồn thiếu", "Retry/circuit breaker, thông báo rõ, khôi phục hoặc rollback đúng trạng thái"),
    ])
    page_break(doc)


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
    add_opt_detailed_appendix(doc)
    add_report_detailed_appendix(doc)
    add_ai_detailed_appendix(doc)
    for item in doc.sections:
        add_page_number(item)
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    doc.save(OUTPUT)
    print(OUTPUT)


if __name__ == "__main__":
    build()
