<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListVia.aspx.cs" Inherits="prjApplication.FinishFlights.ListVia" %>

<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        #tblSource input, select {
            color: black;
        }

        .table > thead > tr {
            background-color: #fff;
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
    <div id="abcxyz" class="well well-sm" style="text-align: center;">
        <b>FROM :</b>
        <input id="txtFromDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" />
       
        <b>TO :</b>
        <input id="txtToDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" />
        
		<b>FROM AIR :</b>
        <input id="txtFromAir" data-autocomplete="AERO" type="text"
            class="wid_100px" />

        <b>TO AIR:</b>
        <input id="txtToAir" data-autocomplete="AERO" type="text"
            class="wid_100px" />

		<b>OPER:</b>
        <input id="txtOPER_ID" data-autocomplete="OPER" type="text"
            class="wid_100px" />
			
        <b class="hidden">P_SIZE :</b>
        <select id="ddlPageSize" class="disabled hidden" style="width: 65px;">
            <option value="100">100</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
            <option value="2000">2000</option>            
            <option value="4000">4000</option>
            <option value="6000">6000</option>
            <option value="8000">8000</option>
        </select>
        <button type="button" id="btnSearch" class="btn btn-sm btn-primary" style="width: 100px" onclick="btnSearch_OnClick()">
            Search</button>
		<button type="button" id="btnExport" class="btn btn-sm btn-primary" style="width: 135px" onclick="LoadDataGrid_Export()">
            Export Excel</button>
        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <div id="abcxyzd" class="well well-sm hidden" style="text-align: left;">
        <button type="button" id="btnUpdateList" class="btn btn-sm btn-primary" style="width: 90px" onclick="btnUpdateList_Onclick()">
            Update</button>
        <button type="button" id="btnDeleteByChecked" class="btn btn-sm btn-primary" style="width: 90px" onclick="btnDeleteByChecked_Onclick()">
            Delete</button>

        <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" style="width: 90px" onclick="btnClearValue_OnClick()">
            Clear Search</button>

       


    </div>


    <label class="radio-inline">
        <span id="totalsfinished">Tổng số : <b>0</b></span></label>
    <div id="exportid" runat="server">

        <table id="tblSource" class="table table-bordered">
           <thead style="color: red"><tr> <th>NO</th>
		   <th>PERMNBR</th><th>OPER</th>
		   <th>CALLSIGN</th><th>CRAFT</th><th>PURPOSE</th><th>P_ TYPE</th><th>FROM</th><th>TO</th><th>FLIGHTDATE</th><th>ETD</th><th>ETA</th><th>VIA</th><th>REMARK</th></tr></thead>
            <tbody>
			
			</tbody>
        </table>

    </div>
    <br />
    <asp:HiddenField ID="hfGridHtml" runat="server" />
    <iframe id="txtArea1" style="display: none"></iframe>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>

    <script>
        var isTarget = true;
        var qEdit = '<%= _Role.R_Edit %>';
        var qDel = '<%= _Role.R_Del %>';       
        var ddlPageSize = document.getElementById('ddlPageSize');       
        var isSearch = false;
        var rowId = 0;
        var pageSize = parseInt(ddlPageSize.value);
        $('#tblSource').paging({ pageSize: pageSize });
        function Render2Table(data) {

            var kq = '';
            $('#tblSource').paging({ pageSize: pageSize });
            var idx = parseInt($('#tblSource').attr('data-pageindex'));           
            var pz = parseInt(ddlPageSize.value);           
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr id='" + b.flight_id + "' data-isUpdate='false' onmouseover='rowId=" + b.flight_id + ";' onmouseout='rowId=0;'>"

                    + "<td id='b_" + b.RNUM + "'>" + stt + "</td>"
                    + "<td><input type=\"checkbox\" id='chk_" + b.flight_id + "' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP" + a + "' data-oldValue='" + b.FROM_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.FROM_AIRP + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP" + a + "' data-oldValue='" + b.TO_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.TO_AIRP + "' onblur='checkIsUpdate(this)'/></td>"                    
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA" + a + "' data-oldValue='" + returnEmpty(b.VIA).replace(/\n/gi, '') + "' value='" + returnEmpty(b.VIA) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtPERMTYPE" + a + "' data-oldValue='" + returnEmpty(b.PERMTYPE) + "' value='" + returnEmpty(b.PERMTYPE) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td style=\"white-space: nowrap;\">"
                    + "<div class=\"action-buttons\">"
                    + "<i id=\"btnDeleteRemark" + b.ID + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"btnDeleteOnclick(" + b.ID + ");\"></i>"
                    + "</td>"
                    + "</tr>"
                stt++;
            });
            return kq;
        }

     
        function returnTd_Remark(val, el, a) {
            if (val != "") {
                var ax = "<div class='ABC'>";
                ax += "<input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "' data-oldValue='"
                    + returnEmpty(val) + "' value='" + returnEmpty(val) + "' onblur='checkIsUpdate(this)'/>"
                    + "<span class='ABCD glyphicon glyphicon-remove' onclick=\"$('#" + el + "').val(''); $('#" + el + "').focus();\" data-toggle='tooltip' title='Delete remark' data-original-title='Delete remark' ></span>"
                    + "</div>";
                return ax;
            }
            return "<input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "' data-oldValue='" + returnEmpty(val) + "' value='" + returnEmpty(val) + "' onblur='checkIsUpdate(this)'/>";
        }

        function returnEmpty(val) {
            return val == null ? "" : val;
        }
        function LoadDataGrid() {
			 
            var _urlPath = "";
           
            _urlPath= "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetDayFlight2021"
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify({ P_DATE: $('#txtFromDate').val(), P_EDATE: $('#txtToDate').val(), P_FROM_AIRP: $('#txtFromAir').val(),P_TO_AIRP: $('#txtToAir').val() ,P_OPER_ID: $('#txtOPER_ID').val() }),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove(); 
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    
					
					unLoadingData('loaddingData');
                    reloadCheckValid();
                    /*$('#tblSource').paging({
                        onClickButton: 'LoadDataGrid',
                        pageSize: pageSize,
                    });*/                    
                    $("#tblSource td").click(function () {
                        $('#tblSource tbody tr').removeClass('select');
                        $('#tblSource tbody tr').eq(parseInt($(this).parent().index())).addClass('select');
                    });
                },
            }).always(function (data) {
                //console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
					$('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
					
                $('#tblSource').attr('data-total', data.ListValue[0]['SUMRECORD']);
				var strAppend = RenderTableKhExport(data);
				
						
              
                $('#tblSource tbody').append(strAppend);
                $("#totalsfinished").html("Tổng số : <b>" + data.ListValue[0]['SUMRECORD'] + "</b>");
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

      
        function GetObjectSearch() {
            var _obj = {};
            if (isSearch) {
                $('#tblSource').attr('data-pageIndex', 1);
            }
            _obj['PAGESIZE'] = parseInt(ddlPageSize.value);
            _obj['PAGEINDEX'] = parseInt($('#tblSource').attr('data-pageIndex') - 1);           
            _obj['FROM_AIRP'] = $('#txtFROM_AIRP').val();
            _obj['TO_AIRP'] = $('#txtTO_AIRP').val();           
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();            
            _obj['VIA'] = $('#txtVIA').val();            
            isSearch = false;
            return _obj;
        }
        function reloadCheckValid() {
            $(document).ready(function () {
                $('[data-number="true"]').keypress(validateNumber);
                $('[data-minlenght]').keyup(validateEmty1);
                $('[data-minlenght]').each(function () {
                    var el = $(this)[0];
                    var val = el.getAttribute('data-minlenght');
                    if (el.value.length < val) {
                        el.style.border = "red solid 1px";
                    }
                    else {
                        el.style.border = "0px";
                    }
                });
            });
            try {
                $('[data-CheckDate="true"]').each(function () {
                    $(this).on('blur', function () {
                        checkInputDate1($(this));
                    });
                });
            } catch (e) {

            }
        }
        function validateEmty1(event) {
            var el = $(this)[0];
            var val = el.getAttribute('data-minlenght');
            if (el.value.length < val) {
                el.style.border = "red solid 1px";
            } else { try { el.style.border = "0px"; } catch (er) { } }
        }
        function checkInputDate1(ele) {
            var $ele = $(ele);
            var v = $(ele).val();
            if (v.length < 8) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 8)
                v = $ele.val().replace(/^(\d{2})(\d{2})(\d{4})$/, '$1/$2/$3');
            if (v.length > 10 || v.length == 9) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 10) {
                v = v.replace(/-/g, '/');
                var d = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$1');
                var m = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$2');
                var y = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3');
                var chk = new Date(y + '/' + m + '/' + d);
                if (chk == 'Invalid Date') {
                    $ele.css('border', '1px solid red');
                    $ele.val('');
                }
                else {
                    $ele.css('border', '');
                    $ele.val(chk.format('dd-mm-yyyy'));
                }
            }
        }

        function onmouseoverInput(id) {
            inputId = id;
        }
        function onmouseoutInput(id) {
            inputId = '';
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
        function binAutocomplete(id, v) {
            var $id = $(id).prop('id');
            var axx = $($id).attr('data-AutoComplete');
            switch (v) {
                case "OPER":
                    $($id).autocomplete({
                        source: [listOper],
                    }).on('blur', function (e, datum) {
                        CheckListOper($(id));
                    });
                    break;
                case "CRAFT":
                    $(id).autocomplete({
                        titleKey: 'name',
                        valueKey: 'name',
                        source: [{
                            data: listCraft,
                        }],
                    }).on('selected.xdsoft', function (e, data) {
                        $($id).val(data.name);
                    }).on('blur', function (e, datum) {
                        CheckCraft($(id));
                    });
                    break;
                case "AERO":
                    $(id).autocomplete({
                        titleKey: 'AE_REM',
                        valueKey: 'AE_REM',
                        source: [{ data: listAero, }],
                    }).on('blur', function (e, datum) {
                        CheckListAero($(id));
                    });
                    break;
                case "PURPOSE":
                    $(id).autocomplete({
                        source: [listPurpose],
                    }).on('blur', function (e, datum) {
                        CheckPurpose($(id));
                    });
                    break;

            }
            $(id).focus();
        }
    </script>

    <script>
        function btnInsertFlight_Onclick() {
            var $lis = $('tr[isInsertFlight="true"]');
            if ($lis.length == 0) { alert('No Insert.'); return; };
            var c = 0;
            $.each($lis, function (a, b) {
                var _obj = {};
                _obj['FLIGHT_ID'] = '0';
                _obj['FLIGHTDATE'] = $('#txtFLIGHTDATE').val() == '' ? new Date().format('yyyy-mm-dd') : $('#txtFLIGHTDATE').val().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
                _obj['REGISTRATION'] = $('#txtREGISTRATION').val();
                _obj['FROM_AIRP'] = $('#txtFROM_AIRP').val();
                _obj['TO_AIRP'] = $('#txtTO_AIRP').val();
                _obj['PURPOSE'] = $('#txtPURPOSE').val();
                _obj['VALIDHOURS'] = '';
                _obj['OPER_ID'] = $('#txtOPER_ID').val();
                _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
                _obj['FLIGHT_TYPE'] = '';
                _obj['CRAFT_TYPE'] = $('#txtCRAFT_TYPE').attr('data-craftid');
                _obj['CRAFT_ID'] = $('#txtCRAFT_ID').attr('data-craftid');
                _obj['REAL_CRAFT_TYPE'] = $('#txtREALCRAFT').val();
                _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
                _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
                _obj['VIA'] = $('#txtVIA').val();
                _obj['FPL_VIA'] = $('#txtFPLVIA').val();
                _obj['REMARK'] = $('#txtREMARK').val();
                _obj['ETA'] = $('#txtETA').val();
                _obj['ETD'] = $('#txtETD').val();
                _obj['ATA'] = $('#txtATA').val();
                _obj['ATD'] = $('#txtATD').val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';

                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/FinishedFlights/Insert",
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            if (c > 0) {
                alert('Insert sussess: ' + c + '/' + $lis.length);
                btnClearValue_OnClick();
                btnSearch_OnClick();
            }


        }

        function btnInsertList_Onclick() {
            var $lis = $('tr[data-isInsert="true"]');
            if ($lis.length == 0) { alert('No Insert.'); return; };
            var d = checkValidCustomMinlenght('_updateAll');
            if (!d) return;
            var c = 0;
            $.each($lis, function (a, b) {
                var _obj = {};
                _obj['FLIGHT_ID'] = $(b).prop('id');
                _obj['FLIGHTDATE'] = $($(b).find('[id^="txtFLIGHTDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PERMTYPE'] = $($(b).find('[id^="txtPERMTYPE"]')[0]).val();
                _obj['PURPOSE'] = $($(b).find('[id^="txtPURPOSE"]')[0]).val();
                _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['VALIDHOURS'] = '';
                _obj['FLIGHT_TYPE'] = '';
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtPERMCRAFT"]')[0]).attr('data-craftid');
                _obj['REAL_CRAFT_TYPE'] = $($(b).find('[id^="txtREALCRAFT"]')[0]).val();
                _obj['ATA'] = $($(b).find('[id^="txtATA"]')[0]).val();
                _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['FPL_VIA'] = $($(b).find('[id^="txtFPLVIA"]')[0]).val();
                _obj['OPER_ID'] = $($(b).find('[id^="txtOPER_ID"]')[0]).val();
                _obj['ATD'] = $($(b).find('[id^="txtATD"]')[0]).val();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['PERMNBR'] = $($(b).find('[id^="txtPERMNBR"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/FinishedFlights/Insert",
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            alert('Insert sussess: ' + c + '/' + $lis.length);
            btnSearch_OnClick();
        }

        function btnUpdateList_Onclick() {
            var $lis = $('tr[data-isUpdate="true"]');
            if ($lis.length == 0) { alert('No update.'); return; };
            var d = checkValidCustomMinlenght('_updateAll');
            if (!d) return;
            var c = 0;
            $.each($lis, function (a, b) {
                var _obj = {};
                _obj['FLIGHT_ID'] = $(b).prop('id');
                _obj['FLIGHTDATE'] = $($(b).find('[id^="txtFLIGHTDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['FLIGHTNBR'] = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _obj['PERMTYPE'] = $($(b).find('[id^="txtPERMTYPE"]')[0]).val();
                _obj['PURPOSE'] = $($(b).find('[id^="txtPURPOSE"]')[0]).val();
                _obj['TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['LASTUSER'] = '<%= _user.UserName.ToString()%>';
                _obj['REGISTRATION'] = $($(b).find('[id^="txtREGISTRATION"]')[0]).val();
                _obj['REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                _obj['VALIDHOURS'] = '';
                _obj['FLIGHT_TYPE'] = '';
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtPERMCRAFT"]')[0]).attr('data-craftid');
                _obj['REAL_CRAFT_TYPE'] = $($(b).find('[id^="txtREALCRAFT"]')[0]).val();
                _obj['ATA'] = $($(b).find('[id^="txtATA"]')[0]).val();
                _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['FPL_VIA'] = $($(b).find('[id^="txtFPLVIA"]')[0]).val();
                _obj['OPER_ID'] = $($(b).find('[id^="txtOPER_ID"]')[0]).val();
                _obj['ATD'] = $($(b).find('[id^="txtATD"]')[0]).val();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['PERMNBR'] = $($(b).find('[id^="txtPERMNBR"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/FinishedFlights/Update",
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            alert('Update sussess: ' + c + '/' + $lis.length);
            btnSearch_OnClick();
        }

      
        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result) {
                var $request = $.ajax({
                    async: false,
                    method: "DELETE",
                    url: urlApi + "api/FinishedFlights/Delete/" + id,
                }).always(function (data) {
                    if (data.Code != -1) {
                        $('#' + id).remove();
                        alert('Delete sussess!');
                        btnSearch_OnClick();
                    } else alert('Delete error!');
                });
            }
        }
        function btnSearch_OnClick() {
         LoadDataGrid();
        }

        
        function btnSearch() {
            pageSize = parseInt(ddlPageSize.value);
            $("#totalsfinished").html("Tổng số : <b>0</b>");
            isSearch = true;
            LoadDataGrid();

        }

        function btnClearValue_OnClick() {           
            $('#txtFROM_AIRP').val('');
            $('#txtTO_AIRP').val('');           
            $('#txtPERMTYPE').val('');          
            $('#txtVIA').val('');           
        }
       

    </script>


    <script>
        (function ($) {
            var $cssStyle = $('<style id="checktipCssstyle">'
                        + '.ABC{ position: relative; display: inline-block;}'
                        + '.ABCD {position: absolute; top: 0px; right: 3px; color: red; font-size: 12px; z-index: 1; white-space: nowrap; height: 10px; width: 10px; tabindex="-1" }'
                        + '</style>');
            if ($('#checktipCssstyle')[0] == undefined)
                $('body').append($cssStyle);
            function preIns(that) {
                var createTip = function (ele) {

                    var $ele = $(ele),
                        _hasDiv = $ele.parent().hasClass('ABC'),
                        _hasSpan = $ele.next().hasClass('ABCD'),
                        isNumber = false, isDate = false, isMinlength = false, isMaxlength = false;
                    var $div = _hasDiv ? $ele.parent() : $('<div class="ABC">');
                    var $span = _hasSpan ? $ele.next() : $('<span class="ABCD glyphicon glyphicon-remove">');
                    if ($ele.attr('data-contenttip') != undefined) {
                        $span.attr('data-toggle', 'tooltip');
                        $span.attr('title', $ele.attr('data-contenttip'));
                        $span.tooltip();
                    } else $span.attr('title', '');
                    if (!_hasDiv) { $ele.wrap($div); $ele.focus(); }
                    if (!_hasSpan) { $ele.after($span); $ele.focus(); }
                }
                var removeTip = function (ele) {
                    if ($(ele).next().hasClass('ABCD')) {
                        $(ele).next().remove();
                        $(ele).focus();
                    }

                }
                $(that).keyup(function (e) {
                    var $ele = $(that);
                    $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                    if (e.keyCode == 13) {
                        var _new = [],
                            c = 0,
                            v = $ele.val().toUpperCase();
                        v = v.replace(/JAN/gi, '01');
                        v = v.replace(/FEB/gi, '02');
                        v = v.replace(/MAR/gi, '03');
                        v = v.replace(/APR/gi, '04');
                        v = v.replace(/MAY/gi, '05');
                        v = v.replace(/JUN/gi, '06');
                        v = v.replace(/JUL/gi, '07');
                        v = v.replace(/AUG/gi, '08');
                        v = v.replace(/SEP/gi, '09');
                        v = v.replace(/OCT/gi, '10');
                        v = v.replace(/NOV/gi, '11');
                        v = v.replace(/DEC/gi, '12');
                        $ele.val(v);
                        var ds = $ele.val().trim().split(',');
                        $(ds).each(function (a, b) {
                            if (b.length == 8) {
                                b = b.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 6) {
                                b = b.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 2) {
                                var chuan = ds[ds.findIndex(item => item.length == 6 && ds.findIndex(x=>x == item) > a)];
                                var mmyy = chuan.substring(2, 6);
                                b = (b + mmyy).replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 10) {
                                if (!isValidDate(b)) {
                                    c = 1;
                                }
                                else _new.push(b);
                            }
                            else {
                                c = 1;
                            }
                        });
                        if (c == 0) {
                            $ele.val('');
                            $(_new).each(function () {
                                $ele.val($ele.val() + this + ',');
                            })
                            $ele.val($ele.val().substring(0, $ele.val().length - 1));
                            $(this).attr('data-contenttip', '');
                            $ele.css({ 'color': '' })
                            removeTip($ele);
                        } else {
                            $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                            $ele.css({ 'color': 'red' });
                            createTip($ele);

                        }
                    }
                })
                $(that).bind('blur', function () {
                    var $ele = $(that);
                    var _new = [],
                            c = 0,
                            v = $ele.val().toUpperCase();
                    v = v.replace(/JAN/gi, '01');
                    v = v.replace(/FEB/gi, '02');
                    v = v.replace(/MAR/gi, '03');
                    v = v.replace(/APR/gi, '04');
                    v = v.replace(/MAY/gi, '05');
                    v = v.replace(/JUN/gi, '06');
                    v = v.replace(/JUL/gi, '07');
                    v = v.replace(/AUG/gi, '08');
                    v = v.replace(/SEP/gi, '09');
                    v = v.replace(/OCT/gi, '10');
                    v = v.replace(/NOV/gi, '11');
                    v = v.replace(/DEC/gi, '12');
                    $ele.val(v);
                    var ds = $ele.val().trim().split(',');
                    $(ds).each(function (a, b) {
                        if (b.length == 8) {
                            b = b.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 6) {
                            b = b.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 2) {
                            var chuan = ds[ds.findIndex(item => item.length == 6 && ds.findIndex(x=>x == item) > a)];
                            var mmyy = chuan.substring(2, 6);
                            b = (b + mmyy).replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 10) {
                            if (!isValidDate(b)) {
                                c = 1;
                            }
                            else _new.push(b);
                        }
                        else {
                            c = 1;
                        }
                    });
                    if (c == 0) {
                        $ele.val('');
                        $(_new).each(function () {
                            $ele.val($ele.val() + this + ',');
                        })
                        $ele.val($ele.val().substring(0, $ele.val().length - 1));
                        $(this).attr('data-contenttip', '');
                        $ele.css({ 'color': '' })
                        removeTip($ele);
                    } else {
                        $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                        $ele.css({ 'color': 'red' });
                        //$ele.focus();
                    }
                })
            }
            $.fn.multiDate = function () {
                if ($('#checktipCssstyle')[0] == undefined)
                    $('body').append($cssStyle);
                $(this).attr('data-contenttip', 'format dd,dd....,ddmmyy');
                preIns(this);
            };
        }(jQuery));

    </script>
    <script>
        window.onkeydown = function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            var c = $('#tblSource tr').length;
            if (rowId == 0 && charCode == 46) {
                return;
            }

            // len 38 xuong 40
            if (charCode == 38 && isTarget) {
                try {
                    var e = $(document.activeElement);
                    $('#tblSource tbody tr').removeClass('select');
                    $(e).closest('tr').prev().addClass('select');
                    $($($(e).closest('tr').prev()).find('td')[$(e).closest('td').index()]).find('input')[0].focus();
                } catch (x) { }
            }
            if (charCode == 40 && isTarget) {
                try {
                    var e = $(document.activeElement);
                    $('#tblSource tbody tr').removeClass('select');
                    $(e).closest('tr').next().addClass('select');
                    $($($(e).closest('tr').next()).find('td')[$(e).closest('td').index()]).find('input')[0].focus();
                } catch (x) { }
            }
            if (charCode == 118) {

                _tr = "<tr id='_" + c + "' data-isInsert='true' onmouseover=\"rowId=$(this).prop('id');\" onmouseout='rowId=0;' style='background-color:darkkhaki;'>"


                    + "<td id='b_" + c + "'>" + (c - 1) + "</td>"
                    + "<td><input type=\"checkbox\" id='chk_" + c + "' /></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtOPER_ID" + c + "' value='' onfocusin='binAutocomplete(this,\"OPER\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtREGISTRATION" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtREALCRAFT" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtPERMCRAFT" + c + "' data-craftid='0' value='' onfocusin='binAutocomplete(this,\"CRAFT\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='8' class='sInput' style='background-color:darkkhaki;' id='txtFLIGHTNBR" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' style='background-color:darkkhaki;' id='txtPURPOSE" + c + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtPERMTYPE" + c + "'  value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' style='background-color:darkkhaki;' id='txtFROM_AIRP" + c + "' value='' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' style='background-color:darkkhaki;' id='txtTO_AIRP" + c + "' value='' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" data-CheckDate='true' class='sInput' style='background-color:darkkhaki;' id='txtFLIGHTDATE" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtATD" + c + "' value='' data-number='true'  onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtATA" + c + "' value='' data-number='true'  onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtVIA" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtFPLVIA" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtREMARK" + c + "'  value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtETD" + c + "' value='' data-number='true' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtETA" + c + "' value='' data-number='true'  onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' style='background-color:darkkhaki;' id='txtPERMNBR" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td style=\"white-space: nowrap;background-color:darkkhaki;\">"
                    + "<div class=\"action-buttons\">"
                    + "<a data-toggle=\"tooltip\" title=\"Delete\">"
                    + "<i id=\"btnDelete_" + c + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"var ax = confirm('Do you want delete?'); if(ax){$('#_" + c + "').remove();}\"></i></a>"
                    + "<a data-toggle=\"tooltip\" onclick=\"btnInsertList_Onclick();\" title=\"Insert\"><i class=\"glyphicon glyphicon glyphicon-plus\"></i></a>"
                    + "</div>"
                    + "</td>"
                    + "</tr>"


                if ($('#tblSource tbody tr').length == 0) {
                    $('#tblSource tbody').append(_tr);
                    $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                        $(b).mouseover(function () {
                            onmouseoverInput(b.id)
                        });
                        $(b).mouseout(function () {
                            onmouseoutInput(b.id)
                        });
                        $(b).ValidateTip();
                    })
                    $('#_' + c + ' input')[0].focus();
                    return;
                }
                if (rowId == 0) {
                    alert('Please select row!');
                    return;
                }
                var $trCurrent = $('#' + rowId),
                    $tr = $(_tr);
                $tr.removeClass('success')
                $tr.addClass('rowCreate');
                $tr.removeAttr('data-isupdate');
                $tr.prop('id', '_' + c);
                $tr.attr('onmouseover', 'rowId=$(this).prop(\'id\')');
                $('#' + rowId + ' input[type!=hidden][tabindex!=-1]').each(function (v, n) {
                    $($tr).find('[id^=' + $(n).prop('id').replace(/[0-9]\abc/gi, '') + ']').val($(n).val());
                });
                $.each($tr.find('input[data-oldValue]'), function () {
                    $(this).removeAttr('data-oldValue');
                });


                $tr.attr('data-isInsert', 'true');
                $($tr.find('td')[0]).text('');
                var $input1 = $($trCurrent).find('input[tabindex!=-1][type!=hidden]');
                var $input2 = $($tr).find('input[tabindex!=-1][type!=hidden]');
                for (var i = 0; i < $input2.length; i++) {

                    $($input2[i]).val($($input1[i]).val());
                    $($input2[i]).prop('checked', $($input1[i]).prop('checked'));
                    $($input2[i]).attr('data-craftid', $($input1[i]).attr('data-craftid'));

                }

                $trCurrent.after($tr);
                $($($tr).find('input[type!=hidden][tabindex!=-1]')).each(function () {
                    $(this).prop('id', $(this).prop('id').replace(/[0-9]\abc/gi, '') + $('#tblSource tr').length + 'abc');
                    $(this).removeAttr('onblur')
                })

                $($tr).find('input')[0].focus();
                $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                    $(b).mouseover(function () {
                        onmouseoverInput(b.id)
                    });
                    $(b).mouseout(function () {
                        onmouseoutInput(b.id)
                    });
                    $(b).ValidateTip();
                })
            }

            reloadAutoComplete();
        }

    </script>
    <script>
        
		$('#txtFromDate').val(dateFormat(new Date().setDate(new Date().getDate()), 'dd-mm-yyyy'));
        $('#txtFromDate').multiDate();
        $('#txtToDate').val(dateFormat(new Date().setDate(new Date().getDate()), 'dd-mm-yyyy'));
        $('#txtToDate').multiDate();	
		//LoadDataGrid();   		
    </script>
    <script>
        function sortOnclick(ele) {

            var s = parseInt($(ele).attr('data-sort'));
            s *= -1;
            $(ele).attr('data-sort', s);
            var span = s == '1' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#tblSource TR').eq(1).find('p').remove();
            $(ele).append(span);

            var n = $(ele).prevAll().length;
            sortTable(s, n);
        }
        function sortTable(f, n) {
            var rows = $('#tblSource tbody  tr').get();
            rows.sort(function (a, b) {
                var A = getVal(a);
                var B = getVal(b);
                if (A < B) {
                    return -1 * f;
                }
                if (A > B) {
                    return 1 * f;
                }
                return 0;
            });
            function getVal(elm) {
                var v = $(elm).children('td').eq(n).find('input').val().toUpperCase();
                if ($.isNumeric(v)) {
                    v = parseInt(v, 10);
                }
                return v;
            }

            $.each(rows, function (index, row) {
                $('#tblSource').children('tbody').append(row);
            });
        }
    </script>
    <script>
        function chkAll_Onchange() {
            $('#tblSource tbody td input[type="checkbox"]').prop('checked', $('#chkAll').prop('checked'));
        }
        function btnDeleteByChecked_Onclick() {
            $('#tblSource tbody td input[type="checkbox"]:checked').each(function (a, b) {
                var idDelete = $(b).prop('id').split('_')[1];
                var $request = $.ajax({
                    async: false,
                    method: "DELETE",
                    url: urlApi + "api/FinishedFlights/Delete/" + idDelete,
                }).always(function (data) {
                    if (data.Code != -1) {
                    }
                });
            })
            btnSearch_OnClick();
        }


    </script>
    <script type="text/javascript">
        function GetObjectSearchExport() {
            var _obj = {};
            if (isSearch) {
                $('#tblSource').attr('data-pageIndex', 1);
            }
            _obj['PAGESIZE'] = parseInt(100000);//$('#tblSource').attr('data-pageSize');
            _obj['PAGEINDEX'] = parseInt($('#tblSource').attr('data-pageIndex') - 1);
            _obj['FLIGHTDATE'] = "";
            _obj['PERMNBR'] = $('#txtPERMNBR').val();
            _obj['REGISTRATION'] = $('#txtREGISTRATION').val();
            _obj['FROM_AIRP'] = $('#txtFROM_AIRP').val();
            _obj['TO_AIRP'] = $('#txtTO_AIRP').val();
            _obj['PURPOSE'] = $('#txtPURPOSE').val();
            _obj['VALIDHOURS'] = '';
            _obj['OPER_ID'] = $('#txtOPER_ID').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['FLIGHT_TYPE'] = '';
            _obj['CRAFT_TYPE'] = $('#txtCRAFT_TYPE').attr('data-craftid');
            _obj['CRAFT_ID'] = $('#txtCRAFT_ID').attr('data-craftid');
            _obj['REAL_CRAFT_TYPE'] = $('#txtREALCRAFT').val();
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['VIA'] = $('#txtVIA').val();
            _obj['FPL_VIA'] = $('#txtFPLVIA').val();
            _obj['REMARK'] = $('#txtREMARK').val();
            _obj['ETA'] = $('#txtETA').val();
            _obj['ETD'] = $('#txtETD').val();
            _obj['ATA'] = $('#txtATA').val();
            _obj['ATD'] = $('#txtATD').val();
            _obj['StartDate'] = $('#txtFromDate').val();
            _obj['FinishDate'] = $('#txtToDate').val();
            _obj['KHUNGGIO1'] = $('#txtFromTime').val();
            _obj['KHUNGGIO2'] = $('#txtToTime').val();
            _obj['WHECONDITION'] = $('#txtFLIGHTDATE').val();
            _obj['CAT_HA'] = parseInt(ddlTime_ID.value);
            _obj['TypeOper'] = parseInt(ddlOPer_ID.value);
            _obj['TypeFlight'] = parseInt(ddlTypeFlight.value);
            _obj['FIR'] = ddlFr.value;
            _obj['SANBAYDI'] = $('#txtFromAir').val();
            _obj['SANBAYDEN'] = $('#txtFromAir').val();
            isSearch = false;
            return _obj;
        }
        function generate_excel(html) {

            //OK
            exportExel(html);

        }

        function exportExel(_html) {

            var dt = new Date();
            var day = dt.getDate();
            var month = dt.getMonth() + 1;
            var year = dt.getFullYear();
            var hour = dt.getHours();
            var mins = dt.getMinutes();
            var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;

            var textToSave = _html;
            var textToSaveAsBlob = new Blob([textToSave], { type: "text/plain" });
            var textToSaveAsURL = window.URL.createObjectURL(textToSaveAsBlob);
            var fileNameToSaveAs = 'exported_day_flight_' + postfix + '.xls';
            var downloadLink = document.createElement("a");
            downloadLink.download = fileNameToSaveAs;
            downloadLink.innerHTML = "Download File";
            downloadLink.href = textToSaveAsURL;
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();

        }
        function destroyClickedElement(event) {
            document.body.removeChild(event.target);
        }
        function RenderTableKhExport(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);
            //var stt = parseInt(((idx - 1) * pz) + 1);
			var stt = 1;
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
               kq += "<tr>"
                    + "<td style=\'text-align: left;\'>" + stt + "</td>"
					 + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                 
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CRAFT_NAME) + "</td>"                    
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + "'" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + "'" + returnEmpty(b.ETA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                   
                   
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"
                   
                   
                    + "</tr>";
                stt++;
            });
            return kq;
        }
        function LoadDataGrid_Export() {
            var _urlPath = "";
           
            _urlPath= "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetDayFlight2021"
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify({ P_DATE: $('#txtFromDate').val(), P_EDATE: $('#txtToDate').val(), P_FROM_AIRP: $('#txtFromAir').val(),P_TO_AIRP: $('#txtToAir').val() ,P_OPER_ID: $('#txtOPER_ID').val() }),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove(); 
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
                if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }


                var strAppend = RenderTableKhExport(data);
                 var strHtml = "<table id='tblSource' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        + "<th>NO</th>"
						+ "<th>PERMNBR</th>" 
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "<th>VIA</th>" 
						
                        + "<th>REMARK</th>"         
                        + "</tr>"
                        + "</thead>"
                        + "<tbody>"
						+ strAppend
                        + "</tbody>"
                        + "</table>";
                generate_excel(strHtml);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }





    </script>


</asp:Content>
