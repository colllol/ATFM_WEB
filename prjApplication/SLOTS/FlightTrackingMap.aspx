<%@ Page Title="Theo dõi chuyến bay ADS-B" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="FlightTrackingMap.aspx.cs" Inherits="prjApplication.SLOTS.FlightTrackingMap" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/SLOTS/FlightTrackingMap.css?v=20260808-1") %>" />
    <section class="track-map-page"
        data-flights-endpoint="<%= ResolveUrl("~/SLOTS/FlightTrackingMap.aspx/GetFlights") %>"
        data-fir-endpoint="<%= ResolveUrl("~/SLOTS/FlightTrackingMap.aspx/GetFirGeoJson") %>">
        <header class="track-map-hero">
            <div>
                <span class="track-map-kicker">VATM • ADS-B LIVE TRACKING</span>
                <h1>Bản đồ theo dõi chuyến bay</h1>
                <p>Vị trí chuyến bay trong FIR Hà Nội và FIR Hồ Chí Minh, tự động cập nhật mỗi 5 giây.</p>
            </div>
            <div class="track-live-state"><i></i><span>Đang theo dõi</span><strong id="trackUpdatedAt">--:--:--</strong></div>
        </header>

        <div class="track-map-summary">
            <article><span>Tổng chuyến đang hiển thị</span><strong id="trackTotal">0</strong></article>
            <article class="track-summary-ld"><span>Phép bay LD</span><strong id="trackLd">0</strong></article>
            <article class="track-summary-of"><span>Phép bay O/F</span><strong id="trackOf">0</strong></article>
            <article class="track-summary-other"><span>Chuyến bay khác</span><strong id="trackOther">0</strong></article>
            <div class="track-map-legend"><span><i class="legend-plane ld"></i>LD</span><span><i class="legend-plane of"></i>O/F</span><span><i class="legend-plane other"></i>Khác</span></div>
        </div>

        <div class="track-map-card">
            <div class="track-map-toolbar">
                <div><strong id="trackDayLabel">Dữ liệu ngày hiện tại</strong><span id="trackMessage">Đang khởi tạo bản đồ...</span></div>
                <button type="button" id="trackRefreshButton" title="Cập nhật vị trí ngay">↻ Cập nhật</button>
            </div>
            <div id="trackMapViewport" class="track-map-viewport" aria-label="Bản đồ vị trí chuyến bay">
                <svg id="trackMapSvg" viewBox="0 0 1100 680" preserveAspectRatio="xMidYMid meet" role="img">
                    <defs>
                        <linearGradient id="trackSeaGradient" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#dedfe2"/><stop offset="1" stop-color="#c8c9ce"/></linearGradient>
                        <filter id="trackPlaneShadow"><feDropShadow dx="0" dy="3" stdDeviation="3" flood-color="#143a59" flood-opacity=".28"/></filter>
                    </defs>
                    <rect width="1100" height="680" fill="url(#trackSeaGradient)" rx="18" />
                    <g id="trackBaseLayer"></g>
                    <g id="trackGridLayer"></g>
                    <g id="trackFirLayer"></g>
                    <g id="trackPlaneLayer"></g>
                </svg>
                <div class="track-map-controls" aria-label="Điều khiển bản đồ">
                    <button type="button" id="trackZoomIn" title="Phóng to">+</button>
                    <button type="button" id="trackZoomOut" title="Thu nhỏ">−</button>
                    <button type="button" id="trackResetView" title="Về khung nhìn ban đầu">⌾</button>
                </div>
                <div class="track-map-scale"><i></i><span>500 km</span></div>
                <div class="track-map-source">Bản đồ offline • Natural Earth</div>
                <div id="trackEmpty" class="track-map-empty" hidden>Không có chuyến bay của ngày hiện tại nằm trong vùng bản đồ.</div>
                <aside id="trackPopup" class="track-popup" hidden></aside>
            </div>
        </div>
    </section>
    <script src="<%= ResolveUrl("~/SLOTS/FlightTrackingMap.js?v=20260808-1") %>"></script>
</asp:Content>
