(function () {
    'use strict';

    var page = document.querySelector('.airport-page');
    if (!page) return;

    var endpoint = window.reportApiBase + 'api/ChartReportAirport/GetData';
    var flights = [];
    var defaultAirports = ['VVBM', 'VVCA', 'VVCI', 'VVCM', 'VVCR', 'VVCS', 'VVCT', 'VVDB', 'VVDH', 'VVDL', 'VVDN', 'VVNB', 'VVPC', 'VVPQ', 'VVRG', 'VVTH', 'VVTS', 'VVTX', 'VVVD', 'VVVH'];
    var statuses = [
        ['total', 'Tổng chuyến bay', '#244b74'],
        ['finished', 'Finished', '#20b486'],
        ['cancel', 'Cancel', '#ef5b5b'],
        ['delay', 'Delay', '#f5a623'],
        ['wait', 'Wait', '#4d8df7']
    ];

    function byId(id) { return document.getElementById(id); }
    function escapeHtml(value) {
        var node = document.createElement('div');
        node.appendChild(document.createTextNode(value == null ? '' : String(value)));
        return node.innerHTML;
    }
    function dateKey(value) {
        return String(value || '').substring(0, 10);
    }
    function todayKey() {
        var date = new Date();
        var month = String(date.getMonth() + 1).padStart(2, '0');
        var day = String(date.getDate()).padStart(2, '0');
        return date.getFullYear() + '-' + month + '-' + day;
    }
    function normalizedStatus(value) {
        var state = String(value || '').toUpperCase();
        if (state.indexOf('DELAY') === 0) return 'delay';
        if (state === 'FINISHED') return 'finished';
        if (state === 'CANCEL') return 'cancel';
        if (state === 'WAIT') return 'wait';
        return '';
    }
    function airportCodes() {
        var found = {};
        defaultAirports.forEach(function (code) { found[code] = true; });
        flights.forEach(function (flight) {
            [flight.fromAirp, flight.toAirp].forEach(function (code) {
                code = String(code || '').trim().toUpperCase();
                if (code) found[code] = true;
            });
        });
        return Object.keys(found).sort();
    }
    function setAirportOptions() {
        var codes = airportCodes();
        var html = codes.map(function (code) {
            return '<option value="' + escapeHtml(code) + '">' + escapeHtml(code) + '</option>';
        }).join('');
        for (var index = 1; index <= 5; index++) {
            byId('airport' + index).innerHTML = html;
            if (codes.length) byId('airport' + index).selectedIndex = Math.min(index - 1, codes.length - 1);
        }
        if (window.ReportControls) window.ReportControls.enhanceAll(page);
    }
    function updateAirportMode() {
        var count = Math.min(5, Math.max(1, parseInt(byId('airportCount').value, 10) || 1));
        Array.prototype.forEach.call(page.querySelectorAll('.airport-choice'), function (choice) {
            choice.style.display = parseInt(choice.getAttribute('data-airport-index'), 10) <= count ? '' : 'none';
        });
        byId('time2Label').textContent = 'Thời gian so sánh';
    }
    function selectedDates() {
        var dates = [byId('time1').value, byId('time2').value].filter(Boolean).sort();
        return { from: dates[0] || todayKey(), to: dates[dates.length - 1] || todayKey() };
    }
    function postData(range) {
        return fetch(endpoint, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({ fromDate: range.from, toDate: range.to })
        }).then(function (response) {
            if (!response.ok) throw new Error('Máy chủ trả về HTTP ' + response.status + '.');
            return response.json();
        }).then(function (response) {
            if (response.Code && response.Code !== '00') throw new Error(response.Message || 'Không thể tải dữ liệu.');
            var data = Object.prototype.hasOwnProperty.call(response, 'ListValue') ? response.ListValue : response.d;
            data = typeof data === 'string' ? JSON.parse(data) : data;
            if (data && data.Flights) {
                data.flights = data.Flights.map(function (item) { return {
                    flightDate: item.FlightDate, callsign: item.Callsign, oper: item.Oper, registration: item.Registration,
                    permType: item.PermType, fromAirp: item.FromAirp, toAirp: item.ToAirp, atdDay: item.AtdDay,
                    ataDay: item.AtaDay, eobtDay: item.EobtDay, status: item.Status
                }; });
            }
            return data;
        });
    }
    function values(airport, date) {
        var result = { total: 0, finished: 0, cancel: 0, delay: 0, wait: 0 };
        flights.forEach(function (flight) {
            var from = String(flight.fromAirp || '').toUpperCase();
            var to = String(flight.toAirp || '').toUpperCase();
            if (dateKey(flight.flightDate) !== date || (from !== airport && to !== airport)) return;
            result.total++;
            var state = normalizedStatus(flight.status);
            if (state) result[state]++;
        });
        return result;
    }
    function renderEmpty(message, isError) {
        byId('airportResult').innerHTML = '<div class="airport-empty' + (isError ? ' airport-error' : '') + '">' + escapeHtml(message) + '</div>';
    }
    function render() {
        var count = Math.min(5, Math.max(1, parseInt(byId('airportCount').value, 10) || 1));
        var date1 = byId('time1').value;
        var date2 = byId('time2').value;
        var selected = [];
        for (var selectedIndex = 1; selectedIndex <= count; selectedIndex++) selected.push(byId('airport' + selectedIndex).value);
        if (!date1 || !date2 || selected.some(function (airport) { return !airport; })) {
            renderEmpty('Vui lòng nhập đủ sân bay và thời gian để so sánh.');
            return;
        }

        var colors = ['#337ab7', '#f5a623', '#20b486', '#8a6ee8', '#ef5b5b'];
        var datasets = selected.map(function (airport, index) {
            var date = index === 0 ? date1 : date2;
            return { airport: airport, date: date, value: values(airport, date), color: colors[index] };
        });
        var max = 1;
        statuses.forEach(function (status) {
            datasets.forEach(function (dataset) { max = Math.max(max, dataset.value[status[0]]); });
        });

        var html = '<div class="airport-chart-layout"><div><h3 class="airport-chart-title">Số lượng chuyến bay</h3>' +
            '<p class="airport-chart-subtitle">' + datasets.map(function (dataset) { return escapeHtml(dataset.airport) + ' · ' + escapeHtml(dataset.date); }).join(' &nbsp;|&nbsp; ') +
            '</p><div class="bar-area"><div class="airport-y-axis">' +
            '<span>' + max + '</span><span>' + Math.round(max * .75) + '</span><span>' + Math.round(max * .5) + '</span><span>' + Math.round(max * .25) + '</span><span>0</span>' +
            '</div><div class="bar-chart">';
        statuses.forEach(function (status) {
            html += '<div class="bar-group">' + datasets.map(function (dataset) {
                return '<div class="bar" style="background:' + dataset.color + ';height:' + Math.max(3, dataset.value[status[0]] / max * 100) + '%"><span class="bar-value">' + dataset.value[status[0]] + '</span></div>';
            }).join('') + '</div>';
        });
        html += '</div><div class="bar-labels">' + statuses.map(function (status) { return '<span>' + status[1] + '</span>'; }).join('') +
            '</div></div></div><div class="airport-legend"><h3>Chú thích</h3>' +
            datasets.map(function (dataset) { return '<div class="legend-item"><i class="legend-dot" style="background:' + dataset.color + '"></i>' + escapeHtml(dataset.airport) + '<strong>' + escapeHtml(dataset.date) + '</strong></div>'; }).join('') + '<hr>';
        statuses.forEach(function (status) {
            html += '<div class="legend-item">' + status[1] + '<strong>' + datasets.map(function (dataset) { return dataset.value[status[0]]; }).join(' / ') + '</strong></div>';
        });
        html += '</div></div>';
        byId('airportResult').innerHTML = html;
    }
    function loadData() {
        var range = selectedDates();
        renderEmpty('Đang tải dữ liệu từ T_FINISHED_FLIGHTS...');
        byId('compareButton').disabled = true;
        postData(range).then(function (data) {
            flights = data && data.flights ? data.flights : [];
            setAirportOptions();
            updateAirportMode();
            render();
        }).catch(function (error) {
            flights = [];
            renderEmpty(error.message || 'Không thể tải dữ liệu từ T_FINISHED_FLIGHTS.', true);
        }).then(function () {
            byId('compareButton').disabled = false;
        });
    }

    var today = todayKey();
    byId('time1').value = byId('time1').value || today;
    byId('time2').value = byId('time2').value || today;
    byId('airportCount').addEventListener('change', updateAirportMode);
    byId('compareButton').addEventListener('click', loadData);
    updateAirportMode();
    loadData();
}());
