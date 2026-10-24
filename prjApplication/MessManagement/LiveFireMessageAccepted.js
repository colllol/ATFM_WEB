(function ($, window, document) {
    'use strict';

    var cfg = window.liveFireApprovalConfig || {};
    var baseUrl = String(cfg.apiUrl || '');
    var tableUrl;
    var returnUrl;
    var lastFocus = null;

    if (baseUrl && baseUrl.charAt(baseUrl.length - 1) !== '/') baseUrl += '/';
    tableUrl = baseUrl + 'api/ApiExtension/ExcuteTable?packageName=LIVE_FIRE_MESSAGE_PKG&storeName=';
    returnUrl = baseUrl + 'api/ApiExtension/ExcuteReturnInt?packageName=LIVE_FIRE_MESSAGE_PKG&storeName=';

    function encode(value) { return $('<div/>').text(value == null ? '' : value).html(); }
    function isoToDisplay(value) {
        var p = String(value || '').split('-');
        return p.length === 3 ? p[2] + '-' + p[1] + '-' + p[0] : '';
    }
    function todayIso() {
        var d = new Date();
        function two(value) { return value < 10 ? '0' + value : String(value); }
        return d.getFullYear() + '-' + two(d.getMonth() + 1) + '-' + two(d.getDate());
    }
    function statusInfo(status) {
        var map = { 1: ['ĐÃ DUYỆT', 'lf-s1'], 2: ['CHỜ DUYỆT', 'lf-s2'], 4: ['ĐÃ EXPORT', 'lf-s4'] };
        return map[parseInt(status, 10)] || ['KHÔNG XÁC ĐỊNH', 'lf-s0'];
    }
    function rows(data) { return data && $.isArray(data.ListValue) ? data.ListValue : []; }
    function apiTable(store, data) {
        return $.ajax({ method: 'PUT', url: tableUrl + store, contentType: 'application/json; charset=utf-8', dataType: 'json', data: JSON.stringify(data || {}) });
    }
    function apiReturn(store, data) {
        return $.ajax({ method: 'PUT', url: returnUrl + store, contentType: 'application/json; charset=utf-8', dataType: 'json', data: JSON.stringify(data || {}) }).then(function (response) {
            var value = response && response.Code === '00' ? parseInt(response.ListValue, 10) : -1;
            return isNaN(value) ? -1 : value;
        });
    }
    function showError(message, xhr) {
        console.error(message, xhr && (xhr.responseJSON || xhr.responseText || xhr));
        alert(message + ' Có lỗi xảy ra trong quá trình xử lý.');
    }
    function explain(value) {
        if (value === -2) return 'Dữ liệu đã được người khác xử lý hoặc trạng thái không còn phù hợp.';
        if (value === -3) return 'Nội dung vượt quá 2.000 byte nên chưa thể export vào T_PLAN_MESSAGE.';
        if (value === -4) return 'Thông tin bắt buộc chưa đầy đủ.';
        if (value === -5) return 'Tài khoản không có quyền duyệt điện văn.';
        return 'Thao tác không thành công. Kiểm tra log API/Oracle.';
    }

    function render(data) {
        var list = rows(data);
        var page = parseInt($('#lfaGrid').attr('data-page'), 10) || 1;
        var size = parseInt($('#lfaPageSize').val(), 10) || 50;
        var total = list.length ? parseInt(list[0].SUMRECORD, 10) || 0 : 0;
        var html = '';
        $.each(list, function (index, item) {
            var state = statusInfo(item.STATUS);
            html += '<tr><td>' + (((page - 1) * size) + index + 1) + '</td>' +
                '<td>' + encode(item.MESSAGE_DATE) + '</td><td>' + encode(item.MESSAGE_CODE) + '</td>' +
                '<td>' + encode(item.SUBJECT) + '</td><td>' + encode(item.LOCATION_TEXT) + '</td>' +
                '<td><span class="lf-status ' + state[1] + '">' + state[0] + '</span></td>' +
                '<td>' + encode(item.CREATED_BY) + '</td><td>' + encode(item.APPROVED_BY) + '</td>' +
                '<td><button type="button" class="lf-view" data-id="' + item.ID + '">XEM</button></td></tr>';
        });
        if (!html) html = '<tr><td colspan="9" class="lf-empty">Không có dữ liệu phù hợp.</td></tr>';
        $('#lfaGrid tbody').html(html);
        $('#lfaGrid').attr('data-total', total);
        $('#lfaTotal').text('TỔNG SỐ: ' + total);
        updatePager();
    }

    function updatePager() {
        var page = parseInt($('#lfaGrid').attr('data-page'), 10) || 1;
        var total = parseInt($('#lfaGrid').attr('data-total'), 10) || 0;
        var size = parseInt($('#lfaPageSize').val(), 10) || 50;
        var pages = Math.max(1, Math.ceil(total / size));
        if (page > pages) page = pages;
        $('#lfaGrid').attr('data-page', page);
        $('#lfaPageLabel').text('Trang ' + page + '/' + pages);
        $('#lfaPrev').prop('disabled', page <= 1);
        $('#lfaNext').prop('disabled', page >= pages);
    }

    function search(reset) {
        var fromDate = $('#lfaFromDate').val();
        var toDate = $('#lfaToDate').val();
        if (!fromDate || !toDate || fromDate > toDate) { alert('Khoảng ngày tìm kiếm không hợp lệ.'); return; }
        if (reset) $('#lfaGrid').attr('data-page', '1');
        $('#lfaSearch').prop('disabled', true);
        apiTable('GET_LIVE_FIRE_MESSAGES', {
            P_STATUS: parseInt($('#lfaStatus').val(), 10),
            P_FROMDATE: isoToDisplay(fromDate), P_TODATE: isoToDisplay(toDate),
            P_KEYWORD: $.trim($('#lfaKeyword').val()),
            P_PAGESIZE: parseInt($('#lfaPageSize').val(), 10),
            P_PAGEINDEX: (parseInt($('#lfaGrid').attr('data-page'), 10) || 1) - 1
        }).done(render).fail(function (xhr) { showError('Không thể tải danh sách duyệt.', xhr); })
          .always(function () { $('#lfaSearch').prop('disabled', false); });
    }

    function openModal() {
        if (!$('#lfaEditor').hasClass('is-open')) lastFocus = document.activeElement;
        $('#lfaEditor').addClass('is-open').attr('aria-hidden', 'false');
        $('body').addClass('lf-modal-open');
        $('#lfaClose').focus();
    }
    function closeModal() {
        if (lastFocus && typeof lastFocus.focus === 'function') lastFocus.focus();
        $('#lfaEditor').removeClass('is-open').attr('aria-hidden', 'true');
        $('body').removeClass('lf-modal-open');
    }
    function text(id, value) { $(id).text(value == null || value === '' ? '-' : value); }
    function updateActions() {
        var status = parseInt($('#lfaCurrentStatus').val(), 10);
        $('#lfaApprove, #lfaReject').prop('disabled', !cfg.canPublish || status !== 2);
        $('#lfaExport').prop('disabled', !cfg.canPublish || status !== 1);
    }

    function loadDetail(id) {
        openModal();
        $('#lfaEditorTitle').text('Đang tải điện văn...');
        apiTable('GET_LIVE_FIRE_MESSAGE', { P_ID: id }).done(function (data) {
            var list = rows(data);
            var item;
            var state;
            if (!list.length) { alert('Không tìm thấy điện văn.'); closeModal(); search(false); return; }
            item = list[0]; state = statusInfo(item.STATUS);
            $('#lfaId').val(item.ID); $('#lfaVersion').val(item.VERSION_NO); $('#lfaCurrentStatus').val(item.STATUS);
            text('#lfaMessageDate', item.MESSAGE_DATE); text('#lfaMessageCode', item.MESSAGE_CODE);
            text('#lfaSubject', item.SUBJECT); text('#lfaStatusText', state[0]); text('#lfaLocation', item.LOCATION_TEXT);
            text('#lfaCoordinates', item.COORDINATES_TEXT); text('#lfaSchedules', item.SCHEDULE_TEXT);
            text('#lfaDirection', item.FIRING_DIRECTION); text('#lfaHeight', item.TRAJECTORY_HEIGHT); text('#lfaRange', item.TRAJECTORY_RANGE);
            text('#lfaCreatedBy', item.CREATED_BY + (item.CREATED_DATE ? ' - ' + item.CREATED_DATE : ''));
            text('#lfaSubmittedBy', (item.SUBMITTED_BY || '-') + (item.SUBMITTED_DATE ? ' - ' + item.SUBMITTED_DATE : ''));
            text('#lfaApprovedBy', (item.APPROVED_BY || '-') + (item.APPROVED_DATE ? ' - ' + item.APPROVED_DATE : ''));
            text('#lfaExportedBy', (item.EXPORTED_BY || '-') + (item.EXPORTED_DATE ? ' - ' + item.EXPORTED_DATE : ''));
            $('#lfaPreview').val(item.APPROVED_CONTENT || item.DRAFT_CONTENT || '');
            if (item.REJECT_REASON) $('#lfaRejectInfo').text('Lý do từ chối: ' + item.REJECT_REASON).show();
            else $('#lfaRejectInfo').hide().empty();
            $('#lfaEditorTitle').text('Điện văn #' + item.ID + ' - ' + state[0]);
            updateActions();
        }).fail(function (xhr) { closeModal(); showError('Không thể tải chi tiết điện văn.', xhr); });
    }

    function workflow(store, question, success, extra) {
        var id = parseInt($('#lfaId').val(), 10) || 0;
        var request = $.extend({ P_ID: id, P_VERSION_NO: parseInt($('#lfaVersion').val(), 10) || 0, P_USER: cfg.userName }, extra || {});
        if (!id || !confirm(question)) return;
        $('#lfaApprove, #lfaReject, #lfaExport').prop('disabled', true);
        apiReturn(store, request).done(function (value) {
            if (value <= 0) { alert(explain(value)); loadDetail(id); return; }
            alert(success + ' thành công.'); closeModal(); search(false);
        }).fail(function (xhr) { showError(success + ' không thành công.', xhr); updateActions(); });
    }

    $('#lfaSearch').on('click', function () { search(true); });
    $('#lfaStatus, #lfaPageSize').on('change', function () { search(true); });
    $('#lfaKeyword').on('keydown', function (event) { if (event.keyCode === 13) search(true); });
    $('#lfaGrid').on('click', '.lf-view', function () { loadDetail(parseInt($(this).attr('data-id'), 10)); });
    $('#lfaPrev').on('click', function () { var p = parseInt($('#lfaGrid').attr('data-page'), 10) || 1; if (p > 1) { $('#lfaGrid').attr('data-page', p - 1); search(false); } });
    $('#lfaNext').on('click', function () { var p = parseInt($('#lfaGrid').attr('data-page'), 10) || 1; $('#lfaGrid').attr('data-page', p + 1); search(false); });
    $('#lfaClose, #lfaCancel').on('click', closeModal);
    $('#lfaEditor').on('click', function (event) { if (event.target === this) closeModal(); });
    $(document).on('keydown', function (event) { if (event.key === 'Escape' && $('#lfaEditor').hasClass('is-open')) closeModal(); });
    $('#lfaApprove').on('click', function () { workflow('APPROVE_LIVE_FIRE_MESSAGE', 'Duyệt và khóa nội dung điện văn này?', 'Duyệt điện văn'); });
    $('#lfaReject').on('click', function () { var reason = prompt('Nhập lý do từ chối:'); if (reason === null) return; reason = $.trim(reason); if (!reason) { alert('Lý do từ chối là bắt buộc.'); return; } workflow('REJECT_LIVE_FIRE_MESSAGE', 'Xác nhận từ chối điện văn?', 'Từ chối điện văn', { P_REASON: reason }); });
    $('#lfaExport').on('click', function () { workflow('EXPORT_LIVE_FIRE_MESSAGE', 'Export nội dung đã duyệt vào T_PLAN_MESSAGE để phát đi?', 'Export điện văn'); });

    $(function () { var today = todayIso(); $('#lfaFromDate, #lfaToDate').val(today); search(true); });
})(jQuery, window, document);
