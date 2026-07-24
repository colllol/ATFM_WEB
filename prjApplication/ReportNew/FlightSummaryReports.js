(function () {
    'use strict';

    function escapeHtml(value) {
        return String(value == null ? '' : value).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    function number(value) { return Number(value || 0).toLocaleString('vi-VN'); }
    function today(offset) { var d = new Date(); d.setDate(d.getDate() + (offset || 0)); return d.toISOString().slice(0, 10); }

    function post(method, payload) {
        return fetch((window.reportApiBase || '/') + 'api/' + method, {
            method: 'POST', headers: { 'Content-Type': 'application/json; charset=utf-8' }, body: JSON.stringify(payload)
        }).then(function (response) {
            if (!response.ok) throw new Error('HTTP ' + response.status);
            return response.json();
        }).then(function (result) {
            if (result && result.d) result = result.d;
            if (!result || result.Code !== '00') throw new Error((result && result.Message) || 'Không thể lấy dữ liệu');
            return result.ListValue || {};
        });
    }

    function shell(app, military) {
        var title = military ? 'Báo cáo chuyến bay quân sự' : 'Tổng hợp chuyến bay dân dụng';
        var subtitle = military ? 'Dữ liệu nghiệp vụ VIP, quân sự từ REPORT_DHB' : 'Tổng hợp trực tiếp dữ liệu chuyến bay dân dụng';
        app.innerHTML = '<header class="rn-hero"><div><span class="rn-eyebrow">VATM • FLIGHT ANALYTICS</span><h1>' + title + '</h1><p>' + subtitle + '</p></div><div class="rn-live"><span>Nguồn dữ liệu</span><strong id="summarySource">API</strong><small id="summaryState">Sẵn sàng</small></div></header>' +
            '<div class="rn-filters"><div class="rn-field"><label>Sân bay</label><select id="summaryAirport"><option value="ALL">Tất cả sân bay</option></select></div><div class="rn-field"><label>Từ ngày</label><input id="summaryFrom" type="date" value="' + today(-30) + '"></div><div class="rn-field"><label>Đến ngày</label><input id="summaryTo" type="date" value="' + today(0) + '"></div><button class="rn-filter-button" id="summaryApply" type="button"><i class="fa fa-filter"></i> Áp dụng</button></div>' +
            '<div id="summaryError" class="rn-card wide" style="display:none;color:#b42318"></div><div id="summaryContent"><article class="rn-card wide rn-empty"><h2>Đang tải dữ liệu...</h2></article></div>';
    }

    function kpis(items) {
        return '<div class="rn-kpis">' + items.map(function (x) { return '<div class="rn-kpi" style="--accent:' + x.color + '"><span>' + x.name + '</span><strong>' + number(x.value) + '</strong><small>' + x.note + '</small></div>'; }).join('') + '</div>';
    }

    function airportTable(items) {
        return '<article class="rn-card wide rn-civil-airport-table"><h2>Lưu lượng sân bay</h2><p class="rn-card-subtitle">Các đầu mối có lưu lượng cao nhất</p><div class="rn-table-wrap"><table class="rn-table"><thead><tr><th>Sân bay</th><th>Đi</th><th>Đến</th><th>Tổng</th></tr></thead><tbody>' +
            (items || []).slice(0, 12).map(function (x) { return '<tr><td><b>' + escapeHtml(x.Code) + '</b></td><td>' + number(x.Departures) + '</td><td>' + number(x.Arrivals) + '</td><td>' + number(x.Total) + '</td></tr>'; }).join('') + '</tbody></table></div></article>';
    }

    function flightTable(rows, military) {
        var headers = military ? '<th>Ngày bay</th><th>Số hiệu</th><th>Loại tàu bay</th><th>Đăng ký</th><th>From</th><th>To</th><th>Đường bay</th><th>Số phép</th>' : '<th>Ngày bay</th><th>Chuyến bay</th><th>Hãng</th><th>From</th><th>To</th><th>Trạng thái</th>';
        var body = (rows || []).map(function (x) {
            if (military) return '<tr><td>' + escapeHtml(x.FlightDate) + '</td><td><b>' + escapeHtml(x.Callsign) + '</b></td><td>' + escapeHtml(x.CraftType) + '</td><td>' + escapeHtml(x.Registration) + '</td><td>' + escapeHtml(x.FromAirp) + '</td><td>' + escapeHtml(x.ToAirp) + '</td><td>' + escapeHtml(x.Route) + '</td><td>' + escapeHtml(x.PermitNbr) + '</td></tr>';
            return '<tr><td>' + escapeHtml(x.FlightDate) + '</td><td><b>' + escapeHtml(x.Callsign) + '</b></td><td>' + escapeHtml(x.Oper) + '</td><td>' + escapeHtml(x.FromAirp) + '</td><td>' + escapeHtml(x.ToAirp) + '</td><td>' + escapeHtml(x.Status) + '</td></tr>';
        }).join('');
        return '<article class="rn-card wide rn-detail-card"><h2>Danh sách chuyến bay</h2><p class="rn-card-subtitle">' + number((rows || []).length) + ' bản ghi trong kỳ đã chọn</p><div class="rn-table-wrap"><table class="rn-table"><thead><tr>' + headers + '</tr></thead><tbody>' + body + '</tbody></table></div></article>';
    }

    function renderData(app, data, military) {
        var items = military ? [
            { name: 'Tổng chuyến VIP, quân sự', value: data.Total, color: '#315a7d', note: 'Trong kỳ đã chọn' },
            { name: 'Chuyên cơ / VIP', value: data.Vip, color: '#8a6ee8', note: 'Theo số phép' },
            { name: 'Quân sự', value: data.Military, color: '#2387c8', note: 'Nguồn REPORT_DHB' },
            { name: 'Sân bay khai thác', value: (data.Airports || []).length, color: '#f5a623', note: 'Đầu đi hoặc đến' }
        ] : [
            { name: 'Tổng chuyến dân dụng', value: data.Total, color: '#2387c8', note: 'Trong kỳ đã chọn' },
            { name: 'Nội địa', value: data.Domestic, color: '#20b486', note: number(data.Total ? data.Domestic * 100 / data.Total : 0) + '%' },
            { name: 'Quốc tế', value: data.International, color: '#8a6ee8', note: number(data.Total ? data.International * 100 / data.Total : 0) + '%' },
            { name: 'Hệ số đúng giờ', value: data.OnTimePercent, color: '#f5a623', note: 'OTP (%)' }
        ];
        document.getElementById('summarySource').textContent = data.Source || 'API';
        document.getElementById('summaryState').textContent = 'Đã cập nhật';
        var airportSelect = document.getElementById('summaryAirport');
        var selectedAirport = airportSelect.value;
        airportSelect.innerHTML = '<option value="ALL">Tất cả sân bay</option>' + (data.Airports || []).map(function (item) {
            return '<option value="' + escapeHtml(item.Code) + '">' + escapeHtml(item.Code) + '</option>';
        }).join('');
        airportSelect.value = selectedAirport;
        document.getElementById('summaryContent').innerHTML = kpis(items) + '<div class="rn-grid">' + airportTable(data.Airports) + flightTable(data.Flights, military) + '</div>';
    }

    function start() {
        var app = document.querySelector('.rn-app[data-report-view="military"],.rn-app[data-report-view="civil"]');
        if (!app) return;
        var military = app.getAttribute('data-report-view') === 'military';
        shell(app, military);
        if (window.ReportControls) window.ReportControls.enhanceAll(app);
        function load() {
            var error = document.getElementById('summaryError'); error.style.display = 'none';
            document.getElementById('summaryState').textContent = 'Đang tải...';
            post((military ? 'MilitaryFlightReport' : 'CivilFlightSummary') + '/GetData', {
                FromDate: document.getElementById('summaryFrom').value,
                ToDate: document.getElementById('summaryTo').value,
                Airport: document.getElementById('summaryAirport').value || 'ALL', Oper: 'ALL', CurrentDay: false
            }).then(function (data) { renderData(app, data, military); }).catch(function (e) {
                document.getElementById('summaryState').textContent = 'Lỗi'; error.textContent = e.message; error.style.display = 'block';
            });
        }
        document.getElementById('summaryApply').onclick = load;
        load();
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', start); else start();
}());
