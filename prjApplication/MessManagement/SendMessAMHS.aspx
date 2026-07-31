<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="SendMessAMHS.aspx.cs" Inherits="prjApplication.MessManagement.SendMessAMHS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .amhs-outbox-page {
            padding: 8px 12px 16px;
            color: #173d5b;
        }

        .amhs-outbox-filter {
            display: flex;
            flex-wrap: nowrap;
            align-items: flex-end;
            gap: 8px;
            overflow-x: auto;
            padding: 10px 12px;
            border: 1px solid #c8dced;
            border-radius: 8px;
            background: #f7fbff;
        }

        .amhs-outbox-field {
            display: flex;
            flex: 0 0 auto;
            flex-direction: column;
            gap: 3px;
        }

        .amhs-outbox-field-content {
            flex: 1 1 360px;
            min-width: 260px;
        }

        .amhs-outbox-field label {
            margin: 0;
            color: #15527f;
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
        }

        .amhs-outbox-field input,
        .amhs-outbox-field select {
            height: 34px;
            padding: 5px 9px;
            border: 1px solid #b9d3e7;
            border-radius: 5px;
            color: #163b59;
            background: #fff;
        }

        .amhs-date-control {
            position: relative;
            display: flex;
            width: 132px;
        }

        .amhs-date-text {
            width: 102px;
            border-radius: 5px 0 0 5px !important;
        }

        .amhs-date-button {
            width: 30px;
            height: 34px;
            padding: 0;
            border: 1px solid #b9d3e7;
            border-left: 0;
            border-radius: 0 5px 5px 0;
            color: #15527f;
            background: #fff;
        }

        .amhs-date-button:hover,
        .amhs-date-button:focus {
            color: #fff;
            background: #337ab7;
        }

        .amhs-native-date {
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

        #txtAmhsContent { width: 100%; min-width: 260px; }
        #ddlAmhsPageSize { width: 76px; }

        .amhs-outbox-actions {
            display: flex;
            flex: 0 0 auto;
            gap: 6px;
        }

        .amhs-outbox-actions .btn {
            min-width: 110px;
            height: 34px;
            font-weight: 700;
            text-transform: uppercase;
        }

        .amhs-outbox-summary {
            display: flex;
            align-items: center;
            justify-content: space-between;
            min-height: 32px;
            padding: 5px 2px;
        }

        #amhsOutboxTotal {
            font-weight: 700;
            text-transform: uppercase;
        }

        .amhs-outbox-table-wrap {
            max-height: 560px;
            overflow: auto;
            border: 1px solid #acd0e8;
            background: #fff;
        }

        #tblAmhsOutbox {
            width: 100%;
            min-width: 1180px;
            margin: 0;
            table-layout: fixed;
        }

        #tblAmhsOutbox thead {
            position: sticky;
            top: 0;
            z-index: 20;
            color: #fff;
            background: #337ab7;
        }

        #tblAmhsOutbox th,
        #tblAmhsOutbox td {
            padding: 6px 5px;
            border-color: #bdd5e6;
            vertical-align: middle;
            text-align: center;
        }

        #tblAmhsOutbox th {
            height: 36px;
            color: #fff !important;
            font-size: 11px;
            text-transform: uppercase;
            background: #337ab7;
        }

        #tblAmhsOutbox tbody tr:nth-child(even) { background: #f5faff; }
        #tblAmhsOutbox tbody tr:hover { background: #dff1ff; }

        #tblAmhsOutbox .amhs-text-left { text-align: left; }

        #tblAmhsOutbox .amhs-cell-ellipsis {
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        #tblAmhsOutbox .amhs-content-cell {
            max-height: 96px;
            overflow: auto;
            white-space: pre-wrap;
            word-break: break-word;
        }

        .amhs-outbox-empty,
        .amhs-outbox-loading {
            height: 72px;
            color: #6f8290 !important;
            font-style: italic;
            text-align: center !important;
        }

        .amhs-outbox-pager {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            margin-top: 9px;
        }

        .amhs-outbox-pager .btn { min-width: 42px; }

        #amhsOutboxPageInfo {
            min-width: 110px;
            text-align: center;
            font-weight: 700;
        }
    </style>

    <div class="amhs-outbox-page">
        <div class="amhs-outbox-filter">
            <div class="amhs-outbox-field">
                <label for="txtAmhsFromDate">Từ ngày</label>
                <div class="amhs-date-control">
                    <input id="txtAmhsFromDate" class="amhs-date-text" type="text" maxlength="10" placeholder="DD-MM-YYYY" />
                    <button type="button" class="amhs-date-button" title="Chọn từ ngày" onclick="openAmhsDatePicker('from')">
                        <i class="fa fa-calendar"></i>
                    </button>
                    <input id="nativeAmhsFromDate" class="amhs-native-date" type="date" tabindex="-1" aria-hidden="true" />
                </div>
            </div>

            <div class="amhs-outbox-field">
                <label for="txtAmhsToDate">Đến ngày</label>
                <div class="amhs-date-control">
                    <input id="txtAmhsToDate" class="amhs-date-text" type="text" maxlength="10" placeholder="DD-MM-YYYY" />
                    <button type="button" class="amhs-date-button" title="Chọn đến ngày" onclick="openAmhsDatePicker('to')">
                        <i class="fa fa-calendar"></i>
                    </button>
                    <input id="nativeAmhsToDate" class="amhs-native-date" type="date" tabindex="-1" aria-hidden="true" />
                </div>
            </div>

            <div class="amhs-outbox-field amhs-outbox-field-content">
                <label for="txtAmhsContent">Nội dung</label>
                <input id="txtAmhsContent" type="text" maxlength="4000" placeholder="Nhập nội dung cần tìm" />
            </div>

            <div class="amhs-outbox-field">
                <label for="ddlAmhsPageSize">Số dòng</label>
                <select id="ddlAmhsPageSize">
                    <option value="50">50</option>
                    <option value="100" selected="selected">100</option>
                    <option value="200">200</option>
                    <option value="500">500</option>
                </select>
            </div>

            <div class="amhs-outbox-actions">
                <button type="button" id="btnSearchAmhsOutbox" class="btn btn-sm btn-primary" onclick="searchAmhsOutbox()">
                    <i class="fa fa-search"></i> Search
                </button>
                <button type="button" id="btnExportAmhsOutbox" class="btn btn-sm btn-success" onclick="exportAmhsOutbox()">
                    <i class="fa fa-file-excel-o"></i> Export Excel
                </button>
            </div>
        </div>

        <div class="amhs-outbox-summary">
            <span id="amhsOutboxTotal">Tổng số: 0</span>
            <span id="amhsOutboxCriteria"></span>
        </div>

        <div class="amhs-outbox-table-wrap">
            <table id="tblAmhsOutbox" class="table table-bordered">
                <colgroup>
                    <col style="width: 48px;" />
                    <col style="width: 65px;" />
                    <col style="width: 138px;" />
                    <col style="width: 70px;" />
                    <col style="width: 225px;" />
                    <col style="width: 120px;" />
                    <col style="width: 78px;" />
                    <col style="width: 70px;" />
                    <col style="width: 360px;" />
                </colgroup>
                <thead>
                    <tr>
                        <th>No</th>
                        <th>ID</th>
                        <th>Thời gian</th>
                        <th>Ưu tiên</th>
                        <th>Địa chỉ gửi</th>
                        <th>Tiêu đề</th>
                        <th>Số địa chỉ</th>
                        <th>Đính kèm</th>
                        <th>Nội dung</th>
                    </tr>
                </thead>
                <tbody>
                    <tr><td colspan="9" class="amhs-outbox-empty">Đang tải dữ liệu...</td></tr>
                </tbody>
            </table>
        </div>

        <div class="amhs-outbox-pager">
            <button type="button" id="btnAmhsOutboxPrev" class="btn btn-sm btn-default" onclick="changeAmhsOutboxPage(-1)" disabled="disabled">&lt;</button>
            <span id="amhsOutboxPageInfo">Trang 1/1</span>
            <button type="button" id="btnAmhsOutboxNext" class="btn btn-sm btn-default" onclick="changeAmhsOutboxPage(1)" disabled="disabled">&gt;</button>
        </div>
    </div>

    <script>
        var amhsOutboxPageIndex = 0;
        var amhsOutboxTotalRecords = 0;
        var amhsOutboxLoading = false;
        var amhsOutboxUrl = (window.reportApiBase || '')
            + 'api/ApiExtension/ExcuteTable?packageName=AMHS_OUTBOX_PKG&storeName=GET_OUTBOX_MESSAGES';

        function escapeAmhsHtml(value) {
            return $('<div/>').text(value == null ? '' : String(value)).html();
        }

        function parseAmhsDate(value) {
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

        function formatAmhsDate(date) {
            return ('0' + date.getDate()).slice(-2)
                + '-' + ('0' + (date.getMonth() + 1)).slice(-2)
                + '-' + date.getFullYear();
        }

        function syncAmhsNativeDate(kind) {
            var textId = kind === 'from' ? '#txtAmhsFromDate' : '#txtAmhsToDate';
            var nativeId = kind === 'from' ? '#nativeAmhsFromDate' : '#nativeAmhsToDate';
            var date = parseAmhsDate($(textId).val());
            if (!date) return;

            $(nativeId).val(
                date.getFullYear()
                + '-' + ('0' + (date.getMonth() + 1)).slice(-2)
                + '-' + ('0' + date.getDate()).slice(-2)
            );
        }

        function openAmhsDatePicker(kind) {
            syncAmhsNativeDate(kind);
            var pickerId = kind === 'from' ? 'nativeAmhsFromDate' : 'nativeAmhsToDate';
            var picker = document.getElementById(pickerId);
            if (!picker) return;

            if (typeof picker.showPicker === 'function') {
                picker.showPicker();
            } else {
                picker.focus();
                picker.click();
            }
        }

        function formatAmhsTimestamp(value) {
            if (!value) return '';

            var text = String(value);
            var dotNetDate = /\/Date\((-?\d+)/.exec(text);
            var date = dotNetDate
                ? new Date(parseInt(dotNetDate[1], 10))
                : new Date(text);

            if (isNaN(date.getTime())) return text;

            return ('0' + date.getDate()).slice(-2)
                + '-' + ('0' + (date.getMonth() + 1)).slice(-2)
                + '-' + date.getFullYear()
                + ' ' + ('0' + date.getHours()).slice(-2)
                + ':' + ('0' + date.getMinutes()).slice(-2)
                + ':' + ('0' + date.getSeconds()).slice(-2);
        }

        function getAmhsPriority(value) {
            var priority = parseInt(value, 10);
            if (priority === 0) return 'FF';
            if (priority === 1) return 'GG';
            return value == null ? '' : String(value);
        }

        function buildAmhsOutboxRequest(exportAll) {
            var fromText = $.trim($('#txtAmhsFromDate').val());
            var toText = $.trim($('#txtAmhsToDate').val());
            var fromDate = parseAmhsDate(fromText);
            var toDate = parseAmhsDate(toText);

            if (!fromDate) {
                alert('Từ ngày phải đúng định dạng DD-MM-YYYY.');
                $('#txtAmhsFromDate').focus();
                return null;
            }

            if (!toDate) {
                alert('Đến ngày phải đúng định dạng DD-MM-YYYY.');
                $('#txtAmhsToDate').focus();
                return null;
            }

            if (fromDate.getTime() > toDate.getTime()) {
                alert('Từ ngày không được lớn hơn Đến ngày.');
                $('#txtAmhsFromDate').focus();
                return null;
            }

            return {
                P_FROM_DATE: fromText,
                P_TO_DATE: toText,
                P_CONTENT: $.trim($('#txtAmhsContent').val()),
                P_PAGESIZE: exportAll ? 100000 : (parseInt($('#ddlAmhsPageSize').val(), 10) || 100),
                P_PAGEINDEX: exportAll ? 0 : amhsOutboxPageIndex
            };
        }

        function setAmhsOutboxLoading(loading) {
            amhsOutboxLoading = loading;
            $('#btnSearchAmhsOutbox, #btnExportAmhsOutbox').prop('disabled', loading);

            if (loading) {
                $('#tblAmhsOutbox tbody').html(
                    '<tr><td colspan="9" class="amhs-outbox-loading">Đang tải dữ liệu...</td></tr>'
                );
            }
        }

        function renderAmhsOutboxRows(rows) {
            if (!rows || rows.length === 0) {
                $('#tblAmhsOutbox tbody').html(
                    '<tr><td colspan="9" class="amhs-outbox-empty">Không có điện văn AMHS phù hợp.</td></tr>'
                );
                return;
            }

            var html = [];
            $.each(rows, function (_, row) {
                var content = row.CONTENT == null ? '' : String(row.CONTENT);
                var fromAddress = row.FROM_ADDRESS == null ? '' : String(row.FROM_ADDRESS);

                html.push('<tr data-outbox-id="' + escapeAmhsHtml(row.ID) + '">');
                html.push('<td>' + escapeAmhsHtml(row.RNUM) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.ID) + '</td>');
                html.push('<td>' + escapeAmhsHtml(formatAmhsTimestamp(row.SENT_TIME)) + '</td>');
                html.push('<td>' + escapeAmhsHtml(getAmhsPriority(row.PRIORITY)) + '</td>');
                html.push('<td class="amhs-text-left amhs-cell-ellipsis" title="'
                    + escapeAmhsHtml(fromAddress) + '">' + escapeAmhsHtml(fromAddress) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.SUBJECT) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.ADDRESS_COUNT) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.ATTACH) + '</td>');
                html.push('<td class="amhs-text-left"><div class="amhs-content-cell">'
                    + escapeAmhsHtml(content) + '</div></td>');
                html.push('</tr>');
            });

            $('#tblAmhsOutbox tbody').html(html.join(''));
        }

        function updateAmhsOutboxPager() {
            var pageSize = parseInt($('#ddlAmhsPageSize').val(), 10) || 100;
            var totalPages = Math.max(Math.ceil(amhsOutboxTotalRecords / pageSize), 1);
            var currentPage = Math.min(amhsOutboxPageIndex + 1, totalPages);

            $('#amhsOutboxPageInfo').text('Trang ' + currentPage + '/' + totalPages);
            $('#btnAmhsOutboxPrev').prop('disabled', amhsOutboxLoading || amhsOutboxPageIndex <= 0);
            $('#btnAmhsOutboxNext').prop(
                'disabled',
                amhsOutboxLoading || amhsOutboxPageIndex + 1 >= totalPages
            );
        }

        function loadAmhsOutbox() {
            if (amhsOutboxLoading) return;

            var request = buildAmhsOutboxRequest(false);
            if (!request) return;

            setAmhsOutboxLoading(true);
            $('#amhsOutboxCriteria').text(
                request.P_FROM_DATE + ' - ' + request.P_TO_DATE
                + (request.P_CONTENT ? ' | Nội dung: ' + request.P_CONTENT : '')
            );

            $.ajax({
                method: 'PUT',
                url: amhsOutboxUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request)
            }).done(function (data) {
                if (!data || String(data.Code) !== '00') {
                    var apiMessage = data && data.Message
                        ? data.Message
                        : 'API không trả về kết quả hợp lệ.';

                    console.error('[GET_OUTBOX_MESSAGES] API error:', data);
                    amhsOutboxTotalRecords = 0;
                    renderAmhsOutboxRows([]);
                    $('#amhsOutboxTotal').text('Tổng số: 0');
                    alert('Không thể tải danh sách điện văn AMHS: ' + apiMessage);
                    return;
                }

                var rows = data.ListValue || [];
                amhsOutboxTotalRecords = rows.length > 0
                    ? parseInt(rows[0].SUMRECORD, 10) || 0
                    : 0;

                renderAmhsOutboxRows(rows);
                $('#amhsOutboxTotal').text('Tổng số: ' + amhsOutboxTotalRecords);
            }).fail(function (xhr) {
                console.error(
                    '[GET_OUTBOX_MESSAGES] Request failed:',
                    xhr.responseJSON || xhr.responseText
                );
                amhsOutboxTotalRecords = 0;
                renderAmhsOutboxRows([]);
                $('#amhsOutboxTotal').text('Tổng số: 0');
                alert('Không thể tải danh sách điện văn AMHS.');
            }).always(function () {
                setAmhsOutboxLoading(false);
                updateAmhsOutboxPager();
            });
        }

        function searchAmhsOutbox() {
            amhsOutboxPageIndex = 0;
            loadAmhsOutbox();
        }

        function changeAmhsOutboxPage(direction) {
            if (amhsOutboxLoading) return;
            var nextPage = amhsOutboxPageIndex + direction;
            if (nextPage < 0) return;
            amhsOutboxPageIndex = nextPage;
            loadAmhsOutbox();
        }

        function buildAmhsOutboxExportTable(rows) {
            var html = [
                '<table border="1"><thead><tr>',
                '<th>NO</th><th>ID</th><th>TIME</th><th>PRIORITY</th>',
                '<th>FROM ADDRESS</th><th>SUBJECT</th><th>ADDRESS COUNT</th>',
                '<th>ATTACH</th><th>CONTENT</th></tr></thead><tbody>'
            ];

            $.each(rows, function (_, row) {
                html.push('<tr>');
                html.push('<td>' + escapeAmhsHtml(row.RNUM) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.ID) + '</td>');
                html.push('<td>' + escapeAmhsHtml(formatAmhsTimestamp(row.SENT_TIME)) + '</td>');
                html.push('<td>' + escapeAmhsHtml(getAmhsPriority(row.PRIORITY)) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.FROM_ADDRESS) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.SUBJECT) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.ADDRESS_COUNT) + '</td>');
                html.push('<td>' + escapeAmhsHtml(row.ATTACH) + '</td>');
                html.push('<td style="white-space:pre-wrap">' + escapeAmhsHtml(row.CONTENT) + '</td>');
                html.push('</tr>');
            });

            html.push('</tbody></table>');
            return html.join('');
        }

        function downloadAmhsOutboxExcel(rows, request) {
            var content = '\ufeff' + buildAmhsOutboxExportTable(rows);
            var blob = new Blob([content], {
                type: 'application/vnd.ms-excel;charset=utf-8'
            });
            var downloadUrl = window.URL.createObjectURL(blob);
            var link = document.createElement('a');

            link.href = downloadUrl;
            link.download = 'AMHS_Outbox_'
                + request.P_FROM_DATE.replace(/-/g, '')
                + '_' + request.P_TO_DATE.replace(/-/g, '') + '.xls';
            link.style.display = 'none';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl);
        }

        function exportAmhsOutbox() {
            if (amhsOutboxLoading) return;

            var request = buildAmhsOutboxRequest(true);
            if (!request) return;

            $('#btnExportAmhsOutbox').prop('disabled', true);
            $.ajax({
                method: 'PUT',
                url: amhsOutboxUrl,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request)
            }).done(function (data) {
                if (!data || String(data.Code) !== '00') {
                    var apiMessage = data && data.Message
                        ? data.Message
                        : 'API không trả về kết quả hợp lệ.';
                    console.error('[EXPORT AMHS OUTBOX] API error:', data);
                    alert('Không thể Export Excel: ' + apiMessage);
                    return;
                }

                var rows = data.ListValue || [];
                if (rows.length === 0) {
                    alert('Không có dữ liệu để Export Excel.');
                    return;
                }

                downloadAmhsOutboxExcel(rows, request);
            }).fail(function (xhr) {
                console.error(
                    '[EXPORT AMHS OUTBOX] Request failed:',
                    xhr.responseJSON || xhr.responseText
                );
                alert('Không thể Export Excel danh sách điện văn AMHS.');
            }).always(function () {
                $('#btnExportAmhsOutbox').prop('disabled', false);
            });
        }

        $(function () {
            var today = formatAmhsDate(new Date());
            $('#txtAmhsFromDate, #txtAmhsToDate').val(today);

            $('#nativeAmhsFromDate, #nativeAmhsToDate').on('change', function () {
                var parts = String(this.value || '').split('-');
                if (parts.length !== 3) return;

                var target = this.id === 'nativeAmhsFromDate'
                    ? '#txtAmhsFromDate'
                    : '#txtAmhsToDate';
                $(target).val(parts[2] + '-' + parts[1] + '-' + parts[0]);
            });

            $('#ddlAmhsPageSize').on('change', searchAmhsOutbox);
            $('#txtAmhsFromDate, #txtAmhsToDate, #txtAmhsContent').on('keydown', function (event) {
                if (event.which === 13) searchAmhsOutbox();
            });

            searchAmhsOutbox();
        });
    </script>
</asp:Content>
