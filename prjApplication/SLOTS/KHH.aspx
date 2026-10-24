<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="KHH.aspx.cs" Inherits="prjApplication.SLOTS.KHH" %>
<%@ Register Src="~/SLOTS/SlotsTable.ascx" TagPrefix="slots" TagName="SlotsTable" %>

<asp:Content ID="KhhContent" ContentPlaceHolderID="MainContent" runat="server">
    <slots:SlotsTable ID="KhhTable" runat="server"
        TableName="T_KHH"
        DisplayTitle="Kế hoạch chuyến bay"
        Description="Khai thác và tra cứu dữ liệu kế hoạch chuyến bay." />
</asp:Content>
