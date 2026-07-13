<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="Schedule_Flight.aspx.cs" Inherits="prjApplication.ScheduleFlights.Schedule_Flight" %>


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
        #abcxyz {
            display: flex;
            align-items: flex-end;
            flex-wrap: wrap;
            gap: 10px;
            margin-bottom: 14px;
            padding: 14px 16px !important;
            border: 1px solid #c8ddeb;
            border-radius: 12px;
            background: #f7fbfe;
            box-shadow: 0 7px 20px rgba(24, 78, 117, .10);
            text-align: left !important;
        }

        #abcxyz .schedule-filter-field { display: flex; flex-direction: column; gap: 5px; min-width: 0; }
        #abcxyz .schedule-filter-label { color: #315a77; font-size: 11px; font-weight: 700; }
        #abcxyz .schedule-date-field { flex: 0 1 190px; }
        #abcxyz .schedule-page-size { flex: 0 0 90px; }
        #abcxyz .schedule-date-field input[type="date"] { width: 100%; }
        #abcxyz .schedule-date-value { display: none !important; }
        #abcxyz input[type="date"], #abcxyz select {
            height: 36px;
            padding: 6px 9px;
            border: 1px solid #8eb9d6;
            border-radius: 7px;
            background: #fff;
            color: #173b59;
        }
        #abcxyz .btn { height: 36px; border-radius: 7px; font-weight: 600; }
        #abcxyz .schedule-actions { display: flex; flex-wrap: wrap; gap: 8px; margin-left: auto; }
        #tblSource {
            width: 100% !important;
            max-width: 100%;
            table-layout: fixed;
            border-collapse: collapse !important;
            border-spacing: 0 !important;
            font-size: 12px !important;
        }
        #tblSource th, #tblSource td {
            padding: 0 !important;
            border: 1px solid #c4d5e1 !important;
            border-radius: 0 !important;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        #tblSource > thead {
            position: sticky !important;
            top: 0;
            z-index: 40;
            background: #fff;
            box-shadow: 0 2px 0 rgba(31, 105, 154, .18);
        }
        #tblSource > thead > tr:first-child > th {
            position: static !important;
            height: 38px;
            background: #f7fbff !important;
            background-clip: padding-box !important;
            border-radius: 0 !important;
        }
        #tblSource > thead > tr:nth-child(2) > th {
            position: static !important;
            height: 35px;
            background: #337ab7 !important;
            background-clip: padding-box !important;
            color: #fff !important;
            border-radius: 0 !important;
        }
        #tblSource input[type="text"] {
            display: block;
            width: 100% !important;
            height: 34px;
            margin: 0 !important;
            padding: 4px 3px;
            border: 0 !important;
            border-radius: 0 !important;
            box-shadow: none !important;
            font-size: 12px !important;
        }
        #tblSource tbody tr.select,
        #tblSource tbody tr.select > td { background-color: #cfe9d6 !important; border-color: #75b488 !important; }
        #tblSource tbody tr.select input[type="text"] { background-color: #cfe9d6 !important; border-radius: 0 !important; }
        @media (max-width: 767px) {
            #abcxyz .schedule-actions { width: 100%; margin-left: 0; }
            #abcxyz .schedule-actions .btn { flex: 1 1 140px; }
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

        #tblSource tbody tr.selectdel input {
            background-color: red;
        }

        #tblSource tbody tr.selectdel {
            background-color: red;
        }
    </style>
    <div id="abcxyz" class="well well-sm">
        <div class="schedule-filter-field schedule-date-field">
            <label class="schedule-filter-label" for="txtFromDatePicker">NGÀY BAY</label>
            <input id="txtFromDatePicker" type="date" aria-label="Ngày bay" />
            <input id="txtFromDate" data-minlenght="1" data-control="checkAccess" type="text" class="schedule-date-value" />
        </div>
        <div class="schedule-filter-field schedule-page-size">
            <label class="schedule-filter-label" for="ddlPageSize">SỐ DÒNG</label>
            <select id="ddlPageSize" onchange="ddlPageSize_OnChange()">
                <option value="100" selected="selected">100</option>
                <option value="500">500</option>
                <option value="1000">1000</option>
                <option value="2000">2000</option>
                <option value="4000">4000</option>
                <option value="6000">6000</option>
                <option value="8000">8000</option>
            </select>
        </div>
        <div class="schedule-actions">
        <button type="button" class="btn btn-sm btn-primary btn-bold" id="btnAccess" onclick="btnExport_OnClick()">Export DailyFlight</button>
        <button type="button" id="btnSearch" class="btn btn-sm btn-primary" onclick="btnSearch_OnClick()">Search</button>
        <button type="button" id="btnDeleteByChecked" class="btn btn-sm btn-primary" onclick="btnDeleteByChecked_Onclick()">Delete</button>
        <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" onclick="btnClearSearch_OnClick()">Clear search</button>
        </div>
        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <div id="pageging"></div>
    <table id="tblSource" class="table table-bordered">

        <caption>
        </caption>

        <thead>
            <tr style="background-color: unset">
                <th></th>
                <th></th>
                <th>
                    <input id="txtPERMNBR" class="wid_160px" type="text" /></th>
                <th>
                    <input id="txtFLIGHTNBR" class="wid_75px" type="text" /></th>
                <th>
                    <input id="txtREGISTRATION" class="wid_70px" type="text" /></th>
                <th>
                    <input id="txtFROM_AIRP" data-autocomplete="AERO" class="wid_60px" type="text" />
                </th>
                <th>
                    <input id="txtTO_AIRP" data-autocomplete="AERO" class="wid_60px" type="text" />
                </th>
                <th>
                    <input id="txtETD" class="wid_50px" type="text" /></th>
                <th>
                    <input id="txtETA" class="wid_50px" type="text" /></th>
                <th>
                    <input id="txtDATE_OLD" class="wid_85px" type="text" /></th>
                <th>
                    <input id="txtFLIGHTDATE" class="wid_85px" type="text" /></th>
                <th style="display: none">
                    <input id="txtATD" class="wid_50px" type="text" /></th>
                <th style="display: none">
                    <input id="txtATA" class="wid_50px" type="text" /></th>
                <th>
                    <input id="txtPERMTYPE" data-autocomplete="PERMTYPE" class="wid_100" type="text" />
                </th>
                <th>
                    <input id="txtFLIGHT_TYPE" data-autocomplete="FLIGHTTYPE" class="wid_100" type="text" />
                </th>
                <th>
                    <input id="txtOPER_ID" data-autocomplete="OPER" class="wid_50px" type="text" />
                </th>

                <th>
                    <input id="txtPURPOSE" data-autocomplete="PURPOSE" class="wid_50px" type="text" />
                </th>
                <th>
                    <input id="txtVALIDHOURS" data-number="true" class="wid_100" type="text" /></th>

                <th>
                    <input id="txtVIA" class="wid_120px" type="text" /></th>
                <th>
                    <input id="txtREMARK" class="wid_120px" type="text" /></th>
                <th></th>
                <th></th>
            </tr>
            <tr style="color: white">
                <th>No</th>
                <th>
                    <input type="checkbox" id="chkAll" onchange="chkAll_Onchange()" /></th>
                <th data-sort="1" onclick="sortOnclick(this);">Number</th>
                <th data-sort="1" onclick="sortOnclick(this);">Flight Nbr</th>
                <th data-sort="1" onclick="sortOnclick(this);">Regis</th>
                <th data-sort="1" onclick="sortOnclick(this);">From</th>
                <th data-sort="1" onclick="sortOnclick(this);">To</th>
                <th data-sort="1" onclick="sortOnclick(this);">Etd</th>
                <th data-sort="1" onclick="sortOnclick(this);">Eta</th>
                <th data-sort="1" onclick="sortOnclick(this);">Date old</th>
                <th data-sort="1" onclick="sortOnclick(this);">Flight date</th>
                <th data-sort="1" onclick="sortOnclick(this);">Perm type</th>
                <th data-sort="1" onclick="sortOnclick(this);">Flight type</th>
                <th data-sort="1" onclick="sortOnclick(this);">Oper</th>


                <th data-sort="1" onclick="sortOnclick(this);">Pur</th>
                <th data-sort="1" onclick="sortOnclick(this);">Valid hour</th>

                <th data-sort="1" onclick="sortOnclick(this);">Via</th>
                <th data-sort="1" onclick="sortOnclick(this);">Remark</th>
                <th>


                </th>
                 <th>


                </th>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>

    <script>
        var isTarget = true;
        var qEdit = '<%= _Role.R_Edit %>';
        var qDel = '<%= _Role.R_Del %>';
        var isSearch = false;
        var rowId = 0;
        var pageSize = parseInt($('#ddlPageSize').val(), 10) || 100;
        $('#tblSource').paging({ pageSize: pageSize });

        function ddlPageSize_OnChange() {
            pageSize = parseInt($('#ddlPageSize').val(), 10) || 100;
            $('#tblSource').attr('data-pageSize', pageSize);
            $('#tblSource').attr('data-pageIndex', 1);
            isSearch = true;
            LoadDataGrid();
        }
        function Render2Table(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt($('#tblSource').attr('data-pagesize'));
            var khb_delete = $('#chkKhbDelete').prop('checked');
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {

                kq += "<tr id='" + b.ID + "' data-isUpdate='false' onmouseover='rowId=" + b.ID + ";' onmouseout='rowId=0;'>"
                + "<td style=\"width:30px\" id='b_" + b.RNUM + "'><input style='width: 30px!important; padding: 0px!important;' type='text' id='txtStt" + stt + "' value='" + stt + "'/></td>"
                + "<td><input type=\"checkbox\" id='chk_" + b.ID + "' /></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPERMNBR" + a + "' data-oldValue='" + b.PERMNBR + "' value='" + b.PERMNBR + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='8' class='sInput' id='txtFLIGHTNBR" + a + "' data-oldValue='" + returnEmpty(b.FLIGHTNBR) + "' value='" + returnEmpty(b.FLIGHTNBR) + "' onblur='checkIsUpdate(this)' /></td>"
                + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREGISTRATION" + a + "' data-oldValue='" + returnEmpty(b.REGISTRATION) + "' value='" + returnEmpty(b.REGISTRATION) + "' onblur='checkIsUpdate(this)' /></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP" + a + "' data-oldValue='" + b.FROM_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.FROM_AIRP + "' onblur='checkIsUpdate(this)' /></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP" + a + "' data-oldValue='" + b.TO_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.TO_AIRP + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' id='txtETD" + a + "' data-oldValue='" + returnEmpty(b.ETD) + "' data-number='true' value='" + returnEmpty(b.ETD) + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA" + a + "' data-oldValue='" + returnEmpty(b.ETA) + "' data-number='true' value='" + returnEmpty(b.ETA) + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" class='sInput' id='txtDATE_OLD" + a + "' data-oldValue='" + new Date(b.DATE_OLD).format('dd-mm-yyyy') + "' value='" + new Date(b.DATE_OLD).format('dd-mm-yyyy') + "' onblur='checkIsUpdate(this)' /></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" class='sInput' id='txtFLIGHTDATE" + a + "' data-oldValue='" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "' value='" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "' onblur='checkIsUpdate(this)' /></td>"
                + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtPERMTYPE" + a + "' onfocusin='binAutocomplete(this,\"PERMTYPE\")' data-oldValue='" + returnEmpty(b.PERMTYPE) + "' value='" + returnEmpty(b.PERMTYPE) + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtFLIGHT_TYPE" + a + "' data-oldValue='" + returnEmpty(b.FLIGHT_TYPE) + "' onfocusin='binAutocomplete(this,\"FLIGHTTYPE\")' value='" + returnEmpty(b.FLIGHT_TYPE) + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtOPER_ID" + a + "' data-oldValue='" + returnEmpty(b.OPER_ID) + "' value='" + returnEmpty(b.OPER_ID) + "' onfocusin='binAutocomplete(this,\"OPER\")' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPURPOSE" + a + "' data-oldValue='" + b.PURPOSE + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' value='" + b.PURPOSE + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtVALIDHOURS" + a + "' data-oldValue='" + b.VALIDHOURS + "' value='" + b.VALIDHOURS + "' data-number=\"true\" maxlength=\"2\" onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA" + a + "' data-oldValue='" + returnEmpty(b.VIA).replace(/\n/gi, '') + "' value='" + returnEmpty(b.VIA) + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "' data-oldValue='" + returnEmpty(b.REMARK) + "' value='" + returnEmpty(b.REMARK) + "' onblur='checkIsUpdate(this)'/></td>"
                + "<td style=\"white-space: nowrap;\">"
                            + "<div class=\"action-buttons\">"
                            + "<i id=\"btnDeleteRemark" + b.ID + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"btnDeleteOnclick(" + b.ID + ");\"></i>"
                            + "</div></td>"
                + "<td style=\"white-space: nowrap;\">"
                            + "<div class=\"action-buttons\">"
                            + "<a><i id=\"btnCancelRemark" + b.ID + "\" class=\"ace-icon fa fa-check bigger-130\" onclick=\"btnCancelOnclick(" + b.ID + ");\"></i>" + b.ISRENDER + "</a>"
                            + "</div></td>"
                + "</tr>"

                stt++;
            });
            return kq;
        }


        function returnEmpty(val) {
            return val == null ? "" : val;
        }
        function LoadDataGrid() {   
			console.log(GetObjectSearch());
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ScheduleFlights/GetBySearchAll",
                //url: urlApi + "api/ScheduleFlights/GetBySearchAll_New",
                //url: urlApi + "api/ApiExtension/ExcuteTable?packageName=SCHEDULEDAYFLIGHTS_2020_PKG&storeName=getBySearch_New",
                
				data: GetObjectSearch(),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loaddingData');
                    reloadCheckValid();
                    $('#tblSource').paging({
                        onClickButton: 'LoadDataGrid',
                        pageSize: pageSize,
                    });
                    $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                        if ($(b).prop('id').indexOf('txtPERMDATE') == -1 && $(b).prop('id').indexOf('txtFLIGHTDATE') == -1)
                            $(b).ValidateTip();
                        else $(b).multiDate();
                    });
                    $("#tblSource td").click(function () {
                        $('#tblSource tbody tr').removeClass('select');
                        $('#tblSource tbody tr').eq(parseInt($(this).parent().index())).addClass('select');
                    });
                },
            }).always(function (data) {
                if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
                $('#tblSource tbody tr').remove();
                $('#tblSource').attr('data-total', data.ListValue[0]['TOTALRECORDS']);
                document.getElementById("pageging").innerHTML = "Tổng số :" + data.ListValue[0]['TOTALRECORDS'] + "";
                var strAppend = Render2Table(data);
                $('#tblSource tbody').append(strAppend);
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
            _obj['PAGESIZE'] = $('#tblSource').attr('data-pageSize');
            _obj['PAGEINDEX'] = parseInt($('#tblSource').attr('data-pageIndex'));
            _obj['FLIGHTDATE'] = $('#txtFLIGHTDATE').val() == '' ? new Date().format('yyyy-mm-dd') : $('#txtFLIGHTDATE').val().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            //_obj['FLIGHTDATE'] = $('#txtFLIGHTDATE').val().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            _obj['DATE_OLD'] = $('#txtDATE_OLD').val().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['REGISTRATION'] = $('#txtREGISTRATION').val();
            _obj['FROM_AIRP'] = $('#txtFROM_AIRP').val();
            _obj['TO_AIRP'] = $('#txtTO_AIRP').val();
            _obj['PURPOSE'] = $('#txtPURPOSE').val();
            _obj['VALIDHOURS'] = $('#txtVALIDHOURS').val();
            _obj['OPER_ID'] = $('#txtOPER_ID').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['FLIGHT_TYPE'] = $('#txtFLIGHT_TYPE').val();                      
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['VIA'] = $('#txtVIA').val();
            _obj['REMARK'] = $('#txtREMARK').val();
            _obj['ETA'] = $('#txtETA').val();
            _obj['ETD'] = $('#txtETD').val();
            _obj['ATA'] = $('#txtATA').val();
            _obj['ATD'] = $('#txtATD').val();            
            _obj['PERMNBR'] = $('#txtPERMNBR').val();
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

	function btnCancelOnclick(id) {
		var result = confirm("Do you want update status?");
	  if (result) {
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=SCHEDULEDAYFLIGHTS_2020_PKG&storeName=oUpdateStatus",
                    data: JSON.stringify({ P_ID: id }),
                }).always(function (data) {
                    if (data.Code != -1) {
                       
                    alert('update sussess!');
                    } else alert('update error!');
                });
            }
	}

        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result) {
                var $request = $.ajax({
                    async: false,
                    method: "DELETE",
                    url: urlApi + "api/ScheduleFlights/Delete/" + id,
                }).always(function (data) {
                    if (data.Code != -1) {
                        $('#' + id).remove();
                        alert('Delete sussess!');
                    } else alert('Delete error!');
                });
            }
        }
        function btnSearch_OnClick() {
            isSearch = true;
            LoadDataGrid();
        }
        function btnClearSearch_OnClick() {
            $('#txtFLIGHTNBR').val('');
            $('#txtREGISTRATION').val('');
            $('#txtFROM_AIRP').val('');
            $('#txtTO_AIRP').val('');
            $('#txtETD').val('');
            $('#txtETA').val('');
            $('#txtDATE_OLD').val('');
            $('#txtFLIGHTDATE').val('');
            $('#txtATD').val('');
            $('#txtATA').val('');
            $('#txtPERMTYPE').val('');
            $('#txtFLIGHT_TYPE').val('');
            $('#txtOPER_ID').val('');            
            $('#txtCRAFT_TYPE').val('');
            $('#txtPURPOSE').val('');
            $('#txtVALIDHOURS').val('');
            $('#txtPERMNBR').val('');
            $('#txtVIA').val('');
            $('#txtREMARK').val('');
            $('#txtVV').val('');
            $('#txtZZ').val('');
            btnSearch_OnClick();
        }
        function btnExport_OnClick() {
            //console.log(JSON.stringify({ P_DATEFLIGHT: new Date($('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd'), P_USER: '<%= _user.UserName%>' }));
              
            var c = checkValidCustomMinlenght('checkAccess');
            if (!c) return;
            var cf = confirm('Do you want export flights date: ' + $('#txtFromDate').val() + '?');
            if (cf) {
                var $request = $.ajax({
                    async: true,
                    method: "PUT",
                    url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=SCHEDULEDAYFLIGHTS_2020_PKG&storeName=RenderKeHoachBayNgay",

                    data: JSON.stringify({ P_DATEFLIGHT: new Date($('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd'), P_USER: '<%= _user.UserName%>' }),
                    complete: function () {
                        unLoadingData('loadingAccess');
                    },
                    beforeSend: function () {
                        preloadImgAfterButton('btnAccess', 'loadingAccess');
                    }
                }).always(function (data) {
                    alert(data.Value == -1 ? 'Error!' : 'Sussess!');
                });
            }
        }


        function Load_Data_Search() {
            LoadDataGrid();
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

                    onmouseoverInput($($($(e).closest('tr').prev()).find('td')[$(e).closest('td').index()]).find('input')[0].id);

                    $($($(e).closest('tr').prev()).find('td')[$(e).closest('td').index()]).find('input')[0].focus();
                } catch (x) { }
            }
            if (charCode == 40 && isTarget) {
                try {
                    var e = $(document.activeElement);
                    $('#tblSource tbody tr').removeClass('select');
                    $(e).closest('tr').next().addClass('select');

                    onmouseoverInput($($($(e).closest('tr').next()).find('td')[$(e).closest('td').index()]).find('input')[0].id);

                    $($($(e).closest('tr').next()).find('td')[$(e).closest('td').index()]).find('input')[0].focus();
                } catch (x) { }
            }
          
            if (charCode == 119) {

                if (inputId == '') {
                    alert('Please select!');
                    return;
                }
                var $ele = $('#' + inputId),
                    $rowIndex = $ele.closest('tr').index(),
                    $columIndex = $ele.closest('td').index(),
                    $type = $ele.prop('type');
                switch ($type) {
                    case 'text':
                        var x = $('#tblSource tr').eq($rowIndex + 1).find('td').eq($columIndex).find('input').val();
                        if (x == undefined)
                            x = $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input').val();
                        if (x == '' || x == undefined) return;
                        $ele.val(x);
                        $ele.focus();
                        break;
                    case 'checkbox':
                        $('#tblSource tr').eq($rowIndex).find('td').eq($columIndex).find('input')[0].prop('checked', $ele.prop('checked'));
                        break;
                }

            }

        }
        function onmouseoverInput(id) {
            inputId = id;
            //alert(inputId)
        }
        function onmouseoutInput(id) {
            inputId = '';
        }




    </script>
    <script>
        $('#txtFromDate').val(dateFormat(new Date().setDate(new Date().getDate() + 1), 'dd-mm-yyyy'));
        $('#txtFromDate').multiDate();        
        (function () {
            var value = $('#txtFromDate').val().split('-');
            if (value.length === 3) $('#txtFromDatePicker').val(value[2] + '-' + value[1] + '-' + value[0]);
            $('#txtFromDatePicker').on('change', function () {
                var parts = (this.value || '').split('-');
                $('#txtFromDate').val(parts.length === 3 ? parts[2] + '-' + parts[1] + '-' + parts[0] : '');
            });
        }());
        $('#txtDATE_OLD').multiDate();
        $('#txtFLIGHTDATE').val(dateFormat(new Date().setDate(new Date().getDate() + 1), 'dd-mm-yyyy'));
        $('#txtFLIGHTDATE').multiDate();
        LoadDataGrid();
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
                //var v = $(elm).children('td').eq(n).text().toUpperCase();
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
            var cf = confirm('Do you want delete ?');
            if (cf) {
                $('#tblSource tbody td input[type="checkbox"]:checked').each(function (a, b) {
                    var idDelete = $(b).prop('id').split('_')[1];
                    var $request = $.ajax({
                        async: false,
                        method: "DELETE",
                        url: urlApi + "api/ScheduleFlights/Delete/" + idDelete,
                    }).always(function (data) {
                        if (data.Code != -1) {
                        }
                    });
                })
            }

            btnSearch_OnClick();
        }

    </script>
    <script>

        function btnInsert() {
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
                _obj['VALIDHOURS'] = $($(b).find('[id^="txtVALIDHOURS"]')[0]).val();
                _obj['FLIGHT_TYPE'] = $($(b).find('[id^="txtFLIGHT_TYPE"]')[0]).val();
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
                _obj['STATUS'] = '0';
                _obj['ISACCESS'] = 1;
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/DayFlights/InsertManual",
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            alert('Insert sussess: ' + c + '/' + $lis.length);
            btnSearch_OnClick();
        }
    </script>
    <script type="text/javascript">

        function generate_excel(html) {
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
            var fileNameToSaveAs = 'exported_khb_' + postfix + '.xls';
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
            var pz = 6000;
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"
                    + "<td>" + stt + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CRAFT_NAME) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + "'" + returnEmpty(b.ETDEXP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + "'" + returnEmpty(b.ETAEXP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"
                    + "</tr>";

                stt++;
            });
            return kq;
        }
        function GetObjectSearchExport() {
            var _obj = {};
            if (isSearch) {
                $('#tblSource').attr('data-pageIndex', 1);
            }
            _obj['PAGESIZE'] = 6000;
            _obj['PAGEINDEX'] = parseInt($('#tblSource').attr('data-pageIndex'));
            _obj['FLIGHTDATE'] = $('#txtFLIGHTDATE').val() == '' ? new Date().format('yyyy-mm-dd') : $('#txtFLIGHTDATE').val().replace(/\//gi, '-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['REGISTRATION'] = $('#txtREGISTRATION').val();
            _obj['FROM_AIRP'] = $('#txtFROM_AIRP').val();
            _obj['TO_AIRP'] = $('#txtTO_AIRP').val();
            _obj['PURPOSE'] = $('#txtPURPOSE').val();
            _obj['VALIDHOURS'] = $('#txtVALIDHOURS').val();
            _obj['OPER_ID'] = $('#txtOPER_ID').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['FLIGHT_TYPE'] = $('#txtFLIGHT_TYPE').val();
            _obj['CRAFT_TYPE'] = $('#txtCRAFT_TYPE').attr('data-craftid');           
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['VIA'] = $('#txtVIA').val();
            _obj['REMARK'] = $('#txtREMARK').val();
            _obj['ETA'] = $('#txtETA').val();
            _obj['ETD'] = $('#txtETD').val();
            _obj['ATA'] = $('#txtATA').val();
            _obj['ATD'] = $('#txtATD').val();
            _obj['STARTDATE'] = '01/04/2019';
            _obj['FINISHDATE'] = '10/04/2019';
            _obj['ISACCESS'] = 2;
            isSearch = false;
            return _obj;
        }
       
    </script>

</asp:Content>
