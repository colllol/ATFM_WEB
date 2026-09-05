<%@ Page Title="Đánh giá số liệu kế hoạch bay hoạt động bay ngày" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="FlightPlanDailyComparison.aspx.cs" Inherits="prjApplication.ReportNew.FlightPlanDailyComparison" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportNew.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportInteractive.css?v=20260905-1") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/FlightPlanDailyComparison.css?v=20260905-2") %>" />
    <section class="fpd-page" data-endpoint="<%= ResolveUrl("~/ReportNew/FlightPlanDailyComparison.aspx/GetComparison") %>">
        <header class="fpd-hero">
            <div>
                <span class="fpd-eyebrow">VATM · REPORT NEW</span>
                <h1>Đánh giá số liệu kế hoạch bay hoạt động bay ngày</h1>
                <p>Đối chiếu kế hoạch bay giữa hai ngày, nhận diện thay đổi theo sân bay và loại kế hoạch.</p>
            </div>
            <div class="fpd-hero-mark" aria-hidden="true"><span>Δ</span><small>DAILY<br />PLAN</small></div>
        </header>

        <section class="fpd-filter-panel" aria-label="Bộ lọc báo cáo">
            <div class="fpd-filter-heading"><span class="fpd-section-kicker">Phạm vi dữ liệu</span><strong>Chọn ngày để bắt đầu đối chiếu</strong></div>
            <div class="fpd-filters">
                <label class="fpd-field"><span>Sân bay</span><select id="fpdAirport"><option value="ALL">Tất cả sân bay</option></select></label>
                <label class="fpd-field"><span>Hãng khai thác</span><select id="fpdOperator"><option value="ALL">Tất cả hãng</option></select></label>
                <label class="fpd-field"><span>Ngày so sánh 1</span><input id="fpdDate1" type="date" /></label>
                <label class="fpd-field fpd-field-readonly"><span>Ngày so sánh 2 <em>Tự động -7 ngày</em></span><input id="fpdDate2" type="date" readonly aria-readonly="true" /></label>
                <button type="button" id="fpdApply" class="fpd-apply"><span aria-hidden="true">↗</span> So sánh</button>
            </div>
            <p class="fpd-filter-note" id="fpdFilterNote">Ngày so sánh 2 được tự động lùi 7 ngày từ ngày so sánh 1.</p>
        </section>

        <div class="fpd-status" id="fpdStatus" role="status" aria-live="polite"><span class="fpd-spinner" aria-hidden="true"></span> Đang tải dữ liệu...</div>
        <section class="fpd-table-panel" aria-labelledby="fpdTableTitle">
            <div class="fpd-table-heading"><div><span class="fpd-section-kicker">Kết quả đối chiếu</span><h2 id="fpdTableTitle">Tổng hợp thay đổi kế hoạch bay</h2></div><span class="fpd-source">Nguồn: T_DAY_FLIGHTS</span></div>
            <div class="fpd-table-wrap">
                <table class="fpd-table">
                    <thead><tr><th scope="col">STT</th><th scope="col">Nội dung so sánh</th><th scope="col" id="fpdHeadDate1">Ngày 1</th><th scope="col" id="fpdHeadDate2">Ngày 2</th><th scope="col">Số chuyến bay khác</th></tr></thead>
                    <tbody id="fpdBody"><tr><td colspan="5" class="fpd-empty">Chọn bộ lọc để tải dữ liệu.</td></tr></tbody>
                </table>
            </div>
            <div class="fpd-footnote"><span class="fpd-dot"></span> Chỉ tính chuyến bay có `FLIGHTDATE`, có `PERMNBR` và `PERMNBR &lt;&gt; NoPerm`.</div>
        </section>
        <div id="fpdError" class="fpd-error" hidden></div>
    </section>

    <div id="fpdModal" class="fpd-modal" hidden role="dialog" aria-modal="true" aria-labelledby="fpdModalTitle">
        <div class="fpd-modal-panel">
            <div class="fpd-modal-head"><div><span class="fpd-section-kicker">Chi tiết sai khác</span><h2 id="fpdModalTitle">Các chuyến bay không trùng</h2><p id="fpdModalInfo"></p></div><button type="button" id="fpdClose" class="fpd-close" aria-label="Đóng cửa sổ">×</button></div>
            <div class="fpd-modal-grid"><section><h3 id="fpdModalDate1">Ngày so sánh 1</h3><div class="fpd-detail-wrap"><table><thead><tr><th>CALLSIGN</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ETD</th><th>ETA</th></tr></thead><tbody id="fpdDetails1"></tbody></table></div></section><section><h3 id="fpdModalDate2">Ngày so sánh 2</h3><div class="fpd-detail-wrap"><table><thead><tr><th>CALLSIGN</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ETD</th><th>ETA</th></tr></thead><tbody id="fpdDetails2"></tbody></table></div></section></div>
        </div>
    </div>
    <script src="<%= ResolveUrl("~/ReportNew/FlightPlanDailyComparison.js?v=20260905-1") %>"></script>
</asp:Content>
