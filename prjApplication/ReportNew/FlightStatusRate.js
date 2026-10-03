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

    // Biểu đồ cột 4 trạng thái, style inline toàn bộ để xuất được ra ảnh PNG.
    function statusBarSvg(items, total) {
        var width = 400, height = 250, padL = 48, padR = 14, padT = 30, padB = 36;
        var plotW = width - padL - padR, plotH = height - padT - padB;
        var max = Math.max(1, Math.max.apply(null, items.map(function (item) { return item.value; })));
        max = Math.max(5, Math.ceil(max * 1.12 / 5) * 5);
        var parts = [];
        for (var g = 0; g <= 4; g++) {
            var y = padT + plotH * g / 4;
            parts.push('<line x1="' + padL + '" y1="' + y + '" x2="' + (width - padR) + '" y2="' + y + '" stroke="#e4edf4" stroke-width="1"></line>');
            parts.push('<text x="' + (padL - 8) + '" y="' + (y + 4) + '" fill="#7b8fa1" font-size="10" text-anchor="end">' + Math.round(max * (4 - g) / 4).toLocaleString('vi-VN') + '</text>');
        }
        var slot = plotW / items.length;
        items.forEach(function (item, index) {
            var barW = slot * 0.52;
            var x = padL + slot * index + (slot - barW) / 2;
            var h = item.value / max * plotH;
            var y = padT + plotH - h;
            var percent = total ? (item.value * 100 / total).toFixed(1) : '0';
            parts.push('<rect class="rn-status-colbar" data-status="' + item.key + '" x="' + x.toFixed(1) + '" y="' + y.toFixed(1) + '" width="' + barW.toFixed(1) + '" height="' + Math.max(0, h).toFixed(1) + '" rx="6" fill="' + item.color + '" style="cursor:pointer"><title>' + esc(item.name) + ': ' + item.value.toLocaleString('vi-VN') + ' (' + percent + '%)</title></rect>');
            parts.push('<text x="' + (x + barW / 2).toFixed(1) + '" y="' + (y - 6).toFixed(1) + '" fill="#31546f" font-size="11" font-weight="700" text-anchor="middle">' + item.value.toLocaleString('vi-VN') + '</text>');
            parts.push('<text x="' + (x + barW / 2).toFixed(1) + '" y="' + (padT + plotH + 16) + '" fill="#60778b" font-size="10" text-anchor="middle">' + esc(item.name) + '</text>');
        });
        return '<svg viewBox="0 0 ' + width + ' ' + height + '" width="100%" role="img" aria-label="Biểu đồ cột trạng thái chuyến bay" style="max-width:440px">' + parts.join('') + '</svg>';
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
            '<div class="rn-donut-layout rn-status-chart-row"><div class="rn-donut"><svg viewBox="0 0 240 240"><circle class="rn-donut-track" cx="120" cy="120" r="82"></circle>' + segments + '</svg>' +
            '<div class="rn-donut-center"><strong>' + total.toLocaleString('vi-VN') + '</strong><span>Tổng chuyến</span></div></div>' +
            '<div class="rn-status-column-chart" id="rnStatusBars">' + statusBarSvg(items, total) + '</div>' +
            '<div class="rn-status-list">' + cards + '</div></div></article>' +
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
            Array.prototype.forEach.call(app.querySelectorAll('.rn-status-colbar'), function (bar) {
                bar.setAttribute('opacity', !selectedStatus || selectedStatus === bar.getAttribute('data-status') ? '1' : '0.25');
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
        Array.prototype.forEach.call(app.querySelectorAll('.rn-status,.rn-donut-segment,.rn-status-colbar'), function (node) {
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
        filterBox.insertAdjacentHTML('beforeend', '<button class="rn-filter-button rn-kpi-button" type="button" id="rnKpisBtn" disabled><i class="fa fa-dashboard"></i> KPIs</button>' +
            '<button class="rn-filter-button rn-export-button" type="button" id="rnStatusExport"><i class="fa fa-file-excel-o"></i> Export Excel</button>' +
            '<button class="rn-filter-button rn-image-button" type="button" id="rnStatusExportImage"><i class="fa fa-picture-o"></i> Export ảnh</button>');
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
                .then(function (result) {
                    render(app, result); loadOperators();
                    var kpiBtn = app.querySelector('#rnKpisBtn');
                    if (kpiBtn) kpiBtn.disabled = !currentStatusFlights.length;
                })
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
        app.querySelector('#rnStatusExportImage').onclick = function () {
            var donutSvg = app.querySelector('.rn-donut svg');
            var barSvg = app.querySelector('#rnStatusBars svg');
            if (!donutSvg && !barSvg) { alert('Chưa có biểu đồ để xuất ảnh.'); return; }
            window.ReportControls.exportSvgsAsImage({
                fileName: 'FlightStatusRate_' + from.value + '_' + to.value,
                title: 'Thống kê trạng thái chuyến bay ' + from.value + ' đến ' + to.value,
                svgs: [donutSvg, barSvg]
            });
        };
        app.querySelector('#rnKpisBtn').onclick = function () {
            if (!currentStatusFlights.length) { alert('Chưa có dữ liệu. Hãy nhấn Áp dụng trước.'); return; }
            openKpiModal(currentStatusFlights);
        };
        from.onchange = loadOperators;
        to.onchange = loadOperators;
        load();
    }

    // ====================== KPIs ENGINE ======================

    var kpiDefs = [
        { id: 'completion', label: 'Tỷ lệ hoàn thành', calc: function (g) { return g.total ? g.finished / g.total * 100 : 0; } },
        { id: 'cancel', label: 'Tỷ lệ hủy', calc: function (g) { return g.total ? g.cancel / g.total * 100 : 0; } },
        { id: 'delay', label: 'Tỷ lệ delay', calc: function (g) { return g.total ? g.delay / g.total * 100 : 0; } },
        { id: 'ontime', label: 'Tỷ lệ đúng giờ', calc: function (g) { var base = g.finished + g.delay; return base ? g.finished / base * 100 : 0; } }
    ];
    var dimDefs = [
        { id: 'oper', label: 'Hãng bay', field: 'oper' },
        { id: 'airport', label: 'Sân bay', field: 'fromAirp' },
        { id: 'date', label: 'Ngày', field: 'atdDay' },
        { id: 'permType', label: 'PermType', field: 'permType' }
    ];
    var statuses = ['finished', 'cancel', 'delay', 'wait'];
    var statusNames = { finished: 'Hoàn thành', cancel: 'Hủy', delay: 'Delay', wait: 'Chờ' };
    var heatColors = ['#f0faf5', '#b8f0da', '#ffe8a8', '#ffc266', '#ff8a65', '#ef5b5b', '#c0392b'];

    function aggregate(flights, dimField) {
        var map = {};
        flights.forEach(function (f) {
            var key = String(f[dimField] || '').trim() || '(trống)';
            if (!map[key]) map[key] = { dim: key, total: 0, finished: 0, cancel: 0, delay: 0, wait: 0 };
            var g = map[key];
            g.total++;
            var sk = statusKey(f.status);
            if (sk === 'finished') g.finished++;
            else if (sk === 'cancel') g.cancel++;
            else if (sk === 'delay') g.delay++;
            else g.wait++;
        });
        var result = [];
        Object.keys(map).forEach(function (k) { result.push(map[k]); });
        return result;
    }

    function calcBenchmark(groups, kpiCalc) {
        if (!groups.length) return 0;
        var sum = 0;
        groups.forEach(function (g) { sum += kpiCalc(g); });
        return sum / groups.length;
    }

    function interpolateColor(ratio) {
        var idx = Math.min(Math.floor(ratio * (heatColors.length - 1)), heatColors.length - 2);
        var t = ratio * (heatColors.length - 1) - idx;
        function hex2rgb(hex) { var n = parseInt(hex.slice(1), 16); return [(n >> 16) & 255, (n >> 8) & 255, n & 255]; }
        var a = hex2rgb(heatColors[idx]), b = hex2rgb(heatColors[Math.min(idx + 1, heatColors.length - 1)]);
        var r = Math.round(a[0] + (b[0] - a[0]) * t), g = Math.round(a[1] + (b[1] - a[1]) * t), bl = Math.round(a[2] + (b[2] - a[2]) * t);
        return 'rgb(' + r + ',' + g + ',' + bl + ')';
    }

    function buildModalHtml() {
        var kpiOpts = kpiDefs.map(function (k) { return '<option value="' + k.id + '">' + esc(k.label) + '</option>'; }).join('');
        var dimBtns = dimDefs.map(function (d, i) { return '<button type="button" data-dim="' + d.id + '"' + (i === 0 ? ' class="is-active"' : '') + '>' + esc(d.label) + '</button>'; }).join('');
        return '<div id="kpiModal" class="kpi-modal" role="dialog" aria-modal="true">' +
            '<div class="kpi-modal-panel">' +
            '<div class="kpi-modal-header"><div><h2><i class="fa fa-dashboard"></i> KPIs Dashboard</h2><small>Phân tích chỉ số hiệu suất khai thác bay</small></div><button type="button" id="kpiClose" class="kpi-modal-close" aria-label="Đóng">×</button></div>' +
            '<div class="kpi-controls"><label>Bộ KPIs <select id="kpiSelect">' + kpiOpts + '</select></label><label>Dimension</label><div class="kpi-dim-group">' + dimBtns + '</div></div>' +
            '<div class="kpi-modal-body"><div class="kpi-grid">' +
            '<div class="kpi-panel"><div class="kpi-panel-heading"><h3>① Ranking</h3><small id="kpiRankInfo">Xếp hạng theo chỉ số đã chọn</small></div><div class="kpi-panel-body"><div class="kpi-ranking-scroll" id="kpiRankBody"></div></div></div>' +
            '<div class="kpi-panel"><div class="kpi-panel-heading"><h3>② Heatmap</h3><small>Dimension × Trạng thái — đậm = nhiều chuyến</small></div><div class="kpi-panel-body"><div class="kpi-heatmap-scroll" id="kpiHeatBody"></div></div></div>' +
            '<div class="kpi-panel kpi-panel-full"><div class="kpi-panel-heading"><h3>③ Trend / So sánh</h3><small id="kpiTrendInfo">Biểu đồ so sánh giữa các dimension</small></div><div class="kpi-panel-body"><div class="kpi-trend-container" id="kpiTrendBody"></div></div></div>' +
            '<div class="kpi-panel kpi-panel-full"><div class="kpi-panel-heading"><h3>④ Chi tiết chuyến bay</h3><small>Nhấn vào ranking hoặc heatmap để xem chi tiết</small></div><div class="kpi-panel-body">' +
            '<div class="kpi-drilldown-info" id="kpiDrillInfo" hidden><span id="kpiDrillLabel"></span><button type="button" class="kpi-drilldown-clear" id="kpiDrillClear">Bỏ lọc</button></div>' +
            '<div class="kpi-drilldown-scroll" id="kpiDrillBody"></div>' +
            '<div class="kpi-drilldown-pager" id="kpiDrillPager" hidden><button type="button" id="kpiDrillPrev">‹ Trước</button><span id="kpiDrillPageInfo"></span><button type="button" id="kpiDrillNext">Sau ›</button></div>' +
            '</div></div></div></div></div></div>';
    }

    function renderRanking(container, groups, kpiCalc, benchmark) {
        if (!groups.length) { container.innerHTML = '<div class="kpi-drilldown-empty">Không có dữ liệu.</div>'; return; }
        var sorted = groups.slice().sort(function (a, b) { return kpiCalc(b) - kpiCalc(a); });
        var maxVal = Math.max.apply(null, sorted.map(function (g) { return kpiCalc(g); }).concat([benchmark]));
        if (maxVal <= 0) maxVal = 100;
        var benchPct = benchmark / maxVal * 100;
        var html = '<table class="kpi-ranking-table"><thead><tr><th class="kpi-ranking-rank">#</th><th>Dimension</th><th>Tổng</th><th>KPI</th><th>Benchmark (' + benchmark.toFixed(1) + '%)</th></tr></thead><tbody>';
        sorted.forEach(function (g, i) {
            var val = kpiCalc(g);
            var above = val >= benchmark;
            var fillPct = Math.min(val / maxVal * 100, 100);
            var cls = '';
            if (i < 5 && sorted.length > 10) cls = ' kpi-ranking-top5';
            else if (i >= sorted.length - 5 && sorted.length > 10) cls = ' kpi-ranking-bottom5';
            html += '<tr class="' + cls + '" data-dim-value="' + esc(g.dim) + '">' +
                '<td class="kpi-ranking-rank">' + (i + 1) + '</td>' +
                '<td class="kpi-ranking-dim">' + esc(g.dim) + '</td>' +
                '<td>' + g.total.toLocaleString('vi-VN') + '</td>' +
                '<td class="kpi-ranking-value ' + (above ? 'above' : 'below') + '">' + val.toFixed(1) + '%</td>' +
                '<td><div class="kpi-benchmark-bar">' +
                '<div class="kpi-benchmark-fill ' + (above ? 'above' : 'below') + '" style="width:' + fillPct.toFixed(1) + '%"></div>' +
                '<div class="kpi-benchmark-line" style="left:' + benchPct.toFixed(1) + '%"><span class="kpi-benchmark-label">' + benchmark.toFixed(1) + '%</span></div>' +
                '</div></td></tr>';
        });
        html += '</tbody></table>';
        container.innerHTML = html;
    }

    function renderHeatmap(container, groups) {
        if (!groups.length) { container.innerHTML = '<div class="kpi-drilldown-empty">Không có dữ liệu.</div>'; return; }
        var maxCell = 1;
        groups.forEach(function (g) {
            statuses.forEach(function (s) { if (g[s] > maxCell) maxCell = g[s]; });
        });
        var sorted = groups.slice().sort(function (a, b) { return b.total - a.total; });
        var html = '<table class="kpi-heatmap-table"><thead><tr><th>Dimension</th>';
        statuses.forEach(function (s) { html += '<th>' + esc(statusNames[s]) + '</th>'; });
        html += '<th>Tổng</th></tr></thead><tbody>';
        sorted.forEach(function (g) {
            html += '<tr><td>' + esc(g.dim) + '</td>';
            statuses.forEach(function (s) {
                var val = g[s];
                var ratio = maxCell > 0 ? val / maxCell : 0;
                var bg = interpolateColor(ratio);
                var textColor = ratio > 0.6 ? '#fff' : '#173b59';
                html += '<td style="background:' + bg + ';color:' + textColor + '" data-dim-value="' + esc(g.dim) + '" data-status="' + s + '">' + val + '</td>';
            });
            html += '<td style="font-weight:800">' + g.total.toLocaleString('vi-VN') + '</td></tr>';
        });
        html += '</tbody></table>';
        container.innerHTML = html;
    }

    function renderTrendChart(container, groups, kpiCalc, dimId, kpiLabel) {
        if (dimId === 'date') {
            var sorted = groups.slice().sort(function (a, b) { return a.dim < b.dim ? -1 : a.dim > b.dim ? 1 : 0; });
            if (!sorted.length) { container.innerHTML = '<div class="kpi-trend-empty">Không có dữ liệu theo ngày.</div>'; return; }
            var width = 700, height = 280, padL = 52, padR = 20, padT = 28, padB = 44;
            var plotW = width - padL - padR, plotH = height - padT - padB;
            var values = sorted.map(kpiCalc);
            var max = Math.max(100, Math.max.apply(null, values) * 1.1);
            var parts = ['<svg viewBox="0 0 ' + width + ' ' + height + '" role="img" aria-label="Xu hướng ' + esc(kpiLabel) + ' theo ngày" style="max-width:100%;height:auto">'];
            for (var g = 0; g <= 4; g++) {
                var y = padT + plotH * g / 4;
                parts.push('<line x1="' + padL + '" y1="' + y + '" x2="' + (width - padR) + '" y2="' + y + '" stroke="#e4edf4" stroke-width="1"/>');
                parts.push('<text x="' + (padL - 8) + '" y="' + (y + 4) + '" fill="#7b8fa1" font-size="10" text-anchor="end">' + Math.round(max * (4 - g) / 4) + '%</text>');
            }
            var path = sorted.map(function (p, i) {
                var x = padL + (sorted.length === 1 ? plotW / 2 : plotW * i / (sorted.length - 1));
                var yVal = padT + plotH - values[i] / max * plotH;
                return (i ? 'L' : 'M') + x.toFixed(1) + ',' + yVal.toFixed(1);
            }).join(' ');
            parts.push('<path d="' + path + '" fill="none" stroke="#2387c8" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"/>');
            sorted.forEach(function (p, i) {
                var x = padL + (sorted.length === 1 ? plotW / 2 : plotW * i / (sorted.length - 1));
                var yVal = padT + plotH - values[i] / max * plotH;
                parts.push('<circle cx="' + x.toFixed(1) + '" cy="' + yVal.toFixed(1) + '" r="4" fill="#2387c8" stroke="#fff" stroke-width="2"><title>' + esc(p.dim) + ': ' + values[i].toFixed(1) + '%</title></circle>');
            });
            var labelStep = Math.max(1, Math.ceil(sorted.length / 14));
            sorted.forEach(function (p, i) {
                if (i % labelStep !== 0 && i !== sorted.length - 1) return;
                var x = padL + (sorted.length === 1 ? plotW / 2 : plotW * i / (sorted.length - 1));
                parts.push('<text x="' + x.toFixed(1) + '" y="' + (height - 14) + '" fill="#60778b" font-size="10" text-anchor="middle">' + esc(p.dim) + '</text>');
            });
            parts.push('</svg>');
            container.innerHTML = parts.join('');
        } else {
            // Bar chart comparing dimensions
            var sorted2 = groups.slice().sort(function (a, b) { return kpiCalc(b) - kpiCalc(a); }).slice(0, 20);
            if (!sorted2.length) { container.innerHTML = '<div class="kpi-trend-empty">Không có dữ liệu.</div>'; return; }
            var w2 = 700, h2 = 280, pL = 100, pR = 20, pT = 20, pB = 16;
            var pW = w2 - pL - pR, pH = h2 - pT - pB;
            var barH = Math.min(22, pH / sorted2.length - 3);
            var maxV = Math.max.apply(null, sorted2.map(kpiCalc));
            if (maxV <= 0) maxV = 100;
            var svg = ['<svg viewBox="0 0 ' + w2 + ' ' + (pT + (barH + 4) * sorted2.length + pB) + '" role="img" style="max-width:100%;height:auto">'];
            sorted2.forEach(function (g, i) {
                var y2 = pT + i * (barH + 4);
                var val = kpiCalc(g);
                var bw = Math.max(2, val / maxV * pW);
                var col = val >= calcBenchmark(groups, kpiCalc) ? '#16aa78' : '#e05252';
                svg.push('<text x="' + (pL - 6) + '" y="' + (y2 + barH / 2 + 4) + '" fill="#405a70" font-size="11" text-anchor="end" font-weight="600">' + esc(g.dim) + '</text>');
                svg.push('<rect x="' + pL + '" y="' + y2 + '" width="' + bw.toFixed(1) + '" height="' + barH + '" rx="4" fill="' + col + '"><title>' + esc(g.dim) + ': ' + val.toFixed(1) + '%</title></rect>');
                svg.push('<text x="' + (pL + bw + 6) + '" y="' + (y2 + barH / 2 + 4) + '" fill="#405a70" font-size="10" font-weight="700">' + val.toFixed(1) + '%</text>');
            });
            svg.push('</svg>');
            container.innerHTML = svg.join('');
        }
    }

    function renderDrillDown(container, pagerEl, flights, filter, label) {
        var filtered = flights;
        if (filter) {
            filtered = flights.filter(function (f) {
                var dimMatch = !filter.dim || String(f[filter.dimField] || '').trim() === filter.dim || (filter.dim === '(trống)' && !String(f[filter.dimField] || '').trim());
                var statusMatch = !filter.status || statusKey(f.status) === filter.status;
                return dimMatch && statusMatch;
            });
        }
        var drillPage = 1, drillSize = 100;
        function renderPage() {
            var pages = Math.max(1, Math.ceil(filtered.length / drillSize));
            if (drillPage > pages) drillPage = pages;
            var start = (drillPage - 1) * drillSize;
            var current = filtered.slice(start, start + drillSize);
            var html = '<table class="kpi-drilldown-table"><thead><tr><th>STT</th><th>CALLSIGN</th><th>OPER</th><th>REGISTRATION</th><th>PERMTYPE</th><th>FROM</th><th>TO</th><th>ATDDAY</th><th>ATADAY</th><th>EOBTDAY</th><th>TRẠNG THÁI</th></tr></thead><tbody>';
            if (!current.length) {
                html += '<tr><td colspan="11" class="kpi-drilldown-empty">Không có chuyến bay phù hợp.</td></tr>';
            } else {
                current.forEach(function (f, i) {
                    html += '<tr><td>' + (start + i + 1) + '</td><td><b>' + esc(f.callsign) + '</b></td><td>' + esc(f.oper) + '</td><td>' + esc(f.registration) + '</td><td>' + esc(f.permType) + '</td><td>' + esc(f.fromAirp) + '</td><td>' + esc(f.toAirp) + '</td><td>' + esc(f.atdDay) + '</td><td>' + esc(f.ataDay) + '</td><td>' + esc(f.eobtDay) + '</td><td><span class="rn-pill" style="background:' + statusColor(f.status) + ';color:#fff">' + esc(statusLabel(f.status)) + '</span></td></tr>';
                });
            }
            html += '</tbody></table>';
            container.innerHTML = html;
            if (filtered.length > drillSize) {
                pagerEl.hidden = false;
                pagerEl.querySelector('#kpiDrillPageInfo').textContent = 'Trang ' + drillPage + '/' + pages + ' · ' + filtered.length.toLocaleString('vi-VN') + ' dòng';
                pagerEl.querySelector('#kpiDrillPrev').disabled = drillPage <= 1;
                pagerEl.querySelector('#kpiDrillNext').disabled = drillPage >= pages;
                pagerEl.querySelector('#kpiDrillPrev').onclick = function () { if (drillPage > 1) { drillPage--; renderPage(); } };
                pagerEl.querySelector('#kpiDrillNext').onclick = function () { if (drillPage < pages) { drillPage++; renderPage(); } };
            } else {
                pagerEl.hidden = true;
            }
        }
        renderPage();
    }

    function openKpiModal(flights) {
        var existing = document.getElementById('kpiModal');
        if (existing) existing.parentNode.removeChild(existing);
        document.body.insertAdjacentHTML('beforeend', buildModalHtml());
        var modal = document.getElementById('kpiModal');
        var currentKpi = 'completion';
        var currentDim = 'oper';

        function getCurrentKpiDef() { return kpiDefs.filter(function (k) { return k.id === currentKpi; })[0] || kpiDefs[0]; }
        function getCurrentDimDef() { return dimDefs.filter(function (d) { return d.id === currentDim; })[0] || dimDefs[0]; }

        function refresh() {
            var kDef = getCurrentKpiDef();
            var dDef = getCurrentDimDef();
            var groups = aggregate(flights, dDef.field);
            var benchmark = calcBenchmark(groups, kDef.calc);
            modal.querySelector('#kpiRankInfo').textContent = 'Xếp hạng ' + kDef.label + ' theo ' + dDef.label + ' — Benchmark: ' + benchmark.toFixed(1) + '%';
            renderRanking(modal.querySelector('#kpiRankBody'), groups, kDef.calc, benchmark);
            renderHeatmap(modal.querySelector('#kpiHeatBody'), groups);
            modal.querySelector('#kpiTrendInfo').textContent = currentDim === 'date' ? 'Xu hướng ' + kDef.label + ' theo ngày' : 'So sánh ' + kDef.label + ' giữa các ' + dDef.label;
            renderTrendChart(modal.querySelector('#kpiTrendBody'), groups, kDef.calc, currentDim, kDef.label);
            clearDrillDown();
            bindInteractions(groups, dDef);
        }

        function clearDrillDown() {
            modal.querySelector('#kpiDrillInfo').hidden = true;
            modal.querySelector('#kpiDrillPager').hidden = true;
            modal.querySelector('#kpiDrillBody').innerHTML = '<div class="kpi-drilldown-empty">Nhấn vào một dòng trong Ranking hoặc một ô trong Heatmap để xem chi tiết chuyến bay.</div>';
            // clear active states
            var activeRows = modal.querySelectorAll('.is-drilldown-active');
            for (var i = 0; i < activeRows.length; i++) activeRows[i].classList.remove('is-drilldown-active');
        }

        function doDrillDown(dimValue, statusFilter, label) {
            var dDef = getCurrentDimDef();
            modal.querySelector('#kpiDrillInfo').hidden = false;
            modal.querySelector('#kpiDrillLabel').textContent = label;
            renderDrillDown(
                modal.querySelector('#kpiDrillBody'),
                modal.querySelector('#kpiDrillPager'),
                flights,
                { dim: dimValue, dimField: dDef.field, status: statusFilter || '' },
                label
            );
            // Scroll to drilldown
            var drillPanel = modal.querySelector('#kpiDrillBody');
            if (drillPanel) drillPanel.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }

        function bindInteractions(groups, dDef) {
            // Ranking row click
            var rankRows = modal.querySelectorAll('.kpi-ranking-table tbody tr');
            for (var r = 0; r < rankRows.length; r++) {
                rankRows[r].addEventListener('click', (function (row) {
                    return function () {
                        var dimVal = row.getAttribute('data-dim-value');
                        var activeRows = modal.querySelectorAll('.is-drilldown-active');
                        for (var j = 0; j < activeRows.length; j++) activeRows[j].classList.remove('is-drilldown-active');
                        row.classList.add('is-drilldown-active');
                        doDrillDown(dimVal, '', dDef.label + ': ' + dimVal);
                    };
                })(rankRows[r]));
            }
            // Heatmap cell click
            var heatCells = modal.querySelectorAll('.kpi-heatmap-table td[data-dim-value]');
            for (var h = 0; h < heatCells.length; h++) {
                heatCells[h].addEventListener('click', (function (cell) {
                    return function () {
                        var dimVal = cell.getAttribute('data-dim-value');
                        var st = cell.getAttribute('data-status');
                        var activeRows = modal.querySelectorAll('.is-drilldown-active');
                        for (var j = 0; j < activeRows.length; j++) activeRows[j].classList.remove('is-drilldown-active');
                        cell.classList.add('is-drilldown-active');
                        doDrillDown(dimVal, st, dDef.label + ': ' + dimVal + ' — ' + (statusNames[st] || st));
                    };
                })(heatCells[h]));
            }
        }

        // Controls
        modal.querySelector('#kpiSelect').onchange = function () { currentKpi = this.value; refresh(); };
        var dimBtns = modal.querySelectorAll('.kpi-dim-group button');
        for (var d = 0; d < dimBtns.length; d++) {
            dimBtns[d].addEventListener('click', (function (btn) {
                return function () {
                    currentDim = btn.getAttribute('data-dim');
                    for (var j = 0; j < dimBtns.length; j++) dimBtns[j].classList.toggle('is-active', dimBtns[j] === btn);
                    refresh();
                };
            })(dimBtns[d]));
        }
        modal.querySelector('#kpiDrillClear').onclick = clearDrillDown;

        // Close
        function closeModal() { modal.hidden = true; modal.parentNode.removeChild(modal); }
        modal.querySelector('#kpiClose').onclick = closeModal;
        modal.addEventListener('click', function (e) { if (e.target === modal) closeModal(); });
        document.addEventListener('keydown', function handler(e) {
            if (e.key === 'Escape' && document.getElementById('kpiModal')) { closeModal(); document.removeEventListener('keydown', handler); }
        });

        refresh();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
