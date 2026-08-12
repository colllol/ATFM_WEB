<%@ Page Title="Import Permission SC V2" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
    AutoEventWireup="true" CodeBehind="ImportsPermSC_LD_V2.aspx.cs"
    Inherits="prjApplication.Tool.ImportsPermSC_LD_V2" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Tool/ImportsPermSC_LD_V2.css?v=20260812-1") %>" />

    <section class="impv2" data-page-method-url="ImportsPermSC_LD_V2.aspx/">
        <header class="impv2__header">
            <div>
                <h2>IMPORT PERMISSION SC V2</h2>
                <p>Phân tích, kiểm tra và xem trước dữ liệu trước khi ghi nhận.</p>
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
                <label>Thao tác
                    <select id="impAction">
                        <option value="TangChuyen">TĂNG CHUYẾN</option>
                        <option value="HuyChuyen">HỦY CHUYẾN</option>
                        <option value="ThayDoi">THAY ĐỔI</option>
                    </select>
                </label>
                <label>Format
                    <select id="impFormat">
                        <option value="AUTO">TỰ NHẬN DIỆN</option>
                        <option value="STANDARD">CHUẨN ATFM</option>
                        <option value="HVN">HVN LEGACY</option>
                        <option value="ALL_OPER">TAB/SPACE CHUNG</option>
                    </select>
                </label>
                <label>Hãng khai thác
                    <select id="impOper">
                        <option value="HVN">HVN</option>
                        <option value="PIC">PIC</option>
                        <option value="VJC">VJC</option>
                        <option value="VFC">VFC</option>
                        <option value="ABW">ABW</option>
                        <option value="BAV">BAV</option>
                        <option value="ALL_OPER">ALL OPER</option>
                    </select>
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
                <div class="impv2__source-actions">
                    <label class="impv2__file-button">CHỌN FILE TXT/CSV
                        <input id="impFile" type="file" accept=".txt,.csv,.tsv" hidden />
                    </label>
                    <button id="btnClearSource" type="button" class="is-muted">XÓA NỘI DUNG</button>
                </div>
            </div>
            <div class="impv2__source-grid">
                <label>Nội dung chuyến bay
                    <textarea id="impSource" spellcheck="false" placeholder="Dán dữ liệu chuyến bay vào đây..."></textarea>
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
            <div class="impv2__confirm-box" id="cancelConfirmBox" hidden>
                <strong>Cảnh báo HỦY CHUYẾN</strong>
                <span>Chỉ các dòng được chọn mới được đưa vào staging. Hãy kiểm tra kỹ kết quả đối chiếu trước khi thực hiện lệnh hủy.</span>
                <label>Nhập <b>HUY CHUYEN</b> để xác nhận
                    <input id="cancelConfirmText" autocomplete="off" />
                </label>
            </div>
            <div class="impv2__primary-actions">
                <button id="btnImportSelected" type="button">IMPORT CÁC DÒNG ĐÃ CHỌN</button>
            </div>
        </section>

        <section id="resultPanel" class="impv2__panel" hidden>
            <h3>Kết quả xử lý</h3>
            <pre id="importResult"></pre>
        </section>

        <div id="impv2Loading" class="impv2__loading" hidden>
            <div class="impv2__spinner"></div><span>Đang xử lý dữ liệu...</span>
        </div>
    </section>

    <script src="<%= ResolveUrl("~/Tool/ImportsPermSC_LD_V2.js?v=20260812-1") %>"></script>
</asp:Content>
