(function ($) {
    'use strict';

    var rows = [];
    var activeFilter = 'ALL';

    function value(id) { return $.trim($('#' + id).val() || ''); }
    function upper(text) { return $.trim(text || '').toUpperCase(); }
    function html(text) { return $('<div/>').text(text == null ? '' : text).html(); }
    function setLoading(show) { $('#impv2Loading').prop('hidden', !show); }
    function message(text, type) {
        $('#impv2Message').text(text || '').attr('class', 'impv2__message ' + (type ? 'is-' + type : '')).prop('hidden', !text);
    }
    function today() {
        var d = new Date(), m = String(d.getMonth() + 1), day = String(d.getDate());
        return d.getFullYear() + '-' + (m.length < 2 ? '0' + m : m) + '-' + (day.length < 2 ? '0' + day : day);
    }
    function normalizeDate(text) {
        var months = { JAN: '01', FEB: '02', MAR: '03', APR: '04', MAY: '05', JUN: '06', JUL: '07', AUG: '08', SEP: '09', OCT: '10', NOV: '11', DEC: '12' };
        var s = upper(text).replace(/[.\/]/g, '-'), m, year;
        if ((m = s.match(/^(\d{2})-(\d{2})-(\d{4})$/))) return m[3] + '-' + m[2] + '-' + m[1];
        if ((m = s.match(/^(\d{4})-(\d{2})-(\d{2})$/))) return s;
        if ((m = s.match(/^(\d{2})-([A-Z]{3})-(\d{2}|\d{4})$/)) && months[m[2]]) {
            year = m[3].length === 2 ? (+m[3] >= 70 ? '19' : '20') + m[3] : m[3];
            return year + '-' + months[m[2]] + '-' + m[1];
        }
        return '';
    }
    function normalizeTime(text) {
        var s = upper(text).replace(/:/g, ''), plus = /\+$/.test(s);
        s = s.replace(/\+/g, '');
        if (!/^\d{3,4}$/.test(s)) return '';
        while (s.length < 4) s = '0' + s;
        if (+s.substr(0, 2) > 23 || +s.substr(2, 2) > 59) return '';
        return s + (plus ? '+' : '');
    }
    function normalizeDaily(text) {
        var s = upper(text).replace('DAILY', '1234567').replace(/[^1-7]/g, '');
        return s.split('').filter(function (v, i, a) { return a.indexOf(v) === i; }).sort().join('');
    }
    function tokenize(line) { return $.trim(line).split(/[\t ]+/).filter(Boolean); }
    function detectedFormat(lines) {
        var sample = lines.slice(0, 10).join('\n').toUpperCase();
        if (/\bĐƯỜNG BAY\b|\bDUONG BAY\b/.test(sample) || /\bVN\d+\b/.test(sample)) return 'HVN';
        return 'STANDARD';
    }
    function routeFor(from, to) {
        var routeText = value('impDefaultVia'), result = routeText;
        routeText.split(/\r?\n/).some(function (line) {
            var parts = line.split(/[:=]/), key = upper(parts[0]).replace(/\s/g, '');
            if (parts.length > 1 && (key === upper(from + '-' + to) || key === upper(from + to))) {
                result = $.trim(parts.slice(1).join(':'));
                return true;
            }
            return false;
        });
        return result.indexOf('\n') >= 0 ? '' : result;
    }
    function makeRow(lineNo, source, d, format) {
        var row = { sourceLine: lineNo, sourceText: source, selected: true, status: 'VALID', errors: [], warnings: [] };
        if (format === 'ALL_OPER') {
            row.callsign = d[0]; row.fromDate = d[1]; row.toDate = d[2]; row.daily = d[3];
            row.craft = value('impDefaultCraft'); row.fromAirp = d[4]; row.etd = d[5]; row.toAirp = d[6]; row.eta = d[7];
            row.remark = d.length > 8 ? d[8] : '';
            row.via = d.length > 9 ? d[9] : '';
        } else if (format === 'HVN') {
            row.callsign = d[0]; row.fromDate = d[1]; row.toDate = d[2]; row.daily = d[3];
            row.craft = d[4]; row.fromAirp = d[5]; row.etd = d[6]; row.toAirp = d[7]; row.eta = d[8];
            row.via = d.length > 9 ? d[9] : '';
        } else {
            row.callsign = d[0]; row.fromDate = d[1]; row.toDate = d[2]; row.daily = d[3];
            row.craft = d[4]; row.fromAirp = d[5]; row.etd = d[6]; row.toAirp = d[7]; row.eta = d[8];
            row.via = d.length > 9 ? d[9] : '';
        }
        row.callsign = upper(row.callsign);
        row.fromDate = normalizeDate(row.fromDate);
        row.toDate = normalizeDate(row.toDate);
        row.daily = normalizeDaily(row.daily);
        row.craft = upper(row.craft || value('impDefaultCraft'));
        row.fromAirp = upper(row.fromAirp);
        row.toAirp = upper(row.toAirp);
        row.etd = normalizeTime(row.etd);
        row.eta = normalizeTime(row.eta);
        row.via = upper(row.via || routeFor(row.fromAirp, row.toAirp));
        row.remark = upper(row.remark || value('impDefaultRemark'));
        validate(row);
        return row;
    }
    function validate(row) {
        if (!/^[A-Z0-9]{2,10}$/.test(row.callsign)) row.errors.push('Callsign không hợp lệ');
        if (!row.fromDate) row.errors.push('Từ ngày không hợp lệ');
        if (!row.toDate) row.errors.push('Đến ngày không hợp lệ');
        if (row.fromDate && row.toDate && row.fromDate > row.toDate) row.errors.push('Từ ngày lớn hơn Đến ngày');
        if (!row.daily) row.errors.push('Daily không hợp lệ');
        if (!/^[A-Z]{4}$/.test(row.fromAirp)) row.errors.push('Sân bay đi không hợp lệ');
        if (!/^[A-Z]{4}$/.test(row.toAirp)) row.errors.push('Sân bay đến không hợp lệ');
        if (!row.etd) row.errors.push('ETD không hợp lệ');
        if (!row.eta) row.errors.push('ETA không hợp lệ');
        if (!row.craft) row.warnings.push('Thiếu loại tàu bay');
        if (!row.via) row.warnings.push('Thiếu VIA');
        row.status = row.errors.length ? 'ERROR' : (row.warnings.length ? 'WARNING' : 'VALID');
        if (row.status === 'ERROR') row.selected = false;
    }
    function parse() {
        var source = $('#impSource').val() || '', sourceLines = source.split(/\r?\n/), nonEmpty = sourceLines.filter(function (x) { return $.trim(x); });
        var format = value('impFormat');
        if (format === 'AUTO') format = detectedFormat(nonEmpty);
        rows = [];
        sourceLines.forEach(function (line, index) {
            var clean = $.trim(line), d;
            if (!clean || /^#/.test(clean) || /ĐƯỜNG BAY|DUONG BAY/i.test(clean)) return;
            d = tokenize(clean);
            if (d.length < 9) {
                rows.push({ sourceLine: index + 1, sourceText: clean, selected: false, status: 'ERROR', callsign: d[0] || '', errors: ['Thiếu cột: yêu cầu tối thiểu 9 trường'], warnings: [] });
                return;
            }
            rows.push(makeRow(index + 1, clean, d, format));
        });
        markDuplicates();
        render();
        $('#previewPanel').prop('hidden', false);
        $('[data-step-indicator]').removeClass('is-active').filter('[data-step-indicator="2"]').addClass('is-active');
        $('#cancelConfirmBox').prop('hidden', value('impAction') !== 'HuyChuyen');
        message('Đã nhận diện format ' + format + '. Hãy kiểm tra các dòng cảnh báo/lỗi trước khi import.', 'success');
    }
    function markDuplicates() {
        var seen = {};
        rows.forEach(function (row) {
            if (row.status === 'ERROR') return;
            var key = [row.callsign, row.fromDate, row.toDate, row.daily, row.fromAirp, row.toAirp, row.etd, row.eta].join('|');
            if (seen[key]) { row.warnings.push('Trùng dữ liệu với dòng ' + seen[key]); row.status = 'WARNING'; }
            else seen[key] = row.sourceLine;
        });
    }
    function render() {
        var query = upper(value('previewSearch')), body = '';
        rows.forEach(function (row, index) {
            var search = upper(JSON.stringify(row));
            if (activeFilter !== 'ALL' && row.status !== activeFilter) return;
            if (query && search.indexOf(query) < 0) return;
            var notes = (row.errors || []).concat(row.warnings || []).join('; ') || row.remark || '';
            body += '<tr class="is-' + row.status.toLowerCase() + '" data-row-index="' + index + '">'
                + '<td><input class="preview-row-check" type="checkbox" ' + (row.selected ? 'checked' : '') + (row.status === 'ERROR' ? ' disabled' : '') + '></td>'
                + '<td>' + row.sourceLine + '</td><td><span class="impv2__status impv2__status--' + row.status.toLowerCase() + '">' + row.status + '</span></td>'
                + '<td>' + html(row.callsign) + '</td><td>' + html(row.fromDate) + '</td><td>' + html(row.toDate) + '</td><td>' + html(row.daily) + '</td>'
                + '<td>' + html(row.craft) + '</td><td>' + html(row.fromAirp) + '</td><td>' + html(row.toAirp) + '</td><td>' + html(row.etd) + '</td><td>' + html(row.eta) + '</td>'
                + '<td>' + html(row.via) + '</td><td>' + html(notes) + '</td></tr>';
        });
        $('#previewTable tbody').html(body);
        $('#sumTotal').text(rows.length);
        $('#sumValid').text(rows.filter(function (x) { return x.status === 'VALID'; }).length);
        $('#sumWarning').text(rows.filter(function (x) { return x.status === 'WARNING'; }).length);
        $('#sumError').text(rows.filter(function (x) { return x.status === 'ERROR'; }).length);
    }
    function requestPayload() {
        return {
            action: value('impAction'), format: value('impFormat'), oper: value('impOper'), permNbr: value('impPermNbr'),
            permDate: value('impPermDate'), author: value('ddlAuthorV2'), version: value('impVersion'), season: value('impSeason'),
            purpose: value('ddlPurposeV2'), flightType: value('impFlightType'), registration: value('impRegistration'),
            rows: rows.filter(function (x) { return x.selected && x.status !== 'ERROR'; })
        };
    }
    function importSelected() {
        var payload = requestPayload();
        if (!/^[A-Z0-9]{1,8}$/i.test(payload.permNbr)) return message('Number bắt buộc, tối đa 8 ký tự chữ/số.', 'error');
        if (!payload.permDate) return message('Ngày cấp phép bắt buộc.', 'error');
        if (!payload.rows.length) return message('Chưa chọn dòng hợp lệ để import.', 'error');
        if (payload.action === 'HuyChuyen' && upper(value('cancelConfirmText')) !== 'HUY CHUYEN') return message('Phải nhập đúng HUY CHUYEN trước khi gửi dữ liệu hủy.', 'error');
        if (!window.confirm('Xác nhận import ' + payload.rows.length + ' dòng ' + payload.action + '?')) return;
        setLoading(true); message('');
        $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/ImportRows', contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ request: payload })
        }).done(function (response) {
            var result = response.d || {};
            $('#resultPanel').prop('hidden', false);
            $('#importResult').text(JSON.stringify(result, null, 2));
            $('[data-step-indicator]').removeClass('is-active').filter('[data-step-indicator="3"]').addClass('is-active');
            message(result.Success ? 'Import hoàn tất. Hãy kiểm tra kết quả chi tiết.' : 'Import có lỗi, chưa thực hiện lệnh HỦY CHUYẾN tự động.', result.Success ? 'success' : 'error');
        }).fail(function (xhr) {
            message('Không thể import: ' + (xhr.responseText || xhr.statusText), 'error');
        }).always(function () { setLoading(false); });
    }

    $(function () {
        $('#impPermDate').val(today());
        $('#btnAnalyze').on('click', parse);
        $('#btnImportSelected').on('click', importSelected);
        $('#btnClearSource').on('click', function () { $('#impSource').val('').focus(); rows = []; $('#previewPanel,#resultPanel').prop('hidden', true); });
        $('#impFile').on('change', function () { var file = this.files && this.files[0], reader; if (!file) return; reader = new FileReader(); reader.onload = function (e) { $('#impSource').val(e.target.result); }; reader.readAsText(file, 'UTF-8'); });
        $('[data-status-filter]').on('click', function () { activeFilter = $(this).data('status-filter'); $('[data-status-filter]').removeClass('is-active'); $(this).addClass('is-active'); render(); });
        $('#previewSearch').on('input', render);
        $('#previewTable').on('change', '.preview-row-check', function () { rows[+$(this).closest('tr').data('row-index')].selected = this.checked; });
        $('#previewCheckAll').on('change', function () { var checked = this.checked; rows.forEach(function (x) { if (x.status !== 'ERROR') x.selected = checked; }); render(); });
        $('#impAction').on('change', function () { $('#cancelConfirmBox').prop('hidden', this.value !== 'HuyChuyen'); });
    });
}(jQuery));
