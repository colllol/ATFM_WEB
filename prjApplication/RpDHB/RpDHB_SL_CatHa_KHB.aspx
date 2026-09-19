<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RpDHB_SL_CatHa_KHB.aspx.cs" Inherits="prjApplication.RpDHB.RpDHB_SL_CatHa_KHB" %>

<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO THEO KHUNG GIỜ BAY THEO SL KẾ HOẠCH BAY</title>
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
        <div id="abcxyz" class="well well-sm" style="text-align: center;">
            <b>AIR PORT:</b>
            <asp:DropDownList ID="ddlAERO" CssClass="disabled" runat="server" Width="100px"></asp:DropDownList>
            <b>OPER:</b>
            <asp:DropDownList ID="ddlOPER" CssClass="disabled" runat="server" Width="100px"></asp:DropDownList>
            <b>FROM DATE :</b>
            <input id="txtFromDate" class="datepicker" autocomplete="off" runat="server"
                onkeypress='return check_num(this,14,event)' type="text"
                placeholder="select date" style="width: 120px;" />
            <input id="txtFromTime" data-minlenght="1" data-control="checkAccess" type="text" runat="server"
                placeholder="select time" style="width: 50px;" maxlength='4' data-number='true' value="0000" />
            <b>TO DATE :</b>
            <input id="txtToDate" class="datepicker" autocomplete="off" runat="server"
                onkeypress='return check_num(this,14,event)' type="text"
                placeholder="select date" style="width: 120px;" />
            <input id="txtToTime" data-minlenght="1" data-control="checkAccess" type="text" runat="server"
                placeholder="select time" style="width: 50px;" maxlength='4' data-number='true' value="2359" />
            <b>HOUR :</b>
            <select id="ddlTime" class="disabled" style="width: 70px;" runat="server">
                <option value="ALL">-All-</option>
                <option value="ATD">ETD</option>
                <option value="ATA">ETA</option>
            </select>
            <asp:Button ID="btnSearch" runat="server" class="btn btn-sm btn-primary" Style="width: 100px" Text="Search" OnClick="btnSearch_Click" />
            <br />
            <br />
            <asp:Button ID="btnExport" class="btn btn-sm btn-primary" runat="server" Style="width: 135px" Text="Export Excel" OnClick="btnExport_Click" />
            <asp:Button ID="btnExportWord" class="btn btn-sm btn-primary" runat="server" Style="width: 135px" Text="Export Word" OnClick="btnExportWord_Click" />
            <button id="btnPrint" type="button" class="btn btn-sm btn-primary" style="width:100px;" onclick="btnPrint_OnClick()">PRINT</button>
        </div>



        <div id="exportid" runat="server">

            <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
            <div id="contentid" runat="server">
                <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
            </div>
            <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>

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
        </script>
    </form>
</body>

</html>
