(function () {
    'use strict';

    var page = document.getElementById('aiReportsPage');
    if (!page) return;

    var allItems = [];
    var filteredItems = [];
    var currentPage = 1;
    var pageSize = 20;

    function byId(id) { return document.getElementById(id); }
    function text(value) { return value == null || value === '' ? '--' : String(value); }
    function number(value, digits) {
        if (value == null || value === '') return '--';
        var parsed = Number(value);
        if (!isFinite(parsed)) return '--';
        return parsed.toLocaleString('vi-VN', { maximumFractionDigits: digits == null ? 0 : digits });
    }
    function duration(value) {
        var ms = Number(value);
        if (!isFinite(ms)) return '--';
        return ms >= 1000 ? number(ms / 1000, 1) + ' giây' : number(ms, 0) + ' ms';
    }
    function dateTime(value) {
        if (!value) return '--';
        var date = new Date(value);
        return isNaN(date.getTime()) ? value : date.toLocaleString('vi-VN', { hour12: false });
    }
    function escapeHtml(value) {
        return String(value == null ? '' : value).replace(/[&<>"']/g, function (char) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[char];
        });
    }
    function unwrap(result) {
        var value = result && Object.prototype.hasOwnProperty.call(result, 'd') ? result.d : result;
        return typeof value === 'string' ? JSON.parse(value) : value;
    }
    function post(payload) {
        return fetch(page.getAttribute('data-endpoint'), {
            method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(payload)
        }).then(function (response) {
            return response.json().then(function (result) {
                if (!response.ok || (result && result.Message)) {
                    throw new Error((result && (result.Message || result.ExceptionType)) || ('HTTP ' + response.status));
                }
                return unwrap(result);
            });
        });
    }
    function setConnection(state, label) {
        var box = document.querySelector('.ai-report-live');
        box.classList.remove('is-online', 'is-error');
        if (state) box.classList.add(state);
        byId('aiConnectionState').textContent = label;
    }
    function showError(message) {
        var box = byId('aiReportError');
        box.textContent = message;
        box.hidden = false;
        setConnection('is-error', 'Mất kết nối');
    }
    function hideError() { byId('aiReportError').hidden = true; }
    function setKpis(summary) {
        summary = summary || {};
        byId('aiTotalRequests').textContent = number(summary.total_requests);
        byId('aiSuccessfulRequests').textContent = number(summary.successful_requests);
        byId('aiFailedRequests').textContent = number(summary.failed_requests);
        byId('aiSuccessRate').textContent = number(summary.success_rate_percent, 1) + '% tỷ lệ thành công';
        byId('aiAverageDuration').textContent = duration(summary.average_duration_ms);
        byId('aiP95Duration').textContent = 'P95: ' + duration(summary.p95_duration_ms);
        byId('aiAverageSql').textContent = duration(summary.average_sql_execution_ms);
        byId('aiGeneratedSql').textContent = number(summary.requests_with_generated_sql) + ' yêu cầu sinh SQL';
        byId('aiSqlRate').textContent = number(summary.sql_generation_rate_percent, 1) + '% có SQL';
    }
    function fillSelect(id, values, allLabel) {
        var select = byId(id);
        var selected = select.value;
        var unique = {};
        values.forEach(function (value) { if (value) unique[value] = true; });
        select.innerHTML = '<option value="ALL">' + escapeHtml(allLabel) + '</option>' + Object.keys(unique).sort().map(function (value) {
            return '<option value="' + escapeHtml(value) + '">' + escapeHtml(value) + '</option>';
        }).join('');
        if (unique[selected]) select.value = selected;
    }
    function populateFilters() {
        fillSelect('aiUserFilter', allItems.map(function (item) { return item.user_id; }), 'Tất cả người dùng');
        fillSelect('aiModelFilter', allItems.map(function (item) { return item.model; }), 'Tất cả model');
    }
    function matches(item, keyword) {
        if (!keyword) return true;
        return [item.question, item.answer, item.generated_sql, item.user_id, item.request_id, item.conversation_id, item.report_id, item.error]
            .join(' ').toLocaleLowerCase('vi-VN').indexOf(keyword) >= 0;
    }
    function applyFilters() {
        var keyword = byId('aiSearch').value.trim().toLocaleLowerCase('vi-VN');
        var status = byId('aiStatusFilter').value;
        var user = byId('aiUserFilter').value;
        var model = byId('aiModelFilter').value;
        filteredItems = allItems.filter(function (item) {
            return matches(item, keyword) &&
                (status === 'ALL' || (status === 'SUCCESS' ? item.success === true : item.success !== true)) &&
                (user === 'ALL' || item.user_id === user) &&
                (model === 'ALL' || item.model === model);
        });
        currentPage = 1;
        renderTable();
    }
    function statusBadge(item) {
        return item.success
            ? '<span class="ai-status-badge success">Thành công</span>'
            : '<span class="ai-status-badge failed">Thất bại</span>';
    }
    function renderTable() {
        var pages = Math.max(1, Math.ceil(filteredItems.length / pageSize));
        if (currentPage > pages) currentPage = pages;
        var start = (currentPage - 1) * pageSize;
        var rows = filteredItems.slice(start, start + pageSize);
        byId('aiReportRows').innerHTML = rows.length ? rows.map(function (item, index) {
            var itemIndex = allItems.indexOf(item);
            return '<tr><td>' + (start + index + 1) + '</td><td>' + escapeHtml(dateTime(item.timestamp)) + '</td>' +
                '<td>' + escapeHtml(text(item.user_id)) + '</td><td class="ai-question-cell" title="' + escapeHtml(item.question) + '">' + escapeHtml(text(item.question)) + '</td>' +
                '<td>' + escapeHtml(text(item.model)) + '</td><td>' + statusBadge(item) + '</td><td>' + escapeHtml(duration(item.duration_ms)) + '</td>' +
                '<td>' + escapeHtml(number(item.rows_returned)) + '</td><td><button class="ai-detail-button" type="button" data-item-index="' + itemIndex + '" title="Xem chi tiết"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="9" class="ai-empty-cell"><i class="fa fa-inbox"></i> Không có dữ liệu phù hợp.</td></tr>';
        byId('aiResultSummary').textContent = 'Hiển thị ' + number(filteredItems.length) + ' / ' + number(allItems.length) + ' yêu cầu';
        byId('aiPageInfo').textContent = 'Trang ' + currentPage + '/' + pages;
        byId('aiPreviousPage').disabled = currentPage <= 1;
        byId('aiNextPage').disabled = currentPage >= pages;
    }
    function meta(label, value) {
        return '<div class="ai-detail-meta"><span>' + escapeHtml(label) + '</span><strong>' + escapeHtml(text(value)) + '</strong></div>';
    }
    function section(title, value, cssClass) {
        if (value == null || value === '') return '';
        return '<section class="ai-detail-section"><h3>' + escapeHtml(title) + '</h3><div class="ai-detail-content ' + (cssClass || '') + '">' + escapeHtml(value) + '</div></section>';
    }
    function toolSummary(item) {
        var executions = item.tool_executions || [];
        if (!executions.length) return '';
        return executions.map(function (execution, index) {
            return 'Lần ' + (index + 1) + ': ' + text(execution.name) + ' · ' + (execution.success ? 'Thành công' : 'Thất bại') +
                ' · ' + duration(execution.execution_time_ms) + ' · ' + number(execution.row_count) + ' dòng' +
                (execution.error ? '\nLỗi: ' + execution.error : '');
        }).join('\n\n');
    }
    function openDetail(index) {
        var item = allItems[index];
        if (!item) return;
        var badge = byId('aiDetailStatus');
        badge.className = 'ai-status-badge ' + (item.success ? 'success' : 'failed');
        badge.textContent = item.success ? 'Thành công' : 'Thất bại';
        byId('aiDetailBody').innerHTML = '<div class="ai-detail-grid">' +
            meta('Thời gian', dateTime(item.timestamp)) + meta('Người dùng', item.user_id) + meta('Model', item.model) + meta('Thời lượng', duration(item.duration_ms)) +
            meta('Request ID', item.request_id) + meta('Conversation ID', item.conversation_id) + meta('Số dòng', number(item.rows_returned)) + meta('SQL time', duration(item.sql_execution_time_ms)) + '</div>' +
            section('Câu hỏi', item.question) + section('SQL được sinh', item.generated_sql, 'ai-sql-content') +
            section('Câu trả lời', item.answer) + section('Thực thi công cụ', toolSummary(item)) + section('Lỗi', item.error, 'ai-error-content');
        byId('aiDetailModal').hidden = false;
        document.body.style.overflow = 'hidden';
    }
    function closeDetail() {
        byId('aiDetailModal').hidden = true;
        document.body.style.overflow = '';
    }
    function load() {
        var button = byId('aiRefreshButton');
        button.disabled = true;
        button.innerHTML = '<span class="ai-inline-spinner"></span> Đang tải';
        hideError();
        setConnection('', 'Đang kết nối');
        return post({ limit: 500, offset: 0 }).then(function (data) {
            data = data || {};
            allItems = data.items || [];
            setKpis(data.summary);
            populateFilters();
            applyFilters();
            setConnection('is-online', 'Hoạt động');
            byId('aiLastUpdated').textContent = 'Cập nhật: ' + new Date().toLocaleString('vi-VN', { hour12: false });
        }).catch(function (error) {
            allItems = [];
            filteredItems = [];
            renderTable();
            showError(error.message || 'Không thể tải dữ liệu báo cáo AI.');
        }).then(function () {
            button.disabled = false;
            button.innerHTML = '<i class="fa fa-refresh"></i> Làm mới';
        });
    }

    // Notification IDs belong to ATFM. The backend resolves access and the upstream job ID.
    function openNotificationJob(notificationId) {
        byId('aiLegacyReports').hidden = true;
        byId('aiJobPanel').hidden = false;
        var resultData = null;
        var resultPage = 1;
        var resultPageSize = 50;
        var jobVersion = 0;
        var loadingResult = false;

        function notifyChanged() {
            var event = document.createEvent('Event');
            event.initEvent('atfm:notifications-changed', true, false);
            document.dispatchEvent(event);
        }
        function request(action, method) {
            return fetch(page.getAttribute('data-notification-endpoint') + '?action=' + action + '&id=' + encodeURIComponent(notificationId), {
                method: method || 'GET', credentials: 'same-origin', cache: 'no-store',
                headers: { 'Accept': 'application/json', 'X-Requested-With': 'XMLHttpRequest' }
            }).then(function (response) {
                return response.json().catch(function () {
                    throw new Error('Không nhận được dữ liệu hợp lệ. Vui lòng kiểm tra phiên đăng nhập và thử lại.');
                }).then(function (data) {
                    if (!response.ok || !data || data.Code !== '00') {
                        var messages = {
                            401: 'Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.',
                            403: 'Bạn không có quyền xem yêu cầu AI này.',
                            404: 'Không tìm thấy thông báo hoặc yêu cầu AI.',
                            409: 'Yêu cầu chưa hoàn thành thành công. Hãy làm mới trạng thái.',
                            410: 'Kết quả đã hết hạn hoặc không còn khả dụng.',
                            502: 'Chưa thể kết nối dịch vụ AI. Vui lòng thử lại.',
                            503: 'Dịch vụ AI hiện chưa sẵn sàng. Vui lòng thử lại sau.'
                        };
                        var error = new Error((data && data.Message) || messages[response.status] || 'Không thể tải dữ liệu yêu cầu AI.');
                        error.status = response.status;
                        throw error;
                    }
                    return data;
                });
            });
        }
        function showJobError(id, message) {
            var box = byId(id);
            box.textContent = message;
            box.hidden = false;
        }
        function setResultButton(loading) {
            loadingResult = loading;
            byId('aiJobShowResult').disabled = loading;
            byId('aiJobShowResult').innerHTML = loading ? '<span class="ai-inline-spinner"></span> Đang tải kết quả' : '<i class="fa fa-table"></i> Xem kết quả';
        }
        function renderJob(job) {
            var statuses = {
                queued: { label: 'Đang chờ', css: 'pending', notice: 'Yêu cầu đang chờ xử lý. Chọn Làm mới để cập nhật trạng thái.' },
                running: { label: 'Đang xử lý', css: 'pending', notice: 'AI đang xử lý yêu cầu. Chọn Làm mới để cập nhật trạng thái.' },
                succeeded: { label: 'Thành công', css: 'success', notice: 'Yêu cầu đã hoàn thành.' },
                failed: { label: 'Thất bại', css: 'failed', notice: 'Yêu cầu xử lý thất bại và không có kết quả để xem.' },
                cancelled: { label: 'Đã hủy', css: 'cancelled', notice: 'Yêu cầu đã bị hủy và không có kết quả để xem.' }
            };
            var status = Object.prototype.hasOwnProperty.call(statuses, job.status) ? statuses[job.status] : { label: 'Chưa xác định', css: 'pending', notice: 'Chưa xác định được trạng thái yêu cầu. Vui lòng làm mới.' };
            var expiry = job.result_expires_at ? new Date(job.result_expires_at).getTime() : NaN;
            var available = job.status === 'succeeded' && job.result_available === true && (isNaN(expiry) || expiry > Date.now());
            byId('aiJobStatus').className = 'ai-status-badge ' + status.css;
            byId('aiJobStatus').textContent = status.label;
            byId('aiJobNotice').textContent = job.status === 'succeeded' && !available ? 'Kết quả đã hết hạn hoặc không còn khả dụng.' : status.notice;
            byId('aiJobMetadata').innerHTML = meta('Mã yêu cầu', job.id) + meta('Người yêu cầu', job.requested_by) +
                meta('Tạo lúc', dateTime(job.created_at)) + meta('Cập nhật lúc', dateTime(job.updated_at)) +
                meta('Hoàn thành lúc', dateTime(job.completed_at)) + meta('Hết hạn kết quả', dateTime(job.result_expires_at)) +
                meta('Số dòng', number(job.row_count)) + meta('Số cột', number(job.column_count));
            byId('aiJobQuestion').textContent = text(job.question);
            byId('aiJobFailure').hidden = !job.error;
            byId('aiJobFailureText').textContent = job.error || '';
            byId('aiJobLimit').hidden = job.row_limit_reached !== true;
            byId('aiJobShowResult').hidden = !available;
            byId('aiJobResultHint').hidden = !available;
            byId('aiJobDetails').hidden = false;
        }
        function loadJob() {
            var version = ++jobVersion;
            byId('aiJobRefresh').disabled = true;
            byId('aiJobLoading').hidden = false;
            byId('aiJobDetails').hidden = true;
            byId('aiJobError').hidden = true;
            byId('aiJobReadWarning').hidden = true;
            byId('aiJobResultError').hidden = true;
            byId('aiJobResultPanel').hidden = true;
            byId('aiJobResultRows').innerHTML = '';
            byId('aiJobResultColumns').innerHTML = '';
            resultData = null;
            setResultButton(false);
            return request('aiJob').then(function (response) {
                if (version !== jobVersion) return;
                if (!response.Job || !response.Job.id) throw new Error('Dịch vụ chưa trả về chi tiết yêu cầu AI hợp lệ.');
                renderJob(response.Job);
                // Read state remains per user inside ATFM; no upstream acknowledgement is sent.
                request('markRead', 'POST').then(notifyChanged).catch(function () {
                    if (version === jobVersion) showJobError('aiJobReadWarning', 'Đã mở chi tiết nhưng chưa cập nhật được trạng thái đã đọc. Chọn Làm mới để thử lại.');
                });
            }).catch(function (error) {
                if (version === jobVersion) showJobError('aiJobError', error.message || 'Không thể tải chi tiết yêu cầu AI.');
            }).then(function () {
                if (version !== jobVersion) return;
                byId('aiJobRefresh').disabled = false;
                byId('aiJobLoading').hidden = true;
            });
        }
        function cellText(value) {
            if (value == null) return '';
            return typeof value === 'object' ? JSON.stringify(value) : String(value);
        }
        function renderResultPage() {
            if (!resultData) return;
            var rows = resultData.rows;
            var columns = resultData.columns;
            var pages = Math.max(1, Math.ceil(rows.length / resultPageSize));
            resultPage = Math.max(1, Math.min(resultPage, pages));
            var start = (resultPage - 1) * resultPageSize;
            byId('aiJobResultColumns').innerHTML = '<tr><th scope="col">STT</th>' + columns.map(function (column) { return '<th scope="col">' + escapeHtml(column) + '</th>'; }).join('') + '</tr>';
            byId('aiJobResultRows').innerHTML = rows.length ? rows.slice(start, start + resultPageSize).map(function (row, index) {
                return '<tr><td>' + (start + index + 1) + '</td>' + columns.map(function (column) {
                    var value = row && Object.prototype.hasOwnProperty.call(row, column) ? row[column] : null;
                    return '<td>' + escapeHtml(cellText(value)) + '</td>';
                }).join('') + '</tr>';
            }).join('') : '<tr><td colspan="' + (columns.length + 1) + '" class="ai-empty-cell">Truy vấn không có dòng kết quả.</td></tr>';
            byId('aiJobResultSummary').textContent = 'Đã tải ' + number(rows.length) + ' dòng · ' + number(columns.length) + ' cột' + (resultData.row_limit_reached === true ? ' · Đã đạt giới hạn số dòng, dữ liệu có thể chưa đầy đủ.' : '');
            byId('aiJobResultPageInfo').textContent = 'Trang ' + resultPage + '/' + pages + (rows.length ? ' · Dòng ' + number(start + 1) + '–' + number(Math.min(start + resultPageSize, rows.length)) : '');
            byId('aiJobResultPrevious').disabled = resultPage <= 1;
            byId('aiJobResultNext').disabled = resultPage >= pages;
            byId('aiJobResultPanel').hidden = false;
        }
        function loadResult() {
            if (loadingResult || byId('aiJobShowResult').hidden) return;
            if (resultData) { renderResultPage(); return; }
            var version = jobVersion;
            byId('aiJobResultError').hidden = true;
            setResultButton(true);
            request('aiResult').then(function (response) {
                if (version !== jobVersion) return;
                var result = response.Result;
                if (!result || !Array.isArray(result.rows) || !Array.isArray(result.columns) || !result.columns.every(function (column) { return typeof column === 'string'; })) {
                    throw new Error('Dữ liệu kết quả không đúng định dạng. Vui lòng thử lại.');
                }
                resultData = result;
                resultPage = 1;
                renderResultPage();
                byId('aiJobResultHint').hidden = true;
                byId('aiJobShowResult').hidden = true;
            }).catch(function (error) {
                if (version !== jobVersion) return;
                showJobError('aiJobResultError', error.message || 'Không thể tải kết quả yêu cầu AI.');
                if (error.status === 410 || error.status === 409) {
                    byId('aiJobShowResult').hidden = true;
                    byId('aiJobResultHint').hidden = true;
                    byId('aiJobNotice').textContent = error.status === 410 ? 'Kết quả đã hết hạn hoặc không còn khả dụng.' : 'Kết quả chưa sẵn sàng. Chọn Làm mới để cập nhật trạng thái.';
                }
            }).then(function () { if (version === jobVersion) setResultButton(false); });
        }

        byId('aiJobRefresh').addEventListener('click', loadJob);
        byId('aiJobShowResult').addEventListener('click', loadResult);
        byId('aiJobResultPageSize').addEventListener('change', function () {
            resultPageSize = Math.min(100, Math.max(25, parseInt(this.value, 10) || 50));
            resultPage = 1;
            renderResultPage();
        });
        byId('aiJobResultPrevious').addEventListener('click', function () { resultPage--; renderResultPage(); });
        byId('aiJobResultNext').addEventListener('click', function () { resultPage++; renderResultPage(); });
        if (!/^[1-9][0-9]{0,18}$/.test(notificationId)) {
            byId('aiJobLoading').hidden = true;
            byId('aiJobRefresh').disabled = true;
            showJobError('aiJobError', 'Mã thông báo không hợp lệ. Vui lòng mở lại từ danh sách thông báo AI.');
            return;
        }
        loadJob();
    }

    var notificationMatch = /(?:^|[?&])notificationId=([^&]*)/i.exec(window.location.search);
    if (notificationMatch) {
        var notificationId;
        try { notificationId = decodeURIComponent(notificationMatch[1]); } catch (error) { notificationId = ''; }
        openNotificationJob(notificationId);
        return;
    }

    var searchTimer;
    byId('aiSearch').addEventListener('input', function () { clearTimeout(searchTimer); searchTimer = setTimeout(applyFilters, 180); });
    ['aiStatusFilter', 'aiUserFilter', 'aiModelFilter'].forEach(function (id) { byId(id).addEventListener('change', applyFilters); });
    byId('aiPageSize').addEventListener('change', function () { pageSize = parseInt(this.value, 10) || 20; currentPage = 1; renderTable(); });
    byId('aiPreviousPage').addEventListener('click', function () { if (currentPage > 1) { currentPage--; renderTable(); } });
    byId('aiNextPage').addEventListener('click', function () { if (currentPage < Math.ceil(filteredItems.length / pageSize)) { currentPage++; renderTable(); } });
    byId('aiRefreshButton').addEventListener('click', load);
    byId('aiReportRows').addEventListener('click', function (event) { var button = event.target.closest('[data-item-index]'); if (button) openDetail(parseInt(button.getAttribute('data-item-index'), 10)); });
    byId('aiDetailModal').addEventListener('click', function (event) { if (event.target.closest('[data-close-modal]')) closeDetail(); });
    document.addEventListener('keydown', function (event) { if (event.key === 'Escape' && !byId('aiDetailModal').hidden) closeDetail(); });

    load();
}());
