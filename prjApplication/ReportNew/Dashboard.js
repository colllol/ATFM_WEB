(function () {
    function camelize(value) { if (Array.isArray(value)) return value.map(camelize); if (!value || typeof value !== 'object') return value; var result = {}; Object.keys(value).forEach(function (key) { result[key.charAt(0).toLowerCase() + key.slice(1)] = camelize(value[key]); }); return result; }
    var airportNames = {
        VVNB: 'Nội Bài', VVTS: 'Tân Sơn Nhất', VVDN: 'Đà Nẵng', VVCR: 'Cam Ranh',
        VVPQ: 'Phú Quốc', VVCI: 'Cát Bi', VVDL: 'Liên Khương', VVPC: 'Phù Cát',
        VVVH: 'Vinh', VVCT: 'Cần Thơ', VVCA: 'Chu Lai', VVDB: 'Điện Biên',
        VVBM: 'Buôn Ma Thuột', VVTH: 'Tuy Hòa', VVPK: 'Pleiku', VVTX: 'Thọ Xuân',
        VVDH: 'Đồng Hới', VVRG: 'Rạch Giá', VVCM: 'Cà Mau', VVCS: 'Côn Đảo', VVVD: 'Vân Đồn'
    };

    function post(url, data) {
        var deferred = window.jQuery.Deferred();
        window.jQuery.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        }).done(function (result) {
            var payload = result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result;
            if (payload && payload.Code && payload.Code !== '00') {
                deferred.reject(new Error(payload.Message || 'Không thể tải dữ liệu dashboard.'));
                return;
            }
            deferred.resolve(camelize(payload && Object.prototype.hasOwnProperty.call(payload, 'ListValue') ? payload.ListValue : payload));
        }).fail(function (xhr) {
            var message = 'Không thể tải dữ liệu dashboard.';
            var result = xhr && xhr.responseJSON;
            if (!result && xhr && xhr.responseText) {
                try { result = JSON.parse(xhr.responseText); } catch (ignore) { result = null; }
            }
            if (result) {
                var payload = Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result;
                message = payload.Message || payload.message || message;
            }
            deferred.reject(new Error(message));
        });
        return deferred.promise();
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : value;
        return node.innerHTML;
    }

    function number(value) { return Number(value || 0).toLocaleString('vi-VN'); }
    function pad2(value) { return value < 10 ? '0' + value : String(value); }
    function formatDate(value) {
        return value.getFullYear() + '-' + pad2(value.getMonth() + 1) + '-' + pad2(value.getDate());
    }
    function statusKey(value) {
        value = String(value || '');
        return value.indexOf('DELAY') === 0 ? 'delay' : value.toLowerCase();
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="dashboard"]');
        if (!app) return;
        var airport = app.querySelector('#rnAirport');
        var from = app.querySelector('#rnFrom');
        var to = app.querySelector('#rnTo');
        var apply = app.querySelector('#rnApply');
        var today = new Date();
        var resizeHandler = null;
        var selectedAirport = '';
        var selectedStatus = '';
        var dashboardData = null;
        // Kiểu hiển thị người dùng chọn cho từng biểu đồ (chỉ liệt kê kiểu phù hợp dữ liệu).
        var chartTypes = { status: 'donut', airports: 'bar', trend: 'line' };
        var airportPalette = ['#2387c8', '#20b486', '#8a6ee8', '#f5a623', '#ef5b5b', '#17a2b8', '#c65fa8', '#7a9e37', '#5a6ee0', '#d98136', '#3aa6a0', '#9b6bd6'];

        function showState(title, message, iconClass) {
            var kpis = app.querySelector('.rn-kpis');
            var grid = app.querySelector('.rn-grid');
            if (kpis) {
                kpis.innerHTML = '<div class="rn-kpi"><span>Dữ liệu</span><strong>--</strong><small>Đang chờ đồng bộ</small></div>' +
                    '<div class="rn-kpi"><span>Hoàn thành</span><strong>--</strong><small>Đang chờ đồng bộ</small></div>' +
                    '<div class="rn-kpi"><span>Delay</span><strong>--</strong><small>Đang chờ đồng bộ</small></div>' +
                    '<div class="rn-kpi"><span>Cần chú ý</span><strong>--</strong><small>Đang chờ đồng bộ</small></div>';
            }
            if (grid) {
                grid.innerHTML = '<article class="rn-card wide rn-empty"><div class="rn-empty-icon"><i class="fa ' + esc(iconClass || 'fa-refresh') + '"></i></div><h2>' + esc(title) + '</h2><p>' + esc(message || '') + '</p></article>';
            }
        }

        if (!airport || !from || !to || !apply) {
            showState('Không thể khởi tạo dashboard', 'Không tìm thấy đầy đủ bộ lọc dữ liệu.', 'fa-exclamation-triangle');
            return;
        }

        airport.innerHTML = '<option value="ALL">Tất cả sân bay</option>' + Object.keys(airportNames).map(function (code) {
            return '<option value="' + code + '">' + code + ' - ' + airportNames[code] + '</option>';
        }).join('');
        if (window.jQuery && window.jQuery.fn && window.jQuery.fn.select2) {
            try {
                window.jQuery(airport).select2({
                    width: '100%',
                    minimumResultsForSearch: 0,
                    dropdownCssClass: 'rn-dashboard-airport-dropdown',
                    language: {
                        noResults: function () { return 'Không tìm thấy sân bay'; },
                        searching: function () { return 'Đang tìm kiếm...'; }
                    }
                });
            } catch (ignore) {
                airport.style.display = '';
            }
        }
        from.max = formatDate(today);
        to.max = formatDate(today);
        from.value = formatDate(today);
        to.value = formatDate(today);
        var heroSubtitle = app.querySelector('.rn-hero p');
        var liveNote = app.querySelector('.rn-live small');
        if (heroSubtitle) heroSubtitle.textContent = 'Tổng hợp trạng thái, khai thác sân bay và xu hướng từ dữ liệu thực tế';
        if (liveNote) liveNote.textContent = '';

        function syncAirports(items) {
            var selected = airport.value;
            (items || []).forEach(function (item) {
                if (!airport.querySelector('option[value="' + item.code + '"]')) {
                    airport.insertAdjacentHTML('beforeend', '<option value="' + esc(item.code) + '">' + esc(item.code + ' - ' + (airportNames[item.code] || item.code)) + '</option>');
                }
            });
            airport.value = selected;
            if (window.jQuery && window.jQuery.fn && window.jQuery.fn.select2) {
                try { window.jQuery(airport).trigger('change.select2'); } catch (ignore) { airport.style.display = ''; }
            }
        }

        function statusItems(flights) {
            var counts = { finished: 0, cancel: 0, delay: 0, wait: 0 };
            (flights || []).forEach(function (flight) {
                var key = statusKey(flight.status);
                if (Object.prototype.hasOwnProperty.call(counts, key)) counts[key]++;
            });
            return [
                { key: 'finished', label: 'Hoàn thành', value: counts.finished, color: '#20b486' },
                { key: 'cancel', label: 'Hủy', value: counts.cancel, color: '#ef5b5b' },
                { key: 'delay', label: 'Delay', value: counts.delay, color: '#f5a623' },
                { key: 'wait', label: 'Chưa thực hiện', value: counts.wait, color: '#4d8df7' }
            ];
        }

        function donutFlights() {
            var flights = dashboardData.status.flights || [];
            if (!selectedAirport) return flights;
            return flights.filter(function (flight) { return flight.fromAirp === selectedAirport || flight.toAirp === selectedAirport; });
        }

        // Biểu đồ cột trạng thái (kiểu thay thế cho donut), style inline để không phụ thuộc CSS.
        function statusBarSvg(items, total) {
            var width = 400, height = 250, padL = 48, padR = 14, padT = 30, padB = 36;
            var plotW = width - padL - padR, plotH = height - padT - padB;
            var max = Math.max(1, Math.max.apply(null, items.map(function (item) { return item.value; })));
            max = Math.max(5, Math.ceil(max * 1.12 / 5) * 5);
            var parts = [];
            for (var g = 0; g <= 4; g++) {
                var y = padT + plotH * g / 4;
                parts.push('<line x1="' + padL + '" y1="' + y + '" x2="' + (width - padR) + '" y2="' + y + '" stroke="#e4edf4" stroke-width="1"></line>');
                parts.push('<text x="' + (padL - 8) + '" y="' + (y + 4) + '" fill="#7b8fa1" font-size="10" text-anchor="end">' + number(Math.round(max * (4 - g) / 4)) + '</text>');
            }
            var slot = plotW / items.length;
            items.forEach(function (item, index) {
                var barW = slot * 0.52;
                var x = padL + slot * index + (slot - barW) / 2;
                var h = item.value / max * plotH;
                var y = padT + plotH - h;
                var dim = selectedStatus && selectedStatus !== item.key;
                parts.push('<rect class="rn-status-colbar" data-status="' + item.key + '" x="' + x.toFixed(1) + '" y="' + y.toFixed(1) + '" width="' + barW.toFixed(1) + '" height="' + Math.max(0, h).toFixed(1) + '" rx="6" fill="' + item.color + '" opacity="' + (dim ? '0.25' : '1') + '" style="cursor:pointer"><title>' + esc(item.label) + ': ' + number(item.value) + '</title></rect>');
                parts.push('<text x="' + (x + barW / 2).toFixed(1) + '" y="' + (y - 6).toFixed(1) + '" fill="#31546f" font-size="11" font-weight="700" text-anchor="middle">' + number(item.value) + '</text>');
                parts.push('<text x="' + (x + barW / 2).toFixed(1) + '" y="' + (padT + plotH + 16) + '" fill="#60778b" font-size="10" text-anchor="middle">' + esc(item.label) + '</text>');
            });
            return '<svg viewBox="0 0 ' + width + ' ' + height + '" width="100%" role="img" style="max-width:430px">' + parts.join('') + '</svg>';
        }

        function renderDonut() {
            var items = statusItems(donutFlights());
            var total = items.reduce(function (sum, item) { return sum + item.value; }, 0);
            var circumference = 2 * Math.PI * 82;
            var offset = 0;
            var segments = items.map(function (item) {
                var length = total ? item.value / total * circumference : 0;
                var html = '<circle class="rn-donut-segment' + (selectedStatus === item.key ? ' is-active' : '') + '" data-status="' + item.key + '" stroke="' + item.color + '" cx="120" cy="120" r="82" stroke-dasharray="' + length + ' ' + (circumference - length) + '" stroke-dashoffset="' + (-offset) + '"></circle>';
                offset += length;
                return html;
            }).join('');
            var cards = items.map(function (item) {
                return '<div class="rn-status' + (selectedStatus === item.key ? ' is-active' : '') + '" data-status="' + item.key + '" style="--status-color:' + item.color + '"><span>' + item.label + '</span><b>' + number(item.value) + '</b><small>' + (total ? (item.value * 100 / total).toFixed(1) : 0) + '%</small></div>';
            }).join('');
            var activeItem = items.filter(function (item) { return item.key === selectedStatus; })[0];
            var centerValue = activeItem ? activeItem.value : total;
            var centerLabel = activeItem ? activeItem.label : selectedAirport || 'Tổng chuyến';
            var holder = app.querySelector('#rnDashboardDonut');
            if (chartTypes.status === 'bar') {
                holder.innerHTML = '<div class="rn-donut-layout"><div class="rn-status-column-chart">' + statusBarSvg(items, total) + '</div><div class="rn-status-list' + (selectedStatus ? ' has-active' : '') + '">' + cards + '</div></div>';
            } else {
                holder.innerHTML = '<div class="rn-donut-layout"><div class="rn-donut' + (selectedStatus ? ' has-active' : '') + '"><svg viewBox="0 0 240 240"><circle class="rn-donut-track" cx="120" cy="120" r="82"></circle>' + segments + '</svg><div class="rn-donut-center"><strong>' + number(centerValue) + '</strong><span>' + esc(centerLabel) + '</span></div></div><div class="rn-status-list' + (selectedStatus ? ' has-active' : '') + '">' + cards + '</div></div>';
            }
            Array.prototype.forEach.call(holder.querySelectorAll('.rn-status,.rn-donut-segment,.rn-status-colbar'), function (node) {
                node.onclick = function () {
                    var key = this.getAttribute('data-status');
                    selectedStatus = selectedStatus === key ? '' : key;
                    renderDonut();
                };
            });
            var filterAirport = airport.value && airport.value !== 'ALL' ? airport.value : '';
            var displayAirport = selectedAirport || filterAirport;
            app.querySelector('#rnDashboardStatusSubtitle').textContent = displayAirport
                ? displayAirport + ' - ' + (airportNames[displayAirport] || displayAirport)
                : 'Tất cả sân bay trong kỳ';
        }

        function renderBars() {
            var items = dashboardData.overview.airports || [];
            var maximum = 0;
            items.forEach(function (item) { maximum = Math.max(maximum, item.departures, item.arrivals); });
            var bars = items.map(function (item) {
                var departureHeight = item.departures ? Math.max(4, item.departures / Math.max(1, maximum) * 100) : 0;
                var arrivalHeight = item.arrivals ? Math.max(4, item.arrivals / Math.max(1, maximum) * 100) : 0;
                var active = selectedAirport === item.code ? ' is-active' : '';
                return '<div class="rn-airport-bar-group' + active + '" data-airport="' + item.code + '"><div class="rn-airport-bar-pair"><button type="button" class="rn-airport-bar departure" style="height:' + departureHeight + '%"><span>' + number(item.departures) + '</span></button><button type="button" class="rn-airport-bar arrival" style="height:' + arrivalHeight + '%"><span>' + number(item.arrivals) + '</span></button></div><b>' + esc(item.code) + '</b><small>' + esc(airportNames[item.code] || item.code) + '</small></div>';
            }).join('');
            if (!bars) bars = '<div class="rn-airport-empty">Không có chuyến Finished hoặc Delay trong khoảng ngày đã chọn.</div>';
            app.querySelector('#rnDashboardBars').innerHTML = bars;
            Array.prototype.forEach.call(app.querySelectorAll('.rn-airport-bar-group[data-airport]'), function (group) {
                group.onclick = function () {
                    var code = this.getAttribute('data-airport');
                    selectedAirport = selectedAirport === code ? '' : code;
                    selectedStatus = '';
                    renderBars();
                    renderDonut();
                    updateBarDetail();
                };
            });
        }

        function updateBarDetail() {
            var detail = app.querySelector('#rnDashboardBarDetail');
            if (!detail) return;
            detail.textContent = selectedAirport
                ? selectedAirport + ' - ' + (airportNames[selectedAirport] || selectedAirport) + ': nhấn lại để trở về phạm vi bộ lọc'
                : 'Nhấn vào một cột hoặc lát cắt để xem trạng thái của sân bay';
        }

        // Kiểu đường: hai đường cất cánh / hạ cánh theo mã sân bay.
        function airportLineSvg() {
            var items = dashboardData.overview.airports || [];
            if (!items.length) return '<div class="rn-airport-empty">Không có chuyến Finished hoặc Delay trong khoảng ngày đã chọn.</div>';
            var width = Math.max(560, items.length * 64 + 100), height = 300;
            var padL = 48, padR = 18, padT = 24, padB = 42;
            var plotW = width - padL - padR, plotH = height - padT - padB;
            var max = Math.max(1, Math.max.apply(null, items.map(function (item) { return Math.max(item.departures, item.arrivals); })));
            max = Math.max(5, Math.ceil(max * 1.12 / 5) * 5);
            var parts = [];
            for (var g = 0; g <= 4; g++) {
                var y = padT + plotH * g / 4;
                parts.push('<line x1="' + padL + '" y1="' + y + '" x2="' + (width - padR) + '" y2="' + y + '" stroke="#e4edf4" stroke-width="1"></line>');
                parts.push('<text x="' + (padL - 8) + '" y="' + (y + 4) + '" fill="#7b8fa1" font-size="10" text-anchor="end">' + number(Math.round(max * (4 - g) / 4)) + '</text>');
            }
            function xAt(index) { return items.length === 1 ? padL + plotW / 2 : padL + plotW * index / (items.length - 1); }
            function yAt(value) { return padT + plotH - value / max * plotH; }
            ['departures', 'arrivals'].forEach(function (field) {
                var color = field === 'departures' ? '#2387c8' : '#20b486';
                parts.push('<path d="' + items.map(function (item, index) { return (index ? 'L' : 'M') + xAt(index).toFixed(1) + ',' + yAt(item[field]).toFixed(1); }).join(' ') + '" fill="none" stroke="' + color + '" stroke-width="3" stroke-linecap="round"></path>');
                items.forEach(function (item, index) {
                    parts.push('<circle cx="' + xAt(index).toFixed(1) + '" cy="' + yAt(item[field]).toFixed(1) + '" r="4" fill="' + color + '"><title>' + esc(item.code) + ' - ' + (field === 'departures' ? 'Cất cánh' : 'Hạ cánh') + ': ' + number(item[field]) + '</title></circle>');
                });
            });
            items.forEach(function (item, index) {
                parts.push('<text x="' + xAt(index).toFixed(1) + '" y="' + (height - 14) + '" fill="#60778b" font-size="10" text-anchor="middle">' + esc(item.code) + '</text>');
            });
            return '<svg viewBox="0 0 ' + width + ' ' + height + '" width="100%" role="img">' + parts.join('') + '</svg>';
        }

        // Kiểu tròn: tỷ trọng tổng lượt (cất + hạ cánh) của từng sân bay, nhấn lát cắt để lọc.
        function airportDonutSvg() {
            var items = (dashboardData.overview.airports || []).filter(function (item) { return item.departures + item.arrivals > 0; });
            if (!items.length) return '<div class="rn-airport-empty">Không có chuyến Finished hoặc Delay trong khoảng ngày đã chọn.</div>';
            var total = items.reduce(function (sum, item) { return sum + item.departures + item.arrivals; }, 0);
            var circumference = 2 * Math.PI * 82;
            var offset = 0;
            var slices = items.map(function (item, index) {
                var value = item.departures + item.arrivals;
                var length = value / total * circumference;
                var dim = selectedAirport && selectedAirport !== item.code;
                var html = '<circle data-airport="' + esc(item.code) + '" fill="none" stroke="' + airportPalette[index % airportPalette.length] + '" stroke-width="' + (selectedAirport === item.code ? 36 : 28) + '" opacity="' + (dim ? '0.25' : '1') + '" cx="120" cy="120" r="82" stroke-dasharray="' + length + ' ' + (circumference - length) + '" stroke-dashoffset="' + (-offset) + '" style="cursor:pointer"><title>' + esc(item.code + ' - ' + (airportNames[item.code] || item.code)) + ': ' + number(value) + ' lượt (' + (value * 100 / total).toFixed(1) + '%)</title></circle>';
                offset += length;
                return html;
            }).join('');
            var legend = items.slice(0, 12).map(function (item, index) {
                return '<span style="display:inline-flex;align-items:center;gap:5px"><i class="rn-dot" style="background:' + airportPalette[index % airportPalette.length] + '"></i>' + esc(item.code) + '</span>';
            }).join('');
            return '<div style="width:100%"><svg viewBox="0 0 240 240" width="240" style="display:block;margin:auto;transform:rotate(-90deg);overflow:visible"><circle fill="none" stroke="#edf3f7" stroke-width="28" cx="120" cy="120" r="82"></circle>' + slices + '</svg>' +
                '<div style="display:flex;flex-wrap:wrap;gap:10px;justify-content:center;margin-top:12px;color:#60778b;font-size:11px">' + legend + (items.length > 12 ? '<span>…</span>' : '') + '</div></div>';
        }

        function renderAirportChart() {
            var wrap = app.querySelector('#rnDashboardBarsWrap');
            if (!wrap) return;
            if (chartTypes.airports === 'bar') {
                wrap.innerHTML = '<div class="rn-airport-chart" id="rnDashboardBars"></div>';
                renderBars();
                return;
            }
            wrap.innerHTML = '<div class="rn-airport-alt-chart">' + (chartTypes.airports === 'line' ? airportLineSvg() : airportDonutSvg()) + '</div>';
            Array.prototype.forEach.call(wrap.querySelectorAll('[data-airport]'), function (slice) {
                slice.onclick = function () {
                    var code = this.getAttribute('data-airport');
                    selectedAirport = selectedAirport === code ? '' : code;
                    selectedStatus = '';
                    renderAirportChart();
                    renderDonut();
                    updateBarDetail();
                };
            });
        }

        function renderKpis() {
            var status = dashboardData.status;
            var attention = Number(status.cancel || 0) + Number(status.wait || 0);
            var total = Number(status.total || 0);
            var html = '<div class="rn-kpis"><div class="rn-kpi" style="--accent:#2387c8"><span>Tổng chuyến</span><strong>' + number(total) + '</strong><small>Đủ 4 trạng thái</small></div>' +
                '<div class="rn-kpi" style="--accent:#20b486"><span>Hoàn thành</span><strong>' + number(status.finished) + '</strong><small>' + (total ? (status.finished * 100 / total).toFixed(1) : 0) + '%</small></div>' +
                '<div class="rn-kpi" style="--accent:#f5a623"><span>Delay</span><strong>' + number(status.delay) + '</strong><small>' + (total ? (status.delay * 100 / total).toFixed(1) : 0) + '%</small></div>' +
                '<div class="rn-kpi" style="--accent:#ef5b5b"><span>Cần chú ý</span><strong>' + number(attention) + '</strong><small>Hủy + Chưa thực hiện</small></div></div>';
            var old = app.querySelector('.rn-kpis');
            if (old) old.outerHTML = html;
        }

        function renderLayout() {
            var trend = dashboardData.trend;
            var html = '<div class="rn-grid"><article class="rn-card"><div class="rn-chart-head"><h2>Trạng thái chuyến bay</h2><select class="rn-chart-type-select" id="rnStatusChartType" title="Chọn kiểu biểu đồ"><option value="donut">Biểu đồ tròn</option><option value="bar">Biểu đồ cột</option></select></div><p class="rn-card-subtitle" id="rnDashboardStatusSubtitle"></p><div id="rnDashboardDonut"></div></article>' +
                '<article class="rn-card"><div class="rn-chart-head"><h2>Cất/hạ cánh sân bay</h2><select class="rn-chart-type-select" id="rnAirportChartType" title="Chọn kiểu biểu đồ"><option value="bar">Biểu đồ cột</option><option value="line">Biểu đồ đường</option><option value="donut">Biểu đồ tròn</option></select></div><p class="rn-card-subtitle">Chỉ tính Finished và Delay</p><div class="rn-airport-chart-scroll rn-dashboard-chart-scroll" id="rnDashboardBarsWrap"><div class="rn-airport-chart" id="rnDashboardBars"></div></div><div class="rn-legend"><span><i class="rn-dot" style="background:#2387c8"></i>Cất cánh</span><span><i class="rn-dot" style="background:#20b486"></i>Hạ cánh</span></div><div class="rn-card-subtitle rn-dashboard-bar-detail" id="rnDashboardBarDetail">Nhấn vào một cụm cột để xem trạng thái của sân bay</div></article>' +
                '<article class="rn-card wide rn-trend-card"><div class="rn-trend-head"><div><h2>Xu hướng khai thác theo ' + (trend.period === 'month' ? 'tháng' : 'ngày') + '</h2><p class="rn-card-subtitle">Chỉ gồm chuyến Hoàn thành và Delay • Nguồn: ' + esc(trend.source) + '</p></div><div class="rn-trend-legend"><span><i style="background:#9bb0c1"></i>Cùng kỳ năm trước</span><span><i style="background:#2387c8"></i>Kỳ hiện tại</span><select class="rn-chart-type-select" id="rnTrendChartType" title="Chọn kiểu biểu đồ"><option value="line">Biểu đồ đường</option><option value="bar">Biểu đồ cột</option></select></div></div><div class="rn-trend-chart-wrap"><canvas id="rnDashboardTrend" class="rn-trend-canvas"></canvas><div id="rnDashboardTrendTooltip" class="rn-trend-tooltip" role="status"></div></div><p class="rn-trend-hint"><i class="fa fa-mouse-pointer"></i> Di chuột lên từng mốc để xem số lượng chuyến bay.</p></article>' +
                '<article class="rn-card wide"><h2>Truy cập nhanh báo cáo</h2><p class="rn-card-subtitle">Mở báo cáo chi tiết tương ứng với từng khu vực dữ liệu</p><div class="rn-links"><a class="rn-link" href="../ReportNew/FlightOperationOverview.aspx">Tổng quan khai thác <i class="fa fa-arrow-right"></i></a><a class="rn-link" href="../ReportNew/FlightStatusRate.aspx">Tỷ lệ trạng thái <i class="fa fa-arrow-right"></i></a><a class="rn-link" href="../ReportNew/FlightTrendAnalysis.aspx">Phân tích xu hướng <i class="fa fa-arrow-right"></i></a></div></article></div>';
            var old = app.querySelector('.rn-grid');
            if (old) old.outerHTML = html;
            var statusType = app.querySelector('#rnStatusChartType');
            var airportType = app.querySelector('#rnAirportChartType');
            var trendType = app.querySelector('#rnTrendChartType');
            statusType.value = chartTypes.status;
            airportType.value = chartTypes.airports;
            trendType.value = chartTypes.trend;
            statusType.onchange = function () { chartTypes.status = this.value; renderDonut(); };
            airportType.onchange = function () { chartTypes.airports = this.value; renderAirportChart(); };
            trendType.onchange = function () {
                chartTypes.trend = this.value;
                renderTrend(app.querySelector('#rnDashboardTrend'), app.querySelector('#rnDashboardTrendTooltip'), trend);
            };
            renderDonut();
            renderAirportChart();
            renderTrend(app.querySelector('#rnDashboardTrend'), app.querySelector('#rnDashboardTrendTooltip'), trend);
        }

        function renderTrend(canvas, tooltip, data) {
            if (resizeHandler) window.removeEventListener('resize', resizeHandler);
            var labels = data.labels || [];
            var current = data.current || [];
            var previous = data.previous || [];
            var hover = -1;
            var progress = 0;
            var startedAt = null;
            var geometry = null;

            function draw(valueProgress) {
                var rect = canvas.getBoundingClientRect();
                var width = Math.max(320, Math.round(rect.width));
                var height = Math.max(320, Math.round(rect.height));
                var dpr = window.devicePixelRatio || 1;
                var targetWidth = Math.round(width * dpr);
                var targetHeight = Math.round(height * dpr);
                if (canvas.width !== targetWidth) canvas.width = targetWidth;
                if (canvas.height !== targetHeight) canvas.height = targetHeight;
                var ctx = canvas.getContext('2d');
                ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
                ctx.clearRect(0, 0, width, height);
                var pad = { left: 58, right: 28, top: 32, bottom: 46 };
                var plotWidth = width - pad.left - pad.right;
                var plotHeight = height - pad.top - pad.bottom;
                var maximum = Math.max.apply(null, current.concat(previous).concat([1]));
                maximum = Math.max(5, Math.ceil(maximum * 1.15 / 5) * 5);
                var barMode = chartTypes.trend === 'bar';
                var stepX = barMode
                    ? (labels.length ? plotWidth / labels.length : plotWidth)
                    : (labels.length > 1 ? plotWidth / (labels.length - 1) : 0);
                var xAt = function (index) {
                    if (barMode) return pad.left + stepX * (index + 0.5);
                    return labels.length > 1 ? pad.left + index * stepX : pad.left + plotWidth / 2;
                };
                var yAt = function (value) { return pad.top + plotHeight - (value * valueProgress / maximum) * plotHeight; };

                ctx.font = '11px Roboto, Arial, sans-serif';
                for (var grid = 0; grid <= 5; grid++) {
                    var y = pad.top + grid * plotHeight / 5;
                    ctx.beginPath(); ctx.moveTo(pad.left, y); ctx.lineTo(width - pad.right, y);
                    ctx.strokeStyle = '#e4edf4'; ctx.lineWidth = 1; ctx.stroke();
                    ctx.fillStyle = '#7b8fa1'; ctx.textAlign = 'right'; ctx.textBaseline = 'middle';
                    ctx.fillText(number(Math.round(maximum * (5 - grid) / 5)), pad.left - 10, y);
                }

                // Chế độ cột: mỗi mốc gồm cặp cột (kỳ trước xám, kỳ hiện tại xanh).
                function barSeries(values, color, side) {
                    var barWidth = Math.max(3, Math.min(24, stepX * 0.3));
                    var points = values.map(function (value, index) {
                        var x = xAt(index) + (side < 0 ? -barWidth - 1 : 1);
                        var y = yAt(value);
                        ctx.fillStyle = color;
                        ctx.beginPath();
                        if (ctx.roundRect) ctx.roundRect(x, y, barWidth, pad.top + plotHeight - y, [3, 3, 0, 0]);
                        else ctx.rect(x, y, barWidth, pad.top + plotHeight - y);
                        ctx.fill();
                        if (index === hover) { ctx.strokeStyle = '#173f59'; ctx.lineWidth = 1.5; ctx.stroke(); }
                        return { x: xAt(index), y: y };
                    });
                    return points;
                }
                function line(values, color, fillArea) {
                    var points = values.map(function (value, index) { return { x: xAt(index), y: yAt(value) }; });
                    if (!points.length) return points;
                    if (fillArea) {
                        var gradient = ctx.createLinearGradient(0, pad.top, 0, pad.top + plotHeight);
                        gradient.addColorStop(0, 'rgba(35,135,200,.22)'); gradient.addColorStop(1, 'rgba(35,135,200,0)');
                        ctx.beginPath(); ctx.moveTo(points[0].x, pad.top + plotHeight); ctx.lineTo(points[0].x, points[0].y);
                        for (var fillIndex = 0; fillIndex < points.length - 1; fillIndex++) {
                            var fillMiddle = (points[fillIndex].x + points[fillIndex + 1].x) / 2;
                            ctx.bezierCurveTo(fillMiddle, points[fillIndex].y, fillMiddle, points[fillIndex + 1].y, points[fillIndex + 1].x, points[fillIndex + 1].y);
                        }
                        ctx.lineTo(points[points.length - 1].x, pad.top + plotHeight); ctx.closePath(); ctx.fillStyle = gradient; ctx.fill();
                    }
                    ctx.beginPath(); ctx.moveTo(points[0].x, points[0].y);
                    for (var index = 0; index < points.length - 1; index++) {
                        var middle = (points[index].x + points[index + 1].x) / 2;
                        ctx.bezierCurveTo(middle, points[index].y, middle, points[index + 1].y, points[index + 1].x, points[index + 1].y);
                    }
                    ctx.strokeStyle = color; ctx.lineWidth = fillArea ? 3.5 : 3; ctx.lineCap = 'round'; ctx.stroke();
                    points.forEach(function (point, pointIndex) {
                        ctx.beginPath(); ctx.arc(point.x, point.y, pointIndex === hover ? 6 : 3.5, 0, Math.PI * 2); ctx.fillStyle = color; ctx.fill();
                        if (pointIndex === hover) { ctx.strokeStyle = '#fff'; ctx.lineWidth = 4; ctx.stroke(); }
                    });
                    return points;
                }
                var previousPoints = barMode ? barSeries(previous, '#9bb0c1', -1) : line(previous, '#9bb0c1', false);
                var currentPoints = barMode ? barSeries(current, '#2387c8', 1) : line(current, '#2387c8', true);
                if (hover >= 0) {
                    ctx.save(); ctx.setLineDash([5, 5]); ctx.beginPath(); ctx.moveTo(xAt(hover), pad.top); ctx.lineTo(xAt(hover), pad.top + plotHeight); ctx.strokeStyle = 'rgba(35,135,200,.45)'; ctx.stroke(); ctx.restore();
                }
                var labelStep = Math.max(1, Math.ceil(labels.length / 12));
                ctx.fillStyle = '#60778b'; ctx.textAlign = 'center'; ctx.textBaseline = 'top';
                labels.forEach(function (label, index) { if (index % labelStep === 0 || index === labels.length - 1) ctx.fillText(label, xAt(index), pad.top + plotHeight + 14); });
                geometry = { left: xAt(0), right: width - pad.right, stepX: stepX, width: width, current: currentPoints, previous: previousPoints };
            }

            function animate(timestamp) {
                if (typeof canvas.isConnected !== 'undefined' && !canvas.isConnected) return;
                if (startedAt === null) startedAt = timestamp;
                progress = Math.min(1, (timestamp - startedAt) / 650);
                var eased = 1 - Math.pow(1 - progress, 3);
                draw(eased);
                if (progress < 1) window.requestAnimationFrame(animate);
            }

            canvas.onmousemove = function (event) {
                if (!geometry || !labels.length) return;
                var rect = canvas.getBoundingClientRect();
                var mouseX = event.clientX - rect.left;
                if (mouseX < geometry.left - 18 || mouseX > geometry.right + 18) return;
                hover = labels.length === 1 ? 0 : Math.max(0, Math.min(labels.length - 1, Math.round((mouseX - geometry.left) / geometry.stepX)));
                draw(1);
                tooltip.innerHTML = '<strong>' + esc(labels[hover]) + '</strong><span><i style="background:#2387c8"></i>Kỳ hiện tại: <b>' + number(current[hover]) + ' chuyến</b></span><span><i style="background:#9bb0c1"></i>Cùng kỳ năm trước: <b>' + number(previous[hover]) + ' chuyến</b></span>';
                tooltip.style.left = Math.max(90, Math.min(geometry.width - 90, geometry.current[hover].x)) + 'px';
                tooltip.style.top = Math.max(8, event.clientY - rect.top - 100) + 'px';
                tooltip.classList.add('is-visible');
            };
            canvas.onmouseleave = function () { hover = -1; tooltip.classList.remove('is-visible'); draw(1); };
            resizeHandler = function () { draw(1); };
            window.addEventListener('resize', resizeHandler);
            window.requestAnimationFrame(animate);
        }

        function render(data) {
            dashboardData = { status: data[0], overview: data[1], trend: data[2] };
            selectedAirport = '';
            selectedStatus = '';
            syncAirports(dashboardData.overview.airports);
            renderKpis();
            renderLayout();
        }

        function load() {
            apply.disabled = true;
            apply.textContent = 'Đang đồng bộ...';
            showState('Đang tải dữ liệu', 'Hệ thống đang tổng hợp dữ liệu theo khoảng ngày đã chọn.', 'fa-refresh fa-spin');
            if (!window.jQuery || !window.jQuery.ajax || !window.jQuery.Deferred) {
                showState('Không thể tải dữ liệu', 'Thư viện xử lý yêu cầu chưa được nạp.', 'fa-exclamation-triangle');
                apply.disabled = false;
                apply.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
                return;
            }
            var currentDay = from.value === formatDate(today) && to.value === formatDate(today);
            var fromValue = new Date(from.value + 'T00:00:00');
            var toValue = new Date(to.value + 'T00:00:00');
            var dayCount = Math.round((toValue - fromValue) / 86400000) + 1;
            var trendPeriod = dayCount > 62 ? 'month' : 'day';
            var statusUrl = '../ReportNew/FlightStatusRate.aspx/GetData';
            var overviewUrl = '../ReportNew/FlightOperationOverview.aspx/GetData';
            var trendUrl = '../ReportNew/FlightTrendAnalysis.aspx/GetTrend';
            window.jQuery.when(
                post(statusUrl, {
                    fromDate: from.value, toDate: to.value, airport: airport.value,
                    oper: 'ALL', currentDay: currentDay
                }),
                post(overviewUrl, {
                    fromDate: from.value, toDate: to.value, airport: airport.value
                }),
                post(trendUrl, {
                    fromDate: from.value, toDate: to.value, airport: airport.value,
                    oper: 'ALL', period: trendPeriod
                })
            ).done(function (status, overview, trend) {
                render([status, overview, trend]);
            }).fail(function (error) {
                showState('Không thể tải dữ liệu', error && error.message ? error.message : 'Máy chủ không trả về dữ liệu dashboard.', 'fa-exclamation-triangle');
            }).always(function () {
                apply.disabled = false;
                apply.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
            });
        }

        apply.onclick = load;
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
