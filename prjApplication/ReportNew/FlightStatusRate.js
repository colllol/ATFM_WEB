(function () {
    var currentStatusFlights = [];
    var defaultOperators = [
        'AAR', 'APG', 'AXM', 'BAV', 'CAL', 'CEB', 'CES', 'CQH', 'DKH', 'ETD',
        'FDX', 'HVN', 'JAL', 'KAL', 'KHV', 'KLM', 'MAS', 'MKR', 'MXD', 'PIC',
        'QTR', 'SIA', 'THA', 'UAE', 'UAL', 'VAG', 'VJC'
    ];

    function camelize(value) {
        if (Array.isArray(value)) return value.map(camelize);
        if (!value || typeof value !== 'object') return value;
        var result = {};
        Object.keys(value).forEach(function (key) { result[key.charAt(0).toLowerCase() + key.slice(1)] = camelize(value[key]); });
        return result;
    }
    function post(method, data) {
        return fetch(window.location.pathname + '/' + method, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        })
            .then(function (response) {
                return response.text().then(function (text) {
                    var result;
                    try {
                        result = text ? JSON.parse(text) : null;
                    } catch (parseError) {
                        var title = /<title[^>]*>([\s\S]*?)<\/title>/i.exec(text || '');
                        var serverMessage = title
                            ? title[1].replace(/<[^>]+>/g, ' ').replace(/\s+/g, ' ').trim()
                            : '';
                        throw new Error(
                            'HTTP ' + response.status + ': Server returned HTML instead of JSON' +
                            (serverMessage ? ' (' + serverMessage + ')' : '')
                        );
                    }
                    if (!response.ok) throw new Error(result.Message || result.message || ('HTTP ' + response.status));
                    return result;
                });
            })
            .then(function (result) {
                result = result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result;
                if (result.Code && result.Code !== '00') throw new Error(result.Message || 'Không thể tải dữ liệu.');
                return camelize(Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result);
            });
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : value;
        return node.innerHTML;
    }

    function statusKey(state) {
        state = state || '';
        return state.indexOf('DELAY') === 0 ? 'delay' : state.toLowerCase();
    }

    function statusColor(state) {
        return state === 'FINISHED' ? '#20b486' :
            state === 'CANCEL' ? '#ef5b5b' :
                state.indexOf('DELAY') === 0 ? '#f5a623' : '#4d8df7';
    }

    function statusLabel(state) {
        if (state === 'FINISHED') return 'Hoàn thành';
        if (state === 'CANCEL') return 'Hủy';
        if (state === 'WAIT') return 'Chưa thực hiện';
        return state.replace('DELAY_', 'Delay ').replace('_PLUS', '+').replace('_', '–') + ' phút';
    }

    function render(app, data) {
        var items = [
            { key: 'finished', name: 'Hoàn thành', value: data.finished, color: '#20b486' },
            { key: 'cancel', name: 'Hủy', value: data.cancel, color: '#ef5b5b' },
            { key: 'delay', name: 'Delay', value: data.delay, color: '#f5a623' },
            { key: 'wait', name: 'Chưa thực hiện', value: data.wait, color: '#4d8df7' }
        ];
        var flights = data.flights || [];
        currentStatusFlights = flights;
        var total = data.total || 0;
        var circumference = 2 * Math.PI * 82;
        var offset = 0;
        var page = 1;
        var pageSize = 100;
        var selectedStatus = '';

        var segments = items.map(function (item) {
            var length = total ? item.value / total * circumference : 0;
            var html = '<circle class="rn-donut-segment" data-status="' + item.key + '" data-value="' + item.value + '" stroke="' + item.color + '" cx="120" cy="120" r="82" stroke-dasharray="' + length + ' ' + (circumference - length) + '" stroke-dashoffset="' + (-offset) + '"></circle>';
            offset += length;
            return html;
        }).join('');

        var cards = items.map(function (item) {
            return '<div class="rn-status" data-status="' + item.key + '" style="--status-color:' + item.color + '">' +
                '<span>' + item.name + '</span><b>' + item.value.toLocaleString('vi-VN') + '</b>' +
                '<small>' + (total ? (item.value * 100 / total).toFixed(1) : '0') + '%</small></div>';
        }).join('');

        var kpis = '<div class="rn-kpis"><div class="rn-kpi" style="--accent:#2387c8"><span>Tổng chuyến</span><strong>' + total.toLocaleString('vi-VN') + '</strong><small>100%</small></div>' +
            items.slice(0, 3).map(function (item) {
                return '<div class="rn-kpi" style="--accent:' + item.color + '"><span>' + item.name + '</span><strong>' + item.value.toLocaleString('vi-VN') + '</strong><small>' + (total ? (item.value * 100 / total).toFixed(1) : 0) + '%</small></div>';
            }).join('') + '</div>';

        var grid = '<div class="rn-grid">' +
            '<article class="rn-card wide"><h2>Tỷ lệ trạng thái</h2><p class="rn-card-subtitle">Nguồn: ' + esc(data.source) + ' • Bốn nhóm loại trừ nhau</p>' +
            '<div class="rn-donut-layout"><div class="rn-donut"><svg viewBox="0 0 240 240"><circle class="rn-donut-track" cx="120" cy="120" r="82"></circle>' + segments + '</svg>' +
            '<div class="rn-donut-center"><strong>' + total.toLocaleString('vi-VN') + '</strong><span>Tổng chuyến</span></div></div><div class="rn-status-list">' + cards + '</div></div></article>' +
            '<article class="rn-card wide rn-flight-list-card"><div class="rn-flight-list-head"><div><h2 id="rnFlightListTitle">Danh sách chuyến bay</h2><p class="rn-card-subtitle">Nhấn biểu đồ hoặc thẻ trạng thái để lọc danh sách</p></div>' +
            '<label class="rn-page-size">Số dòng <select id="rnPageSize"><option>25</option><option>50</option><option selected>100</option><option>200</option><option>500</option><option>1000</option></select></label></div>' +
            '<div class="rn-table-wrap rn-flight-detail-wrap"><table class="rn-table rn-flight-detail-table"><thead><tr>' +
            '<th>STT</th><th>CALLSIGN</th><th>OPER</th><th>REGISTRATION</th><th>PERMTYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ATDDAY</th><th>ATADAY</th><th>EOBTDAY</th><th>TRẠNG THÁI</th>' +
            '</tr></thead><tbody id="rnFlightRows"></tbody></table></div>' +
            '<div class="rn-flight-pager"><button type="button" class="rn-filter-button" id="rnPrev">‹ Trước</button><span id="rnPageInfo"></span><button type="button" class="rn-filter-button" id="rnNext">Sau ›</button></div></article></div>';

        var oldKpis = app.querySelector('.rn-kpis');
        var oldGrid = app.querySelector('.rn-grid');
        if (oldKpis) oldKpis.outerHTML = kpis;
        if (oldGrid) oldGrid.outerHTML = grid;

        function filteredFlights() {
            if (!selectedStatus) return flights;
            return flights.filter(function (flight) { return statusKey(flight.status) === selectedStatus; });
        }

        function renderPage() {
            var visible = filteredFlights();
            var pages = Math.max(1, Math.ceil(visible.length / pageSize));
            if (page > pages) page = pages;
            var start = (page - 1) * pageSize;
            var currentRows = visible.slice(start, start + pageSize);
            var body = app.querySelector('#rnFlightRows');

            body.innerHTML = currentRows.map(function (flight, index) {
                return '<tr><td class="rn-stt">' + (start + index + 1) + '</td>' +
                    '<td><b>' + esc(flight.callsign) + '</b></td>' +
                    '<td>' + esc(flight.oper) + '</td>' +
                    '<td>' + esc(flight.registration) + '</td>' +
                    '<td>' + esc(flight.permType) + '</td>' +
                    '<td>' + esc(flight.fromAirp) + '</td>' +
                    '<td>' + esc(flight.toAirp) + '</td>' +
                    '<td>' + esc(flight.atdDay) + '</td>' +
                    '<td>' + esc(flight.ataDay) + '</td>' +
                    '<td>' + esc(flight.eobtDay) + '</td>' +
                    '<td><span class="rn-pill" style="background:' + statusColor(flight.status) + ';color:#fff">' + esc(statusLabel(flight.status)) + '</span></td></tr>';
            }).join('');

            if (!currentRows.length) {
                body.innerHTML = '<tr><td colspan="11" class="rn-no-data">Không có chuyến bay phù hợp.</td></tr>';
            }

            app.querySelector('#rnFlightListTitle').textContent = 'Danh sách chuyến bay (' + visible.length.toLocaleString('vi-VN') + ')';
            app.querySelector('#rnPageInfo').textContent = 'Trang ' + page + '/' + pages + ' • ' + visible.length.toLocaleString('vi-VN') + ' dòng';
            app.querySelector('#rnPrev').disabled = page <= 1;
            app.querySelector('#rnNext').disabled = page >= pages;
        }

        function selectStatus(key) {
            selectedStatus = selectedStatus === key ? '' : key;
            page = 1;
            Array.prototype.forEach.call(app.querySelectorAll('.rn-status'), function (card) {
                card.classList.toggle('is-active', selectedStatus === card.getAttribute('data-status'));
            });
            Array.prototype.forEach.call(app.querySelectorAll('.rn-donut-segment'), function (segment) {
                segment.classList.toggle('is-active', selectedStatus === segment.getAttribute('data-status'));
            });
            app.querySelector('.rn-donut').classList.toggle('has-active', !!selectedStatus);
            app.querySelector('.rn-status-list').classList.toggle('has-active', !!selectedStatus);
            renderPage();
        }

        app.querySelector('#rnPageSize').onchange = function () {
            pageSize = parseInt(this.value, 10);
            page = 1;
            renderPage();
        };
        app.querySelector('#rnPrev').onclick = function () {
            if (page > 1) { page--; renderPage(); }
        };
        app.querySelector('#rnNext').onclick = function () {
            var pages = Math.max(1, Math.ceil(filteredFlights().length / pageSize));
            if (page < pages) { page++; renderPage(); }
        };
        Array.prototype.forEach.call(app.querySelectorAll('.rn-status,.rn-donut-segment'), function (node) {
            node.onclick = function () { selectStatus(this.getAttribute('data-status')); };
        });
        renderPage();
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="status"]');
        if (!app) return;
        var today = new Date();
        var iso = today.getFullYear() + '-' + String(today.getMonth() + 1).padStart(2, '0') + '-' + String(today.getDate()).padStart(2, '0');
        var from = app.querySelector('#rnFrom');
        var to = app.querySelector('#rnTo');
        var airport = app.querySelector('#rnAirport');
        var airportField = airport.closest('.rn-field');
        var filterBox = app.querySelector('.rn-filters');
        filterBox.classList.add('rn-status-filters');
        from.max = iso;
        to.max = iso;
        from.value = iso;
        to.value = iso;
        airportField.querySelector('label').textContent = 'Sân bay';
        airport.innerHTML = '<option value="ALL">Tất cả sân bay</option><option>VVNB</option><option>VVTS</option><option>VVDN</option><option>VVCR</option><option>VVPQ</option><option>VVCI</option><option>VVDL</option><option>VVPC</option>';
        airportField.insertAdjacentHTML('afterend', '<div class="rn-field"><label>Hãng bay</label><select id="rnOper"><option value="ALL">Tất cả hãng bay</option></select></div>');
        var oper = app.querySelector('#rnOper');
        filterBox.insertAdjacentHTML('beforeend', '<button class="rn-filter-button rn-export-button" type="button" id="rnStatusExport"><i class="fa fa-file-excel-o"></i> Export Excel</button>');
        if (window.ReportControls) window.ReportControls.enhanceAll(filterBox);

        function isCurrentDay() { return from.value === iso && to.value === iso; }
        function renderOperators(values, selected) {
            var unique = {};
            defaultOperators.concat(values || []).forEach(function (value) {
                var code = String(value || '').trim().toUpperCase();
                if (code && code !== 'ALL') unique[code] = true;
            });
            oper.innerHTML = '<option value="ALL">Tất cả hãng bay</option>' + Object.keys(unique).sort().map(function (value) {
                return '<option value="' + esc(value) + '">' + esc(value) + '</option>';
            }).join('');
            if (oper.querySelector('option[value="' + selected + '"]')) oper.value = selected;
        }
        function loadOperators() {
            var selected = oper.value;
            post('GetOperators', { fromDate: from.value, toDate: to.value, currentDay: isCurrentDay() }).then(function (values) {
                renderOperators(values, selected);
            }).catch(function () {
                renderOperators([], selected);
            });
        }
        renderOperators([], 'ALL');
        function load() {
            var button = app.querySelector('#rnApply');
            button.disabled = true;
            button.textContent = 'Đang tải...';
            post('GetData', { fromDate: from.value, toDate: to.value, oper: oper.value, airport: airport.value, currentDay: isCurrentDay() })
                .then(function (result) { render(app, result); loadOperators(); })
                .catch(function (error) { alert(error.message); })
                .then(function () {
                    button.disabled = false;
                    button.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
                });
        }

        app.querySelector('#rnApply').onclick = load;
        app.querySelector('#rnStatusExport').onclick = function () {
            if (!currentStatusFlights.length) { alert('Không có dữ liệu để xuất Excel.'); return; }
            window.ReportControls.exportExcel({
                fileName: 'FlightStatusRate_' + from.value + '_' + to.value,
                rows: currentStatusFlights,
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
        from.onchange = loadOperators;
        to.onchange = loadOperators;
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
