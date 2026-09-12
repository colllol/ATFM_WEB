(function () {
    'use strict';

    var page = document.querySelector('.adsb-report-page');
    if (!page) return;

    var rows = [], currentTrend = [], pageIndex = 1;
    var restoreSidebarAfterModal = false;
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
            // method: 'POST', credentials: 'same-origin',
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(payload || {})
        }).then(function (response) {
            if (!response.ok) return response.text().then(function (body) { throw new Error(body || ('HTTP ' + response.status)); });
            return response.json();
            // }).then(function (result) { return result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result; });
        }).then(function (result) {
            if (result && Object.prototype.hasOwnProperty.call(result, 'd')) result = result.d;
            if (result && result.Code && result.Code !== '00') throw new Error(result.Message || 'Không thể lấy dữ liệu.');
            return result && Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result;
        });
    }
    function normalizeTrend(item) {
        return {
            key: item.Key != null ? item.Key : item.key,
            label: item.Label != null ? item.Label : item.label,
            ld: item.Ld != null ? item.Ld : item.ld,
            of: item.Of != null ? item.Of : item.of,
            other: item.Other != null ? item.Other : item.other
        };
    }
    function normalizeRow(row) {
        return {
            no: row.No != null ? row.No : row.no,
            id: row.Id != null ? row.Id : row.id,
            callsign: row.Callsign != null ? row.Callsign : row.callsign,
            oper: row.Oper != null ? row.Oper : row.oper,
            permType: row.PermType != null ? row.PermType : row.permType,
            fromAirp: row.FromAirp != null ? row.FromAirp : row.fromAirp,
            toAirp: row.ToAirp != null ? row.ToAirp : row.toAirp,
            etd: row.Etd != null ? row.Etd : row.etd,
            eta: row.Eta != null ? row.Eta : row.eta,
            status: row.Status != null ? row.Status : row.status,
            statusText: row.StatusText != null ? row.StatusText : row.statusText,
            date: row.Date != null ? row.Date : row.date,
            updatedAtUtc: row.UpdatedAtUtc != null ? row.UpdatedAtUtc : row.updatedAtUtc,
            timeIn: row.TimeIn != null ? row.TimeIn : row.timeIn,
            timeOut: row.TimeOut != null ? row.TimeOut : row.timeOut,
            isOther: row.IsOther != null ? row.IsOther : row.isOther
        };
    }
    function normalizeData(data) {
        data = data || {};
        var normalizedRows = (data.Rows || data.rows || []).map(normalizeRow);
        var normalizedTrend = (data.Trend || data.trend || []).map(normalizeTrend);
        var otherByDate = {};
        normalizedRows.forEach(function (row) {
            if (!isOther(row)) return;
            var match = String(row.date || '').match(/^(\d{2})[\/-](\d{2})[\/-](\d{4})$/);
            if (!match) return;
            var key = match[3] + '-' + match[2] + '-' + match[1];
            otherByDate[key] = (otherByDate[key] || 0) + 1;
        });
        normalizedTrend.forEach(function (point) {
            if (point.other == null) point.other = otherByDate[point.key] || 0;
        });
        return {
            total: data.Total != null ? data.Total : data.total,
            ld: data.Ld != null ? data.Ld : data.ld,
            of: data.Of != null ? data.Of : data.of,
            other: data.Other != null ? data.Other : data.other,
            operatorCount: data.OperatorCount != null ? data.OperatorCount : data.operatorCount,
            serverTime: data.ServerTime != null ? data.ServerTime : data.serverTime,
            trend: normalizedTrend,
            rows: normalizedRows
        };
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
        var other = data.other == null ? rows.filter(isOther).length : Number(data.other || 0);
        byId('adsbTotal').textContent = number(total);
        byId('adsbLd').textContent = number(ld);
        byId('adsbOf').textContent = number(of);
        byId('adsbOther').textContent = number(other);
        byId('adsbOperCount').textContent = number(data.operatorCount);
        byId('adsbLdRate').textContent = (total ? ld * 100 / total : 0).toFixed(1) + '%';
        byId('adsbOfRate').textContent = (total ? of * 100 / total : 0).toFixed(1) + '%';
        byId('adsbUpdatedAt').textContent = data.serverTime || '--:--';
    }
    function isOther(row) {
        return row && (row.isOther === true || Number(row.isOther) === 1 || String(row.permType || '').toUpperCase() === 'OTHER' || !String(row.fromAirp || '').trim() || !String(row.toAirp || '').trim() || !String(row.oper || '').trim());
    }
    function statusText(status) {
        return Number(status) === 1 ? 'VVHN' : (Number(status) === 2 ? 'VVHM' : 'Không xác định');
    }
    function renderEndDay(data) {
        var list = (data && (data.rows || data.Rows) || []).map(normalizeRow);
        byId('adsbEndDayBody').innerHTML = list.map(function (row) {
            return '<tr><td><strong>' + escapeHtml(row.callsign) + '</strong></td><td>' + escapeHtml(row.oper || '-') + '</td>' +
                '<td><span class="adsb-perm ' + (isOther(row) ? 'other' : (row.permType === 'LD' ? 'ld' : 'of')) + '">' + escapeHtml(isOther(row) ? 'OTHER' : (row.permType || '-')) + '</span></td>' +
                '<td>' + escapeHtml(row.fromAirp || '-') + '</td><td>' + escapeHtml(row.toAirp || '-') + '</td><td>' + escapeHtml(row.etd || '-') + '</td><td>' + escapeHtml(row.eta || '-') + '</td>' +
                '<td>' + escapeHtml(statusText(row.status)) + '</td><td>' + escapeHtml(row.timeIn || '-') + '</td><td>' + escapeHtml(row.timeOut || '-') + '</td><td>' + escapeHtml(row.date || '-') + '</td></tr>';
        }).join('');
        if (!list.length) byId('adsbEndDayBody').innerHTML = '<tr><td colspan="11" class="adsb-no-data">Không có dữ liệu.</td></tr>';
        byId('adsbEndDayInfo').textContent = number(list.length) + ' bản ghi · ' + (data && data.fromDate ? data.fromDate + ' đến ' + data.toDate : 'theo bộ lọc hiện tại');
    }
    function toggleSidebarForModal(opening) {
        var workspace = document.querySelector('#main-container > .atfm-workspace');
        var toggle = document.querySelector('#sidebar .atfm-sidebar-toggle');
        if (!workspace || !toggle || window.innerWidth < 992) return;
        if (opening) {
            restoreSidebarAfterModal = !workspace.classList.contains('atfm-sidebar-collapsed');
            if (restoreSidebarAfterModal) toggle.click();
            return;
        }
        if (restoreSidebarAfterModal && workspace.classList.contains('atfm-sidebar-collapsed')) toggle.click();
        restoreSidebarAfterModal = false;
    }
    function openEndDay() {
        var modal = byId('adsbEndDayModal');
        toggleSidebarForModal(true);
        modal.hidden = false;
        byId('adsbEndDayInfo').textContent = 'Đang tải dữ liệu...';
        byId('adsbEndDayBody').innerHTML = '<tr><td colspan="11" class="adsb-no-data">Đang tải...</td></tr>';
        post(page.dataset.enddayEndpoint, payload(true)).then(function (data) {
            renderEndDay(data && data.d ? data.d : data);
        }).catch(function (error) {
            byId('adsbEndDayInfo').textContent = 'Không thể tải báo cáo: ' + (error.message || error);
        });
    }
    function closeEndDay() {
        byId('adsbEndDayModal').hidden = true;
        toggleSidebarForModal(false);
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
        var max = Math.max(1, Math.max.apply(null, points.map(function (p) { return Math.max(Number(p.ld), Number(p.of), Number(p.other)); })));
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
        ['ld', 'of', 'other'].forEach(function (field) {
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
    // Biểu đồ cột chồng "Tổng số chuyến theo ngày": trục Y tổng chuyến, trục X ngày,
    // mỗi cột chia LD / O/F / OTHER. Style inline toàn bộ để nhúng được vào bản in PDF.
    var stackColors = { ld: '#16aa78', of: '#f59e0b', other: '#e05252' };
    function renderTotalChart(points) {
        var host = byId('adsbTotalChart');
        host.innerHTML = '';
        if (!points || !points.length) { host.innerHTML = '<div class="adsb-empty">Không có dữ liệu trong khoảng lọc.</div>'; return; }
        var width = Math.max(760, host.clientWidth || 900), height = 360, left = 54, right = 24, top = 30, bottom = 48;
        var plotW = width - left - right, plotH = height - top - bottom;
        var totals = points.map(function (p) { return Number(p.ld || 0) + Number(p.of || 0) + Number(p.other || 0); });
        var max = Math.max(1, Math.max.apply(null, totals));
        max = Math.max(5, Math.ceil(max * 1.12 / 5) * 5);
        var svg = svgNode('svg', { viewBox: '0 0 ' + width + ' ' + height, width: width, height: height, role: 'img', 'aria-label': 'Biểu đồ tổng số chuyến theo ngày', style: 'max-width:100%;height:auto' });
        for (var i = 0; i <= 5; i++) {
            var y = top + plotH * i / 5;
            svg.appendChild(svgNode('line', { x1: left, y1: y, x2: width - right, y2: y, stroke: '#e2ecf4', 'stroke-width': 1 }));
            var tick = svgNode('text', { x: left - 10, y: y + 4, fill: '#7b8fa1', 'font-size': 11, 'text-anchor': 'end' });
            tick.textContent = number(Math.round(max * (5 - i) / 5)); svg.appendChild(tick);
        }
        var slot = plotW / points.length;
        var barW = Math.min(46, Math.max(10, slot * 0.6));
        var labelStep = Math.max(1, Math.ceil(points.length / 12));
        points.forEach(function (point, index) {
            var x = left + slot * index + (slot - barW) / 2;
            var baseline = top + plotH;
            var total = totals[index];
            var tip = point.label + ' · Tổng: ' + number(total) + ' (LD: ' + number(point.ld) + ', O/F: ' + number(point.of) + ', Khác: ' + number(point.other) + ')';
            ['ld', 'of', 'other'].forEach(function (field) {
                var valueNumber = Number(point[field] || 0);
                if (!valueNumber) return;
                var h = valueNumber * plotH / max;
                baseline -= h;
                var rect = svgNode('rect', { x: x.toFixed(1), y: baseline.toFixed(1), width: barW.toFixed(1), height: h.toFixed(1), fill: stackColors[field] });
                var title = svgNode('title'); title.textContent = tip; rect.appendChild(title); svg.appendChild(rect);
                if (h >= 15 && barW >= 22) {
                    var segText = svgNode('text', { x: (x + barW / 2).toFixed(1), y: (baseline + h / 2 + 4).toFixed(1), fill: '#ffffff', 'font-size': 10, 'font-weight': 700, 'text-anchor': 'middle' });
                    segText.textContent = number(valueNumber); svg.appendChild(segText);
                }
            });
            if (total && barW >= 16) {
                var totalText = svgNode('text', { x: (x + barW / 2).toFixed(1), y: (baseline - 6).toFixed(1), fill: '#31546f', 'font-size': 11, 'font-weight': 700, 'text-anchor': 'middle' });
                totalText.textContent = number(total); svg.appendChild(totalText);
            }
            if (index % labelStep === 0 || index === points.length - 1) {
                var label = svgNode('text', { x: (x + barW / 2).toFixed(1), y: height - 16, fill: '#60778b', 'font-size': 11, 'text-anchor': 'middle' });
                label.textContent = point.label; svg.appendChild(label);
            }
        });
        host.appendChild(svg);
    }
    function exportColumns() {
        return [
            { label: 'STT', key: '__no' },
            { label: 'CALLSIGN', key: 'callsign' },
            { label: 'OPER', key: 'oper' },
            { label: 'PERMTYPE', key: 'permType', format: function (value, row) { return isOther(row) ? 'OTHER' : (value || '-'); } },
            { label: 'FROM_AIRP', key: 'fromAirp' },
            { label: 'TO_AIRP', key: 'toAirp' },
            { label: 'ETD', key: 'etd' },
            { label: 'ETA', key: 'eta' },
            { label: 'STATUS', key: 'status', format: function (value) { return statusText(value); } },
            { label: 'DATE', key: 'date' },
            { label: 'UPDATED_AT_UTC', key: 'updatedAtUtc' }
        ];
    }
    function exportFileName() { return 'AdsBPerformance_' + value('adsbFromDate') + '_' + value('adsbToDate'); }
    function exportPdf() {
        if (!rows.length) { alert('Không có dữ liệu để xuất PDF.'); return; }
        var columns = exportColumns();
        var chartSvg = document.querySelector('#adsbTotalChart svg');
        var chartHtml = chartSvg && window.ReportControls
            ? new XMLSerializer().serializeToString(window.ReportControls.inlineSvgStyles(chartSvg))
            : '<p>Không có biểu đồ.</p>';
        var legend = '<p style="margin:6px 0 0;font-size:11px;color:#5d768c">' +
            '<span style="color:#16aa78;font-weight:700">■ LD</span> &nbsp; ' +
            '<span style="color:#f59e0b;font-weight:700">■ O/F</span> &nbsp; ' +
            '<span style="color:#e05252;font-weight:700">■ Chuyến bay khác</span></p>';
        var tableHtml = '<table><thead><tr>' + columns.map(function (column) { return '<th>' + escapeHtml(column.label) + '</th>'; }).join('') + '</tr></thead><tbody>' +
            rows.map(function (row, rowIndex) {
                return '<tr>' + columns.map(function (column) {
                    var cell = column.key === '__no' ? rowIndex + 1 : row[column.key];
                    if (column.format) cell = column.format(cell, row);
                    return '<td>' + escapeHtml(cell == null ? '-' : cell) + '</td>';
                }).join('') + '</tr>';
            }).join('') + '</tbody></table>';
        window.ReportControls.printReport({
            title: 'Tổng hợp chỉ số hiệu suất bay từ ADS-B',
            subtitle: 'Nguồn dữ liệu T_TRACKS_LOG • Xuất lúc ' + new Date().toLocaleString('vi-VN'),
            meta: [
                { label: 'Từ ngày', value: value('adsbFromDate') },
                { label: 'Đến ngày', value: value('adsbToDate') },
                { label: 'PERMTYPE', value: value('adsbPermType') },
                { label: 'Hãng', value: value('adsbOper') },
                { label: 'Tổng bản ghi', value: number(rows.length) }
            ],
            sections: [
                { heading: 'Tổng số chuyến theo ngày (LD / O/F / Khác)', html: chartHtml + legend },
                { heading: 'Chi tiết dữ liệu', html: tableHtml }
            ]
        });
    }

    function renderTable() {
        var size = Number(value('adsbPageSize')), pages = Math.max(1, Math.ceil(rows.length / size));
        pageIndex = Math.min(Math.max(1, pageIndex), pages);
        var start = (pageIndex - 1) * size, current = rows.slice(start, start + size);
        byId('adsbTableBody').innerHTML = current.map(function (row) {
            var statusClass = Number(row.status) === 1 ? 's1' : (Number(row.status) === 2 ? 's2' : 'unknown');
            return '<tr><td>' + row.no + '</td><td><strong>' + escapeHtml(row.callsign) + '</strong></td><td>' + escapeHtml(row.oper || '-') + '</td>' +
                '<td><span class="adsb-perm ' + (isOther(row) ? 'other' : (row.permType === 'LD' ? 'ld' : 'of')) + '">' + escapeHtml(isOther(row) ? 'OTHER' : (row.permType || '-')) + '</span></td>' +
                '<td>' + escapeHtml(row.fromAirp || '-') + '</td><td>' + escapeHtml(row.toAirp || '-') + '</td><td>' + escapeHtml(row.etd || '-') + '</td><td>' + escapeHtml(row.eta || '-') + '</td>' +
                '<td><span class="adsb-status ' + statusClass + '">' + escapeHtml(row.statusText) + '</span></td><td>' + escapeHtml(row.date) + '</td><td>' + escapeHtml(row.updatedAtUtc || '-') + '</td></tr>';
        }).join('');
        if (!current.length) byId('adsbTableBody').innerHTML = '<tr><td colspan="11" class="adsb-no-data">Không có dữ liệu.</td></tr>';
        byId('adsbTableInfo').textContent = number(rows.length) + ' bản ghi';
        byId('adsbPageInfo').textContent = 'Trang ' + pageIndex + '/' + pages;
        byId('adsbPrev').disabled = pageIndex <= 1; byId('adsbNext').disabled = pageIndex >= pages;
    }
    function loadData() {
        // var button = byId('adsbApply'); button.disabled = true; hideError();
        // return post(page.dataset.endpoint, payload(true)).then(function (data) {
        // rows = data.rows || []; currentTrend = data.trend || []; pageIndex = 1; renderKpis(data); renderChart(currentTrend); renderTable();

        var button = byId('adsbApply'); button.disabled = true; hideError();
        return post(page.dataset.endpoint, payload(true)).then(function (data) {
            data = normalizeData(data);
            rows = data.rows || []; currentTrend = data.trend || []; pageIndex = 1; renderKpis(data); renderChart(currentTrend); renderTotalChart(currentTrend); renderTable();
        }).catch(showError).then(function () { button.disabled = false; });
    }
    function apply() { loadOperators(true).then(loadData).catch(showError); }

    byId('adsbApply').addEventListener('click', apply);
    byId('adsbEndDay').addEventListener('click', openEndDay);
    byId('adsbEndDayClose').addEventListener('click', closeEndDay);
    byId('adsbEndDayModal').addEventListener('click', function (event) { if (event.target === this) closeEndDay(); });
    document.addEventListener('keydown', function (event) { if (event.key === 'Escape') closeEndDay(); });
    byId('adsbPermType').addEventListener('change', function () { loadOperators(false).catch(showError); });
    byId('adsbPageSize').addEventListener('change', function () { pageIndex = 1; renderTable(); });
    byId('adsbPrev').addEventListener('click', function () { if (pageIndex > 1) { pageIndex--; renderTable(); } });
    byId('adsbNext').addEventListener('click', function () { pageIndex++; renderTable(); });
    byId('adsbExportPdf').addEventListener('click', exportPdf);
    byId('adsbExportExcel').addEventListener('click', function () {
        if (!rows.length) { alert('Không có dữ liệu để xuất Excel.'); return; }
        window.ReportControls.exportExcel({ fileName: exportFileName(), rows: rows, columns: exportColumns() });
    });
    byId('adsbExportCsv').addEventListener('click', function () {
        if (!rows.length) { alert('Không có dữ liệu để xuất CSV.'); return; }
        window.ReportControls.exportCsv({ fileName: exportFileName(), rows: rows, columns: exportColumns() });
    });
    window.addEventListener('resize', function () { clearTimeout(window.__adsbResize); window.__adsbResize = setTimeout(function () { renderChart(currentTrend); renderTotalChart(currentTrend); }, 180); });

    setDefaultDates();
    loadOperators(false).then(loadData).catch(showError);
}());
