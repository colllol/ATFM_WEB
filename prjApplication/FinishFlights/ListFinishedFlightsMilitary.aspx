<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListFinishedFlightsMilitary.aspx.cs" Inherits="prjApplication.FinishFlights.ListFinishedFlightsMilitary" %>

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
            background-color: rgb(65, 142, 214);
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
	.sInputCompare {
            border-width: 0px !important;

            width: 100%;    
		background-color: red!important;
    		color: #fff!important;
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

        .military-date-filter {
            display: inline-flex;
            align-items: center;
            flex: 0 0 auto;
            gap: 4px;
            margin: 0;
            vertical-align: middle;
        }

        .military-date-filter > label {
            margin: 0;
            color: #315a77;
            font-size: 12px;
            font-weight: 700;
        }

        .military-date-picker {
            width: 130px !important;
            height: 36px;
            padding: 6px 9px;
            border: 1px solid #8eb9d6 !important;
            border-radius: 7px;
            background: #fff;
            color: #173b59;
            font-size: 13px;
            outline: 0;
            transition: border-color .18s ease, box-shadow .18s ease;
        }

        .military-date-picker:focus {
            border-color: #2388c6 !important;
            box-shadow: 0 0 0 3px rgba(35, 136, 198, .14);
        }

        .military-accepted-filter {
            flex: 0 0 135px;
            width: 135px;
            height: 34px;
            margin: 0;
            border: 1px solid #8eb9d6;
            border-radius: 6px;
            background: #fff;
            color: #173b59;
        }

        .military-date-value { display: none !important; }

        #abcxyz {
            display: flex;
            align-items: center;
            flex-wrap: wrap;
            gap: 5px;
            text-align: left !important;
            white-space: nowrap;
        }

        #abcxyz > input,
        #abcxyz > select,
        #abcxyz > button,
        #abcxyz > .btn,
        #abcxyz > label {
            flex: 0 0 auto;
            margin: 0;
        }

        #abcxyz > input.wid_50px {
            width: 46px !important;
        }

        #abcxyz > button,
        #abcxyz > .btn {
            width: auto !important;
            min-width: 88px;
            padding-left: 10px;
            padding-right: 10px;
        }

        #abcxyz > .military-toolbar-actions {
            display: flex;
            flex: 0 0 100%;
            align-items: center;
            gap: 5px;
            padding-top: 5px;
        }

        #abcxyz > .military-toolbar-actions > button {
            flex: 0 0 auto;
            width: auto !important;
            min-width: 88px;
            margin: 0;
            padding-left: 10px;
            padding-right: 10px;
        }

        #abcxyz > .radio-inline {
            padding-left: 5px;
        }

        #abcxyz #totalsfinished {
            white-space: nowrap;
        }

        #abcxyz #ddlPageSize {
            width: 58px !important;
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
        <span class="military-date-filter">
            <label for="txtFromDatePicker">FROM</label>
            <input id="txtFromDatePicker" type="date" class="military-date-picker" aria-label="Ngày bắt đầu" />
            <input id="txtFromDate" data-minlenght="1" data-control="checkAccess" type="text" class="military-date-value" />
        </span>
        <input id="txtFromTime" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select time" class="wid_50px" maxlength='4' data-number='true' value="0000" />
        <span class="military-date-filter">
            <label for="txtToDatePicker">TO</label>
            <input id="txtToDatePicker" type="date" class="military-date-picker" aria-label="Ngày kết thúc" />
            <input id="txtToDate" data-minlenght="1" data-control="checkAccess" type="text" class="military-date-value" />
        </span>

        <input id="txtToTime" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select time" class="wid_50px" maxlength='4' data-number='true' value="2359" />

        <b>HOUR :</b>
        <select id="ddlTime" class="disabled" style="width: 70px;" onchange="ddlTime_Change();">
            <option value="0">-All-</option>
            <option value="1">ETD</option>
            <option value="2">ETA</option>
            <option value="3">ATD</option>
            <option value="4">ATA</option>
        </select>

        <select id="ddlAcceptedStatus" class="military-accepted-filter" aria-label="Trạng thái Accepted"
            onchange="acceptedStatus_OnChange();">
            <option value="0">CHƯA ACCEPTED</option>
            <option value="1">ĐÃ ACCEPTED</option>
            <option value="-1">TẤT CẢ</option>
        </select>

        <select id="ddlPageSize" class="disabled" style="width: 65px;">
            <option value="100">100</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
            <option value="2000">2000</option>
            <option value="4000">4000</option>
            <option value="6000">6000</option>
            <option value="8000">8000</option>
        </select>

        <div class="military-toolbar-actions">
            <button type="button" id="btnSearch" class="btn btn-sm btn-primary" style="width: 100px" onclick="btnSearch_OnClick()">
                Search</button>
            <button type="button" id="btnAccepted" class="btn btn-sm btn-success" style="width: 100px"
                onclick="btnAccepted_OnClick()">
                Accepted</button>
            <button type="button" id="btnExportQsMessage" class="btn btn-sm btn-primary" disabled="disabled"
                onclick="btnExportQsMessage_OnClick()">
                Export Message</button>
            <button type="button" id="btnNews" class="btn btn-sm btn-primary" style="width: 100px" onclick="AddNews()">
                Add New</button>
            <button type="button" id="btnUpdateAll" class="btn btn-sm btn-primary" style="width: 100px" onclick="btnInsertList_Onclick()">
                Update All</button>
        </div>

        <label class="radio-inline" style="display: none;">
            <span id="totalsfinished">Tổng số : <b>0</b></span></label>
        <!--

        <b>OPER :</b>
        <select id="ddlSelect" class="disabled" style="width: 90px;">
            <option value="0">-All-</option>
            <option value="1">QN</option>
            <option value="2">QT</option>
        </select>

        <br />
        <br />

        <b>FLIGHT :</b>
        <select id="ddlType" class="disabled" style="width: 90px;">
            <option value="0">-All-</option>
            <option value="1">QN</option>
            <option value="2">QT</option>
        </select>
        <b>FIR :</b>
        <select id="ddlFir" class="disabled" style="width: 65px;">
            <option value="0">-ALL-</option>
            <option value="1">HN</option>
            <option value="2">HCM</option>
        </select>
        <b>AIR PORT :</b>
        <input id="txtFromAir" data-control="checkAccess" type="text"
            placeholder="AIR PORT" class="wid_100px" />
        
        <b>P_SIZE :</b>-->
        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <div id="abcxyzd" class="well well-sm" style="text-align: left; display: none">
        <button type="button" id="btnUpdateList" class="btn btn-sm btn-primary" style="width: 90px" onclick="btnUpdateList_Onclick()">
            Update</button>
        <button type="button" id="btnDeleteByChecked" class="btn btn-sm btn-primary" style="width: 90px" onclick="btnDeleteByChecked_Onclick()">
            Delete</button>
        <!--<button type="button" id="btnMove55" class="btn btn-sm btn-primary" style="width: 90px" onclick="myFunction()">
            Sort</button>-->
        <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" style="width: 90px" onclick="btnClearValue_OnClick()">
            Clear Search</button>

        <button type="button" id="btnExport" class="btn btn-sm btn-primary" style="width: 135px" onclick="LoadDataGrid_Export()">
            Export Excel</button>

        <button type="button" id="btnExport80" class="btn btn-sm btn-primary" style="width: 135px" onclick="LoadDataGrid_ExportCancel()">
            Export Excel Cancel</button>
        <button type="button" id="btnExport801" class="btn btn-sm btn-primary" style="width: 135px" onclick="ExportBravo()">
            Export Bravo</button>
    </div>

    <label class="radio-inline" style="display: none;">
        <input id="chkKhb" checked onchange="chkKhb_CheckedChanged();" type="radio" style="display: none;" name="optradio" />HOÀN THÀNH</label>
    <!--<label class="radio-inline">
                <input id="chkQndi" onchange="chkQndi_CheckedChanged();" type="radio" name="optradio" />QUỐC NỘI ĐI</label>
             <label class="radio-inline">
                <input id="chkQtve" onchange="chkQtve_CheckedChanged();" type="radio" name="optradio" />QUỐC TẾ VỀ</label>
             <label class="radio-inline">
                <input id="chkChot" onchange="chkChot_CheckedChanged();" type="radio" name="optradio" />CHỐT SỐ LIỆU</label>-->
    <label class="radio-inline" style="display: none;">
        <input id="chkKhbDelete" onchange="chkKhbDelete_CheckedChanged();" type="radio" name="optradio" />CANCEL
    </label>

    <div id="exportid" runat="server">

        <table id="tblSource" class="table table-bordered">
            <caption>
            </caption>
            <thead>
                <tr style="background-color: unset" isinsertflight="true" id="n0">

                    <th>
                        <div class="action-buttons">
                        </div>
                    </th>
                    <th></th>
                    <th>
                        <input id="txtOper" class="wid_75px" type="text" /></th>
                    <th>
                        <input id="txtFLIGHTDATE" class="wid_85px" type="text" /></th>
                    <th>
                        <input id="txtCALLSIGN" class="wid_75px" type="text" /></th>
                    <th>
                        <input id="txtRegis" class="wid_75px" type="text" /></th>
                    <th>
                        <input id="txtRCraf" class="wid_75px" type="text" /></th>
                    <th>
                        <input id="txtFCraf" class="wid_75px" type="text" /></th>
                    <th>
                        <input id="txtPurpose" class="wid_75px" type="text" /></th>
                    <th>
                        <input id="txtP_TYPE" class="wid_70px" type="text" /></th>
                    <th>
                        <input id="txtFROM_AIRP" class="wid_60px" type="text" />
                    </th>
                    <th>
                        <input id="txtTO_AIRP" class="wid_60px" type="text" />
                    </th>
                    <th>
                        <input id="txtATD" class="wid_60px" type="text" /></th>
                    <th>
                        <input id="txtATA" class="wid_60px" type="text" /></th>
                    <th>
                        <input id="txtVia" class="wid_60px" type="text" />
                    </th>
                    <th>
                        <input id="txtFPLVia" class="wid_60px" type="text" />
                    </th>
                    <th>
                        <input id="txtREMARK" class="wid_80px" type="text" /></th>
                    <th>
                        <input id="txtETD" class="wid_60px" type="text" /></th>
                    <th>
                        <input id="txtETA" class="wid_60px" type="text" /></th>
                    <th></th>
                    <th>
                        <div class="action-buttons">
                        </div>
                    </th>
                </tr>

                <tr style="color: white">

                    <th>No</th>

                    <th>
                        <input type="checkbox" id="chkAll" onchange="chkAll_Onchange()" /></th>
                    <th data-sort="1" onclick="sortOnclick(this);">OPER</th>
                    <th data-sort="1" onclick="sortOnclick(this);">FLIGHTDATE</th>
                    <th data-sort="1" onclick="sortOnclick(this);">CALLSIGN</th>
                    <th data-sort="1" onclick="sortOnclick(this);">REGIS</th>
                    <th data-sort="1" onclick="sortOnclick(this);">R_CRAFT</th>
                    <th data-sort="1" onclick="sortOnclick(this);">F_CRAFT</th>
                    <th data-sort="1" onclick="sortOnclick(this);">PURPOSE</th>
                    <th data-sort="1" onclick="sortOnclick(this);">P_TYPE</th>
                    <th data-sort="1" onclick="sortOnclick(this);">FROM</th>
                    <th data-sort="1" onclick="sortOnclick(this);">TO</th>

                    <th data-sort="1" onclick="sortOnclick(this);">ATD</th>
                    <th data-sort="1" onclick="sortOnclick(this);">ATA</th>

                    <th data-sort="1" onclick="sortOnclick(this);">VIA</th>
                    <th data-sort="1" onclick="sortOnclick(this);">FPL_VIA</th>
                    <th data-sort="1" onclick="sortOnclick(this);">REMARK</th>
                    <th data-sort="1" onclick="sortOnclick(this);">ETD</th>
                    <th data-sort="1" onclick="sortOnclick(this);">ETA</th>
                    <th data-sort="1" onclick="sortOnclick(this);">LASTUSER</th>

                    <th></th>
                </tr>
            </thead>
            <tbody></tbody>
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
        var ddlTime_ID = document.getElementById('ddlTime');
        var ddlOPer_ID = document.getElementById('ddlSelect');
        var ddlTypeFlight = document.getElementById('ddlType');       
        var ddlPageSize = document.getElementById('ddlPageSize');
        var ddlFr = document.getElementById('ddlFir');
        document.getElementById("txtFromTime").disabled = 'true';
        document.getElementById("txtToTime").disabled = 'true';
        var isSearch = false;
        var rowId = 0;
        var pageSize = parseInt(ddlPageSize.value);
        $('#tblSource').paging({ pageSize: pageSize });

        function Render2Table(data) {

            var kq = '';
            $('#tblSource').paging({ pageSize: pageSize });
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            //var pz = parseInt($('#tblSource').attr('data-pagesize'));
            var pz = parseInt(ddlPageSize.value);

            var khb_delete = $('#chkKhbDelete').prop('checked');
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
	        
            $.each(data.ListValue, function (a, b) {
                 
                kq += "<tr id='" + b.FLIGHT_ID + "' data-isUpdate='false' onmouseover='rowId=" + b.FLIGHT_ID + ";' onmouseout='rowId=0;'>"

                        + "<td id='b_" + b.RNUM + "'>" + stt + "</td>"
                       
                        + "<td><input type=\"checkbox\" id='chk_" + b.CALLSIGN + "' /></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtOper" + a + "' data-oldValue='" + returnEmpty(b.OPER) + "' value='" + returnEmpty(b.OPER) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" class='sInput' id='txtFLIGHTDATE" + a + "' data-oldValue='" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "' value='" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "' onblur='checkIsUpdate(this)' /></td>"
                        + "<td><input type='text' data-control='_updateAll' data-minlenght='2' maxlength='8' class='sInput' id='txtCALLSIGN" + a + "' data-oldValue='" + returnEmpty(b.CALLSIGN) + "' value='" + returnEmpty(b.CALLSIGN) + "' onblur='checkIsUpdate(this)' /></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtRegis" + a + "' data-oldValue='" + returnEmpty(b.REGIS) + "' value='" + returnEmpty(b.REGIS) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtRCRAFT" + a + "' data-oldValue='" + returnEmpty(b.RCRAFT) + "' value='" + returnEmpty(b.RCRAFT) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtFCRAFT" + a + "' data-oldValue='" + returnEmpty(b.FCRAFT) + "' value='" + returnEmpty(b.FCRAFT) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtPurpose" + a + "' data-oldValue='" + returnEmpty(b.PURPOSE) + "' value='" + returnEmpty(b.PURPOSE) + "' onblur='checkIsUpdate(this)'/></td>"

                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtP_TYPE" + a + "' data-oldValue='" + returnEmpty(b.P_TYPE) + "' value='" + returnEmpty(b.P_TYPE) + "' onblur='checkIsUpdate(this)'/></td>"

                        + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP" + a + "' data-oldValue='" + returnEmpty(b.FROM_AIRP) + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + returnEmpty(b.FROM_AIRP) + "' onblur='checkIsUpdate(this)' /></td>"
                        + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP" + a + "' data-oldValue='" + returnEmpty(b.TO_AIRP) + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + returnEmpty(b.TO_AIRP) + "' onblur='checkIsUpdate(this)'/></td>"
                       + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtATD" + a + "' data-oldValue='" + returnEmpty(b.ATD) + "'  value='" + returnEmpty(b.ATD) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtATA" + a + "' data-oldValue='" + returnEmpty(b.ATA) + "'  value='" + returnEmpty(b.ATA) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA" + a + "' data-oldValue='" + returnEmpty(b.VIA) + "' value='" + returnEmpty(b.VIA) + "' onblur='checkIsUpdate(this)'/></td>"
                         + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtFPLVIA" + a + "' data-oldValue='" + returnEmpty(b.FPLVIA) + "' value='" + returnEmpty(b.FPLVIA) + "' onblur='checkIsUpdate(this)'/></td>"

                        + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "'  data-oldValue='" + returnEmpty(b.REMARK) + "' value='" + returnEmpty(b.REMARK) + "' onblur='checkIsUpdate(this)' data-toggle='tooltip' data-placement='top' title ='" + returnEmpty(b.REMARK) + "'/></td>"
                        + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' id='txtETD" + a + "' data-oldValue='" + returnEmpty(b.ETD) + "'  value='" + returnEmpty(b.ETD) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA" + a + "' data-oldValue='" + returnEmpty(b.ETA) + "'  value='" + returnEmpty(b.ETA) + "' onblur='checkIsUpdate(this)'/></td>"
                        + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtLastUser" + a + "' data-oldValue='" + returnEmpty(b.USERCREATE) + "'  value='" + returnEmpty(b.USERCREATE) + "' onblur='checkIsUpdate(this)'/></td>"

                        + "<td style=\"white-space: nowrap;\">"
                        + "<div class=\"action-buttons\">"
                        + "<i id=\"btnDeleteRemark" + b.FLIGHT_ID + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"btnDeleteOnclick(" + b.FLIGHT_ID + ");\"></i>"
                       
                        + "</td>"
                        + "</tr>";
                stt++;
            });
            return kq;
        }

        function RenderTable(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);            
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"                    
                    + "<td>" + stt + "</td>"
                    + "<td></td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.REAL_CRAFT_TYPE) + "</td>"                   
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.CRAFT_TYPE) + "</td>"
                   
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: center;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.ATD) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.ATA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FPL_VIA) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.REMARK) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.LASTUSER) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.ETA) + "</td>"                    
                   
                    + "<td></td>"
                    + "</tr>";
                stt++;
            });
            return kq;
        }

        function RenderTableKhDelete(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);
            var khb_delete = $('#chkKhbDelete').prop('checked');
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"
                   
                    + "<td>" + stt + "</td>"
                    + "<td></td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REAL_CRAFT_TYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CRAFT_ID) + "</td>"
                    
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATA) + "</td>"                    
                    + "<td style=\'text-align: left;\'><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtVIA" + a + "'  value='" + returnEmpty(b.VIA) + "' /></td>"
                    + "<td></td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"                 
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.LASTUSER) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETA) + "</td>"

                   
                    + "<td></td>"
                    + "</tr>";
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

        

        function LoadDataGrid_Finished() {
           
           var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=make_finished_flights_news";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_STRING: 'TEST' }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                    alert('Update error!');
                } else alert('Update success!');
            })
        }
        function btnDeleteRemark_Onclick(el) {
            var isDel = false;
            var $ele = $(el);
            var $remark = $(el).closest('tr').find('[id^=txtREMARK]');
            var isDel = $ele.hasClass('fa-trash-o');
            if (isDel) {
                $remark.val('');
            }
            if ($remark.attr('data-oldvalue') != '') {

            }
        }
        function returnEmpty(val) {
            return val == null ? "" : val;
        }
        function LoadDataGrid() {

            var _urlPath = "";
           // _urlPath: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=sp_ListFinishedFlight";
            //if ((parseInt(ddlOPer_ID.value) == 0) && (parseInt(ddlTypeFlight.value) == 0))
            //    _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_KHUNGTIME";
            //else if ((parseInt(ddlOPer_ID.value) == 1) && (parseInt(ddlTypeFlight.value) == 0))
            //    _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_HANG_QNOI";
            //else if ((parseInt(ddlOPer_ID.value) == 2) && (parseInt(ddlTypeFlight.value) == 0))
            //    _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_HANG_QTE";
            //else if ((parseInt(ddlOPer_ID.value) == 0) && (parseInt(ddlTypeFlight.value) == 1)) {
            //    _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_BAY_QNOI";
            //}
            //else if ((parseInt(ddlOPer_ID.value) == 0) && (parseInt(ddlTypeFlight.value) == 2))
            //    _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_BAY_QTE";
            //else if ((parseInt(ddlOPer_ID.value) == 1) && (parseInt(ddlTypeFlight.value) == 1))
            //    _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQN_BAY_QNOI";
            //else if ((parseInt(ddlOPer_ID.value) == 1) && (parseInt(ddlTypeFlight.value) == 2))
            //    _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQN_BAY_QTE";
            //else if ((parseInt(ddlOPer_ID.value) == 2) && (parseInt(ddlTypeFlight.value) == 1))
            //    _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQT_BAY_QNOI";
            //else if ((parseInt(ddlOPer_ID.value) == 2) && (parseInt(ddlTypeFlight.value) == 2))
            //    _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQT_BAY_QTE";

            var FROMDATE = $('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            var TODATE = $('#txtToDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            var FROM_AIRP = $('#txtFROM_AIRP').val();
            var TO_AIRP = $('#txtTO_AIRP').val();

            var requestData = GetObjectSearch();
            console.log('[GET_FINISHED_FLIGHTS_MINITARY] Request:', requestData);

            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GET_FINISHED_FLIGHTS_MINITARY",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(requestData),
                //data: JSON.stringify({ P_FROMDATE: FROMDATE, P_TODATE: TODATE, P_FROM_AIRP: FROM_AIRP, P_TO_AIRP: TO_AIRP }),
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
                    /*
                    $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                        if ($(b).prop('id').indexOf('txtPERMDATE') == -1 && $(b).prop('id').indexOf('txtFLIGHTDATE') == -1)
                            $(b).ValidateTip();
                        else $(b).multiDate();
                    });
                    $("#tblSource td").click(function () {
                        $('#tblSource tbody tr').removeClass('select');
                        $('#tblSource tbody tr').eq(parseInt($(this).parent().index())).addClass('select');
                    });*/

                },
            }).done(function (data) {
                console.log('[GET_FINISHED_FLIGHTS_MINITARY] Response:', data);

                if (!data || data.ListValue == null || data.ListValue.length == 0) {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    $("#totalsfinished").html("Tổng số : <b>0</b>");
                    return;
                }
             
                $('#tblSource tbody tr').remove();
                $('#tblSource').attr('data-total', data.ListValue[0]['SUMRECORD']);
                var strAppend = Render2Table(data);
                $('#tblSource tbody').append(strAppend);
                $("#totalsfinished").html("Tổng số : <b>" + data.ListValue[0]['SUMRECORD'] + "</b>");
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error(
                    '[GET_FINISHED_FLIGHTS_MINITARY] Request failed:',
                    {
                        status: xhr.status,
                        textStatus: textStatus,
                        error: errorThrown,
                        response: xhr.responseJSON || xhr.responseText
                    }
                );

                $('#tblSource tbody tr').remove();
                $('#tblSource').attr('data-total', 0);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                alert('Không tải được dữ liệu chuyến bay quân sự. Vui lòng xem Console để biết chi tiết.');
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

        function LoadDataGrid_Finished_QNDI() {

           
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/FinishedFlights/GET_FINISHED_FLIGHTS_BAY_QNOI_MOVE",
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
                        onClickButton: 'LoadDataGrid_Finished_QNDI',
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
                $('#tblSource').attr('data-total', data.ListValue[0]['SUMRECORD']);
                var strAppend = RenderTable(data);
                $('#tblSource tbody').append(strAppend);
                $("#totalsfinished").html("Tổng số : <b>" + data.ListValue[0]['SUMRECORD'] + "</b>");
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

        function LoadDataGrid_Finished_QTVE() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/FinishedFlights/GET_FINISHED_FLIGHTS_QTVE_MOVE",
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
                        onClickButton: 'LoadDataGrid_Finished_QTVE',
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
                $('#tblSource').attr('data-total', data.ListValue[0]['SUMRECORD']);
                var strAppend = RenderTable(data);
                $('#tblSource tbody').append(strAppend);
                $("#totalsfinished").html("Tổng số : <b>" + data.ListValue[0]['SUMRECORD'] + "</b>");
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

        function LoadDataGrid_Finished_CHOT_SL() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/FinishedFlights/GET_FINISHED_FLIGHTS_CHOTSL_MOVE",
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
                        onClickButton: 'LoadDataGrid_Finished_CHOT_SL',
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
                $('#tblSource').attr('data-total', data.ListValue[0]['SUMRECORD']);
                var strAppend = RenderTable(data);
                $('#tblSource tbody').append(strAppend);
                $("#totalsfinished").html("Tổng số : <b>" + data.ListValue[0]['SUMRECORD'] + "</b>");
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }


        function LoadDataGrid_Finished_NotComplate() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/FinishedFlights/GET_FINISHED_F_NOTCOMPLATE",
		//url: urlApi + "api/ApiExtension/ExcuteTable?packageName=FINISH_FLIGHTS_PKG&storeName=GET_FINISHED_F_NOTCOMPLATE",
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
                        onClickButton: 'LoadDataGrid_Finished_NotComplate',
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
                $('#tblSource').attr('data-total', data.ListValue[0]['SUMRECORD']);
                var strAppend = RenderTableKhDelete(data);
                $('#tblSource tbody').append(strAppend);
                $("#totalsfinished").html("Tổng số : <b>" + data.ListValue[0]['SUMRECORD'] + "</b>");
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }




        function GetObjectSearch() {
            var _obj = {};
            var currentPageIndex;

            if (isSearch) {
                $('#tblSource').attr('data-pageIndex', 1);
            }

            currentPageIndex = parseInt($('#tblSource').attr('data-pageIndex'), 10);
            if (isNaN(currentPageIndex) || currentPageIndex < 1) {
                currentPageIndex = 1;
                $('#tblSource').attr('data-pageIndex', currentPageIndex);
            }

            _obj['P_PAGESIZE'] = parseInt(ddlPageSize.value, 10);
            _obj['P_PAGEINDEX'] = currentPageIndex - 1;
            _obj['P_CALLSIGN'] = $.trim($('#txtCALLSIGN').val());
            _obj['P_REGIS'] = $.trim($('#txtRegis').val());
            _obj['P_FROM_AIRP'] = $('#txtFROM_AIRP').val();
            _obj['P_TO_AIRP'] = $('#txtTO_AIRP').val();
            _obj['P_PURPOSE'] = $.trim($('#txtPurpose').val());

            _obj['P_OPER'] = $.trim($('#txtOper').val());


            _obj['P_FCRAFT'] = $('#txtFCraf').val();
            _obj['P_RCRAFT'] = $('#txtRCraf').val();

            _obj['P_P_TYPE'] = $('#txtP_TYPE').val();

            _obj['P_VIA'] = $.trim($('#txtVia').val());
            _obj['P_FPLVIA'] = $.trim($('#txtFPLVia').val());
            _obj['P_REMARK'] = $('#txtREMARK').val();
            _obj['P_ETA'] = $('#txtETA').val();
            _obj['P_ETD'] = $('#txtETD').val();
            _obj['P_ATA'] = $('#txtATA').val();
            _obj['P_ATD'] = $('#txtATD').val();
            _obj['P_ISACCEPTED'] = parseInt($('#ddlAcceptedStatus').val(), 10);

            _obj['P_STARTDATE'] = $('#txtFromDate').val();
            _obj['P_FINISHDATE'] = $('#txtToDate').val();
            _obj['P_KHUNGGIO1'] = $('#txtFromTime').val();
            _obj['P_KHUNGGIO2'] = $('#txtToTime').val();

            _obj['P_CAT_HA'] = parseInt(ddlTime_ID.value, 10) || 0;


            isSearch = false;
            return _obj;
        }

        function btnAccepted_OnClick() {
            var startDateIso = $('#txtFromDatePicker').val();
            var finishDateIso = $('#txtToDatePicker').val();

            if (!startDateIso || !finishDateIso) {
                alert('Vui lòng chọn đầy đủ ngày FROM và TO.');
                return;
            }

            if (startDateIso > finishDateIso) {
                alert('Ngày FROM không được lớn hơn ngày TO.');
                return;
            }

            var startDate = militaryDateToDisplay(startDateIso);
            var finishDate = militaryDateToDisplay(finishDateIso);
            var message = 'Accepted toàn bộ chuyến bay quân sự từ '
                + startDate + ' đến ' + finishDate + '?';

            if (!confirm(message)) {
                return;
            }

            $('#btnAccepted').prop('disabled', true);

            $.ajax({
                method: 'PUT',
                url: urlApi
                    + 'api/ApiExtension/ExcuteReturnInt'
                    + '?packageName=A_TEST_SEARCH'
                    + '&storeName=ACCEPT_FIN_FLIGHTS_MILITARY',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    P_STARTDATE: startDate,
                    P_FINISHDATE: finishDate,
                    P_USER: '<%= _user.UserName.ToString()%>'
                })
            }).done(function (data) {
                var updatedRows = data && data.Code === '00'
                    ? parseInt(data.ListValue, 10)
                    : -1;

                if (isNaN(updatedRows) || updatedRows < 0) {
                    console.error('[ACCEPT_FIN_FLIGHTS_MILITARY] Response:', data);
                    alert('Accepted không thành công. Vui lòng xem Console để biết chi tiết.');
                    return;
                }

                alert('Accepted thành công ' + updatedRows + ' chuyến bay.');
                $('#ddlAcceptedStatus').val('0');
                acceptedStatus_OnChange();
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error(
                    '[ACCEPT_FIN_FLIGHTS_MILITARY] Request failed:',
                    {
                        status: xhr.status,
                        textStatus: textStatus,
                        error: errorThrown,
                        responseText: xhr.responseText
                    }
                );
                alert('Không thể thực hiện Accepted. Vui lòng xem Console để biết chi tiết.');
            }).always(function () {
                $('#btnAccepted').prop('disabled', false);
            });
        }

        function updateQsExportButtonState() {
            var canExport = $('#ddlAcceptedStatus').val() === '1';
            $('#btnExportQsMessage').prop('disabled', !canExport);
        }

        function acceptedStatus_OnChange() {
            updateQsExportButtonState();
            btnSearch_OnClick();
        }

        function btnExportQsMessage_OnClick() {
            if ($('#ddlAcceptedStatus').val() !== '1') {
                alert('Chỉ được Export Message khi đang chọn ĐÃ ACCEPTED.');
                updateQsExportButtonState();
                return;
            }

            var startDateIso = $('#txtFromDatePicker').val();
            var finishDateIso = $('#txtToDatePicker').val();

            if (!startDateIso || !finishDateIso) {
                alert('Vui lòng chọn đầy đủ ngày FROM và TO.');
                return;
            }

            if (startDateIso > finishDateIso) {
                alert('Ngày FROM không được lớn hơn ngày TO.');
                return;
            }

            var startDate = militaryDateToDisplay(startDateIso);
            var finishDate = militaryDateToDisplay(finishDateIso);

            if (!confirm(
                'Export các chuyến ĐÃ ACCEPTED từ '
                + startDate + ' đến ' + finishDate
                + ' thành QS MESSAGE?'
            )) {
                return;
            }

            $('#btnExportQsMessage').prop('disabled', true);

            $.ajax({
                method: 'PUT',
                url: urlApi
                    + 'api/ApiExtension/ExcuteReturnInt'
                    + '?packageName=A_TEST_SEARCH'
                    + '&storeName=EXPORT_QS_PLAN_MESSAGE',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    P_STARTDATE: startDate,
                    P_FINISHDATE: finishDate,
                    P_USER: '<%= _user.UserName.ToString()%>'
                })
            }).done(function (data) {
                var exportedFlights = data && data.Code === '00'
                    ? parseInt(data.ListValue, 10)
                    : -1;

                if (isNaN(exportedFlights) || exportedFlights < 0) {
                    console.error('[EXPORT_QS_PLAN_MESSAGE] Response:', data);
                    alert('Export QS MESSAGE không thành công. Vui lòng xem Console.');
                    return;
                }

                alert(
                    'Đã Export ' + exportedFlights
                    + ' chuyến bay thành QS MESSAGE.'
                );
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error(
                    '[EXPORT_QS_PLAN_MESSAGE] Request failed:',
                    {
                        status: xhr.status,
                        textStatus: textStatus,
                        error: errorThrown,
                        responseText: xhr.responseText
                    }
                );
                alert('Không thể Export QS MESSAGE. Vui lòng xem Console.');
            }).always(function () {
                updateQsExportButtonState();
            });
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
        function UpdateFlightAll(){
           const local_date = new Date().toLocaleString();
           var $lis = $('tr[data-isUpdate="true"]');
            if ($lis.length == 0) { alert('No Update.'); return; };
            var d = checkValidCustomMinlenght('_updateAll');
            if (!d) return;
            var c = 0;
            $.each($lis, function (a, b) {
                var _obj = {};
                _obj['P_FLIGHT_ID'] = '0';
                _obj['P_FLIGHTDATE'] = $($(b).find('[id^="txtFLIGHTDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['P_OPER'] = $($(b).find('[id^="txtOper"]')[0]).val();
                _obj['P_CALLSIGN'] = $($(b).find('[id^="txtCALLSIGN"]')[0]).val();
                _obj['P_REGIS'] = $($(b).find('[id^="txtRegis"]')[0]).val();
                _obj['P_RCRAFT'] = $($(b).find('[id^="txtRcraf"]')[0]).val();
                _obj['P_FCRAFT'] = $($(b).find('[id^="txtFcraf"]')[0]).val();
                _obj['P_PURPOSE'] = $($(b).find('[id^="txtPurpose"]')[0]).val();
                _obj['P_PTYPE'] = $($(b).find('[id^="txtP_TYPE"]')[0]).val();
                _obj['P_FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['P_TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['P_ATD'] = $($(b).find('[id^="txtATD"]')[0]).val();
                _obj['P_ATA'] = $($(b).find('[id^="txtATA"]')[0]).val();
                _obj['P_VIA'] = $($(b).find('[id^="txtVia"]')[0]).val();
                _obj['P_FPLVIA'] = $($(b).find('[id^="txtFPLVia"]')[0]).val();
                _obj['P_ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['P_ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                
                _obj['P_USERCREATE'] = '<%= _user.UserName.ToString()%>';
                _obj['P_DATECREATE'] = local_date;
                _obj['P_REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
             
                var $request = $.ajax({
                    async: false,
                    method: "Put",
                    //url: urlApi + "api/FinishedFlights/Delete/" + id,
                    url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=sp_UpdateFinishedFlight",
                    data: JSON.stringify(_obj)
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            alert('Update sussess: ' + c + '/' + $lis.length);
            btnSearch_OnClick();
        }

        function btnInsertList_Onclick() {
            const local_date = new Date().toLocaleString();
            var $lis = $('tr[data-isInsert="true"]');
            if ($lis.length == 0) { alert('No Insert.'); return; };
            var d = checkValidCustomMinlenght('_updateAll');
            if (!d) return;
            var c = 0;
            $.each($lis, function (a, b) {
                var _obj = {};
                _obj['P_FLIGHT_ID'] = '0';
                _obj['P_FLIGHTDATE'] = $($(b).find('[id^="txtFLIGHTDATE"]')[0]).val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                _obj['P_OPER'] = $($(b).find('[id^="txtOper"]')[0]).val();
                _obj['P_CALLSIGN'] = $($(b).find('[id^="txtCALLSIGN"]')[0]).val();
                _obj['P_REGIS'] = $($(b).find('[id^="txtRegis"]')[0]).val();
                _obj['P_RCRAFT'] = $($(b).find('[id^="txtRcraf"]')[0]).val();
                _obj['P_FCRAFT'] = $($(b).find('[id^="txtFcraf"]')[0]).val();
                _obj['P_PURPOSE'] = $($(b).find('[id^="txtPurpose"]')[0]).val();
                _obj['P_PTYPE'] = $($(b).find('[id^="txtP_TYPE"]')[0]).val();
                _obj['P_FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                _obj['P_TO_AIRP'] = $($(b).find('[id^="txtTO_AIRP"]')[0]).val();
                _obj['P_ATD'] = $($(b).find('[id^="txtATD"]')[0]).val();
                _obj['P_ATA'] = $($(b).find('[id^="txtATA"]')[0]).val();
                _obj['P_VIA'] = $($(b).find('[id^="txtVia"]')[0]).val();
                _obj['P_FPLVIA'] = $($(b).find('[id^="txtFPLVia"]')[0]).val();
                _obj['P_ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['P_ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                
                _obj['P_USERCREATE'] = '<%= _user.UserName.ToString()%>';
                _obj['P_DATECREATE'] = local_date;
                _obj['P_REMARK'] = $($(b).find('[id^="txtREMARK"]')[0]).val();
                console.log(_obj);
                console.log(_obj);
                var $request = $.ajax({
                    async: false,
                    method: "Put",
                    //url: urlApi + "api/FinishedFlights/Delete/" + id,
                    url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=sp_InsertFinishedFligh",
                    data: JSON.stringify(_obj)
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
                    _obj['PERMNBR'] = '';
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
                //btnSearch_OnClick();
            }

            function btnMakeFinished_Onclick() {
                var result = confirm("Do you want move data to finished fly?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "Put",
                        url: urlApi + "api/FinishedFlights/Make_Finished",
                    }).always(function (data) {
                        if (data.Code != -1) {                        
                            alert('Sussess!');
                            btnSearch_OnClick();
                        } else alert('Error!');
                    });
                }
            }

        
            function btnSendOnclick(id) {
                var result = confirm("Do you want send?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "Put",
                        //url: urlApi + "api/FinishedFlights/Delete/" + id,
                        url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=sp_SendFinishedFlight",
                        data: JSON.stringify({ P_FLIGHT_ID: id, P_USER: '<%= _user.UserName.ToString()%>' }),
                    }).always(function (data) {
                        if (data.Code != -1) {
                            $('#' + id).remove();
                            alert('Send sussess!');
                            btnSearch_OnClick();
                        } else alert('Send error!');
                    });
                }
            }
            function btnDeleteOnclick(id) {
                var result = confirm("Do you want delete?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "Put",
                        //url: urlApi + "api/FinishedFlights/Delete/" + id,
                        url: urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=A_TEST_SEARCH&storeName=sp_DeleteFinishedFlight",
                        data: JSON.stringify({ P_FLIGHT_ID: id, P_USER: '<%= _user.UserName.ToString()%>' }),
                    }).always(function (data) {
                        if (data.Code != -1) {
                            $('#' + id).remove();
                            alert('Delete sussess!');
                            btnSearch_OnClick();
                        } else alert('Delete error!');
                    });
                }
            }



            function UpdateMoveDateQNDi() {
                var result = confirm("Do you want move date?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "PUT",
                        url: urlApi + "api/FinishedFlights/UpdateMoveQNDI",
                        data: GetObjectSearch(),
                    }).always(function (data) {
                        if (data.Code != -1) {
                            btnSearch_OnClick();
                        } else alert('Move date error!');
                    });
                }
            }
            function UpdateMoveDateQTVE() {
                var result = confirm("Do you want move date?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "PUT",
                        url: urlApi + "api/FinishedFlights/UpdateMoveQTVE",
                        data: GetObjectSearch(),
                    }).always(function (data) {
                        if (data.Code != -1) {                        
                            btnSearch_OnClick();
                        } else alert('Move date error!');
                    });
                }
            }
            function ExportBravo()
            {
           
                var result = confirm("Do you want move data to Bravo?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "PUT",
                        url:  urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=sp_CoppyBravo",
                        data: JSON.stringify({ P_DATE: $('#txtFromDate').val(), P_EDATE: $('#txtToDate').val() }),
                    }).always(function (data) {
                        if (data.ListValue == null || data.ListValue == -1) {
                            alert('Move error!');
                        } else alert('Move success!');
                    })
                }
        
            }
            function UpdateMoveDateCHOT() {
                var result = confirm("Do you want move date?");
                if (result) {
                    var $request = $.ajax({
                        async: false,
                        method: "PUT",
                        url: urlApi + "api/FinishedFlights/UpdateMoveCHOTSL",
                        data: GetObjectSearch(),
                    }).always(function (data) {
                        if (data.Code != -1) {                        
                            btnSearch_OnClick();
                        } else alert('Move date error!');
                    });
                }
            }

            function chkKhb_CheckedChanged() {
                pageSize = parseInt(ddlPageSize.value);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                $('#tblSource').attr('data-pageIndex', 1);
                isSearch = true;
                btnSearch_Full();
            }
            function chkKhbDelete_CheckedChanged() {
                pageSize = parseInt(ddlPageSize.value);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                $('#tblSource').attr('data-pageIndex', 1);
                isSearch = true;
                btnSearch_Full();
            }
            function chkQndi_CheckedChanged() {
                pageSize = parseInt(ddlPageSize.value);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                $('#tblSource').attr('data-pageIndex', 1);
                isSearch = true;
                btnSearch_Full();
            }
            function chkQtve_CheckedChanged() {
                pageSize = parseInt(ddlPageSize.value);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                $('#tblSource').attr('data-pageIndex', 1);
                isSearch = true;
                btnSearch_Full();
            }
            function chkChot_CheckedChanged() {
                pageSize = parseInt(ddlPageSize.value);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                $('#tblSource').attr('data-pageIndex', 1);
                isSearch = true;
                btnSearch_Full();
            }
            function btnSearch_OnClick() {
                btnSearch_Full();
            }
            function btnExportexcel_OnClick() {
                var FROMDATE = $('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
                var TODATE = $('#txtToDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');

                var $request = $.ajax({
                    method: "PUT",
                    url: urlApi + "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=sp_ListFinishedFlight",
                    data: JSON.stringify({ P_FROMDATE: FROMDATE, P_TODATE: TODATE }),
                    beforeSend: function () {
                    },
                    complete: function () {
                        unLoadingData('loaddingData');
                    },
                }).always(function (data) {
                    if (data.ListValue == null) {
                        unLoadingData('loaddingData');
                        return;
                    }

                    var strAppend = RenderTableKhExportExcel(data);

                    var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                    strHtml += "<thead style='color: red'>"
                            + "<tr>"
                            + "<th>No</th>"
                            + "<th>OPER</th>"
                            + "<th>FLIGHTDATE</th>"
                            + "<th>CALLSIGN</th>"
                            + "<th>REGIS</th>"
                            + "<th>R_CRAFT</th>"
                            + "<th>F_CRAFT</th>"
                            + "<th>PURPOSE</th>"
                            + "<th>P_TYPE</th>"
                            + "<th>FROM</th>"
                            + "<th>TO</th>"
                       
                            + "<th>ATD</th>"
                            + "<th>ATA</th>"
                            + "<th>VIA</th>"
                            + "<th>FPL_VIA</th>"
                            + "<th>REMARK</th>"
                       
                            + "<th>ETD</th>"
                            + "<th>ETA</th>"
                            + "</tr>"
                            + "</thead>"
                            + strAppend
                            + "<tbody>"
                            + "</tbody>"
                            + "</table>";


                    generate_excel(strHtml);


                });
                $request.onreadystatechange = null;
                $request.abort = null;
                $request = null;



           
            }
        

            function btnSearch_Full() {
                /*
                var khb = $('#chkKhb').prop('checked');
                var khb_delete = $('#chkKhbDelete').prop('checked');
                var khb_qndi = $('#chkQndi').prop('checked');
                var khb_qtve = $('#chkQtve').prop('checked');
                var khb_chot = $('#chkChot').prop('checked');
    
                if (khb) {$('#btnExport80').attr('disabled', 'disabled'); $('#btnInsertList').removeAttr('disabled'); $('#btnUpdateList').removeAttr('disabled'); $('#btnDeleteByChecked').removeAttr('disabled'); $('#btnExport').removeAttr('disabled'); $('#btnMove').attr('disabled', 'disabled'); $('#ddlTime').removeAttr('disabled'); $('#ddlSelect').removeAttr('disabled'); $('#ddlType').removeAttr('disabled'); $('#ddlFir').removeAttr('disabled'); btnSearch(); }
                if (khb_delete) { $('#btnExport80').removeAttr('disabled', 'disabled');$('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); $('#btnInsertList').attr('disabled', 'disabled'); $('#ddlSelect').attr('disabled', 'disabled'); $('#ddlType').attr('disabled', 'disabled'); $('#ddlTime').attr('disabled', 'disabled'); $('#btnExport').attr('disabled', 'disabled'); $('#btnMove').removeAttr('disabled'); LoadDataGrid_Finished_NotComplate(); }
                if (khb_qndi) { $('#btnExport80').attr('disabled', 'disabled');$('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); $('#btnInsertList').attr('disabled', 'disabled'); $('#ddlSelect').attr('disabled', 'disabled'); $('#ddlType').attr('disabled', 'disabled'); $('#ddlTime').attr('disabled', 'disabled'); $('#btnExport').attr('disabled', 'disabled'); $('#btnMove').removeAttr('disabled'); LoadDataGrid_Finished_QNDI(); }
                if (khb_qtve) { $('#btnExport80').attr('disabled', 'disabled');$('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); $('#btnInsertList').attr('disabled', 'disabled'); $('#ddlSelect').attr('disabled', 'disabled'); $('#ddlType').attr('disabled', 'disabled'); $('#ddlTime').attr('disabled', 'disabled'); $('#btnExport').attr('disabled', 'disabled'); $('#btnMove').removeAttr('disabled'); LoadDataGrid_Finished_QTVE(); }
                if (khb_chot) { $('#btnExport80').attr('disabled', 'disabled');$('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); $('#btnInsertList').attr('disabled', 'disabled'); $('#ddlSelect').attr('disabled', 'disabled'); $('#ddlType').attr('disabled', 'disabled'); $('#ddlTime').attr('disabled', 'disabled'); $('#btnExport').attr('disabled', 'disabled'); $('#btnMove').removeAttr('disabled'); LoadDataGrid_Finished_CHOT_SL(); }
                */
                btnSearch();
            }
            function btnSearch() {
                pageSize = parseInt(ddlPageSize.value);
                $("#totalsfinished").html("Tổng số : <b>0</b>");
                isSearch = true;
                LoadDataGrid();

            }

            function btnClearValue_OnClick() {
                $('#txtCALLSIGN').val('');
                $('#txtRegis').val('');
                $('#txtFROM_AIRP').val('');
                $('#txtTO_AIRP').val('');
                $('#txtETD').val('');
                $('#txtETA').val('');
                $('#txtDATE_OLD').val('');
                $('#txtFLIGHTDATE').val('');
                $('#txtATD').val('');
                $('#txtATA').val('');
                $('#txtPERMTYPE').val('');
                $('#txtOper').val('');
                $('#txtRCraf').val('');
                $('#txtFCraf').val('');
                $('#txtPurpose').val('');
                $('#txtP_TYPE').val('');
                $('#txtLASTUSER').val('');
                $('#txtVia').val('');
                $('#txtREMARK').val('');
                $('#txtFPLVia').val('');
                ddlTime_ID.selectedIndex = 0;
                $('#ddlAcceptedStatus').val('0');
                updateQsExportButtonState();
                if (ddlOPer_ID) ddlOPer_ID.selectedIndex = 0;
                if (ddlTypeFlight) ddlTypeFlight.selectedIndex = 0;
                ddlPageSize.selectedIndex = 0;
                document.getElementById("txtFromTime").disabled = 'true';
                document.getElementById("txtToTime").disabled = 'true';
                document.getElementById("txtFromTime").value = '0000';
                document.getElementById("txtToTime").value = '2359';
                var txtFromAir = document.getElementById("txtFromAir");
                if (txtFromAir) txtFromAir.value = '';
            }
            function ddlTime_Change() {
                if (document.getElementById("ddlTime").value != "0") {
                    document.getElementById("txtFromTime").disabled = '';
                    document.getElementById("txtToTime").disabled = '';
                } else {
                    document.getElementById("txtFromTime").disabled = 'true';
                    document.getElementById("txtToTime").disabled = 'true';
                    document.getElementById("txtFromTime").value = '0000';
                    document.getElementById("txtToTime").value = '2359';
                }
            }

            function btnMove_OnClick() {            
                var khb_qndi = $('#chkQndi').prop('checked');
                var khb_qtve = $('#chkQtve').prop('checked');
                var khb_chot = $('#chkChot').prop('checked');
                       
                if (khb_qndi) { UpdateMoveDateQNDi(); }
                if (khb_qtve) { UpdateMoveDateQTVE(); }
                if (khb_chot) { UpdateMoveDateCHOT(); }
            }

            function btnMove_OnClick() {

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
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' maxlength='8' class='sInput' style='background-color:darkkhaki;' id='txtOper" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" data-CheckDate='true' class='sInput' style='background-color:darkkhaki;' id='txtFLIGHTDATE" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' maxlength='8' class='sInput' style='background-color:darkkhaki;' id='txtCALLSIGN" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtRegis" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtRcraf" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtFcraf" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtPurpose" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtP_TYPE" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"

                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' style='background-color:darkkhaki;' id='txtFROM_AIRP" + c + "' value='' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' style='background-color:darkkhaki;' id='txtTO_AIRP" + c + "' value='' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtATD" + c + "' value=''   onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtATA" + c + "' value=''   onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtVia" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtFPLVia" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                     + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtREMARK" + c + "'  value='' onblur='checkIsUpdate(this)'/></td>"

                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtETD" + c + "' value=''  onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtETA" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                   
                   
                    
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

            //reloadAutoComplete();
        }

    </script>
    <script>

        $('#txtFromDate').val(dateFormat(new Date().setDate(new Date().getDate() - 1), 'dd-mm-yyyy'));
        $('#txtFromDate').multiDate();
        $('#txtToDate').val(dateFormat(new Date().setDate(new Date().getDate() - 1), 'dd-mm-yyyy'));
        $('#txtToDate').multiDate();

        LoadDataGrid();

        function militaryDateToIso(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3 ? parts[2] + '-' + parts[1] + '-' + parts[0] : '';
        }
        function militaryDateToDisplay(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3 ? parts[2] + '-' + parts[1] + '-' + parts[0] : '';
        }
        $('#txtFromDatePicker').val(militaryDateToIso($('#txtFromDate').val()));
        $('#txtToDatePicker').val(militaryDateToIso($('#txtToDate').val()));
        $('#txtFromDatePicker').on('change', function () {
            $('#txtFromDate').val(militaryDateToDisplay(this.value));
        });
        $('#txtToDatePicker').on('change', function () {
            $('#txtToDate').val(militaryDateToDisplay(this.value));
        });
        $('#txtDATE_OLD').multiDate();
        $('#txtFLIGHTDATE').multiDate();
        $('#btnMove').attr('disabled', 'disabled');
    </script>
    <script>
        function AddNews() {
            var c = $('#tblSource tr').length;
            var _tr = "<tr id='_" + c + "' data-isInsert='true' onmouseover=\"rowId=$(this).prop('id');\" onmouseout='rowId=0;' style='background-color:darkkhaki;'>"


                    + "<td id='b_" + c + "'>" + (c - 1) + "</td>"
                    + "<td><input type=\"checkbox\" id='chk_" + c + "' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' maxlength='8' class='sInput' style='background-color:darkkhaki;' id='txtOper" + c + "' value='QSU' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" data-CheckDate='true' class='sInput' style='background-color:darkkhaki;' id='txtFLIGHTDATE" + c + "' value='" + $('#txtFromDate').val() + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' maxlength='8' class='sInput' style='background-color:darkkhaki;' id='txtCALLSIGN" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtRegis" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtRcraf" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtFcraf" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtPurpose" + c + "' value='' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtP_TYPE" + c + "' value='LD' onblur='checkIsUpdate(this)' /></td>"

                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' style='background-color:darkkhaki;' id='txtFROM_AIRP" + c + "' value='' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' style='background-color:darkkhaki;' id='txtTO_AIRP" + c + "' value='' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtATD" + c + "' value=''   onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtATA" + c + "' value=''   onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtVia" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtFPLVia" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"
                     + "<td><input type='text' data-control='_updateAll' class='sInput' style='background-color:darkkhaki;' id='txtREMARK" + c + "'  value='' onblur='checkIsUpdate(this)'/></td>"

                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtETD" + c + "' value=''  onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' style='background-color:darkkhaki;' id='txtETA" + c + "' value='' onblur='checkIsUpdate(this)'/></td>"



                    + "<td style=\"white-space: nowrap;background-color:darkkhaki;\">"
                    + "<div class=\"action-buttons\">"
                    + "<a data-toggle=\"tooltip\" title=\"Delete\">"
                    + "<i id=\"btnDelete_" + c + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"var ax = confirm('Do you want delete?'); if(ax){$('#_" + c + "').remove();}\"></i></a>"
                    + "<a data-toggle=\"tooltip\" onclick=\"btnInsertList_Onclick();\" title=\"Insert\"><i class=\"glyphicon glyphicon glyphicon-plus\"></i></a>"
                    + "</div>"
                    + "</td>"
                    + "</tr>"

            
                $('#tblSource tbody').append(_tr);
                $('#tblSource input[data-control="_updateAll"]').each(function(a, b) {
                    $(b).mouseover(function() {
                        onmouseoverInput(b.id)
                    });
                    $(b).mouseout(function() {
                        onmouseoutInput(b.id)
                    });
                    $(b).ValidateTip();
                })
                $('#_' + c + ' input')[0].focus();
                return;
          
            
        }

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
            myOrder();
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
            var fileNameToSaveAs = 'exported_finished_flight_' + postfix + '.xls';
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
        
        function RenderTableKhExportExcel(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"
                    + "<td>" + stt + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CALLSIGN) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REGIS) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.RCRAFT) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FCRAFT) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.P_TYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FPLVIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"
                    
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETA) + "</td>"

                    + "</tr>";
                stt++;
            });
            return kq;
        }

        function RenderTableKhExport(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"
                    + "<td>" + stt + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REAL_CRAFT_TYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CRAFT_TYPE) + "</td>"                    
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ATA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FPL_VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.LASTUSER) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETA) + "</td>"
                   
                    + "</tr>";
                stt++;
            });
            return kq;
        }
		
		 function LoadDataGrid_ExportCancel() {
            var _urlPath = "";
            _urlPath= "api/FinishedFlights/GET_FINISHED_F_NOTCOMPLATE"
			
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: GetObjectSearchExport(),
                beforeSend: function () {
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

                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        + "<th>No</th>"
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>REGIS</th>"
                        + "<th>R_CRAFT</th>"
                        + "<th>F_CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ATD</th>"
                        + "<th>ATA</th>"
                        + "<th>VIA</th>"
                        + "<th>FPL_VIA</th>"
                        + "<th>REMARK</th>"
                        + "<th>LASTUSER</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";


                generate_excel(strHtml);


            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
		
        function LoadDataGrid_Export() {
            var _urlPath = "";
            if ((parseInt(ddlOPer_ID.value) == 0) && (parseInt(ddlTypeFlight.value) == 0))
                _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_KHUNGTIME";
            else if ((parseInt(ddlOPer_ID.value) == 1) && (parseInt(ddlTypeFlight.value) == 0))
                _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_HANG_QNOI";
            else if ((parseInt(ddlOPer_ID.value) == 2) && (parseInt(ddlTypeFlight.value) == 0))
                _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_HANG_QTE";
            else if ((parseInt(ddlOPer_ID.value) == 0) && (parseInt(ddlTypeFlight.value) == 1))
                _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_BAY_QNOI";
            else if ((parseInt(ddlOPer_ID.value) == 0) && (parseInt(ddlTypeFlight.value) == 2))
                _urlPath = "api/FinishedFlights/GET_FINISHED_FLIGHTS_BAY_QTE";
            else if ((parseInt(ddlOPer_ID.value) == 1) && (parseInt(ddlTypeFlight.value) == 1))
                _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQN_BAY_QNOI";
            else if ((parseInt(ddlOPer_ID.value) == 1) && (parseInt(ddlTypeFlight.value) == 2))
                _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQN_BAY_QTE";
            else if ((parseInt(ddlOPer_ID.value) == 2) && (parseInt(ddlTypeFlight.value) == 1))
                _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQT_BAY_QNOI";
            else if ((parseInt(ddlOPer_ID.value) == 2) && (parseInt(ddlTypeFlight.value) == 2))
                _urlPath = "api/FinishedFlights/GET_FIN_FLIGHTS_HQT_BAY_QTE";

            var $request = $.ajax({
                method: "PUT",                
                url: urlApi + _urlPath,
                data: GetObjectSearchExport(),
                beforeSend: function () {
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

                var strHtml = "<table id='tblSourceExport' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        + "<th>No</th>"
                        + "<th>OPER</th>"                        
                        + "<th>CALLSIGN</th>"
                        + "<th>REGIS</th>"                       
                        + "<th>R_CRAFT</th>"
                        + "<th>F_CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"                        
                        + "<th>ATD</th>"
                        + "<th>ATA</th>"                       
                        + "<th>VIA</th>"
                        + "<th>FPL_VIA</th>"
                        + "<th>REMARK</th>"
                        + "<th>LASTUSER</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "</tr>"
                        + "</thead>"
                        + strAppend
                        + "<tbody>"
                        + "</tbody>"
                        + "</table>";


                generate_excel(strHtml);
                

            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

        function myOrder() {
            
            var x = document.getElementById("tblSource").rows.length;            
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);
            var stt = parseInt(((idx - 1) * pz) + 1);
            var i;
            for (i = 2; i < x; i++)
            {
                var x2 = document.getElementById("tblSource").rows[i].cells;
                x2[0].innerHTML = stt;
                stt++;
            }            
        }

        

    </script>


</asp:Content>
