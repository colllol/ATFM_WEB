(function ($, window, document) {
    'use strict';

    var cfg = window.liveFireConfig || {};
    var baseUrl = String(cfg.apiUrl || '');
    var tableUrl;
    var returnUrl;
    var editorDirty = false;
    var lastFocus = null;

    if (baseUrl && baseUrl.charAt(baseUrl.length - 1) !== '/') baseUrl += '/';
    tableUrl = baseUrl + 'api/ApiExtension/ExcuteTable?packageName=LIVE_FIRE_MESSAGE_PKG&storeName=';
    returnUrl = baseUrl + 'api/ApiExtension/ExcuteReturnInt?packageName=LIVE_FIRE_MESSAGE_PKG&storeName=';

    function encode(value) {
        return $('<div/>').text(value == null ? '' : value).html();
    }

    function isoToDisplay(value) {
        var parts = String(value || '').split('-');
        return parts.length === 3 ? parts[2] + '-' + parts[1] + '-' + parts[0] : '';
    }

    function displayToIso(value) {
        var parts = String(value || '').substring(0, 10).split('-');
        return parts.length === 3 ? parts[2] + '-' + parts[1] + '-' + parts[0] : '';
    }

    function todayIso() {
        var now = new Date();
        return now.getFullYear() + '-' + String(now.getMonth() + 1).padStart(2, '0') + '-' + String(now.getDate()).padStart(2, '0');
    }

    function normalizeText(value) {
        return $.trim(String(value == null ? '' : value)).toUpperCase();
    }

    function statusInfo(status) {
        var values = {
            0: ['NHÁP', 'lf-s0'],
            1: ['ĐÃ DUYỆT', 'lf-s1'],
            2: ['CHỜ DUYỆT', 'lf-s2'],
            3: ['TỪ CHỐI', 'lf-s3'],
            4: ['ĐÃ EXPORT', 'lf-s4']
        };
        return values[parseInt(status, 10)] || ['KHÔNG XÁC ĐỊNH', 'lf-s0'];
    }

    function apiTable(store, data) {
        return $.ajax({
            method: 'PUT',
            url: tableUrl + store,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data || {})
        });
    }

    function apiReturn(store, data) {
        return $.ajax({
            method: 'PUT',
            url: returnUrl + store,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data || {})
        }).then(function (response) {
            var value = response && response.Code === '00' ? parseInt(response.ListValue, 10) : -1;
            if (isNaN(value)) value = -1;
            return value;
        });
    }

    function showError(prefix, xhr) {
        console.error(prefix, xhr && (xhr.responseJSON || xhr.responseText || xhr));
        alert(prefix + ' Có lỗi xảy ra trong quá trình xử lý dữ liệu.');
    }

    function addCoordinate(item) {
        item = item || {};
        var index = $('#lfCoordinates tbody tr').length + 1;
        $('#lfCoordinates tbody').append(
            '<tr><td><input data-field="point" maxlength="20" value="' + encode(item.point || ('D' + index)) + '" /></td>' +
            '<td><input data-field="latitude" maxlength="100" value="' + encode(item.latitude || '') + '" placeholder="15 53 20" /></td>' +
            '<td><input data-field="longitude" maxlength="100" value="' + encode(item.longitude || '') + '" placeholder="108 37 00" /></td>' +
            '<td><button type="button" class="lf-remove" title="Xóa">×</button></td></tr>'
        );
    }

    function addSchedule(item) {
        item = item || {};
        $('#lfSchedules tbody').append(
            '<tr><td><input data-field="fromTime" maxlength="5" value="' + encode(item.fromTime || '') + '" placeholder="08:00" /></td>' +
            '<td><input data-field="toTime" maxlength="5" value="' + encode(item.toTime || '') + '" placeholder="09:00" /></td>' +
            '<td><input data-field="date" type="date" value="' + encode(item.date || $('#lfMessageDate').val() || todayIso()) + '" /></td>' +
            '<td><input data-field="altitude" maxlength="100" value="' + encode(item.altitude || '') + '" placeholder="6700 M" /></td>' +
            '<td><button type="button" class="lf-remove" title="Xóa">×</button></td></tr>'
        );
    }

    function coordinates() {
        var result = [];
        $('#lfCoordinates tbody tr').each(function () {
            var row = {
                point: normalizeText($(this).find('[data-field=point]').val()),
                latitude: normalizeText($(this).find('[data-field=latitude]').val()),
                longitude: normalizeText($(this).find('[data-field=longitude]').val())
            };
            if (row.point || row.latitude || row.longitude) result.push(row);
        });
        return result;
    }

    function schedules() {
        var result = [];
        $('#lfSchedules tbody tr').each(function () {
            var row = {
                fromTime: $.trim($(this).find('[data-field=fromTime]').val()),
                toTime: $.trim($(this).find('[data-field=toTime]').val()),
                date: $.trim($(this).find('[data-field=date]').val()),
                altitude: normalizeText($(this).find('[data-field=altitude]').val())
            };
            if (row.fromTime || row.toTime || row.date || row.altitude) result.push(row);
        });
        return result;
    }

    function formatTime(value) {
        var digits = String(value || '').replace(/\D/g, '').substring(0, 4);
        if (digits.length !== 4) return normalizeText(value);
        return digits.substring(0, 2) + 'H' + digits.substring(2);
    }

    function coordinateText(items) {
        return $.map(items, function (item) {
            return ' ' + item.point + ' ' + item.latitude + '-' + item.longitude;
        }).join('\n');
    }

    function scheduleText(items) {
        return $.map(items, function (item) {
            var line = '  -TU ' + formatTime(item.fromTime) + '-' + formatTime(item.toTime);
            if (item.date) line += ' NGAY ' + isoToDisplay(item.date).replace(/-/g, '/');
            if (item.altitude) line += ' (DO CAO:' + item.altitude + ')';
            return line + ',';
        }).join('\n');
    }

    function buildPreview() {
        var coords = coordinates();
        var times = schedules();
        var lines = [];
        var intro = normalizeText($('#lfIntro').val());
        var subject = normalizeText($('#lfSubject').val()) || 'THONG BAO BAN DAN THAT';
        var restriction = normalizeText($('#lfRestriction').val());

        if (intro) lines.push(intro, '');
        lines.push('        ' + subject, '');
        lines.push(' BTTM DONG Y CHO DON VI BAN DAN THAT THEO KE HOACH SAU:');
        lines.push('1.DIA DIEM BAN:' + normalizeText($('#lfLocation').val()));
        if (coords.length) lines.push(' CO TOA DO:', coordinateText(coords));
        lines.push('2.PHUONG VI BAN:' + (normalizeText($('#lfDirection').val()) || '-'));
        lines.push('3.DO CAO DUONG DAN:' + (normalizeText($('#lfHeight').val()) || '-'));
        lines.push('4.CU LY DUONG DAN:' + (normalizeText($('#lfRange').val()) || '-'));
        lines.push('5.THOI GIAN BAN:');
        lines.push(times.length ? scheduleText(times) : '  -');
        lines.push('6.CHI HUY BAN:');
        lines.push('-' + [normalizeText($('#lfCommanderRank').val()), normalizeText($('#lfCommanderName').val())].filter(Boolean).join(':') + (normalizeText($('#lfCommanderPhone').val()) ? ' SDT ' + normalizeText($('#lfCommanderPhone').val()) : ''));
        lines.push('-NGUOI THAY THE:' + [normalizeText($('#lfReplacementRank').val()), normalizeText($('#lfReplacementName').val())].filter(Boolean).join(' ') + (normalizeText($('#lfReplacementPhone').val()) ? ' SDT ' + normalizeText($('#lfReplacementPhone').val()) : ''));
        if (restriction) lines.push(' +' + restriction);
        lines.push('                       KI DIEN');
        if (normalizeText($('#lfSignatoryTitle').val())) lines.push('                    ' + normalizeText($('#lfSignatoryTitle').val()));
        if (normalizeText($('#lfSignatoryName').val())) lines.push('', '                  ' + normalizeText($('#lfSignatoryName').val()));
        return lines.join('\n').replace(/[ \t]+\n/g, '\n');
    }

    function refreshPreview() {
        $('#lfPreview').val(buildPreview());
    }

    function resetEditor() {
        $('#lfId').val('0');
        $('#lfVersion').val('0');
        $('#lfCurrentStatus').val('0');
        $('#lfEditor input:not([type=hidden]), #lfEditor textarea').val('');
        $('#lfSubject').val('THONG BAO BAN DAN THAT');
        $('#lfMessageDate').val(todayIso());
        $('#lfCoordinates tbody, #lfSchedules tbody').empty();
        addCoordinate(); addCoordinate(); addCoordinate(); addCoordinate();
        addSchedule({ date: todayIso() });
        $('#lfRejectInfo').hide().empty();
        editorDirty = true;
        refreshPreview();
        updateEditorState();
    }

    function openEditor() {
        if (!$('#lfEditor').hasClass('is-open')) lastFocus = document.activeElement;
        $('#lfEditor').addClass('is-open').attr('aria-hidden', 'false');
        $('body').addClass('lf-modal-open');
        $('#lfClose').focus();
    }

    function closeEditor() {
        if (lastFocus && typeof lastFocus.focus === 'function') lastFocus.focus();
        $('#lfEditor').removeClass('is-open').attr('aria-hidden', 'true');
        $('body').removeClass('lf-modal-open');
    }

    function updateEditorState() {
        var id = parseInt($('#lfId').val(), 10) || 0;
        var status = parseInt($('#lfCurrentStatus').val(), 10) || 0;
        var editable = (status === 0 || status === 3) && ((id === 0 && cfg.canAdd) || (id > 0 && cfg.canEdit));
        $('#lfEditor .lf-editor-body').toggleClass('lf-readonly', !editable);
        $('#lfSave').prop('disabled', !editable);
        $('#lfSubmit').prop('disabled', id === 0 || !cfg.canEdit || !(status === 0 || status === 3) || editorDirty);
        $('#lfApprove, #lfReject').prop('disabled', !cfg.canPublish || status !== 2);
        $('#lfExport').prop('disabled', !cfg.canPublish || status !== 1);
    }

    function resultRows(data) {
        return data && $.isArray(data.ListValue) ? data.ListValue : [];
    }

    function renderList(data) {
        var rows = resultRows(data);
        var page = parseInt($('#lfGrid').attr('data-page'), 10) || 1;
        var size = parseInt($('#lfPageSize').val(), 10) || 50;
        var total = rows.length ? parseInt(rows[0].SUMRECORD, 10) || 0 : 0;
        var html = '';
        $.each(rows, function (index, item) {
            var state = statusInfo(item.STATUS);
            html += '<tr><td>' + (((page - 1) * size) + index + 1) + '</td>' +
                '<td>' + encode(item.MESSAGE_DATE) + '</td>' +
                '<td>' + encode(item.MESSAGE_CODE) + '</td>' +
                '<td>' + encode(item.SUBJECT) + '</td>' +
                '<td>' + encode(item.LOCATION_TEXT) + '</td>' +
                '<td><span class="lf-status ' + state[1] + '">' + state[0] + '</span></td>' +
                '<td>' + encode(item.CREATED_BY) + '</td>' +
                '<td>' + encode(item.APPROVED_BY) + '</td>' +
                '<td><button type="button" class="lf-view" data-id="' + item.ID + '">XEM</button></td></tr>';
        });
        if (!html) html = '<tr><td colspan="9" class="lf-empty">Không có dữ liệu phù hợp.</td></tr>';
        $('#lfGrid tbody').html(html);
        $('#lfGrid').attr('data-total', total);
        $('#lfTotal').text('TỔNG SỐ: ' + total);
        updatePager();
    }

    function updatePager() {
        var page = parseInt($('#lfGrid').attr('data-page'), 10) || 1;
        var total = parseInt($('#lfGrid').attr('data-total'), 10) || 0;
        var size = parseInt($('#lfPageSize').val(), 10) || 50;
        var pages = Math.max(1, Math.ceil(total / size));
        if (page > pages) page = pages;
        $('#lfGrid').attr('data-page', page);
        $('#lfPageLabel').text('Trang ' + page + '/' + pages);
        $('#lfPrev').prop('disabled', page <= 1);
        $('#lfNext').prop('disabled', page >= pages);
    }

    function search(resetPage) {
        var fromDate = $('#lfFromDate').val();
        var toDate = $('#lfToDate').val();
        if (!fromDate || !toDate || fromDate > toDate) {
            alert('Khoảng ngày tìm kiếm không hợp lệ.');
            return;
        }
        if (resetPage) $('#lfGrid').attr('data-page', '1');
        $('#lfSearch').prop('disabled', true);
        apiTable('GET_LIVE_FIRE_MESSAGES', {
            P_STATUS: parseInt($('#lfStatus').val(), 10),
            P_FROMDATE: isoToDisplay(fromDate),
            P_TODATE: isoToDisplay(toDate),
            P_KEYWORD: $.trim($('#lfKeyword').val()),
            P_PAGESIZE: parseInt($('#lfPageSize').val(), 10),
            P_PAGEINDEX: (parseInt($('#lfGrid').attr('data-page'), 10) || 1) - 1
        }).done(renderList).fail(function (xhr) {
            showError('Không thể tải danh sách điện văn.', xhr);
        }).always(function () { $('#lfSearch').prop('disabled', false); });
    }

    function parseJson(value, fallback) {
        try { return value ? JSON.parse(value) : fallback; }
        catch (error) {
            console.warn('Invalid stored JSON:', error);
            return fallback;
        }
    }

    function loadDetail(id) {
        $('#lfEditorTitle').text('Đang tải điện văn...');
        openEditor();
        apiTable('GET_LIVE_FIRE_MESSAGE', { P_ID: id }).done(function (data) {
            var rows = resultRows(data);
            var item;
            var coords;
            var times;
            if (!rows.length) {
                alert('Không tìm thấy điện văn hoặc dữ liệu đã thay đổi.');
                closeEditor();
                search(false);
                return;
            }
            item = rows[0];
            $('#lfId').val(item.ID);
            $('#lfVersion').val(item.VERSION_NO);
            $('#lfCurrentStatus').val(item.STATUS);
            $('#lfMessageDate').val(displayToIso(item.MESSAGE_DATE));
            $('#lfMessageCode').val(item.MESSAGE_CODE || '');
            $('#lfSubject').val(item.SUBJECT || '');
            $('#lfIntro').val(item.INTRO_TEXT || '');
            $('#lfLocation').val(item.LOCATION_TEXT || '');
            $('#lfDirection').val(item.FIRING_DIRECTION || '');
            $('#lfHeight').val(item.TRAJECTORY_HEIGHT || '');
            $('#lfRange').val(item.TRAJECTORY_RANGE || '');
            $('#lfCommanderRank').val(item.COMMANDER_RANK || '');
            $('#lfCommanderName').val(item.COMMANDER_NAME || '');
            $('#lfCommanderPhone').val(item.COMMANDER_PHONE || '');
            $('#lfReplacementRank').val(item.REPLACEMENT_RANK || '');
            $('#lfReplacementName').val(item.REPLACEMENT_NAME || '');
            $('#lfReplacementPhone').val(item.REPLACEMENT_PHONE || '');
            $('#lfRestriction').val(item.RESTRICTION_TEXT || '');
            $('#lfSignatoryTitle').val(item.SIGNATORY_TITLE || '');
            $('#lfSignatoryName').val(item.SIGNATORY_NAME || '');
            coords = parseJson(item.COORDINATES_JSON, []);
            times = parseJson(item.SCHEDULE_JSON, []);
            $('#lfCoordinates tbody, #lfSchedules tbody').empty();
            $.each(coords, function (_, coord) { addCoordinate(coord); });
            $.each(times, function (_, time) {
                if (time.date && /^\d{2}-\d{2}-\d{4}$/.test(time.date)) time.date = displayToIso(time.date);
                addSchedule(time);
            });
            if (!coords.length) addCoordinate();
            if (!times.length) addSchedule({ date: $('#lfMessageDate').val() });
            $('#lfPreview').val(item.STATUS === 1 || item.STATUS === 4
                ? (item.APPROVED_CONTENT || item.DRAFT_CONTENT || '')
                : (item.DRAFT_CONTENT || buildPreview()));
            if (item.REJECT_REASON) {
                $('#lfRejectInfo').text('Lý do từ chối: ' + item.REJECT_REASON).show();
            } else {
                $('#lfRejectInfo').hide().empty();
            }
            $('#lfEditorTitle').text('Điện văn #' + item.ID + ' - ' + statusInfo(item.STATUS)[0]);
            editorDirty = false;
            updateEditorState();
        }).fail(function (xhr) {
            closeEditor();
            showError('Không thể tải chi tiết điện văn.', xhr);
        });
    }

    function validateEditor() {
        var validTime = /^([01]\d|2[0-3]):[0-5]\d$/;
        var okay = true;
        if (!$('#lfMessageDate').val() || !$.trim($('#lfSubject').val()) || !$.trim($('#lfLocation').val())) {
            alert('Ngày điện văn, Tiêu đề và Địa điểm bắn là bắt buộc.');
            return false;
        }
        $('#lfSchedules tbody tr').each(function () {
            var fromTime = $.trim($(this).find('[data-field=fromTime]').val());
            var toTime = $.trim($(this).find('[data-field=toTime]').val());
            if ((fromTime && !validTime.test(fromTime)) || (toTime && !validTime.test(toTime)) || (fromTime && toTime && fromTime >= toTime)) {
                okay = false;
                return false;
            }
        });
        if (!okay) alert('Khung giờ bắn phải đúng HH:mm và Từ giờ nhỏ hơn Đến giờ.');
        return okay;
    }

    function formRequest() {
        var coords = coordinates();
        var times = schedules();
        var preview = buildPreview();
        $('#lfPreview').val(preview);
        return {
            P_ID: parseInt($('#lfId').val(), 10) || 0,
            P_VERSION_NO: parseInt($('#lfVersion').val(), 10) || 0,
            P_MESSAGE_DATE: isoToDisplay($('#lfMessageDate').val()),
            P_MESSAGE_CODE: normalizeText($('#lfMessageCode').val()),
            P_SUBJECT: normalizeText($('#lfSubject').val()),
            P_INTRO_TEXT: normalizeText($('#lfIntro').val()),
            P_LOCATION_TEXT: normalizeText($('#lfLocation').val()),
            P_COORDINATES_JSON: JSON.stringify(coords),
            P_COORDINATES_TEXT: coordinateText(coords),
            P_FIRING_DIRECTION: normalizeText($('#lfDirection').val()),
            P_TRAJECTORY_HEIGHT: normalizeText($('#lfHeight').val()),
            P_TRAJECTORY_RANGE: normalizeText($('#lfRange').val()),
            P_SCHEDULE_JSON: JSON.stringify(times),
            P_SCHEDULE_TEXT: scheduleText(times),
            P_COMMANDER_RANK: normalizeText($('#lfCommanderRank').val()),
            P_COMMANDER_NAME: normalizeText($('#lfCommanderName').val()),
            P_COMMANDER_PHONE: normalizeText($('#lfCommanderPhone').val()),
            P_REPLACEMENT_RANK: normalizeText($('#lfReplacementRank').val()),
            P_REPLACEMENT_NAME: normalizeText($('#lfReplacementName').val()),
            P_REPLACEMENT_PHONE: normalizeText($('#lfReplacementPhone').val()),
            P_RESTRICTION_TEXT: normalizeText($('#lfRestriction').val()),
            P_SIGNATORY_TITLE: normalizeText($('#lfSignatoryTitle').val()),
            P_SIGNATORY_NAME: normalizeText($('#lfSignatoryName').val()),
            P_DRAFT_CONTENT: preview,
            P_USER: cfg.userName
        };
    }

    function explainReturn(value, action) {
        if (value === -2) return 'Dữ liệu đã được người khác cập nhật hoặc trạng thái không còn phù hợp. Vui lòng tải lại.';
        if (value === -3) return 'Nội dung điện văn vượt quá 2.000 byte nên chưa thể đưa vào T_PLAN_MESSAGE.';
        if (value === -4) return 'Thông tin bắt buộc chưa đầy đủ.';
        if (value === -5) return 'Tài khoản không có quyền thực hiện thao tác này.';
        return action + ' không thành công. Kiểm tra log API/Oracle để biết nguyên nhân.';
    }

    function saveMessage() {
        var deferred = $.Deferred();
        if (!validateEditor()) {
            deferred.reject();
            return deferred.promise();
        }
        $('#lfSave').prop('disabled', true);
        apiReturn('SAVE_LIVE_FIRE_MESSAGE', formRequest()).done(function (value) {
            if (value <= 0) {
                alert(explainReturn(value, 'Lưu điện văn'));
                deferred.reject(value);
                return;
            }
            alert('Lưu điện văn thành công.');
            editorDirty = false;
            search(false);
            loadDetail(value);
            deferred.resolve(value);
        }).fail(function (xhr) {
            showError('Không thể lưu điện văn.', xhr);
            deferred.reject(xhr);
        }).always(updateEditorState);
        return deferred.promise();
    }

    function workflow(store, confirmText, successText, extra) {
        var id = parseInt($('#lfId').val(), 10) || 0;
        var request = $.extend({
            P_ID: id,
            P_VERSION_NO: parseInt($('#lfVersion').val(), 10) || 0,
            P_USER: cfg.userName
        }, extra || {});
        if (!id || !confirm(confirmText)) return;
        $('#lfSubmit, #lfApprove, #lfReject, #lfExport').prop('disabled', true);
        apiReturn(store, request).done(function (value) {
            if (value <= 0) {
                alert(explainReturn(value, successText));
                loadDetail(id);
                return;
            }
            alert(successText + ' thành công.');
            search(false);
            loadDetail(id);
        }).fail(function (xhr) {
            showError(successText + ' không thành công.', xhr);
            updateEditorState();
        });
    }

    $('#lfSearch').on('click', function () { search(true); });
    $('#lfStatus, #lfPageSize').on('change', function () { search(true); });
    $('#lfKeyword').on('keydown', function (event) { if (event.keyCode === 13) search(true); });
    $('#lfPrev').on('click', function () {
        var page = parseInt($('#lfGrid').attr('data-page'), 10) || 1;
        if (page > 1) { $('#lfGrid').attr('data-page', page - 1); search(false); }
    });
    $('#lfNext').on('click', function () {
        var page = parseInt($('#lfGrid').attr('data-page'), 10) || 1;
        $('#lfGrid').attr('data-page', page + 1); search(false);
    });
    $('#lfGrid').on('click', '.lf-view', function () { loadDetail(parseInt($(this).attr('data-id'), 10)); });
    $('#lfNew').on('click', function () { resetEditor(); $('#lfEditorTitle').text('Thêm mới điện văn bắn đạn thật'); openEditor(); });
    $('#lfClose, #lfCancel').on('click', closeEditor);
    $('#lfEditor').on('click', function (event) { if (event.target === this) closeEditor(); });
    $(document).on('keydown', function (event) { if (event.key === 'Escape' && $('#lfEditor').hasClass('is-open')) closeEditor(); });
    $('#lfAddCoordinate').on('click', function () { addCoordinate(); editorDirty = true; refreshPreview(); updateEditorState(); });
    $('#lfAddSchedule').on('click', function () { addSchedule(); editorDirty = true; refreshPreview(); updateEditorState(); });
    $('#lfCoordinates, #lfSchedules').on('click', '.lf-remove', function () { $(this).closest('tr').remove(); editorDirty = true; refreshPreview(); updateEditorState(); });
    $('#lfEditor').on('input change', 'input:not([type=hidden]), textarea:not(#lfPreview)', function () { editorDirty = true; refreshPreview(); updateEditorState(); });
    $('#lfSave').on('click', saveMessage);
    $('#lfSubmit').on('click', function () {
        if (editorDirty) { alert('Vui lòng Lưu nháp các thay đổi trước khi gửi duyệt.'); return; }
        workflow('SUBMIT_LIVE_FIRE_MESSAGE', 'Gửi điện văn này để duyệt?', 'Gửi duyệt');
    });
    $('#lfApprove').on('click', function () { workflow('APPROVE_LIVE_FIRE_MESSAGE', 'Duyệt và khóa nội dung điện văn này?', 'Duyệt điện văn'); });
    $('#lfReject').on('click', function () {
        var reason = prompt('Nhập lý do từ chối:');
        if (reason === null) return;
        reason = $.trim(reason);
        if (!reason) { alert('Lý do từ chối là bắt buộc.'); return; }
        workflow('REJECT_LIVE_FIRE_MESSAGE', 'Xác nhận từ chối điện văn này?', 'Từ chối điện văn', { P_REASON: reason });
    });
    $('#lfExport').on('click', function () {
        workflow('EXPORT_LIVE_FIRE_MESSAGE', 'Export nội dung đã duyệt vào T_PLAN_MESSAGE để phát đi?', 'Export điện văn');
    });

    $(function () {
        var today = todayIso();
        $('#lfFromDate, #lfToDate').val(today);
        $('#lfNew').prop('disabled', !cfg.canAdd);
        search(true);
    });
})(jQuery, window, document);
