(function ($) {
    'use strict';

    var root = document.getElementById('notificationsPage');
    if (!root || !$) return;

    var endpoint = root.getAttribute('data-endpoint');
    var pageSize = 100;
    var currentPage = 1;
    var currentStatus = -1;
    var totalRecords = 0;
    var unreadCount = 0;
    var requestInFlight = false;
    var reloadPending = false;

    var rows = document.getElementById('notificationsRows');
    var summary = document.getElementById('notificationsSummary');
    var unreadNode = document.getElementById('notificationsUnreadCount');
    var pageInfo = document.getElementById('notificationsPageInfo');
    var pager = document.getElementById('notificationsPager');
    var loading = document.getElementById('notificationsLoading');
    var errorBox = document.getElementById('notificationsError');
    var refreshButton = document.getElementById('notificationsRefresh');
    var markAllButton = document.getElementById('notificationsMarkAll');
    var filterButtons = root.querySelectorAll('.notifications-segmented button[data-status]');

    function sendRequest(options) {
        options = $.extend({}, options, {
            global: false,
            xhr: function () {
                var xhr = $.ajaxSettings.xhr();
                xhr.atfmSuppressLoading = true;
                return xhr;
            }
        });
        return $.ajax(options);
    }

    function parseDate(value) {
        if (!value) return null;
        var match = /\/Date\((\d+)/.exec(value);
        var date = match ? new Date(parseInt(match[1], 10)) : new Date(value);
        return isNaN(date.getTime()) ? null : date;
    }

    function formatDate(value) {
        var date = parseDate(value);
        if (!date) return '';
        var pad = function (number) { return number < 10 ? '0' + number : String(number); };
        return pad(date.getDate()) + '/' + pad(date.getMonth() + 1) + '/' + date.getFullYear()
            + ' ' + pad(date.getHours()) + ':' + pad(date.getMinutes()) + ':' + pad(date.getSeconds());
    }

    function setLoading(isLoading) {
        requestInFlight = isLoading;
        loading.hidden = !isLoading;
        refreshButton.disabled = isLoading;
    }

    function showError(message) {
        errorBox.textContent = message || '';
        errorBox.hidden = !message;
    }

    function appendCell(row, className, value) {
        var cell = document.createElement('td');
        cell.className = className;
        cell.textContent = value == null ? '' : String(value);
        row.appendChild(cell);
        return cell;
    }

    function createStatusCell(row, isUnread) {
        var cell = document.createElement('td');
        cell.className = 'notifications-status-cell';
        var status = document.createElement('span');
        status.className = 'notifications-status ' + (isUnread ? 'is-unread' : 'is-read');
        status.innerHTML = isUnread
            ? '<i class="fa fa-envelope" aria-hidden="true"></i><span>Chưa đọc</span>'
            : '<i class="fa fa-check-circle" aria-hidden="true"></i><span>Đã đọc</span>';
        cell.appendChild(status);
        row.appendChild(cell);
    }

    function createActionCell(row, item, isUnread) {
        var cell = document.createElement('td');
        cell.className = 'notifications-action-cell';
        if (isUnread) {
            var button = document.createElement('button');
            button.type = 'button';
            button.className = 'notifications-row-read';
            button.setAttribute('data-notification-id', item.ID);
            button.setAttribute('title', 'Đánh dấu đã đọc');
            button.innerHTML = '<i class="fa fa-check" aria-hidden="true"></i><span>Đọc</span>';
            cell.appendChild(button);
        } else {
            var indicator = document.createElement('span');
            indicator.className = 'notifications-read-indicator';
            indicator.setAttribute('title', 'Thông báo đã đọc');
            indicator.innerHTML = '<i class="fa fa-check-circle" aria-hidden="true"></i>';
            cell.appendChild(indicator);
        }
        row.appendChild(cell);
    }

    function renderRows(items) {
        while (rows.firstChild) rows.removeChild(rows.firstChild);
        items = Object.prototype.toString.call(items) === '[object Array]' ? items : [];

        if (!items.length) {
            var emptyRow = document.createElement('tr');
            var emptyCell = document.createElement('td');
            emptyCell.colSpan = 6;
            emptyCell.className = 'notifications-empty-cell';
            emptyCell.textContent = 'Không có thông báo phù hợp.';
            emptyRow.appendChild(emptyCell);
            rows.appendChild(emptyRow);
            return;
        }

        for (var i = 0; i < items.length; i++) {
            var item = items[i] || {};
            var isUnread = parseInt(item.STATUS, 10) === 0;
            var row = document.createElement('tr');
            row.className = isUnread ? 'is-unread' : 'is-read';
            row.setAttribute('data-notification-id', item.ID);
            createStatusCell(row, isUnread);
            appendCell(row, 'notifications-title-cell', item.TITLE || 'Thông báo');
            appendCell(row, 'notifications-content-cell', item.CONTENT || '');
            appendCell(row, 'notifications-type-cell', item.SOURCE_TYPE || '--');
            appendCell(row, 'notifications-time-cell', formatDate(item.DATETIME));
            createActionCell(row, item, isUnread);
            rows.appendChild(row);
        }
    }

    function createPagerButton(label, targetPage, title, disabled, current) {
        var button = document.createElement('button');
        button.type = 'button';
        button.setAttribute('data-page', targetPage);
        button.setAttribute('title', title || '');
        button.setAttribute('aria-label', title || String(label));
        button.disabled = !!disabled;
        if (current) {
            button.className = 'is-current';
            button.setAttribute('aria-current', 'page');
        }
        button.innerHTML = label;
        pager.appendChild(button);
    }

    function renderPager() {
        var totalPages = Math.max(1, Math.ceil(totalRecords / pageSize));
        var firstRecord = totalRecords === 0 ? 0 : ((currentPage - 1) * pageSize) + 1;
        var lastRecord = Math.min(currentPage * pageSize, totalRecords);
        pageInfo.textContent = 'Trang ' + currentPage + '/' + totalPages + ' · ' + firstRecord + '-' + lastRecord + ' / ' + totalRecords;

        while (pager.firstChild) pager.removeChild(pager.firstChild);
        createPagerButton('<i class="fa fa-angle-double-left" aria-hidden="true"></i>', 1, 'Trang đầu', currentPage <= 1, false);
        createPagerButton('<i class="fa fa-angle-left" aria-hidden="true"></i>', currentPage - 1, 'Trang trước', currentPage <= 1, false);

        var start = Math.max(1, currentPage - 2);
        var end = Math.min(totalPages, start + 4);
        start = Math.max(1, end - 4);
        for (var page = start; page <= end; page++) {
            createPagerButton(String(page), page, 'Trang ' + page, false, page === currentPage);
        }

        createPagerButton('<i class="fa fa-angle-right" aria-hidden="true"></i>', currentPage + 1, 'Trang sau', currentPage >= totalPages, false);
        createPagerButton('<i class="fa fa-angle-double-right" aria-hidden="true"></i>', totalPages, 'Trang cuối', currentPage >= totalPages, false);
    }

    function updateHeader() {
        unreadNode.textContent = String(unreadCount);
        markAllButton.disabled = unreadCount === 0 || requestInFlight;
        var filterName = currentStatus === 0 ? 'chưa đọc' : (currentStatus === 1 ? 'đã đọc' : 'tất cả');
        summary.textContent = totalRecords + ' thông báo ' + filterName + ', ' + unreadCount + ' thông báo chưa đọc';
    }

    function dispatchNotificationsChanged() {
        var event;
        if (typeof window.CustomEvent === 'function') event = new CustomEvent('atfm:notifications-changed');
        else {
            event = document.createEvent('CustomEvent');
            event.initCustomEvent('atfm:notifications-changed', false, false, null);
        }
        document.dispatchEvent(event);
    }

    function loadPage() {
        if (requestInFlight) {
            reloadPending = true;
            return;
        }
        var reloadAdjustedPage = false;
        setLoading(true);
        showError('');
        sendRequest({
            type: 'GET',
            url: endpoint,
            dataType: 'json',
            cache: false,
            data: { action: 'list', status: currentStatus, page: currentPage }
        }).done(function (response) {
            if (!response || response.Code !== '00') {
                showError('Không thể tải danh sách thông báo.');
                return;
            }

            totalRecords = Math.max(0, parseInt(response.SumRecord, 10) || 0);
            unreadCount = Math.max(0, parseInt(response.Value, 10) || 0);
            var totalPages = Math.max(1, Math.ceil(totalRecords / pageSize));
            if (currentPage > totalPages) {
                currentPage = totalPages;
                reloadAdjustedPage = true;
                return;
            }

            renderRows(response.ListValue || []);
            renderPager();
            updateHeader();
        }).fail(function () {
            showError('Không thể kết nối dịch vụ thông báo.');
        }).always(function () {
            setLoading(false);
            updateHeader();
            if (reloadAdjustedPage || reloadPending) {
                reloadPending = false;
                loadPage();
            }
        });
    }

    function markRead(button) {
        var id = parseInt(button.getAttribute('data-notification-id'), 10);
        if (!id || button.disabled) return;
        button.disabled = true;
        sendRequest({
            type: 'POST',
            url: endpoint + '?action=markRead&id=' + encodeURIComponent(id),
            dataType: 'json',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        }).done(function (response) {
            if (!response || response.Code !== '00') {
                button.disabled = false;
                showError('Không thể cập nhật trạng thái thông báo.');
                return;
            }

            var row = button.closest ? button.closest('tr') : button.parentNode.parentNode;
            if (currentStatus === 0 && row) row.classList.add('is-removing');
            window.setTimeout(function () {
                dispatchNotificationsChanged();
                loadPage();
            }, currentStatus === 0 ? 220 : 0);
        }).fail(function () {
            button.disabled = false;
            showError('Không thể cập nhật trạng thái thông báo.');
        });
    }

    for (var i = 0; i < filterButtons.length; i++) {
        filterButtons[i].addEventListener('click', function () {
            if (requestInFlight) return;
            currentStatus = parseInt(this.getAttribute('data-status'), 10);
            currentPage = 1;
            for (var j = 0; j < filterButtons.length; j++) {
                var active = filterButtons[j] === this;
                filterButtons[j].classList.toggle('is-active', active);
                filterButtons[j].setAttribute('aria-pressed', active ? 'true' : 'false');
            }
            loadPage();
        });
    }

    rows.addEventListener('click', function (event) {
        var button = event.target;
        while (button && button !== rows && !button.classList.contains('notifications-row-read')) button = button.parentNode;
        if (button && button !== rows) markRead(button);
    });

    pager.addEventListener('click', function (event) {
        var button = event.target;
        while (button && button !== pager && !button.getAttribute('data-page')) button = button.parentNode;
        if (!button || button === pager || button.disabled || requestInFlight) return;
        currentPage = Math.max(1, parseInt(button.getAttribute('data-page'), 10) || 1);
        loadPage();
    });

    refreshButton.addEventListener('click', loadPage);
    markAllButton.addEventListener('click', function () {
        if (markAllButton.disabled || unreadCount === 0) return;
        markAllButton.disabled = true;
        sendRequest({
            type: 'POST',
            url: endpoint + '?action=markAllRead',
            dataType: 'json',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        }).done(function (response) {
            if (!response || response.Code !== '00') {
                showError('Không thể đánh dấu đọc tất cả thông báo.');
                return;
            }
            currentPage = 1;
            dispatchNotificationsChanged();
            loadPage();
        }).fail(function () {
            showError('Không thể đánh dấu đọc tất cả thông báo.');
        }).always(function () {
            markAllButton.disabled = unreadCount === 0 || requestInFlight;
        });
    });

    loadPage();
})(window.jQuery);
