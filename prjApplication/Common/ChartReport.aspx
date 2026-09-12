<%@ Page Title="VATM Flight Analytics" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ChartReport.aspx.cs" Inherits="prjApplication.Common.ChartReport" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportNew.css") %>" />
    <link rel="stylesheet" data-report-interactive="true" href="<%= ResolveUrl("~/ReportNew/ReportInteractive.css?v=20260911-1") %>" />
    <section class="rn-app" data-report-view="dashboard"></section>
    <script src="<%= ResolveUrl("~/Style/assets/js/select2.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/ReportNew/ReportNew.js") %>"></script>
    <script src="<%= ResolveUrl("~/ReportNew/Dashboard.js?v=20260911-1") %>"></script>
</asp:Content>
