<%@ Page Title="Live Fire Message" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
    AutoEventWireup="true" CodeBehind="LiveFireMessage.aspx.cs"
    Inherits="prjApplication.MessManagement.LiveFireMessage" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/MessManagement/LiveFireMessage.css?v=20260809-1") %>" />

    <section class="lf-app">
        <div class="lf-toolbar">
            <label>TỪ NGÀY
                <input id="lfFromDate" type="date" />
            </label>
            <label>ĐẾN NGÀY
                <input id="lfToDate" type="date" />
            </label>
            <label>TRẠNG THÁI
                <select id="lfStatus">
                    <option value="-2">NHÁP VÀ TỪ CHỐI</option>
                    <option value="0">NHÁP</option>
                    <option value="3">TỪ CHỐI</option>
                </select>
            </label>
            <label class="lf-keyword">TỪ KHÓA
                <input id="lfKeyword" type="text" maxlength="200"
                    placeholder="Mã, tiêu đề, địa điểm, người nhập" />
            </label>
            <label>SỐ DÒNG
                <select id="lfPageSize">
                    <option value="20">20</option>
                    <option value="50" selected="selected">50</option>
                    <option value="100">100</option>
                </select>
            </label>
            <button id="lfSearch" type="button" class="lf-btn lf-primary">TÌM KIẾM</button>
            <button id="lfNew" type="button" class="lf-btn lf-success">THÊM MỚI</button>
        </div>

        <div class="lf-summary">
            <span id="lfTotal">TỔNG SỐ: 0</span>
            <span class="lf-status-help">Màn hình nhập liệu: Nháp và điện văn bị từ chối</span>
        </div>

        <div class="lf-grid-wrap">
            <table id="lfGrid" class="lf-grid" data-page="1" data-total="0">
                <thead>
                    <tr>
                        <th>NO</th>
                        <th>NGÀY ĐIỆN VĂN</th>
                        <th>MÃ</th>
                        <th>TIÊU ĐỀ</th>
                        <th>ĐỊA ĐIỂM BẮN</th>
                        <th>TRẠNG THÁI</th>
                        <th>NGƯỜI NHẬP</th>
                        <th>NGƯỜI DUYỆT</th>
                        <th>THAO TÁC</th>
                    </tr>
                </thead>
                <tbody>
                    <tr><td colspan="9" class="lf-empty">Nhấn Tìm kiếm để tải dữ liệu.</td></tr>
                </tbody>
            </table>
        </div>
        <div class="lf-pager">
            <button id="lfPrev" type="button">&lt;</button>
            <strong id="lfPageLabel">Trang 1/1</strong>
            <button id="lfNext" type="button">&gt;</button>
        </div>
    </section>

    <div id="lfEditor" class="lf-modal" aria-hidden="true">
        <div class="lf-modal-dialog" role="dialog" aria-modal="true" aria-labelledby="lfEditorTitle">
            <header>
                <h2 id="lfEditorTitle">Điện văn bắn đạn thật</h2>
                <button id="lfClose" type="button" class="lf-close" aria-label="Đóng">×</button>
            </header>

            <div class="lf-editor-body">
                <input id="lfId" type="hidden" value="0" />
                <input id="lfVersion" type="hidden" value="0" />
                <input id="lfCurrentStatus" type="hidden" value="0" />

                <fieldset>
                    <legend>1. Thông tin chung</legend>
                    <div class="lf-fields lf-fields-4">
                        <label>Ngày điện văn (*)<input id="lfMessageDate" type="date" /></label>
                        <label>Mã điện văn<input id="lfMessageCode" type="text" maxlength="100" /></label>
                        <label class="lf-span-2">Tiêu đề (*)<input id="lfSubject" type="text" maxlength="300" value="THONG BAO BAN DAN THAT" /></label>
                        <label class="lf-span-4">Nội dung mở đầu<textarea id="lfIntro" rows="2" maxlength="2000"></textarea></label>
                        <label class="lf-span-4">Địa điểm bắn (*)<textarea id="lfLocation" rows="2" maxlength="1000"></textarea></label>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>2. Tọa độ khu vực bắn</legend>
                    <table class="lf-subgrid" id="lfCoordinates">
                        <thead><tr><th>Điểm</th><th>Vĩ độ</th><th>Kinh độ</th><th></th></tr></thead>
                        <tbody></tbody>
                    </table>
                    <button id="lfAddCoordinate" type="button" class="lf-btn lf-light">+ THÊM TỌA ĐỘ</button>
                </fieldset>

                <fieldset>
                    <legend>3. Thông số bắn</legend>
                    <div class="lf-fields lf-fields-3">
                        <label>Phương vị bắn<input id="lfDirection" type="text" maxlength="300" /></label>
                        <label>Độ cao đường đạn<input id="lfHeight" type="text" maxlength="100" /></label>
                        <label>Cự ly đường đạn<input id="lfRange" type="text" maxlength="100" /></label>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>4. Thời gian bắn</legend>
                    <table class="lf-subgrid" id="lfSchedules">
                        <thead><tr><th>Từ giờ</th><th>Đến giờ</th><th>Ngày</th><th>Độ cao</th><th></th></tr></thead>
                        <tbody></tbody>
                    </table>
                    <button id="lfAddSchedule" type="button" class="lf-btn lf-light">+ THÊM KHUNG GIỜ</button>
                </fieldset>

                <fieldset>
                    <legend>5. Chỉ huy và ký điện</legend>
                    <div class="lf-fields lf-fields-3">
                        <label>Cấp bậc chỉ huy<input id="lfCommanderRank" type="text" maxlength="100" /></label>
                        <label>Họ tên chỉ huy<input id="lfCommanderName" type="text" maxlength="200" /></label>
                        <label>Số điện thoại<input id="lfCommanderPhone" type="text" maxlength="50" /></label>
                        <label>Cấp bậc người thay thế<input id="lfReplacementRank" type="text" maxlength="100" /></label>
                        <label>Họ tên người thay thế<input id="lfReplacementName" type="text" maxlength="200" /></label>
                        <label>Số điện thoại<input id="lfReplacementPhone" type="text" maxlength="50" /></label>
                        <label class="lf-span-3">Nội dung hạn chế/cấm bay<textarea id="lfRestriction" rows="2" maxlength="2000"></textarea></label>
                        <label class="lf-span-2">Chức danh người ký<input id="lfSignatoryTitle" type="text" maxlength="300" /></label>
                        <label>Họ tên người ký<input id="lfSignatoryName" type="text" maxlength="200" /></label>
                    </div>
                </fieldset>

                <fieldset>
                    <legend>6. Xem trước điện văn</legend>
                    <textarea id="lfPreview" rows="15" readonly="readonly"></textarea>
                    <div id="lfRejectInfo" class="lf-reject-info"></div>
                </fieldset>
            </div>

            <footer>
                <button id="lfSave" type="button" class="lf-btn lf-primary">LƯU NHÁP</button>
                <button id="lfSubmit" type="button" class="lf-btn lf-success">GỬI DUYỆT</button>
                <button id="lfCancel" type="button" class="lf-btn lf-light">ĐÓNG</button>
            </footer>
        </div>
    </div>

    <script>
        window.liveFireConfig = {
            apiUrl: '<%= System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] %>',
            userName: '<%= System.Web.HttpUtility.JavaScriptStringEncode(_user == null ? "" : _user.UserName) %>',
            canAdd: <%= (_Role != null && _Role.R_Add).ToString().ToLowerInvariant() %>,
            canEdit: <%= (_Role != null && _Role.R_Edit).ToString().ToLowerInvariant() %>,
            canPublish: <%= (_Role != null && _Role.R_Pub).ToString().ToLowerInvariant() %>
        };
    </script>
    <script src="<%= ResolveUrl("~/MessManagement/LiveFireMessage.js?v=20260809-1") %>"></script>
</asp:Content>
