(function () {
    'use strict';
    var page = document.getElementById('emailReportsPage');
    if (!page) return;
    var emailEndpoint = page.getAttribute('data-email-endpoint');
    var incomingEndpoint = page.getAttribute('data-incoming-endpoint');
    var endpoint = emailEndpoint, sourceMode = 'email';
    var jobEndpoint = page.getAttribute('data-job-endpoint');
    var sendReportEndpoint = page.getAttribute('data-send-report-endpoint');
    var allItems = [], filteredItems = [], currentPage = 1, pageSize = 50, totalItems = 0, totalPages = 1;
    var detailRequestId = 0;
    var loadRequestId = 0;
    // Each search owns its data/cache. Old requests cannot update a newer search.
    var activeLoad = null;
    var loading = false, listReady = false, reportBusy = false;
    var summary = { found: 0, missing: 0, error: 0, pending: 0, unknown: 0, checked: 0 };
    var $ = function (id) { return document.getElementById(id); };
    function esc(value) { var node = document.createElement('div'); node.textContent = value == null ? '' : String(value); return node.innerHTML.replace(/"/g, '&quot;').replace(/'/g, '&#39;'); }
    function text(item, keys) { for (var i = 0; i < keys.length; i++) if (item && item[keys[i]] != null) return String(item[keys[i]]); return ''; }
    function payloadItems(payload) {
        var data = payload && (payload.content || payload.data || payload.items || payload.emails || payload.results || payload.ListValue || payload.d || payload);
        if (data && !Array.isArray(data)) data = data.content || data.items || data.emails || data.results || data.data;
        return Array.isArray(data) ? data : [];
    }
    function status(item) { return text(item, ['processingStatus','status','state','mailStatus','result']) || 'Không xác định'; }
    function date(item) { return text(item, ['receivedAt','sentAt','createdAt','date','emailDate','created_at','timestamp']); }
    function dateKey(value) { var match = String(value || '').match(/(20\d\d)[-\/]?(\d\d)[-\/]?(\d\d)/); return match ? match[1] + '-' + match[2] + '-' + match[3] : ''; }
    function attachmentName(item) {
        var name = text(item, ['storedFileName']);
        var attachments = item && item.attachments;
        if (!name && Array.isArray(attachments) && attachments.length)
            name = typeof attachments[0] === 'string' ? attachments[0] : text(attachments[0], ['storedFileName']);
        return name;
    }
    function storageFolder(item) {
        var normalized = status(item).trim().toUpperCase();
        if (normalized === 'SAVED' || normalized === 'SKIPPED') return 'processed';
        if (normalized === 'FAILED' || normalized === 'ERROR') {
            var errorMessage = text(item, ['errorMessage']).trim();
            return errorMessage === 'File Word chưa có format YAML phù hợp' ? 'error-format' : 'error';
        }
        if (normalized === 'QUARANTINED') return 'quarantine';
        return '';
    }
    function downloadUrl(item) {
        var fileName = attachmentName(item);
        var itemDate = dateKey(date(item));
        var folder = storageFolder(item);
        if (!fileName || !itemDate || !folder) return '';
        var datePath = itemDate.replace(/-/g, '/');
        return 'http://172.29.79.49/vatm-storage/' + folder + '/' + datePath + '/' + encodeURIComponent(fileName);
    }
    function statusClass(value) { var normalized = String(value).toLowerCase(); return /fail|error|reject/.test(normalized) ? 'failed' : /pending|wait|queue/.test(normalized) ? 'pending' : /sent|success|deliver|complete|ok/.test(normalized) ? 'success' : ''; }
    function oper(item) { return item._oper || text(item, ['operName','operatorName','airlineName','OPER_NAME','oper','OPER','airline','carrier']) || '--'; }
    function setConnection(ok, message) { $('emailConnectionState').textContent = message; page.querySelector('.email-live').className = 'email-live ' + (ok ? 'is-online' : 'is-error'); }
    function sourceLabel() { return sourceMode === 'incoming' ? 'Incoming' : 'email'; }
    function selectSource(mode) {
        sourceMode = mode === 'incoming' ? 'incoming' : 'email';
        endpoint = sourceMode === 'incoming' ? incomingEndpoint : emailEndpoint;
        currentPage = 1;
        Array.prototype.forEach.call(document.querySelectorAll('[data-report-source]'), function (button) {
            var active = button.getAttribute('data-report-source') === sourceMode;
            button.classList.toggle('is-active', active);
            button.setAttribute('aria-pressed', active ? 'true' : 'false');
        });
        $('emailTableTitle').textContent = sourceMode === 'incoming' ? 'Danh sách Incoming' : 'Danh sách email';
        $('emailError').hidden = true;
        load(1);
    }
    function fileTypeOf(item) {
        var name = attachmentName(item) || text(item, ['attachmentName']);
        if (!name) return 'none';
        var ext = (name.lastIndexOf('.') >= 0 ? name.slice(name.lastIndexOf('.') + 1) : '').toLowerCase();
        if (ext === 'doc' || ext === 'docx' || ext === 'rtf') return 'word';
        if (ext === 'xls' || ext === 'xlsx' || ext === 'xlsm' || ext === 'csv') return 'excel';
        if (ext === 'pdf') return 'pdf';
        if (ext === 'png' || ext === 'jpg' || ext === 'jpeg' || ext === 'gif' || ext === 'bmp' || ext === 'tif' || ext === 'tiff' || ext === 'webp') return 'image';
        return 'other';
    }
    function readFilters() {
        return { query: $('emailSearch').value.trim(), processingStatus: $('emailStatus').value,
            fromDate: $('emailFrom').value, toDate: $('emailTo').value, fileType: $('emailFileType').value };
    }
    function filterItems(items, filters, source) {
        return items.filter(function (item) {
            if (filters.fileType && fileTypeOf(item) !== filters.fileType) return false;
            // Incoming currently ignores query/status/date parameters in the upstream API.
            if (source !== 'incoming') return true;
            if (filters.processingStatus && status(item).toUpperCase() !== filters.processingStatus.toUpperCase()) return false;
            var day = dateKey(date(item));
            if (filters.fromDate && (!day || day < filters.fromDate)) return false;
            if (filters.toDate && (!day || day > filters.toDate)) return false;
            var searchText = [text(item, ['subject','title']), text(item, ['sender','from','senderEmail']),
                text(item, ['body','content','message','text','html']), attachmentName(item),
                text(item, ['attachmentName']), text(item, ['errorMessage'])].join(' ').toLowerCase();
            return !filters.query || searchText.indexOf(filters.query.toLowerCase()) >= 0;
        });
    }
    function applyFilters() { return load(); }
    function updateSummary() {
        summary = { found: 0, missing: 0, error: 0, pending: 0, unknown: 0, checked: 0 };
        filteredItems.forEach(function (item) {
            var state = item._databaseState;
            if (/^(found|missing|error|pending)$/.test(state || '')) summary[state]++;
            else summary.unknown++;
            if (state && state !== 'checking') summary.checked++;
        });
    }
    function draw() {
        totalItems = filteredItems.length;
        totalPages = Math.max(1, Math.ceil(totalItems / pageSize));
        currentPage = Math.max(1, Math.min(currentPage, totalPages));
        var start = (currentPage - 1) * pageSize, rows = filteredItems.slice(start, start + pageSize);
        $('emailRows').innerHTML = rows.length ? rows.map(function (item, index) {
            var state = status(item), subject = text(item,['subject','title']) || '(Không có tiêu đề)';
            var sender = text(item,['sender','from','senderEmail']);
            var errorMsg = text(item,['errorMessage']) || '';
            var dbState = item._databaseState;
            var rowClass = dbState === 'found' ? 'email-row--has-data' : /^(missing|error)$/.test(dbState || '') ? 'email-row--no-data' : '';
            var databaseTitle = dbState === 'found' ? 'Có dữ liệu trong database'
                : dbState === 'pending' ? 'Đang chờ xử lý'
                : dbState === 'lookup-error' ? 'Không kiểm tra được database; chưa tính vào số file lỗi'
                : /^(missing|error)$/.test(dbState || '') ? 'Chưa có dữ liệu trong database' : 'Đang kiểm tra database';
            return '<tr class="' + rowClass + '" title="' + databaseTitle + '"><td>' + (start + index + 1) + '</td><td><span class="email-cell-clamp" title="' + esc(date(item)) + '">' + esc(date(item)) + '</span></td><td class="email-subject" title="' + esc(subject) + '"><span class="email-cell-clamp">' + esc(subject) + '</span></td><td><span class="email-cell-clamp" title="' + esc(sender) + '">' + esc(sender) + '</span></td><td><span class="email-cell-clamp" title="' + esc(errorMsg) + '">' + esc(errorMsg) + '</span></td><td><span class="email-status ' + statusClass(state) + '">' + esc(state) + '</span></td><td><button type="button" class="email-detail-button" data-index="' + (start + index) + '" title="Xem chi tiết email"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="7" class="email-empty-cell">' + (loading ? 'Đang lấy đầy đủ dữ liệu theo bộ lọc...' : listReady ? 'Không có email phù hợp.' : 'Chưa có kết quả đầy đủ. Vui lòng tìm kiếm lại.') + '</td></tr>';
        Array.prototype.forEach.call(document.querySelectorAll('.email-detail-button'), function (button) { button.onclick = function () { showDetail(filteredItems[parseInt(this.getAttribute('data-index'), 10)]); }; });
        var statsReady = listReady && summary.checked === totalItems;
        $('emailTotal').textContent = listReady ? totalItems.toLocaleString('vi-VN') : '--';
        $('emailSent').textContent = statsReady ? summary.found.toLocaleString('vi-VN') : '--';
        $('emailFailed').textContent = statsReady ? (summary.missing + summary.error).toLocaleString('vi-VN') : '--';
        $('emailPageLabel').textContent = currentPage;
        $('emailPageInfo').textContent = 'Trang ' + currentPage + '/' + totalPages + ' · ' + (listReady ? totalItems.toLocaleString('vi-VN') : '--') + ' ' + sourceLabel();
        $('emailTableInfo').textContent = listReady ? (totalItems ? 'Hiển thị ' + (start + 1) + '–' + (start + rows.length) + ' / ' + totalItems.toLocaleString('vi-VN') + ' bản ghi sau lọc' : '0 bản ghi sau lọc') : 'Chưa có kết quả đầy đủ';
        if (listReady) $('emailStatsInfo').textContent = !statsReady
            ? 'Đang kiểm tra database: ' + summary.checked + '/' + totalItems + ' file. Chưa công bố số thành công/lỗi.'
            : 'Thống kê trên toàn bộ ' + totalItems.toLocaleString('vi-VN') + ' bản ghi sau lọc, không phụ thuộc số dòng/trang.'
                + (summary.pending ? ' Đang chờ xử lý: ' + summary.pending + '.' : '')
                + (summary.unknown ? ' Không kiểm tra được DB: ' + summary.unknown + '; chưa tính thành công/lỗi. Bấm làm mới để thử lại.' : '');
        $('emailPrev').disabled = !listReady || currentPage <= 1;
        $('emailNext').disabled = !listReady || currentPage >= totalPages;
        $('emailReportDownload').disabled = !statsReady || !totalItems || reportBusy;
        $('emailReportSend').disabled = !statsReady || !totalItems || reportBusy;
    }
    function unwrapJobResult(payload) {
        var result = payload && payload.d != null ? payload.d : payload;
        if (typeof result === 'string') result = JSON.parse(result);
        return result || {};
    }
    function applyDatabaseResult(item, syncJobId, result, context) {
        if (context !== activeLoad) return;
        var targetPermId = result.targetPermId || null;
        var state = result.state || (result.hasDatabaseData === true && targetPermId ? 'found' : 'missing');
        if (result.hasDatabaseData === false && state === 'found') state = 'missing';
        var cached = {
            state: state,
            hasDatabaseData: state === 'found',
            oper: result.oper || '',
            targetPermId: targetPermId,
            editPage: result.editPage || (result.targetTable === 'NO' ? 'Edit_PermNO4Mail.aspx' : 'Edit_PermSC4Mail.aspx'),
            targetTable: result.targetTable || '',
            message: result.message || '',
            permitImportStatus: result.permitImportStatus || '',
            permitImportError: result.permitImportError || ''
        };
        context.cache[syncJobId] = cached;
        item._databaseState = cached.state;
        item._oper = cached.oper;
        item._targetPermId = cached.targetPermId;
        item._editPage = cached.editPage;
        item._databaseMessage = cached.message || cached.permitImportError;
    }
    function fetchDatabaseResult(syncJobId, context) {
        if (context.requests[syncJobId]) return context.requests[syncJobId];
        context.requests[syncJobId] = fetch(jobEndpoint, {
            method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({ syncJobId: syncJobId })
        }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(payload && (payload.Message || payload.message) || 'Không kiểm tra được database');
                return unwrapJobResult(payload);
            });
        }).then(function (result) {
            delete context.requests[syncJobId];
            return result;
        }).catch(function (error) {
            delete context.requests[syncJobId];
            throw error;
        });
        return context.requests[syncJobId];
    }
    function inspectDatabase(item, context) {
        if (context !== activeLoad) return Promise.resolve();
        var syncJobId = text(item, ['syncJobId']);
        if (!syncJobId) {
            item._databaseState = /^(DISCOVERED|DOWNLOADED|PROCESSING|RESERVED|PENDING)$/.test(status(item).trim().toUpperCase()) ? 'pending' : 'missing';
            return Promise.resolve();
        }
        if (context.cache[syncJobId]) {
            applyDatabaseResult(item, syncJobId, context.cache[syncJobId], context);
            return Promise.resolve();
        }
        item._databaseState = 'checking';
        return fetchDatabaseResult(syncJobId, context).then(function (result) {
            applyDatabaseResult(item, syncJobId, result, context);
        }).catch(function (error) {
            if (context !== activeLoad) return;
            context.cache[syncJobId] = { state: 'lookup-error', oper: '', targetPermId: null, message: error.message || 'Không kiểm tra được database' };
            applyDatabaseResult(item, syncJobId, context.cache[syncJobId], context);
        });
    }
    function inspectItems(items, context) {
        var cursor = 0, completed = 0, lastDraw = 0;
        function worker() {
            if (context !== activeLoad || cursor >= items.length) return Promise.resolve();
            return inspectDatabase(items[cursor++], context).then(function () {
                if (context !== activeLoad) return;
                completed++;
                if (completed === items.length || Date.now() - lastDraw > 250) {
                    lastDraw = Date.now(); updateSummary(); draw();
                }
                return worker();
            });
        }
        return Promise.all([worker(), worker(), worker(), worker()]);
    }
    function showDetail(item) {
        if (!item || !activeLoad) return;
        var context = activeLoad;
        var subject = text(item,['subject','title']) || '(Không có tiêu đề)';
        var syncJobId = text(item, ['syncJobId']);
        var fileName = attachmentName(item);
        var fileUrl = downloadUrl(item);
        var downloadLink = $('emailDownloadLink');
        var permissionLink = $('emailPermissionLink');
        var permissionMessage = $('emailPermissionMessage');
        var requestId = ++detailRequestId;
        $('emailDetailSubject').textContent = subject;
        $('emailDetailMeta').innerHTML = '<b>Người gửi:</b> ' + esc(text(item,['sender','from','senderEmail'])) + '<br><b>Tệp đính kèm:</b> ' + esc(fileName) + '<br><b>Thời gian:</b> ' + esc(date(item)) + '<br><b>Trạng thái xử lý:</b> ' + esc(status(item)) + '<br><b>Trạng thái xác nhận:</b> ' + esc(text(item,['acknowledgementStatus'])) + '<br><b>Thông báo lỗi:</b> ' + esc(text(item,['errorMessage'])) + '<br><b>Sync Job ID:</b> ' + esc(syncJobId);
        $('emailDetailBody').textContent = text(item,['body','content','message','text','html']) || JSON.stringify(item, null, 2);
        downloadLink.removeAttribute('href');
        downloadLink.classList.add('is-disabled');
        downloadLink.setAttribute('aria-disabled', 'true');
        if (fileUrl) {
            downloadLink.href = fileUrl;
            downloadLink.classList.remove('is-disabled');
            downloadLink.removeAttribute('aria-disabled');
            downloadLink.title = fileUrl;
        } else {
            downloadLink.title = 'JSON email thiếu ngày hoặc tên file.';
        }
        permissionLink.removeAttribute('href');
        permissionLink.classList.add('is-disabled');
        permissionLink.setAttribute('aria-disabled', 'true');
        permissionMessage.className = 'email-permission-message';
        $('emailDetailBackdrop').hidden = false;

        if (!syncJobId) {
            permissionMessage.textContent = 'Email này không có syncJobId.';
            permissionMessage.classList.add('is-error');
            return;
        }

        permissionMessage.textContent = 'Đang lấy số phép bay...';
        var cached = context.cache[syncJobId];
        var lookup = cached && cached.state === 'found'
            ? Promise.resolve(cached)
            : fetchDatabaseResult(syncJobId, context).then(function (result) {
                if (context !== activeLoad) return null;
                applyDatabaseResult(item, syncJobId, result, context);
                updateSummary(); draw();
                return context.cache[syncJobId];
            });
        lookup.then(function (result) {
            if (requestId !== detailRequestId) return;
            if (!result || result.state !== 'found' || !result.targetPermId) {
                permissionMessage.textContent = result && result.message
                    ? result.message
                    : 'Chưa có dữ liệu số phép bay trong database.';
                permissionMessage.classList.add(result && result.state === 'pending' ? 'is-pending' : 'is-error');
                return;
            }

            var editPage = result.targetTable === 'NO'
                ? 'Edit_PermNO4Mail.aspx'
                : 'Edit_PermSC4Mail.aspx';
            permissionLink.href = '../Permission/' + editPage + '?Menu_ID=51&ID=' + encodeURIComponent(result.targetPermId);
            permissionLink.classList.remove('is-disabled');
            permissionLink.removeAttribute('aria-disabled');
            permissionMessage.textContent = 'Số phép bay: ' + result.targetPermId + (result.targetTable === 'NO' ? ' (ngày đơn)' : '');
        }).catch(function (error) {
            if (requestId !== detailRequestId) return;
            permissionMessage.textContent = 'Không thể lấy số phép bay: ' + error.message;
            permissionMessage.classList.add('is-error');
        });
    }
    function closeDetail() {
        detailRequestId++;
        $('emailDetailBackdrop').hidden = true;
    }
    function reportRows(items) {
        return items.map(function (item) {
            return { Oper: oper(item), FileName: attachmentName(item) || '--', Status: status(item), HasDatabaseData: item._databaseState === 'found', DatabaseState: item._databaseState === 'lookup-error' ? 'error' : item._databaseState };
        });
    }
    function reportHtml(rows) {
        var body = rows.map(function (row, index) {
            var state = row.HasDatabaseData ? row.Status + ' - CÓ DỮ LIỆU DB' : row.DatabaseState === 'pending' ? '[~] ĐANG CHỜ WORKER XỬ LÝ' : row.DatabaseState === 'error' ? '[?] KHÔNG KIỂM TRA ĐƯỢC DATABASE' : '[!] KHÔNG CÓ DỮ LIỆU TRONG DATABASE';
            return '<tr' + (row.HasDatabaseData ? '' : ' style="color:#b4232c;background:#fff0f1;font-weight:bold"') + '><td>' + (index + 1) + '</td><td>' + esc(row.Oper) + '</td><td>' + esc(row.FileName) + '</td><td>' + esc(state) + '</td></tr>';
        }).join('');
        return '<html><head><meta charset="utf-8"><style>body{font-family:Arial}table{border-collapse:collapse;width:100%}th,td{border:1px solid #777;padding:7px}th{background:#dceef8}</style></head><body><h2>BÁO CÁO TRẠNG THÁI DỮ LIỆU EMAIL</h2><table><thead><tr><th>STT</th><th>OPER (Tên hãng)</th><th>Tên file</th><th>Trạng thái</th></tr></thead><tbody>' + body + '</tbody></table></body></html>';
    }
    function loadReportItems() {
        if (!listReady || summary.checked !== totalItems) return Promise.reject(new Error('Vui lòng chờ thống kê đầy đủ.'));
        var senderFilter = $('emailReportSender').value.trim().toLowerCase();
        // Export the same applied snapshot; do not replace its query or lose its file-type filter.
        var items = filteredItems.filter(function (item) {
            return !senderFilter || text(item, ['sender','from','senderEmail']).toLowerCase().indexOf(senderFilter) >= 0;
        }).slice(0, 500);
        return items.length ? Promise.resolve(items) : Promise.reject(new Error('Không có email phù hợp với bộ lọc báo cáo.'));
    }
    function validateReportSender() {
        var input = $('emailReportSender');
        if (input.value.trim() && !input.checkValidity()) {
            $('emailReportMessage').className = 'email-report-message is-error';
            $('emailReportMessage').textContent = 'Email người gửi dùng để lọc không hợp lệ.';
            return false;
        }
        return true;
    }
    function downloadReport() {
        if (!validateReportSender()) return;
        var button = $('emailReportDownload'), message = $('emailReportMessage');
        reportBusy = true; draw();
        button.disabled = true; button.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Đang tạo...';
        message.className = 'email-report-message'; message.textContent = 'Đang lấy tối đa 500 bản ghi ' + sourceLabel() + ' và kiểm tra database...';
        loadReportItems().then(function (items) {
            var rows = reportRows(items), format = $('emailReportFormat').value, extension = format === 'word' ? '.doc' : '.xls';
            var blob = new Blob(['\ufeff', reportHtml(rows)], { type: format === 'word' ? 'application/msword' : 'application/vnd.ms-excel' });
            var link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.download = 'Bao_cao_' + (sourceMode === 'incoming' ? 'Incoming_' : 'Email_') + new Date().toISOString().slice(0, 19).replace(/[-:T]/g, '') + extension;
            document.body.appendChild(link); link.click(); document.body.removeChild(link); URL.revokeObjectURL(link.href);
            message.className = 'email-report-message is-success'; message.textContent = 'Đã tạo báo cáo ' + rows.length + ' dòng.';
        }).catch(function (error) {
            message.className = 'email-report-message is-error'; message.textContent = 'Không thể tạo báo cáo: ' + error.message;
        }).then(function () {
            reportBusy = false; draw();
            button.innerHTML = '<i class="fa fa-download"></i> Tải báo cáo';
        });
    }
    function sendReport() {
        var recipient = $('emailReportRecipient').value.trim(), button = $('emailReportSend'), message = $('emailReportMessage');
        if (!validateReportSender()) return;
        if (!recipient || !$('emailReportRecipient').checkValidity()) {
            message.className = 'email-report-message is-error'; message.textContent = 'Vui lòng nhập Gmail người nhận hợp lệ.'; return;
        }
        reportBusy = true; draw();
        button.disabled = true; button.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Đang gửi...';
        message.className = 'email-report-message'; message.textContent = '';
        message.textContent = 'Đang lấy tối đa 500 bản ghi ' + sourceLabel() + ' và kiểm tra database...';
        loadReportItems().then(function (items) {
            return fetch(sendReportEndpoint, {
                method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({ recipient: recipient, format: $('emailReportFormat').value, rows: reportRows(items) })
            });
        }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(payload && (payload.Message || payload.message) || ('HTTP ' + response.status));
                return payload && payload.d != null ? payload.d : payload;
            });
        }).then(function (result) {
            message.className = 'email-report-message is-success'; message.textContent = result.message || 'Đã gửi báo cáo.';
        }).catch(function (error) {
            message.className = 'email-report-message is-error'; message.textContent = 'Không thể gửi báo cáo: ' + error.message;
        }).then(function () {
            reportBusy = false; draw();
            button.innerHTML = '<i class="fa fa-paper-plane"></i> Gửi qua Gmail';
        });
    }
    function readListPayload(payload) {
        var data = payload && payload.d != null ? payload.d : payload;
        if (typeof data === 'string') data = JSON.parse(data);
        // Do not interpret an invalid/error response as an empty, successful search.
        var candidates = data && (data.content || data.items || data.emails || data.results || data.ListValue || data.data || data);
        if (candidates && !Array.isArray(candidates)) candidates = candidates.content || candidates.items || candidates.emails || candidates.results || candidates.data;
        if (!Array.isArray(candidates)) throw new Error('API trả dữ liệu danh sách không hợp lệ.');
        return { data: data, items: candidates };
    }
    function loadAllItems(context, retriesRemaining) {
        var filters = context.filters, pages = [], expectedTotal = null, expectedPages = null;
        function requestPage(pageNumber, retries) {
            if (context !== activeLoad) return Promise.reject(new Error('Search superseded'));
            var request = { query: filters.query, processingStatus: filters.processingStatus,
                fromDate: filters.fromDate ? filters.fromDate + 'T00:00:00' : '',
                toDate: filters.toDate ? filters.toDate + 'T23:59:59' : '', page: pageNumber, size: 100 };
            return fetch(context.endpoint, { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' }, body: JSON.stringify(request) }).then(function (response) {
                if (!response.ok) throw new Error('HTTP ' + response.status);
                return response.json();
            }).then(function (payload) {
                if (context !== activeLoad) throw new Error('Search superseded');
                var parsed = readListPayload(payload), data = parsed.data;
                if (data.page != null && Number(data.page) !== pageNumber) throw new Error('API trả sai trang dữ liệu.');
                if (expectedTotal !== null && data.totalElements != null && Number(data.totalElements) !== expectedTotal)
                    throw new Error('Dữ liệu nguồn thay đổi trong lúc tải. Vui lòng tìm kiếm lại để có thống kê đầy đủ.');
                if (expectedPages !== null && data.totalPages != null && Math.max(1, Number(data.totalPages)) !== expectedPages)
                    throw new Error('Số trang nguồn thay đổi trong lúc tải. Vui lòng tìm kiếm lại.');
                return parsed;
            }).catch(function (error) {
                if (context === activeLoad && retries > 0) return new Promise(function (resolve) { window.setTimeout(resolve, 600); }).then(function () { return requestPage(pageNumber, retries - 1); });
                throw error;
            });
        }
        return requestPage(0, retriesRemaining).then(function (first) {
            var data = first.data;
            if (data.totalElements != null) {
                expectedTotal = Number(data.totalElements);
                if (!isFinite(expectedTotal) || expectedTotal < 0 || Math.floor(expectedTotal) !== expectedTotal) throw new Error('Tổng bản ghi từ API không hợp lệ.');
            }
            if (data.totalPages != null) {
                expectedPages = Number(data.totalPages);
                if (!isFinite(expectedPages) || expectedPages < 0 || Math.floor(expectedPages) !== expectedPages) throw new Error('Số trang từ API không hợp lệ.');
                expectedPages = Math.max(1, expectedPages);
            } else if (expectedTotal !== null) {
                var apiSize = Number(data.size) || first.items.length || 100;
                expectedPages = Math.max(1, Math.ceil(expectedTotal / apiSize));
            }
            pages[0] = first.items;
            var completed = 1, nextPage = 1;
            function progress() {
                if (context === activeLoad) $('emailStatsInfo').textContent = 'Đang lấy đủ dữ liệu: ' + completed + (expectedPages !== null ? '/' + expectedPages : '') + ' trang nguồn. Chưa công bố tổng số.';
            }
            progress();
            function worker() {
                if (context !== activeLoad || nextPage >= expectedPages) return Promise.resolve();
                var number = nextPage++;
                return requestPage(number, retriesRemaining).then(function (result) {
                    pages[number] = result.items; completed++; progress(); return worker();
                });
            }
            // Legacy array responses/hasNext-only APIs: continue until the explicit end.
            function sequential(previous) {
                if (context !== activeLoad || previous.data.hasNext === false || (previous.data.hasNext == null && previous.items.length < (Number(previous.data.size) || 100))) return Promise.resolve();
                var number = nextPage++;
                return requestPage(number, retriesRemaining).then(function (result) {
                    if (!result.items.length && result.data.hasNext === true) throw new Error('API báo còn trang nhưng trả danh sách rỗng.');
                    pages[number] = result.items; completed++; progress(); return sequential(result);
                });
            }
            return expectedPages !== null ? Promise.all([worker(), worker(), worker(), worker()]) : sequential(first);
        }).then(function () {
            if (context !== activeLoad) throw new Error('Search superseded');
            var items = [];
            pages.forEach(function (batch) { items = items.concat(batch); });
            var seen = Object.create(null);
            items.forEach(function (item) {
                if (item.id == null) return;
                var key = String(item.id) + ':' + (item.attachmentIndex == null ? '' : item.attachmentIndex);
                if (seen[key]) throw new Error('Các trang nguồn có bản ghi trùng. Vui lòng tìm kiếm lại để tránh đếm sai.');
                seen[key] = true;
            });
            if (expectedTotal !== null && items.length !== expectedTotal) throw new Error('Chưa lấy đủ bản ghi từ API. Vui lòng tìm kiếm lại; không sử dụng tổng số chưa đầy đủ.');
            return items;
        });
    }
    function load(retriesRemaining) {
        var filters = readFilters();
        if (filters.fromDate && filters.toDate && filters.fromDate > filters.toDate) {
            $('emailError').textContent = 'Từ ngày không được lớn hơn đến ngày.'; $('emailError').hidden = false;
            return Promise.resolve();
        }
        var context = { id: ++loadRequestId, endpoint: endpoint, source: sourceMode, filters: filters, cache: Object.create(null), requests: Object.create(null) };
        activeLoad = context;
        retriesRemaining = Math.max(0, parseInt(retriesRemaining, 10) || 0);
        closeDetail();
        loading = true; listReady = false; currentPage = 1; allItems = []; filteredItems = [];
        updateSummary(); draw();
        $('emailError').hidden = true; $('emailLastUpdated').textContent = 'Đang cập nhật...';
        $('emailStatsInfo').textContent = 'Đang lấy đầy đủ dữ liệu theo bộ lọc...';
        var apply = $('emailApply');
        apply.disabled = true; apply.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Đang tìm...';
        setConnection(false, 'Đang tải dữ liệu...');
        return loadAllItems(context, retriesRemaining).then(function (items) {
            if (context !== activeLoad) return;
            allItems = items;
            filteredItems = filterItems(allItems, context.filters, context.source);
            listReady = true; loading = false;
            updateSummary(); draw(); setConnection(true, 'Đã kết nối');
            return inspectItems(filteredItems, context).then(function () {
                if (context !== activeLoad) return;
                updateSummary(); draw();
                $('emailLastUpdated').textContent = 'Cập nhật ' + new Date().toLocaleTimeString('vi-VN');
            });
        }).catch(function (error) {
            if (context !== activeLoad) return;
            activeLoad = null; loading = false; listReady = false; allItems = []; filteredItems = [];
            updateSummary(); draw(); setConnection(false, 'Không tải đủ dữ liệu');
            $('emailStatsInfo').textContent = 'Chưa có thống kê đầy đủ. Vui lòng thử lại.';
            $('emailLastUpdated').textContent = 'Cập nhật thất bại';
            $('emailError').textContent = 'Không thể tải dữ liệu ' + sourceLabel() + ': ' + error.message; $('emailError').hidden = false;
        }).then(function () {
            if (context.id !== loadRequestId) return;
            apply.disabled = false; apply.innerHTML = '<i class="fa fa-search"></i> Tìm kiếm';
        });
    }
    $('emailApply').onclick = applyFilters;
    $('emailSearch').onkeydown = function (event) { if (event.key === 'Enter') { event.preventDefault(); applyFilters(); } };
    $('emailFileType').onchange = applyFilters;
    $('emailRefresh').onclick = function () { return load(); };
    $('emailPageSize').onchange = function () { pageSize = parseInt(this.value, 10) || 50; currentPage = 1; draw(); };
    $('emailPrev').onclick = function () { if (currentPage > 1) { currentPage--; draw(); } };
    $('emailNext').onclick = function () { if (currentPage < totalPages) { currentPage++; draw(); } };
    $('emailReportDownload').onclick = downloadReport; $('emailReportSend').onclick = sendReport;
    Array.prototype.forEach.call(document.querySelectorAll('[data-report-source]'), function (button) { button.onclick = function () { if (this.getAttribute('data-report-source') !== sourceMode) selectSource(this.getAttribute('data-report-source')); }; });
    $('emailDetailClose').onclick = closeDetail; $('emailDetailBackdrop').onclick = function (event) { if (event.target === this) closeDetail(); };
    load(1);
}());
