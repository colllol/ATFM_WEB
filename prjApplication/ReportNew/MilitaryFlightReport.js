(function () {
    var currentFlights = [];

    function camelize(value) {
        if (Array.isArray(value)) return value.map(camelize);
        if (!value || typeof value !== 'object') return value;
        var result = {};
        Object.keys(value).forEach(function (key) {
            result[key.charAt(0).toLowerCase() + key.slice(1)] = camelize(value[key]);
        });
        return result;
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : value;
        return node.innerHTML;
    }

    function xmlEsc(value) {
        return String(value == null ? '' : value).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    function iso(date) {
        return date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');
    }

    function displayDate(value) {
        var parts = String(value || '').split('-');
        return parts.length === 3 ? parts[2] + '/' + parts[1] + '/' + parts[0] : value;
    }

    function post(data) {
        return fetch(window.reportApiBase + 'api/MilitaryFlightReport/GetData', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        }).then(function (response) {
            return response.json().then(function (result) {
                if (!response.ok) throw new Error(result.Message || result.message || ('HTTP ' + response.status));
                return result;
            });
        }).then(function (result) {
            if (result.Code && result.Code !== '00') throw new Error(result.Message || 'Không thể tải dữ liệu.');
            return camelize(Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result.d) || {};
        });
    }

    function renderShell(app) {
        var today = iso(new Date());
        app.innerHTML = '<header class="rn-hero"><div><span class="rn-eyebrow">VATM • FLIGHT ANALYTICS</span>' +
            '<h1>Báo cáo chuyến bay quân sự</h1><p>Tra cứu dữ liệu chuyến bay quân sự theo sân bay, mục đích và khoảng ngày</p></div>' +
            '<div class="rn-live"><span>Dữ liệu cập nhật</span><strong id="rnMilitaryTime">--:--</strong><small>API T_FINISHFLIGHTS_MILITARY</small></div></header>' +
            '<div class="rn-filters rn-military-filters"><div class="rn-field"><label>Sân bay</label><select id="rnAirport"><option value="ALL">Tất cả sân bay</option></select></div>' +
            '<div class="rn-field"><label>Mục đích</label><select id="rnPurpose"><option value="ALL">Tất cả mục đích</option></select></div>' +
            '<div class="rn-field"><label>Từ ngày</label><input id="rnFrom" type="date" value="' + today + '"></div>' +
            '<div class="rn-field"><label>Đến ngày</label><input id="rnTo" type="date" value="' + today + '"></div>' +
            '<div class="rn-military-actions"><button class="rn-filter-button" type="button" id="rnApply"><i class="fa fa-filter"></i> Áp dụng</button>' +
            '<button class="rn-filter-button rn-export-button" type="button" id="rnExport" disabled><i class="fa fa-file-excel-o"></i> Export Excel</button></div></div>' +
            '<div id="rnMilitaryResult"><article class="rn-card wide rn-empty"><p>Đang tải dữ liệu...</p></article></div>';
    }

    function updateSelect(select, values, allLabel) {
        var selected = select.value;
        select.innerHTML = '<option value="ALL">' + allLabel + '</option>' + (values || []).map(function (value) {
            return '<option value="' + esc(value) + '">' + esc(value) + '</option>';
        }).join('');
        if ((values || []).indexOf(selected) >= 0) select.value = selected;
    }

    function renderTable(app, data) {
        currentFlights = data.flights || [];
        var page = 1;
        var pageSize = 100;
        app.querySelector('#rnMilitaryResult').innerHTML = '<div class="rn-grid"><article class="rn-card wide rn-flight-list-card">' +
            '<div class="rn-flight-list-head"><div><h2 id="rnFlightListTitle">Chi tiết các chuyến bay</h2><p class="rn-card-subtitle">Nguồn: ' + esc(data.source || 'T_FINISHFLIGHTS_MILITARY') + '</p></div>' +
            '<label class="rn-page-size">Số dòng <select id="rnPageSize"><option>25</option><option>50</option><option selected>100</option><option>200</option><option>500</option></select></label></div>' +
            '<div class="rn-table-wrap rn-flight-detail-wrap"><table class="rn-table rn-flight-detail-table"><thead><tr>' +
            '<th>STT</th><th>P_TYPE</th><th>FROM_AIRP</th><th>TO_AIRP</th><th>ETD</th><th>ETA</th><th>ATD</th><th>ATA</th><th>PURPOSE</th><th>FLIGHTDATE</th>' +
            '</tr></thead><tbody id="rnFlightRows"></tbody></table></div>' +
            '<div class="rn-flight-pager"><button type="button" class="rn-filter-button" id="rnPrev">‹ Trước</button><span id="rnPageInfo"></span><button type="button" class="rn-filter-button" id="rnNext">Sau ›</button></div></article></div>';

        function draw() {
            var pages = Math.max(1, Math.ceil(currentFlights.length / pageSize));
            if (page > pages) page = pages;
            var start = (page - 1) * pageSize;
            var rows = currentFlights.slice(start, start + pageSize);
            app.querySelector('#rnFlightRows').innerHTML = rows.length ? rows.map(function (flight, index) {
                return '<tr><td class="rn-stt">' + (start + index + 1) + '</td><td><b>' + esc(flight.pType) + '</b></td>' +
                    '<td>' + esc(flight.fromAirp) + '</td><td>' + esc(flight.toAirp) + '</td><td>' + esc(flight.etd) + '</td><td>' + esc(flight.eta) + '</td>' +
                    '<td>' + esc(flight.atd) + '</td><td>' + esc(flight.ata) + '</td><td>' + esc(flight.purpose) + '</td><td>' + esc(displayDate(flight.flightDate)) + '</td></tr>';
            }).join('') : '<tr><td colspan="10" class="rn-no-data">Không có chuyến bay phù hợp.</td></tr>';
            app.querySelector('#rnFlightListTitle').textContent = 'Chi tiết các chuyến bay (' + currentFlights.length.toLocaleString('vi-VN') + ')';
            app.querySelector('#rnPageInfo').textContent = 'Trang ' + page + '/' + pages + ' • ' + currentFlights.length.toLocaleString('vi-VN') + ' dòng';
            app.querySelector('#rnPrev').disabled = page <= 1;
            app.querySelector('#rnNext').disabled = page >= pages;
        }
        app.querySelector('#rnPageSize').onchange = function () { pageSize = parseInt(this.value, 10); page = 1; draw(); };
        app.querySelector('#rnPrev').onclick = function () { if (page > 1) { page--; draw(); } };
        app.querySelector('#rnNext').onclick = function () { if (page < Math.max(1, Math.ceil(currentFlights.length / pageSize))) { page++; draw(); } };
        draw();
    }

    function exportExcel(app) {
        if (!currentFlights.length) return;
        if (window.ReportControls) {
            window.ReportControls.exportExcel({
                fileName: 'MilitaryFlightReport_' + app.querySelector('#rnFrom').value + '_' + app.querySelector('#rnTo').value,
                rows: currentFlights,
                columns: [
                    { label: 'No', key: '__no' }, { label: 'P_TYPE', key: 'pType' },
                    { label: 'FROM_AIRP', key: 'fromAirp' }, { label: 'TO_AIRP', key: 'toAirp' },
                    { label: 'ETD', key: 'etd' }, { label: 'ETA', key: 'eta' },
                    { label: 'ATD', key: 'atd' }, { label: 'ATA', key: 'ata' },
                    { label: 'PURPOSE', key: 'purpose' }, { label: 'FLIGHTDATE', key: 'flightDate', format: displayDate }
                ]
            });
            return;
        }
        var headers = ['P_TYPE', 'FROM_AIRP', 'TO_AIRP', 'ETD', 'ETA', 'ATD', 'ATA', 'PURPOSE', 'FLIGHTDATE'];
        var keys = ['pType', 'fromAirp', 'toAirp', 'etd', 'eta', 'atd', 'ata', 'purpose', 'flightDate'];
        var rows = '<Row>' + headers.map(function (header) { return '<Cell><Data ss:Type="String">' + header + '</Data></Cell>'; }).join('') + '</Row>' +
            currentFlights.map(function (flight) {
                return '<Row>' + keys.map(function (key) { return '<Cell><Data ss:Type="String">' + xmlEsc(key === 'flightDate' ? displayDate(flight[key]) : flight[key]) + '</Data></Cell>'; }).join('') + '</Row>';
            }).join('');
        var workbook = '<?xml version="1.0" encoding="UTF-8"?><?mso-application progid="Excel.Sheet"?>' +
            '<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"><Worksheet ss:Name="Military Flights"><Table>' + rows + '</Table></Worksheet></Workbook>';
        var url = URL.createObjectURL(new Blob([workbook], { type: 'application/vnd.ms-excel;charset=utf-8' }));
        var link = document.createElement('a');
        link.href = url;
        link.download = 'MilitaryFlightReport_' + app.querySelector('#rnFrom').value + '_' + app.querySelector('#rnTo').value + '.xls';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        setTimeout(function () { URL.revokeObjectURL(url); }, 0);
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="military"]');
        if (!app) return;
        renderShell(app);
        if (window.ReportControls) window.ReportControls.enhanceAll(app);

        function load() {
            var from = app.querySelector('#rnFrom').value;
            var to = app.querySelector('#rnTo').value;
            if (!from || !to || from > to) { alert('Khoảng ngày lọc không hợp lệ.'); return; }
            var button = app.querySelector('#rnApply');
            button.disabled = true;
            button.textContent = 'Đang tải...';
            app.querySelector('#rnExport').disabled = true;
            post({ fromDate: from, toDate: to, airport: app.querySelector('#rnAirport').value, purpose: app.querySelector('#rnPurpose').value })
                .then(function (data) {
                    updateSelect(app.querySelector('#rnAirport'), data.airports, 'Tất cả sân bay');
                    updateSelect(app.querySelector('#rnPurpose'), data.purposes, 'Tất cả mục đích');
                    renderTable(app, data);
                    app.querySelector('#rnMilitaryTime').textContent = new Date().toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
                    app.querySelector('#rnExport').disabled = !currentFlights.length;
                })
                .catch(function (error) {
                    currentFlights = [];
                    app.querySelector('#rnMilitaryResult').innerHTML = '<article class="rn-card wide rn-empty"><div class="rn-empty-icon"><i class="fa fa-exclamation-triangle"></i></div><h2>Không thể tải dữ liệu</h2><p>' + esc(error.message) + '</p></article>';
                })
                .then(function () {
                    button.disabled = false;
                    button.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
                });
        }
        app.querySelector('#rnApply').onclick = load;
        app.querySelector('#rnExport').onclick = function () { exportExcel(app); };
        app.querySelector('#rnFrom').onchange = app.querySelector('#rnTo').onchange = function () {
            app.querySelector('#rnAirport').value = 'ALL';
            app.querySelector('#rnPurpose').value = 'ALL';
        };
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
