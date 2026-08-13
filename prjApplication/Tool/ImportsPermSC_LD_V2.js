(function ($) {
    'use strict';

    var rows = [];
    var activeFilter = 'ALL';
    var pendingStagingIds = [];
    var pendingRows = [];
    var cancellationRequestRunning = false;
    var formatGuides = {
        WITH_CRAFT: {
            name: 'MẪU HỦY CÓ LOẠI TÀU BAY',
            description: 'Bảng Schedules có cột Aircraft Type.',
            columns: ['CALLSIGN', 'FROM DATE', 'TO DATE', 'DAILY', 'CRAFT', 'FROM', 'ETD', 'TO', 'ETA', 'VIA (tùy chọn)'],
            example: 'HVN123 12-AUG-26 30-SEP-26 1234567 A321 VVNB 0830 VVTS 1035 R474',
            rules: ['Có thể dán cả dòng tiêu đề từ Word/Excel.', 'Aircraft có thể để trống đối với chuyến hủy.', 'Ngày, DAILY và giờ sẽ được chuẩn hóa tự động.']
        },
        WITHOUT_CRAFT: {
            name: 'MẪU HỦY KHÔNG CÓ LOẠI TÀU BAY',
            description: 'Bảng Schedules kết thúc ở cột ETA, không có Aircraft Type.',
            columns: ['CALLSIGN', 'FROM DATE', 'TO DATE', 'DAILY', 'FROM', 'ETD', 'TO', 'ETA'],
            example: 'MMA-711 29 Mar 26 24 Oct 26 Daily VYYY 0210 ZGGG 0515',
            rules: ['Callsign có dấu gạch sẽ tự bỏ dấu gạch.', 'Sân bay chấp nhận IATA 3 ký tự hoặc ICAO 4 ký tự.', 'Craft không bắt buộc đối với đối chiếu hủy chuyến.']
        },
        NORMALIZED: {
            name: 'DỮ LIỆU ĐÃ CHUẨN HÓA ATFM',
            description: 'Dữ liệu không có tiêu đề và đã đúng thứ tự cột ATFM.',
            columns: ['CALLSIGN', 'FROM DATE', 'TO DATE', 'DAILY', 'CRAFT', 'FROM', 'ETD', 'TO', 'ETA', 'VIA (tùy chọn)'],
            example: 'HVN123 12-AUG-26 30-SEP-26 1234567 A321 VVNB 0830 VVTS 1035 R474',
            rules: ['Mỗi chuyến bay một dòng.', 'Các cột phân cách bằng Tab hoặc khoảng trắng.', 'Nên dùng Tự nhận diện nếu dán trực tiếp từ Word/Excel.']
        }
    };

    function value(id) { return $.trim($('#' + id).val() || ''); }
    function upper(text) { return $.trim(text || '').toUpperCase(); }
    function html(text) { return $('<div/>').text(text == null ? '' : text).html(); }
    function setLoading(show) { $('#impv2Loading').prop('hidden', !show); }
    function setCancellationProcessing(processing) {
        cancellationRequestRunning = processing;
        $('#btnApplyCancellation,#btnDeleteCancellation,#btnDeletePending,#btnReviewPending').prop('disabled', processing);
        $('#impv2Loading span').text(processing
            ? 'Đang xác nhận hủy chuyến, vui lòng không đóng trang...'
            : 'Đang xử lý dữ liệu...');
        setLoading(processing);
    }
    function message(text, type) {
        $('#impv2Message').text(text || '').attr('class', 'impv2__message ' + (type ? 'is-' + type : '')).prop('hidden', !text);
    }
    function currentFormatGuide() {
        var selected = value('impFormat');
        return selected === 'AUTO' ? {
            name: 'TỰ NHẬN DIỆN',
            description: 'Hệ thống tìm dòng tiêu đề, ánh xạ cột theo tên và tự nhận diện bảng có hoặc không có Aircraft Type.',
            columns: formatGuides.WITH_CRAFT.columns,
            example: formatGuides.WITH_CRAFT.example,
            rules: ['Khuyến nghị sao chép cả dòng tiêu đề và các dòng chuyến bay.', 'Có thể dán từ Word hoặc Excel.', 'Luôn kiểm tra Preview trước khi đưa vào danh sách hủy.']
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
    function today() {
        var d = new Date(), m = String(d.getMonth() + 1), day = String(d.getDate());
        return d.getFullYear() + '-' + (m.length < 2 ? '0' + m : m) + '-' + (day.length < 2 ? '0' + day : day);
    }
    function normalizeDate(text) {
        var months = { JAN: '01', FEB: '02', MAR: '03', APR: '04', MAY: '05', JUN: '06', JUL: '07', AUG: '08', SEP: '09', OCT: '10', NOV: '11', DEC: '12' };
        var s = upper(text).replace(/[.\/]/g, '-').replace(/\s+/g, '-').replace(/-+/g, '-'), m, year;
        if ((m = s.match(/^(\d{2})-(\d{2})-(\d{4})$/))) return m[3] + '-' + m[2] + '-' + m[1];
        if ((m = s.match(/^(\d{2})-(\d{2})-(\d{2})$/))) return '20' + m[3] + '-' + m[2] + '-' + m[1];
        if ((m = s.match(/^(\d{4})-(\d{2})-(\d{2})$/))) return s;
        if ((m = s.match(/^(\d{1,2})-?([A-Z]{3})-?(\d{2}|\d{4})$/)) && months[m[2]]) {
            year = m[3].length === 2 ? (+m[3] >= 70 ? '19' : '20') + m[3] : m[3];
            return year + '-' + months[m[2]] + '-' + ('0' + m[1]).slice(-2);
        }
        return '';
    }
    function normalizeTime(text) {
        var s = upper(text).replace(/\s+/g, '').replace(/:/g, ''), plus = /\+(?:1)?$/.test(s);
        s = s.replace(/\+(?:1)?$/g, '');
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
    function tabCells(line) { return line.split('\t').map(function (x) { return $.trim(x); }); }
    function fold(text) {
        var s = upper(text);
        if (s.normalize) s = s.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
        return s.replace(/Đ/g, 'D').replace(/[^A-Z0-9]+/g, ' ').replace(/\s+/g, ' ').trim();
    }
    function headerKey(text) {
        var s = fold(text);
        if (/CALL ?SIGN|FLIGHT (NUMBER|NBR)/.test(s)) return 'callsign';
        if (/EFFECTIVE FROM|BEGIN DATE|FROM DATE/.test(s)) return 'fromDate';
        if (/EFFECTIVE TO|END DATE|TO DATE/.test(s)) return 'toDate';
        if (/DAY(?: S|S)? OF SERVICE|DAY(?: S|S)? OF OPERATION|DAILY|DAY DATE/.test(s)) return 'daily';
        if (/DEPARTURE (AIRPORT|AERODROME)|FROM AIRP/.test(s)) return 'fromAirp';
        if (/ARRIVAL (AIRPORT|AERODROME)|TO AIRP/.test(s)) return 'toAirp';
        if (/^ETD/.test(s)) return 'etd';
        if (/^ETA/.test(s)) return 'eta';
        if (/AIRCRAFT TYPE|CRAFT TYPE|^CRAFT$/.test(s)) return 'craft';
        if (/^VIA$|ROUTE/.test(s)) return 'via';
        if (/REMARK|NOTE/.test(s)) return 'remark';
        return '';
    }
    function headerMap(cells) {
        var map = {};
        cells.forEach(function (cell, index) { var key = headerKey(cell); if (key && map[key] == null) map[key] = index; });
        return map.callsign != null && map.fromDate != null && map.toDate != null && map.etd != null && map.eta != null ? map : null;
    }
    function isSectionEnd(line) {
        return /^(3\.|TYPE OF SERVICES|REASON OF CANCELLATION|REF TO PERMIT|APPLICANT|NOTE|RGDS)/.test(fold(line));
    }
    function formatName(format) {
        return { AUTO: 'TỰ NHẬN DIỆN', WITH_CRAFT: 'MẪU CÓ LOẠI TÀU BAY', WITHOUT_CRAFT: 'MẪU KHÔNG CÓ LOẠI TÀU BAY', NORMALIZED: 'DỮ LIỆU CHUẨN HÓA ATFM' }[format] || format;
    }
    function tableTextFromClipboard(event) {
        var clipboard = event.originalEvent && event.originalEvent.clipboardData;
        var rawHtml = clipboard ? clipboard.getData('text/html') : '';
        var container, tables, best = null, bestScore = -1;
        if (!rawHtml || !/<table[\s>]/i.test(rawHtml)) return '';
        container = document.createElement('div'); container.innerHTML = rawHtml;
        tables = container.querySelectorAll('table');
        Array.prototype.forEach.call(tables, function (table) {
            var text = fold(table.textContent || ''), score = 0;
            if (/FLIGHT (NUMBER|NBR)/.test(text)) score += 4;
            if (/EFFECTIVE FROM/.test(text)) score += 3;
            if (/ETD/.test(text) && /ETA/.test(text)) score += 2;
            score += table.rows ? Math.min(table.rows.length, 20) / 100 : 0;
            if (score > bestScore) { bestScore = score; best = table; }
        });
        if (!best || bestScore < 4) return '';
        return Array.prototype.map.call(best.rows, function (tr) {
            return Array.prototype.map.call(tr.cells, function (cell) {
                return (cell.innerText || cell.textContent || '').replace(/\s+/g, ' ').trim();
            }).join('\t');
        }).filter(function (line) { return $.trim(line); }).join('\n');
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
        if (format === 'WITHOUT_CRAFT') {
            row.callsign = d[0]; row.fromDate = d[1]; row.toDate = d[2]; row.daily = d[3];
            row.craft = value('impDefaultCraft'); row.fromAirp = d[4]; row.etd = d[5]; row.toAirp = d[6]; row.eta = d[7];
            row.remark = d.length > 8 ? d[8] : '';
            row.via = d.length > 9 ? d[9] : '';
        } else {
            row.callsign = d[0]; row.fromDate = d[1]; row.toDate = d[2]; row.daily = d[3];
            row.craft = d[4]; row.fromAirp = d[5]; row.etd = d[6]; row.toAirp = d[7]; row.eta = d[8];
            row.via = d.length > 9 ? d[9] : '';
        }
        row.callsign = upper(row.callsign).replace(/[^A-Z0-9]/g, '');
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
        if (!/^[A-Z]{3,4}$/.test(row.fromAirp)) row.errors.push('Sân bay đi không hợp lệ');
        if (!/^[A-Z]{3,4}$/.test(row.toAirp)) row.errors.push('Sân bay đến không hợp lệ');
        if (!row.etd) row.errors.push('ETD không hợp lệ');
        if (!row.eta) row.errors.push('ETA không hợp lệ');
        row.status = row.errors.length ? 'ERROR' : (row.warnings.length ? 'WARNING' : 'VALID');
        if (row.status === 'ERROR') row.selected = false;
    }
    function rowFromHeader(lineNo, source, cells, map) {
        function cell(key) { return map[key] == null ? '' : (cells[map[key]] || ''); }
        return makeNormalizedRow(lineNo, source, {
            callsign: cell('callsign'), fromDate: cell('fromDate'), toDate: cell('toDate'), daily: cell('daily'),
            craft: cell('craft'), fromAirp: cell('fromAirp'), etd: cell('etd'), toAirp: cell('toAirp'), eta: cell('eta'),
            via: cell('via'), remark: cell('remark')
        });
    }
    function makeNormalizedRow(lineNo, source, data) {
        var row = $.extend({ sourceLine: lineNo, sourceText: source, selected: true, status: 'VALID', errors: [], warnings: [] }, data);
        row.callsign = upper(row.callsign).replace(/[^A-Z0-9]/g, '');
        row.fromDate = normalizeDate(row.fromDate); row.toDate = normalizeDate(row.toDate);
        row.daily = normalizeDaily(row.daily); row.craft = upper(row.craft || value('impDefaultCraft'));
        row.fromAirp = upper(row.fromAirp); row.toAirp = upper(row.toAirp);
        row.etd = normalizeTime(row.etd); row.eta = normalizeTime(row.eta);
        row.via = upper(row.via || routeFor(row.fromAirp, row.toAirp));
        row.remark = upper(row.remark || value('impDefaultRemark'));
        validate(row); return row;
    }
    function parse() {
        var source = $('#impSource').val() || '', sourceLines = source.split(/\r?\n/), format = value('impFormat');
        var map = null, headerIndex = -1, detected = format;
        rows = [];
        sourceLines.some(function (line, index) {
            var candidate = headerMap(tabCells(line));
            if (candidate) { map = candidate; headerIndex = index; return true; }
            return false;
        });
        if (map) detected = map.craft == null ? 'WITHOUT_CRAFT' : 'WITH_CRAFT';
        if (format !== 'AUTO') detected = format;
        sourceLines.forEach(function (line, index) {
            var clean = $.trim(line), d;
            if (!clean || /^#/.test(clean) || index === headerIndex || (headerIndex >= 0 && index < headerIndex)) return;
            if (headerIndex >= 0 && isSectionEnd(clean)) return;
            if (map && headerIndex >= 0) {
                d = tabCells(line);
                if (!d[map.callsign]) return;
                rows.push(rowFromHeader(index + 1, clean, d, map));
                return;
            }
            d = tokenize(clean);
            if (detected === 'AUTO') detected = /^[A-Z]{3,4}$/i.test(d[4] || '') ? 'WITHOUT_CRAFT' : 'WITH_CRAFT';
            if (d.length < (detected === 'WITHOUT_CRAFT' ? 8 : 9)) {
                rows.push({ sourceLine: index + 1, sourceText: clean, selected: false, status: 'ERROR', callsign: d[0] || '', errors: ['Thiếu cột theo format đã chọn'], warnings: [] });
                return;
            }
            rows.push(makeRow(index + 1, clean, d, detected));
        });
        markDuplicates();
        render();
        $('#previewPanel').prop('hidden', false);
        $('[data-step-indicator]').removeClass('is-active').filter('[data-step-indicator="2"]').addClass('is-active');
        message('Đã nhận diện ' + formatName(detected) + ', đọc ' + rows.length + ' dòng. Hãy kiểm tra Preview trước khi tiếp tục.', 'success');
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
            action: 'HuyChuyen', format: value('impFormat'), permNbr: value('impPermNbr'),
            permDate: value('impPermDate'), author: value('ddlAuthorV2'), version: value('impVersion'), season: value('impSeason'),
            purpose: value('ddlPurposeV2'), flightType: value('impFlightType'), registration: value('impRegistration'),
            allowExistingPermit: false,
            rows: rows.filter(function (x) { return x.selected && x.status !== 'ERROR'; })
        };
    }
    function importSelected() {
        var payload = requestPayload();
        if (!/^[A-Z0-9]{1,8}$/i.test(payload.permNbr)) return message('Number bắt buộc, tối đa 8 ký tự chữ/số.', 'error');
        if (!payload.permDate) return message('Ngày cấp phép bắt buộc.', 'error');
        if (!payload.rows.length) return message('Chưa chọn dòng hợp lệ để import.', 'error');
        setLoading(true); message('');
        $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/CheckPermitNumber',
            contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ request: payload })
        }).done(function (response) {
            var check = response.d || {};
            if (!check.Success) {
                setLoading(false);
                $('#resultPanel').prop('hidden', false);
                showResult('Không thể kiểm tra số phép: ' + (check.Message || 'Lỗi không xác định.'), 'error');
                return;
            }
            if (check.Exists && !window.confirm(check.Message)) { setLoading(false); return; }
            payload.allowExistingPermit = !!check.Exists;
            if (!window.confirm('Xác nhận đưa ' + payload.rows.length + ' dòng vào danh sách HỦY CHUYẾN?')) { setLoading(false); return; }
            performImport(payload);
        }).fail(function (xhr) {
            setLoading(false);
            $('#resultPanel').prop('hidden', false);
            showResult('Không thể kiểm tra số phép: ' + (xhr.responseText || xhr.statusText), 'error');
        });
    }

    function performImport(payload) {
        setLoading(true); message('');
        $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/ImportRows', contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ request: payload })
        }).done(function (response) {
            var result = response.d || {};
            $('#resultPanel').prop('hidden', false);
            showResult(result.Message, result.Success ? 'success' : 'error', result.Errors);
            if (result.Success) {
                renderConfirmation(result);
                searchPending(true);
                $('#btnApplyCancellation').prop('disabled', false);
                $('#confirmPanel').prop('hidden', false);
                $('[data-step-indicator]').removeClass('is-active').filter('[data-step-indicator="3"]').addClass('is-active');
            } else {
                $('#confirmPanel').prop('hidden', true);
                $('[data-step-indicator]').removeClass('is-active').filter('[data-step-indicator="2"]').addClass('is-active');
            }
        }).fail(function (xhr) {
            $('#resultPanel').prop('hidden', false);
            showResult('Không thể kiểm tra dữ liệu: ' + (xhr.responseText || xhr.statusText), 'error');
        }).always(function () { setLoading(false); });
    }

    function showResult(text, type, errors) {
        var detail = (errors || []).join('\n');
        $('#importResult').removeClass('is-success is-error').addClass(type === 'success' ? 'is-success' : 'is-error')
            .text((text || '') + (detail ? '\n' + detail : ''));
    }

    function renderConfirmation(result) {
        var imported = '', cancelled = '';
        pendingStagingIds = (result.ImportedRows || []).map(function (x) {
            return +x.StagingId;
        }).filter(function (id, index, list) {
            return id > 0 && list.indexOf(id) === index;
        });
        (result.ImportedRows || []).forEach(function (x, i) {
            imported += '<tr><td>' + (i + 1) + '</td><td>' + html(x.Callsign) + '</td><td>' + html(x.FromDate)
                + '</td><td>' + html(x.ToDate) + '</td><td>' + html(x.Daily) + '</td><td>' + html(x.Craft)
                + '</td><td>' + html(x.FromAirp) + '</td><td>' + html(x.ToAirp) + '</td><td>' + html(x.Etd)
                + '</td><td>' + html(x.Eta) + '</td><td>' + html(x.Via) + '</td><td>' + html(x.Remark) + '</td></tr>';
        });
        (result.CancelledFlights || []).forEach(function (x, i) {
            cancelled += '<tr><td>' + (i + 1) + '</td><td>' + html(x.Callsign) + '</td><td>' + html(x.PermNbr)
                + '</td><td>' + html(x.FromDate) + '</td><td>' + html(x.ToDate) + '</td><td>' + html(x.FromAirp)
                + '</td><td>' + html(x.ToAirp) + '</td><td>' + html(x.Daily) + '</td><td>' + html(x.Etd)
                + '</td><td>' + html(x.Eta) + '</td><td>' + html(x.Oper) + '</td><td>' + html(x.PermType)
                + '</td><td>' + html(x.Remark) + '</td><td>' + html(x.Purpose) + '</td><td>' + html(x.CancelDaily)
                + '</td></tr>';
        });
        $('#importedTable tbody').html(imported);
        $('#cancelledTable tbody').html(cancelled);
    }

    function selectedPendingIds() {
        var ids = [];
        $('#pendingTable tbody .pending-row-check:checked').each(function () {
            var id = +$(this).closest('tr').attr('data-staging-id');
            if (id > 0 && ids.indexOf(id) < 0) ids.push(id);
        });
        return ids;
    }

    function renderPending() {
        var body = '';
        pendingRows.forEach(function (x) {
            var statusClass = upper(x.Status) === 'ERROR' ? 'is-error' : 'is-valid';
            body += '<tr class="' + statusClass + '" data-staging-id="' + (+x.StagingId) + '">'
                + '<td><input type="checkbox" class="pending-row-check"></td>'
                + '<td>' + html(x.StagingId) + '</td><td>' + html(x.PermNbr) + '</td>'
                + '<td>' + html(x.Callsign) + '</td><td>' + html(x.FromDate) + '</td>'
                + '<td>' + html(x.ToDate) + '</td><td>' + html(x.Daily) + '</td>'
                + '<td>' + html(x.FromAirp) + '</td><td>' + html(x.ToAirp) + '</td>'
                + '<td>' + html(x.Etd) + '</td><td>' + html(x.Eta) + '</td>'
                + '<td>' + html(x.Oper) + '</td><td>' + html(x.Status) + '</td>'
                + '<td>' + html(x.CreatedAt) + '</td><td>' + html(x.ErrorMessage) + '</td></tr>';
        });
        if (!body) body = '<tr><td colspan="15" class="impv2__empty">Khong co du lieu dang cho xu ly.</td></tr>';
        $('#pendingTable tbody').html(body);
        $('#pendingCount').text(pendingRows.length + ' dong');
        $('#pendingCheckAll').prop('checked', false);
    }

    function searchPending(silent) {
        if (!silent) setLoading(true);
        return $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/SearchPending',
            contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ request: {
                permNbr: value('pendingPermNbr'), callsign: value('pendingCallsign'),
                fromDate: value('pendingFromDate'), toDate: value('pendingToDate')
            } })
        }).done(function (response) {
            var result = response.d || {};
            if (!result.Success) {
                pendingRows = [];
                renderPending();
                if (!silent) showResult(result.Message || 'Khong the tai danh sach cho xu ly.', 'error');
                return;
            }
            pendingRows = result.Rows || [];
            renderPending();
        }).fail(function (xhr) {
            pendingRows = [];
            renderPending();
            if (!silent) showResult('Khong the tai danh sach cho xu ly: ' + (xhr.responseText || xhr.statusText), 'error');
        }).always(function () { if (!silent) setLoading(false); });
    }

    function reviewPending() {
        var ids = selectedPendingIds();
        if (!ids.length) return message('Hay chon it nhat mot dong dang cho xu ly.', 'error');
        setLoading(true);
        $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/ReviewPending',
            contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ stagingIds: ids })
        }).done(function (response) {
            var result = response.d || {};
            $('#resultPanel').prop('hidden', false);
            showResult(result.Message, result.Success ? 'success' : 'error', result.Errors);
            if (result.ImportedRows && result.ImportedRows.length) renderConfirmation(result);
            if (result.Success) {
                $('#confirmPanel').prop('hidden', false);
                $('[data-step-indicator]').removeClass('is-active').filter('[data-step-indicator="3"]').addClass('is-active');
                $('html,body').animate({ scrollTop: $('#confirmPanel').offset().top - 80 }, 200);
            } else {
                $('#confirmPanel').prop('hidden', true);
                searchPending(true);
            }
        }).fail(function (xhr) {
            showResult('Khong the kiem tra lai du lieu: ' + (xhr.responseText || xhr.statusText), 'error');
        }).always(function () { setLoading(false); });
    }

    function deleteByIds(ids) {
        if (!ids.length) return message('Khong co dong nao duoc chon de xoa.', 'error');
        if (!window.confirm('Xoa cac dong huy chuyen da chon khoi danh sach cho xu ly?')) return;
        setLoading(true);
        $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/DeleteCancellation',
            contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ stagingIds: ids })
        }).done(function (response) {
            var result = response.d || {};
            showResult(result.Message, result.Success ? 'success' : 'error');
            if (result.Success) {
                pendingStagingIds = [];
                $('#confirmPanel').prop('hidden', true);
                searchPending(true);
            }
        }).fail(function (xhr) {
            showResult('Khong the xoa danh sach huy chuyen: ' + (xhr.responseText || xhr.statusText), 'error');
        }).always(function () { setLoading(false); });
    }

    function applyCancellation() {
        if (cancellationRequestRunning) return;
        if (!pendingStagingIds.length) {
            showResult('Không có dữ liệu staging để xác nhận hủy chuyến.', 'error');
            return;
        }
        if (!window.confirm('Xác nhận thực hiện HỦY CHUYẾN cho danh sách đã kiểm tra?')) return;
        var succeeded = false;
        var successMessage = '';
        setCancellationProcessing(true);
        $.ajax({
            type: 'POST', url: 'ImportsPermSC_LD_V2.aspx/ApplyCancellation',
            contentType: 'application/json; charset=utf-8', dataType: 'json',
            data: JSON.stringify({ stagingIds: pendingStagingIds })
        }).done(function (response) {
            var result = response.d || {};
            showResult(result.Message, result.Success ? 'success' : 'error');
            succeeded = !!result.Success;
            if (succeeded) {
                pendingStagingIds = [];
                successMessage = result.Message || 'Hủy chuyến thành công. Quy trình đã hoàn tất.';
            }
        }).fail(function (xhr) {
            showResult('Không thể thực hiện hủy chuyến: ' + (xhr.responseText || xhr.statusText), 'error');
        }).always(function () {
            setCancellationProcessing(false);
            if (succeeded) {
                window.alert(successMessage);
                window.location.reload();
            }
        });
    }

    function deleteCurrentCancellation() {
        deleteByIds(pendingStagingIds.slice(0));
    }

    $(function () {
        setLoading(false);
        $('#impPermDate').val(today());
        $('#btnAnalyze').on('click', parse);
        $('#btnViewFormat').on('click', showFormatGuide);
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
        $('#btnApplyCancellation').on('click', applyCancellation);
        $('#btnDeleteCancellation').on('click', deleteCurrentCancellation);
        $('#btnSearchPending').on('click', function () { searchPending(false); });
        $('#btnReviewPending').on('click', reviewPending);
        $('#btnDeletePending').on('click', function () { deleteByIds(selectedPendingIds()); });
        $('#pendingCheckAll').on('change', function () {
            $('#pendingTable tbody .pending-row-check').prop('checked', this.checked);
        });
        $('#impSource').on('paste', function (event) {
            var tableText = tableTextFromClipboard(event);
            if (!tableText) return;
            event.preventDefault();
            $(this).val(tableText);
            rows = [];
            $('#previewPanel,#resultPanel,#confirmPanel').prop('hidden', true);
            message('Đã nhận bảng Word/Excel và giữ nguyên từng ô. Bấm PHÂN TÍCH & KIỂM TRA để xem kết quả.', 'success');
        });
        $('#btnClearSource').on('click', function () { $('#impSource').val('').focus(); rows = []; $('#previewPanel,#resultPanel,#confirmPanel').prop('hidden', true); });
        $('#impFile').on('change', function () { var file = this.files && this.files[0], reader; if (!file) return; reader = new FileReader(); reader.onload = function (e) { $('#impSource').val(e.target.result); }; reader.readAsText(file, 'UTF-8'); });
        $('[data-status-filter]').on('click', function () { activeFilter = $(this).data('status-filter'); $('[data-status-filter]').removeClass('is-active'); $(this).addClass('is-active'); render(); });
        $('#previewSearch').on('input', render);
        $('#previewTable').on('change', '.preview-row-check', function () { rows[+$(this).closest('tr').data('row-index')].selected = this.checked; });
        $('#previewCheckAll').on('change', function () { var checked = this.checked; rows.forEach(function (x) { if (x.status !== 'ERROR') x.selected = checked; }); render(); });
        searchPending(true);
    });
}(jQuery));
