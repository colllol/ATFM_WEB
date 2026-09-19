<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="SlotAero.aspx.cs" Inherits="prjApplication.SLOTS.SlotAero" %>
<%@ Register Src="~/SLOTS/SlotsTable.ascx" TagPrefix="slots" TagName="SlotsTable" %>

<asp:Content ID="SlotAeroContent" ContentPlaceHolderID="MainContent" runat="server">
    <slots:SlotsTable ID="SlotAeroTable" runat="server"
        TableName="T_SLOT_AERO"
        DisplayTitle="SLOT sân bay"
        Description="Khai thác và tra cứu dữ liệu slot sân bay." />
</asp:Content>
