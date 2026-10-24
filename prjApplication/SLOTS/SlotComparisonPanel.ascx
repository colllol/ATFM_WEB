<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlotComparisonPanel.ascx.cs"
    Inherits="prjApplication.SLOTS.SlotComparisonPanel" %>
<link rel="stylesheet" href="<%= ResolveUrl("~/SLOTS/SlotComparison.css") %>" />

<div class="slot-compare-page">
    <section class="slot-compare-hero">
        <div>
            <h2><asp:Literal ID="litPageTitle" runat="server" /></h2>
            <p><asp:Literal ID="litPageDescription" runat="server" /></p>
        </div>
        <div class="slot-compare-hero__icon"><i class="fa fa-random"></i></div>
    </section>

    <asp:Panel ID="pnlError" runat="server" CssClass="slot-message slot-message--error" Visible="false"><asp:Literal ID="litError" runat="server" /></asp:Panel>
    <asp:Panel ID="pnlSuccess" runat="server" CssClass="slot-message slot-message--success" Visible="false"><asp:Literal ID="litSuccess" runat="server" /></asp:Panel>

    <section class="slot-filter-card">
        <div class="slot-filter-field">
            <label for="<%= txtCompareDate.ClientID %>">Ngày đối chiếu</label>
            <asp:TextBox ID="txtCompareDate" runat="server" TextMode="Date" />
        </div>
        <div class="slot-filter-field">
            <label for="<%= ddlOper.ClientID %>">Hãng khai thác (OPER)</label>
            <asp:DropDownList ID="ddlOper" runat="server" />
        </div>
        <div class="slot-kq-actions">
            <asp:Button ID="btnFilter" runat="server" CssClass="slot-search-btn" Text="Tìm kiếm" OnClick="btnFilter_Click" />
            <asp:LinkButton ID="btnKQ1" runat="server" CssClass="slot-kq-btn slot-kq-btn--1" CommandArgument="KQ1" OnCommand="Kq_Command" Text="KQ1 (0)" ToolTip="Có SLOT nhưng không có Phép bay" />
            <asp:LinkButton ID="btnKQ2" runat="server" CssClass="slot-kq-btn slot-kq-btn--2" CommandArgument="KQ2" OnCommand="Kq_Command" Text="KQ2 (0)" ToolTip="Có Phép bay nhưng không có SLOT" />
            <asp:LinkButton ID="btnKQ3" runat="server" CssClass="slot-kq-btn slot-kq-btn--3" CommandArgument="KQ3" OnCommand="Kq_Command" Text="KQ3 (0)" ToolTip="Kế hoạch hãng không có đồng thời SLOT và Phép bay" />
            <asp:LinkButton ID="btnKQ4" runat="server" CssClass="slot-kq-btn slot-kq-btn--4" CommandArgument="KQ4" OnCommand="Kq_Command" Text="KQ4 (0)" ToolTip="Có SLOT và Phép bay nhưng không có Kế hoạch hãng" />
            <asp:Button ID="btnExport" runat="server" CssClass="slot-export-btn" Text="↓ Xuất Excel" OnClick="btnExport_Click" />
        </div>
    </section>

    <section class="slot-result-card">
        <header class="slot-result-head">
            <div><h3><asp:Literal ID="litResultTitle" runat="server" /></h3><span><asp:Literal ID="litResultDescription" runat="server" /></span></div>
            <asp:Label ID="lblResultTotal" runat="server" />
        </header>
        <div class="slot-result-scroll"><asp:Literal ID="litResultTable" runat="server" /></div>
        <footer class="slot-result-footer">
            <asp:Label ID="lblPaging" runat="server" />
            <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-primary" Text="‹ Trang trước" OnClick="btnPrevious_Click" />
            <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Trang sau ›" OnClick="btnNext_Click" />
        </footer>
    </section>
</div>
