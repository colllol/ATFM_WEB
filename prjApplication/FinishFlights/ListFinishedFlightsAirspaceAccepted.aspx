<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ListFinishedFlightsAirspaceAccepted.aspx.cs"
    Inherits="prjApplication.FinishFlights.ListFinishedFlightsAirspaceAccepted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .airspace-approve-toolbar {
            display: flex;
            align-items: center;
            flex-wrap: nowrap;
            gap: 6px;
            overflow-x: auto;
            padding: 10px;
            white-space: nowrap;
        }

        .airspace-approve-date {
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }

        .airspace-approve-date label {
            margin: 0;
            color: #315a77;
            font-size: 12px;
            font-weight: 700;
        }

        .airspace-approve-date input[type=date] {
            width: 130px;
            height: 36px;
            padding: 6px 8px;
            border: 1px solid #8eb9d6;
            border-radius: 6px;
        }

        .airspace-approve-hidden-date {
            display: none !important;
        }

        .airspace-approve-toolbar input[type=text] {
            width: 48px;
            height: 34px;
            padding: 5px;
            text-align: center;
        }

        .airspace-approve-toolbar select {
            height: 34px;
            color: #173b59;
        }

        .airspace-approve-toolbar button {
            min-width: 96px;
            text-transform: uppercase;
        }

        .airspace-approve-wrap {
            overflow: auto;
        }

        #tblAirspaceAccepted {
            min-width: 1560px;
            table-layout: fixed;
        }

        #tblAirspaceAccepted th,
        #tblAirspaceAccepted td {
            padding: 4px;
            text-align: center;
            vertical-align: middle;
        }

        #tblAirspaceAccepted thead tr:last-child {
            color: #fff;
            background: #418ed6;
            text-transform: uppercase;
        }

        #tblAirspaceAccepted thead input {
            width: 100%;
            min-width: 48px;
            height: 30px;
            padding: 3px;
            color: #111;
            text-align: center;
        }
    </style>

    <div class="well well-sm airspace-approve-toolbar">
        <span class="airspace-approve-date">
            <label for="txtApproveFromPicker">FROM</label>
            <input id="txtApproveFromPicker" type="date" aria-label="Ngày bắt đầu" />
            <input id="txtApproveFrom" type="text" class="airspace-approve-hidden-date" />
        </span>
        <input id="txtApproveFromTime" type="text" maxlength="4" value="0000" disabled="disabled" />

        <span class="airspace-approve-date">
            <label for="txtApproveToPicker">TO</label>
            <input id="txtApproveToPicker" type="date" aria-label="Ngày kết thúc" />
            <input id="txtApproveTo" type="text" class="airspace-approve-hidden-date" />
        </span>
        <input id="txtApproveToTime" type="text" maxlength="4" value="2359" disabled="disabled" />

        <b>HOUR:</b>
        <select id="ddlApproveTime" onchange="approveTimeChanged();">
            <option value="0">-ALL-</option>
            <option value="1">ETD</option>
            <option value="2">ETA</option>
            <option value="3">ATD</option>
            <option value="4">ATA</option>
        </select>

        <select id="ddlApproveStatus" onchange="approveStatusChanged();">
            <option value="2">CHỜ DUYỆT</option>
            <option value="1">ĐÃ DUYỆT</option>
        </select>

        <select id="ddlApprovePageSize" onchange="searchAirspaceApproval();">
            <option value="100">100</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
        </select>

        <button type="button" id="btnApproveSearch" class="btn btn-sm btn-primary"
            onclick="searchAirspaceApproval();">Search</button>
        <button type="button" id="btnApproveFlights" class="btn btn-sm btn-success"
            onclick="approveAirspaceFlights();">Duyệt</button>
        <button type="button" id="btnExportAirspaceMessage" class="btn btn-sm btn-primary"
            disabled="disabled" onclick="exportAirspaceMessage();">Export Message</button>
    </div>

    <div class="airspace-approve-wrap">
        <table id="tblAirspaceAccepted" class="table table-bordered"
            data-pageindex="1" data-total="0">
            <thead>
                <tr>
                    <th></th>
                    <th><input id="approveOper" type="text" /></th>
                    <th></th>
                    <th><input id="approveCallsign" type="text" /></th>
                    <th><input id="approveRegis" type="text" /></th>
                    <th><input id="approveRCraft" type="text" /></th>
                    <th><input id="approveFCraft" type="text" /></th>
                    <th><input id="approvePurpose" type="text" /></th>
                    <th><input id="approvePType" type="text" /></th>
                    <th><input id="approveFrom" type="text" /></th>
                    <th><input id="approveTo" type="text" /></th>
                    <th><input id="approveATD" type="text" /></th>
                    <th><input id="approveATA" type="text" /></th>
                    <th><input id="approveVia" type="text" /></th>
                    <th><input id="approveFplVia" type="text" /></th>
                    <th><input id="approveRemark" type="text" /></th>
                    <th><input id="approveETD" type="text" /></th>
                    <th><input id="approveETA" type="text" /></th>
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
        var airspaceApprovalSearchUrl =
            urlApi + 'api/ApiExtension/ExcuteTable?packageName=AIRSPACE_PKG&storeName=GET_FINISHED_AIRSPACE';
        var airspaceApproveUrl =
            urlApi + 'api/ApiExtension/ExcuteReturnInt?packageName=AIRSPACE_PKG&storeName=APPROVE_FINISHED_AIRSPACE';
        var airspaceExportMessageUrl =
            urlApi + 'api/ApiExtension/ExcuteReturnInt?packageName=AIRSPACE_PKG&storeName=EXPORT_AIRSPACE_MESSAGE';
        var airspaceApprover = '<%= _user.UserName.ToString() %>';

        function approvalEncode(value) {
            return $('<div/>').text(value == null ? '' : value).html();
        }

        function approvalDisplayDate(value) {
            if (!value) return '';
            var date = new Date(value);
            return isNaN(date.getTime()) ? value : dateFormat(date, 'dd-mm-yyyy');
        }

        function approvalIsoToDisplay(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3
                ? parts[2] + '-' + parts[1] + '-' + parts[0]
                : '';
        }

        function approvalDisplayToIso(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3
                ? parts[2] + '-' + parts[1] + '-' + parts[0]
                : '';
        }

        function validateApprovalDates() {
            var fromDate = $('#txtApproveFromPicker').val();
            var toDate = $('#txtApproveToPicker').val();

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

        function getApprovalSearchObject(resetPage) {
            var pageIndex;
            if (resetPage) $('#tblAirspaceAccepted').attr('data-pageindex', 1);

            pageIndex = parseInt($('#tblAirspaceAccepted').attr('data-pageindex'), 10);
            if (isNaN(pageIndex) || pageIndex < 1) pageIndex = 1;

            return {
                P_PAGESIZE: parseInt($('#ddlApprovePageSize').val(), 10),
                P_PAGEINDEX: pageIndex - 1,
                P_CALLSIGN: $.trim($('#approveCallsign').val()),
                P_REGIS: $.trim($('#approveRegis').val()),
                P_FROM_AIRP: $.trim($('#approveFrom').val()),
                P_TO_AIRP: $.trim($('#approveTo').val()),
                P_PURPOSE: $.trim($('#approvePurpose').val()),
                P_OPER: $.trim($('#approveOper').val()),
                P_FCRAFT: $.trim($('#approveFCraft').val()),
                P_RCRAFT: $.trim($('#approveRCraft').val()),
                P_P_TYPE: $.trim($('#approvePType').val()),
                P_VIA: $.trim($('#approveVia').val()),
                P_FPLVIA: $.trim($('#approveFplVia').val()),
                P_REMARK: $.trim($('#approveRemark').val()),
                P_ETA: $.trim($('#approveETA').val()),
                P_ETD: $.trim($('#approveETD').val()),
                P_ATA: $.trim($('#approveATA').val()),
                P_ATD: $.trim($('#approveATD').val()),
                P_ISACCEPTED: parseInt($('#ddlApproveStatus').val(), 10),
                P_STARTDATE: $('#txtApproveFrom').val(),
                P_FINISHDATE: $('#txtApproveTo').val(),
                P_KHUNGGIO1: $('#txtApproveFromTime').val(),
                P_KHUNGGIO2: $('#txtApproveToTime').val(),
                P_CAT_HA: parseInt($('#ddlApproveTime').val(), 10) || 0
            };
        }

        function renderApprovalRows(data) {
            var html = '';
            var page = parseInt($('#tblAirspaceAccepted').attr('data-pageindex'), 10) || 1;
            var size = parseInt($('#ddlApprovePageSize').val(), 10);
            var number = ((page - 1) * size) + 1;

            $.each(data.ListValue || [], function (_, item) {
                html += '<tr>' +
                    '<td>' + number + '</td>' +
                    '<td>' + approvalEncode(item.OPER) + '</td>' +
                    '<td>' + approvalEncode(approvalDisplayDate(item.FLIGHTDATE)) + '</td>' +
                    '<td>' + approvalEncode(item.CALLSIGN) + '</td>' +
                    '<td>' + approvalEncode(item.REGIS) + '</td>' +
                    '<td>' + approvalEncode(item.RCRAFT) + '</td>' +
                    '<td>' + approvalEncode(item.FCRAFT) + '</td>' +
                    '<td>' + approvalEncode(item.PURPOSE) + '</td>' +
                    '<td>' + approvalEncode(item.P_TYPE) + '</td>' +
                    '<td>' + approvalEncode(item.FROM_AIRP) + '</td>' +
                    '<td>' + approvalEncode(item.TO_AIRP) + '</td>' +
                    '<td>' + approvalEncode(item.ATD) + '</td>' +
                    '<td>' + approvalEncode(item.ATA) + '</td>' +
                    '<td>' + approvalEncode(item.VIA) + '</td>' +
                    '<td>' + approvalEncode(item.FPLVIA) + '</td>' +
                    '<td>' + approvalEncode(item.REMARK) + '</td>' +
                    '<td>' + approvalEncode(item.ETD) + '</td>' +
                    '<td>' + approvalEncode(item.ETA) + '</td>' +
                    '</tr>';
                number++;
            });
            return html;
        }

        function LoadAirspaceApprovalData() {
            if (!validateApprovalDates()) return;

            var request = getApprovalSearchObject(false);
            console.log('[AIRSPACE APPROVAL SEARCH] Request:', request);

            $.ajax({
                method: 'PUT',
                url: airspaceApprovalSearchUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request),
                beforeSend: function () {
                    $('#tblAirspaceAccepted tbody').empty();
                    $('#btnApproveSearch').prop('disabled', true);
                }
            }).done(function (data) {
                var rows = data && data.ListValue ? data.ListValue : [];
                var total = rows.length ? parseInt(rows[0].SUMRECORD, 10) : 0;

                $('#tblAirspaceAccepted').attr('data-total', total || 0);
                $('#tblAirspaceAccepted tbody').html(
                    renderApprovalRows(data || { ListValue: [] })
                );
                $('#tblAirspaceAccepted').paging({
                    onClickButton: 'LoadAirspaceApprovalData',
                    pageSize: parseInt($('#ddlApprovePageSize').val(), 10)
                });
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error('[AIRSPACE APPROVAL SEARCH] Request failed:', {
                    status: xhr.status,
                    textStatus: textStatus,
                    error: errorThrown,
                    response: xhr.responseJSON || xhr.responseText
                });
                alert('Không tải được danh sách phê duyệt Airspace.');
            }).always(function () {
                $('#btnApproveSearch').prop('disabled', false);
            });
        }

        function searchAirspaceApproval() {
            $('#tblAirspaceAccepted').attr('data-pageindex', 1);
            LoadAirspaceApprovalData();
        }

        function approveStatusChanged() {
            updateApprovalActionState();
            searchAirspaceApproval();
        }

        function updateApprovalActionState() {
            var status = $('#ddlApproveStatus').val();
            $('#btnApproveFlights').prop('disabled', status !== '2');
            $('#btnExportAirspaceMessage').prop('disabled', status !== '1');
        }

        function approveTimeChanged() {
            var enabled = $('#ddlApproveTime').val() !== '0';
            $('#txtApproveFromTime, #txtApproveToTime').prop('disabled', !enabled);
            if (!enabled) {
                $('#txtApproveFromTime').val('0000');
                $('#txtApproveToTime').val('2359');
            }
        }

        function approveAirspaceFlights() {
            if ($('#ddlApproveStatus').val() !== '2' || !validateApprovalDates()) return;
            if (!confirm('Duyệt toàn bộ chuyến bay đang chờ trong khoảng ngày đã chọn?')) return;

            $('#btnApproveFlights').prop('disabled', true);
            $.ajax({
                method: 'PUT',
                url: airspaceApproveUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    P_STARTDATE: $('#txtApproveFrom').val(),
                    P_FINISHDATE: $('#txtApproveTo').val(),
                    P_USER: airspaceApprover
                })
            }).done(function (data) {
                var affected = data && data.Code === '00'
                    ? parseInt(data.ListValue, 10)
                    : -1;

                if (isNaN(affected) || affected < 0) {
                    alert('Duyệt dữ liệu không thành công.');
                    return;
                }

                alert('Đã duyệt ' + affected + ' chuyến bay.');
                $('#ddlApproveStatus').val('1');
                approveStatusChanged();
            }).fail(function (xhr) {
                console.error(
                    '[AIRSPACE APPROVE] Request failed:',
                    xhr.responseJSON || xhr.responseText
                );
                alert('Không thể duyệt dữ liệu Airspace.');
            }).always(function () {
                updateApprovalActionState();
            });
        }

        function exportAirspaceMessage() {
            if ($('#ddlApproveStatus').val() !== '1' || !validateApprovalDates()) {
                updateApprovalActionState();
                return;
            }

            var startDate = $('#txtApproveFrom').val();
            var finishDate = $('#txtApproveTo').val();

            if (!confirm(
                'Export các chuyến ĐÃ DUYỆT từ '
                + startDate + ' đến ' + finishDate
                + ' thành AIRSPACE MESSAGE?'
            )) {
                return;
            }

            $('#btnExportAirspaceMessage').prop('disabled', true);
            $.ajax({
                method: 'PUT',
                url: airspaceExportMessageUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    P_STARTDATE: startDate,
                    P_FINISHDATE: finishDate,
                    P_USER: airspaceApprover
                })
            }).done(function (data) {
                var exportedFlights = data && data.Code === '00'
                    ? parseInt(data.ListValue, 10)
                    : -1;

                if (isNaN(exportedFlights) || exportedFlights < 0) {
                    console.error('[EXPORT_AIRSPACE_MESSAGE] Response:', data);
                    alert('Export AIRSPACE MESSAGE không thành công.');
                    return;
                }

                alert(
                    'Đã Export ' + exportedFlights
                    + ' chuyến bay thành AIRSPACE MESSAGE.'
                );
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error('[EXPORT_AIRSPACE_MESSAGE] Request failed:', {
                    status: xhr.status,
                    textStatus: textStatus,
                    error: errorThrown,
                    response: xhr.responseJSON || xhr.responseText
                });
                alert('Không thể Export AIRSPACE MESSAGE.');
            }).always(function () {
                updateApprovalActionState();
            });
        }

        $('#txtApproveFromPicker').on('change', function () {
            $('#txtApproveFrom').val(approvalIsoToDisplay(this.value));
        });
        $('#txtApproveToPicker').on('change', function () {
            $('#txtApproveTo').val(approvalIsoToDisplay(this.value));
        });

        $(function () {
            var yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            var display = dateFormat(yesterday, 'dd-mm-yyyy');

            $('#txtApproveFrom, #txtApproveTo').val(display);
            $('#txtApproveFromPicker, #txtApproveToPicker')
                .val(approvalDisplayToIso(display));
            approveStatusChanged();
        });
    </script>
</asp:Content>
