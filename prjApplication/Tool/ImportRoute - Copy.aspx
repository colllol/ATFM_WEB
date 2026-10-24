<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ImportRoute.aspx.cs" Inherits="prjApplication.Tool.ImportRoute" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/autocomplete.css" rel="stylesheet" />
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
        .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
        padding:0px;
        }
        .sInput{
            width: 100%;
            border: none!important;
            color: black!important;
        }
        input, textarea {
            text-transform: uppercase;
        }
    </style>
    <div class="wid_25">
        <label>Select Oper</label><input data-minlenght="1" data-control="update" type="text" id="txtOper" class="wid_250px" data-autocomplete="OPER"/>
        
    </div>
    <div class="wid_100">
        <table class="wid_100">
            <tr>
                <td><label>Via</label></td>
                <td><label>Other</label></td>
            </tr>
            <tr>
                <td><textarea data-control="update" data-minlenght="1" id="txtViaText" rows="10" class="wid_95"></textarea></td>
                <td><textarea id="txtOther" rows="10" class="wid_95"></textarea></td>
            </tr>
        </table>
        <asp:Label runat="server" ID="lblListNotFromTo"></asp:Label>
        <div class="pdb_10px"> 
            <label>Option type</label>           
            <select id="ddlType">                
                <option value="BX">Update Via</option>
                <option value="DEL">Delete and Add</option>
            </select>
            
            <select id="ddlOption">
                <option value="FromTo">Form-To</option>
                <option value="CallSign">CallSign</option>
                <option value="Craft">Craft</option>
            </select>
        </div>
        
    </div>
    <div>
        <div class="mgb_15px">
        <button id="btnSearch" type="button" onclick="btnSearch_Click()" class="btn btn-primary">Search</button>
        <button id="btnImport" type="button" onclick="btnImport_Click()" class="btn btn-primary">Import</button>
        <button id="btnImportAll" type="button" onclick="btnImportAll_Click()" class="btn btn-primary">Import One Format</button>
        <button id="btnUpdate" type="button" onclick="btnUpdate_Click()" class="btn btn-primary">Update</button>
</div>
        <table id="tblSource" class="table table-bordered">
            <thead>
                <tr>
                    <th></th>
                    <th><input id="txtSearchFrom" type="text" class="sInput" data-autocomplete="AERO" /></th>
                    <th><input id="txtSearchTo" type="text" class="sInput" data-autocomplete="AERO" /></th>
                    <th><input id="txtSearchRoute" class="sInput" type="text"/></th>
                    <th></th>
                </tr>
                <tr>
                    <th class="wid_20px">#</th>
                    <th class="wid_90px">From</th>
                    <th class="wid_90px">To</th>
                    <th>Route</th>
                    <th class="wid_50px"></th>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>
    
    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>

    <script>
        var
            phanCachArg = '<%=_phanCachArg%>',
            phanCach = '<%= _phanCach%>';
        function DisplayResult(resulf, context) {
            if (context == "btnImport_Click") {
                alert(resulf);
                btnSearch_Click();
                
            }
            if (context == "btnImportAll_Click") {
                alert(resulf);
                btnSearch_Click();

            }
            
            if (context == 'btnSearch_Click') {
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
                unLoadingData('loadding');
                ButtonState(false);
            }
            if (context == 'btnUpdate_Click') {
                alert(resulf);
                btnSearch_Click();
            }

        }
        function btnImport_Click() {
            if (checkValidCustomMinlenght('update')) {
                var obj = new Object();
                obj["TXTVIA"] = $('#txtViaText').val();
                obj["TXTOTHER"] = $('#txtOther').val();
                obj["OPER"] = $('#txtOper').val();
                obj["OPTION"] = $('#ddlOption').val();
                obj["TYPE"] = $('#ddlType').val();
                if ($('#txtOper').val()=='QTR')
				{
					console.log($('#txtViaText').val().trimStart().replace(/\s/g, ' '));
					//alert($('#txtViaText').val().trimStart().replace(/\s/g, ' '));
					obj["TXTVIA"] = $('#txtViaText').val().trimStart().replace(/\s/g, ' ');
				}
				
				GetArgWithPostBack(JSON.stringify(obj) + phanCachArg + 'btnImport_Click', 'btnImport_Click');
                ButtonState(true);
            }
        }
        function btnImportAll_Click() {
            if (checkValidCustomMinlenght('update')) {
                var obj = new Object();
                obj["TXTVIA"] = $('#txtViaText').val();
                obj["TXTOTHER"] = $('#txtOther').val();
                obj["OPER"] = $('#txtOper').val();
                obj["OPTION"] = $('#ddlOption').val();
                obj["TYPE"] = $('#ddlType').val();
				/*if ($('#txtOper').val()=='QTR')
				{
					console.log($('#txtViaText').val().trimStart().replace(/\s/g, ' '));
					//alert($('#txtViaText').val().trimStart().replace(/\s/g, ' '));
					obj["TXTVIA"] = $('#txtViaText').val().trimStart().replace(/\s/g, ' ');
				}*/
                GetArgWithPostBack(JSON.stringify(obj) + phanCachArg + 'btnImportAll_Click', 'btnImportAll_Click');
                ButtonState(true);
            }
        }
        function btnUpdate_Click() {
            //if ($('#tblSource tbody tr').length < 1) {
            //    alert('No update data!');
            //    return;
            //}
            //var _str = '';
            //_str += '{';
            //_str += '\"OPER\": \"' + $('#txtOper').val() + '\",';
            //_str += '\"Via\":['
            //$('#tblSource tbody tr').each(function (a, b) {
            //    _str += '{';
            //    _str += '\"From_Airp\": \"' + $(b).find('[id^="txtFrom"]').val() + '\",';
            //    _str += '\"To_Airp\":\"' + $(b).find('[id^="txtTo"]').val() + '\",';
            //    _str += '\"Via\":\"' + $(b).find('[id^="txtVia"]').val() + '\"';
            //    _str += '},';
            //})
            //_str = _str.substring(0, _str.length - 1);
            //_str += ']';
            //_str += '}';
            //GetArgWithPostBack(_str + phanCachArg + 'btnUpdate_Click', 'btnUpdate_Click');


           
            var $lis = $('tr[data-isUpdate="true"]');
            var $lisIns = $('tr[data-isinsert="true"]');
            if ($lis.length == 0 && $lisIns.length == 0) { alert('No update.'); return; };
            var c = 0;


            $.each($lis, function (a, b) {
                var _str = '';
                _str += '{';
                _str += '\"OPER\": \"' + $('#txtOper').val() + '\",';
                _str += '\"Via\":['
                $('#tblSource tbody tr').each(function (a, b) {
                    _str += '{';
                    _str += '\"From_Airp\": \"' + $(b).find('[id^="txtFrom"]').val() + '\",';
                    _str += '\"To_Airp\":\"' + $(b).find('[id^="txtTo"]').val() + '\",';
                    _str += '\"Via\":\"' + $(b).find('[id^="txtVia"]').val() + '\"';
                    _str += '},';
                })
                _str = _str.substring(0, _str.length - 1);
                _str += ']';
                _str += '}';
                GetArgWithPostBack(_str + phanCachArg + 'btnUpdate_Click', 'btnUpdate_Click');
            });

           
            $.each($lisIns, function (a, b) {
                var _str = '';
                _str += '{';
                _str += '\"OPER\": \"' + $('#txtOper').val() + '\",';
                _str += '\"Via\":['
                $('#tblSource tbody tr').each(function (a, b) {
                    _str += '{';
                    _str += '\"From_Airp\": \"' + $(b).find('[id^="txtFrom"]').val() + '\",';
                    _str += '\"To_Airp\":\"' + $(b).find('[id^="txtTo"]').val() + '\",';
                    _str += '\"Via\":\"' + $(b).find('[id^="txtVia"]').val() + '\"';
                    _str += '},';
                })
                _str = _str.substring(0, _str.length - 1);
                _str += ']';
                _str += '}';
                GetArgWithPostBack(_str + phanCachArg + 'btnUpdate_Click', 'btnUpdate_Click');
            });
        }
        function btnSearch_Click() {
            if ($('#txtOper').val() == '') {
                $('#txtOper').focus();
                alert('Please select Oper!');
                return;
            };
            $('#tblSource tbody tr').remove();
            preloadImg('tblSource', 'loadding', '30px', '30px');
            GetArgWithPostBack(JSON.stringify(GetObjSearch()) + phanCachArg + 'btnSearch_Click', 'btnSearch_Click');
        }
        
        function ButtonState(v) {
            $('#btnSearch').prop('disabled', v);
            $('#btnImport').prop('disabled', v);
            $('#btnImportAll').prop('disabled', v);
            $('#btnUpdate').prop('disabled', v);
        }
        function checkIsUpdate(id) {
            if ($(id).attr('data-isInsert') != undefined) return;
            $inputs = $(id).closest('tr').find('[data-oldValue]');
            var ci = 0;
            $.each($inputs, function (a, b) {
                switch ($(b).attr('type')) {
                    case 'text':
                        if ($(b).attr('data-oldValue').toUpperCase() != $(b).val().toUpperCase()) { ci++; };
                        break;
                    case 'checkbox':
                        if ($(b).attr('data-oldValue') != ($(b).prop('checked') ? '1' : '0')) ci++;
                        break;
                }
            })
            if (ci > 0) { $(id).closest('tr').attr('data-isUpdate', 'true'); $(id).closest('tr').addClass('success'); }
            else { $(id).closest('tr').attr('data-isUpdate', 'false'); $(id).closest('tr').removeClass('success'); }
        }
        function GetObjSearch() {
            var obj = new Object();
            obj['FROM_AIRP'] = $('#txtSearchFrom').val();
            obj['TO_AIRP'] = $('#txtSearchTo').val();
            obj['OPER'] = $('#txtOper').val();
            obj['FLIGHTTYPE'] = '';
            obj['VIA'] = $('#txtSearchRoute').val();
            return obj;
        }

        function btnDeleteBy_Onclick(idDelete) {
             var cf = confirm('Do you want delete ?');
             if (cf) {              
                       var url = "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=DELETEMVIA_New";
                    $.ajax({
                        async: false,
                        method: "PUT",
                        url: url,
                        data: JSON.stringify({ P_ID: idDelete,P_USER: '<%= _user.UserName%>'}),
                    }).always(function (data) {
                        if (data.ListValue == null || data.ListValue == -1) {                       
                        }
                    })
                 btnSearch_Click();
                }
                 
           
        }
       

        //btnSearch_Click();
        window.onkeydown = function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            var c = $('#tblSource tr').length;
            if (charCode == 118) {
                if ($('#txtOper').val() == '') {
                    alert('Please select Oper!')
                    $('#txtOper').focus();
                    return;
                }
                var _count = $('#tblSource tbody tr').length;
                var _tr = "<tr data-isInsert='true'>"
                    + "<td>" + (_count+1) + "</td>"
                   + "<td><input type='text' class='sInput' id='txtFrom" + _count + "'></td>"
                   + "<td><input type='text' class='sInput' id='txtTo" + _count + "'></td>"
                   + "<td><input type='text' class='sInput' id='txtVia" + _count + "'></td>"
                   + "<td><div class=\"action-buttons\"><i onclick=\"$(this).closest('tr').remove();\">Xoa</i></div></td>"
                   + "</tr>";
                
                    $('#tblSource tbody').append(_tr);                    
                    $("#tblSource tbody tr").last().find('input')[0].focus();
               
            }
            //if (charCode == 13) {
            //    $('#btnSearch').focus();
            //    $('#btnSearch').click();
            //}

        }
    </script>
</asp:Content>

