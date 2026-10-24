<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="SlotResults.aspx.cs" Inherits="prjApplication.SLOTS.SlotResults" %>
<%@ Register Src="~/SLOTS/SlotComparisonPanel.ascx" TagPrefix="slots" TagName="ComparisonPanel" %>

<asp:Content ID="SlotResultsContent" ContentPlaceHolderID="MainContent" runat="server">
    <slots:ComparisonPanel ID="ResultsPanel" runat="server" Mode="Results" />
</asp:Content>
