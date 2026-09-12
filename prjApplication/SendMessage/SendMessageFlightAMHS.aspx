<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFMNoneMenu.Master" AutoEventWireup="true" CodeBehind="SendMessageFlightAMHS.aspx.cs" Inherits="prjApplication.SendMessage.SendMessageFlightAMHS" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />

        <div class="row">
        <span class="TitlePanel">SEND MESSAGE</span>
            
    </div>
    <div class="row">
        <label>Group</label>
        <select id="ddlGroupAddress" onchange="ddlGroupAddress_Onchange()">
            <option value=" ">-- [Select] --</option>
            <% foreach (DataRow r in new GroupAddressDAL().GetAll().Rows) %>
              <% {%>
                <option value="<%: r["ID"] %>"><%: r["GROUP_NAME"] %></option>
               <%  } %>
        </select>
        <button id="btnSend" onclick="btnSendNew_Onclick()" class="btn btn-primary btn-sm" type="button">Send</button>
        <button id="btnCheckICAO" type="button" style="display:none;" class="btn btn-primary btn-sm" onclick="checkMessageICAO_Click()">Check ICAO 4444</button>
    </div>
    <div class="row">
        Address
    </div>
    <div class="row">
        <textarea id="txtAddress" class="wid_70" rows="3"></textarea>
    </div>
    <div class="row">
        Origin
    </div>
    <div class="row">
        <input id="txtOrigin" class="wid_250px" value="VVGLSLBT" />
    </div>
    <div class="row">
        Content
    </div>
    <div class="row">
        <textarea id="txtContent" class="wid_70" rows="15"></textarea>
    </div>
    <div id="divcontent" hidden="hidden" runat="server"></div>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script>
         var phanCachArg = '<%= _phanCachArg%>';
        var phanCach = '<%= _phanCach%>';
        var partNo = ' ';
        var messType = ' ';
        function ddlGroupAddress_Onchange() {
            var n = document.getElementById('ddlGroupAddress').options[document.getElementById('ddlGroupAddress').selectedIndex].value

            GetArgWithPostBack(n + phanCachArg + 'btnGetAddredd', 'btnGetAddredd');
        }
        function btnSendNew_Onclick() {
          
            var bx = checkMaxlength();
            if (!bx) return;
            //var ax = checkValidCustomMinlenght('mCheck');
            //if (!ax) return;
            if (confirm('Do you want confirm???')) {
                var obj = {
                    P_TOADD: $('#txtAddress').val(),
                    P_CONTENT: $('#txtContent').val()
                };
                GetArgWithPostBack($('#txtOrigin').val() + phanCach + $('#txtAddress').val() + phanCach + $('#txtContent').val() + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
            }
            else {
                return false;
            }

        }
        function btnSend_Onclick() {
            alert('Do you want confirm???');
            var bx = checkMaxlength();
            if (!bx) return;
            //var ax = checkValidCustomMinlenght('mCheck');
            //if (!ax) return;
            if (confirm('Do you want confirm???')) {
                var obj = {
                    P_TOADD: $('#txtAddress').val(),
                    P_CONTENT: $('#txtContent').val()
                };
                GetArgWithPostBack($('#txtOrigin').val() + phanCach + $('#txtAddress').val() + phanCach + $('#txtContent').val() + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
            }
            else {
                return false;
            }

        }
        function DisplayResult(resulf, context) {
            if (context == 'btnGetAddredd') {

                $('#txtAddress').val('');
                $('#txtAddress').val(resulf);
            }
            else if (context == 'btnSend_Onclick') {
                alert(resulf);
            }
            else if (context == 'RenderTopBarInfo') {
                $('#divExcuteScript').html(resulf);
            }
            else if (context == 'ViewContent') {
                $('#txtContent').val(resulf);
            }
            else if (context == 'checkMessageICAO_Click') {
                if (resulf != 'null') alert(resulf);
                else alert('no error');
            }

        }
        function checkMaxlength() {
            if ($('#txtAddress').val().length >= 3800) {
                alert('Address long >3800 ky tu!');
                return false;
            }
            if ($('#txtContent').val().length >= 1799) {
                alert('Content long >1800 ky tu!')
                return false;
            }
            return true;
        }
        function replaceAll(find, replace) {
            var result = this;
            do {
                var split = result.split(find);
                result = split.join(replace);
            } while (split.length > 1);
            return result;
        }
        function checkMessageICAO_Click() {
            GetArgWithPostBack($('#txtContent').val() + phanCachArg + 'checkMessageICAO_Click', 'checkMessageICAO_Click');
        }
        var newline = String.fromCharCode(13, 10);
        $('#txtContent').val($('#<%= divcontent.ClientID%>').html().replace('NNNN', ''));
        $('#txtAddress').focus();
    </script>
    
</asp:Content>
