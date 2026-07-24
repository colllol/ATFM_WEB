<%@ Page Title="Báo cáo Email" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/SLOTS/EmailReports.css?v=20260724-1") %>" />
    <section id="emailReportsPage" class="email-reports-page" data-api-endpoint="http://192.168.100.131:8080/api/reports/emails">
        <header class="email-hero">
            <div><span class="email-eyebrow"><i class="fa fa-envelope"></i> VATM · EMAIL REPORTS</span><h1>Báo cáo Email</h1><p>Tra cứu, lọc và xem chi tiết email từ hệ thống báo cáo.</p></div>
            <div class="email-live"><i class="email-live-dot"></i><div><small>Trạng thái kết nối</small><strong id="emailConnectionState">Chưa tải</strong></div></div>
        </header>
        <div id="emailError" class="email-error" hidden></div>
        <section class="email-card email-filter-card">
            <div class="email-filter-heading"><div><h2>Bộ lọc email</h2><p>Nhập từ khóa hoặc chọn trạng thái để thu hẹp danh sách.</p></div><span id="emailLastUpdated" class="email-muted">--</span></div>
            <div class="email-filters">
                <label>Tìm kiếm<input id="emailSearch" type="search" placeholder="Tiêu đề, người gửi, nội dung..." /></label>
                <label>Trạng thái<select id="emailStatus"><option value="">Tất cả trạng thái</option></select></label>
                <label>Từ ngày<input id="emailFrom" type="date" /></label>
                <label>Đến ngày<input id="emailTo" type="date" /></label>
                <button id="emailApply" type="button"><i class="fa fa-search"></i> Tìm kiếm</button>
                <button id="emailRefresh" type="button" class="email-secondary-button"><i class="fa fa-refresh"></i></button>
            </div>
        </section>
        <section class="email-kpis">
            <article><small>Tổng email</small><strong id="emailTotal">0</strong></article>
            <article><small>Đã gửi</small><strong id="emailSent">0</strong></article>
            <article><small>Lỗi</small><strong id="emailFailed">0</strong></article>
            <article><small>Trang hiện tại</small><strong id="emailPageLabel">1</strong></article>
        </section>
        <section class="email-card email-table-card">
            <div class="email-table-heading"><div><h2>Danh sách email</h2><p id="emailTableInfo">Chưa có dữ liệu</p></div><label class="email-page-size">Số dòng<select id="emailPageSize"><option>25</option><option selected>50</option><option>100</option><option>200</option></select></label></div>
            <div class="email-table-wrap"><table class="email-table"><thead><tr><th>STT</th><th>Thời gian</th><th>Tiêu đề</th><th>Người gửi</th><th>Người nhận</th><th>Trạng thái</th><th></th></tr></thead><tbody id="emailRows"></tbody></table></div>
            <div class="email-pagination"><button id="emailPrev" type="button">‹ Trước</button><span id="emailPageInfo">Trang 1/1</span><button id="emailNext" type="button">Sau ›</button></div>
        </section>
        <div id="emailDetailBackdrop" class="email-detail-backdrop" hidden><article class="email-detail"><header><h2 id="emailDetailSubject">Chi tiết email</h2><button id="emailDetailClose" type="button">&times;</button></header><div id="emailDetailMeta" class="email-detail-meta"></div><pre id="emailDetailBody" class="email-detail-body"></pre></article></div>
    </section>
    <script src="<%= ResolveUrl("~/SLOTS/EmailReports.js?v=20260724-1") %>"></script>
</asp:Content>
