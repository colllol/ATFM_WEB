<%@ Page Title="Tổng hợp chỉ số hiệu suất bay từ ADS-B" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="AdsBPerformanceReport.aspx.cs" Inherits="prjApplication.ReportNew.AdsBPerformanceReport" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/AdsBPerformanceReport.css?v=20260722-1") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/AdsBPerformanceReportExtras.css?v=20260808-3") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportInteractive.css?v=20260724-1") %>" />
    <section class="adsb-report-page"
            data-endpoint="<%= System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] %>api/AdsBPerformance/GetData"
            data-operators-endpoint="<%= System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] %>api/AdsBPerformance/GetOperators"
            data-endday-endpoint="<%= ResolveUrl("~/ReportNew/AdsBPerformanceReport.aspx/GetEndOfDay") %>">
        <!-- data-endpoint="<%= ResolveUrl("~/ReportNew/AdsBPerformanceReport.aspx/GetData") %>"
        data-operators-endpoint="<%= ResolveUrl("~/ReportNew/AdsBPerformanceReport.aspx/GetOperators") %>"> -->
        <header class="adsb-hero">
            <div><span>VATM • ADS-B PERFORMANCE</span><h1>Tổng hợp chỉ số hiệu suất bay từ ADS-B</h1><p>Theo dõi lưu lượng LD và O/F dựa trên dữ liệu TRACKS_LOG.</p></div>
            <div class="adsb-hero-badge"><small>Dữ liệu cập nhật</small><strong id="adsbUpdatedAt">--:--</strong></div>
        </header>

        <div class="adsb-filter-card">
            <div><label for="adsbFromDate">Từ ngày</label><input id="adsbFromDate" type="date" /></div>
            <div><label for="adsbToDate">Đến ngày</label><input id="adsbToDate" type="date" /></div>
            <div><label for="adsbPermType">PERMTYPE</label><select id="adsbPermType"><option value="ALL">Tất cả</option><option value="LD">LD</option><option value="O/F">O/F</option></select></div>
            <div><label for="adsbOper">Hãng khai thác (OPER)</label><select id="adsbOper"><option value="ALL">Tất cả hãng</option></select></div>
            <button type="button" id="adsbApply">⌕ Áp dụng</button>
            <button type="button" id="adsbEndDay" class="adsb-endday-button">Báo cáo cuối ngày</button>
        </div>

        <div class="adsb-kpis">
            <article><span>Tổng bản ghi</span><strong id="adsbTotal">0</strong><small>Trong khoảng lọc</small></article>
            <article class="adsb-kpi-ld"><span>Phép bay LD</span><strong id="adsbLd">0</strong><small id="adsbLdRate">0%</small></article>
            <article class="adsb-kpi-of"><span>Phép bay O/F</span><strong id="adsbOf">0</strong><small id="adsbOfRate">0%</small></article>
            <article class="adsb-kpi-other"><span>Chuyến bay khác</span><strong id="adsbOther">0</strong><small>Thiếu tuyến hoặc không xác định</small></article>
            <article class="adsb-kpi-oper"><span>Hãng khai thác</span><strong id="adsbOperCount">0</strong><small>Có dữ liệu trong kỳ</small></article>
        </div>

        <section class="adsb-card adsb-chart-card">
            <div class="adsb-card-heading"><div><h2>Biến động chuyến bay theo ngày</h2><p>Di chuột vào từng mốc để xem số lượng LD, O/F và chuyến bay khác.</p></div><div class="adsb-chart-legend"><span><i class="ld"></i>LD</span><span><i class="of"></i>O/F</span><span><i class="other"></i>Chuyến bay khác</span></div></div>
            <div id="adsbChart" class="adsb-chart"><div class="adsb-empty">Đang tải biểu đồ...</div></div>
        </section>

        <section class="adsb-card adsb-table-card">
            <div class="adsb-card-heading"><div><h2>Chi tiết dữ liệu.</h2><p id="adsbTableInfo">Chưa có dữ liệu</p></div><label class="adsb-page-size">Số dòng <select id="adsbPageSize"><option>50</option><option selected>100</option><option>200</option><option>500</option></select></label></div>
            <div class="adsb-table-scroll"><table><thead><tr><th>STT</th><th>CALLSIGN</th><th>OPER</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ETD</th><th>ETA</th><th>STATUS</th><th>DATE</th><th>UPDATED_AT_UTC</th></tr></thead><tbody id="adsbTableBody"></tbody></table></div>
            <div class="adsb-pagination"><span id="adsbPageInfo"></span><div><button type="button" id="adsbPrev">‹ Trang trước</button><button type="button" id="adsbNext">Trang sau ›</button></div></div>
        </section>
        <div id="adsbError" class="adsb-error" hidden></div>
    </section>
    <div id="adsbEndDayModal" class="adsb-modal" hidden role="dialog" aria-modal="true" aria-labelledby="adsbEndDayTitle">
        <div class="adsb-modal-panel">
            <div class="adsb-modal-heading"><div><h2 id="adsbEndDayTitle">Báo cáo cuối ngày</h2><p id="adsbEndDayInfo">Đang tải dữ liệu...</p></div><button type="button" id="adsbEndDayClose" class="adsb-modal-close" aria-label="Đóng">×</button></div>
            <div class="adsb-modal-table">
                <table class="adsb-endday-grid">
                    <colgroup>
                        <col class="adsb-col-callsign" />
                        <col class="adsb-col-oper" />
                        <col class="adsb-col-permtype" />
                        <col class="adsb-col-airport" />
                        <col class="adsb-col-airport" />
                        <col class="adsb-col-time" />
                        <col class="adsb-col-time" />
                        <col class="adsb-col-status" />
                        <col class="adsb-col-contact" />
                        <col class="adsb-col-contact" />
                        <col class="adsb-col-date" />
                    </colgroup>
                    <thead><tr><th>CALLSIGN</th><th>OPER</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ETD</th><th>ETA</th><th>STATUS</th><th>TIME_IN</th><th>TIME_OUT</th><th>DATE</th></tr></thead>
                    <tbody id="adsbEndDayBody"></tbody>
                </table>
            </div>
        </div>
    </div>
    <script src="<%= ResolveUrl("~/ReportNew/ReportControls.js?v=20260724-1") %>"></script>
    <script src="<%= ResolveUrl("~/ReportNew/AdsBPerformanceReport.js?v=20260808-3") %>"></script>
</asp:Content>
