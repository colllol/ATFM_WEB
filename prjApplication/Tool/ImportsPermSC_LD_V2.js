(function ($) {
    'use strict';

    var rows = [];
    var activeFilter = 'ALL';
    var formatGuides = {
        STANDARD: {
            name: 'CHUẨN ATFM',
            description: 'Dùng khi mỗi dòng đã có đầy đủ loại tàu bay và lịch bay.',
            columns: ['CALLSIGN', 'FROM DATE', 'TO DATE', 'DAILY', 'CRAFT', 'FROM', 'ETD', 'TO', 'ETA', 'VIA (tùy chọn)'],
            example: 'HVN123 12-AUG-26 30-SEP-26 1234567 A321 VVNB 0830 VVTS 1035 R474',
            rules: ['Ngày chấp nhận DD-MMM-YY, DD-MM-YYYY hoặc DD/MM/YYYY.', 'DAILY dùng 1234567; thứ Hai là 1, Chủ nhật là 7.', 'ETD/ETA dùng HHmm hoặc HH:mm; ETA qua ngày có thể thêm dấu +.']
        },
        HVN: {
            name: 'HVN LEGACY',
            description: 'Dùng cho dữ liệu lịch bay HVN theo thứ tự cột của chức năng cũ.',
            columns: ['CALLSIGN', 'FROM DATE', 'TO DATE', 'DAILY', 'CRAFT', 'FROM', 'ETD', 'TO', 'ETA', 'VIA (tùy chọn)'],
            example: 'VN714 12-AUG-26 30-SEP-26 1234567 321 VVNB 0730 VVDN 0855 W1',
            rules: ['Craft 319/320/321/787 được chuẩn hóa thành A319/A320/A321/B787.', 'Nếu VIA bỏ trống, hệ thống lấy VIA/Route mặc định.', 'Một dòng tiêu đề ĐƯỜNG BAY sẽ được bỏ qua.']
        },
        ALL_OPER: {
            name: 'TAB/SPACE CHUNG',
            description: 'Dùng cho nhiều hãng; Craft lấy từ ô Craft mặc định thay vì lấy trong từng dòng.',
            columns: ['CALLSIGN', 'FROM DATE', 'TO DATE', 'DAILY', 'FROM', 'ETD', 'TO', 'ETA', 'REMARK (tùy chọn)', 'VIA (tùy chọn)'],
            example: 'VJC101 12-AUG-26 30-SEP-26 1234567 VVTS 0600 VVNB 0810 NORMAL R474',
            rules: ['Phải nhập Craft mặc định nếu dữ liệu nguồn không có Craft.', 'Remark/VIA không được chứa khoảng trắng nếu nhập trực tiếp trong dòng.', 'Có thể khai báo VIA/Route mặc định ở khu vực dữ liệu nguồn.']
        }
    };

    function value(id) { return $.trim($('#' + id).val() || ''); }
    function upper(text) { return $.trim(text || '').toUpperCase(); }
    function html(text) { return $('<div/>').text(text == null ? '' : text).html(); }
    function setLoading(show) { $('#impv2Loading').prop('hidden', !show); }
    function message(text, type) {
        $('#impv2Message').text(text || '').attr('class', 'impv2__message ' + (type ? 'is-' + type : '')).prop('hidden', !text);
    }
    function currentFormatGuide() {
        var selected = value('impFormat');
        return selected === 'AUTO' ? {
            name: 'TỰ NHẬN DIỆN',
            description: 'Hệ thống nhận diện HVN khi thấy dòng ĐƯỜNG BAY hoặc callsign VN; trường hợp còn lại dùng CHUẨN ATFM.',
            columns: formatGuides.STANDARD.columns,
            example: formatGuides.STANDARD.example,
            rules: ['Nếu kết quả preview lệch cột, hãy chọn trực tiếp CHUẨN ATFM, HVN LEGACY hoặc TAB/SPACE CHUNG.', 'Luôn kiểm tra Preview trước khi Import.', 'Dòng lỗi sẽ tự bỏ chọn và không được gửi lên server.']
        } : formatGuides[selected];
    }
    function showFormatGuide() {
        var guide = currentFormatGuide(), columns = '';
        guide.columns.forEach(function (column, index) {
            columns += '<span><b>' + (index + 1) + '</b>' + html(column) + '</span>';
        });
        $('#formatGuideName').text(guide.name);
        $('#formatGuideDescription').text(guide.description);
        $('#formatGuideColumns').html(columns);
        $('#formatGuideExample').text(guide.example);
        $('#formatGuideRules').html(guide.rules.map(function (rule) { return '<li>' + html(rule) + '</li>'; }).join(''));
        $('#formatGuide').prop('hidden', false).attr('aria-hidden', 'false');
        $('#btnCloseFormat').focus();
    }
    function hideFormatGuide() {
        $('#formatGuide').prop('hidden', true).attr('aria-hidden', 'true');
        $('#btnViewFormat').focus();
    }
    function syncOperAvailability() {
        var autoOper = value('impFormat') === 'ALL_OPER';
        $('#impOper').prop('disabled', autoOper).val(autoOper ? '' : value('impOper'));
        $('#impOperHint')
            .text(autoOper ? 'Tự xác định từ Callsign bằng GetOper.' : 'Bắt buộc với format đang chọn.')
            .toggleClass('is-auto', autoOper);
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
        if (payload.format !== 'ALL_OPER' && !payload.oper) return message('Hãy chọn Hãng khai thác cho format ' + payload.format + '.', 'error');
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
        setLoading(false);
        syncOperAvailability();
        $('#impPermDate').val(today());
        $('#btnAnalyze').on('click', parse);
        $('#btnViewFormat').on('click', showFormatGuide);
        $('#impFormat').on('change', syncOperAvailability);
        $('#btnCloseFormat,#btnCloseFormatBottom').on('click', hideFormatGuide);
        $('#formatGuide').on('click', function (event) { if (event.target === this) hideFormatGuide(); });
        $(document).on('keydown', function (event) { if (event.key === 'Escape' && !$('#formatGuide').prop('hidden')) hideFormatGuide(); });
        $('#btnCopyFormat').on('click', function () {
            var sample = $('#formatGuideExample').text();
            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(sample).then(function () { message('Đã sao chép dòng format mẫu.', 'success'); });
            } else {
                window.prompt('Sao chép dòng mẫu:', sample);
            }
        });
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
