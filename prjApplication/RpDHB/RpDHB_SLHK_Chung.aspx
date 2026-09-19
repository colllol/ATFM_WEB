<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RpDHB_SLHK_Chung.aspx.cs" Inherits="prjApplication.RpDHB.RpDHB_SLHK_Chung" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO SỐ LIỆU BAY HÀNG KHÔNG CHUNG</title>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/StyleRpDHB.css" rel="stylesheet" />

     <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <style>
        #tblSource input, select {
            color: black;
        }

        .table > thead > tr {
            background-color: blue;
            background-image: none;
            color: black;
        }

        .cssTrung {
            color: seagreen !important;
        }

        #tblSource > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
            padding: unset;
        }

        #tblSource tr > td, tr > th {
            padding: unset;
            vertical-align: middle;
            text-align: center;
        }

        .sInput {
            /*border-width: 0px !important;*/
            border: 0px;
            width: 100%;
        }

        input[type=text] {
            border: 0px;
        }

        .success {
            background-color: blue;
        }

        .rowCreate {
            background-color: mediumvioletred;
        }

        input, select, label, textarea {
            text-transform: uppercase;
        }

        caption {
            text-align: left;
            padding-bottom: 0px;
            padding-top: 0px;
        }

        #abcxyz.report-toolbar {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            justify-content: center;
            gap: 6px;
        }

        .activity-days-backdrop {
            position: fixed;
            z-index: 1050;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            display: none;
            align-items: center;
            justify-content: center;
            padding: 20px;
            background: rgba(0, 0, 0, 0.55);
        }

        .activity-days-backdrop.is-open {
            display: flex;
        }

        .activity-days-dialog {
            display: flex;
            flex-direction: column;
            width: 720px;
            max-width: calc(100% - 32px);
            max-height: calc(100vh - 40px);
            overflow: hidden;
            border: 1px solid #c9d5df;
            border-radius: 6px;
            background: #fff;
            box-shadow: 0 16px 45px rgba(0, 0, 0, 0.28);
            text-align: left;
        }

        .activity-days-header,
        .activity-days-footer {
            display: flex;
            align-items: center;
            gap: 10px;
            padding: 12px 16px;
            background: #f5f7f9;
        }

        .activity-days-header {
            justify-content: space-between;
            border-bottom: 1px solid #d8e0e7;
        }

        .activity-days-header h3 {
            margin: 0;
            color: #244763;
            font-size: 18px;
            font-weight: 700;
            letter-spacing: 0;
        }

        .activity-days-close {
            width: 34px;
            height: 34px;
            padding: 0;
            border: 0;
            background: transparent;
            color: #52677a;
            font-size: 26px;
            line-height: 34px;
        }

        .activity-days-body {
            min-height: 180px;
            overflow: auto;
            padding: 14px 16px;
        }

        .activity-days-summary {
            margin-bottom: 12px;
            color: #425b70;
            font-size: 13px;
        }

        .activity-days-table {
            width: 100%;
            margin: 0;
            border-collapse: collapse;
        }

        .activity-days-table th,
        .activity-days-table td {
            padding: 8px 10px !important;
            border: 1px solid #cbd6df;
            text-align: center;
        }

        .activity-days-table th {
            position: sticky;
            top: 0;
            background: #337ab7 !important;
            color: #fff !important;
        }

        .activity-days-footer {
            justify-content: flex-end;
            border-top: 1px solid #d8e0e7;
        }

        @media (max-width: 760px) {
            .activity-days-backdrop {
                padding: 10px;
            }

            .activity-days-dialog {
                max-width: 100%;
                max-height: calc(100vh - 20px);
            }
        }
    </style>
    <style>
        .preloader {
            display: inline-block;
            padding: 0px;
            border-radius: 100%;
            border: 2px solid;
            border-top-color: rgba(0,0,0, 0.65);
            border-bottom-color: rgba(0,0,0, 0.15);
            border-left-color: rgba(0,0,0, 0.65);
            border-right-color: rgba(0,0,0, 0.15);
            -webkit-animation: preloader 0.8s linear infinite;
            animation: preloader 0.8s linear infinite;
        }

        @keyframes preloader {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        @-webkit-keyframes preloader {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }
    </style>
    <style>
        #tblSource tbody tr.select input {
            background-color: darkseagreen;
        }

        #tblSource tbody tr.select {
            background-color: darkseagreen;
        }
    </style>
     </head>
<body>
    <form id="form2" runat="server">
    <div id="abcxyz" class="well well-sm report-toolbar" style="text-align: center;">
        <b>FROM DATE :</b>
        <input id="txtFromDate" class="datepicker" autocomplete="off" runat="server"
            onkeypress='return check_num(this,14,event)' type="text"
            placeholder="select date" />
        <b>TO DATE :</b>
        <input id="txtToDate" class="datepicker" autocomplete="off" runat="server"
            onkeypress='return check_num(this,14,event)' type="text"
            placeholder="select date" />
        <asp:Button ID="btnSearch" runat="server" class="btn btn-sm btn-primary" Style="width: 100px" Text="Search" OnClick="btnSearch_Click" />
        <asp:Button ID="btnExport" class="btn btn-sm btn-primary" runat="server" Style="width: 135px" Text="Export Excel" OnClick="btnExport_Click" />
        <asp:Button ID="btnExportWord" class="btn btn-sm btn-primary" runat="server" Style="width: 135px" Text="Export Word" OnClick="btnExportWord_Click" />
        <asp:Button ID="btnCountActiveDays" class="btn btn-sm btn-primary" runat="server" Style="width: 175px" Text="Đếm ngày hoạt động" OnClick="btnCountActiveDays_Click" />
        <button id="btnPrint" type="button" class="btn btn-sm btn-primary" style="width:100px;" onclick="btnPrint_OnClick()">PRINT</button>
    </div>

    <div id="exportid" runat="server">

        <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
        <div id="contentid" runat="server">
            <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
        </div>
        <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>

    </div>

    <div id="activityDaysModal" class="activity-days-backdrop" role="presentation" aria-hidden="true"
        onclick="closeActivityDaysModalOnBackdrop(event)">
        <div class="activity-days-dialog" role="dialog" aria-modal="true" aria-labelledby="activityDaysTitle">
            <div class="activity-days-header">
                <h3 id="activityDaysTitle">Đếm ngày hoạt động sân bay</h3>
                <button type="button" class="activity-days-close" title="Đóng" aria-label="Đóng"
                    onclick="closeActivityDaysModal()">&times;</button>
            </div>
            <div class="activity-days-body">
                <div class="activity-days-summary">
                    <asp:Literal ID="ltrActiveDaysSummary" runat="server"></asp:Literal>
                </div>
                <asp:GridView ID="grdActiveDays" runat="server" AutoGenerateColumns="false"
                    CssClass="activity-days-table" GridLines="None"
                    EmptyDataText="Không có dữ liệu hoạt động sân bay trong khoảng ngày đã chọn.">
                    <Columns>
                        <asp:BoundField DataField="STT" HeaderText="STT" />
                        <asp:BoundField DataField="AIRPORT_CODE" HeaderText="Sân bay" />
                        <asp:BoundField DataField="ACTIVE_DAYS" HeaderText="Số ngày hoạt động" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="activity-days-footer">
                <button type="button" class="btn btn-sm btn-default" onclick="closeActivityDaysModal()">Đóng</button>
                <asp:Button ID="btnExportActiveDays" runat="server" class="btn btn-sm btn-primary"
                    Text="Xuất Excel" Enabled="false" OnClick="btnExportActiveDays_Click" />
            </div>
        </div>
    </div>

    <script language="javascript" type="text/javascript">

        $('#<%= txtFromDate.ClientID %>').datetimepicker({
            timepicker: false,
            formatTime: '',
            format: 'd-m-Y',
            formatDate: 'd-m-Y'
        });
        $('#<%= txtToDate.ClientID %>').datetimepicker({
            timepicker: false,
            formatTime: '',
            format: 'd-m-Y',
            formatDate: 'd-m-Y'
        });
        function btnPrint_OnClick() {
            var content = document.getElementById('exportid').innerHTML;
            var newline = String.fromCharCode(13, 10);
            Popup(content);
        }
        function Popup(data) {
            var mywindow = window.open('', 'print page', 'height=800,width=600');
            mywindow.document.write('<html><head><title>print page</title>');
            mywindow.document.write('<link rel="stylesheet" href="../Style/assets/css/bootstrap.min.css" type="text/css" /><link rel="stylesheet" href="../Style/StyleRpDHB.css" type="text/css" />');
            mywindow.document.write('</head><body >');
            mywindow.document.write(data);
            mywindow.document.write('</body></html>');
            mywindow.print();
            mywindow.close();
            return true;
        }

        function openActivityDaysModal() {
            $('#activityDaysModal').addClass('is-open').attr('aria-hidden', 'false');
            $('body').css('overflow', 'hidden');
        }

        function closeActivityDaysModal() {
            $('#activityDaysModal').removeClass('is-open').attr('aria-hidden', 'true');
            $('body').css('overflow', '');
        }

        function closeActivityDaysModalOnBackdrop(e) {
            e = e || window.event;
            var target = e.target || e.srcElement;
            if (target && target.id === 'activityDaysModal') {
                closeActivityDaysModal();
            }
        }

        $(document).keydown(function (e) {
            if (e.keyCode === 27) {
                closeActivityDaysModal();
            }
        });
    </script>
</form>
</body>

</html>
