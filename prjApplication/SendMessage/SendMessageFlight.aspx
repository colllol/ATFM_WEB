<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFMNoneMenu.Master" AutoEventWireup="true"
    CodeBehind="SendMessageFlight.aspx.cs" Inherits="prjApplication.SendMessage.SendMessageFlight" %>
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
                    <div id="divGroup"><lable> &nbsp;A LANDING&nbsp; <input type="checkbox" id="chk314" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;A2 LANDING&nbsp; <input type="checkbox" id="chk401" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 2&nbsp; <input type="checkbox" id="chk315" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 3&nbsp; <input type="checkbox" id="chk316" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 4&nbsp; <input type="checkbox" id="chk317" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 5&nbsp; <input type="checkbox" id="chk318" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;QUA CANH&nbsp; <input type="checkbox" id="chk319" onchange="checkaddress_Change(this)"></lable>     
    
                </div>
    <div class="row">
        Address
    </div>
    <div class="row">
        <textarea id="txtAddress" class="wid_70" rows="5"></textarea>
    </div>
    <div class="row">
        Origin
    </div>
    <div class="row">
        <input id="txtOrigin" class="wid_250px" value="VVVVZGZX" />
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
        //GetGroupAddress(); 
GetListAddressByGroup();
         var phanCachArg = '<%= _phanCachArg%>';
        var phanCach = '<%= _phanCach%>';
        var partNo = ' ';
        var messType = ' ';
        function ddlGroupAddress_Onchange() {
            var n = document.getElementById('ddlGroupAddress').options[document.getElementById('ddlGroupAddress').selectedIndex].value

            GetArgWithPostBack(n + phanCachArg + 'btnGetAddredd', 'btnGetAddredd');
        }
        function btnSendNew_Onclick() {
            var ax = checkICAOlength();
            if (!ax) return;
            

            var bx = checkMaxlength();
            if (!bx) return;
           
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

                //$('#txtAddress').val('');
                //$('#txtAddress').val(resulf);

                var hd = 'FF',
                ads = '',
                oldValue = $('#txtAddress').val();
                if (oldValue != '') {
                    oldValue = oldValue.replace(/\,/gi, ' ');
                    var o = oldValue.split(' ');
                    if (o[0].length <= 5)
                        hd = o[0];
                }
                $('#txtAddress').val('');
               
                $('#txtAddress').val(hd + ' ' + resulf);

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
        function checkaddress_Change(ele) {
            var hd = 'FF',
                ads = '',
                oldValue = $('#txtAddress').val();
            if (oldValue != '') {
                oldValue = oldValue.replace(/\,/gi, ' ');
                var o = oldValue.split(' ');
                if (o[0].length <= 5)
                    hd = o[0];
            }
            $('#txtAddress').val('');
            $.each($("input[id^='chk']"), function (a, b) {
                if ($(b).prop('checked')) {
                    ads += $(b).attr('data-address').trim() + ' ';
                }
            })
            $('#txtAddress').val(hd + ' ' + ads);
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
        $('#txtContent').val($('#<%= divcontent.ClientID%>').html().replace('NNNN', newline + newline + newline + newline + newline + newline + 'NNNN'));
        $('#txtAddress').focus();



        function checkICAOlength() {
            if ($('#txtAddress').val().length == 0)
            {
                alert('Address is not empty!');
                return false;
            }
            var arr = $('#txtAddress').val().split(' ');
           
            var pHeader = arr[0];

            parrayHeader = 'FF SS DD GG';
            if (parrayHeader.indexOf(pHeader, 0) == -1) { alert('Address is invalid!'); return false; }
            
            for (i = 1; i < arr.length; i++) {
                if (arr[i].length>0 )
                {
                    if (arr[i].length !=8){
                        alert('Address is invalid!');
                        return false;
                    }
                }
            }
            if ($('#txtAddress').val().length >= 3800) {
                alert('Address long >3800 ky tu!');
                return false;
            }
            if ($('#txtContent').val().length == 0) {
                alert('txtContent is not empty!');
                return false;
            }

            var newline = String.fromCharCode(13, 10);
            var p_footer = 'NNNN';
            var content = $('#txtContent').val();
            
            if (content.indexOf(p_footer) == -1) { alert('Content is invalid! Not NNNN'); return false; }
           /*
            var re = new RegExp("^([A-Z0-9]{1800,})$");
            if (re.test(content)) {
                console.log("Valid");
            } else {
                console.log("Invalid");
            }
            
            */
            return true;
        }

        function GetGroupAddress() {
            var kq = '';
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>/api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GroupAddress_GetAll",
                data: {},
            }).always(function (data) {
                if (data.ListValue == null) {
                    return;
                }
                $.each(data.ListValue, function (a, b) {
                    
                        var html = '<lable> &nbsp;' + b["GROUP_NAME"] + '&nbsp; <input type=\"checkbox\" id=\"chk' + b['ID'] + '\" onchange=\"checkaddress_Change(this)\"/></lable>     ';
                        $('#divGroup').append(html);
                   

                })
            });
        }

        function GetListAddressByGroup() {
            var $request = $.ajax({
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>/api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=getGroupAddress_Ref",
                data: {},
            }).always(function (data) {
                if (data.ListValue == null) {
                    return;
                }
                var a = '', j = 0;
                $.each(data.ListValue, function (a, b) {
                    var id = '#chk' + b["G_A_M_ID"];
                    var ele = $(id),
                        ad = $(ele).attr('data-address') == null ? '' : $(ele).attr('data-address');
                    $(ele).attr('data-address', (ad + ' ' + b["ADDRESS"]));

                })
            });
        }

    </script>
    
</asp:Content>

