<%@ Page Title="Cảnh báo chuyến bay delay" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="AnomalyWarning.aspx.cs" Inherits="prjApplication.ReportNew.AnomalyWarning" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportNew.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/AnomalyWarning.css?v=20260727-1") %>" />
    <section class="rn-app" data-report-view="anomaly"></section>
    <script src="<%= ResolveUrl("~/ReportNew/ReportNew.js") %>"></script>
    <script src="<%= ResolveUrl("~/ReportNew/AnomalyWarning.js?v=20260727-1") %>"></script>
</asp:Content>
