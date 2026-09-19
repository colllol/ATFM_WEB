<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="MessManagement.aspx.cs" Inherits="prjApplication.MessManagement.MessManagement" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <script language="Javascript" type="text/javascript">
        function f_CallReports(valuePath) {
            SubmitImage('../' + valuePath + '', 1200, 580);
        }
    </script>
    <style type="text/css">
        .w3-example {
            background-color: #f1f1f1;
        }

        textarea, input {
            text-transform: uppercase;
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
    <div class="container-fluid">
        <div class="row w3-example" style="margin-bottom: 20px;">
            <div class="col-xs-3"></div>
            <div class="row" style="text-align: center">
                <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                    onkeypress='return check_num(this,14,event)' type="text" />
                <select id="ddlType" class="disabled" runat="server" style="width: 13%; height: 34px;" onchange="ddlType_Change();">
                    <option value="0">FLIGHTS</option>
                    <option value="1">FLIGHTS CANCEL</option>
                </select>
                <asp:Button ID="btnOption" runat="server" OnClick="btnSelect_Click" Text="Select"
                    class="btn btn-primary btn-sm" type="button"></asp:Button>

                <button type="button" id="btnNewMess" class="btn btn-sm btn-primary" style="width: 120px" onclick="f_CallReports('SendMessage/SendMessageFlight.aspx?Menu_ID=89')">
                    New Message</button>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-3 w3-example">
                <div class="scroll-area1">
                    <div class="table-responsive">
                        <table id="tblSource" class="table table-bordered table-responsive">
                            <thead>
                                <tr>
                                    <th>Part No</th>
                                    <th>Message Type</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="grdListPartMessage" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 20%; text-align: center"><%# Eval("PART_NO") %></td>
                                            <td style="width: 80%; text-align: center;">
                                                <a style="cursor: pointer; color: <%# Eval("STATUS") %>" onclick="ViewContent(<%# Eval("ID") %>, '<%# Eval("MESS_TYPE") %>')">
                                                    <%# Eval("MESS_TYPE") %></a></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                                 <asp:Repeater ID="rptCacels" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 20%; text-align: center"><%# Eval("PART_NO") %></td>
                                            <td style="width: 80%; text-align: center;">
                                                <a style="cursor: pointer; color: <%# Eval("STATUS") %>" onclick="ViewContentCL(<%# Eval("ID") %>, '<%# Eval("MESS_TYPE") %>')">
                                                    <%# Eval("MESS_TYPE") %></a></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div id="abcxyz" class="col-xs-9 " style="padding-left: 40px;">

                <div class="row">
                    <div id="divGroup"><lable> &nbsp;A LANDING&nbsp; <input type="checkbox" id="chk314" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;A2 LANDING&nbsp; <input type="checkbox" id="chk401" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 2&nbsp; <input type="checkbox" id="chk315" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 3&nbsp; <input type="checkbox" id="chk316" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 4&nbsp; <input type="checkbox" id="chk317" onchange="checkaddress_Change(this)"></lable>     <lable> &nbsp;ADDRESS 5&nbsp; <input type="checkbox" id="chk318" onchange="checkaddress_Change(this)"></lable>  <lable> &nbsp;ADDRESS 6&nbsp; <input type="checkbox" id="chk441" onchange="checkaddress_Change(this)"></lable>    <lable> &nbsp;QUA CANH&nbsp; <input type="checkbox" id="chk319" onchange="checkaddress_Change(this)"></lable>     
    </div>
                </div>
                <div class="row" style="height: 10px;">
                </div>
                <div class="row">
                    <label style="padding-left: 2px">
                        HVN
                        <input type="checkbox" id="chkVN" value="1" />
                        LANDING
                        <input type="checkbox" id="chkLD" value="2" />
                        OVER
                        <input type="checkbox" id="chkOF" value="3" />
                    </label>
                </div>
                <div class="row" style="height: 10px;">
                </div>
                <div class="row">

                    <button id="btnSend" onclick="btnOnclick()" class="btn btn-primary btn-sm" type="button">
                        Send</button>
                    <button id="btnSendAll" onclick="btnOnclickAll()" class="btn btn-primary btn-sm"
                        type="button">
                        Send All</button>

                </div>

                <div class="row">
                    Address
                </div>
                <div class="row">
                    <textarea id="txtAddress" class="wid_70" rows="3" data-minlenght="8" data-control="mCheck"></textarea>
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
                    <textarea id="txtContent" class="wid_70" data-minlenght="50" data-control="mCheck" rows="15"></textarea>
                </div>

            </div>
        </div>
    </div>
    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
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
        function btnOnclickAll() {
            //var hd = '',
            //    dd = $('#ctl00_MainContent_txtBEGINDATE').val();
            //var hd = prompt('Do you want send all message date: ' + dd + '. ' + (hd == '' ? 'Header: FF' : 'Header: ' + hd) + '???', '');

            //var obj = {
            //    P_HEADER: hd == '' ? 'FF' : hd,
            //    P_DATE: dd == '' ? new Date().format('yyyy/mm/dd') : dd.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3/$2/$1')
            //};
            //alert(hd + phanCach + (dd == '' ? new Date().format('yyyy/mm/dd') : dd.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3/$2/$1')) + phanCachArg + 'btnOnclickAll');
            // preloadImgAfterButton('btnSendAll', 'loaddingSendAll');

            vn = 0;
            ld = 0;
            of = 0;
            if ($('#chkVN').prop("checked") == true) {
                vn = 1;
            }
            if ($('#chkLD').prop("checked") == true) {
                ld = 1;
            }
            if ($('#chkOF').prop("checked") == true) {
                of = 1;
            }

            dd = $('#ctl00_MainContent_txtBEGINDATE').val();
            var bx = checkMaxlength();
            if (!bx) return;
            //var ax = checkValidCustomMinlenght('mCheck');
            //if (!ax) return;
            if (confirm('Do you want confirm???')) {
                var obj = {
                    P_TOADD: $('#txtAddress').val(),
                    P_CONTENT: $('#txtContent').val()
                };
                preloadImgAfterButton('btnSendAll', 'loaddingSendAll');
                GetArgWithPostBack(vn + phanCach + ld + phanCach + of + phanCach + $('#txtAddress').val() + phanCach + $('#txtOrigin').val() + phanCach + (dd == '' ? new Date().format('yyyy/mm/dd') : dd.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3/$2/$1')) + phanCachArg + 'btnOnclickAll', 'btnOnclickAll');
            }
            else {
                return false;
            }
            //GetArgWithPostBack(hd + phanCach + (dd == '' ? new Date().format('yyyy/mm/dd') : dd.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3/$2/$1')) + phanCachArg + 'btnOnclickAll', 'btnOnclickAll');
        }
        function confirmAction() {
            if (confirm('Do you want confirm???')) {
                return true;
            }
            else {
                return false;
            }
        }
        function btnOnclick() {
            var bx = checkMaxlength();
            if (!bx) return;
            var ax = checkValidCustomMinlenght('mCheck');
            if (!ax) return;
            if (confirm('Do you want confirm???')) {
                var obj = {
                    P_TOADD: $('#txtAddress').val(),
                    P_CONTENT: $('#txtContent').val()
                };
				
                GetArgWithPostBack(partNo + phanCach + messType + phanCach + $('#txtAddress').val() + phanCach + $('#txtContent').val() + phanCach + $('#txtOrigin').val() + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
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
            else if (context == 'ViewContentCL') {
                $('#txtContent').val(resulf);
            }
            else if (context == 'btnOnclickAll') {
                unLoadingData('loaddingSendAll');
                alert(resulf);
            }
        }
        function ViewContent(cl, u) {
            partNo = cl;
            messType = u;
            GetArgWithPostBack(cl + phanCach + u + phanCachArg + 'ViewContent', 'ViewContent');
        }
        function ViewContentCL(cl, u) {
            partNo = cl;
            messType = u;
            GetArgWithPostBack(cl + phanCach + u + phanCachArg + 'ViewContentCL', 'ViewContentCL');
        }
    </script>
    <script language="javascript" type="text/javascript">
        
        <%--$('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });--%>
        var _oY = $('#abcxyz').offset().top;
        $(window).scroll(function () { //detact scroll
            if ($(window).scrollTop() >= 330) { //scrolled to bottom of the page
                $('#abcxyz').offset({ top: $(window).scrollTop() });
            }
            else {
                $('#abcxyz').offset({ top: _oY });
            }
        });
    </script>
    <script>
        function GetGroupAddress() {
            var kq = '';
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GroupAddress_GetAll",
                data: {},
            }).always(function (data) {
                console.log(data.ListValue);
		if (data.ListValue == null) {
                    return;
                }
		
                $.each(data.ListValue, function (a, b) {

                    var html = '<lable> &nbsp;' + b["GROUP_NAME"] + '&nbsp; <input type=\"checkbox\" id=\"chk' + b['ID'] + '\" onchange=\"checkaddress_Change(this)\"/></lable>     ';
                    $('#divGroup').append(html);
                })
            });
        }

        function GetGroupAddressOver() {
            var kq = '';
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GroupAddress_GetAll",
                data: {},
            }).always(function (data) {
                if (data.ListValue == null) {
                    return;
                }
                $.each(data.ListValue, function (a, b) {
                    if (b['ID'] == '319') {
                        var html = '<lable> &nbsp;' + b["GROUP_NAME"] + '&nbsp; <input type=\"checkbox\" id=\"chk' + b['ID'] + '\" onchange=\"checkaddress_Change(this)\"/></lable>     ';
                        $('#divGroupOver').append(html);
                    }

                })
            });
        }
	



        function GetListAddressByGroup() {
            var $request = $.ajax({
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=getGroupAddress_Ref",
                data: {},
            }).always(function (data) {
               
		if (data.ListValue == null) {
                    return;
                }
		console.log(data.ListValue);
                var a = '', j = 0;
                $.each(data.ListValue, function (a, b) {
                    var id = '#chk' + b["G_A_M_ID"];
                    console.log(id);
		    var ele = $(id),
                        ad = $(ele).attr('data-address') == null ? '' : $(ele).attr('data-address');
		    
		   
			
                    $(ele).attr('data-address', (ad + ' ' + b["ADDRESS"]));
		    	
                })
            });
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
                   console.log($(b)); 
	           ads += $(b).attr('data-address').trim() + ' ';
 		   //ads += $(b).attr('data-address') + ' ';
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
	
        
	
        
     //GetGroupAddress();    
    </script>
    <script>
   
GetListAddressByGroup();
	  </script>	
</asp:Content>

