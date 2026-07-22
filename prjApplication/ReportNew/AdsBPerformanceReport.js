(function () {
    'use strict';

    var page = document.querySelector('.adsb-report-page');
    if (!page) return;

    var rows = [], currentTrend = [], pageIndex = 1;
    function byId(id) { return document.getElementById(id); }
    function value(id) { return byId(id).value; }
    function number(value) { return Number(value || 0).toLocaleString('vi-VN'); }
    function escapeHtml(value) {
        return String(value == null ? '' : value).replace(/[&<>'"]/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', "'": '&#39;', '"': '&quot;' }[c];
        });
    }
    function post(url, payload) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(payload || {})
        }).then(function (response) {
            if (!response.ok) return response.text().then(function (body) { throw new Error(body || ('HTTP ' + response.status)); });
            return response.json();
        }).then(function (result) { return result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result; });
    }
    function iso(date) {
        var y = date.getFullYear(), m = String(date.getMonth() + 1).padStart(2, '0'), d = String(date.getDate()).padStart(2, '0');
        return y + '-' + m + '-' + d;
    }
    function setDefaultDates() {
        var today = new Date(), first = new Date(today.getFullYear(), today.getMonth(), 1);
        if (!value('adsbFromDate')) byId('adsbFromDate').value = iso(first);
        if (!value('adsbToDate')) byId('adsbToDate').value = iso(today);
    }
    function payload(includeOper) {
        var data = { fromDate: value('adsbFromDate'), toDate: value('adsbToDate'), permType: value('adsbPermType') };
        if (includeOper) data.oper = value('adsbOper');
        return data;
    }
    function showError(error) {
        var box = byId('adsbError');
        box.textContent = 'Không thể tải dữ liệu: ' + (error && error.message ? error.message : error);
        box.hidden = false;
    }
    function hideError() { byId('adsbError').hidden = true; }
    function loadOperators(keepValue) {
        var current = keepValue ? value('adsbOper') : 'ALL';
        return post(page.dataset.operatorsEndpoint, payload(false)).then(function (operators) {
            var select = byId('adsbOper');
            select.innerHTML = '<option value="ALL">Tất cả hãng</option>' + (operators || []).map(function (oper) {
                return '<option value="' + escapeHtml(oper) + '">' + escapeHtml(oper) + '</option>';
            }).join('');
            if ([].some.call(select.options, function (option) { return option.value === current; })) select.value = current;
        });
    }
    function renderKpis(data) {
        var total = Number(data.total || 0), ld = Number(data.ld || 0), of = Number(data.of || 0);
        byId('adsbTotal').textContent = number(total);
        byId('adsbLd').textContent = number(ld);
        byId('adsbOf').textContent = number(of);
        byId('adsbOperCount').textContent = number(data.operatorCount);
        byId('adsbLdRate').textContent = (total ? ld * 100 / total : 0).toFixed(1) + '%';
        byId('adsbOfRate').textContent = (total ? of * 100 / total : 0).toFixed(1) + '%';
        byId('adsbUpdatedAt').textContent = data.serverTime || '--:--';
    }
    function svgNode(name, attrs) {
        var node = document.createElementNS('http://www.w3.org/2000/svg', name);
        Object.keys(attrs || {}).forEach(function (key) { node.setAttribute(key, attrs[key]); });
        return node;
    }
    function renderChart(points) {
        var host = byId('adsbChart');
        host.innerHTML = '';
        if (!points || !points.length) { host.innerHTML = '<div class="adsb-empty">Không có dữ liệu trong khoảng lọc.</div>'; return; }
        var width = Math.max(760, host.clientWidth || 900), height = 350, left = 54, right = 24, top = 24, bottom = 48;
        var plotW = width - left - right, plotH = height - top - bottom;
        var max = Math.max(1, Math.max.apply(null, points.map(function (p) { return Math.max(Number(p.ld), Number(p.of)); })));
        var svg = svgNode('svg', { viewBox: '0 0 ' + width + ' ' + height, role: 'img', 'aria-label': 'Biểu đồ LD và O/F theo ngày' });
        for (var i = 0; i <= 5; i++) {
            var y = top + plotH * i / 5, grid = svgNode('line', { x1: left, y1: y, x2: width - right, y2: y, class: 'adsb-grid' });
            svg.appendChild(grid);
            var tick = svgNode('text', { x: left - 10, y: y + 4, class: 'adsb-axis-label', 'text-anchor': 'end' });
            tick.textContent = Math.round(max * (5 - i) / 5); svg.appendChild(tick);
        }
        function xAt(index) { return left + (points.length === 1 ? plotW / 2 : plotW * index / (points.length - 1)); }
        function yAt(v) { return top + plotH - Number(v || 0) * plotH / max; }
        function linePath(field) { return points.map(function (p, index) { return (index ? 'L' : 'M') + xAt(index).toFixed(1) + ',' + yAt(p[field]).toFixed(1); }).join(' '); }
        ['ld', 'of'].forEach(function (field) {
            svg.appendChild(svgNode('path', { d: linePath(field), class: 'adsb-line ' + field }));
            points.forEach(function (point, index) {
                var circle = svgNode('circle', { cx: xAt(index), cy: yAt(point[field]), r: 5, class: 'adsb-point ' + field, tabindex: '0' });
                var title = svgNode('title'); title.textContent = point.label + ' · ' + field.toUpperCase() + ': ' + number(point[field]); circle.appendChild(title); svg.appendChild(circle);
            });
        });
        var labelStep = Math.max(1, Math.ceil(points.length / 12));
        points.forEach(function (point, index) {
            if (index % labelStep !== 0 && index !== points.length - 1) return;
            var label = svgNode('text', { x: xAt(index), y: height - 16, class: 'adsb-axis-label', 'text-anchor': 'middle' });
            label.textContent = point.label; svg.appendChild(label);
        });
        host.appendChild(svg);
    }
    function renderTable() {
        var size = Number(value('adsbPageSize')), pages = Math.max(1, Math.ceil(rows.length / size));
        pageIndex = Math.min(Math.max(1, pageIndex), pages);
        var start = (pageIndex - 1) * size, current = rows.slice(start, start + size);
        byId('adsbTableBody').innerHTML = current.map(function (row) {
            var statusClass = Number(row.status) === 1 ? 's1' : (Number(row.status) === 2 ? 's2' : 'unknown');
            return '<tr><td>' + row.no + '</td><td><strong>' + escapeHtml(row.callsign) + '</strong></td><td>' + escapeHtml(row.oper || '-') + '</td>' +
                '<td><span class="adsb-perm ' + (row.permType === 'LD' ? 'ld' : 'of') + '">' + escapeHtml(row.permType || '-') + '</span></td>' +
                '<td>' + escapeHtml(row.fromAirp || '-') + '</td><td>' + escapeHtml(row.toAirp || '-') + '</td><td>' + escapeHtml(row.etd || '-') + '</td><td>' + escapeHtml(row.eta || '-') + '</td>' +
                '<td><span class="adsb-status ' + statusClass + '">' + escapeHtml(row.statusText) + '</span></td><td>' + escapeHtml(row.date) + '</td><td>' + escapeHtml(row.updatedAtUtc || '-') + '</td></tr>';
        }).join('');
        if (!current.length) byId('adsbTableBody').innerHTML = '<tr><td colspan="11" class="adsb-no-data">Không có dữ liệu.</td></tr>';
        byId('adsbTableInfo').textContent = number(rows.length) + ' bản ghi';
        byId('adsbPageInfo').textContent = 'Trang ' + pageIndex + '/' + pages;
        byId('adsbPrev').disabled = pageIndex <= 1; byId('adsbNext').disabled = pageIndex >= pages;
    }
    function loadData() {
        var button = byId('adsbApply'); button.disabled = true; hideError();
        return post(page.dataset.endpoint, payload(true)).then(function (data) {
            rows = data.rows || []; currentTrend = data.trend || []; pageIndex = 1; renderKpis(data); renderChart(currentTrend); renderTable();
        }).catch(showError).then(function () { button.disabled = false; });
    }
    function apply() { loadOperators(true).then(loadData).catch(showError); }

    byId('adsbApply').addEventListener('click', apply);
    byId('adsbPermType').addEventListener('change', function () { loadOperators(false).catch(showError); });
    byId('adsbPageSize').addEventListener('change', function () { pageIndex = 1; renderTable(); });
    byId('adsbPrev').addEventListener('click', function () { if (pageIndex > 1) { pageIndex--; renderTable(); } });
    byId('adsbNext').addEventListener('click', function () { pageIndex++; renderTable(); });
    window.addEventListener('resize', function () { clearTimeout(window.__adsbResize); window.__adsbResize = setTimeout(function () { renderChart(currentTrend); }, 180); });

    setDefaultDates();
    loadOperators(false).then(loadData).catch(showError);
}());
