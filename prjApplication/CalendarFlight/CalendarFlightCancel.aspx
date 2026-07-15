<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="CalendarFlightCancel.aspx.cs" Inherits="prjApplication.CalendarFlight.CalendarFlightCancel" %>
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
    <div id="abcxyz" class="well well-sm" style="text-align: center;display:none;">

        <%--<asp:LinkButton runat="server" ID="btnExportExel" OnClick=" " CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
        </asp:LinkButton>--%>


        <input id="txtFromDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" style="display:none;"/>
        <button type="button" class="btn btn-sm btn-primary btn-bold" style="display:none;" id="btnAccess" onclick="btnAccess_OnClick()">
            Accept</button>
        <select id="ddlSelect" class="disabled" style="width: 100px;display:none;">
            <option value="1">All</option>
            <option value="2">Ariport</option>
        </select>
        <button id="btnRenderKhb" class="btn btn-sm btn-primary btn-bold" style="display:none;" type="button" onclick="btnRenderKhb_OnClick()">
            Export</button>



        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <table id="tblSource" class="table table-bordered">
        <caption>
            <label class="radio-inline">
                <input id="chkKhb" checked onchange="chkKhb_CheckedChanged();" type="radio" name="optradio">KHB</label>
            <label class="radio-inline">
                <input id="chkKhbTrungLap" onchange="chkKhbTrungLap_CheckedChanged();" type="radio"
                    name="optradio">KHB
                trùng lặp</label>
            <label class="radio-inline">
                <input id="chkKhbDelete" onchange="chkKhbDelete_CheckedChanged();" type="radio" name="optradio">KHB
                delete</label>
            <label class="radio-inline">
                <input id="chkKhbOrigin" onchange="chkKhbOrigin_CheckedChanged();" type="radio" name="optradio">KHB
                origin</label>
            <label class="radio-inline">
                <input id="chkKhbNotInScheduleMonth" onchange="chkKhbDelete_CheckedChanged();" type="radio"
                    name="optradio">KHB
                Vênh theo mùa</label>
            <label class="radio-inline">
                <input id="chkKhbNotInSchedule" onchange="chkKhbDelete_CheckedChanged();" type="radio"
                    name="optradio">KHB
                Vênh theo tháng</label>

            <button type="button" id="btnUpdateList" class="btn btn-sm btn-primary" style="display:none;" onclick="btnUpdateList_Onclick()">
                Update all</button>
            <button type="button" id="btnSearch" class="btn btn-sm btn-primary" onclick="btnSearch_OnClick()">
                Search</button>
            <button type="button" id="btnDeleteByChecked" class="btn btn-sm btn-primary" style="display:none;" onclick="btnDeleteByChecked_Onclick()">
                Delete</button>
            <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" onclick="btnClearSearch_OnClick()">
                Clear search</button>
        </caption>
        <thead>
            <tr style="background-color: unset">
                <th></th>
                <th></th>
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
                <th style="display: none">
                    <input id="txtDATE_OLD" class="wid_85px" disabled type="text" /></th>
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
                    <input id="txtCRAFT_ID" data-autocomplete="CRAFT" class="wid_50px" type="text" />
                </th>
                <%--<th>
                    <input id="txtMtow" class="wid_100" type="text" /></th>--%>
                <th>
                    <input id="txtCRAFT_TYPE" data-autocomplete="CRAFT" class="wid_50px" type="text" />
                </th>
                <th>
                    <input id="txtPURPOSE" data-autocomplete="PURPOSE" class="wid_50px" type="text" />
                </th>
                <th>
                    <input id="txtVALIDHOURS" data-number="true" class="wid_100" type="text" /></th>
                <th>
                    <input id="txtPERMNBR" class="wid_160px" type="text" /></th>
                <th>
                    <input id="txtVIA" class="wid_200px" type="text" /></th>
                <th>
                    <input id="txtREMARK" class="wid_200px" type="text" /></th>
                <th></th>
            </tr>
            <tr style="color: white">
                <th>No</th>
                <th>
                    <input type="checkbox" id="chkAll" onchange="chkAll_Onchange()" /></th>
                <th data-sort="1" onclick="sortOnclick(this);">CallSign</th>
                <th data-sort="1" onclick="sortOnclick(this);">Regis</th>
                <th data-sort="1" onclick="sortOnclick(this);">From</th>
                <th data-sort="1" onclick="sortOnclick(this);">To</th>
                <th data-sort="1" onclick="sortOnclick(this);">Etd</th>
                <th data-sort="1" onclick="sortOnclick(this);">Eta</th>
                <th data-sort="1" onclick="sortOnclick(this);">Perm date</th>
                <%--<th>Flight date</th>--%>
                <%--<th>Atd</th>--%>
                <%--<th>Ata</th>--%>
                <th data-sort="1" onclick="sortOnclick(this);">Perm type</th>
                <th data-sort="1" onclick="sortOnclick(this);">Flight type</th>
                <th data-sort="1" onclick="sortOnclick(this);">Oper</th>
                <th data-sort="1" onclick="sortOnclick(this);">Perm craft</th>
                <%--<th>Mtow</th>--%>
                <th data-sort="1" onclick="sortOnclick(this);">Real craft</th>
                <th data-sort="1" onclick="sortOnclick(this);">Pur</th>
                <th data-sort="1" onclick="sortOnclick(this);">Valid hour</th>
                <th data-sort="1" onclick="sortOnclick(this);">Number</th>
                <th data-sort="1" onclick="sortOnclick(this);">Via</th>
                <th data-sort="1" onclick="sortOnclick(this);">Remark</th>
                <th></th>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>
    <script>
        function RenderTableKhbVenh(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt($('#tblSource').attr('data-pagesize'));
            var khb_delete = $('#chkKhbDelete').prop('checked');
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr>"
                    + "<td>" + stt + "</td>"
                    + "<td></td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETD) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.ETA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.FLIGHT_TYPE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.CRAFT_NAME) + "</td>"
                    + "<td></td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PURPOSE) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VALIDHOURS) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.PERMNBR) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.VIA) + "</td>"
                    + "<td style=\'text-align: left;\'>" + returnEmpty(b.REMARK) + "</td>"
                    + "</tr>";
                stt++;
            });
            return kq;
        }
    </script>
    <script>
        var isTarget = true;
        var qEdit = '<%= _Role.R_Edit %>';
        var qDel = '<%= _Role.R_Del %>';
        var isSearch = false;
        var rowId = 0;
        var pageSize = 100;
        $('#tblSource').paging({ pageSize: pageSize });
        function Render2Table(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt($('#tblSource').attr('data-pagesize'));
            var khb_delete = $('#chkKhbDelete').prop('checked');
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
                kq += "<tr id='" + b.FLIGHT_ID + "' data-isUpdate='false' onmouseover='rowId=" + b.FLIGHT_ID + ";' onmouseout='rowId=0;'>"
                    + "<td style=\"width:30px\" id='b_" + b.RNUM + "'><input style='width: 30px!important; padding: 0px!important;' type='text' id='txtStt" + stt + "' value='" + stt + "'/></td>"
                    + "<td><input type=\"checkbox\" id='chk_" + b.FLIGHT_ID + "' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='8' class='sInput' id='txtFLIGHTNBR" + a + "' data-oldValue='" + returnEmpty(b.FLIGHTNBR) + "' value='" + returnEmpty(b.FLIGHTNBR) + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREGISTRATION" + a + "' data-oldValue='" + returnEmpty(b.REGISTRATION) + "' value='" + returnEmpty(b.REGISTRATION) + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtFROM_AIRP" + a + "' data-oldValue='" + b.FROM_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.FROM_AIRP + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='2' class='sInput' id='txtTO_AIRP" + a + "' data-oldValue='" + b.TO_AIRP + "' onfocusin='binAutocomplete(this,\"AERO\")' value='" + b.TO_AIRP + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='4' maxlength='6' class='sInput' id='txtETD" + a + "' data-oldValue='" + returnEmpty(b.ETD) + "' data-number='true' value='" + returnEmpty(b.ETD) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtETA" + a + "' data-oldValue='" + returnEmpty(b.ETA) + "' data-number='true' value='" + returnEmpty(b.ETA) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' maxlength=\"10\" class='sInput' id='txtFLIGHTDATE" + a + "' data-oldValue='" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "' value='" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "' onblur='checkIsUpdate(this)' /></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtPERMTYPE" + a + "' onfocusin='binAutocomplete(this,\"PERMTYPE\")' data-oldValue='" + returnEmpty(b.PERMTYPE) + "' value='" + returnEmpty(b.PERMTYPE) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtFLIGHT_TYPE" + a + "' data-oldValue='" + returnEmpty(b.FLIGHT_TYPE) + "' onfocusin='binAutocomplete(this,\"FLIGHTTYPE\")' value='" + returnEmpty(b.FLIGHT_TYPE) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtOPER_ID" + a + "' data-oldValue='" + returnEmpty(b.OPER_ID) + "' value='" + returnEmpty(b.OPER_ID) + "' onfocusin='binAutocomplete(this,\"OPER\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtPERMCRAFT" + a + "' data-craftid='" + returnEmpty(b.CRAFT_ID) + "' data-oldValue='" + returnEmpty(b.CRAFT_NAME) + "' value='" + returnEmpty(b.CRAFT_NAME) + "' onfocusin='binAutocomplete(this,\"CRAFT\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtREALCRAFT" + a + "' data-craftid='" + returnEmpty(b.REALCRAFT) + "' data-oldValue='" + returnEmpty(b.REALCRAFT) + "' value='" + returnEmpty(b.REALCRAFT) + "' onfocusin='binAutocomplete(this,\"CRAFT\")' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPURPOSE" + a + "' data-oldValue='" + b.PURPOSE + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' value='" + b.PURPOSE + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtVALIDHOURS" + a + "' data-oldValue='" + b.VALIDHOURS + "' value='" + b.VALIDHOURS + "' data-number=\"true\" maxlength=\"2\" onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' data-minlenght='1' class='sInput' id='txtPERMNBR" + a + "' data-oldValue='" + b.PERMNBR + "' value='" + b.PERMNBR + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtVIA" + a + "' data-oldValue='" + returnEmpty(b.VIA).replace(/\n/gi, '') + "' value='" + returnEmpty(b.VIA) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "' data-oldValue='" + returnEmpty(b.REMARK) + "' value='" + returnEmpty(b.REMARK) + "' onblur='checkIsUpdate(this)'/></td>"
                    + "<td style=\"white-space: nowrap;display:none;>\""
                                + "<div class=\"action-buttons\">"
                                + "<i id=\"btnDeleteRemark" + b.FLIGHT_ID + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"btnDeleteRemark_Onclick(this);\"></i></a>"
                                + "</td>"
                    //+ "<td><input type='text' data-control='_updateAll' class='sInput' id='txtREMARK" + a + "' data-oldValue='" + returnEmpty(b.REMARK) + "' value='" + returnEmpty(b.REMARK) + "' onblur='checkIsUpdate(this)'/></td>"
                    //+ "<td style=\"white-space: nowrap;>\""
                    //            + "<div class=\"action-buttons\">"
                    //            + (khb_delete ? "<a data-toggle=\"tooltip\" title=\"Restore\"><i id=\"btnRestore_" + b.FLIGHT_ID + "\" class=\"glyphicon glyphicon-refresh bigger-130\" onclick=\"RestoreDelete(" + b.FLIGHT_ID + ", " + b.NOVERSION + ");\"></i></a>" : (qDel == "True" ? "<a data-toggle=\"tooltip\" title=\"Delete\">"
                    //            + "<i id=\"btnDelete_" + b.FLIGHT_ID + "\" class=\"ace-icon fa fa-trash-o bigger-130\" onclick=\"btnDeleteOnclick(" + b.FLIGHT_ID + ");\"></i></a>" : ""))
                    //            + "</td>"
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
        function btnDeleteRemark_Onclick(el) {
            var isDel = false;
            var $ele = $(el);
            var $remark = $(el).closest('tr').find('[id^=txtREMARK]');
            var isDel = $ele.hasClass('fa-trash-o');
            if(isDel){
                $remark.val('');
            }
            if ($remark.attr('data-oldvalue') != '') {

            }
        }
        function returnEmpty(val) {
            return val == null ? "" : val;
        }
        function LoadDataGrid() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/KeHoachBay/GetKeHoachBay",
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
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
                var strAppend = Render2Table(data);
                $('#tblSource tbody').append(strAppend);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        
        function LoadDataGrid_Khb_Venh_Mua() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/KeHoachBay/KeHoachBayVenhTheoLichMua",
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
                        onClickButton: 'LoadDataGrid_Khb_Venh_Mua',
                        pageSize: pageSize,
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
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
                var strAppend = RenderTableKhbVenh(data);
                $('#tblSource tbody').append(strAppend);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function LoadDataGrid_Khb_Venh_Thang() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/KeHoachBay/KeHoachBayVenhTheoLichThang",
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
                        onClickButton: 'LoadDataGrid_Khb_Venh_Thang',
                        pageSize: pageSize,
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
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
                var strAppend = RenderTableKhbVenh(data);
                $('#tblSource tbody').append(strAppend);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function LoadDataGrid_Origin() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ScheduleFlights/GetBySearch/",
                data: GetObjectSearch(),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loaddingData');
                    $('#tblSource').paging({
                        onClickButton: 'LoadDataGrid_Origin',
                        pageSize: pageSize,
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
                var strAppend = RenderTableKhbVenh(data);
                $('#tblSource tbody').append(strAppend);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function LoadDataGrid_Delete() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/KeHoachBay/KeHoachBayDelete",
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
                        onClickButton: 'LoadDataGrid_Delete',
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
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
                var strAppend = Render2Table(data);
                $('#tblSource tbody').append(strAppend);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function LoadDataGrid_TrungLap() {
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/KeHoachBay/GetKeHoachBayTrungLap",
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
                        onClickButton: 'LoadDataGrid_TrungLap',
                        pageSize: pageSize,
                    });
                    $('#tblSource input[data-control="_updateAll"]').each(function (a, b) {
                        if ($(b).prop('id').indexOf('txtPERMDATE') == -1 && $(b).prop('id').indexOf('txtFLIGHTDATE') == -1)
                            $(b).ValidateTip();
                        else $(b).multiDate();
                    })
                    khb_TrungLap_SetColor();
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
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
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
            _obj['FLIGHTDATE'] = $('#txtFLIGHTDATE').val() == '' ? new Date().format('yyyy-mm-dd') : $('#txtFLIGHTDATE').val().replace(/\//gi,'-').replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
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
            _obj['CRAFT_ID'] = $('#txtCRAFT_ID').attr('data-craftid');
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['PERMTYPE'] = $('#txtPERMTYPE').val();
            _obj['VIA'] = $('#txtVIA').val();
            _obj['REMARK'] = $('#txtREMARK').val();
            _obj['ETA'] = $('#txtETA').val();
            _obj['ETD'] = $('#txtETD').val();
            _obj['ATA'] = $('#txtATA').val();
            _obj['ATD'] = $('#txtATD').val();
            _obj['ISACCESS'] = 2;
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
                case "PERMTYPE":
                    $(id).on('blur', function (e, datum) {
                        CheckPermType($(id));
                    });
                    break;
                case "FLIGHTTYPE":
                    $(id).on('blur', function (e, datum) {
                        CheckFlightType($(id));
                    });
                    break;
                case "CRAFT":
                    $(id).on('blur', function (e, datum) {
                        CheckCraft($(id));
                    });
                    break;
                case "AERO":
                    $(id).on('blur', function (e, datum) {
                        CheckListAero($(id));
                    });
                    break;
                case "PURPOSE":
                    $(id).on('blur', function (e, datum) {
                        CheckPurpose($(id));
                    });
                    break;
                case "OPER":
                    $(id).on('blur', function (e, datum) {
                        CheckListOper($(id));
                    });
                    break;
            }
            $(id).focus();
        }
    </script>

    <script>
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
                _obj['VALIDHOURS'] = $($(b).find('[id^="txtVALIDHOURS"]')[0]).val();
                _obj['FLIGHT_TYPE'] = $($(b).find('[id^="txtFLIGHT_TYPE"]')[0]).val();
                _obj['CRAFT_ID'] = $($(b).find('[id^="txtPERMCRAFT"]')[0]).attr('data-craftid');
                _obj['CRAFT_TYPE'] = $($(b).find('[id^="txtREALCRAFT"]')[0]).attr('data-craftid');
                _obj['ATA'] = $($(b).find('[id^="txtATA"]')[0]).val();
                _obj['VIA'] = $($(b).find('[id^="txtVIA"]')[0]).val();
                _obj['OPER_ID'] = $($(b).find('[id^="txtOPER_ID"]')[0]).val();
                _obj['ATD'] = $($(b).find('[id^="txtATD"]')[0]).val();
                _obj['ETA'] = $($(b).find('[id^="txtETA"]')[0]).val();
                _obj['ETD'] = $($(b).find('[id^="txtETD"]')[0]).val();
                _obj['PERMNBR'] = $($(b).find('[id^="txtPERMNBR"]')[0]).val();
                _obj['FROM_AIRP'] = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val();
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: urlApi + "api/DayFlights/Update",
                    data: _obj,
                }).always(function (data) {
                    if (data.Code != -1) c++;
                });
            });
            alert('Update sussess: ' + c + '/' + $lis.length);
            LoadDataGrid();
        }

        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result) {
                var $request = $.ajax({
                    async: false,
                    method: "DELETE",
                    url: urlApi + "api/DayFlights/Delete/" + id,
                }).always(function (data) {
                    if (data.Code != -1) {
                        $('#' + id).remove();
                        alert('Delete sussess!');
                        //LoadDataGrid();
                    } else alert('Delete error!');
                });
            }
        }
        function btnSearch_OnClick() {
            isSearch = true;
            Load_Data_Search();
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
            $('#txtCRAFT_ID').val('');
            $('#txtCRAFT_TYPE').val('');
            $('#txtPURPOSE').val('');
            $('#txtVALIDHOURS').val('');
            $('#txtPERMNBR').val('');
            $('#txtVIA').val('');
            $('#txtREMARK').val('');
            btnSearch_OnClick();
        }
        function btnAccess_OnClick() {
            var c = checkValidCustomMinlenght('checkAccess');
            if (!c) return;
            var cf = confirm('Do you want Access date: ' + $('#txtFromDate').val() + '?');
            if (cf) {
                var $request = $.ajax({
                    async: true,
                    method: "GET",
                    url: urlApi + "api/KeHoachBay/AccessKeHoachBayNgay?user=" + '<%= _user.UserName%>' + "&date=" + new Date($('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd'),
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
        function btnRenderKhb_OnClick() {
            var cf = confirm('Do you want render Message Date:' + $('#txtFromDate').val() + '?');
            if (!cf) return;
            switch ($('#ddlSelect').val()) {
                case "1":
                    var $request = $.ajax({
                        async: true,
                        method: "PUT",
                        url: urlApi + "api/KeHoachBay/KeHoachBayToMessage?date=" + $('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1'),
                        complete: function () {
                            unLoadingData('loadingImgA');
                        },
                        beforeSend: function () {
                            preloadImgAfterButton('btnRenderKhb', 'loadingImgA');
                        }
                    }).always(function (data) {
                        alert(data.Code < 0 ? 'Error!' : 'Sussess!');
                    });
                    break;
                case "2":
                    var p = {};
                    p['DATE_FLY'] = new Date($('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
                    var $request = $.ajax({
                        async: true,
                        method: "PUT",
                        url: urlApi + "api/KeHoachBay/KeHoachBayToMessageByAirport?date=" + $('#txtFromDate').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1'),
                        complete: function () {
                            unLoadingData('loadingImgA');
                        },
                        beforeSend: function () {
                            preloadImgAfterButton('btnRenderKhb', 'loadingImgA');
                        }
                    }).always(function (data) {
                        alert(data.Code < 0 ? 'Error!' : 'Sussess!');
                    });
                    break;
            }
        }
        function RestoreDelete(id, vs) {
            var url = urlApi + "api/DayFlights/RestoreHis/" + id + "?version=" + vs + "&iduser=" + '<%= _user.UserID%>';
                var $request = $.ajax({
                    async: false,
                    method: "GET",
                    url: urlApi + "api/DayFlights/RestoreHis/" + id + "?version=" + vs + "&iduser=" + '<%= _user.UserID%>',
            }).always(function (data) {
                alert(data.Code < 0 ? 'Error!' : 'Sussess!');
            });
            LoadDataGrid_Delete();
        }
        function chkKhbTrungLap_CheckedChanged() {
            $('#tblSource').attr('data-pageIndex', 1);
            Load_Data_Search();
        }
        function chkKhb_CheckedChanged() {
            $('#tblSource').attr('data-pageIndex', 1);
            Load_Data_Search();
        }
        function chkKhbOrigin_CheckedChanged() {
            $('#tblSource').attr('data-pageIndex', 1);
            Load_Data_Search();
        }
        function chkKhbDelete_CheckedChanged() {
            $('#tblSource').attr('data-pageIndex', 1);
            Load_Data_Search();
        }
        function Load_Data_Search() {
            var khb = $('#chkKhb').prop('checked'),
                khb_trung = $('#chkKhbTrungLap').prop('checked'),
                khb_delete = $('#chkKhbDelete').prop('checked'),
                khb_VenhLech = $('#chkKhbNotInScheduleMonth').prop('checked'),
                khb_VenhTheoThang = $('#chkKhbNotInSchedule').prop('checked'),
                khb_Origin = $('#chkKhbOrigin').prop('checked');
            if (khb) { $('#btnUpdateList').removeAttr('disabled'); $('#btnDeleteByChecked').removeAttr('disabled'); LoadDataGrid(); }
            if (khb_trung) { $('#btnUpdateList').removeAttr('disabled'); $('#btnDeleteByChecked').removeAttr('disabled'); LoadDataGrid_TrungLap(); }
            if (khb_delete) { $('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); LoadDataGrid_Delete(); }
            if (khb_VenhLech) { $('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); LoadDataGrid_Khb_Venh_Mua(); }
            if (khb_VenhTheoThang) { $('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); LoadDataGrid_Khb_Venh_Thang(); }
            if (khb_Origin) { $('#btnUpdateList').attr('disabled', 'disabled'); $('#btnDeleteByChecked').attr('disabled', 'disabled'); LoadDataGrid_Origin(); }
        }
    </script>

    <script>
        function khb_TrungLap_SetColor() {
            var isTrungLap = $('#chkKhbTrungLap').prop('checked'),
                _cs = '', _f = '', _t = '', _c = '', _h = '', ic = false;
            if (isTrungLap) {
                if ($('#tblSource tbody tr').length < 1) return;
                var xx = $('#tblSource tbody tr').eq(0);
                _cs = $($(xx).find('[id^="txtFLIGHTNBR"]')[0]).val();
                _f = $($(xx).find('[id^="txtFROM_AIRP"]')[0]).val();
                _t = $($(xx).find('[id^="txtTO_AIRP"]')[0]).val();
                _c = $($(xx).find('[id^="txtETD"]')[0]).val();
                _h = $($(xx).find('[id^="txtETA"]')[0]).val();

                $('tblSource tbody tr').each(function (a, b) {
                    if (a == 0) { }
                    else {
                        var _rcs = $($(b).find('[id^="txtFLIGHTNBR"]')[0]).val(),
                            _rf = $($(b).find('[id^="txtFROM_AIRP"]')[0]).val(),
                            _rt = $($(b).find('[id^="txtTO_AIRP"]')[0]).val(),
                            _rc = $($(b).find('[id^="txtETD"]')[0]).val(),
                            _rh = $($(b).find('[id^="txtETA"]')[0]).val();
                        var c = comp(_cs, _f, _t, _c, _h, _rcs, _rf, _rt, _rc, _rh);
                        if (!c) {
                            _cs = _rcs; _f = _rf; _t = _rt; _c = _rc; _h = _rh;
                        }
                        else {
                            $('#tblSource tbody tr').eq(a).addClass('cssTrung');
                            $('#tblSource tbody tr').eq((a - 1)).addClass('cssTrung');
                        }
                    }
                })
            }
        }
        function comp(a1, b1, c1, d1, e1, a2, b2, c2, d2, e2, idx) {
            if (a1 != a2) return false;
            if (b1 != b2) return false;
            if (c1 != c2) return false;
            if (d1 != d2) return false;
            if (e1 != e2) return false;
            return true;
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
            if (rowId == 0 && charCode == 46) {
                //alert('Please select row delete!');
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
            if (charCode == 46) {
                if ($(document.activeElement).closest('table').prop('id') != 'tblSource' && $(document.activeElement).closest('tr').index() > 2) {
                    alert('Please select row delete!');
                    return;
                }
                var khb_delete = $('#chkKhbDelete').prop('checked');
                if (khb_delete) {
                    alert('Cant delete!');
                    return;
                }
                var idDelete = $(document.activeElement).closest('tr').prop('id');
                var cf = confirm('Do you want delete STT: ' + $($(document.activeElement).closest('tr').find('td input')[0]).val());
                if (cf) {
                    var $request = $.ajax({
                        async: true,
                        method: "DELETE",
                        url: urlApi + "api/DayFlights/Delete/" + idDelete,
                    }).always(function (data) {
                        if (data.Code != -1) {
                            alert('Delete sussess!');
                            var ci = $(document.activeElement).closest('td').index();
                            $($('#' + idDelete).next().find('td')[ci]).find('input')[0].focus();
                            $('#' + idDelete).next().addClass('select');
                            $('#' + idDelete).remove();
                        } else alert('Delete error!');
                    });
                }
            }
        }

    </script>
    <script>
        LoadDataGrid();
        $('#txtFromDate').val(new Date().format('dd-mm-yyyy'));
        $('#txtFromDate').multiDate();
        $('#txtDATE_OLD').multiDate();
        $('#txtFLIGHTDATE').multiDate();
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
            $('#tblSource tbody td input[type="checkbox"]:checked').each(function (a, b) {
                var idDelete = $(b).prop('id').split('_')[1];
                var $request = $.ajax({
                    async: false,
                    method: "DELETE",
                    url: urlApi + "api/DayFlights/Delete/" + idDelete,
                }).always(function (data) {
                    if (data.Code != -1) {
                    }
                });
            })
            btnSearch_OnClick();
        }
    </script>

    <%--<script>

        window.onkeydown = function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            // len 38 xuong 40
            if (charCode == 38 && isTarget) {
                var iRowSelect = $('#tblSource tbody tr.select').index();
                if (iRowSelect < 0) {
                    $('#tblSource tbody tr').eq(0).addClass('select');
                    return;
                }else{
                    $('#tblSource tbody tr').removeClass('select');
                    $('#tblSource tbody tr').eq(iRowSelect - 1).addClass('select');

                }
            }
            else if (charCode == 40 && isTarget) {
                var iRowSelect = $('#tblSource tbody tr.select').index();
                if (iRowSelect <0) {
                    $('#tblSource tbody tr').eq(0).addClass('select');
                    return;
                } else {
                    $('#tblSource tbody tr').removeClass('select');
                    $('#tblSource tbody tr').eq(iRowSelect+1).addClass('select');
                }
            }
        }
        
        $("#tblSource td").click(function () {
            $('#tblSource tbody tr').removeClass('select');
            $('#tblSource tbody tr').eq(parseInt($(this).parent().index())).addClass('select');
        });
    </script>--%>
</asp:Content>
