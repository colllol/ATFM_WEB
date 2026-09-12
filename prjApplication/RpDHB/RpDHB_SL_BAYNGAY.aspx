<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RpDHB_SL_BAYNGAY.aspx.cs" Inherits="prjApplication.RpDHB.RpDHB_SL_BAYNGAY" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO SỐ LIỆU BAY</title>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/StyleRpDHB.css" rel="stylesheet" />

    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>



</head>
<body>
    <form id="form2" runat="server">
    
    <div id="abcxyz" class="well well-sm" style="text-align: center;">
        <b>FROM DATE :</b>
        <input id="txtFromDate"  class="datepicker" autocomplete="off" runat="server"
            onkeypress='return check_num(this,14,event)' type="text"
            placeholder="select date" />
        <b>TO DATE :</b>
        <input id="txtToDate" class="datepicker" autocomplete="off" runat="server"
            onkeypress='return check_num(this,14,event)' type="text"
            placeholder="select date" />
        <asp:Button ID="btnSearch" runat="server" class="btn btn-sm btn-primary" Style="width: 100px" Text="Search" OnClick="btnSearch_Click" />
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
