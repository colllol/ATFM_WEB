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
        return fetch(window.location.pathname + '/' + method, {
            method: 'POST', headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        }).then(function (response) {
            return response.text().then(function (text) {
                var result;
                try { result = JSON.parse(text); }
                catch (_) { throw new Error('Không nhận được dữ liệu JSON. Vui lòng đăng nhập lại hoặc thử lại.'); }
                result = result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result;
                if (!response.ok || !result || (result.Code && result.Code !== '00'))
                    throw new Error(result && result.Message || 'Không thể tải dữ liệu báo cáo.');
                return camelize(Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result);
            });
        });
    }

    function airportSeries(flights, code, fromDate, toDate) {
        var daily = fromDate.slice(0, 7) === toDate.slice(0, 7);
        var start = new Date(fromDate + 'T00:00:00Z'), end = new Date(toDate + 'T00:00:00Z');
        if (!daily) start.setUTCDate(1);
        var buckets = [], lookup = {};
        while (start <= end) {
            var key = start.toISOString().slice(0, daily ? 10 : 7);
            var item = { key: key, label: daily ? key.slice(8) + '/' + key.slice(5,7) : key.slice(5,7) + '/' + key.slice(0,4), departures: 0, arrivals: 0 };
            buckets.push(item); lookup[key] = item;
            if (daily) start.setUTCDate(start.getUTCDate() + 1); else start.setUTCMonth(start.getUTCMonth() + 1);
        }
        flights.forEach(function (flight) {
            var date = String(flight.flightDate || '').slice(0,10);
            if (date < fromDate || date > toDate) return;
            var bucket = lookup[date.slice(0, daily ? 10 : 7)];
            if (!bucket) return;
            if (flight.fromAirp === code) bucket.departures++;
            if (flight.toAirp === code) bucket.arrivals++;
        });
        return { daily: daily, buckets: buckets };
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
        app.querySelector('.rn-filters').insertAdjacentHTML('afterend', '<div class="rn-kpis" id="rnOverviewKpis"></div>');
        var airport = app.querySelector('#rnAirport');
        var from = app.querySelector('#rnFrom');
        var to = app.querySelector('#rnTo');
        var apply = app.querySelector('#rnApply');
        app.querySelector('.rn-filters').insertAdjacentHTML('beforeend', '<button class="rn-filter-button rn-export-button" type="button" id="rnOverviewExport"><i class="fa fa-file-excel-o"></i> Export Excel</button>');
        if (window.ReportControls && window.ReportControls.enhanceAll) window.ReportControls.enhanceAll(app.querySelector('.rn-filters'));
        var today = new Date();
        var firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
        var data = { flights: [] };
        var appliedFrom = '', appliedTo = '';
        app.querySelector('.rn-filters').insertAdjacentHTML('afterend', '<p id="rnOverviewLoadState" role="status" aria-live="polite" hidden></p>');
        var loadState = app.querySelector('#rnOverviewLoadState');
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

        function renderKpis() {
            var items = [
                { label: 'T?ng chuy?n bay', value: data.total, color: '#2387c8' },
                { label: 'Ho?n th?nh', value: data.finished, color: '#20b486' },
                { label: 'Delay', value: data.delay, color: '#f5a623' },
                { label: 'S?n bay khai th?c', value: data.airportCount, color: '#8a6ee8' }
            ];
            app.querySelector('#rnOverviewKpis').innerHTML = items.map(function (item) {
                return '<div class="rn-kpi" style="--accent:' + item.color + '"><span>' + item.label + '</span><strong>' + number(item.value) + '</strong></div>';
            }).join('');
        }

        function renderStatusDonut() {
            var finished = Number(data.finished || 0), delay = Number(data.delay || 0), total = finished + delay;
            var radius = 82, circumference = 2 * Math.PI * radius, offset = 0;
            var values = [
                { key: 'finished', label: 'Ho?n th?nh', value: finished, color: '#20b486' },
                { key: 'delay', label: 'Delay', value: delay, color: '#f5a623' }
            ];
            var segments = values.map(function (item) {
                var length = total ? item.value / total * circumference : 0;
                var segment = '<circle class="rn-donut-segment" data-status="' + item.key + '" data-value="' + item.value + '" cx="120" cy="120" r="' + radius + '" stroke="' + item.color + '" stroke-dasharray="' + length + ' ' + (circumference - length) + '" stroke-dashoffset="-' + offset + '"></circle>';
                offset += length;
                return segment;
            }).join('');
            var cards = values.map(function (item) {
                var percent = total ? Math.round(item.value / total * 100) : 0;
                return '<div class="rn-status" style="--status-color:' + item.color + '" data-status="' + item.key + '"><span>' + item.label + '</span><b>' + number(item.value) + '</b><small>' + percent + '%</small></div>';
            }).join('');
            app.querySelector('#rnOverviewDonut').innerHTML = '<div class="rn-donut-layout"><div class="rn-donut"><svg viewBox="0 0 240 240" role="img" aria-label="C? c?u tr?ng th?i chuy?n bay"><circle class="rn-donut-track" cx="120" cy="120" r="' + radius + '"></circle>' + segments + '</svg><div class="rn-donut-center"><strong>' + number(total) + '</strong><span>T?ng chuy?n</span></div></div><div class="rn-status-list">' + cards + '</div></div>';
        }

        function renderBars() {
            var items = data.airports || [];
            if (!items.length) {
                var codes = airport.value && airport.value !== 'ALL' ? [airport.value] : Object.keys(airportNames);
                items = codes.map(function (code) { return { code: code, departures: 0, arrivals: 0 }; });
            }
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
                    renderAirportTrend();
                    renderPage();
                };
            });
        }

        function renderAirportTrend() {
            var host = app.querySelector('#rnAirportTrend');
            host.hidden = !selectedBar;
            if (!selectedBar) { host.innerHTML = ''; return; }
            var series = airportSeries(data.flights || [], selectedBar.code, appliedFrom, appliedTo);
            var width = Math.max(760, series.buckets.length * 58), height = 300, left = 55, top = 25, plotH = 225;
            var maximum = Math.max(1, series.buckets.reduce(function (m, b) { return Math.max(m,b.departures,b.arrivals); }, 0));
            var step = (width - left - 20) / Math.max(1,series.buckets.length), parts = [];
            for (var g=0;g<=4;g++) {
                var y=top+plotH*g/4;
                parts.push('<line x1="'+left+'" y1="'+y+'" x2="'+(width-20)+'" y2="'+y+'" stroke="#e0e9f0"/><text x="'+(left-8)+'" y="'+(y+4)+'" text-anchor="end" font-size="11">'+Math.round(maximum*(4-g)/4)+'</text>');
            }
            series.buckets.forEach(function (bucket,i) {
                ['departures','arrivals'].forEach(function (key,j) {
                    var value=bucket[key], h=value/maximum*plotH, x=left+i*step+step*(j ? .52 : .17);
                    parts.push('<rect x="'+x+'" y="'+(top+plotH-h)+'" width="'+(step*.3)+'" height="'+h+'" fill="'+(j ? '#20b486' : '#2387c8')+'"><title>'+esc(bucket.label)+': '+(j ? 'Hạ cánh' : 'Cất cánh')+' '+value+'</title></rect>');
                });
                parts.push('<text x="'+(left+step*(i+.5))+'" y="280" text-anchor="middle" font-size="11">'+esc(bucket.label)+'</text>');
            });
            host.innerHTML='<h2>'+esc(selectedBar.code+' - '+airportName(selectedBar.code))+'</h2><p class="rn-card-subtitle">Số lượt cất/hạ cánh theo '+(series.daily ? 'ngày' : 'tháng')+'</p><div style="overflow-x:auto"><svg role="img" aria-label="Biểu đồ cất hạ cánh theo thời gian" viewBox="0 0 '+width+' '+height+'" style="width:100%;min-width:'+width+'px">'+parts.join('')+'</svg></div><div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>Cất cánh</span><span><i class="rn-dot" style="background:#20b486"></i>Hạ cánh</span></div>';
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
            appliedFrom = result.fromDate; appliedTo = result.toDate;
            selectedBar = null;
            if (liveNote) liveNote.textContent = 'Nguồn ' + result.source;
            page = 1;
            syncAirportOptions(data.airports);
            var grid = '<div class="rn-grid"><article class="rn-card wide"><h2>C? c?u tr?ng th?i chuy?n bay</h2><p class="rn-card-subtitle">Chuy?n ho?n th?nh v? chuy?n delay trong k?</p><div id="rnOverviewDonut"></div></article><article class="rn-card wide"><h2>C\u1ea5t/h\u1ea1 c\u00e1nh theo s\u00e2n bay</h2><p class="rn-card-subtitle">Ch\u1ecdn c\u1ed9t \u0111\u1ec3 l\u1ecdc danh s\u00e1ch chi ti\u1ebft chuy\u1ebfn bay</p><div class="rn-airport-chart-scroll rn-overview-chart-scroll"><div class="rn-airport-chart" id="rnOverviewBars"></div></div><div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>C\u1ea5t c\u00e1nh</span><span><i class="rn-dot" style="background:#20b486"></i>H\u1ea1 c\u00e1nh</span></div></article>' +
                '<article id="rnAirportTrend" class="rn-card wide" hidden></article><article class="rn-card wide rn-flight-list-card"><div class="rn-flight-list-head"><div><h2 id="rnOverviewListTitle">Danh s\u00e1ch chuy\u1ebfn bay</h2><p class="rn-card-subtitle">Chi ti\u1ebft c\u00e1c chuy\u1ebfn bay theo b\u1ed9 l\u1ecdc v\u00e0 s\u00e2n bay \u0111\u00e3 ch\u1ecdn</p></div><label class="rn-page-size">S\u1ed1 d\u00f2ng <select id="rnOverviewPageSize"><option>25</option><option>50</option><option selected>100</option><option>200</option><option>500</option><option>1000</option></select></label></div>' +
                '<div class="rn-table-wrap rn-flight-detail-wrap"><table class="rn-table rn-flight-detail-table"><thead><tr><th>STT</th><th>CALLSIGN</th><th>OPER</th><th>REGISTRATION</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ATDDAY</th><th>ATADAY</th><th>EOBTDAY</th><th>TR\u1ea0NG TH\u00c1I</th></tr></thead><tbody id="rnOverviewRows"></tbody></table></div>' +
                '<div class="rn-flight-pager"><button type="button" class="rn-filter-button" id="rnOverviewPrev">\u2039 Tr\u01b0\u1edbc</button><span id="rnOverviewPageInfo"></span><button type="button" class="rn-filter-button" id="rnOverviewNext">Sau \u203a</button></div></article></div>';
            var oldGrid = app.querySelector('.rn-grid');
            if (oldGrid) oldGrid.outerHTML = grid;
            app.querySelector('#rnOverviewPageSize').onchange = function () { pageSize = parseInt(this.value, 10); page = 1; renderPage(); };
            app.querySelector('#rnOverviewPrev').onclick = function () { if (page > 1) { page--; renderPage(); } };
            app.querySelector('#rnOverviewNext').onclick = function () { var pages = Math.max(1, Math.ceil(detailFlights().length / pageSize)); if (page < pages) { page++; renderPage(); } };
            renderKpis();
            renderStatusDonut();
            renderBars();
            renderAirportTrend();
            renderPage();
        }

        function load() {
            if (!from.value || !to.value || from.value > to.value || to.value > formatDate(new Date())) { alert('Khoảng ngày lọc không hợp lệ.'); return; }
            loadState.hidden = false;
            loadState.textContent = 'Đang tải dữ liệu khai thác bay…';
            apply.disabled = true;
            apply.textContent = 'Đang tải...';
            post('GetData', { fromDate: from.value, toDate: to.value, airport: airport.value })
                .then(function (result) {
                    render(result);
                    loadState.textContent = (result.flights || []).length ? '' : 'Không có chuyến bay trong khoảng lọc đã chọn.';
                    loadState.hidden = !!(result.flights || []).length;
                })
                .catch(function (error) {
                    loadState.hidden = false;
                    loadState.textContent = 'Không tải được dữ liệu: ' + error.message;
                    if (!appliedFrom) app.querySelector('.rn-grid').innerHTML = '<article class="rn-card wide"><p>Chưa có dữ liệu báo cáo. Kiểm tra thông báo phía trên và nhấn Áp dụng để thử lại.</p></article>';
                })
                .then(function () {
                    apply.disabled = false;
                    apply.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
                });
        }

        apply.onclick = load;
        app.querySelector('#rnOverviewExport').onclick = function () {
            var rows = detailFlights();
            if (!rows.length) { alert('Không có dữ liệu để xuất Excel.'); return; }
            if (!window.ReportControls || !window.ReportControls.exportExcel) { alert('Chức năng xuất Excel chưa sẵn sàng.'); return; }
            window.ReportControls.exportExcel({
                fileName: 'FlightOperationOverview_' + appliedFrom + '_' + appliedTo,
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
        render({ flights: [], airports: [], fromDate: from.value, toDate: to.value, source: 'd? li?u khai th?c bay' });
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
