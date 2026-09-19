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
        return fetch(window.reportApiBase + 'api/FlightOperationOverview/' + method, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        }).then(function (response) {
            return response.json().then(function (result) {
                if (!response.ok || (result.Code && result.Code !== '00')) throw new Error(result.Message || 'Không thể tải dữ liệu báo cáo.');
                return camelize(Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result.d);
            });
        });
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : value;
        return node.innerHTML;
    }

    function number(value) { return Number(value || 0).toLocaleString('vi-VN'); }
    function airportName(code) { return airportNames[code] || code; }
    function statusKey(value) { return String(value || '').indexOf('DELAY') === 0 ? 'delay' : 'finished'; }
    function statusColor(value) { return statusKey(value) === 'finished' ? '#20b486' : '#f5a623'; }
    function statusLabel(value) {
        if (value === 'FINISHED') return 'Hoàn thành';
        return String(value || '').replace('DELAY_', 'Delay ').replace('_PLUS', '+').replace('_', '–') + ' phút';
    }

    function formatDate(value) {
        return value.getFullYear() + '-' + String(value.getMonth() + 1).padStart(2, '0') + '-' + String(value.getDate()).padStart(2, '0');
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="overview"]');
        if (!app) return;
        var airport = app.querySelector('#rnAirport');
        var from = app.querySelector('#rnFrom');
        var to = app.querySelector('#rnTo');
        var apply = app.querySelector('#rnApply');
        app.querySelector('.rn-filters').insertAdjacentHTML('beforeend', '<button class="rn-filter-button rn-export-button" type="button" id="rnOverviewExport"><i class="fa fa-file-excel-o"></i> Export Excel</button>');
        if (window.ReportControls) window.ReportControls.enhanceAll(app.querySelector('.rn-filters'));
        var today = new Date();
        var firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
        var data = null;
        var selectedBar = null;
        var selectedStatus = '';
        var page = 1;
        var pageSize = 100;

        var heroTitle = app.querySelector('.rn-hero h1');
        var heroSubtitle = app.querySelector('.rn-hero p');
        var liveNote = app.querySelector('.rn-live small');
        if (heroTitle) heroTitle.textContent = 'Thông tin tổng quan khai thác bay';
        if (heroSubtitle) heroSubtitle.textContent = 'Tổng hợp chuyến hoàn thành và chuyến delay từ dữ liệu khai thác thực tế';
        if (liveNote) liveNote.textContent = 'Nguồn T_FINISHED_FLIGHTS';

        from.value = formatDate(firstDay);
        to.value = formatDate(today);
        airport.innerHTML = '<option value="ALL">Tất cả sân bay</option>' + Object.keys(airportNames).map(function (code) {
            return '<option value="' + code + '">' + code + ' - ' + airportNames[code] + '</option>';
        }).join('');

        function syncAirportOptions(items) {
            var selected = airport.value;
            (items || []).forEach(function (item) {
                if (!airport.querySelector('option[value="' + item.code + '"]')) {
                    airport.insertAdjacentHTML('beforeend', '<option value="' + esc(item.code) + '">' + esc(item.code + ' - ' + airportName(item.code)) + '</option>');
                }
            });
            airport.value = selected;
        }

        function airportFlights(code) {
            var flights = data.flights || [];
            if (!code) return flights;
            return flights.filter(function (flight) { return flight.fromAirp === code || flight.toAirp === code; });
        }

        function detailFlights() {
            var flights = data.flights || [];
            if (selectedBar) {
                flights = flights.filter(function (flight) {
                    return selectedBar.movement === 'departure'
                        ? flight.fromAirp === selectedBar.code
                        : flight.toAirp === selectedBar.code;
                });
            }
            if (selectedStatus) flights = flights.filter(function (flight) { return statusKey(flight.status) === selectedStatus; });
            return flights;
        }

        function renderKpis() {
            var total = data.total || 0;
            var kpis = '<div class="rn-kpis">' +
                '<div class="rn-kpi" style="--accent:#2387c8"><span>Tổng chuyến bay</span><strong>' + number(total) + '</strong><small>Hoàn thành + Delay</small></div>' +
                '<div class="rn-kpi" style="--accent:#20b486"><span>Chuyến hoàn thành</span><strong>' + number(data.finished) + '</strong><small>' + (total ? (data.finished * 100 / total).toFixed(1) : 0) + '%</small></div>' +
                '<div class="rn-kpi" style="--accent:#f5a623"><span>Chuyến Delay</span><strong>' + number(data.delay) + '</strong><small>' + (total ? (data.delay * 100 / total).toFixed(1) : 0) + '%</small></div>' +
                '<div class="rn-kpi" style="--accent:#8a6ee8"><span>Sân bay khai thác</span><strong>' + number(data.airportCount) + '</strong><small>Nguồn ' + esc(data.source) + '</small></div></div>';
            var old = app.querySelector('.rn-kpis');
            if (old) old.outerHTML = kpis;
        }

        function donutHtml() {
            var flights = airportFlights(selectedBar ? selectedBar.code : '');
            var finished = flights.filter(function (flight) { return statusKey(flight.status) === 'finished'; }).length;
            var delay = flights.length - finished;
            var total = flights.length;
            var items = [
                { key: 'finished', name: 'Hoàn thành', value: finished, color: '#20b486' },
                { key: 'delay', name: 'Delay', value: delay, color: '#f5a623' }
            ];
            var circumference = 2 * Math.PI * 82;
            var offset = 0;
            var segments = items.map(function (item) {
                var length = total ? item.value / total * circumference : 0;
                var segment = '<circle class="rn-donut-segment' + (selectedStatus === item.key ? ' is-active' : '') + '" data-status="' + item.key + '" stroke="' + item.color + '" cx="120" cy="120" r="82" stroke-dasharray="' + length + ' ' + (circumference - length) + '" stroke-dashoffset="' + (-offset) + '"></circle>';
                offset += length;
                return segment;
            }).join('');
            var cards = items.map(function (item) {
                return '<div class="rn-status' + (selectedStatus === item.key ? ' is-active' : '') + '" data-status="' + item.key + '" style="--status-color:' + item.color + '"><span>' + item.name + '</span><b>' + number(item.value) + '</b><small>' + (total ? (item.value * 100 / total).toFixed(1) : 0) + '%</small></div>';
            }).join('');
            var centerValue = selectedStatus ? items.filter(function (item) { return item.key === selectedStatus; })[0].value : total;
            var centerLabel = selectedStatus === 'finished' ? 'Hoàn thành' : selectedStatus === 'delay' ? 'Delay' : selectedBar ? selectedBar.code : 'Tổng chuyến';
            return '<div class="rn-donut-layout"><div class="rn-donut' + (selectedStatus ? ' has-active' : '') + '"><svg viewBox="0 0 240 240"><circle class="rn-donut-track" cx="120" cy="120" r="82"></circle>' + segments + '</svg><div class="rn-donut-center"><strong>' + number(centerValue) + '</strong><span>' + esc(centerLabel) + '</span></div></div><div class="rn-status-list' + (selectedStatus ? ' has-active' : '') + '">' + cards + '</div></div>';
        }

        function renderDonut() {
            var holder = app.querySelector('#rnOverviewDonut');
            holder.innerHTML = donutHtml();
            Array.prototype.forEach.call(holder.querySelectorAll('.rn-status,.rn-donut-segment'), function (node) {
                node.onclick = function () {
                    var key = this.getAttribute('data-status');
                    selectedStatus = selectedStatus === key ? '' : key;
                    page = 1;
                    renderDonut();
                    renderPage();
                };
            });
            var subtitle = app.querySelector('#rnDonutSubtitle');
            subtitle.textContent = selectedBar
                ? selectedBar.code + ' - ' + airportName(selectedBar.code) + ' • Hoàn thành + Delay'
                : 'Tất cả sân bay • Hoàn thành + Delay';
        }

        function renderBars() {
            var items = data.airports || [];
            var max = 0;
            items.forEach(function (item) { max = Math.max(max, item.departures, item.arrivals); });
            var bars = items.map(function (item) {
                var departureHeight = item.departures ? Math.max(4, item.departures / Math.max(1, max) * 100) : 0;
                var arrivalHeight = item.arrivals ? Math.max(4, item.arrivals / Math.max(1, max) * 100) : 0;
                return '<div class="rn-airport-bar-group"><div class="rn-airport-bar-pair">' +
                    '<button type="button" class="rn-airport-bar departure" data-airport="' + item.code + '" data-movement="departure" style="height:' + departureHeight + '%"><span>' + number(item.departures) + '</span></button>' +
                    '<button type="button" class="rn-airport-bar arrival" data-airport="' + item.code + '" data-movement="arrival" style="height:' + arrivalHeight + '%"><span>' + number(item.arrivals) + '</span></button>' +
                    '</div><b>' + esc(item.code) + '</b><small>' + esc(airportName(item.code)) + '</small></div>';
            }).join('');
            if (!bars) bars = '<div class="rn-airport-empty">Không có chuyến hoàn thành hoặc delay trong khoảng ngày đã chọn.</div>';
            app.querySelector('#rnOverviewBars').innerHTML = bars;
            Array.prototype.forEach.call(app.querySelectorAll('.rn-airport-bar'), function (bar) {
                var active = selectedBar && selectedBar.code === bar.getAttribute('data-airport') && selectedBar.movement === bar.getAttribute('data-movement');
                bar.classList.toggle('is-active', !!active);
                bar.onclick = function () {
                    var clicked = { code: this.getAttribute('data-airport'), movement: this.getAttribute('data-movement') };
                    if (selectedBar && selectedBar.code === clicked.code && selectedBar.movement === clicked.movement) selectedBar = null;
                    else selectedBar = clicked;
                    selectedStatus = '';
                    page = 1;
                    renderBars();
                    renderDonut();
                    renderPage();
                };
            });
        }

        function renderPage() {
            var flights = detailFlights();
            var pages = Math.max(1, Math.ceil(flights.length / pageSize));
            if (page > pages) page = pages;
            var start = (page - 1) * pageSize;
            var rows = flights.slice(start, start + pageSize).map(function (flight, index) {
                return '<tr><td class="rn-stt">' + (start + index + 1) + '</td><td><b>' + esc(flight.callsign) + '</b></td><td>' + esc(flight.oper) + '</td><td>' + esc(flight.registration) + '</td><td>' + esc(flight.permType) + '</td><td>' + esc(flight.fromAirp) + '</td><td>' + esc(flight.toAirp) + '</td><td>' + esc(flight.atdDay) + '</td><td>' + esc(flight.ataDay) + '</td><td>' + esc(flight.eobtDay) + '</td><td><span class="rn-pill" style="background:' + statusColor(flight.status) + ';color:#fff">' + esc(statusLabel(flight.status)) + '</span></td></tr>';
            }).join('');
            if (!rows) rows = '<tr><td colspan="11" class="rn-no-data">Không có chuyến bay phù hợp.</td></tr>';
            app.querySelector('#rnOverviewRows').innerHTML = rows;
            var title = 'Danh sách chuyến bay';
            if (selectedBar) title += ' ' + (selectedBar.movement === 'departure' ? 'cất cánh' : 'hạ cánh') + ' tại ' + selectedBar.code;
            if (selectedStatus) title += ' - ' + (selectedStatus === 'finished' ? 'Hoàn thành' : 'Delay');
            app.querySelector('#rnOverviewListTitle').textContent = title + ' (' + number(flights.length) + ')';
            app.querySelector('#rnOverviewPageInfo').textContent = 'Trang ' + page + '/' + pages + ' • ' + number(flights.length) + ' dòng';
            app.querySelector('#rnOverviewPrev').disabled = page <= 1;
            app.querySelector('#rnOverviewNext').disabled = page >= pages;
        }

        function render(result) {
            data = result;
            selectedBar = null;
            selectedStatus = '';
            page = 1;
            syncAirportOptions(data.airports);
            renderKpis();
            var grid = '<div class="rn-grid"><article class="rn-card"><h2>Cơ cấu trạng thái</h2><p class="rn-card-subtitle" id="rnDonutSubtitle"></p><div id="rnOverviewDonut"></div></article>' +
                '<article class="rn-card"><h2>Cất/hạ cánh theo sân bay</h2><p class="rn-card-subtitle">Nhấn vào từng cột để cập nhật biểu đồ trạng thái và danh sách chi tiết</p><div class="rn-airport-chart-scroll rn-overview-chart-scroll"><div class="rn-airport-chart" id="rnOverviewBars"></div></div><div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>Cất cánh</span><span><i class="rn-dot" style="background:#20b486"></i>Hạ cánh</span></div></article>' +
                '<article class="rn-card wide rn-flight-list-card"><div class="rn-flight-list-head"><div><h2 id="rnOverviewListTitle">Danh sách chuyến bay</h2><p class="rn-card-subtitle">Dữ liệu chi tiết giống báo cáo trạng thái; có thể lọc tiếp bằng cột sân bay và biểu đồ tròn</p></div><label class="rn-page-size">Số dòng <select id="rnOverviewPageSize"><option>25</option><option>50</option><option selected>100</option><option>200</option><option>500</option><option>1000</option></select></label></div>' +
                '<div class="rn-table-wrap rn-flight-detail-wrap"><table class="rn-table rn-flight-detail-table"><thead><tr><th>STT</th><th>CALLSIGN</th><th>OPER</th><th>REGISTRATION</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ATDDAY</th><th>ATADAY</th><th>EOBTDAY</th><th>TRẠNG THÁI</th></tr></thead><tbody id="rnOverviewRows"></tbody></table></div>' +
                '<div class="rn-flight-pager"><button type="button" class="rn-filter-button" id="rnOverviewPrev">‹ Trước</button><span id="rnOverviewPageInfo"></span><button type="button" class="rn-filter-button" id="rnOverviewNext">Sau ›</button></div></article></div>';
            var oldGrid = app.querySelector('.rn-grid');
            if (oldGrid) oldGrid.outerHTML = grid;
            app.querySelector('#rnOverviewPageSize').onchange = function () { pageSize = parseInt(this.value, 10); page = 1; renderPage(); };
            app.querySelector('#rnOverviewPrev').onclick = function () { if (page > 1) { page--; renderPage(); } };
            app.querySelector('#rnOverviewNext').onclick = function () { var pages = Math.max(1, Math.ceil(detailFlights().length / pageSize)); if (page < pages) { page++; renderPage(); } };
            renderBars();
            renderDonut();
            renderPage();
        }

        function load() {
            apply.disabled = true;
            apply.textContent = 'Đang tải...';
            post('GetData', { fromDate: from.value, toDate: to.value, airport: airport.value })
                .then(render)
                .catch(function (error) { alert(error.message); })
                .then(function () {
                    apply.disabled = false;
                    apply.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
                });
        }

        apply.onclick = load;
        app.querySelector('#rnOverviewExport').onclick = function () {
            var rows = detailFlights();
            if (!rows.length) { alert('Không có dữ liệu để xuất Excel.'); return; }
            window.ReportControls.exportExcel({
                fileName: 'FlightOperationOverview_' + from.value + '_' + to.value,
                rows: rows,
                columns: [
                    { label: 'No', key: '__no' }, { label: 'CALLSIGN', key: 'callsign' },
                    { label: 'OPER', key: 'oper' }, { label: 'REGISTRATION', key: 'registration' },
                    { label: 'PERMTYPE', key: 'permType' }, { label: 'FROM_AIRP', key: 'fromAirp' },
                    { label: 'TO_AIRP', key: 'toAirp' }, { label: 'ATDDAY', key: 'atdDay' },
                    { label: 'ATADAY', key: 'ataDay' }, { label: 'EOBTDAY', key: 'eobtDay' },
                    { label: 'TRẠNG THÁI', key: 'status', format: statusLabel }
                ]
            });
        };
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
