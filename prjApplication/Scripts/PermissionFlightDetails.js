(function (window, $) {
    'use strict';

    function text(value) {
        return value === null || value === undefined ? '' : String(value);
    }

    function formatDate(value) {
        var raw = text(value).trim();
        var matched = /^(\d{4})-(\d{2})-(\d{2})/.exec(raw);
        return matched ? matched[3] + '-' + matched[2] + '-' + matched[1] : raw;
    }

    function formatScDays(row) {
        var result = '';
        for (var day = 1; day <= 7; day++) {
            var value = text(row['DAY' + day]).trim();
            if (value !== '' && value !== '0') result += day;
        }
        return result;
    }

    function getColumns(permissionType) {
        var commonStart = [
            { title: 'NO', value: function (row, index) { return row.RNUM || index + 1; } },
            { title: 'CALLSIGN', key: 'FLIGHTNBR' },
            { title: 'REGISTRATION', key: 'REGISTRATION' },
            { title: 'FROM', key: 'FROM_AIRP' },
            { title: 'TO', key: 'TO_AIRP' },
            { title: 'ETD', key: 'ETD' },
            { title: 'ETA', key: 'ETA' }
        ];
        var commonEnd = [
            { title: 'CRAFT', key: 'CRAFT_NAME' },
            { title: 'PURPOSE', key: 'PURPOSE_ID' },
            { title: 'MTOW', key: 'MTOW' },
            { title: 'VIA', key: 'VIA' },
            { title: 'REMARK', key: 'REMARK' },
            { title: 'STATUS', key: 'STATUS' },
            { title: 'LAST USER', key: 'LASTUSER' }
        ];

        if (permissionType === 'NO') {
            return commonStart.concat([
                { title: 'DAY/DATE', key: 'DAYSFLIGHT' }
            ], commonEnd);
        }

        return commonStart.concat([
            { title: 'DAY/DATE', value: formatScDays },
            { title: 'BEGIN DATE', key: 'BEGINDATE', formatter: formatDate },
            { title: 'END DATE', key: 'ENDDATE', formatter: formatDate }
        ], commonEnd);
    }

    function renderHeader(table, columns) {
        var row = document.createElement('tr');
        for (var index = 0; index < columns.length; index++) {
            var cell = document.createElement('th');
            cell.textContent = columns[index].title;
            row.appendChild(cell);
        }
        table.querySelector('thead').appendChild(row);
    }

    function renderMessage(table, columnCount, message, cssClass) {
        var row = document.createElement('tr');
        var cell = document.createElement('td');
        cell.colSpan = columnCount;
        cell.className = cssClass || 'permission-flight-details__message';
        cell.textContent = message;
        row.appendChild(cell);
        table.querySelector('tbody').appendChild(row);
    }

    function renderRows(table, rows, columns) {
        var body = table.querySelector('tbody');
        var fragment = document.createDocumentFragment();

        for (var rowIndex = 0; rowIndex < rows.length; rowIndex++) {
            var rowData = rows[rowIndex];
            var row = document.createElement('tr');
            for (var columnIndex = 0; columnIndex < columns.length; columnIndex++) {
                var column = columns[columnIndex];
                var rawValue = column.value
                    ? column.value(rowData, rowIndex)
                    : rowData[column.key];
                var cell = document.createElement('td');
                cell.textContent = column.formatter
                    ? column.formatter(rawValue)
                    : text(rawValue);
                row.appendChild(cell);
            }
            fragment.appendChild(row);
        }

        body.appendChild(fragment);
    }

    function unwrapResponse(response, usePageMethod) {
        var payload = response;
        if (usePageMethod && payload && payload.d !== undefined) payload = payload.d;
        if (typeof payload === 'string') {
            try {
                payload = JSON.parse(payload);
            } catch (ignore) {
                payload = null;
            }
        }

        if (usePageMethod) {
            return payload && payload.Rows ? payload.Rows : [];
        }
        return payload && payload.ListValue ? payload.ListValue : [];
    }

    function init(options) {
        var table = document.getElementById(options.tableId);
        var total = document.getElementById(options.totalId);
        var permissionType = text(options.permissionType).toUpperCase() === 'NO' ? 'NO' : 'SC';
        var permissionId = parseInt(options.permissionId, 10);
        var flightNbr = text(options.flightNbr).trim().toUpperCase();
        if (!table) return;

        var columns = getColumns(permissionType);
        table.querySelector('thead').innerHTML = '';
        table.querySelector('tbody').innerHTML = '';
        renderHeader(table, columns);

        if (isNaN(permissionId) || permissionId <= 0) {
            renderMessage(table, columns.length, 'Permission ID is invalid.', 'permission-flight-details__error');
            if (total) total.textContent = 'TOTAL: 0';
            return;
        }

        renderMessage(table, columns.length, 'Loading flight details...');

        var endpoint = permissionType === 'NO'
            ? 'api/PermDetailNo/GetBySearch'
            : 'api/PermDetailSc/GetBySearch';
        var pageMethod = text(options.pageMethod).trim();
        var usePageMethod = pageMethod.length > 0;
        var payload = {
            PERM_ID: permissionId,
            FLIGHTNBR: flightNbr,
            PageSize: 10000,
            PageIndex: 0,
            rStart: 0,
            rFinish: 10000
        };

        $.ajax({
            method: usePageMethod ? 'POST' : 'PUT',
            url: usePageMethod
                ? pageMethod
                : text(options.apiBase).replace(/\/?$/, '/') + endpoint,
            data: JSON.stringify(usePageMethod
                ? { permissionId: permissionId, flightNbr: flightNbr }
                : payload),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        }).done(function (response) {
            var rows = $.grep(unwrapResponse(response, usePageMethod), function (item) {
                return item !== null;
            });
            if (flightNbr) {
                rows = $.grep(rows, function (item) {
                    return text(item.FLIGHTNBR).trim().toUpperCase() === flightNbr;
                });
            }
            table.querySelector('tbody').innerHTML = '';

            if (!rows.length) {
                renderMessage(
                    table,
                    columns.length,
                    flightNbr
                        ? 'No flight details found for callsign ' + flightNbr + '.'
                        : 'No flight details found.'
                );
                if (total) total.textContent = 'TOTAL: 0';
                return;
            }

            renderRows(table, rows, columns);
            var recordTotal = flightNbr
                ? rows.length
                : (rows[0].Record_Sum || rows[0].RECORD_SUM || rows.length);
            if (total) {
                total.textContent = 'TOTAL: ' + recordTotal
                    + (recordTotal > rows.length ? ' (DISPLAYING ' + rows.length + ')' : '');
            }
        }).fail(function (xhr) {
            table.querySelector('tbody').innerHTML = '';
            var message = 'Unable to load flight details.';
            if (xhr && xhr.responseJSON && xhr.responseJSON.Message) {
                message += ' ' + xhr.responseJSON.Message;
            }
            renderMessage(table, columns.length, message, 'permission-flight-details__error');
            if (total) total.textContent = 'TOTAL: 0';
        });
    }

    window.PermissionFlightDetails = {
        init: init
    };
})(window, window.jQuery);
