<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="DayFlightSearch.aspx.cs" Inherits="prjApplication.Day_Flights.DayFlightSearch" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--style for day flight--%>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <style type="text/css">
        #dhtmltooltip {
            position: absolute;
            width: 150px;
            border: 2px solid black;
            padding: 2px;
            background-color: lightyellow;
            visibility: hidden;
            z-index: 100;
            /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
            filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
        }
    </style>
    <style>
        #infoTotal {
            position: absolute;
            width: 250px;
            font-size: 11px;
            top: -15px;
        }

        #topBar1 {
            position: absolute;
            left: 250px;
            width: 1000px;
            text-align: center;
        }

            #topBar1 span {
                background-color: blue;
            }

        #topBar2 {
            position: absolute;
            left: 250px;
            text-align: center;
            width: 1000px;
            top: 30px;
            font-size: 30px;
        }

            #topBar2 span {
                border: solid black 1px;
                width: 70px;
            }

        #topBar3 {
            position: absolute;
            bottom: 5px;
            right: 20px;
        }

        #lblTimeRefresh {
            position: absolute;
            right: 160px;
            bottom: 0px;
            width: 120px;
        }

        #grdSource span {
            border: 1px solid blue;
            border-radius: 4px;
            color: forestgreen;
            padding: 2px 3px;
        }

            #grdSource span:hover {
                cursor: pointer;
            }

        #grdSource > tr.active {
            background-color: red;
        }

        #grdSource input:focus {
            width: 180px !important;
        }

        #divStatusIcon {
            font-size: 13px;
            width: 60px;
        }

        #grdSource .tdIconStatus {
            background-color: ghostwhite;
            color: chocolate;
        }

        #grdSource .iconSearch {
            cursor: pointer;
        }

        #grdSource thead tr td {
            text-transform: uppercase;
            font-weight: 700;
            color: black;
        }

        #ddlPERMTYPE {
            position: absolute;
            right: 520px;
            height: 25px;
            width: 90px;
            font-size: 12px;
            bottom: 0px;
        }

        #grdSource tr td:first-child + td + td {
            white-space: nowrap;
        }

        .divHeader {
            position: relative;
            border-bottom: dotted 1px black;
            height: 115px;
            margin-bottom: 8px;
        }

        .btn.active {
            background-color: red !important;
        }

        .table > thead > tr.Spec {
            background-position: 0% 0%;
            color: #707070;
            font-weight: 400;
            background-image: unset;
            background-color: unset;
            background-repeat: repeat-x;
            background-attachment: scroll;
        }

        .table-bordered > thead > tr > td.headSpec {
            border: unset;
        }

        .table-bordered {
            border: unset;
        }

        .tableTip {
            font-size: 12px;
            background-color: aquamarine;
            text-align: left;
        }
        /*.dControl{font-size: 12px;
            width: 50px;
            height: 20px;}*/
        .sControl {
            font-size: 12px;
            width: 50px;
            height: 20px;
        }
    </style>
    <style>
        .CssTrDefault {
        }

        .CssTdDefault {
        }

        .CssTrDo {
            background-color: beige;
        }

        .CssTrVang {
            background-color: yellowgreen;
        }

        .CssTrXanh {
        }

        .CssTrCam {
            background-color: brown;
        }

        .CssTrTim {
            background-color: blueviolet;
        }

        .CssTrLuc {
            background-color: aliceblue;
        }

        .CssTrHong {
            background-color: crimson;
        }

        .CssTdDo {
            background-color: red;
        }

        .CssTdVang {
            background-color: yellow;
        }

        .CssTdXanh {
            background-color: forestgreen;
        }

        .CssTdCam {
            background-color: deeppink;
        }

        .CssTdTim {
            background-color: darkmagenta;
        }

        .CssTdLuc {
            background-color: chartreuse;
        }

        .CssTdHong {
            background-color: magenta;
        }

        .CssTextDo {
            background-color: red;
        }

        .CssTextVang {
            background-color: yellow;
        }

        .CssTextXanh {
            background-color: darkblue;
        }

        .CssTextCam {
            background-color: coral;
        }

        .CssTextTim {
            background-color: brown;
        }

        .CssTextLuc {
            background-color: cornflowerblue;
        }

        .CssTextHong {
            background-color: hotpink;
        }
    </style>
    <style>
        .icon-quay {
            -webkit-animation: spin 1000ms infinite linear;
            animation: spin 1000ms infinite linear;
        }

        @-webkit-keyframes spin {
            20% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            40% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            60% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            80% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            100% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        }

        @keyframes spin {
            20% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            40% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            60% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            80% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            100% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        }
    </style>
    <div class="row">
        <div class="text-center">
            Form date:
            <input id="txtStartDate" class="date-picker" data-date-format="dd/mm/yyyy" />
            To date:
            <input id="txtFinishDate" class="date-picker" data-date-format="dd/mm/yyyy" />
        </div>
        <div class="table-responsive">
            <table id="grdSource" class="table table-bordered">
                <caption class="text-left">List flight</caption>
                <thead>
                    <tr class="Spec">
                        <td class="headSpec">
                            <div class="action-buttons wid_45px">
                                <a data-toggle="tooltip" id="btnSearch" onclick="btnSearch_Onclick()" title="Search">
                                    <i class="glyphicon glyphicon-search"></i>
                                </a>
                                <a id="btnClearInput" onclick="btnClearInput_Onclick();" data-toggle="tooltip" title="Clear value search">
                                    <i class="glyphicon glyphicon-trash"></i>
                                </a>
                            </div>
                        </td>
                        <td class="headSpec">
                            <select id="ddlStatus" class="sControl">
                                <option value="0">--</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                            </select>
                        </td>
                        <td class="headSpec">
                            <input id="txtPERMNBR" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtFLIGHTNBR" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <select id="ddlFROM_AIRP" class="sControl">
                                <option value="">--</option>
                                <%foreach (var item in new AeroDAL().GetListAll())
                                    {%>
                                <option value="<%: item.AE_CODE %>"><%: item.AE_CODE %> - <%: item.AE_NAME %></option>
                                <%} %>
                            </select></td>
                        <td class="headSpec">
                            <select id="ddlTO_AIRP" class="sControl">
                                <option value="">--</option>
                                <%foreach (var item in new AeroDAL().GetListAll())
                                    {%>
                                <option value="<%: item.AE_CODE %>"><%: item.AE_CODE %> - <%: item.AE_NAME %></option>
                                <%} %>
                            </select></td>
                        <td class="headSpec"></td>
                        <td class="headSpec"></td>
                        <td class="headSpec">
                            <input id="txtPtd" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtAtd" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtATA" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtROUTE" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtROUTE_TP" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <select id="ddlCRAFT_T" class="sControl">
                                <option value="">--</option>
                                <% foreach (var item in new CraftTypeDAL().GetAllCraftType())%><% {%>
                                <option value="<%: item.CRAFT_ID %>"><%: item.MA %></option>
                                <% } %>
                            </select>
                        </td>
                        <td class="headSpec">
                            <select id="ddlCRAFT_TP" class="sControl">
                                <option value="">--</option>
                                <% foreach (var item in new CraftTypeDAL().GetAllCraftType())%><% {%>
                                <option value="<%: item.CRAFT_ID %>"><%: item.MA %></option>
                                <% } %>
                            </select></td>
                        <td class="headSpec">
                            <input type="text" class="sControl" id="txtREGISTRATION" /></td>
                        <td class="headSpec">
                            <input id="txtPURPOSE" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtFLIGHTDATE" type="text" data-date-format="dd/mm/yyyy" class="sControl date-picker" />
                        </td>

                        <td class="headSpec">
                            <input id="txtVIA" type="text" class="sControl" /></td>
                    </tr>
                    <tr>
                        <th></th>
                        <th>Status</th>
                        <th data-sort="1" onclick="sortOnclick(this);">PERM</th>
                        <th data-sort="1" onclick="sortOnclick(this);">FLIGHT</th>
                        <th data-sort="1" onclick="sortOnclick(this);">FROM</th>
                        <th data-sort="1" onclick="sortOnclick(this);">TO</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ETD</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ETA</th>
                        <th data-sort="1" onclick="sortOnclick(this);">PTB</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ATD</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ATA</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ROUTE</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ROUTE_FPL</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CRAFT_T</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CRAFT_TP</th>
                        <th data-sort="1" onclick="sortOnclick(this);">REGIS</th>
                        <th data-sort="1" onclick="sortOnclick(this);">PUPOSE</th>
                        <th data-sort="1" onclick="sortOnclick(this);">FLIGHT DATE</th>
                        <th data-sort="1" onclick="sortOnclick(this);">VIA</th>
                    </tr>
                </thead>
                <tbody>
                    <%= grdSourceLoadFirt() %>
                </tbody>
            </table>
        </div>

    </div>

    <div id="popFlightInfoExtension" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Flight extension</h4>
                </div>
                <div class="modal-body" id="txtContentExtensionInfoFlight">
                    <%--<div class="row">--%>
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="txtChange" class="mLable control-label">Change: </label>
                            <label class="mLable text-info" id="txtChange"></label>
                        </div>
                    </div>
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="txtValidateTime" class="mLable control-label">Time valid: </label>
                            <label class="mLable text-danger" id="txtValidateTime"></label>
                        </div>
                    </div>
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="txtHasPermission" class="mLable control-label">Has permission: </label>
                            <label class="mLable text-primary" style="font-weight: 100" id="txtHasPermission">
                            </label>
                        </div>
                    </div>
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="txtContentChange" class="mLable control-label">Content change: </label>
                            <label class="mLable text-uppercase" id="txtContentChange"></label>
                        </div>
                    </div>
                    <div style="border-top: 1px dotted black;"></div>
                    <%--</div>--%>
                    <div class="row">
                        <div class="table-responsive">
                            <table id="tblDienVan" class="table table-bordered">
                                <caption class="text-left">History Message</caption>
                                <thead>
                                    <tr>
                                        <th>Hour</th>
                                        <%--column 1--%>
                                        <th>Date</th>
                                        <%--column 2--%>
                                        <th>Type</th>
                                        <%--column 3--%>
                                        <th>CallSign</th>
                                        <%--column 4--%>
                                        <th>Registration</th>
                                        <%--column 5--%>
                                        <th>From</th>
                                        <%--column 6--%>
                                        <th>To</th>
                                        <%--column 7--%>
                                        <th>ETD</th>
                                        <%--column 8--%>
                                        <th>ETA</th>
                                        <%--column 9--%>
                                        <th>Via</th>
                                        <%--column 10--%>
                                        <th>Content</th>
                                        <%--column 11--%>
                                        <th>Cnl Type</th>
                                        <%--column 12--%>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnAcceseeInfoChange" class="btn btn-sm btn-primary" style="display: none;">
                            <i class="ace-icon fa fa-times"></i>
                            Access
                        </button>
                        <button id="btnCancel" type="button"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="dhtmltooltip"></div>
    <div id="divExcuteScript"></div>

    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>

    <script>
        $('body').attr('onSubmit', 'return false;');
    </script>
    <script>
        var appenLoad = 20;
        var phanCach = '<%= _phanCach%>';
        var phanCachArg = '<%= _phanCachArg %>';
        var khungGio = [0, 23];
        var loadTopbarInfo = true;
        var loadGrdSource = false;
        var loadTopbar3 = false;
        var isRefresh = false;
        var isSearch = false;
        var isSroll = false;
        var timeRefresh = 10;
        var cd = timeRefresh;
        var sObj = JSON.parse('<%= _ObjSearch%>');
        var btnSearch = document.getElementById('btnSearch');
        var ddlStatus = document.getElementById('ddlStatus');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtFLIGHTNBR = document.getElementById('txtFLIGHTNBR');
        var txtFROM_AIRP = document.getElementById('ddlFROM_AIRP');
        var txtTO_AIRP = document.getElementById('ddlTO_AIRP');
        var txtPtd = document.getElementById('txtPtd');
        var txtAtd = document.getElementById('txtAtd');
        var txtATA = document.getElementById('txtATA');
        var ddlCRAFT_TP = document.getElementById('ddlCRAFT_TP');
        var ddlCRAFT_T = document.getElementById('ddlCRAFT_T');
        var txtREGISTRATION = document.getElementById('txtREGISTRATION');
        var txtPURPOSE = document.getElementById('txtPURPOSE');
        var ddlMTOW = document.getElementById('ddlMTOW');
        var txtFLIGHTDATE = document.getElementById('txtFLIGHTDATE');
        var txtFLIGHT_TYPE = document.getElementById('txtFLIGHT_TYPE');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var txtDATE_OLD = document.getElementById('txtDATE_OLD');
        function btnClearInput_Onclick() {
            $('#grdSource input').val('')
            $('#ddlStatus').prop('selectedIndex', 0);
            $('#ddlCRAFT_TP').prop('selectedIndex', 0);
            $('#ddlCRAFT_T').prop('selectedIndex', 0);
            $('#ddlMTOW').prop('selectedIndex', 0);
            $('#ddlFROM_AIRP').prop('selectedIndex', 0);
            $('#ddlTO_AIRP').prop('selectedIndex', 0);
        }
        function getValueSearchDaylyFlight() {
            var _obj = {
                STATUS: document.getElementById('ddlStatus').value,
                PERMNBR: $('#txtPERMNBR').val(),
                FLIGHTNBR: $('#txtFLIGHTNBR').val(),
                FROM_AIRP: $('#ddlFROM_AIRP').val(),
                TO_AIRP: $('#ddlTO_AIRP').val(),
                PTD: $('#txtPtd').val(),
                ATD: $('#txtAtd').val(),
                ATA: $('#txtATA').val(),
                CRAFT_ID: $('#ddlCRAFT_T').val(),
                CRAFT_TYPE: $('#ddlCRAFT_TP').val(),
                REGISTRATION: $('#txtREGISTRATION').val(),
                PURPOSE: $('#txtPURPOSE').val(),
                FLIGHTDATE: $('#txtFLIGHTDATE').val(),
                PERMTYPE: $('#ddlPERMTYPE').val(),
                STARTDATE: $('#txtStartDate').val(),
                FINISHDATE: $('#txtFinishDate').val(),
                RowStart: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber')) + 1) : 0,
                RowFinish: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber')) + appenLoad) : 20,
            }
            sObj = _obj;
            return _obj;
        }
        function DisplayResult(resulf, context) {
            if (context == 'btnSearch_Onclick') {
                $('#grdSource tbody tr').remove();
                $('#grdSource tbody').append(resulf);
                grdSource_Fixer();
                CustumTooltip('yellow', 500);
            }
            else if (context == 'LoadGrdSourceScroll') {
                $('#grdSource tr').last().after(resulf).fadeIn();
                grdSource_Fixer();
                CustumTooltip('yellow', 500);
            }
            else if (context == 'viewPopupInfoExtension') {
                $('#divExcuteScript').html(resulf);
                $('#popFlightInfoExtension').modal('show');
                CustumTooltip('yellow', 500);
            }
        }
        function btnSearch_Onclick() {
            isSearch = true;
            if (isSearch) {
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg + 'btnSearch_Onclick', 'btnSearch_Onclick');
            }
            isSearch = false;
        }

        function sortTable(iCol) {
            var table, rows, switching, i, x, y, shouldSwitch, s, indd, span;
            table = document.getElementById("grdSource");
            s = $(iCol).attr('data-sort');
            $(iCol).attr('data-sort', s == 'false' ? 'true' : 'false');
            indd = $(iCol.cellIndex)[0];
            span = s == 'true' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>' : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(iCol).append(span);
            rows = table.getElementsByTagName("TR");
            switching = true;
            while (switching) {
                switching = false;
                for (i = 2; i < (rows.length - 1) ; i++) {
                    shouldSwitch = false;
                    x = rows[i].getElementsByTagName("TD")[indd];
                    y = rows[i + 1].getElementsByTagName("TD")[indd];
                    if (s == 'true') {
                        if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                    else {
                        if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                }
                if (shouldSwitch) {
                    rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                    switching = true;
                }
            }
        }
    </script>

    <%--script test--%>
    <script>
        $(window).scroll(function () { //detact scroll
            if ($(window).scrollTop() + $(window).height() >= $(document).height()) { //scrolled to bottom of the page
                isSroll = true;
                LoadGrdSourceScroll();
                isSroll = false;
            }
        });
        function LoadGrdSourceScroll() {
            if (isSroll)
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg + 'LoadGrdSourceScroll', 'LoadGrdSourceScroll');
        }
        function sortTable(f, n) {
            var rows = $('#grdSource tbody  tr').get();
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
                var v = $(elm).children('td').eq(n).text().toUpperCase();
                if ($.isNumeric(v)) {
                    v = parseInt(v, 10);
                }
                return v;
            }

            $.each(rows, function (index, row) {
                $('#grdSource').children('tbody').append(row);
            });
        }
        function sortOnclick(ele) {

            var s = parseInt($(ele).attr('data-sort'));
            s *= -1;
            $(ele).attr('data-sort', s);
            var span = s == '1' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>' : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(ele).append(span);

            var n = $(ele).prevAll().length;
            sortTable(s, n);
        }
    </script>
    <script>
        function viewPopupInfoExtension(ele) {
            var ax = $(ele);
            $('#grdSource tr').removeClass('active');
            ax.addClass('active');
            GetArgWithPostBack(ax.attr('data-id') + phanCach + ax.attr('data-timeM') + phanCach + ax.attr('data-CallSign') + phanCachArg + 'viewPopupInfoExtension', 'viewPopupInfoExtension');
        }
    </script>
    <script>
        function grdSource_Fixer() {
            // $("#grdSource").tableHeadFixer({ 'left': 4 });
        }
        function CustumTooltip(color, wid) {
            $('[data-custip="true"]').each(function () {
                var ele = $(this);
                var txt = $(this).val();
                var htm = $(this).html();
                var txt1 = $(this).text();
                if (txt1.length > 60) {
                    $(this).text(txt1.substring(0, 60) + '.....');
                    $(this).attr('onmouseout', 'hideddrivetip();');
                    $(this).attr('onmouseover', 'ddrivetip(\'' + htm.replace(/\n/g, "<br />").replace(/'/g, '\'') + '\', \''+color+'\', '+wid+')');
                }
            });
        }
        CustumTooltip('yellow', 500);
        
    </script>
</asp:Content>

