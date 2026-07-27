(function () {
    'use strict';

    function escapeHtml(value) {
        return String(value == null ? '' : value).replace(/[&<>"']/g, function (character) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[character];
        });
    }

    function requestAlerts() {
        var deferred = window.jQuery.Deferred();
        window.jQuery.ajax({
            type: 'POST',
            url: window.location.pathname + '/GetDelayAlerts',
            data: '{}',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        }).done(function (result) {
            deferred.resolve(result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result);
        }).fail(function (xhr) {
            var response = xhr && xhr.responseJSON;
            var message = response && (response.Message || response.message);
            deferred.reject(new Error(message || 'Không thể tải cảnh báo delay trong ngày hiện tại.'));
        });
        return deferred.promise();
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="anomaly"]');
        if (!app) return;

        var hero = app.querySelector('.rn-hero');
        if (hero) {
            hero.querySelector('h1').textContent = 'Cảnh báo chuyến bay delay';
            hero.querySelector('p').textContent = 'Theo dõi độ lệch giữa giờ dự kiến và giờ cất cánh thực tế trong ngày hiện tại';
            var liveNote = hero.querySelector('.rn-live small');
            if (liveNote) liveNote.textContent = '';
        }

        var heroHtml = hero ? hero.outerHTML : '';
        app.innerHTML = heroHtml +
            '<section class="anomaly-actions">' +
                '<div><span>Nguồn dữ liệu hiện tại</span><strong id="anomalyDataDay">Đang tải...</strong><small>T_DAY_FLIGHTS_GOINGON</small></div>' +
                '<button type="button" id="anomalyViewButton"><i class="fa fa-exclamation-triangle"></i> Xem cảnh báo delay</button>' +
            '</section>' +
            '<div class="rn-kpis anomaly-kpis">' +
                '<div class="rn-kpi" style="--accent:#2387c8"><span>Tổng cảnh báo</span><strong id="anomalyTotal">--</strong><small>Từ 15 phút trở lên</small></div>' +
                '<div class="rn-kpi" style="--accent:#b38a13"><span>Mức 1</span><strong id="anomalyLevel1">--</strong><small>15 đến 29 phút</small></div>' +
                '<div class="rn-kpi" style="--accent:#d36b17"><span>Mức 2</span><strong id="anomalyLevel2">--</strong><small>30 đến 59 phút</small></div>' +
                '<div class="rn-kpi" style="--accent:#c43c45"><span>Mức 3</span><strong id="anomalyLevel3">--</strong><small>Từ 60 phút</small></div>' +
            '</div>' +
            '<div id="anomalyError" class="anomaly-error" hidden></div>' +
            '<section class="anomaly-status">' +
                '<div><i class="fa fa-clock-o"></i></div><strong id="anomalyStatusTitle">Đang tổng hợp dữ liệu</strong>' +
                '<span id="anomalyStatusText">ETD 4 số và ATD 6 số</span>' +
            '</section>' +
            '<div id="anomalyModal" class="anomaly-backdrop" role="presentation" aria-hidden="true" hidden>' +
                '<div class="anomaly-dialog" role="dialog" aria-modal="true" aria-labelledby="anomalyDialogTitle">' +
                    '<header><div><h2 id="anomalyDialogTitle">Cảnh báo chuyến bay delay</h2><p id="anomalyDialogSummary"></p></div>' +
                    '<button type="button" id="anomalyCloseButton" title="Đóng" aria-label="Đóng">&times;</button></header>' +
                    '<div class="anomaly-dialog-body">' +
                        '<div id="anomalyFilters" class="anomaly-level-filters" role="group" aria-label="Lọc cảnh báo theo mức độ"></div>' +
                        '<div class="anomaly-table-wrap"><table class="anomaly-table"><thead><tr>' +
                            '<th>STT</th><th>Mức</th><th>Callsign</th><th>Hãng</th><th>Đăng ký</th><th>PERMTYPE</th>' +
                            '<th>FROM</th><th>TO</th><th>ETD (4 số)</th><th>ATD (6 số)</th><th>Chậm</th>' +
                        '</tr></thead><tbody id="anomalyRows"></tbody></table></div>' +
                    '</div>' +
                    '<footer><span>Đang hiển thị: <strong id="anomalyVisibleCount">0</strong> chuyến</span>' +
                    '<button type="button" id="anomalyFooterClose">Đóng</button></footer>' +
                '</div>' +
            '</div>';

        var data = null;
        var selectedLevel = '';
        var viewButton = app.querySelector('#anomalyViewButton');
        var modal = app.querySelector('#anomalyModal');
        var errorBox = app.querySelector('#anomalyError');

        function number(value) {
            return Number(value || 0).toLocaleString('vi-VN');
        }

        function setText(selector, value) {
            var node = app.querySelector(selector);
            if (node) node.textContent = value;
        }

        function renderSummary(result) {
            setText('#anomalyDataDay', 'Ngày ' + result.reportDay);
            setText('#anomalyTotal', number(result.total));
            setText('#anomalyLevel1', number(result.level1));
            setText('#anomalyLevel2', number(result.level2));
            setText('#anomalyLevel3', number(result.level3));
            setText('#anomalyStatusTitle', result.total ? number(result.total) + ' chuyến cần cảnh báo' : 'Không có chuyến cần cảnh báo');
            setText('#anomalyStatusText', 'Nguồn ' + result.source + ' • ETD 4 số, ATD 6 số');
            setText('#anomalyDialogSummary', 'Ngày dữ liệu ' + result.reportDay + ' • Nguồn ' + result.source);
        }

        function renderFilters() {
            var filters = app.querySelector('#anomalyFilters');
            var items = [
                { level: '', label: 'Tất cả', count: data.total },
                { level: '1', label: 'Mức 1: 15-29 phút', count: data.level1 },
                { level: '2', label: 'Mức 2: 30-59 phút', count: data.level2 },
                { level: '3', label: 'Mức 3: từ 60 phút', count: data.level3 }
            ];
            filters.innerHTML = items.map(function (item) {
                return '<button type="button" data-level="' + item.level + '" class="' +
                    (selectedLevel === item.level ? 'is-active' : '') + '">' +
                    escapeHtml(item.label) + ' (' + number(item.count) + ')</button>';
            }).join('');
            Array.prototype.forEach.call(filters.querySelectorAll('button'), function (button) {
                button.onclick = function () {
                    selectedLevel = this.getAttribute('data-level');
                    renderFilters();
                    renderRows();
                };
            });
        }

        function renderRows() {
            var rows = (data.alerts || []).filter(function (item) {
                return !selectedLevel || String(item.level) === selectedLevel;
            });
            var body = app.querySelector('#anomalyRows');
            if (!rows.length) {
                body.innerHTML = '<tr><td colspan="11" class="anomaly-empty">Không có chuyến bay thuộc mức cảnh báo đã chọn.</td></tr>';
            } else {
                body.innerHTML = rows.map(function (item) {
                    return '<tr class="level-' + item.level + '"><td>' + item.no + '</td>' +
                        '<td><span class="anomaly-level level-' + item.level + '">' + escapeHtml(item.levelName) + '</span></td>' +
                        '<td><strong>' + escapeHtml(item.flightNumber) + '</strong></td><td>' + escapeHtml(item.oper) + '</td>' +
                        '<td>' + escapeHtml(item.registration) + '</td><td>' + escapeHtml(item.permitType) + '</td>' +
                        '<td>' + escapeHtml(item.fromAirport) + '</td><td>' + escapeHtml(item.toAirport) + '</td>' +
                        '<td>' + escapeHtml(item.etd) + '</td><td>' + escapeHtml(item.atd) + '</td>' +
                        '<td><strong>' + escapeHtml(item.delayDuration) + '</strong></td></tr>';
                }).join('');
            }
            setText('#anomalyVisibleCount', number(rows.length));
        }

        function openModal() {
            selectedLevel = '';
            renderFilters();
            renderRows();
            modal.hidden = false;
            modal.setAttribute('aria-hidden', 'false');
            document.body.style.overflow = 'hidden';
        }

        function closeModal() {
            modal.hidden = true;
            modal.setAttribute('aria-hidden', 'true');
            document.body.style.overflow = '';
        }

        function load(openAfterLoad) {
            viewButton.disabled = true;
            viewButton.innerHTML = '<i class="fa fa-refresh fa-spin"></i> Đang tải...';
            errorBox.hidden = true;
            requestAlerts().done(function (result) {
                data = result;
                renderSummary(result);
                if (openAfterLoad) openModal();
            }).fail(function (error) {
                errorBox.textContent = error.message;
                errorBox.hidden = false;
                setText('#anomalyStatusTitle', 'Không thể tải dữ liệu cảnh báo');
            }).always(function () {
                viewButton.disabled = false;
                viewButton.innerHTML = '<i class="fa fa-exclamation-triangle"></i> Xem cảnh báo delay';
            });
        }

        viewButton.onclick = function () { load(true); };
        app.querySelector('#anomalyCloseButton').onclick = closeModal;
        app.querySelector('#anomalyFooterClose').onclick = closeModal;
        modal.onclick = function (event) { if (event.target === modal) closeModal(); };
        document.addEventListener('keydown', function (event) {
            if (event.key === 'Escape' && !modal.hidden) closeModal();
        });
        load(false);
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
}());
