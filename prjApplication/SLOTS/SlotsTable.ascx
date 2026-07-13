<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlotsTable.ascx.cs"
    Inherits="prjApplication.SLOTS.SlotsTable" %>

<style>
    .slots-page { width: 100%; min-width: 0; color: #173b59; }
    .slots-hero { display:flex; align-items:center; justify-content:space-between; gap:18px; margin-bottom:14px; padding:18px 20px; border:1px solid #c7ddeb; border-radius:12px; background:linear-gradient(135deg,#f8fcff,#e8f4fc); box-shadow:0 6px 18px rgba(27,76,112,.10); }
    .slots-hero h2 { margin:0 0 4px; color:#176da9; font-size:23px; font-weight:600; }
    .slots-hero p { margin:0; color:#6d8497; }
    .slots-source-badge { padding:9px 14px; border-radius:20px; background:#1f6d9f; color:#fff; font-weight:700; white-space:nowrap; }
    .slots-toolbar { display:flex; align-items:flex-end; flex-wrap:wrap; gap:10px; margin-bottom:12px; padding:12px 14px; border:1px solid #c7ddeb; border-radius:10px; background:#f2f8fc; box-shadow:0 4px 12px rgba(27,76,112,.08); }
    .slots-filter-field { display:flex; flex-direction:column; gap:4px; min-width:0; }
    .slots-filter-field label { margin:0; color:#315b79; font-size:11px; font-weight:700; letter-spacing:.2px; text-transform:uppercase; }
    .slots-filter-search { flex:0 1 280px; width:280px; }
    .slots-filter-date { flex:0 0 170px; }
    .slots-filter-airport { flex:0 0 170px; }
    .slots-filter-page-size { flex:0 0 78px; }
    .slots-toolbar select, .slots-toolbar input[type="text"], .slots-toolbar input[type="date"] { width:100%; height:36px; padding:7px 10px; border:1px solid #8eb9d6; border-radius:7px; background:#fff; color:#173b59; outline:none; }
    .slots-toolbar select:focus, .slots-toolbar input:focus { border-color:#258ac7; box-shadow:0 0 0 3px rgba(37,138,199,.13); }
    .slots-toolbar .slots-toolbar-actions { display:flex; align-items:center; gap:8px; margin-left:auto; }
    .slots-toolbar .btn { min-height:36px; border-radius:7px; font-weight:600; }
    .slots-toolbar .btn i { margin-right:5px; }
    .slots-table-card { position:relative; border:1px solid #c8d9e8; border-radius:10px; background:#fff; box-shadow:0 7px 22px rgba(26,67,105,.13); }
    .slots-table-scroll { width:100%; max-height:calc(100vh - 390px); min-height:340px; overflow:auto !important; }
    .slots-data-table { width:max-content; min-width:100%; margin:0; border-collapse:separate; border-spacing:0; }
    .slots-data-table thead th { position:sticky; top:0; z-index:20; padding:10px 9px !important; border-color:#75a4c7 !important; background:#1e5f91 !important; color:#fff !important; font-size:12px; white-space:nowrap; }
    .slots-data-table tbody td { max-width:280px; padding:6px 9px !important; border-color:#d6e2eb !important; color:#173b59; white-space:nowrap; overflow:hidden; text-overflow:ellipsis; }
    .slots-data-table tbody tr:nth-child(even) td { background:#f6fafc; }
    .slots-data-table tbody tr:hover td { background:#e5f4ff; }
    .slots-data-table .slots-frozen-column { position:sticky; z-index:13; }
    .slots-data-table thead .slots-frozen-column { z-index:25; background:#174f7c !important; }
    .slots-data-table tbody tr:nth-child(odd) .slots-frozen-column { background:#fff; }
    .slots-data-table tbody tr:nth-child(even) .slots-frozen-column { background:#f6fafc; }
    .slots-data-table .slots-frozen-column:nth-child(3) { border-right:3px solid #00a6d6 !important; box-shadow:7px 0 9px -6px rgba(0,80,120,.9); }
    .slots-empty { padding:40px 20px; color:#7890a3; text-align:center; }
    .slots-paging { display:flex; align-items:center; justify-content:flex-end; flex-wrap:wrap; gap:8px; padding:10px 12px; border-top:1px solid #d9e4ec; }
    .slots-paging .btn[disabled] { opacity:.45; }
    .slots-error { margin-bottom:12px; padding:11px 14px; border:1px solid #efb9b9; border-radius:8px; background:#fff1f1; color:#a62c2c; }
    @media (max-width:767px) {
        .slots-hero { align-items:flex-start; flex-direction:column; padding:14px; }
        .slots-toolbar > * { max-width:100%; }
        .slots-filter-search, .slots-filter-date, .slots-filter-airport, .slots-filter-page-size { flex:1 1 145px; width:auto; }
        .slots-toolbar .slots-toolbar-actions { width:100%; margin-left:0; }
        .slots-toolbar .slots-toolbar-actions .btn { flex:1; }
        .slots-table-scroll { max-height:calc(100vh - 450px); min-height:300px; }
    }
</style>

<div class="slots-page">
    <section class="slots-hero">
        <div>
            <h2><asp:Literal ID="litTitle" runat="server" /></h2>
            <p><asp:Literal ID="litDescription" runat="server" /></p>
        </div>
        <asp:Label ID="lblSourceBadge" runat="server" CssClass="slots-source-badge" />
    </section>

    <asp:Panel ID="pnlError" runat="server" CssClass="slots-error" Visible="false">
        <asp:Literal ID="litError" runat="server" />
    </asp:Panel>

    <div class="slots-toolbar">
        <div class="slots-filter-field slots-filter-search">
            <label for="<%= txtSearch.ClientID %>">Tìm kiếm</label>
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Nhập nội dung cần tìm..." />
        </div>
        <div class="slots-filter-field slots-filter-date">
            <label for="<%= txtFilterDate.ClientID %>">Ngày bay</label>
            <asp:TextBox ID="txtFilterDate" runat="server" TextMode="Date" />
        </div>
        <div class="slots-filter-field slots-filter-airport">
            <label for="<%= txtAirport.ClientID %>">Sân bay</label>
            <asp:TextBox ID="txtAirport" runat="server" MaxLength="8" placeholder="VD: VVNB" />
        </div>
        <div class="slots-filter-field slots-filter-page-size">
            <label for="<%= ddlPageSize.ClientID %>">Số dòng</label>
            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="100" Text="100" Selected="true" />
                <asp:ListItem Value="500" Text="500" />
                <asp:ListItem Value="1000" Text="1000" />
                <asp:ListItem Value="2000" Text="2000" />
                <asp:ListItem Value="4000" Text="4000" />
                <asp:ListItem Value="6000" Text="6000" />
                <asp:ListItem Value="8000" Text="8000" />
            </asp:DropDownList>
        </div>
        <div class="slots-toolbar-actions">
            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Tìm kiếm" OnClick="btnSearch_Click" />
            <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-info" Text="Làm mới" OnClick="btnRefresh_Click" />
        </div>
    </div>

    <section class="slots-table-card">
        <div class="table-responsive slots-table-scroll"><asp:Literal ID="litTable" runat="server" /></div>
        <div class="slots-paging">
            <asp:Label ID="lblPaging" runat="server" />
            <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-primary" Text="‹ Trang trước" OnClick="btnPrevious_Click" />
            <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Trang sau ›" OnClick="btnNext_Click" />
        </div>
    </section>
</div>

<script>
    (function () {
        function applySlotsFreeze() {
            var tables = document.querySelectorAll('.slots-data-table');
            for (var t = 0; t < tables.length; t++) {
                var table = tables[t];
                if (!table.tHead || !table.tHead.rows.length) continue;
                var header = table.tHead.rows[0], left = 0;
                var frozenCount = Math.min(3, header.cells.length);
                for (var column = 0; column < frozenCount; column++) {
                    var width = header.cells[column].getBoundingClientRect().width;
                    for (var row = 0; row < table.rows.length; row++) {
                        if (!table.rows[row].cells[column]) continue;
                        table.rows[row].cells[column].classList.add('slots-frozen-column');
                        table.rows[row].cells[column].style.left = Math.round(left) + 'px';
                    }
                    left += width;
                }
            }
        }
        if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', applySlotsFreeze);
        else applySlotsFreeze();
        window.addEventListener('resize', function () { window.requestAnimationFrame(applySlotsFreeze); });
    })();
</script>
