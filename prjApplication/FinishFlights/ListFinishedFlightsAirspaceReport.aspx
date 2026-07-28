<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ListFinishedFlightsAirspaceReport.aspx.cs"
    Inherits="prjApplication.FinishFlights.ListFinishedFlightsAirspaceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .airspace-report-toolbar {
            display: flex;
            align-items: center;
            flex-wrap: nowrap;
            gap: 6px;
            overflow-x: auto;
            padding: 10px;
            white-space: nowrap;
        }

        .airspace-report-date {
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }

        .airspace-report-date label {
            margin: 0;
            color: #315a77;
            font-size: 12px;
            font-weight: 700;
        }

        .airspace-report-date input[type=date] {
            width: 130px;
            height: 36px;
            padding: 6px 8px;
            border: 1px solid #8eb9d6;
            border-radius: 6px;
        }

        .airspace-report-hidden-date {
            display: none !important;
        }

        .airspace-report-toolbar input[type=text] {
            width: 48px;
            height: 34px;
            padding: 5px;
            text-align: center;
        }

        .airspace-report-toolbar select {
            height: 34px;
            color: #173b59;
        }

        .airspace-report-toolbar button {
            min-width: 105px;
            text-transform: uppercase;
        }

        .airspace-report-wrap {
            overflow: auto;
        }

        #tblAirspaceReport {
            min-width: 1560px;
            table-layout: fixed;
        }

        #tblAirspaceReport th,
        #tblAirspaceReport td {
            padding: 4px;
            text-align: center;
            vertical-align: middle;
        }

        #tblAirspaceReport thead tr:last-child {
            color: #fff;
            background: #418ed6;
            text-transform: uppercase;
        }

        #tblAirspaceReport thead input {
            width: 100%;
            min-width: 48px;
            height: 30px;
            padding: 3px;
            color: #111;
            text-align: center;
        }
    </style>

    <div class="well well-sm airspace-report-toolbar">
        <span class="airspace-report-date">
            <label for="txtReportFromPicker">FROM</label>
            <input id="txtReportFromPicker" type="date" aria-label="Ngày bắt đầu" />
            <input id="txtReportFrom" type="text" class="airspace-report-hidden-date" />
        </span>
        <input id="txtReportFromTime" type="text" maxlength="4" value="0000" disabled="disabled" />

        <span class="airspace-report-date">
            <label for="txtReportToPicker">TO</label>
            <input id="txtReportToPicker" type="date" aria-label="Ngày kết thúc" />
            <input id="txtReportTo" type="text" class="airspace-report-hidden-date" />
        </span>
        <input id="txtReportToTime" type="text" maxlength="4" value="2359" disabled="disabled" />

        <b>HOUR:</b>
        <select id="ddlReportTime" onchange="reportTimeChanged();">
            <option value="0">-ALL-</option>
            <option value="1">ETD</option>
            <option value="2">ETA</option>
            <option value="3">ATD</option>
            <option value="4">ATA</option>
        </select>

        <select id="ddlReportPageSize" onchange="searchAirspaceReport();">
            <option value="100">100</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
        </select>

        <button type="button" id="btnReportSearch" class="btn btn-sm btn-primary"
            onclick="searchAirspaceReport();">Search</button>
        <button type="button" id="btnReportExport" class="btn btn-sm btn-primary"
            onclick="exportAirspaceReportExcel();">Export Excel</button>
    </div>

    <div class="airspace-report-wrap">
        <table id="tblAirspaceReport" class="table table-bordered"
            data-pageindex="1" data-total="0">
            <thead>
                <tr>
                    <th></th>
                    <th><input id="reportOper" type="text" /></th>
                    <th></th>
                    <th><input id="reportCallsign" type="text" /></th>
                    <th><input id="reportRegis" type="text" /></th>
                    <th><input id="reportRCraft" type="text" /></th>
                    <th><input id="reportFCraft" type="text" /></th>
                    <th><input id="reportPurpose" type="text" /></th>
                    <th><input id="reportPType" type="text" /></th>
                    <th><input id="reportFrom" type="text" /></th>
                    <th><input id="reportTo" type="text" /></th>
                    <th><input id="reportATD" type="text" /></th>
                    <th><input id="reportATA" type="text" /></th>
                    <th><input id="reportVia" type="text" /></th>
                    <th><input id="reportFplVia" type="text" /></th>
                    <th><input id="reportRemark" type="text" /></th>
                    <th><input id="reportETD" type="text" /></th>
                    <th><input id="reportETA" type="text" /></th>
                </tr>
                <tr>
                    <th>NO</th>
                    <th>OPER</th>
                    <th>FLIGHTDATE</th>
                    <th>CALLSIGN</th>
                    <th>REGIS</th>
                    <th>R_CRAFT</th>
                    <th>F_CRAFT</th>
                    <th>PURPOSE</th>
                    <th>P_TYPE</th>
                    <th>FROM</th>
                    <th>TO</th>
                    <th>ATD</th>
                    <th>ATA</th>
                    <th>VIA</th>
                    <th>FPL_VIA</th>
                    <th>REMARK</th>
                    <th>ETD</th>
                    <th>ETA</th>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>

    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        var airspaceReportUrl =
            urlApi + 'api/ApiExtension/ExcuteTable?packageName=AIRSPACE_PKG&storeName=GET_FINISHED_AIRSPACE';

        function reportEncode(value) {
            return $('<div/>').text(value == null ? '' : value).html();
        }

        function reportDisplayDate(value) {
            if (!value) return '';
            var date = new Date(value);
            return isNaN(date.getTime()) ? value : dateFormat(date, 'dd-mm-yyyy');
        }

        function reportIsoToDisplay(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3
                ? parts[2] + '-' + parts[1] + '-' + parts[0]
                : '';
        }

        function reportDisplayToIso(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3
                ? parts[2] + '-' + parts[1] + '-' + parts[0]
                : '';
        }

        function validateReportDates() {
            var fromDate = $('#txtReportFromPicker').val();
            var toDate = $('#txtReportToPicker').val();

            if (!fromDate || !toDate) {
                alert('Vui lòng chọn đầy đủ ngày FROM và TO.');
                return false;
            }
            if (fromDate > toDate) {
                alert('Ngày FROM không được lớn hơn ngày TO.');
                return false;
            }
            return true;
        }

        function getAirspaceReportSearchObject(resetPage) {
            if (resetPage) $('#tblAirspaceReport').attr('data-pageindex', 1);

            var pageIndex = parseInt(
                $('#tblAirspaceReport').attr('data-pageindex'),
                10
            );
            if (isNaN(pageIndex) || pageIndex < 1) pageIndex = 1;

            return {
                P_PAGESIZE: parseInt($('#ddlReportPageSize').val(), 10),
                P_PAGEINDEX: pageIndex - 1,
                P_CALLSIGN: $.trim($('#reportCallsign').val()),
                P_REGIS: $.trim($('#reportRegis').val()),
                P_FROM_AIRP: $.trim($('#reportFrom').val()),
                P_TO_AIRP: $.trim($('#reportTo').val()),
                P_PURPOSE: $.trim($('#reportPurpose').val()),
                P_OPER: $.trim($('#reportOper').val()),
                P_FCRAFT: $.trim($('#reportFCraft').val()),
                P_RCRAFT: $.trim($('#reportRCraft').val()),
                P_P_TYPE: $.trim($('#reportPType').val()),
                P_VIA: $.trim($('#reportVia').val()),
                P_FPLVIA: $.trim($('#reportFplVia').val()),
                P_REMARK: $.trim($('#reportRemark').val()),
                P_ETA: $.trim($('#reportETA').val()),
                P_ETD: $.trim($('#reportETD').val()),
                P_ATA: $.trim($('#reportATA').val()),
                P_ATD: $.trim($('#reportATD').val()),
                P_ISACCEPTED: 1,
                P_STARTDATE: $('#txtReportFrom').val(),
                P_FINISHDATE: $('#txtReportTo').val(),
                P_KHUNGGIO1: $('#txtReportFromTime').val(),
                P_KHUNGGIO2: $('#txtReportToTime').val(),
                P_CAT_HA: parseInt($('#ddlReportTime').val(), 10) || 0
            };
        }

        function renderReportRows(items, startNumber) {
            var html = '';
            var number = startNumber;

            $.each(items || [], function (_, item) {
                html += '<tr>' +
                    '<td>' + number + '</td>' +
                    '<td>' + reportEncode(item.OPER) + '</td>' +
                    '<td>' + reportEncode(reportDisplayDate(item.FLIGHTDATE)) + '</td>' +
                    '<td>' + reportEncode(item.CALLSIGN) + '</td>' +
                    '<td>' + reportEncode(item.REGIS) + '</td>' +
                    '<td>' + reportEncode(item.RCRAFT) + '</td>' +
                    '<td>' + reportEncode(item.FCRAFT) + '</td>' +
                    '<td>' + reportEncode(item.PURPOSE) + '</td>' +
                    '<td>' + reportEncode(item.P_TYPE) + '</td>' +
                    '<td>' + reportEncode(item.FROM_AIRP) + '</td>' +
                    '<td>' + reportEncode(item.TO_AIRP) + '</td>' +
                    '<td>' + reportEncode(item.ATD) + '</td>' +
                    '<td>' + reportEncode(item.ATA) + '</td>' +
                    '<td>' + reportEncode(item.VIA) + '</td>' +
                    '<td>' + reportEncode(item.FPLVIA) + '</td>' +
                    '<td>' + reportEncode(item.REMARK) + '</td>' +
                    '<td>' + reportEncode(item.ETD) + '</td>' +
                    '<td>' + reportEncode(item.ETA) + '</td>' +
                    '</tr>';
                number++;
            });
            return html;
        }

        function LoadAirspaceReportData() {
            if (!validateReportDates()) return;

            var request = getAirspaceReportSearchObject(false);
            $.ajax({
                method: 'PUT',
                url: airspaceReportUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request),
                beforeSend: function () {
                    $('#tblAirspaceReport tbody').empty();
                    $('#btnReportSearch, #btnReportExport').prop('disabled', true);
                }
            }).done(function (data) {
                var rows = data && data.ListValue ? data.ListValue : [];
                var total = rows.length ? parseInt(rows[0].SUMRECORD, 10) : 0;
                var page = parseInt(
                    $('#tblAirspaceReport').attr('data-pageindex'),
                    10
                ) || 1;
                var size = parseInt($('#ddlReportPageSize').val(), 10);

                $('#tblAirspaceReport').attr('data-total', total || 0);
                $('#tblAirspaceReport tbody').html(
                    renderReportRows(rows, ((page - 1) * size) + 1)
                );
                $('#tblAirspaceReport').paging({
                    onClickButton: 'LoadAirspaceReportData',
                    pageSize: size
                });
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error('[AIRSPACE REPORT] Request failed:', {
                    status: xhr.status,
                    textStatus: textStatus,
                    error: errorThrown,
                    response: xhr.responseJSON || xhr.responseText
                });
                alert('Không tải được báo cáo Airspace.');
            }).always(function () {
                $('#btnReportSearch, #btnReportExport').prop('disabled', false);
            });
        }

        function searchAirspaceReport() {
            $('#tblAirspaceReport').attr('data-pageindex', 1);
            LoadAirspaceReportData();
        }

        function reportTimeChanged() {
            var enabled = $('#ddlReportTime').val() !== '0';
            $('#txtReportFromTime, #txtReportToTime').prop('disabled', !enabled);
            if (!enabled) {
                $('#txtReportFromTime').val('0000');
                $('#txtReportToTime').val('2359');
            }
        }

        function exportAirspaceReportExcel() {
            if (!validateReportDates()) return;

            var request = getAirspaceReportSearchObject(false);
            request.P_PAGESIZE = 1000000;
            request.P_PAGEINDEX = 0;

            $('#btnReportExport').prop('disabled', true);
            $.ajax({
                method: 'PUT',
                url: airspaceReportUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request)
            }).done(function (data) {
                var rows = data && data.ListValue ? data.ListValue : [];
                if (!rows.length) {
                    alert('Không có dữ liệu để Export Excel.');
                    return;
                }

                var html =
                    '<table border="1"><thead><tr>' +
                    '<th>NO</th><th>OPER</th><th>FLIGHTDATE</th>' +
                    '<th>CALLSIGN</th><th>REGIS</th><th>R_CRAFT</th>' +
                    '<th>F_CRAFT</th><th>PURPOSE</th><th>P_TYPE</th>' +
                    '<th>FROM</th><th>TO</th><th>ATD</th><th>ATA</th>' +
                    '<th>VIA</th><th>FPL_VIA</th><th>REMARK</th>' +
                    '<th>ETD</th><th>ETA</th></tr></thead><tbody>' +
                    renderReportRows(rows, 1) +
                    '</tbody></table>';

                downloadAirspaceReportExcel(html);
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error('[AIRSPACE REPORT EXPORT] Request failed:', {
                    status: xhr.status,
                    textStatus: textStatus,
                    error: errorThrown,
                    response: xhr.responseJSON || xhr.responseText
                });
                alert('Không thể Export Excel báo cáo Airspace.');
            }).always(function () {
                $('#btnReportExport').prop('disabled', false);
            });
        }

        function downloadAirspaceReportExcel(html) {
            var now = new Date();
            var fileName =
                'airspace_report_' +
                dateFormat(now, 'dd-mm-yyyy') + '_' +
                now.getHours() + '-' + now.getMinutes() + '.xls';
            var content =
                '\ufeff<html><head><meta charset="utf-8"></head><body>' +
                html +
                '</body></html>';
            var blob = new Blob([content], {
                type: 'application/vnd.ms-excel;charset=utf-8'
            });
            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');

            link.href = url;
            link.download = fileName;
            link.style.display = 'none';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
        }

        $('#txtReportFromPicker').on('change', function () {
            $('#txtReportFrom').val(reportIsoToDisplay(this.value));
        });
        $('#txtReportToPicker').on('change', function () {
            $('#txtReportTo').val(reportIsoToDisplay(this.value));
        });

        $(function () {
            var yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            var display = dateFormat(yesterday, 'dd-mm-yyyy');

            $('#txtReportFrom, #txtReportTo').val(display);
            $('#txtReportFromPicker, #txtReportToPicker')
                .val(reportDisplayToIso(display));
            LoadAirspaceReportData();
        });
    </script>
</asp:Content>
