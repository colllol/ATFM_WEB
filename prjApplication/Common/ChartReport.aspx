<%@ Page Title="VATM Flight Analytics" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ChartReport.aspx.cs" Inherits="prjApplication.Common.ChartReport" %>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/ReportNew/ReportNew.css") %>" />
    <section class="rn-app" data-report-view="dashboard"></section>
    <script src="<%= ResolveUrl("~/ReportNew/ReportNew.js") %>"></script>
</asp:Content>
