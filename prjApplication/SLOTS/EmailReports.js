(function () {
    'use strict';
    var page = document.getElementById('emailReportsPage');
    if (!page) return;

    var endpoints = {
        email: page.getAttribute('data-email-endpoint'),
        incoming: page.getAttribute('data-incoming-endpoint'),
        emailSummary: page.getAttribute('data-email-summary-endpoint'),
        incomingSummary: page.getAttribute('data-incoming-summary-endpoint'),
        emailDetail: page.getAttribute('data-email-detail-endpoint'),
        incomingDetail: page.getAttribute('data-incoming-detail-endpoint'),
        job: page.getAttribute('data-job-endpoint'),
        sendReport: page.getAttribute('data-send-report-endpoint')
    };
    var sourceMode = 'email', currentPage = 1, pageSize = 50;
    var rows = [], totalItems = 0, totalPages = 1, summary = null, snapshotId = '';
    var loadRequestId = 0, detailRequestId = 0, loading = false, reportBusy = false;
    var appliedFilters = null, forceReload = false;
    var viewCache = {}, viewCacheOrder = [], VIEW_CACHE_MAX = 20, VIEW_CACHE_TTL = 5 * 60 * 1000;
    var inFlight = {}, activeAbort = null, activeLoad = null, generation = 0, snapshotAt = 0;
    var detailCache = new Map(), jobCache = new Map();
    var displayed = null;
    var sourceStates = { email: null, incoming: null };
    var $ = function (id) { return document.getElementById(id); };

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : String(value);
        return node.innerHTML.replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function text(item, keys) {
        for (var i = 0; i < keys.length; i++) if (item && item[keys[i]] != null) return String(item[keys[i]]);
        return '';
    }
    function unwrap(payload) {
        var value = payload && payload.d != null ? payload.d : payload;
        if (typeof value === 'string') value = JSON.parse(value);
        return value;
    }
    function errorMessage(payload, fallback) {
        var value = payload;
        if (value && value.d != null) value = value.d;
        if (typeof value === 'string') {
            try { value = JSON.parse(value); } catch (ignore) { return value || fallback; }
        }
        return value && (value.Message || value.message || value.detail || value.title) || fallback;
    }
    function postJson(url, body, signal) {
        var controller = window.AbortController ? new window.AbortController() : null;
        var timeout = /Summary/.test(url) ? 40000 : /Detail/.test(url) ? 100000 : url === endpoints.job ? 40000 : 315000;
        return new Promise(function (resolve, reject) {
            var timer, finished = false;
            function finish(error, value) {
                if (finished) return;
                finished = true; clearTimeout(timer);
                if (signal) signal.removeEventListener('abort', cancel);
                if (error) reject(error); else resolve(value);
            }
            function cancel() {
                if (controller) controller.abort();
                var error = new Error('Request cancelled'); error.name = 'AbortError'; finish(error);
            }
            if (signal && signal.aborted) { cancel(); return; }
            if (signal) signal.addEventListener('abort', cancel);
            timer = setTimeout(function () {
                if (controller) controller.abort();
                finish(new Error('API phản hồi quá thời gian chờ. Vui lòng thử lại.'));
            }, timeout);
            var options = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' }, body: JSON.stringify(body) };
            if (controller) options.signal = controller.signal;
            fetch(url, options).then(function (response) {
                return response.json().catch(function () { return null; }).then(function (payload) {
                    if (!response.ok) {
                        var error = new Error(errorMessage(payload, 'HTTP ' + response.status)); error.status = response.status; throw error;
                    }
                    return unwrap(payload);
                });
            }).then(function (value) { finish(null, value); }, finish);
        });
    }
    function cacheGet(key) {
        var entry = viewCache[key];
        if (!entry || Date.now() - entry.createdAt >= VIEW_CACHE_TTL) {
            if (entry) delete viewCache[key];
            viewCacheOrder = viewCacheOrder.filter(function (item) { return item !== key; });
            return null;
        }
        viewCacheOrder = viewCacheOrder.filter(function (item) { return item !== key; });
        viewCacheOrder.push(key);
        return entry;
    }
    function cachePut(key, value) {
        viewCache[key] = value;
        viewCacheOrder = viewCacheOrder.filter(function (item) { return item !== key; });
        viewCacheOrder.push(key);
        while (viewCacheOrder.length > VIEW_CACHE_MAX) delete viewCache[viewCacheOrder.shift()];
    }
    function requestOnce(key, url, body, signal) {
        if (inFlight[key]) return inFlight[key];
        inFlight[key] = postJson(url, body, signal).then(function (value) {
            delete inFlight[key]; return value;
        }, function (error) {
            delete inFlight[key]; throw error;
        });
        return inFlight[key];
    }
    function captureView() {
        return { filters: appliedFilters, page: currentPage, size: pageSize, summary: summary,
            snapshotId: snapshotId, snapshotAt: snapshotAt, generation: generation, at: displayed && displayed.at || Date.now(),
            rows: rows.slice(), totalItems: totalItems, totalPages: totalPages };
    }
    function applyView(view) {
        appliedFilters = view.filters; currentPage = view.page; pageSize = view.size;
        summary = view.summary; snapshotId = view.snapshotId; snapshotAt = view.snapshotAt;
        generation = view.generation; rows = view.rows.slice(); totalItems = view.totalItems; totalPages = view.totalPages;
        $('emailPageSize').value = String(pageSize);
    }
    function writeFilters(filters) {
        $('emailSearch').value = filters.query || ''; $('emailStatus').value = filters.processingStatus || '';
        $('emailFileType').value = filters.fileType;
        $('emailFrom').value = (filters.fromDate || '').slice(0, 10);
        $('emailTo').value = (filters.toDate || '').slice(0, 10);
    }
    function rememberSourceState() {
        if (displayed) sourceStates[sourceMode] = displayed;
    }
    function restoreSourceState(mode) {
        displayed = sourceStates[mode];
        if (displayed) { applyView(displayed); writeFilters(displayed.filters); return; }
        appliedFilters = null; currentPage = 1; pageSize = 50; summary = null; snapshotId = ''; snapshotAt = 0;
        rows = []; totalItems = 0; totalPages = 1;
        writeFilters({ query: '', processingStatus: '', fileType: 'word', fromDate: '', toDate: '' });
        $('emailPageSize').value = '50';
    }
    function cachedDetail(cache, key, url, body) {
        var item = cache.get(key);
        if (item && Date.now() - item.at < 120000) {
            cache.delete(key); cache.set(key, item); return Promise.resolve(item.value);
        }
        cache.delete(key);
        return requestOnce(key, url, body).then(function (value) {
            if (!value) throw new Error('API không trả dữ liệu chi tiết.');
            cache.set(key, { at: Date.now(), value: value });
            while (cache.size > 30) cache.delete(cache.keys().next().value);
            return value;
        });
    }
    function status(item) { return text(item, ['processingStatus','status','state','mailStatus','result']) || 'Không xác định'; }
    function date(item) { return text(item, ['receivedAt','sentAt','createdAt','date','emailDate','created_at','timestamp']); }
    function dateKey(value) {
        var match = String(value || '').match(/(20\d\d)[-\/]?(\d\d)[-\/]?(\d\d)/);
        return match ? match[1] + '-' + match[2] + '-' + match[3] : '';
    }
    function attachmentName(item) {
        var name = text(item, ['storedFileName','attachmentName']);
        var attachments = item && item.attachments;
        if (!name && Array.isArray(attachments) && attachments.length)
            name = typeof attachments[0] === 'string' ? attachments[0] : text(attachments[0], ['storedFileName','attachmentName']);
        return name;
    }
    function storageFolder(item) {
        var normalized = status(item).trim().toUpperCase();
        if (normalized === 'SAVED' || normalized === 'SKIPPED') return 'processed';
        if (normalized === 'FAILED' || normalized === 'ERROR')
            return text(item, ['errorMessage']).trim() === 'File Word chưa có format YAML phù hợp' ? 'error-format' : 'error';
        if (normalized === 'QUARANTINED') return 'quarantine';
        return '';
    }
    function downloadUrl(item) {
        var fileName = attachmentName(item), itemDate = dateKey(date(item)), folder = storageFolder(item);
        if (!fileName || !itemDate || !folder) return '';
        return 'http://172.29.79.49/vatm-storage/' + folder + '/' + itemDate.replace(/-/g, '/') + '/' + encodeURIComponent(fileName);
    }
    function statusClass(value) {
        var normalized = String(value).toLowerCase();
        return /fail|error|reject/.test(normalized) ? 'failed' : /pending|wait|queue/.test(normalized) ? 'pending' : /sent|success|deliver|complete|ok/.test(normalized) ? 'success' : '';
    }
    function databaseState(item) {
        var state = text(item, ['databaseState','DatabaseState']).toLowerCase();
        if (!state && (item.hasDatabaseData === true || item.HasDatabaseData === true)) state = 'found';
        return /^(found|missing|error|pending|unknown)$/.test(state) ? state : 'unknown';
    }
    function oper(item) { return text(item, ['operName','operatorName','airlineName','OPER_NAME','oper','OPER','airline','carrier']) || '--'; }
    function sourceLabel() { return sourceMode === 'incoming' ? 'Incoming' : 'email'; }
    function setConnection(ok, message) {
        $('emailConnectionState').textContent = message;
        page.querySelector('.email-live').className = 'email-live ' + (ok ? 'is-online' : 'is-error');
    }
    function readFilters() {
        return {
            query: $('emailSearch').value.trim(), processingStatus: $('emailStatus').value,
            fileType: $('emailFileType').value,
            fromDate: $('emailFrom').value ? $('emailFrom').value + 'T00:00:00' : '',
            toDate: $('emailTo').value ? $('emailTo').value + 'T23:59:59' : ''
        };
    }
    function requestBody(filters, pageNumber, size, snapshot) {
        return {
            query: filters.query, processingStatus: filters.processingStatus, fileType: filters.fileType,
            fromDate: filters.fromDate, toDate: filters.toDate, snapshotId: snapshot || '',
            page: pageNumber, size: size
        };
    }
    function summaryBody(filters) {
        return { query: filters.query, processingStatus: filters.processingStatus, fileType: filters.fileType,
            fromDate: filters.fromDate, toDate: filters.toDate };
    }
    function listEndpoint() { return sourceMode === 'incoming' ? endpoints.incoming : endpoints.email; }
    function summaryEndpoint() { return sourceMode === 'incoming' ? endpoints.incomingSummary : endpoints.emailSummary; }
    function detailEndpoint() { return sourceMode === 'incoming' ? endpoints.incomingDetail : endpoints.emailDetail; }
    function cacheKey(filters, pageNumber, size) { return sourceMode + '|' + JSON.stringify(filters) + '|' + pageNumber + '|' + size; }

    function normalizeSummary(value) {
        if (!value || value.totalFiles == null) return null;
        var result = {
            totalFiles: Number(value.totalFiles), successFiles: Number(value.successFiles),
            errorFiles: Number(value.errorFiles), pendingFiles: Number(value.pendingFiles),
            unknownFiles: Number(value.unknownFiles), snapshotId: text(value, ['snapshotId','SnapshotId'])
        };
        if (![result.totalFiles,result.successFiles,result.errorFiles,result.pendingFiles,result.unknownFiles].every(function (n) { return isFinite(n) && n >= 0 && Math.floor(n) === n; })) return null;
        if (result.totalFiles !== result.successFiles + result.errorFiles + result.pendingFiles + result.unknownFiles) return null;
        return result;
    }
    function draw() {
        var start = (currentPage - 1) * pageSize;
        $('emailRows').innerHTML = rows.length ? rows.map(function (item, index) {
            var state = status(item), subject = text(item,['subject','title']) || '(Không có tiêu đề)';
            var sender = text(item,['sender','from','senderEmail']), errorMsg = text(item,['errorMessage']);
            var dbState = databaseState(item);
            var rowClass = dbState === 'found' ? 'email-row--has-data' : /^(missing|error)$/.test(dbState) ? 'email-row--no-data' : '';
            var dbTitle = dbState === 'found' ? 'Có dữ liệu trong database' : dbState === 'pending' ? 'Đang chờ xử lý' : dbState === 'unknown' ? 'Chưa xác minh được database' : 'Không có dữ liệu trong database';
            return '<tr class="' + rowClass + '" title="' + dbTitle + '"><td>' + (start + index + 1) + '</td>' +
                '<td><span class="email-cell-clamp" title="' + esc(date(item)) + '">' + esc(date(item)) + '</span></td>' +
                '<td class="email-subject" title="' + esc(subject) + '"><span class="email-cell-clamp">' + esc(subject) + '</span></td>' +
                '<td><span class="email-cell-clamp" title="' + esc(sender) + '">' + esc(sender) + '</span></td>' +
                '<td><span class="email-cell-clamp" title="' + esc(errorMsg) + '">' + esc(errorMsg) + '</span></td>' +
                '<td><span class="email-status ' + statusClass(state) + '">' + esc(state) + '</span></td>' +
                '<td><button type="button" class="email-detail-button" data-index="' + index + '" title="Xem chi tiết email"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="7" class="email-empty-cell">' + (loading ? 'Đang tải dữ liệu...' : 'Không có email phù hợp.') + '</td></tr>';
        Array.prototype.forEach.call(document.querySelectorAll('.email-detail-button'), function (button) {
            button.onclick = function () { showDetail(rows[parseInt(this.getAttribute('data-index'), 10)]); };
        });
        $('emailTotal').textContent = displayed || rows.length ? totalItems.toLocaleString('vi-VN') : '--';
        $('emailSent').textContent = summary ? summary.successFiles.toLocaleString('vi-VN') : '--';
        $('emailFailed').textContent = summary ? summary.errorFiles.toLocaleString('vi-VN') : '--';
        $('emailPageLabel').textContent = currentPage;
        $('emailPageInfo').textContent = 'Trang ' + currentPage + '/' + totalPages + ' · ' + totalItems.toLocaleString('vi-VN') + ' ' + sourceLabel();
        $('emailTableInfo').textContent = totalItems ? 'Hiển thị ' + (start + 1) + '–' + (start + rows.length) + ' / ' + totalItems.toLocaleString('vi-VN') + ' bản ghi sau lọc' : '0 bản ghi sau lọc';
        $('emailPrev').disabled = loading || currentPage <= 1;
        $('emailNext').disabled = loading || currentPage >= totalPages;
        $('emailReportDownload').disabled = loading || !rows.length || reportBusy;
        $('emailReportSend').disabled = loading || !rows.length || reportBusy;
        if (summary) $('emailStatsInfo').textContent = 'KPI trên toàn bộ ' + summary.totalFiles.toLocaleString('vi-VN') + ' bản ghi sau lọc.' +
            (summary.pendingFiles ? ' Đang chờ: ' + summary.pendingFiles + '.' : '') +
            (summary.unknownFiles ? ' Chưa xác minh DB: ' + summary.unknownFiles + '.' : '');
        else $('emailStatsInfo').textContent = 'Danh sách đã phân trang. KPI thành công/lỗi chưa khả dụng do API summary chưa phản hồi hợp lệ.';
    }

    function parseList(value) {
        var items = value && (value.content || value.items || value.data);
        if (!Array.isArray(items)) throw new Error('API trả dữ liệu danh sách không hợp lệ.');
        if (value.totalElements == null) throw new Error('API thiếu tổng số bản ghi.');
        var total = Number(value.totalElements), pages = Number(value.totalPages);
        if (!isFinite(total) || total < 0) throw new Error('API không trả tổng số bản ghi hợp lệ.');
        if (!isFinite(pages) || pages < 0) pages = Math.ceil(total / pageSize);
        return { items: items, total: total, pages: Math.max(1, pages) };
    }
    function loadPage(options) {
        options = options || {};
        var filters = options.keepFilters && appliedFilters ? appliedFilters : readFilters();
        if (filters.fromDate && filters.toDate && filters.fromDate > filters.toDate) {
            $('emailError').textContent = 'Từ ngày không được lớn hơn đến ngày.'; $('emailError').hidden = false;
            return Promise.resolve();
        }
        var targetPage = options.keepFilters ? currentPage : 1, targetSize = pageSize;
        var signature = JSON.stringify([sourceMode, filters, targetPage, targetSize]);
        var force = forceReload; forceReload = false;
        if (activeLoad && activeLoad.signature === signature) return activeLoad.promise;
        var requestId = ++loadRequestId;
        if (activeAbort) activeAbort.abort();
        activeAbort = window.AbortController ? new window.AbortController() : null;
        var controller = activeAbort;
        closeDetail();
        var validSnapshot = options.keepSnapshot && snapshotAt && Date.now() - snapshotAt < VIEW_CACHE_TTL;
        if (force || !validSnapshot || JSON.stringify(appliedFilters) !== JSON.stringify(filters)) {
            summary = null; snapshotId = ''; snapshotAt = 0; generation++;
        }
        var pendingSummary = summary, pendingSnapshot = snapshotId, pendingAt = snapshotAt, pendingGeneration = generation;
        var key = cacheKey(filters, targetPage - 1, targetSize) + '|' + pendingGeneration + '|' + pendingSnapshot;
        var cached = !force && validSnapshot && cacheGet(key);
        var source = sourceMode, listUrl = listEndpoint(), summaryUrl = summaryEndpoint();
        function complete(view, fromCache) {
            applyView(view); displayed = captureView(); displayed.at = view.at; rememberSourceState(); loading = false;
            $('emailError').hidden = true; draw(); setConnection(true, fromCache ? 'Đã kết nối (cache)' : 'Đã kết nối');
            $('emailLastUpdated').textContent = 'Dữ liệu lúc ' + new Date(view.at).toLocaleTimeString('vi-VN');
            $('emailApply').disabled = false;
            $('emailApply').innerHTML = '<i class="fa fa-search"></i> Tìm kiếm';
        }
        if (cached) { complete(cached, true); activeLoad = null; return Promise.resolve(); }
        // Only a completed view from this source may remain visible while loading.
        if (displayed) applyView(displayed);
        loading = true; draw(); $('emailError').hidden = true;
        if (!displayed) $('emailTotal').textContent = '--';
        $('emailStatsInfo').textContent = displayed ? 'Đang cập nhật; bảng và KPI đang hiển thị dữ liệu cũ.' : 'Đang tải dữ liệu...';
        $('emailApply').disabled = true; setConnection(false, 'Đang tải dữ liệu...');
        var promise = (pendingAt ? Promise.resolve(pendingSummary) :
            requestOnce('summary|' + requestId, summaryUrl, summaryBody(filters), controller && controller.signal)
                .then(function (value) { return normalizeSummary(value); })
                .catch(function (error) { if (error.name === 'AbortError') throw error; return null; }))
        .then(function (value) {
            if (requestId !== loadRequestId) return null;
            pendingSummary = value; pendingSnapshot = value && value.snapshotId || '';
            if (!pendingAt) pendingAt = Date.now();
            return requestOnce('list|' + requestId, listUrl, requestBody(filters, targetPage - 1, targetSize, pendingSnapshot), controller && controller.signal);
        }).then(function (value) {
            if (requestId !== loadRequestId || value == null) return;
            var list = parseList(value);
            if (pendingSummary && pendingSummary.totalFiles !== list.total) throw new Error('Summary và danh sách không cùng tổng dữ liệu. Hãy làm mới.');
            if (value.snapshotId && pendingSnapshot && value.snapshotId !== pendingSnapshot) throw new Error('Snapshot không khớp. Hãy làm mới.');
            if (targetPage > list.pages) throw new Error('Trang đã hết dữ liệu. Hãy làm mới để về trang đầu.');
            var view = { filters: filters, page: targetPage, size: targetSize, summary: pendingSummary,
                snapshotId: pendingSnapshot, snapshotAt: pendingAt, generation: pendingGeneration,
                rows: list.items, totalItems: list.total, totalPages: list.pages, at: Date.now(), createdAt: Date.now() };
            key = source + '|' + JSON.stringify(filters) + '|' + (targetPage - 1) + '|' + targetSize + '|' + pendingGeneration + '|' + pendingSnapshot;
            cachePut(key, view); complete(view, false);
        }).catch(function (error) {
            if (requestId !== loadRequestId) return;
            if (displayed) applyView(displayed);
            loading = false; draw();
            if (!displayed) $('emailTotal').textContent = '--';
            setConnection(false, 'Cập nhật thất bại');
            $('emailError').textContent = (displayed ? 'Đang hiển thị dữ liệu cũ. ' : '') + error.message + ' Bấm Làm mới để thử lại.';
            $('emailError').hidden = false;
            if (error.status === 410 || /snapshot/i.test(error.message)) snapshotAt = 0;
        }).then(function () {
            if (requestId !== loadRequestId) return;
            activeLoad = null; $('emailApply').disabled = false;
            $('emailApply').innerHTML = '<i class="fa fa-search"></i> Tìm kiếm';
        });
        activeLoad = { signature: signature, promise: promise };
        return promise;
    }

    function selectSource(mode) {
        rememberSourceState();
        sourceMode = mode === 'incoming' ? 'incoming' : 'email';
        var hadSourceState = !!sourceStates[sourceMode];
        restoreSourceState(sourceMode);
        Array.prototype.forEach.call(document.querySelectorAll('[data-report-source]'), function (button) {
            var active = button.getAttribute('data-report-source') === sourceMode;
            button.classList.toggle('is-active', active); button.setAttribute('aria-pressed', active ? 'true' : 'false');
        });
        $('emailTableTitle').textContent = sourceMode === 'incoming' ? 'Danh sách Incoming' : 'Danh sách email';
        loadPage(hadSourceState ? { keepFilters: true, keepSnapshot: true } : undefined);
    }

    function showDetail(item) {
        if (!item) return;
        ['emailPermissionLink','emailDownloadLink'].forEach(function (name) { $(name).removeAttribute('href'); $(name).classList.add('is-disabled'); $(name).setAttribute('aria-disabled','true'); });
        $('emailPermissionMessage').textContent = '';
        var requestId = ++detailRequestId, id = Number(text(item, ['id','ID']));
        $('emailDetailSubject').textContent = text(item,['subject','title']) || '(Không có tiêu đề)';
        $('emailDetailMeta').textContent = 'Đang tải chi tiết...'; $('emailDetailBody').textContent = '';
        $('emailDetailBackdrop').hidden = false;
        var detailPromise = id > 0 ? cachedDetail(detailCache, sourceMode + '|detail|' + id, detailEndpoint(), { id: id }).catch(function () { return item; }) : Promise.resolve(item);
        detailPromise.then(function (detail) {
            if (requestId !== detailRequestId) return;
            detail = detail || item;
            var syncJobId = text(detail, ['syncJobId']) || text(item, ['syncJobId']);
            var fileName = attachmentName(detail) || attachmentName(item), fileUrl = downloadUrl(detail) || downloadUrl(item);
            $('emailDetailSubject').textContent = text(detail,['subject','title']) || '(Không có tiêu đề)';
            $('emailDetailMeta').innerHTML = '<b>Người gửi:</b> ' + esc(text(detail,['sender','from','senderEmail'])) + '<br><b>Tệp đính kèm:</b> ' + esc(fileName) + '<br><b>Thời gian:</b> ' + esc(date(detail)) + '<br><b>Trạng thái xử lý:</b> ' + esc(status(detail)) + '<br><b>Thông báo lỗi:</b> ' + esc(text(detail,['errorMessage'])) + '<br><b>Sync Job ID:</b> ' + esc(syncJobId);
            $('emailDetailBody').textContent = text(detail,['body','content','message','text','html']) || JSON.stringify(detail, null, 2);
            var download = $('emailDownloadLink'); download.removeAttribute('href'); download.classList.add('is-disabled'); download.setAttribute('aria-disabled','true');
            if (fileUrl) { download.href = fileUrl; download.classList.remove('is-disabled'); download.removeAttribute('aria-disabled'); }
            loadPermission(syncJobId, requestId);
        });
    }
    function loadPermission(syncJobId, requestId) {
        var link = $('emailPermissionLink'), message = $('emailPermissionMessage');
        link.removeAttribute('href'); link.classList.add('is-disabled'); link.setAttribute('aria-disabled','true');
        message.className = 'email-permission-message';
        if (!syncJobId) { message.textContent = 'Email này không có syncJobId.'; message.classList.add('is-error'); return; }
        message.textContent = 'Đang lấy số phép bay...';
        cachedDetail(jobCache, sourceMode + '|job|' + syncJobId, endpoints.job, { syncJobId: syncJobId }).then(function (result) {
            if (requestId !== detailRequestId) return;
            if (!result || result.state !== 'found' || !result.targetPermId) {
                message.textContent = result && result.message || 'Chưa có dữ liệu số phép bay trong database.';
                message.classList.add(result && result.state === 'pending' ? 'is-pending' : 'is-error'); return;
            }
            var editPage = result.targetTable === 'NO' ? 'Edit_PermNO4Mail.aspx' : 'Edit_PermSC4Mail.aspx';
            link.href = '../Permission/' + editPage + '?Menu_ID=51&ID=' + encodeURIComponent(result.targetPermId);
            link.classList.remove('is-disabled'); link.removeAttribute('aria-disabled');
            message.textContent = 'Số phép bay: ' + result.targetPermId + (result.targetTable === 'NO' ? ' (ngày đơn)' : '');
        }).catch(function (error) {
            if (requestId !== detailRequestId) return;
            message.textContent = 'Không thể lấy số phép bay: ' + error.message; message.classList.add('is-error');
        });
    }
    function closeDetail() { detailRequestId++; $('emailDetailBackdrop').hidden = true; }

    function reportRows(items) {
        return items.map(function (item) { var state = databaseState(item); return { Oper: oper(item), FileName: attachmentName(item) || '--', Status: status(item), HasDatabaseData: state === 'found', DatabaseState: state }; });
    }
    function reportHtml(items) {
        var body = reportRows(items).map(function (row,index) {
            var state = row.HasDatabaseData ? row.Status + ' - CÓ DỮ LIỆU DB' : row.DatabaseState === 'pending' ? '[~] ĐANG CHỜ XỬ LÝ' : row.DatabaseState === 'unknown' ? '[?] CHƯA XÁC MINH DB' : '[!] KHÔNG CÓ DỮ LIỆU DB';
            return '<tr><td>'+(index+1)+'</td><td>'+esc(row.Oper)+'</td><td>'+esc(row.FileName)+'</td><td>'+esc(state)+'</td></tr>';
        }).join('');
        return '<html><head><meta charset="utf-8"><style>body{font-family:Arial}table{border-collapse:collapse;width:100%}th,td{border:1px solid #777;padding:7px}</style></head><body><h2>BÁO CÁO TRẠNG THÁI DỮ LIỆU EMAIL</h2><table><tr><th>STT</th><th>OPER</th><th>Tên file</th><th>Trạng thái</th></tr>'+body+'</table></body></html>';
    }
    function filteredReportRows() {
        var sender = $('emailReportSender').value.trim().toLowerCase();
        return rows.filter(function (item) { return !sender || text(item,['sender','from','senderEmail']).toLowerCase().indexOf(sender) >= 0; });
    }
    function validateReport() {
        if ($('emailReportSender').value.trim() && !$('emailReportSender').checkValidity()) { $('emailReportMessage').textContent='Email người gửi không hợp lệ.'; return false; }
        return true;
    }
    function downloadReport() {
        if (!validateReport()) return;
        var items=filteredReportRows(), message=$('emailReportMessage'); if(!items.length){message.textContent='Không có dữ liệu trên trang hiện tại.';return;}
        var format=$('emailReportFormat').value, blob=new Blob(['\ufeff',reportHtml(items)],{type:format==='word'?'application/msword':'application/vnd.ms-excel'});
        var link=document.createElement('a'); link.href=URL.createObjectURL(blob); link.download='Bao_cao_'+sourceLabel()+'_trang_'+currentPage+(format==='word'?'.doc':'.xls'); document.body.appendChild(link); link.click(); document.body.removeChild(link); URL.revokeObjectURL(link.href);
        message.className='email-report-message is-success'; message.textContent='Đã tạo báo cáo '+items.length+' dòng của trang hiện tại.';
    }
    function sendReport() {
        var recipient=$('emailReportRecipient').value.trim(), items=filteredReportRows(), message=$('emailReportMessage');
        if(!validateReport() || !recipient || !$('emailReportRecipient').checkValidity()){message.textContent='Vui lòng nhập Gmail người nhận hợp lệ.';return;}
        if(!items.length){message.textContent='Không có dữ liệu trên trang hiện tại.';return;}
        reportBusy=true; draw();
        postJson(endpoints.sendReport,{recipient:recipient,format:$('emailReportFormat').value,rows:reportRows(items)}).then(function(result){message.className='email-report-message is-success';message.textContent=result.message||'Đã gửi báo cáo.';}).catch(function(error){message.className='email-report-message is-error';message.textContent='Không thể gửi báo cáo: '+error.message;}).then(function(){reportBusy=false;draw();});
    }

    $('emailApply').onclick = function () { forceReload = true; loadPage(); };
    $('emailSearch').onkeydown = function (event) { if (event.key === 'Enter') { event.preventDefault(); forceReload = true; loadPage(); } };
    $('emailFileType').onchange = function () { forceReload = true; loadPage(); };
    $('emailRefresh').onclick = function () { forceReload = true; loadPage(); };
    $('emailPageSize').onchange = function () { pageSize = parseInt(this.value,10)||50; currentPage=1; loadPage({keepFilters:true,keepSnapshot:true}); };
    $('emailPrev').onclick = function () { if(currentPage>1){currentPage--;loadPage({keepFilters:true,keepSnapshot:true});} };
    $('emailNext').onclick = function () { if(currentPage<totalPages){currentPage++;loadPage({keepFilters:true,keepSnapshot:true});} };
    $('emailReportDownload').onclick=downloadReport; $('emailReportSend').onclick=sendReport;
    Array.prototype.forEach.call(document.querySelectorAll('[data-report-source]'),function(button){button.onclick=function(){if(this.getAttribute('data-report-source')!==sourceMode)selectSource(this.getAttribute('data-report-source'));};});
    $('emailDetailClose').onclick=closeDetail; $('emailDetailBackdrop').onclick=function(event){if(event.target===this)closeDetail();};
    loadPage();
}());
