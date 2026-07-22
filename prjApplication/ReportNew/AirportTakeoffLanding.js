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
            .then(function (response) { return response.json(); })
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
            syncAirportOptions(data.airports || []);
            var kpis = '<div class="rn-kpis">' +
                '<div class="rn-kpi" style="--accent:#2387c8"><span>Tổng cất cánh</span><strong>' + number(data.totalDepartures) + '</strong><small>Finished + Delay</small></div>' +
                '<div class="rn-kpi" style="--accent:#20b486"><span>Tổng hạ cánh</span><strong>' + number(data.totalArrivals) + '</strong><small>Finished + Delay</small></div>' +
                '<div class="rn-kpi" style="--accent:#8a6ee8"><span>Sân bay khai thác</span><strong>' + number(data.airportCount) + '</strong><small>Sân bay Việt Nam</small></div>' +
                '<div class="rn-kpi" style="--accent:#f5a623"><span>Lưu lượng cao nhất</span><strong>' + esc(data.peakAirport || '—') + '</strong><small>' + number(data.peakTotal) + ' lượt</small></div></div>';
            var max = 0;
            (data.airports || []).forEach(function (item) { max = Math.max(max, item.departures, item.arrivals); });
            var bars = (data.airports || []).map(function (item) {
                var departureHeight = item.departures ? Math.max(4, item.departures / Math.max(1, max) * 100) : 0;
                var arrivalHeight = item.arrivals ? Math.max(4, item.arrivals / Math.max(1, max) * 100) : 0;
                return '<div class="rn-airport-bar-group"><div class="rn-airport-bar-pair">' +
                    '<button type="button" class="rn-airport-bar departure" data-airport="' + item.code + '" data-movement="departure" style="height:' + departureHeight + '%" title="' + esc(item.code + ' - Cất cánh: ' + number(item.departures)) + '"><span>' + number(item.departures) + '</span></button>' +
                    '<button type="button" class="rn-airport-bar arrival" data-airport="' + item.code + '" data-movement="arrival" style="height:' + arrivalHeight + '%" title="' + esc(item.code + ' - Hạ cánh: ' + number(item.arrivals)) + '"><span>' + number(item.arrivals) + '</span></button>' +
                    '</div><b>' + esc(item.code) + '</b><small>' + esc(airportName(item.code)) + '</small></div>';
            }).join('');
            if (!bars) bars = '<div class="rn-airport-empty">Không có chuyến bay Finished hoặc Delay trong khoảng ngày đã chọn.</div>';

            var grid = '<div class="rn-grid"><article class="rn-card wide"><h2>Lưu lượng cất/hạ cánh theo sân bay</h2>' +
                '<p class="rn-card-subtitle">Nhấn trực tiếp vào từng cột để xem danh sách chuyến bay tương ứng • Nguồn: ' + esc(data.source) + '</p>' +
                '<div class="rn-airport-chart-scroll"><div class="rn-airport-chart">' + bars + '</div></div>' +
                '<div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>Cất cánh</span><span><i class="rn-dot" style="background:#20b486"></i>Hạ cánh</span></div></article>' +
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
        loadSummary();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
