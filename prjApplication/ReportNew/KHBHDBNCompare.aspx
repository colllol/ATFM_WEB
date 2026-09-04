<%@ Page Title="Đánh giá, so sánh KHBHĐBN" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="KHBHDBNCompare.aspx.cs" Inherits="prjApplication.ReportNew.KHBHDBNCompare" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportNew.css") %>" />
<section class="rn-app" data-report-view="khbhdbn"><h1>Đánh giá, so sánh KHBHĐBN</h1>
<div class="rn-filters"><label>Kỳ hiện tại <input id="currentDate" type="date" /></label><label>Kỳ so sánh <input id="compareDate" type="date" /></label><label>Phân loại <select id="category"><option value="">Tất cả</option><option>NEW</option><option>MISSING</option><option>CHANGED</option><option>DUPLICATE</option><option>NORMAL</option><option>UNRESOLVED</option></select></label><button id="btnSearch" type="button">Tìm kiếm</button><button id="btnExport" type="button">Export Excel</button></div>
<div id="kpis" class="rn-kpis"></div><div class="rn-table-wrap"><table class="rn-table"><thead><tr><th>LOẠI</th><th>CALLSIGN</th><th>FROM</th><th>TO</th><th>ETD</th><th>ETA</th><th>CRAFT</th><th>PERM</th><th>NGÀY HIỆN TẠI</th><th>NGÀY SO SÁNH</th><th>DIỄN GIẢI</th></tr></thead><tbody id="rows"></tbody></table></div><p id="message"></p></section>
<script src="<%= ResolveUrl("~/ReportNew/KHBHDBNCompare.js?v=20260904-1") %>"></script>
</asp:Content>
