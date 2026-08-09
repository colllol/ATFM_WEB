(function () {
    'use strict';

    var pageSize = 100;
    var currentPage = 1;
    var permissions = [];
    var lastDetailTrigger = null;

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

    function setSearchLoading(loading) {
        var button = document.getElementById('spaSearch');
        button.disabled = loading;
        button.innerHTML = loading
            ? '<i class="fa fa-spinner fa-spin"></i> Loading...'
            : '<i class="fa fa-search"></i> Search';
    }

    function renderPermissions() {
        var body = document.getElementById('spaPermissionRows');
        var totalPages = Math.max(1, Math.ceil(permissions.length / pageSize));
        if (currentPage > totalPages) currentPage = totalPages;
        var start = (currentPage - 1) * pageSize;
        var rows = permissions.slice(start, start + pageSize);

        if (!rows.length) {
            body.innerHTML = '<tr><td colspan="9" class="spa-empty">Không có phép phù hợp.</td></tr>';
        } else {
            body.innerHTML = rows.map(function (item, index) {
                return '<tr>' +
                    '<td>' + (start + index + 1) + '</td>' +
                    '<td><button type="button" class="spa-perm-link" data-source="' + esc(item.SourceType) +
                    '" data-perm-id="' + esc(item.PermId) + '">' + esc(item.PermNbr) + '</button></td>' +
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

        document.getElementById('spaTotal').textContent = 'Tổng số: ' + permissions.length.toLocaleString('vi-VN');
        document.getElementById('spaPager').hidden = permissions.length <= pageSize;
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
    }

    function search() {
        var dateInput = document.getElementById('spaPermissionDate');
        if (!dateInput.value) {
            alert('Vui lòng chọn ngày cấp phép.');
            dateInput.focus();
            return;
        }

        setSearchLoading(true);
        document.getElementById('spaPermissionRows').innerHTML =
            '<tr><td colspan="9" class="spa-empty"><i class="fa fa-spinner fa-spin"></i> Đang tải dữ liệu...</td></tr>';

        post('SearchByPermissionDate', { permissionDate: dateInput.value })
            .then(function (result) {
                permissions = result.Items || [];
                currentPage = 1;
                document.getElementById('spaSearchCaption').textContent =
                    'Ngày cấp phép: ' + (result.PermissionDate || dateInput.value);
                renderPermissions();
            })
            .catch(function (error) {
                permissions = [];
                currentPage = 1;
                renderPermissions();
                alert(error.message);
            })
            .then(function () { setSearchLoading(false); });
    }

    function renderDetail(result, permNbr) {
        var detailBody = document.getElementById('spaDetailRows');

        document.getElementById('spaDetailTitle').textContent = 'Flight details - ' + permNbr;
        document.getElementById('spaDetailSubtitle').textContent =
            result.SourceType + ' • PERM_ID: ' + result.PermId + ' • ' + result.Total + ' flight(s)';

        var flights = result.Flights || [];
        detailBody.innerHTML = flights.length ? flights.map(function (flight, index) {
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

        post('GetPermissionDetail', { sourceType: sourceType, permId: permId })
            .then(function (result) { renderDetail(result, permNbr); })
            .catch(function (error) {
                document.getElementById('spaDetailRows').innerHTML =
                    '<tr><td colspan="17" class="spa-empty">' + esc(error.message) + '</td></tr>';
            });
    }

    function init() {
        var dateInput = document.getElementById('spaPermissionDate');
        if (!dateInput) return;

        dateInput.value = isoToday();
        document.getElementById('spaSearch').addEventListener('click', search);
        document.getElementById('spaClear').addEventListener('click', function () {
            dateInput.value = isoToday();
            permissions = [];
            currentPage = 1;
            document.getElementById('spaSearchCaption').textContent = 'Chọn ngày cấp phép và nhấn Search.';
            renderPermissions();
        });
        dateInput.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') search();
        });
        document.getElementById('spaPrev').addEventListener('click', function () {
            if (currentPage > 1) { currentPage--; renderPermissions(); }
        });
        document.getElementById('spaNext').addEventListener('click', function () {
            var totalPages = Math.max(1, Math.ceil(permissions.length / pageSize));
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
