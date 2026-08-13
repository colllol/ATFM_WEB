<%@ Page Title="Cancel Permission SC V2" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
    AutoEventWireup="true" CodeBehind="ImportsPermSC_LD_V2.aspx.cs"
    Inherits="prjApplication.Tool.ImportsPermSC_LD_V2" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Tool/ImportsPermSC_LD_V2.css?v=20260812-1") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Tool/ImportsPermSC_LD_V2.fix.css?v=20260812-4") %>" />

    <section class="impv2" data-page-method-url="ImportsPermSC_LD_V2.aspx/">
        <header class="impv2__header">
            <div>
                <h2>HỦY CHUYẾN PERMISSION SC V2</h2>
                <p>Phân tích, kiểm tra và xem trước danh sách chuyến bay cần hủy trước khi ghi nhận.</p>
            </div>
            <span class="impv2__legacy">Chức năng cũ vẫn được giữ nguyên</span>
        </header>

        <nav class="impv2__steps" aria-label="Các bước import">
            <span class="is-active" data-step-indicator="1"><b>1</b> Thông tin</span>
            <span data-step-indicator="2"><b>2</b> Kiểm tra dữ liệu</span>
            <span data-step-indicator="3"><b>3</b> Xác nhận xử lý</span>
        </nav>

        <div id="impv2Message" class="impv2__message" role="alert" hidden></div>

        <section class="impv2__panel">
            <h3>1. Thông tin import</h3>
            <div class="impv2__form-grid">
                <label class="impv2__format-field">Format
                    <select id="impFormat">
                        <option value="AUTO">TỰ NHẬN DIỆN — KHUYÊN DÙNG</option>
                        <option value="WITH_CRAFT">MẪU HỦY CÓ LOẠI TÀU BAY</option>
                        <option value="WITHOUT_CRAFT">MẪU HỦY KHÔNG CÓ LOẠI TÀU BAY</option>
                        <option value="NORMALIZED">DỮ LIỆU ĐÃ CHUẨN HÓA ATFM</option>
                    </select>
                    <button id="btnViewFormat" type="button" class="impv2__format-help">? XEM FORMAT CHUẨN</button>
                </label>
                <label>Number
                    <input id="impPermNbr" maxlength="8" autocomplete="off" />
                </label>
                <label>Ngày cấp phép
                    <input id="impPermDate" type="date" />
                </label>
                <label>Author
                    <asp:DropDownList ID="ddlAuthorV2" runat="server" ClientIDMode="Static" />
                </label>
                <label>Version
                    <input id="impVersion" maxlength="1" value="A" />
                </label>
                <label>Season
                    <select id="impSeason"><option value="W">WINTER</option><option value="S">SUMMER</option></select>
                </label>
                <label>Purpose
                    <asp:DropDownList ID="ddlPurposeV2" runat="server" ClientIDMode="Static" />
                </label>
                <label>Flight type
                    <select id="impFlightType"><option value="SC">SC</option><option value="NO">NO</option></select>
                </label>
                <label>Craft mặc định
                    <input id="impDefaultCraft" maxlength="12" />
                </label>
                <label>Registration
                    <input id="impRegistration" maxlength="30" />
                </label>
            </div>
        </section>

        <section class="impv2__panel">
            <div class="impv2__panel-title">
                <h3>2. Dữ liệu nguồn</h3>
                <div class="impv2__source-actions" style="display:none" aria-hidden="true">
                    <label class="impv2__file-button">CHỌN FILE TXT/CSV
                        <input id="impFile" type="file" accept=".txt,.csv,.tsv" hidden />
                    </label>
                </div>
            </div>
            <div class="impv2__source-grid">
                <label>Dán bảng lịch bay từ Word/Excel
                    <textarea id="impSource" spellcheck="false" placeholder="Sao chép cả dòng tiêu đề và các dòng chuyến bay tại mục Schedules, sau đó dán vào đây bằng Ctrl+V."></textarea>
                    <small>Khuyến nghị chỉ sao chép bảng Schedules; không cần phần Carrier, Applicant hoặc Note.</small>
                </label>
                <label>VIA/Route mặc định
                    <textarea id="impDefaultVia" spellcheck="false" placeholder="Route mặc định hoặc bảng route theo chặng"></textarea>
                </label>
            </div>
            <label>Remark mặc định
                <input id="impDefaultRemark" maxlength="1000" />
            </label>
            <div class="impv2__primary-actions">
                <button id="btnAnalyze" type="button">PHÂN TÍCH &amp; KIỂM TRA</button>
                <button id="btnClearSource" type="button" class="is-muted">XÓA NỘI DUNG</button>
            </div>
        </section>

        <section id="previewPanel" class="impv2__panel" hidden>
            <div class="impv2__panel-title">
                <h3>3. Preview</h3>
                <div class="impv2__summary">
                    <span>Tổng <b id="sumTotal">0</b></span>
                    <span class="is-valid">Hợp lệ <b id="sumValid">0</b></span>
                    <span class="is-warning">Cảnh báo <b id="sumWarning">0</b></span>
                    <span class="is-error">Lỗi <b id="sumError">0</b></span>
                </div>
            </div>
            <div class="impv2__filters">
                <button type="button" data-status-filter="ALL" class="is-active">TẤT CẢ</button>
                <button type="button" data-status-filter="VALID">HỢP LỆ</button>
                <button type="button" data-status-filter="WARNING">CẢNH BÁO</button>
                <button type="button" data-status-filter="ERROR">LỖI</button>
                <input id="previewSearch" placeholder="Tìm Callsign, sân bay, lỗi..." />
            </div>
            <div class="impv2__table-wrap">
                <table id="previewTable">
                    <thead><tr>
                        <th><input id="previewCheckAll" type="checkbox" checked /></th>
                        <th>Dòng</th><th>Trạng thái</th><th>Callsign</th><th>Từ ngày</th><th>Đến ngày</th>
                        <th>Daily</th><th>Craft</th><th>From</th><th>To</th><th>ETD</th><th>ETA</th>
                        <th>VIA</th><th>Remark/Lỗi</th>
                    </tr></thead>
                    <tbody></tbody>
                </table>
            </div>
            <div class="impv2__confirm-box" id="cancelConfirmBox">
                <strong>Cảnh báo HỦY CHUYẾN</strong>
                <span>Thao tác mặc định là HỦY CHUYẾN. Chỉ các dòng được chọn mới được đưa vào staging; hãng khai thác được tự xác định từ Callsign bằng GetOper.</span>
            </div>
            <div class="impv2__primary-actions">
                <button id="btnImportSelected" type="button">ĐƯA CÁC DÒNG ĐÃ CHỌN VÀO DANH SÁCH HỦY</button>
            </div>
        </section>

        <section id="resultPanel" class="impv2__panel" hidden>
            <h3>Kết quả xử lý</h3>
            <div id="importResult" class="impv2__result-text"></div>
        </section>

        <section id="confirmPanel" class="impv2__panel" hidden>
            <h3>3. Xác nhận xử lý</h3>
            <h4>DANH SÁCH CHUYẾN ĐÃ IMPORT THÀNH CÔNG</h4>
            <div class="impv2__table-wrap"><table id="importedTable"><thead><tr>
                <th>NO</th><th>CALLSIGN</th><th>FROM DATE</th><th>TO DATE</th><th>DAILY</th>
                <th>CRAFT</th><th>FROM</th><th>TO</th><th>ETD</th><th>ETA</th><th>VIA</th><th>REMARK</th>
            </tr></thead><tbody></tbody></table></div>
            <h4>DANH SÁCH CHUYẾN BAY BỊ HỦY</h4>
            <div class="impv2__table-wrap"><table id="cancelledTable"><thead><tr>
                <th>NO</th><th>CALLSIGN</th><th>PERM</th><th>BEGIN</th><th>END</th><th>FROM</th><th>TO</th>
                <th>DAILY</th><th>ETD</th><th>ETA</th><th>OPER</th><th>TYPE</th><th>REMARK</th><th>PURPOSE</th>
                <th>HỦY</th>
            </tr></thead><tbody></tbody></table></div>
            <div class="impv2__primary-actions">
                <button id="btnDeleteCancellation" type="button" class="is-danger">DELETE HỦY CHUYẾN</button>
                <button id="btnApplyCancellation" type="button">XÁC NHẬN HỦY CHUYẾN</button>
            </div>
        </section>

        <div id="formatGuide" class="impv2__guide" hidden aria-hidden="true" role="dialog" aria-modal="true" aria-labelledby="formatGuideTitle">
            <div class="impv2__guide-card">
                <div class="impv2__guide-header">
                    <div>
                        <h3 id="formatGuideTitle">FORMAT DỮ LIỆU CHUẨN</h3>
                        <span id="formatGuideName"></span>
                    </div>
                    <button id="btnCloseFormat" type="button" aria-label="Đóng">×</button>
                </div>
                <p id="formatGuideDescription"></p>
                <div class="impv2__guide-note"><b>Khuyến nghị:</b> sao chép nguyên bảng Schedules từ Word/Excel, gồm dòng tiêu đề và toàn bộ các dòng chuyến bay. Hệ thống giữ từng ô và tự ánh xạ theo tên cột.</div>
                <h4>Thứ tự cột</h4>
                <div id="formatGuideColumns" class="impv2__guide-columns"></div>
                <h4>Dòng mẫu có thể sao chép</h4>
                <pre id="formatGuideExample"></pre>
                <ul id="formatGuideRules"></ul>
                <div class="impv2__guide-actions">
                    <button id="btnCopyFormat" type="button">SAO CHÉP DÒNG MẪU</button>
                    <button id="btnCloseFormatBottom" type="button" class="is-muted">ĐÓNG</button>
                </div>
            </div>
        </div>

        <div id="impv2Loading" class="impv2__loading" hidden>
            <div class="impv2__spinner"></div><span>Đang xử lý dữ liệu...</span>
        </div>
    </section>

    <script src="<%= ResolveUrl("~/Tool/ImportsPermSC_LD_V2.js?v=20260813-5") %>"></script>
</asp:Content>
