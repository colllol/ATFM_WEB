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
            return flights;
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
                    page = 1;
                    renderBars();
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
            app.querySelector('#rnOverviewListTitle').textContent = title + ' (' + number(flights.length) + ')';
            app.querySelector('#rnOverviewPageInfo').textContent = 'Trang ' + page + '/' + pages + ' • ' + number(flights.length) + ' dòng';
            app.querySelector('#rnOverviewPrev').disabled = page <= 1;
            app.querySelector('#rnOverviewNext').disabled = page >= pages;
        }

        function render(result) {
            data = result;
            selectedBar = null;
            page = 1;
            syncAirportOptions(data.airports);
            var grid = '<div class="rn-grid"><article class="rn-card wide"><h2>C\u1ea5t/h\u1ea1 c\u00e1nh theo s\u00e2n bay</h2><p class="rn-card-subtitle">Ch\u1ecdn c\u1ed9t \u0111\u1ec3 l\u1ecdc danh s\u00e1ch chi ti\u1ebft chuy\u1ebfn bay</p><div class="rn-airport-chart-scroll rn-overview-chart-scroll"><div class="rn-airport-chart" id="rnOverviewBars"></div></div><div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>C\u1ea5t c\u00e1nh</span><span><i class="rn-dot" style="background:#20b486"></i>H\u1ea1 c\u00e1nh</span></div></article>' +
                '<article class="rn-card wide rn-flight-list-card"><div class="rn-flight-list-head"><div><h2 id="rnOverviewListTitle">Danh s\u00e1ch chuy\u1ebfn bay</h2><p class="rn-card-subtitle">Chi ti\u1ebft c\u00e1c chuy\u1ebfn bay theo b\u1ed9 l\u1ecdc v\u00e0 s\u00e2n bay \u0111\u00e3 ch\u1ecdn</p></div><label class="rn-page-size">S\u1ed1 d\u00f2ng <select id="rnOverviewPageSize"><option>25</option><option>50</option><option selected>100</option><option>200</option><option>500</option><option>1000</option></select></label></div>' +
                '<div class="rn-table-wrap rn-flight-detail-wrap"><table class="rn-table rn-flight-detail-table"><thead><tr><th>STT</th><th>CALLSIGN</th><th>OPER</th><th>REGISTRATION</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ATDDAY</th><th>ATADAY</th><th>EOBTDAY</th><th>TR\u1ea0NG TH\u00c1I</th></tr></thead><tbody id="rnOverviewRows"></tbody></table></div>' +
                '<div class="rn-flight-pager"><button type="button" class="rn-filter-button" id="rnOverviewPrev">\u2039 Tr\u01b0\u1edbc</button><span id="rnOverviewPageInfo"></span><button type="button" class="rn-filter-button" id="rnOverviewNext">Sau \u203a</button></div></article></div>';
            var oldGrid = app.querySelector('.rn-grid');
            if (oldGrid) oldGrid.outerHTML = grid;
            app.querySelector('#rnOverviewPageSize').onchange = function () { pageSize = parseInt(this.value, 10); page = 1; renderPage(); };
            app.querySelector('#rnOverviewPrev').onclick = function () { if (page > 1) { page--; renderPage(); } };
            app.querySelector('#rnOverviewNext').onclick = function () { var pages = Math.max(1, Math.ceil(detailFlights().length / pageSize)); if (page < pages) { page++; renderPage(); } };
            renderBars();
            renderPage();
        }

        function load() {
            apply.disabled = true;
            apply.textContent = 'Đang tải...';
            var currentDay = from.value === formatDate(today) && to.value === formatDate(today);
            post('GetData', { fromDate: from.value, toDate: to.value, airport: airport.value, currentDay: currentDay })
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
