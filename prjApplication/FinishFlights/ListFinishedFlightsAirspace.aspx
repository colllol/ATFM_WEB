<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ListFinishedFlightsAirspace.aspx.cs"
    Inherits="prjApplication.FinishFlights.ListFinishedFlightsAirspace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .airspace-toolbar {
            display: flex;
            align-items: center;
            flex-wrap: nowrap;
            gap: 6px;
            overflow-x: auto;
            padding: 10px;
            white-space: nowrap;
        }

        .airspace-date {
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }

        .airspace-date label {
            margin: 0;
            color: #315a77;
            font-size: 12px;
            font-weight: 700;
        }

        .airspace-date input[type=date] {
            width: 130px;
            height: 36px;
            padding: 6px 8px;
            border: 1px solid #8eb9d6;
            border-radius: 6px;
        }

        .airspace-hidden-date {
            display: none !important;
        }

        .airspace-toolbar input[type=text] {
            width: 48px;
            height: 34px;
            padding: 5px;
            text-align: center;
        }

        .airspace-toolbar select {
            height: 34px;
            color: #173b59;
        }

        .airspace-toolbar button {
            min-width: 96px;
            text-transform: uppercase;
        }

        #tblAirspace {
            min-width: 1640px;
            table-layout: fixed;
        }

        #tblAirspace th,
        #tblAirspace td {
            padding: 2px;
            text-align: center;
            vertical-align: middle;
        }

        #tblAirspace thead tr:last-child {
            color: #fff;
            background: #418ed6;
            text-transform: uppercase;
        }

        #tblAirspace thead input {
            width: 100%;
            min-width: 48px;
            height: 30px;
            padding: 3px;
            color: #111;
            text-align: center;
        }

        #tblAirspace tbody input {
            width: 100%;
            height: 30px;
            border: 0;
            text-align: center;
            text-transform: uppercase;
        }

        #tblAirspace tbody tr[data-mode=new] input {
            background: #fff4c8;
        }

        #tblAirspace tbody tr[data-dirty=true] input {
            background: #e7f4ff;
        }

        #tblAirspace tbody input:disabled {
            color: #333;
            background: #f3f5f7;
        }

        .airspace-grid-wrap {
            overflow: auto;
        }

        .airspace-action {
            white-space: nowrap;
        }

        #airspaceTotal {
            display: none;
            min-width: 85px;
            font-weight: 700;
        }
    </style>

    <div class="well well-sm airspace-toolbar">
        <span class="airspace-date">
            <label for="txtAirspaceFromPicker">FROM</label>
            <input id="txtAirspaceFromPicker" type="date" aria-label="Ngày bắt đầu" />
            <input id="txtAirspaceFrom" type="text" class="airspace-hidden-date" />
        </span>
        <input id="txtAirspaceFromTime" type="text" maxlength="4" value="0000" disabled="disabled" />

        <span class="airspace-date">
            <label for="txtAirspaceToPicker">TO</label>
            <input id="txtAirspaceToPicker" type="date" aria-label="Ngày kết thúc" />
            <input id="txtAirspaceTo" type="text" class="airspace-hidden-date" />
        </span>
        <input id="txtAirspaceToTime" type="text" maxlength="4" value="2359" disabled="disabled" />

        <b>HOUR:</b>
        <select id="ddlAirspaceTime" onchange="airspaceTimeChanged();">
            <option value="0">-ALL-</option>
            <option value="1">ETD</option>
            <option value="2">ETA</option>
            <option value="3">ATD</option>
            <option value="4">ATA</option>
        </select>

        <select id="ddlAirspaceStatus" onchange="airspaceStatusChanged();">
            <option value="0">CHƯA GỬI DUYỆT</option>
            <option value="2">CHỜ DUYỆT</option>
        </select>

        <select id="ddlAirspacePageSize" onchange="airspaceSearch();">
            <option value="100">100</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
        </select>

        <button type="button" id="btnAirspaceSearch" class="btn btn-sm btn-primary"
            onclick="airspaceSearch();">Search</button>
        <button type="button" id="btnAirspaceSubmit" class="btn btn-sm btn-success"
            onclick="submitAirspaceForApproval();">Gửi duyệt</button>
        <button type="button" id="btnAirspaceAdd" class="btn btn-sm btn-primary"
            onclick="addAirspaceRow();">Add New</button>
        <button type="button" id="btnAirspaceSave" class="btn btn-sm btn-primary"
            onclick="saveAirspaceRows();">Update All</button>
        <span id="airspaceTotal">TỔNG SỐ: 0</span>
    </div>

    <div class="airspace-grid-wrap">
        <table id="tblAirspace" class="table table-bordered" data-pageindex="1" data-total="0">
            <thead>
                <tr>
                    <th></th>
                    <th><input id="fltOper" type="text" /></th>
                    <th></th>
                    <th><input id="fltCallsign" type="text" /></th>
                    <th><input id="fltRegis" type="text" /></th>
                    <th><input id="fltRCraft" type="text" /></th>
                    <th><input id="fltFCraft" type="text" /></th>
                    <th><input id="fltPurpose" type="text" /></th>
                    <th><input id="fltPType" type="text" /></th>
                    <th><input id="fltFrom" type="text" /></th>
                    <th><input id="fltTo" type="text" /></th>
                    <th><input id="fltATD" type="text" /></th>
                    <th><input id="fltATA" type="text" /></th>
                    <th><input id="fltVia" type="text" /></th>
                    <th><input id="fltFplVia" type="text" /></th>
                    <th><input id="fltRemark" type="text" /></th>
                    <th><input id="fltETD" type="text" /></th>
                    <th><input id="fltETA" type="text" /></th>
                    <th></th>
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
                    <th>ACTION</th>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>

    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        var airspacePackageUrl =
            urlApi + 'api/ApiExtension/ExcuteTable?packageName=AIRSPACE_PKG&storeName=GET_FINISHED_AIRSPACE';
        var airspaceReturnUrl =
            urlApi + 'api/ApiExtension/ExcuteReturnInt?packageName=AIRSPACE_PKG&storeName=';
        var airspaceUser = '<%= _user.UserName.ToString() %>';

        function airspaceEncode(value) {
            return $('<div/>').text(value == null ? '' : value).html();
        }

        function airspaceDisplayDate(value) {
            if (!value) return '';
            var date = new Date(value);
            if (isNaN(date.getTime())) return value;
            return dateFormat(date, 'dd-mm-yyyy');
        }

        function airspaceIsoToDisplay(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3
                ? parts[2] + '-' + parts[1] + '-' + parts[0]
                : '';
        }

        function airspaceDisplayToIso(value) {
            var parts = String(value || '').split('-');
            return parts.length === 3
                ? parts[2] + '-' + parts[1] + '-' + parts[0]
                : '';
        }

        function validateAirspaceDates() {
            var fromDate = $('#txtAirspaceFromPicker').val();
            var toDate = $('#txtAirspaceToPicker').val();

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

        function getAirspaceSearchObject(resetPage) {
            var pageIndex;

            if (resetPage) {
                $('#tblAirspace').attr('data-pageindex', 1);
            }

            pageIndex = parseInt($('#tblAirspace').attr('data-pageindex'), 10);
            if (isNaN(pageIndex) || pageIndex < 1) pageIndex = 1;

            return {
                P_PAGESIZE: parseInt($('#ddlAirspacePageSize').val(), 10),
                P_PAGEINDEX: pageIndex - 1,
                P_CALLSIGN: $.trim($('#fltCallsign').val()),
                P_REGIS: $.trim($('#fltRegis').val()),
                P_FROM_AIRP: $.trim($('#fltFrom').val()),
                P_TO_AIRP: $.trim($('#fltTo').val()),
                P_PURPOSE: $.trim($('#fltPurpose').val()),
                P_OPER: $.trim($('#fltOper').val()),
                P_FCRAFT: $.trim($('#fltFCraft').val()),
                P_RCRAFT: $.trim($('#fltRCraft').val()),
                P_P_TYPE: $.trim($('#fltPType').val()),
                P_VIA: $.trim($('#fltVia').val()),
                P_FPLVIA: $.trim($('#fltFplVia').val()),
                P_REMARK: $.trim($('#fltRemark').val()),
                P_ETA: $.trim($('#fltETA').val()),
                P_ETD: $.trim($('#fltETD').val()),
                P_ATA: $.trim($('#fltATA').val()),
                P_ATD: $.trim($('#fltATD').val()),
                P_ISACCEPTED: parseInt($('#ddlAirspaceStatus').val(), 10),
                P_STARTDATE: $('#txtAirspaceFrom').val(),
                P_FINISHDATE: $('#txtAirspaceTo').val(),
                P_KHUNGGIO1: $('#txtAirspaceFromTime').val(),
                P_KHUNGGIO2: $('#txtAirspaceToTime').val(),
                P_CAT_HA: parseInt($('#ddlAirspaceTime').val(), 10) || 0
            };
        }

        function airspaceInput(field, value, disabled) {
            return '<input data-field="' + field + '" value="' +
                airspaceEncode(value) + '"' + (disabled ? ' disabled="disabled"' : '') + ' />';
        }

        function renderAirspaceRows(data) {
            var html = '';
            var page = parseInt($('#tblAirspace').attr('data-pageindex'), 10) || 1;
            var size = parseInt($('#ddlAirspacePageSize').val(), 10);
            var number = ((page - 1) * size) + 1;
            var waiting = $('#ddlAirspaceStatus').val() === '2';

            $.each(data.ListValue || [], function (_, item) {
                html += '<tr data-id="' + item.FLIGHT_ID + '" data-dirty="false">' +
                    '<td>' + number + '</td>' +
                    '<td>' + airspaceInput('OPER', item.OPER, waiting) + '</td>' +
                    '<td>' + airspaceInput('FLIGHTDATE', airspaceDisplayDate(item.FLIGHTDATE), waiting) + '</td>' +
                    '<td>' + airspaceInput('CALLSIGN', item.CALLSIGN, waiting) + '</td>' +
                    '<td>' + airspaceInput('REGIS', item.REGIS, waiting) + '</td>' +
                    '<td>' + airspaceInput('RCRAFT', item.RCRAFT, waiting) + '</td>' +
                    '<td>' + airspaceInput('FCRAFT', item.FCRAFT, waiting) + '</td>' +
                    '<td>' + airspaceInput('PURPOSE', item.PURPOSE, waiting) + '</td>' +
                    '<td>' + airspaceInput('PTYPE', item.P_TYPE, waiting) + '</td>' +
                    '<td>' + airspaceInput('FROM_AIRP', item.FROM_AIRP, waiting) + '</td>' +
                    '<td>' + airspaceInput('TO_AIRP', item.TO_AIRP, waiting) + '</td>' +
                    '<td>' + airspaceInput('ATD', item.ATD, waiting) + '</td>' +
                    '<td>' + airspaceInput('ATA', item.ATA, waiting) + '</td>' +
                    '<td>' + airspaceInput('VIA', item.VIA, waiting) + '</td>' +
                    '<td>' + airspaceInput('FPLVIA', item.FPLVIA, waiting) + '</td>' +
                    '<td>' + airspaceInput('REMARK', item.REMARK, waiting) + '</td>' +
                    '<td>' + airspaceInput('ETD', item.ETD, waiting) + '</td>' +
                    '<td>' + airspaceInput('ETA', item.ETA, waiting) + '</td>' +
                    '<td class="airspace-action">' +
                    (waiting ? '' :
                        '<button type="button" class="btn btn-xs btn-danger" ' +
                        'onclick="deleteAirspaceRow(' + item.FLIGHT_ID + ')">XÓA</button>') +
                    '</td></tr>';
                number++;
            });

            return html;
        }

        function LoadAirspaceData() {
            if (!validateAirspaceDates()) return;

            var request = getAirspaceSearchObject(false);
            console.log('[AIRSPACE SEARCH] Request:', request);

            $.ajax({
                method: 'PUT',
                url: airspacePackageUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request),
                beforeSend: function () {
                    $('#tblAirspace tbody').empty();
                    $('#btnAirspaceSearch').prop('disabled', true);
                }
            }).done(function (data) {
                var rows = data && data.ListValue ? data.ListValue : [];
                var total = rows.length ? parseInt(rows[0].SUMRECORD, 10) : 0;

                $('#tblAirspace').attr('data-total', total || 0);
                $('#airspaceTotal').text('TỔNG SỐ: ' + (total || 0));
                $('#tblAirspace tbody').html(renderAirspaceRows(data || { ListValue: [] }));
                $('#tblAirspace').paging({
                    onClickButton: 'LoadAirspaceData',
                    pageSize: parseInt($('#ddlAirspacePageSize').val(), 10)
                });
            }).fail(function (xhr, textStatus, errorThrown) {
                console.error('[AIRSPACE SEARCH] Request failed:', {
                    status: xhr.status,
                    textStatus: textStatus,
                    error: errorThrown,
                    response: xhr.responseJSON || xhr.responseText
                });
                $('#airspaceTotal').text('TỔNG SỐ: 0');
                alert('Không tải được dữ liệu chuyến bay vùng trời.');
            }).always(function () {
                $('#btnAirspaceSearch').prop('disabled', false);
            });
        }

        function airspaceSearch() {
            $('#tblAirspace').attr('data-pageindex', 1);
            LoadAirspaceData();
        }

        function airspaceStatusChanged() {
            var isNew = $('#ddlAirspaceStatus').val() === '0';
            $('#btnAirspaceSubmit, #btnAirspaceAdd, #btnAirspaceSave').prop('disabled', !isNew);
            airspaceSearch();
        }

        function airspaceTimeChanged() {
            var enabled = $('#ddlAirspaceTime').val() !== '0';
            $('#txtAirspaceFromTime, #txtAirspaceToTime').prop('disabled', !enabled);
            if (!enabled) {
                $('#txtAirspaceFromTime').val('0000');
                $('#txtAirspaceToTime').val('2359');
            }
        }

        function addAirspaceRow() {
            if ($('#ddlAirspaceStatus').val() !== '0') return;

            var html = '<tr data-mode="new" data-dirty="true">' +
                '<td>NEW</td>' +
                '<td>' + airspaceInput('OPER', '', false) + '</td>' +
                '<td>' + airspaceInput('FLIGHTDATE', $('#txtAirspaceFrom').val(), false) + '</td>' +
                '<td>' + airspaceInput('CALLSIGN', '', false) + '</td>' +
                '<td>' + airspaceInput('REGIS', '', false) + '</td>' +
                '<td>' + airspaceInput('RCRAFT', '', false) + '</td>' +
                '<td>' + airspaceInput('FCRAFT', '', false) + '</td>' +
                '<td>' + airspaceInput('PURPOSE', '', false) + '</td>' +
                '<td>' + airspaceInput('PTYPE', 'LD', false) + '</td>' +
                '<td>' + airspaceInput('FROM_AIRP', '', false) + '</td>' +
                '<td>' + airspaceInput('TO_AIRP', '', false) + '</td>' +
                '<td>' + airspaceInput('ATD', '', false) + '</td>' +
                '<td>' + airspaceInput('ATA', '', false) + '</td>' +
                '<td>' + airspaceInput('VIA', '', false) + '</td>' +
                '<td>' + airspaceInput('FPLVIA', '', false) + '</td>' +
                '<td>' + airspaceInput('REMARK', '', false) + '</td>' +
                '<td>' + airspaceInput('ETD', '', false) + '</td>' +
                '<td>' + airspaceInput('ETA', '', false) + '</td>' +
                '<td><button type="button" class="btn btn-xs btn-danger" ' +
                'onclick="$(this).closest(\'tr\').remove()">XÓA</button></td></tr>';

            $('#tblAirspace tbody').prepend(html);
            $('#tblAirspace tbody tr:first input[data-field=CALLSIGN]').focus();
        }

        function airspaceRowObject($row) {
            function value(field) {
                return $.trim($row.find('[data-field=' + field + ']').val());
            }

            return {
                P_FLIGHT_ID: parseInt($row.attr('data-id'), 10) || 0,
                P_FLIGHTDATE: value('FLIGHTDATE'),
                P_OPER: value('OPER'),
                P_CALLSIGN: value('CALLSIGN'),
                P_REGIS: value('REGIS'),
                P_RCRAFT: value('RCRAFT'),
                P_FCRAFT: value('FCRAFT'),
                P_PURPOSE: value('PURPOSE'),
                P_PTYPE: value('PTYPE'),
                P_FROM_AIRP: value('FROM_AIRP'),
                P_TO_AIRP: value('TO_AIRP'),
                P_ATD: value('ATD'),
                P_ATA: value('ATA'),
                P_VIA: value('VIA'),
                P_FPLVIA: value('FPLVIA'),
                P_ETD: value('ETD'),
                P_ETA: value('ETA'),
                P_USERCREATE: airspaceUser,
                P_REMARK: value('REMARK')
            };
        }

        function validAirspaceRow($row) {
            var item = airspaceRowObject($row);
            var datePattern = /^\d{2}-\d{2}-\d{4}$/;

            if (!item.P_CALLSIGN || !datePattern.test(item.P_FLIGHTDATE)) {
                alert('CALLSIGN và FLIGHTDATE (dd-MM-yyyy) là bắt buộc.');
                $row.find('[data-field=CALLSIGN]').focus();
                return false;
            }
            return true;
        }

        function saveAirspaceRows() {
            if ($('#ddlAirspaceStatus').val() !== '0') return;

            var $rows = $('#tblAirspace tbody tr[data-mode=new], ' +
                '#tblAirspace tbody tr[data-dirty=true]:not([data-mode=new])');
            var requests = [];

            if (!$rows.length) {
                alert('Không có dữ liệu cần lưu.');
                return;
            }

            var valid = true;
            $rows.each(function () {
                if (!validAirspaceRow($(this))) {
                    valid = false;
                    return false;
                }
            });
            if (!valid) return;

            $('#btnAirspaceSave').prop('disabled', true);
            $rows.each(function () {
                var $row = $(this);
                var isNew = $row.attr('data-mode') === 'new';
                var data = airspaceRowObject($row);
                if (isNew) delete data.P_FLIGHT_ID;

                requests.push($.ajax({
                    method: 'PUT',
                    url: airspaceReturnUrl +
                        (isNew
                            ? 'INSERT_FINISHED_AIRSPACE'
                            : 'UPDATE_FINISHED_AIRSPACE'),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(data)
                }).then(function (response) {
                    var affected = response && response.Code === '00'
                        ? parseInt(response.ListValue, 10)
                        : -1;
                    if (isNaN(affected) || affected <= 0) {
                        return $.Deferred().reject(response).promise();
                    }
                    return response;
                }));
            });

            $.when.apply($, requests).done(function () {
                alert('Lưu dữ liệu thành công.');
                airspaceSearch();
            }).fail(function (xhr) {
                console.error('[AIRSPACE SAVE] Request failed:', xhr.responseJSON || xhr.responseText);
                alert('Có lỗi khi lưu dữ liệu vùng trời.');
            }).always(function () {
                $('#btnAirspaceSave').prop('disabled', false);
            });
        }

        function deleteAirspaceRow(flightId) {
            if (!confirm('Bạn có chắc muốn xóa chuyến bay này?')) return;

            $.ajax({
                method: 'PUT',
                url: airspaceReturnUrl + 'DELETE_FINISHED_AIRSPACE',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    P_FLIGHT_ID: flightId,
                    P_USER: airspaceUser
                })
            }).done(function (data) {
                var affected = data && data.Code === '00'
                    ? parseInt(data.ListValue, 10)
                    : -1;
                if (affected !== 1) {
                    alert('Không thể xóa chuyến bay đã gửi duyệt hoặc không tồn tại.');
                    return;
                }
                airspaceSearch();
            }).fail(function (xhr) {
                console.error('[AIRSPACE DELETE] Request failed:', xhr.responseJSON || xhr.responseText);
                alert('Xóa chuyến bay không thành công.');
            });
        }

        function submitAirspaceForApproval() {
            if ($('#ddlAirspaceStatus').val() !== '0' || !validateAirspaceDates()) return;
            if ($('#tblAirspace tbody tr[data-mode=new], ' +
                '#tblAirspace tbody tr[data-dirty=true]').length) {
                alert('Vui lòng lưu các dòng mới hoặc đã sửa trước khi gửi duyệt.');
                return;
            }
            if (!confirm('Gửi duyệt toàn bộ chuyến bay chưa gửi trong khoảng ngày đã chọn?')) return;

            $('#btnAirspaceSubmit').prop('disabled', true);
            $.ajax({
                method: 'PUT',
                url: airspaceReturnUrl + 'SUBMIT_FINISHED_AIRSPACE',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    P_STARTDATE: $('#txtAirspaceFrom').val(),
                    P_FINISHDATE: $('#txtAirspaceTo').val(),
                    P_USER: airspaceUser
                })
            }).done(function (data) {
                var affected = data && data.Code === '00'
                    ? parseInt(data.ListValue, 10)
                    : -1;
                if (isNaN(affected) || affected < 0) {
                    alert('Gửi duyệt không thành công.');
                    return;
                }
                alert('Đã gửi duyệt ' + affected + ' chuyến bay.');
                $('#ddlAirspaceStatus').val('2');
                airspaceStatusChanged();
            }).fail(function (xhr) {
                console.error('[AIRSPACE SUBMIT] Request failed:', xhr.responseJSON || xhr.responseText);
                alert('Không thể gửi duyệt dữ liệu vùng trời.');
            }).always(function () {
                $('#btnAirspaceSubmit').prop('disabled',
                    $('#ddlAirspaceStatus').val() !== '0');
            });
        }

        $('#tblAirspace').on('input change', 'tbody input', function () {
            $(this).closest('tr').attr('data-dirty', 'true');
        });

        $('#txtAirspaceFromPicker').on('change', function () {
            $('#txtAirspaceFrom').val(airspaceIsoToDisplay(this.value));
        });
        $('#txtAirspaceToPicker').on('change', function () {
            $('#txtAirspaceTo').val(airspaceIsoToDisplay(this.value));
        });

        $(function () {
            var yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            var display = dateFormat(yesterday, 'dd-mm-yyyy');
            $('#txtAirspaceFrom, #txtAirspaceTo').val(display);
            $('#txtAirspaceFromPicker, #txtAirspaceToPicker')
                .val(airspaceDisplayToIso(display));
            airspaceStatusChanged();
        });
    </script>
</asp:Content>
