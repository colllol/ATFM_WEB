<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListFlightOnMess.aspx.cs" Inherits="prjApplication.FinishFlights.ListFlightOnMess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .message-flight-page {
            padding: 8px 14px 16px;
            color: #163b59;
        }

        .message-flight-filter {
            display: flex;
            flex-wrap: nowrap;
            align-items: flex-end;
            gap: 8px;
            overflow-x: auto;
            margin-bottom: 8px;
            padding: 10px 12px;
            border: 1px solid #c8dced;
            border-radius: 8px;
            background: #f7fbff;
        }

        .message-flight-field {
            display: flex;
            flex: 0 0 auto;
            flex-direction: column;
            gap: 3px;
        }

        .message-flight-field label {
            margin: 0;
            color: #15527f;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
        }

        .message-flight-field input,
        .message-flight-field select {
            height: 34px;
            padding: 5px 9px;
            border: 1px solid #b9d3e7;
            border-radius: 5px;
            color: #163b59;
            background: #fff;
            text-transform: uppercase;
        }

        .message-flight-date-control {
            position: relative;
            display: flex;
            width: 118px;
        }

        #txtMessageDate {
            width: 88px;
            border-radius: 5px 0 0 5px;
        }

        #btnMessageDatePicker {
            width: 30px;
            height: 34px;
            padding: 0;
            border: 1px solid #b9d3e7;
            border-left: 0;
            border-radius: 0 5px 5px 0;
            color: #15527f;
            background: #fff;
        }

        #btnMessageDatePicker:hover,
        #btnMessageDatePicker:focus {
            color: #fff;
            background: #337ab7;
        }

        #nativeMessageDatePicker {
            position: absolute;
            right: 0;
            bottom: 0;
            width: 1px;
            height: 1px;
            padding: 0;
            border: 0;
            opacity: 0;
            pointer-events: none;
        }

        #ddlMessageType { width: 160px; }
        #ddlThongBaoMode { width: 178px; }
        #txtFromAir, #txtToAir, #txtOper { width: 82px; }
        #ddlPageSize { width: 70px; }

        .message-flight-actions {
            display: flex;
            flex: 0 0 auto;
            gap: 6px;
            align-items: center;
        }

        .message-flight-actions .btn {
            min-width: 98px;
            height: 34px;
            font-weight: 700;
            text-transform: uppercase;
        }

        .message-flight-summary {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin: 7px 0;
        }

        #messageFlightTotal {
            font-weight: 700;
            text-transform: uppercase;
        }

        .message-flight-table-wrap {
            max-height: 560px;
            overflow: auto;
            border: 1px solid #acd0e8;
            background: #fff;
        }

        #tblMessageFlights {
            width: 100%;
            min-width: 1130px;
            margin: 0;
            table-layout: fixed;
        }

        #tblMessageFlights thead {
            position: sticky;
            top: 0;
            z-index: 20;
            color: #fff;
            background: #337ab7;
        }

        #tblMessageFlights th,
        #tblMessageFlights td {
            padding: 6px 5px;
            border-color: #bdd5e6;
            vertical-align: middle;
            text-align: center;
        }

        #tblMessageFlights th {
            height: 34px;
            font-size: 11px;
            color: #fff !important;
            text-transform: uppercase;
            background: #337ab7;
        }

        #tblMessageFlights td {
            overflow: hidden;
            color: #173d5b;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        #tblMessageFlights tbody tr:nth-child(even) {
            background: #f5faff;
        }

        #tblMessageFlights tbody tr:hover {
            background: #dff1ff;
        }

        #tblMessageFlights .text-left {
            text-align: left;
        }

        .message-flight-pager {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            margin-top: 9px;
        }

        .message-flight-pager .btn {
            min-width: 42px;
        }

        #messageFlightPageInfo {
            min-width: 110px;
            text-align: center;
            font-weight: 700;
        }

        .message-flight-empty,
        .message-flight-loading {
            height: 70px;
            color: #6f8290 !important;
            font-style: italic;
            text-align: center !important;
        }
    </style>

    <div class="message-flight-page">
        <div class="message-flight-filter">
            <div class="message-flight-field">
                <label for="txtMessageDate">Ngày điện văn</label>
                <div class="message-flight-date-control">
                    <input id="txtMessageDate" type="text" maxlength="10" placeholder="DD-MM-YYYY" />
                    <button type="button" id="btnMessageDatePicker" title="Chọn ngày điện văn" aria-label="Chọn ngày điện văn" onclick="openMessageDatePicker()">
                        <i class="fa fa-calendar"></i>
                    </button>
                    <input id="nativeMessageDatePicker" type="date" tabindex="-1" aria-hidden="true" />
                </div>
            </div>

            <div class="message-flight-field">
                <label for="ddlMessageType">Loại điện văn</label>
                <select id="ddlMessageType">
                    <option value="HVN MESSAGE">HVN MESSAGE</option>
                    <option value="LANDING FLIGHTS">LANDING FLIGHTS</option>
                    <option value="OVER FLIGHTS">OVER FLIGHTS</option>
                    <option value="QS MESSAGE">QS MESSAGE</option>
                    <option value="AIRSPACE MESSAGE">AIRSPACE MESSAGE</option>
                </select>
            </div>

            <div class="message-flight-field">
                <label for="ddlThongBaoMode">Mục đích</label>
                <select id="ddlThongBaoMode">
                    <option value="0" selected="selected">KẾ HOẠCH 15H</option>
                    <option value="1">BỔ SUNG</option>
                    <option value="2">TẤT CẢ</option>
                </select>
            </div>

            <div class="message-flight-field">
                <label for="txtFromAir">Sân bay đi</label>
                <input id="txtFromAir" data-autocomplete="AERO" type="text" maxlength="4" />
            </div>

            <div class="message-flight-field">
                <label for="txtToAir">Sân bay đến</label>
                <input id="txtToAir" data-autocomplete="AERO" type="text" maxlength="4" />
            </div>

            <div class="message-flight-field">
                <label for="txtOper">Hãng khai thác</label>
                <input id="txtOper" data-autocomplete="OPER" type="text" maxlength="3" />
            </div>

            <div class="message-flight-field">
                <label for="ddlPageSize">Số dòng</label>
                <select id="ddlPageSize">
                    <option value="50">50</option>
                    <option value="100" selected="selected">100</option>
                    <option value="200">200</option>
                    <option value="500">500</option>
                </select>
            </div>

            <div class="message-flight-actions">
                <button type="button" id="btnSearchMessageFlights" class="btn btn-sm btn-primary" onclick="searchMessageFlights()">
                    <i class="fa fa-search"></i> Search
                </button>
                <button type="button" id="btnExportMessageFlights" class="btn btn-sm btn-success" onclick="exportMessageFlights()">
                    <i class="fa fa-file-excel-o"></i> Export Excel
                </button>
            </div>
        </div>

        <div class="message-flight-summary">
            <span id="messageFlightTotal">Tổng số: 0</span>
            <span id="messageFlightCriteria"></span>
        </div>

        <div class="message-flight-table-wrap">
            <table id="tblMessageFlights" class="table table-bordered">
                <colgroup>
                    <col style="width: 45px;" />
                   <%-- <col style="width: 82px;" />
                    <col style="width: 55px;" />--%>
                    <col style="width: 60px;" />
                    <col style="width: 90px;" />
                    <col style="width: 85px;" />
                    <col style="width: 65px;" />
                    <col style="width: 70px;" />
                    <col style="width: 65px;" />
                    <col style="width: 60px;" />
                    <col style="width: 60px;" />
                    <col style="width: 85px;" />
                    <col style="width: 60px;" />
                    <%--<col style="width: 62px;" />--%>
                    <col style="width: 60px;" />
                    <col style="width: 145px;" />
                    <col style="width: 180px;" />
                </colgroup>
                <thead>
                    <tr>
                        <th>No</th>
                       <%-- <th>Message ID</th>
                        <th>Part</th>--%>
                        <th>Oper</th>
                        <th>Callsign</th>
                        <th>Registration</th>
                        <th>Craft</th>
                        <th>Purpose</th>
                        <th>Perm type</th>
                        <th>From</th>
                        <th>To</th>
                        <th>Flight date</th>
                        <th>ETD</th>
                        <th>ETA</th>
                        <%--<th>Flight ID</th>--%>
                        <th>Via</th>
                        <th>Remark</th>
                    </tr>
                </thead>
                <tbody>
                    <tr><td colspan="14" class="message-flight-empty">Nhấn Search để tải dữ liệu.</td></tr>
                </tbody>
            </table>
        </div>

        <div class="message-flight-pager">
            <button type="button" id="btnMessageFlightPrev" class="btn btn-sm btn-default" onclick="changeMessageFlightPage(-1)" disabled="disabled">&lt;</button>
            <span id="messageFlightPageInfo">Trang 1/1</span>
            <button type="button" id="btnMessageFlightNext" class="btn btn-sm btn-default" onclick="changeMessageFlightPage(1)" disabled="disabled">&gt;</button>
        </div>
    </div>

    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        var messageFlightPageIndex = 0;
        var messageFlightTotalRecords = 0;
        var messageFlightLoading = false;
        var messageFlightUrl = urlApi
            + 'api/ApiExtension/ExcuteTable?packageName=MESSAGE_FLIGHT_PKG&storeName=GET_FLIGHTS_ON_MESSAGE';

        function escapeMessageFlightHtml(value) {
            return $('<div/>').text(value == null ? '' : String(value)).html();
        }

        function parseMessageDate(value) {
            var match = /^(\d{2})-(\d{2})-(\d{4})$/.exec($.trim(value || ''));
            if (!match) return null;

            var date = new Date(
                parseInt(match[3], 10),
                parseInt(match[2], 10) - 1,
                parseInt(match[1], 10)
            );

            if (date.getFullYear() !== parseInt(match[3], 10)
                || date.getMonth() !== parseInt(match[2], 10) - 1
                || date.getDate() !== parseInt(match[1], 10)) {
                return null;
            }

            return date;
        }

        function syncNativeMessageDatePicker() {
            var date = parseMessageDate($('#txtMessageDate').val());
            if (!date) return;

            var year = date.getFullYear();
            var month = ('0' + (date.getMonth() + 1)).slice(-2);
            var day = ('0' + date.getDate()).slice(-2);
            $('#nativeMessageDatePicker').val(year + '-' + month + '-' + day);
        }

        function openMessageDatePicker() {
            syncNativeMessageDatePicker();

            var picker = document.getElementById('nativeMessageDatePicker');
            if (!picker) return;

            if (typeof picker.showPicker === 'function') {
                picker.showPicker();
            } else {
                picker.focus();
                picker.click();
            }
        }

        function formatMessageFlightDate(value) {
            if (!value) return '';

            var dotNetDate = /\/Date\((\d+)/.exec(String(value));
            var date = dotNetDate
                ? new Date(parseInt(dotNetDate[1], 10))
                : new Date(value);

            if (isNaN(date.getTime())) return String(value);

            var day = ('0' + date.getDate()).slice(-2);
            var month = ('0' + (date.getMonth() + 1)).slice(-2);
            return day + '-' + month + '-' + date.getFullYear();
        }

        function getMessageFlightQueryValue(names) {
            var query = window.location.search.substring(1).split('&');
            for (var i = 0; i < query.length; i++) {
                var pair = query[i].split('=');
                var key = decodeURIComponent(pair[0] || '').toLowerCase();
                for (var j = 0; j < names.length; j++) {
                    if (key === names[j].toLowerCase()) {
                        return decodeURIComponent((pair[1] || '').replace(/\+/g, ' '));
                    }
                }
            }
            return '';
        }

        function buildMessageFlightRequest(exportAll) {
            var dateText = $.trim($('#txtMessageDate').val());
            if (!parseMessageDate(dateText)) {
                alert('Ngày điện văn phải đúng định dạng DD-MM-YYYY.');
                $('#txtMessageDate').focus();
                return null;
            }

            return {
                P_DATE: dateText,
                P_MESS_TYPE: $('#ddlMessageType').val(),
                P_THONG_BAO_MODE: parseInt($('#ddlThongBaoMode').val(), 10) || 0,
                P_PART_NO: 0,
                P_FROM_AIRP: $.trim($('#txtFromAir').val()),
                P_TO_AIRP: $.trim($('#txtToAir').val()),
                P_OPER_ID: $.trim($('#txtOper').val()),
                P_PAGESIZE: exportAll ? 100000 : (parseInt($('#ddlPageSize').val(), 10) || 100),
                P_PAGEINDEX: exportAll ? 0 : messageFlightPageIndex
            };
        }

        function setMessageFlightLoading(loading) {
            messageFlightLoading = loading;
            $('#btnSearchMessageFlights, #btnExportMessageFlights').prop('disabled', loading);
            if (loading) {
                $('#tblMessageFlights tbody').html(
                    '<tr><td colspan="14" class="message-flight-loading">Đang tải dữ liệu...</td></tr>'
                );
            }
        }

        function renderMessageFlightRows(rows) {
            if (!rows || rows.length === 0) {
                $('#tblMessageFlights tbody').html(
                    '<tr><td colspan="14" class="message-flight-empty">Không có chuyến bay phù hợp.</td></tr>'
                );
                return;
            }

            var html = [];
            $.each(rows, function (_, row) {
                html.push('<tr data-flight-id="' + escapeMessageFlightHtml(row.FLIGHT_ID) + '">');
                html.push('<td>' + escapeMessageFlightHtml(row.RNUM) + '</td>');
                //html.push('<td>' + escapeMessageFlightHtml(row.MESSAGE_ID) + '</td>');
                //html.push('<td>' + escapeMessageFlightHtml(row.PART_NO) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.OPER_ID) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.FLIGHTNBR) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.REGISTRATION) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.CRAFT_NAME) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.PURPOSE) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.PERMTYPE) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.FROM_AIRP) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.TO_AIRP) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(formatMessageFlightDate(row.FLIGHTDATE)) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.ETD) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.ETA) + '</td>');
                //html.push('<td>' + escapeMessageFlightHtml(row.FLIGHT_ID) + '</td>');
                html.push('<td class="text-left" title="' + escapeMessageFlightHtml(row.VIA) + '">' + escapeMessageFlightHtml(row.VIA) + '</td>');
                html.push('<td class="text-left" title="' + escapeMessageFlightHtml(row.REMARK) + '">' + escapeMessageFlightHtml(row.REMARK) + '</td>');
                html.push('</tr>');
            });

            $('#tblMessageFlights tbody').html(html.join(''));
        }

        function updateMessageFlightPager() {
            var pageSize = parseInt($('#ddlPageSize').val(), 10) || 100;
            var totalPages = Math.max(Math.ceil(messageFlightTotalRecords / pageSize), 1);
            var currentPage = Math.min(messageFlightPageIndex + 1, totalPages);

            $('#messageFlightPageInfo').text('Trang ' + currentPage + '/' + totalPages);
            $('#btnMessageFlightPrev').prop('disabled', messageFlightLoading || messageFlightPageIndex <= 0);
            $('#btnMessageFlightNext').prop(
                'disabled',
                messageFlightLoading || messageFlightPageIndex + 1 >= totalPages
            );
        }

        function updateMessageFlightCriteria(request) {
            var contentMode = $('#ddlThongBaoMode option:selected').text();
            $('#messageFlightCriteria').text(
                request.P_DATE + ' - ' + request.P_MESS_TYPE
                + ' - ' + contentMode
            );
        }

        function loadMessageFlights() {
            if (messageFlightLoading) return;

            var request = buildMessageFlightRequest(false);
            if (!request) return;

            setMessageFlightLoading(true);
            updateMessageFlightCriteria(request);

            $.ajax({
                method: 'PUT',
                url: messageFlightUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request)
            }).done(function (data) {
                if (!data || String(data.Code) !== '00') {
                    var apiMessage = data && data.Message
                        ? data.Message
                        : 'API không trả về kết quả hợp lệ.';

                    console.error('[GET_FLIGHTS_ON_MESSAGE] API error:', data);
                    messageFlightTotalRecords = 0;
                    $('#tblMessageFlights tbody').html(
                        '<tr><td colspan="14" class="message-flight-empty">'
                        + escapeMessageFlightHtml(apiMessage)
                        + '</td></tr>'
                    );
                    $('#messageFlightTotal').text('Tổng số: 0');
                    alert('Không thể tải dữ liệu: ' + apiMessage);
                    return;
                }

                var rows = data.ListValue || [];

                messageFlightTotalRecords = rows.length > 0
                    ? parseInt(rows[0].SUMRECORD, 10) || 0
                    : 0;

                renderMessageFlightRows(rows);
                $('#messageFlightTotal').text('Tổng số: ' + messageFlightTotalRecords);
            }).fail(function (xhr) {
                console.error(
                    '[GET_FLIGHTS_ON_MESSAGE] Request failed:',
                    xhr.responseJSON || xhr.responseText
                );
                messageFlightTotalRecords = 0;
                renderMessageFlightRows([]);
                $('#messageFlightTotal').text('Tổng số: 0');
                alert('Không thể tải danh sách chuyến bay trong điện văn.');
            }).always(function () {
                setMessageFlightLoading(false);
                updateMessageFlightPager();
            });
        }

        function searchMessageFlights() {
            messageFlightPageIndex = 0;
            loadMessageFlights();
        }

        function changeMessageFlightPage(direction) {
            if (messageFlightLoading) return;
            var nextPage = messageFlightPageIndex + direction;
            if (nextPage < 0) return;
            messageFlightPageIndex = nextPage;
            loadMessageFlights();
        }

        function buildMessageFlightExportTable(rows) {
            var html = [
                '<table border="1"><thead><tr>',
                '<th>NO</th><th>MESSAGE ID</th><th>PART</th><th>OPER</th>',
                '<th>CALLSIGN</th><th>REGISTRATION</th><th>CRAFT</th>',
                '<th>PURPOSE</th><th>PERM TYPE</th><th>FROM</th><th>TO</th>',
                '<th>FLIGHT DATE</th><th>ETD</th><th>ETA</th><th>FLIGHT ID</th>',
                '<th>VIA</th><th>REMARK</th></tr></thead><tbody>'
            ];

            $.each(rows, function (_, row) {
                html.push('<tr>');
                html.push('<td>' + escapeMessageFlightHtml(row.RNUM) + '</td>');
                //html.push('<td>' + escapeMessageFlightHtml(row.MESSAGE_ID) + '</td>');
                //html.push('<td>' + escapeMessageFlightHtml(row.PART_NO) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.OPER_ID) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.FLIGHTNBR) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.REGISTRATION) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.CRAFT_NAME) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.PURPOSE) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.PERMTYPE) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.FROM_AIRP) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.TO_AIRP) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(formatMessageFlightDate(row.FLIGHTDATE)) + '</td>');
                html.push('<td>\'' + escapeMessageFlightHtml(row.ETD) + '</td>');
                html.push('<td>\'' + escapeMessageFlightHtml(row.ETA) + '</td>');
                //html.push('<td>' + escapeMessageFlightHtml(row.FLIGHT_ID) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.VIA) + '</td>');
                html.push('<td>' + escapeMessageFlightHtml(row.REMARK) + '</td>');
                html.push('</tr>');
            });

            html.push('</tbody></table>');
            return html.join('');
        }

        function downloadMessageFlightExcel(rows, request) {
            var content = '\ufeff' + buildMessageFlightExportTable(rows);
            var blob = new Blob([content], {
                type: 'application/vnd.ms-excel;charset=utf-8'
            });
            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
            var safeType = request.P_MESS_TYPE.replace(/[^0-9A-Za-z]+/g, '_');

            link.href = url;
            link.download = 'Flights_On_Message_'
                + safeType + '_' + request.P_DATE.replace(/-/g, '') + '.xls';
            link.style.display = 'none';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
        }

        function exportMessageFlights() {
            if (messageFlightLoading) return;

            var request = buildMessageFlightRequest(true);
            if (!request) return;

            $('#btnExportMessageFlights').prop('disabled', true);
            $.ajax({
                method: 'PUT',
                url: messageFlightUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request)
            }).done(function (data) {
                if (!data || String(data.Code) !== '00') {
                    var apiMessage = data && data.Message
                        ? data.Message
                        : 'API không trả về kết quả hợp lệ.';
                    console.error('[EXPORT FLIGHTS ON MESSAGE] API error:', data);
                    alert('Không thể Export Excel: ' + apiMessage);
                    return;
                }

                var rows = data.ListValue || [];

                if (rows.length === 0) {
                    alert('Không có dữ liệu để Export Excel.');
                    return;
                }

                downloadMessageFlightExcel(rows, request);
            }).fail(function (xhr) {
                console.error(
                    '[EXPORT FLIGHTS ON MESSAGE] Request failed:',
                    xhr.responseJSON || xhr.responseText
                );
                alert('Không thể Export Excel danh sách chuyến bay.');
            }).always(function () {
                $('#btnExportMessageFlights').prop('disabled', false);
            });
        }

        $(function () {
            var queryDate = getMessageFlightQueryValue(['FlightDate', 'Date']);
            var queryType = getMessageFlightQueryValue(['MessType', 'MessageType']);
            var queryThongBaoMode = getMessageFlightQueryValue([
                'ThongBaoMode',
                'NotificationMode'
            ]);

            $('#txtMessageDate').val(
                queryDate || dateFormat(new Date(), 'dd-mm-yyyy')
            ).multiDate();

            syncNativeMessageDatePicker();
            $('#nativeMessageDatePicker').on('change', function () {
                var parts = String(this.value || '').split('-');
                if (parts.length !== 3) return;

                $('#txtMessageDate').val(
                    parts[2] + '-' + parts[1] + '-' + parts[0]
                ).trigger('change');
            });

            if (queryType) {
                var existingType = $('#ddlMessageType option').filter(function () {
                    return this.value === queryType;
                });

                if (existingType.length === 0) {
                    $('#ddlMessageType').append(
                        $('<option/>').val(queryType).text(queryType)
                    );
                }
                $('#ddlMessageType').val(queryType);
            }

            if (/^[012]$/.test(queryThongBaoMode)) {
                $('#ddlThongBaoMode').val(queryThongBaoMode);
            }

            $('#ddlPageSize').on('change', searchMessageFlights);
            $('#txtMessageDate, #txtFromAir, #txtToAir, #txtOper').on('keydown', function (event) {
                if (event.which === 13) searchMessageFlights();
            });

            if (queryDate || queryType) {
                searchMessageFlights();
            }
        });
    </script>
</asp:Content>
