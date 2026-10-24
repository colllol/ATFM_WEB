<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="SlotComparison.aspx.cs" Inherits="prjApplication.SLOTS.SlotComparison" %>
<%@ Register Src="~/SLOTS/SlotComparisonPanel.ascx" TagPrefix="slots" TagName="ComparisonPanel" %>

<asp:Content ID="SlotComparisonContent" ContentPlaceHolderID="MainContent" runat="server">
    <slots:ComparisonPanel ID="ComparisonPanel" runat="server" Mode="Compare" />
</asp:Content>
