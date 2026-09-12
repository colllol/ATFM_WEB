<%@ Page Title="Báo cáo hoạt động AI" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="AIReports.aspx.cs" Inherits="prjApplication.SLOTS.AIReports" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/SLOTS/AIReports.css?v=20260722-1") %>" />
    <section id="aiReportsPage" class="ai-reports-page" data-endpoint="<%= ResolveUrl("~/SLOTS/AIReports.aspx/GetReports") %>">
        <header class="ai-report-hero">
            <div>
                <span class="ai-report-eyebrow"><i class="fa fa-microchip"></i> VATM · AI OBSERVABILITY</span>
                <h1>Báo cáo hoạt động AI</h1>
                <p>Theo dõi chất lượng trả lời, SQL được sinh và thời gian xử lý của trợ lý AI.</p>
            </div>
            <div class="ai-report-live">
                <span class="ai-live-dot"></span>
                <div><small>Kết nối dịch vụ</small><strong id="aiConnectionState">Đang kiểm tra</strong></div>
            </div>
        </header>

        <div id="aiReportError" class="ai-report-error" hidden></div>

        <section class="ai-kpis" aria-label="Tổng quan hoạt động AI">
            <article class="ai-kpi ai-kpi-total"><span>Tổng yêu cầu</span><strong id="aiTotalRequests">--</strong><small id="aiSqlRate">--% có SQL</small></article>
            <article class="ai-kpi ai-kpi-success"><span>Thành công</span><strong id="aiSuccessfulRequests">--</strong><small id="aiSuccessRate">--%</small></article>
            <article class="ai-kpi ai-kpi-failed"><span>Thất bại</span><strong id="aiFailedRequests">--</strong><small>Cần kiểm tra</small></article>
            <article class="ai-kpi ai-kpi-duration"><span>Thời gian trung bình</span><strong id="aiAverageDuration">--</strong><small id="aiP95Duration">P95: --</small></article>
            <article class="ai-kpi ai-kpi-sql"><span>Thực thi SQL TB</span><strong id="aiAverageSql">--</strong><small id="aiGeneratedSql">-- yêu cầu sinh SQL</small></article>
        </section>

        <section class="ai-report-card ai-filter-card">
            <div class="ai-filter-heading">
                <div><h2><i class="fa fa-filter"></i> Bộ lọc báo cáo</h2><p>Lọc nhanh dữ liệu đã tải từ dịch vụ AI</p></div>
                <button id="aiRefreshButton" class="ai-primary-button" type="button"><i class="fa fa-refresh"></i> Làm mới</button>
            </div>
            <div class="ai-filters">
                <label class="ai-search-field"><span>Tìm kiếm</span><div><i class="fa fa-search"></i><input id="aiSearch" type="search" placeholder="Câu hỏi, người dùng, SQL, mã yêu cầu..." autocomplete="off" /></div></label>
                <label><span>Trạng thái</span><select id="aiStatusFilter"><option value="ALL">Tất cả</option><option value="SUCCESS">Thành công</option><option value="FAILED">Thất bại</option></select></label>
                <label><span>Người dùng</span><select id="aiUserFilter"><option value="ALL">Tất cả người dùng</option></select></label>
                <label><span>Model</span><select id="aiModelFilter"><option value="ALL">Tất cả model</option></select></label>
                <label><span>Số dòng</span><select id="aiPageSize"><option value="10">10</option><option value="20" selected>20</option><option value="50">50</option><option value="100">100</option></select></label>
            </div>
        </section>

        <section class="ai-report-card ai-table-card">
            <div class="ai-table-heading">
                <div><h2>Danh sách yêu cầu AI</h2><p id="aiResultSummary">Đang tải dữ liệu...</p></div>
                <span id="aiLastUpdated" class="ai-last-updated">--</span>
            </div>
            <div class="ai-table-wrap">
                <table class="ai-report-table">
                    <thead><tr><th>STT</th><th>Thời gian</th><th>Người dùng</th><th>Câu hỏi</th><th>Model</th><th>Trạng thái</th><th>Thời gian</th><th>Số dòng</th><th>Chi tiết</th></tr></thead>
                    <tbody id="aiReportRows"><tr><td colspan="9" class="ai-empty-cell"><span class="ai-inline-spinner"></span> Đang tải dữ liệu...</td></tr></tbody>
                </table>
            </div>
            <div class="ai-pagination">
                <span id="aiPageInfo">Trang 1/1</span>
                <div><button id="aiPreviousPage" type="button"><i class="fa fa-angle-left"></i> Trước</button><button id="aiNextPage" type="button">Sau <i class="fa fa-angle-right"></i></button></div>
            </div>
        </section>
    </section>

    <div id="aiDetailModal" class="ai-detail-modal" hidden>
        <div class="ai-detail-backdrop" data-close-modal="true"></div>
        <article class="ai-detail-dialog" role="dialog" aria-modal="true" aria-labelledby="aiDetailTitle">
            <header><div><span id="aiDetailStatus" class="ai-status-badge"></span><h2 id="aiDetailTitle">Chi tiết yêu cầu AI</h2></div><button type="button" class="ai-modal-close" data-close-modal="true" aria-label="Đóng"><i class="fa fa-times"></i></button></header>
            <div id="aiDetailBody" class="ai-detail-body"></div>
        </article>
    </div>
    <script src="<%= ResolveUrl("~/SLOTS/AIReports.js?v=20260722-1") %>"></script>
</asp:Content>
