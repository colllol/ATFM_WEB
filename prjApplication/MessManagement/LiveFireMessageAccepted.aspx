<%@ Page Title="Live Fire Message Approval" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
    AutoEventWireup="true" CodeBehind="LiveFireMessageAccepted.aspx.cs"
    Inherits="prjApplication.MessManagement.LiveFireMessageAccepted" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/MessManagement/LiveFireMessage.css?v=20260809-2") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/MessManagement/LiveFireMessageAccepted.css?v=20260809-1") %>" />

    <section class="lf-app">
        <div class="lf-toolbar">
            <label>TỪ NGÀY<input id="lfaFromDate" type="date" /></label>
            <label>ĐẾN NGÀY<input id="lfaToDate" type="date" /></label>
            <label>TRẠNG THÁI
                <select id="lfaStatus">
                    <option value="2">CHỜ DUYỆT</option>
                    <option value="1">ĐÃ DUYỆT</option>
                    <option value="4">ĐÃ EXPORT</option>
                    <option value="-3">TẤT CẢ TRẠNG THÁI DUYỆT</option>
                </select>
            </label>
            <label class="lf-keyword">TỪ KHÓA
                <input id="lfaKeyword" type="text" maxlength="200"
                    placeholder="Mã, tiêu đề, địa điểm, người nhập" />
            </label>
            <label>SỐ DÒNG
                <select id="lfaPageSize">
                    <option value="20">20</option>
                    <option value="50" selected="selected">50</option>
                    <option value="100">100</option>
                </select>
            </label>
            <button id="lfaSearch" type="button" class="lf-btn lf-primary">TÌM KIẾM</button>
        </div>

        <div class="lf-summary">
            <span id="lfaTotal">TỔNG SỐ: 0</span>
            <span class="lf-status-help">Màn hình duyệt: Chờ duyệt, Đã duyệt và Đã export</span>
        </div>
        <div class="lf-grid-wrap">
            <table id="lfaGrid" class="lf-grid" data-page="1" data-total="0">
                <thead><tr>
                    <th>NO</th><th>NGÀY ĐIỆN VĂN</th><th>MÃ</th><th>TIÊU ĐỀ</th>
                    <th>ĐỊA ĐIỂM BẮN</th><th>TRẠNG THÁI</th><th>NGƯỜI NHẬP</th>
                    <th>NGƯỜI DUYỆT</th><th>THAO TÁC</th>
                </tr></thead>
                <tbody><tr><td colspan="9" class="lf-empty">Đang tải dữ liệu...</td></tr></tbody>
            </table>
        </div>
        <div class="lf-pager">
            <button id="lfaPrev" type="button">&lt;</button>
            <strong id="lfaPageLabel">Trang 1/1</strong>
            <button id="lfaNext" type="button">&gt;</button>
        </div>
    </section>

    <div id="lfaEditor" class="lf-modal" aria-hidden="true">
        <div class="lf-modal-dialog lfa-dialog" role="dialog" aria-modal="true" aria-labelledby="lfaEditorTitle">
            <header>
                <h2 id="lfaEditorTitle">Duyệt điện văn bắn đạn thật</h2>
                <button id="lfaClose" type="button" class="lf-close" aria-label="Đóng">×</button>
            </header>
            <div class="lf-editor-body">
                <input id="lfaId" type="hidden" value="0" />
                <input id="lfaVersion" type="hidden" value="0" />
                <input id="lfaCurrentStatus" type="hidden" value="2" />

                <div class="lfa-meta">
                    <div><b>Ngày điện văn</b><span id="lfaMessageDate"></span></div>
                    <div><b>Mã điện văn</b><span id="lfaMessageCode"></span></div>
                    <div><b>Tiêu đề</b><span id="lfaSubject"></span></div>
                    <div><b>Trạng thái</b><span id="lfaStatusText"></span></div>
                    <div class="lfa-wide"><b>Địa điểm bắn</b><span id="lfaLocation"></span></div>
                    <div class="lfa-wide"><b>Tọa độ</b><pre id="lfaCoordinates"></pre></div>
                    <div><b>Phương vị</b><span id="lfaDirection"></span></div>
                    <div><b>Độ cao đường đạn</b><span id="lfaHeight"></span></div>
                    <div><b>Cự ly đường đạn</b><span id="lfaRange"></span></div>
                    <div class="lfa-wide"><b>Thời gian bắn</b><pre id="lfaSchedules"></pre></div>
                    <div><b>Người nhập</b><span id="lfaCreatedBy"></span></div>
                    <div><b>Người gửi duyệt</b><span id="lfaSubmittedBy"></span></div>
                    <div><b>Người duyệt</b><span id="lfaApprovedBy"></span></div>
                    <div><b>Người export</b><span id="lfaExportedBy"></span></div>
                </div>

                <fieldset>
                    <legend>Nội dung điện văn</legend>
                    <textarea id="lfaPreview" rows="20" readonly="readonly"></textarea>
                    <div id="lfaRejectInfo" class="lf-reject-info"></div>
                </fieldset>
            </div>
            <footer>
                <button id="lfaApprove" type="button" class="lf-btn lf-success">DUYỆT</button>
                <button id="lfaReject" type="button" class="lf-btn lf-danger">TỪ CHỐI</button>
                <button id="lfaExport" type="button" class="lf-btn lf-warning">EXPORT ĐIỆN VĂN</button>
                <button id="lfaCancel" type="button" class="lf-btn lf-light">ĐÓNG</button>
            </footer>
        </div>
    </div>

    <script>
        window.liveFireApprovalConfig = {
            apiUrl: '<%= System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] %>',
            userName: '<%= System.Web.HttpUtility.JavaScriptStringEncode(_user == null ? "" : _user.UserName) %>',
            canPublish: <%= (_Role != null && _Role.R_Pub).ToString().ToLowerInvariant() %>
        };
    </script>
    <script src="<%= ResolveUrl("~/MessManagement/LiveFireMessageAccepted.js?v=20260809-1") %>"></script>
</asp:Content>
