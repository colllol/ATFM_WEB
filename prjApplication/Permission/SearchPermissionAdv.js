(function () {
    'use strict';

    var pageSize = 100;
    var currentPage = 1;
    var permissions = [];
    var filteredPermissions = [];
    var lastDetailTrigger = null;
    var detailCache = {};
    var activePermissionDate = '';
    var activeFromTime = '';
    var activeToTime = '';
    var activeFromAirp = '';
    var activeToAirp = '';
    var activeVia = '';

    function post(method, data) {
        return fetch(window.location.pathname + '/' + method, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        }).then(function (response) {
            return response.text().then(function (text) {
                var result;
                try {
                    result = text ? JSON.parse(text) : null;
                } catch (error) {
                    throw new Error('HTTP ' + response.status + ': Máy chủ trả về dữ liệu không hợp lệ.');
                }

                if (!response.ok) {
                    throw new Error((result && (result.Message || result.message)) || ('HTTP ' + response.status));
                }
                return result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result;
            });
        }).then(function (payload) {
            if (!payload) throw new Error('Máy chủ không trả về dữ liệu.');
            if (payload.Code && payload.Code !== '00') {
                throw new Error(payload.Message || 'Không thể tải dữ liệu.');
            }
            return payload;
        });
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null || value === '' ? '-' : value;
        return node.innerHTML;
    }

    function isoToday() {
        var now = new Date();
        return now.getFullYear() + '-' +
            String(now.getMonth() + 1).padStart(2, '0') + '-' +
            String(now.getDate()).padStart(2, '0');
    }

    function formatTimeInput(input) {
        var digits = input.value.replace(/\D/g, '').slice(0, 4);
        input.value = digits.length > 2
            ? digits.slice(0, 2) + ':' + digits.slice(2)
            : digits;
    }

    function isValid24HourTime(value) {
        return /^([01]\d|2[0-3]):[0-5]\d$/.test(value);
    }

    function setSearchLoading(loading) {
        var button = document.getElementById('spaSearch');
        button.disabled = loading;
        button.innerHTML = loading
            ? '<i class="fa fa-spinner fa-spin"></i> Loading...'
            : '<i class="fa fa-search"></i> Search';
    }

    function setExportEnabled(enabled) {
        document.getElementById('spaExportExcel').disabled = !enabled;
    }

    function renderPermissions() {
        var body = document.getElementById('spaPermissionRows');
        var totalPages = Math.max(1, Math.ceil(filteredPermissions.length / pageSize));
        if (currentPage > totalPages) currentPage = totalPages;
        var start = (currentPage - 1) * pageSize;
        var rows = filteredPermissions.slice(start, start + pageSize);

        if (!rows.length) {
            body.innerHTML = '<tr><td colspan="9" class="spa-empty">Không có phép phù hợp.</td></tr>';
        } else {
            body.innerHTML = rows.map(function (item, index) {
                return '<tr>' +
                    '<td>' + (start + index + 1) + '</td>' +
                    '<td><div class="spa-perm-actions">' +
                    '<button type="button" class="spa-perm-link" data-source="' + esc(item.SourceType) +
                    '" data-perm-id="' + esc(item.PermId) + '">' + esc(item.PermNbr) + '</button>' +
                    '<button type="button" class="spa-inline-toggle" data-source="' + esc(item.SourceType) +
                    '" data-perm-id="' + esc(item.PermId) +
                    '" aria-expanded="false" title="Hiển thị chuyến bay ngay dưới dòng">' +
                    '<i class="fa fa-angle-double-down"></i></button></div></td>' +
                    '<td>' + esc(item.Author) + '</td>' +
                    '<td><span class="spa-badge spa-badge-type">' + esc(item.PType) + '</span></td>' +
                    '<td><span class="spa-badge spa-badge-source">' + esc(item.FType) + '</span></td>' +
                    '<td>' + esc(item.Number) + '</td>' +
                    '<td>' + esc(item.Version) + '</td>' +
                    '<td>' + esc(item.PermissionDate) + '</td>' +
                    '<td>' + esc(item.Oper) + '</td>' +
                    '</tr>';
            }).join('');
        }

        document.getElementById('spaTotal').textContent = filteredPermissions.length === permissions.length
            ? 'Tổng số: ' + permissions.length.toLocaleString('vi-VN')
            : 'Tổng số: ' + filteredPermissions.length.toLocaleString('vi-VN') +
                ' / ' + permissions.length.toLocaleString('vi-VN');
        document.getElementById('spaPager').hidden = filteredPermissions.length <= pageSize;
        document.getElementById('spaPageInfo').textContent = 'Trang ' + currentPage + '/' + totalPages;
        document.getElementById('spaPrev').disabled = currentPage <= 1;
        document.getElementById('spaNext').disabled = currentPage >= totalPages;

        Array.prototype.forEach.call(body.querySelectorAll('.spa-perm-link'), function (button) {
            button.addEventListener('click', function () {
                lastDetailTrigger = this;
                loadDetail(
                    this.getAttribute('data-source'),
                    Number(this.getAttribute('data-perm-id')),
                    this.textContent
                );
            });
        });
        Array.prototype.forEach.call(body.querySelectorAll('.spa-inline-toggle'), function (button) {
            button.addEventListener('click', function () { toggleInlineDetail(this); });
        });
    }

    function normalizeFilterValue(value) {
        return String(value == null ? '' : value).trim().toLocaleUpperCase('vi-VN');
    }

    function applyColumnFilters() {
        var filters = Array.prototype.map.call(
            document.querySelectorAll('[data-filter-field]'),
            function (input) {
                return {
                    field: input.getAttribute('data-filter-field'),
                    value: normalizeFilterValue(input.value)
                };
            }
        ).filter(function (filter) { return filter.value !== ''; });

        filteredPermissions = filters.length ? permissions.filter(function (item) {
            return filters.every(function (filter) {
                return normalizeFilterValue(item[filter.field]).indexOf(filter.value) !== -1;
            });
        }) : permissions.slice();

        currentPage = 1;
        renderPermissions();
    }

    function clearColumnFilters() {
        Array.prototype.forEach.call(document.querySelectorAll('[data-filter-field]'), function (input) {
            input.value = '';
        });
    }

    function search() {
        var dateInput = document.getElementById('spaPermissionDate');
        var fromTimeInput = document.getElementById('spaFromTime');
        var toTimeInput = document.getElementById('spaToTime');
        var fromAirpInput = document.getElementById('spaFromAirp');
        var toAirpInput = document.getElementById('spaToAirp');
        var viaInput = document.getElementById('spaVia');
        if (!dateInput.value) {
            alert('Vui lòng chọn ngày cấp phép.');
            dateInput.focus();
            return;
        }
        var fromTimeValue = fromTimeInput.value.trim();
        var toTimeValue = toTimeInput.value.trim();
        if ((fromTimeValue && !isValid24HourTime(fromTimeValue)) ||
            (toTimeValue && !isValid24HourTime(toTimeValue))) {
            alert('Khung giờ phải đúng định dạng 24h HH:mm, từ 00:00 đến 23:59. Có thể để trống nếu không lọc theo giờ.');
            (fromTimeValue && !isValid24HourTime(fromTimeValue) ? fromTimeInput : toTimeInput).focus();
            return;
        }
        if ((fromTimeValue || toTimeValue) &&
            (fromTimeValue || '00:00') > (toTimeValue || '23:59')) {
            alert('Từ giờ không được lớn hơn Đến giờ.');
            fromTimeInput.focus();
            return;
        }

        setSearchLoading(true);
        setExportEnabled(false);
        detailCache = {};
        document.getElementById('spaPermissionRows').innerHTML =
            '<tr><td colspan="9" class="spa-empty"><i class="fa fa-spinner fa-spin"></i> Đang tải dữ liệu...</td></tr>';

        post('SearchByPermissionDate', {
            permissionDate: dateInput.value,
            fromTime: fromTimeValue,
            toTime: toTimeValue,
            fromAirp: fromAirpInput.value,
            toAirp: toAirpInput.value,
            via: viaInput.value
        })
            .then(function (result) {
                permissions = result.Items || [];
                activePermissionDate = dateInput.value;
                activeFromTime = result.HasTimeFilter ? (result.FromTime || '') : '';
                activeToTime = result.HasTimeFilter ? (result.ToTime || '') : '';
                activeFromAirp = result.FromAirp || '';
                activeToAirp = result.ToAirp || '';
                activeVia = result.Via || '';
                document.getElementById('spaSearchCaption').textContent =
                    'Ngày cấp phép: ' + (result.PermissionDate || dateInput.value) +
                    (result.HasTimeFilter ? ' • ETD/ETA ' + activeFromTime + ' - ' + activeToTime : '') +
                    (activeFromAirp ? ' • FROM ' + activeFromAirp : '') +
                    (activeToAirp ? ' • TO ' + activeToAirp : '') +
                    (activeVia ? ' • VIA ' + activeVia : '');
                applyColumnFilters();
                setExportEnabled(permissions.length > 0);
            })
            .catch(function (error) {
                activePermissionDate = '';
                permissions = [];
                filteredPermissions = [];
                applyColumnFilters();
                setExportEnabled(false);
                alert(error.message);
            })
            .then(function () { setSearchLoading(false); });
    }

    function exportExcel() {
        if (!activePermissionDate || !permissions.length) {
            alert('Vui lòng Search dữ liệu trước khi Export Excel.');
            return;
        }

        function columnFilter(field) {
            var input = document.querySelector('[data-filter-field="' + field + '"]');
            return input ? input.value.trim() : '';
        }

        var query = [
            'permissionDate=' + encodeURIComponent(activePermissionDate),
            'fromTime=' + encodeURIComponent(activeFromTime),
            'toTime=' + encodeURIComponent(activeToTime),
            'fromAirp=' + encodeURIComponent(activeFromAirp),
            'toAirp=' + encodeURIComponent(activeToAirp),
            'via=' + encodeURIComponent(activeVia),
            'permNbr=' + encodeURIComponent(columnFilter('PermNbr')),
            'author=' + encodeURIComponent(columnFilter('Author')),
            'pType=' + encodeURIComponent(columnFilter('PType')),
            'fType=' + encodeURIComponent(columnFilter('FType')),
            'number=' + encodeURIComponent(columnFilter('Number')),
            'version=' + encodeURIComponent(columnFilter('Version')),
            'masterDate=' + encodeURIComponent(columnFilter('PermissionDate')),
            'oper=' + encodeURIComponent(columnFilter('Oper'))
        ].join('&');
        var handlerPath = window.location.pathname.replace(/[^/]+$/, 'SearchPermissionAdvExport.ashx');
        window.location.href = handlerPath + '?' + query;
    }

    function flightRowsHtml(result) {
        var flights = result.Flights || [];
        return flights.length ? flights.map(function (flight, index) {
            return '<tr>' +
                '<td>' + (index + 1) + '</td>' +
                '<td><strong>' + esc(flight.Callsign) + '</strong></td>' +
                '<td>' + esc(flight.Registration) + '</td>' +
                '<td>' + esc(flight.FromAirp) + '</td>' +
                '<td>' + esc(flight.ToAirp) + '</td>' +
                '<td>' + esc(flight.Etd) + '</td>' +
                '<td>' + esc(flight.Eta) + '</td>' +
                '<td>' + esc(flight.DaysFlight) + '</td>' +
                '<td>' + esc(flight.BeginDate) + '</td>' +
                '<td>' + esc(flight.EndDate) + '</td>' +
                '<td>' + esc(flight.Craft) + '</td>' +
                '<td>' + esc(flight.Purpose) + '</td>' +
                '<td>' + esc(flight.Mtow) + '</td>' +
                '<td class="spa-text-cell">' + esc(flight.Via) + '</td>' +
                '<td class="spa-text-cell">' + esc(flight.Remark) + '</td>' +
                '<td>' + esc(flight.Status) + '</td>' +
                '<td title="' + esc(flight.LastModify) + '">' + esc(flight.LastUser) + '</td>' +
                '</tr>';
        }).join('') : '<tr><td colspan="17" class="spa-empty">Phép chưa có thông tin chi tiết.</td></tr>';
    }

    function inlineDetailTableHeadHtml() {
        return '<thead><tr>' +
            '<th>NO</th><th>CALLSIGN</th><th>FROM</th><th>TO</th>' +
            '<th>ETD</th><th>ETA</th><th>DAY/DATE</th><th>BEGIN DATE</th><th>END DATE</th>' +
            '<th>CRAFT</th><th>PURPOSE</th><th>MTOW</th><th>VIA</th><th>REMARK</th>' +
            '<th>STATUS</th><th>LAST USER</th></tr></thead>';
    }

    function inlineDetailColgroupHtml() {
        var widths = [3, 7, 4, 4, 5, 5, 7, 7, 7, 5, 6, 5, 10, 13, 5, 7];
        return '<colgroup>' + widths.map(function (width) {
            return '<col style="width:' + width + '%" />';
        }).join('') + '</colgroup>';
    }

    function inlineFlightRowsHtml(result) {
        var flights = result.Flights || [];
        return flights.length ? flights.map(function (flight, index) {
            return '<tr>' +
                '<td>' + (index + 1) + '</td>' +
                '<td><strong>' + esc(flight.Callsign) + '</strong></td>' +
                '<td>' + esc(flight.FromAirp) + '</td>' +
                '<td>' + esc(flight.ToAirp) + '</td>' +
                '<td>' + esc(flight.Etd) + '</td>' +
                '<td>' + esc(flight.Eta) + '</td>' +
                '<td>' + esc(flight.DaysFlight) + '</td>' +
                '<td>' + esc(flight.BeginDate) + '</td>' +
                '<td>' + esc(flight.EndDate) + '</td>' +
                '<td>' + esc(flight.Craft) + '</td>' +
                '<td>' + esc(flight.Purpose) + '</td>' +
                '<td>' + esc(flight.Mtow) + '</td>' +
                '<td>' + esc(flight.Via) + '</td>' +
                '<td>' + esc(flight.Remark) + '</td>' +
                '<td>' + esc(flight.Status) + '</td>' +
                '<td title="' + esc(flight.LastModify) + '">' + esc(flight.LastUser) + '</td>' +
                '</tr>';
        }).join('') : '<tr><td colspan="16" class="spa-empty">Phép chưa có thông tin chi tiết.</td></tr>';
    }

    function getPermissionDetail(sourceType, permId) {
        var key = [sourceType, permId, activeFromTime, activeToTime,
            activeFromAirp, activeToAirp, activeVia].join(':');
        if (!detailCache[key]) {
            detailCache[key] = post('GetPermissionDetail', {
                sourceType: sourceType,
                permId: permId,
                fromTime: activeFromTime,
                toTime: activeToTime,
                fromAirp: activeFromAirp,
                toAirp: activeToAirp,
                via: activeVia
            }).catch(function (error) {
                delete detailCache[key];
                throw error;
            });
        }
        return detailCache[key];
    }

    function renderDetail(result, permNbr) {
        document.getElementById('spaDetailTitle').textContent = 'Flight details - ' + permNbr;
        document.getElementById('spaDetailSubtitle').textContent =
            result.SourceType + ' • PERM_ID: ' + result.PermId + ' • ' + result.Total + ' flight(s)';
        document.getElementById('spaDetailRows').innerHTML = flightRowsHtml(result);
    }

    function closeInlineDetails(exceptButton) {
        Array.prototype.forEach.call(document.querySelectorAll('.spa-inline-detail-row'), function (row) {
            row.parentNode.removeChild(row);
        });
        Array.prototype.forEach.call(document.querySelectorAll('.spa-inline-toggle'), function (button) {
            if (button !== exceptButton) {
                button.setAttribute('aria-expanded', 'false');
                button.innerHTML = '<i class="fa fa-angle-double-down"></i>';
            }
        });
    }

    function toggleInlineDetail(button) {
        var sourceType = button.getAttribute('data-source');
        var permId = Number(button.getAttribute('data-perm-id'));
        var permNbr = button.parentNode.querySelector('.spa-perm-link').textContent;
        var permissionRow = button.closest('tr');
        var nextRow = permissionRow.nextElementSibling;

        if (nextRow && nextRow.classList.contains('spa-inline-detail-row')) {
            nextRow.parentNode.removeChild(nextRow);
            button.setAttribute('aria-expanded', 'false');
            button.innerHTML = '<i class="fa fa-angle-double-down"></i>';
            return;
        }

        closeInlineDetails(button);
        button.setAttribute('aria-expanded', 'true');
        button.innerHTML = '<i class="fa fa-angle-double-up"></i>';

        var detailRow = document.createElement('tr');
        detailRow.className = 'spa-inline-detail-row';
        detailRow.innerHTML = '<td colspan="9" class="spa-inline-cell">' +
            '<div class="spa-inline-panel"><div class="spa-inline-head">' +
            '<strong>Flight details - ' + esc(permNbr) + '</strong>' +
            '<span class="spa-inline-caption">Đang tải dữ liệu...</span>' +
            '<button type="button" class="spa-inline-close" title="Đóng chi tiết">&times;</button>' +
            '</div><div class="spa-inline-table-wrap">' +
            '<div class="spa-inline-loading"><i class="fa fa-spinner fa-spin"></i> Đang tải...</div>' +
            '</div></div></td>';
        permissionRow.parentNode.insertBefore(detailRow, permissionRow.nextSibling);

        detailRow.querySelector('.spa-inline-close').addEventListener('click', function () {
            if (detailRow.parentNode) detailRow.parentNode.removeChild(detailRow);
            button.setAttribute('aria-expanded', 'false');
            button.innerHTML = '<i class="fa fa-angle-double-down"></i>';
            button.focus();
        });

        getPermissionDetail(sourceType, permId)
            .then(function (result) {
                if (!detailRow.parentNode) return;
                detailRow.querySelector('.spa-inline-caption').textContent =
                    result.SourceType + ' • PERM_ID: ' + result.PermId + ' • ' + result.Total + ' flight(s)';
                detailRow.querySelector('.spa-inline-table-wrap').innerHTML =
                    '<table class="spa-table spa-inline-detail-table">' + inlineDetailColgroupHtml() +
                    inlineDetailTableHeadHtml() + '<tbody>' + inlineFlightRowsHtml(result) + '</tbody></table>';
            })
            .catch(function (error) {
                if (!detailRow.parentNode) return;
                detailRow.querySelector('.spa-inline-caption').textContent = 'Không thể tải dữ liệu.';
                detailRow.querySelector('.spa-inline-table-wrap').innerHTML =
                    '<div class="spa-inline-error">' + esc(error.message) + '</div>';
            });
    }

    function openModal() {
        var modal = document.getElementById('spaDetailModal');
        modal.hidden = false;
        document.body.classList.add('spa-modal-open');
        document.getElementById('spaDetailClose').focus();
    }

    function closeModal() {
        var modal = document.getElementById('spaDetailModal');
        if (modal.hidden) return;
        modal.hidden = true;
        document.body.classList.remove('spa-modal-open');
        if (lastDetailTrigger) lastDetailTrigger.focus();
    }

    function loadDetail(sourceType, permId, permNbr) {
        document.getElementById('spaDetailTitle').textContent = 'Flight details - ' + permNbr;
        document.getElementById('spaDetailSubtitle').textContent = 'Đang tải dữ liệu...';
        document.getElementById('spaDetailRows').innerHTML =
            '<tr><td colspan="17" class="spa-empty">Đang tải dữ liệu...</td></tr>';
        openModal();

        getPermissionDetail(sourceType, permId)
            .then(function (result) { renderDetail(result, permNbr); })
            .catch(function (error) {
                document.getElementById('spaDetailRows').innerHTML =
                    '<tr><td colspan="17" class="spa-empty">' + esc(error.message) + '</td></tr>';
            });
    }

    function init() {
        var dateInput = document.getElementById('spaPermissionDate');
        var fromTimeInput = document.getElementById('spaFromTime');
        var toTimeInput = document.getElementById('spaToTime');
        var fromAirpInput = document.getElementById('spaFromAirp');
        var toAirpInput = document.getElementById('spaToAirp');
        var viaInput = document.getElementById('spaVia');
        if (!dateInput) return;

        dateInput.value = isoToday();
        fromTimeInput.value = '';
        toTimeInput.value = '';
        document.getElementById('spaSearch').addEventListener('click', search);
        document.getElementById('spaExportExcel').addEventListener('click', exportExcel);
        document.getElementById('spaClear').addEventListener('click', function () {
            dateInput.value = isoToday();
            fromTimeInput.value = '';
            toTimeInput.value = '';
            fromAirpInput.value = '';
            toAirpInput.value = '';
            viaInput.value = '';
            clearColumnFilters();
            permissions = [];
            filteredPermissions = [];
            detailCache = {};
            activePermissionDate = '';
            activeFromTime = '';
            activeToTime = '';
            activeFromAirp = '';
            activeToAirp = '';
            activeVia = '';
            setExportEnabled(false);
            document.getElementById('spaSearchCaption').textContent = 'Chọn ngày cấp phép và nhấn Search.';
            renderPermissions();
        });
        Array.prototype.forEach.call(document.querySelectorAll('[data-filter-field]'), function (input) {
            input.addEventListener('input', applyColumnFilters);
            input.addEventListener('keydown', function (event) {
                if (event.key === 'Enter') event.preventDefault();
            });
        });
        dateInput.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') search();
        });
        fromTimeInput.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') search();
        });
        toTimeInput.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') search();
        });
        fromTimeInput.addEventListener('input', function () { formatTimeInput(this); });
        toTimeInput.addEventListener('input', function () { formatTimeInput(this); });
        [fromAirpInput, toAirpInput, viaInput].forEach(function (input) {
            input.addEventListener('input', function () { this.value = this.value.toUpperCase(); });
            input.addEventListener('keydown', function (event) {
                if (event.key === 'Enter') search();
            });
        });
        document.getElementById('spaPrev').addEventListener('click', function () {
            if (currentPage > 1) { currentPage--; renderPermissions(); }
        });
        document.getElementById('spaNext').addEventListener('click', function () {
            var totalPages = Math.max(1, Math.ceil(filteredPermissions.length / pageSize));
            if (currentPage < totalPages) { currentPage++; renderPermissions(); }
        });
        document.getElementById('spaDetailClose').addEventListener('click', closeModal);
        document.getElementById('spaDetailCancel').addEventListener('click', closeModal);
        document.getElementById('spaDetailModal').addEventListener('mousedown', function (event) {
            if (event.target === this) closeModal();
        });
        document.addEventListener('keydown', function (event) {
            if (event.key === 'Escape') closeModal();
        });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();
