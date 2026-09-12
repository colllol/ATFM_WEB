(function () {
    function camelize(value) { if (Array.isArray(value)) return value.map(camelize); if (!value || typeof value !== 'object') return value; var result = {}; Object.keys(value).forEach(function (key) { result[key.charAt(0).toLowerCase() + key.slice(1)] = camelize(value[key]); }); return result; }
    var airportNames = {
        VVNB: 'Nội Bài', VVTS: 'Tân Sơn Nhất', VVDN: 'Đà Nẵng', VVCR: 'Cam Ranh',
        VVPQ: 'Phú Quốc', VVCI: 'Cát Bi', VVDL: 'Liên Khương', VVPC: 'Phù Cát',
        VVVH: 'Vinh', VVCT: 'Cần Thơ', VVCA: 'Chu Lai', VVDB: 'Điện Biên',
        VVBM: 'Buôn Ma Thuột', VVTH: 'Tuy Hòa', VVPK: 'Pleiku', VVTX: 'Thọ Xuân',
        VVDH: 'Đồng Hới', VVRG: 'Rạch Giá', VVCM: 'Cà Mau', VVCS: 'Côn Đảo', VVVD: 'Vân Đồn'
    };

    function post(method, data) {
        return fetch(window.reportApiBase + 'api/AirportTakeoffLanding/' + method, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                return response.json().then(function (result) {
                    if (!response.ok) throw new Error(result.Message || result.message || ('HTTP ' + response.status));
                    return result;
                });
            })
            .then(function (result) {
                if (result.Code && result.Code !== '00') throw new Error(result.Message || 'Không thể tải dữ liệu.');
                return camelize(Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result.d);
            });
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : value;
        return node.innerHTML;
    }

    function number(value) { return Number(value || 0).toLocaleString('vi-VN'); }
    function airportName(code) { return airportNames[code] || code; }
    function statusColor(state) {
        return state === 'FINISHED' ? '#20b486' : '#f5a623';
    }
    function statusLabel(state) {
        if (state === 'FINISHED') return 'Hoàn thành';
        return state.replace('DELAY_', 'Delay ').replace('_PLUS', '+').replace('_', '–') + ' phút';
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="airport"]');
        if (!app) return;
        var airport = app.querySelector('#rnAirport');
        var from = app.querySelector('#rnFrom');
        var to = app.querySelector('#rnTo');
        var apply = app.querySelector('#rnApply');
        var today = new Date();
        var toIso = formatDate(today);
        var fromIso = formatDate(today);
        var detailState = null;
        var summaryData = null;

        apply.insertAdjacentHTML('afterend', '<button type="button" class="rn-filter-button rn-report-button" id="rnAirportReport" disabled><i class="fa fa-table"></i> Báo cáo</button>');
        var reportButton = app.querySelector('#rnAirportReport');

        from.value = fromIso;
        to.value = toIso;
        airport.innerHTML = '<option value="ALL">Tất cả sân bay</option>' + Object.keys(airportNames).map(function (code) {
            return '<option value="' + code + '">' + code + ' - ' + airportNames[code] + '</option>';
        }).join('');

        function formatDate(value) {
            return value.getFullYear() + '-' + String(value.getMonth() + 1).padStart(2, '0') + '-' + String(value.getDate()).padStart(2, '0');
        }

        function syncAirportOptions(items) {
            var selected = airport.value;
            items.forEach(function (item) {
                if (!airport.querySelector('option[value="' + item.code + '"]')) {
                    airport.insertAdjacentHTML('beforeend', '<option value="' + esc(item.code) + '">' + esc(item.code + ' - ' + airportName(item.code)) + '</option>');
                }
            });
            airport.value = selected;
        }

        function renderSummary(data) {
            summaryData = data;
            reportButton.disabled = !(data.airports || []).length;
            syncAirportOptions(data.airports || []);
            var kpis = '<div class="rn-kpis">' +
                '<div class="rn-kpi" style="--accent:#2387c8"><span>Tổng cất cánh</span><strong>' + number(data.totalDepartures) + '</strong><small>Finished + Delay</small></div>' +
                '<div class="rn-kpi" style="--accent:#20b486"><span>Tổng hạ cánh</span><strong>' + number(data.totalArrivals) + '</strong><small>Finished + Delay</small></div>' +
                '<div class="rn-kpi" style="--accent:#8a6ee8"><span>Sân bay khai thác</span><strong>' + number(data.airportCount) + '</strong><small>Sân bay Việt Nam</small></div>' +
                '<div class="rn-kpi" style="--accent:#f5a623"><span>Lưu lượng cao nhất</span><strong>' + esc(data.peakAirport || '—') + '</strong><small>' + number(data.peakTotal) + ' lượt</small></div></div>';
            var max = 0;
            (data.airports || []).forEach(function (item) { max = Math.max(max, item.departures, item.arrivals); });
            var bars = (data.airports || []).map(function (item) {
                var isPeak = !!data.peakAirport && item.code === data.peakAirport;
                var departureHeight = item.departures ? Math.max(4, item.departures / Math.max(1, max) * 100) : 0;
                var arrivalHeight = item.arrivals ? Math.max(4, item.arrivals / Math.max(1, max) * 100) : 0;
                return '<div class="rn-airport-bar-group' + (isPeak ? ' is-peak' : '') + '"><div class="rn-airport-bar-pair">' +
                    '<button type="button" class="rn-airport-bar departure" data-airport="' + item.code + '" data-movement="departure" style="height:' + departureHeight + '%" title="' + esc(item.code + ' - Cất cánh: ' + number(item.departures)) + '"><span>' + number(item.departures) + '</span></button>' +
                    '<button type="button" class="rn-airport-bar arrival" data-airport="' + item.code + '" data-movement="arrival" style="height:' + arrivalHeight + '%" title="' + esc(item.code + ' - Hạ cánh: ' + number(item.arrivals)) + '"><span>' + number(item.arrivals) + '</span></button>' +
                    '</div><b>' + esc(item.code) + '</b><small>' + esc(airportName(item.code)) + '</small>' +
                    (isPeak ? '<span class="rn-peak-badge"><i class="fa fa-star"></i> Cao nhất</span>' : '') + '</div>';
            }).join('');
            if (!bars) bars = '<div class="rn-airport-empty">Không có chuyến bay Finished hoặc Delay trong khoảng ngày đã chọn.</div>';

            var grid = '<div class="rn-grid"><article class="rn-card wide"><h2>Lưu lượng cất/hạ cánh theo sân bay</h2>' +
                '<p class="rn-card-subtitle">Nhấn trực tiếp vào từng cột để xem danh sách chuyến bay tương ứng • Nguồn: ' + esc(data.source) + '</p>' +
                '<div class="rn-airport-chart-scroll"><div class="rn-airport-chart">' + bars + '</div></div>' +
                '<div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>Cất cánh</span><span><i class="rn-dot" style="background:#20b486"></i>Hạ cánh</span><span><i class="fa fa-star" style="color:#f5a623"></i> Sân bay lưu lượng cao nhất</span></div></article>' +
                '<article class="rn-card wide rn-airport-report-card" id="rnAirportReportCard" hidden></article>' +
                '<article class="rn-card wide rn-airport-detail-card" id="rnAirportDetail"><div class="rn-airport-detail-placeholder"><i class="fa fa-bar-chart"></i><strong>Chọn một cột cất cánh hoặc hạ cánh</strong><span>Danh sách chuyến bay chi tiết sẽ hiển thị tại đây.</span></div></article></div>';
            var oldKpis = app.querySelector('.rn-kpis');
            var oldGrid = app.querySelector('.rn-grid');
            if (oldKpis) oldKpis.outerHTML = kpis;
            if (oldGrid) oldGrid.outerHTML = grid;
            Array.prototype.forEach.call(app.querySelectorAll('.rn-airport-bar'), function (bar) {
                bar.onclick = function () {
                    Array.prototype.forEach.call(app.querySelectorAll('.rn-airport-bar'), function (item) { item.classList.remove('is-active'); });
                    this.classList.add('is-active');
                    detailState = { airport: this.getAttribute('data-airport'), movement: this.getAttribute('data-movement'), page: 1, pageSize: 100 };
                    loadDetails();
                };
            });
        }

        function renderDetails(data) {
            var detail = app.querySelector('#rnAirportDetail');
            var movementLabel = data.movement === 'departure' ? 'Cất cánh' : 'Hạ cánh';
            var rows = (data.rows || []).map(function (row) {
                return '<tr><td class="rn-stt">' + row.stt + '</td><td><b>' + esc(row.callsign) + '</b></td><td>' + esc(row.oper) + '</td>' +
                    '<td>' + esc(row.registration) + '</td><td>' + esc(row.permType) + '</td><td>' + esc(row.fromAirp) + '</td><td>' + esc(row.toAirp) + '</td>' +
                    '<td>' + esc(row.atdDay) + '</td><td>' + esc(row.ataDay) + '</td><td>' + esc(row.eobtDay) + '</td>' +
                    '<td><span class="rn-pill" style="background:' + statusColor(row.status) + ';color:#fff">' + esc(statusLabel(row.status)) + '</span></td></tr>';
            }).join('');
            if (!rows) rows = '<tr><td colspan="11" class="rn-no-data">Không có dữ liệu phù hợp.</td></tr>';
            detail.innerHTML = '<div class="rn-airport-detail-head"><div><h2>' + movementLabel + ' tại ' + esc(data.airport) + ' - ' + esc(airportName(data.airport)) + '</h2>' +
                '<p class="rn-card-subtitle">' + number(data.total) + ' chuyến bay Finished hoặc Delay</p></div>' +
                '<label class="rn-page-size">Số dòng <select id="rnAirportPageSize"><option>25</option><option>50</option><option>100</option><option>200</option><option>500</option></select></label></div>' +
                '<div class="rn-table-wrap rn-airport-detail-wrap"><table class="rn-table rn-flight-detail-table"><thead><tr>' +
                '<th>STT</th><th>CALLSIGN</th><th>OPER</th><th>REGISTRATION</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ATDDAY</th><th>ATADAY</th><th>EOBTDAY</th><th>TRẠNG THÁI</th>' +
                '</tr></thead><tbody>' + rows + '</tbody></table></div>' +
                '<div class="rn-flight-pager"><button type="button" class="rn-filter-button" id="rnAirportPrev">‹ Trước</button><span>Trang ' + data.page + '/' + data.totalPages + ' • ' + number(data.total) + ' dòng</span><button type="button" class="rn-filter-button" id="rnAirportNext">Sau ›</button></div>';
            var size = detail.querySelector('#rnAirportPageSize');
            size.value = String(detailState.pageSize);
            size.onchange = function () { detailState.pageSize = parseInt(this.value, 10); detailState.page = 1; loadDetails(); };
            var previous = detail.querySelector('#rnAirportPrev');
            var next = detail.querySelector('#rnAirportNext');
            previous.disabled = data.page <= 1;
            next.disabled = data.page >= data.totalPages;
            previous.onclick = function () { if (detailState.page > 1) { detailState.page--; loadDetails(); } };
            next.onclick = function () { if (detailState.page < data.totalPages) { detailState.page++; loadDetails(); } };
        }

        // Bảng báo cáo tổng hợp theo sân bay, sân bay lưu lượng cao nhất được tô nổi bật.
        function reportTableHtml(forPrint) {
            var items = (summaryData && summaryData.airports) || [];
            var grandTotal = items.reduce(function (sum, item) { return sum + Number(item.total || 0); }, 0);
            return '<table class="rn-table"><thead><tr><th>STT</th><th>Sân bay</th><th>Tên sân bay</th><th>Cất cánh</th><th>Hạ cánh</th><th>Tổng lượt</th><th>Tỷ trọng</th></tr></thead><tbody>' +
                items.map(function (item, index) {
                    var isPeak = !!summaryData.peakAirport && item.code === summaryData.peakAirport;
                    var style = isPeak ? ' style="background:#fff6e0;font-weight:700"' : '';
                    return '<tr' + style + '><td>' + (index + 1) + '</td><td><b>' + esc(item.code) + '</b>' + (isPeak ? (forPrint ? ' ★' : ' <i class="fa fa-star" style="color:#f5a623"></i>') : '') + '</td>' +
                        '<td>' + esc(airportName(item.code)) + '</td><td>' + number(item.departures) + '</td><td>' + number(item.arrivals) + '</td><td>' + number(item.total) + '</td>' +
                        '<td>' + (grandTotal ? (item.total * 100 / grandTotal).toFixed(1) : '0') + '%</td></tr>';
                }).join('') + '</tbody></table>';
        }

        function toggleReport() {
            var card = app.querySelector('#rnAirportReportCard');
            if (!card || !summaryData) return;
            if (!card.hidden) { card.hidden = true; return; }
            card.innerHTML = '<div class="rn-airport-detail-head"><div><h2>Báo cáo lưu lượng cất/hạ cánh theo sân bay</h2>' +
                '<p class="rn-card-subtitle">Từ ' + esc(from.value) + ' đến ' + esc(to.value) + ' • Nguồn: ' + esc(summaryData.source || '') + '</p></div>' +
                '<div class="rn-military-actions"><button type="button" class="rn-filter-button rn-pdf-button" id="rnAirportReportPrint"><i class="fa fa-print"></i> In / PDF</button>' +
                '<button type="button" class="rn-filter-button" id="rnAirportReportClose">Đóng</button></div></div>' +
                '<div class="rn-table-wrap">' + reportTableHtml(false) + '</div>';
            card.hidden = false;
            card.querySelector('#rnAirportReportClose').onclick = function () { card.hidden = true; };
            card.querySelector('#rnAirportReportPrint').onclick = function () {
                window.ReportControls.printReport({
                    title: 'Báo cáo lưu lượng cất/hạ cánh theo sân bay',
                    subtitle: 'Xuất lúc ' + new Date().toLocaleString('vi-VN'),
                    meta: [
                        { label: 'Từ ngày', value: from.value },
                        { label: 'Đến ngày', value: to.value },
                        { label: 'Sân bay lưu lượng cao nhất', value: (summaryData.peakAirport || '—') + ' (' + number(summaryData.peakTotal) + ' lượt)' }
                    ],
                    sections: [{ heading: 'Bảng dữ liệu theo sân bay', html: reportTableHtml(true) }]
                });
            };
            card.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }

        function loadDetails() {
            if (!detailState) return;
            var detail = app.querySelector('#rnAirportDetail');
            detail.innerHTML = '<div class="rn-airport-detail-placeholder"><i class="fa fa-spinner fa-spin"></i><strong>Đang tải danh sách chuyến bay...</strong></div>';
            post('GetDetails', {
                fromDate: from.value, toDate: to.value, airport: detailState.airport,
                movement: detailState.movement, page: detailState.page, pageSize: detailState.pageSize
            }).then(renderDetails).catch(function (error) {
                detail.innerHTML = '<div class="rn-airport-detail-placeholder rn-airport-error"><strong>Không tải được dữ liệu</strong><span>' + esc(error.message) + '</span></div>';
            });
        }

        function loadSummary() {
            apply.disabled = true;
            apply.textContent = 'Đang tải...';
            detailState = null;
            post('GetSummary', { fromDate: from.value, toDate: to.value, airport: airport.value })
                .then(renderSummary)
                .catch(function (error) { alert(error.message); })
                .then(function () {
                    apply.disabled = false;
                    apply.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
                });
        }

        apply.onclick = loadSummary;
        reportButton.onclick = toggleReport;
        loadSummary();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
