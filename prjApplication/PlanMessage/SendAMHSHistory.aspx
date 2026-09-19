<%@ Page Title="AMHS SEND HISTORY" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master"
    AutoEventWireup="true" CodeBehind="SendAMHSHistory.aspx.cs"
    Inherits="prjApplication.PlanMessage.SendAMHSHistory" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <style type="text/css">
        .amhsh-page-title {
            display: block;
            margin-bottom: 12px;
            color: #1769aa;
            font-size: 18px;
            font-weight: 600;
        }

        .amhsh-search {
            margin-bottom: 12px;
            padding: 12px 14px;
            border: 1px solid #d8e6f2;
            border-radius: 4px;
            background: #f3f9fd;
        }

        .amhsh-search label {
            margin: 0 6px 0 0;
            color: #405568;
            font-weight: 600;
        }

        .amhsh-search .amhsh-date {
            display: inline-block;
            width: 125px;
            margin-right: 14px;
        }

        .amhsh-grid {
            min-width: 1550px;
            margin-bottom: 0;
        }

        .amhsh-grid th {
            white-space: nowrap;
            background: #2877a8;
            color: #fff;
            text-align: center;
            vertical-align: middle;
        }

        .amhsh-grid td {
            vertical-align: top;
        }

        .amhsh-content {
            max-width: 400px;
            max-height: 100px;
            overflow: auto;
            white-space: pre-wrap;
            word-break: break-word;
        }

        .amhsh-status {
            display: block;
            margin: 6px 0;
            color: #a94442;
        }
    </style>

    <span class="amhsh-page-title">+ AMHS SEND HISTORY</span>

    <div class="amhsh-search">
        <label for="txtFromDate">TỪ NGÀY</label>
        <input id="txtFromDate" runat="server" type="text"
            class="inputControl date-picker amhsh-date" data-date-format="dd/mm/yyyy" />
        <label for="txtToDate">ĐẾN NGÀY</label>
        <input id="txtToDate" runat="server" type="text"
            class="inputControl date-picker amhsh-date" data-date-format="dd/mm/yyyy" />
        <asp:Button ID="btnSearch" runat="server" Text="TÌM KIẾM"
            CssClass="btn btn-sm btn-primary" CausesValidation="false"
            OnClick="btnSearch_Click" />
        <asp:Literal ID="litStatus" runat="server" />
    </div>

    <div class="table-responsive">
        <asp:GridView ID="grdSource" runat="server" CssClass="table table-bordered table-hover amhsh-grid"
            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:BoundField DataField="RNUM" HeaderText="STT" />
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="OUTBOX_ORACLE_ID" HeaderText="OUTBOX ID" />
                <asp:BoundField DataField="OUTBOX_ADDRESS_ORACLE_ID" HeaderText="ADDRESS ID" />
                <asp:BoundField DataField="ATTACH" HeaderText="ATTACH" />
                <asp:TemplateField HeaderText="CONTENT">
                    <ItemTemplate>
                        <div class="amhsh-content"><%# HtmlText(Eval("CONTENT")) %></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FROM ADDRESS">
                    <ItemTemplate><%# HtmlText(Eval("FROM_ADDRESS")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RECIPIENT ADDRESS">
                    <ItemTemplate><%# HtmlText(Eval("RECIPIENT_ADDRESS")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PRIORITY" HeaderText="PRIORITY" />
                <asp:TemplateField HeaderText="SUBJECT">
                    <ItemTemplate><%# HtmlText(Eval("SUBJECT")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OUTBOX TIME">
                    <ItemTemplate><%# FormatDateTime(Eval("OUTBOX_TIME")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TPL FILE NAME">
                    <ItemTemplate><%# HtmlText(Eval("TPL_FILE_NAME")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SEND STATUS">
                    <ItemTemplate><%# HtmlText(Eval("SEND_STATUS")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EXPORTED AT">
                    <ItemTemplate><%# FormatDateTime(Eval("EXPORTED_AT")) %></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div style="text-align: right" class="pageNavTotal">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="20" />
    </div>

    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.amhsh-date').datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd/mm/yyyy'
            });
        });

        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }

        function DisplayResult(result, context) {
            if (context === 'LoadDataGrid') {
                document.getElementById('<%= grdSource.ClientID %>').innerHTML = result;
            }
        }
    </script>
</asp:Content>
