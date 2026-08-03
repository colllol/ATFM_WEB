(function () {
    'use strict';
    var page = document.getElementById('emailReportsPage');
    if (!page) return;
    var endpoint = page.getAttribute('data-email-endpoint');
    var jobEndpoint = page.getAttribute('data-job-endpoint');
    var sendReportEndpoint = page.getAttribute('data-send-report-endpoint');
    var allItems = [], filteredItems = [], currentPage = 1, pageSize = 50, totalItems = 0, totalPages = 1;
    var detailRequestId = 0;
    var databaseCache = {};
    var $ = function (id) { return document.getElementById(id); };
    function esc(value) { var node = document.createElement('div'); node.textContent = value == null ? '' : String(value); return node.innerHTML; }
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
    function downloadUrl(item) {
        var fileName = attachmentName(item);
        var itemDate = dateKey(date(item));
        if (!fileName || !itemDate) return '';
        var folder = status(item).trim().toLowerCase() === 'saved' ? 'processed' : 'error';
        var datePath = itemDate.replace(/-/g, '/');
        return 'http://172.29.79.49/vatm-storage/' + folder + '/' + datePath + '/' + encodeURIComponent(fileName);
    }
    function statusClass(value) { var normalized = String(value).toLowerCase(); return /fail|error|reject/.test(normalized) ? 'failed' : /pending|wait|queue/.test(normalized) ? 'pending' : /sent|success|deliver|complete|ok/.test(normalized) ? 'success' : ''; }
    function oper(item) { return item._oper || text(item, ['operName','operatorName','airlineName','OPER_NAME','oper','OPER','airline','carrier']) || '--'; }
    function databaseBadge(item) {
        if (item._databaseState === 'found') return '<span class="email-db-status found"><i class="fa fa-check-circle"></i> Có dữ liệu</span>';
        if (item._databaseState === 'missing') return '<span class="email-db-status missing"><i class="fa fa-exclamation-triangle"></i> Không có dữ liệu</span>';
        if (item._databaseState === 'error') return '<span class="email-db-status error"><i class="fa fa-question-circle"></i> Không kiểm tra được</span>';
        return '<span class="email-db-status checking"><i class="fa fa-spinner fa-spin"></i> Đang kiểm tra</span>';
    }
    function setConnection(ok, message) { $('emailConnectionState').textContent = message; page.querySelector('.email-live').className = 'email-live ' + (ok ? 'is-online' : 'is-error'); }
    function applyFilters() {
        if ($('emailFrom').value && $('emailTo').value && $('emailFrom').value > $('emailTo').value) {
            $('emailError').textContent = 'Từ ngày không được lớn hơn đến ngày.';
            $('emailError').hidden = false;
            return;
        }
        $('emailError').hidden = true;
        currentPage = 1;
        load();
    }
    function draw() {
        var pages = Math.max(1, totalPages), start = (currentPage - 1) * pageSize, rows = filteredItems;
        $('emailRows').innerHTML = rows.length ? rows.map(function (item, index) {
            var state = status(item), subject = text(item,['subject','title']) || '(Không có tiêu đề)';
            var sender = text(item,['sender','from','senderEmail']);
            var fileName = text(item,['attachmentName']);
            var rowClass = item._databaseState === 'missing' ? ' class="email-row--no-data"' : '';
            return '<tr' + rowClass + '><td>' + (start + index + 1) + '</td><td><span class="email-cell-clamp" title="' + esc(date(item)) + '">' + esc(date(item)) + '</span></td><td class="email-subject" title="' + esc(subject) + '"><span class="email-cell-clamp">' + esc(subject) + '</span></td><td><span class="email-cell-clamp" title="' + esc(sender) + '">' + esc(sender) + '</span></td><td><span class="email-cell-clamp" title="' + esc(fileName) + '">' + esc(fileName) + '</span></td><td><span class="email-status ' + statusClass(state) + '">' + esc(state) + '</span></td><td>' + databaseBadge(item) + '</td><td><button type="button" class="email-detail-button" data-index="' + index + '"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="8" class="email-empty-cell">Không có email phù hợp.</td></tr>';
        Array.prototype.forEach.call(document.querySelectorAll('.email-detail-button'), function (button) { button.onclick = function () { showDetail(filteredItems[parseInt(this.getAttribute('data-index'), 10)]); }; });
        $('emailTotal').textContent = totalItems.toLocaleString('vi-VN'); $('emailPageLabel').textContent = currentPage;
        $('emailPageInfo').textContent = 'Trang ' + currentPage + '/' + pages + ' · ' + totalItems.toLocaleString('vi-VN') + ' email';
        $('emailTableInfo').textContent = rows.length.toLocaleString('vi-VN') + ' bản ghi trên trang'; $('emailPrev').disabled = currentPage <= 1; $('emailNext').disabled = currentPage >= pages;
        var checking = rows.some(function (item) { return !/^(found|missing|error)$/.test(item._databaseState || ''); });
        $('emailReportDownload').disabled = !rows.length || checking;
        $('emailReportSend').disabled = !rows.length || checking;
    }
    function inspectDatabase(item) {
        var syncJobId = text(item, ['syncJobId']);
        if (!syncJobId) { item._databaseState = 'missing'; return Promise.resolve(); }
        if (databaseCache[syncJobId]) {
            item._databaseState = databaseCache[syncJobId].state;
            item._oper = databaseCache[syncJobId].oper;
            item._targetPermId = databaseCache[syncJobId].targetPermId;
            return Promise.resolve();
        }
        item._databaseState = 'checking';
        return fetch(jobEndpoint, {
            method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({ syncJobId: syncJobId })
        }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(payload && (payload.Message || payload.message) || 'Không kiểm tra được database');
                return payload;
            });
        }).then(function (payload) {
            var result = payload && payload.d != null ? payload.d : payload;
            if (typeof result === 'string') result = JSON.parse(result);
            if (!result || !result.targetPermId) throw new Error('Không tìm thấy dữ liệu');
            databaseCache[syncJobId] = { state: 'found', oper: result.oper || '', targetPermId: result.targetPermId };
            item._databaseState = 'found'; item._oper = result.oper || ''; item._targetPermId = result.targetPermId;
        }).catch(function (error) {
            var missing = /targetPermId|không tìm thấy dữ liệu|không có dữ liệu số phép bay/i.test(error.message || '');
            var state = missing ? 'missing' : 'error';
            databaseCache[syncJobId] = { state: state, oper: '', targetPermId: null };
            item._databaseState = state;
        });
    }
    function inspectCurrentPage() {
        var queue = filteredItems.slice(), cursor = 0;
        function worker() {
            if (cursor >= queue.length) return Promise.resolve();
            return inspectDatabase(queue[cursor++]).then(worker);
        }
        return Promise.all([worker(), worker(), worker(), worker()]).then(draw);
    }
    function showDetail(item) {
        if (!item) return;
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
        fetch(jobEndpoint, {
            method: 'POST',
            headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({ syncJobId: syncJobId })
        }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(payload && (payload.Message || payload.message) || ('HTTP ' + response.status));
                return payload;
            });
        }).then(function (payload) {
            if (requestId !== detailRequestId) return;
            var result = payload && payload.d != null ? payload.d : payload;
            if (typeof result === 'string') result = JSON.parse(result);
            var targetPermId = result && result.targetPermId;
            if (!targetPermId) throw new Error('API job không trả về targetPermId.');

            permissionLink.href = '../Permission/Edit_PermSC.aspx?Menu_ID=51&ID=' + encodeURIComponent(targetPermId);
            permissionLink.classList.remove('is-disabled');
            permissionLink.removeAttribute('aria-disabled');
            permissionMessage.textContent = 'Số phép bay: ' + targetPermId;
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
    function reportRows() {
        return filteredItems.map(function (item) {
            return { Oper: oper(item), Status: status(item), HasDatabaseData: item._databaseState === 'found', DatabaseState: item._databaseState };
        });
    }
    function reportHtml() {
        var rows = reportRows();
        var body = rows.map(function (row, index) {
            var state = row.HasDatabaseData ? row.Status + ' - CÓ DỮ LIỆU DB' : row.DatabaseState === 'error' ? '[?] KHÔNG KIỂM TRA ĐƯỢC DATABASE' : '[!] KHÔNG CÓ DỮ LIỆU TRONG DATABASE';
            return '<tr' + (row.HasDatabaseData ? '' : ' style="color:#b4232c;background:#fff0f1;font-weight:bold"') + '><td>' + (index + 1) + '</td><td>' + esc(row.Oper) + '</td><td>' + esc(state) + '</td></tr>';
        }).join('');
        return '<html><head><meta charset="utf-8"><style>body{font-family:Arial}table{border-collapse:collapse;width:100%}th,td{border:1px solid #777;padding:7px}th{background:#dceef8}</style></head><body><h2>BÁO CÁO TRẠNG THÁI DỮ LIỆU EMAIL</h2><table><thead><tr><th>STT</th><th>OPER (Tên hãng)</th><th>Trạng thái</th></tr></thead><tbody>' + body + '</tbody></table></body></html>';
    }
    function downloadReport() {
        var format = $('emailReportFormat').value, extension = format === 'word' ? '.doc' : '.xls';
        var blob = new Blob(['\ufeff', reportHtml()], { type: format === 'word' ? 'application/msword' : 'application/vnd.ms-excel' });
        var link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = 'Bao_cao_Email_' + new Date().toISOString().slice(0, 19).replace(/[-:T]/g, '') + extension;
        document.body.appendChild(link); link.click(); document.body.removeChild(link); URL.revokeObjectURL(link.href);
    }
    function sendReport() {
        var recipient = $('emailReportRecipient').value.trim(), button = $('emailReportSend'), message = $('emailReportMessage');
        if (!recipient || !$('emailReportRecipient').checkValidity()) {
            message.className = 'email-report-message is-error'; message.textContent = 'Vui lòng nhập Gmail người nhận hợp lệ.'; return;
        }
        button.disabled = true; button.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Đang gửi...';
        message.className = 'email-report-message'; message.textContent = '';
        fetch(sendReportEndpoint, {
            method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({ recipient: recipient, format: $('emailReportFormat').value, rows: reportRows() })
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
            button.disabled = false; button.innerHTML = '<i class="fa fa-paper-plane"></i> Gửi qua Gmail';
        });
    }
    function load(retriesRemaining) {
        retriesRemaining = Math.max(0, parseInt(retriesRemaining, 10) || 0);
        var apply = $('emailApply');
        apply.disabled = true;
        apply.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Đang tìm...';
        setConnection(false, 'Đang kết nối...');
        var request = {
            query: $('emailSearch').value.trim(),
            processingStatus: $('emailStatus').value,
            fromDate: $('emailFrom').value ? $('emailFrom').value + 'T00:00:00' : '',
            toDate: $('emailTo').value ? $('emailTo').value + 'T23:59:59' : '',
            page: currentPage - 1,
            size: pageSize
        };
        fetch(endpoint, { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' }, body: JSON.stringify(request) }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(payload && (payload.Message || payload.message) || ('HTTP ' + response.status));
                return payload;
            });
        }).then(function (payload) {
            if (payload && typeof payload.d === 'string') payload = JSON.parse(payload.d);
            allItems = payloadItems(payload); filteredItems = allItems.slice();
            totalItems = Number(payload && payload.totalElements != null ? payload.totalElements : allItems.length);
            totalPages = Number(payload && payload.totalPages != null ? payload.totalPages : 1);
            var sent = 0, failed = 0; allItems.forEach(function (item) { var cls = statusClass(status(item)); if (cls === 'success') sent++; if (cls === 'failed') failed++; });
            $('emailSent').textContent = sent.toLocaleString('vi-VN'); $('emailFailed').textContent = failed.toLocaleString('vi-VN'); $('emailLastUpdated').textContent = 'Cập nhật ' + new Date().toLocaleTimeString('vi-VN'); setConnection(true, 'Đã kết nối'); draw(); inspectCurrentPage();
        }).catch(function (error) {
            if (retriesRemaining > 0) {
                setConnection(false, 'Đang thử lại...');
                window.setTimeout(function () { load(retriesRemaining - 1); }, 600);
                return;
            }
            allItems = []; filteredItems = []; totalItems = 0; totalPages = 1; draw(); setConnection(false, 'Không kết nối'); $('emailError').textContent = 'Không thể tải dữ liệu email: ' + error.message; $('emailError').hidden = false;
        }).then(function () {
            apply.disabled = false;
            apply.innerHTML = '<i class="fa fa-search"></i> Tìm kiếm';
        });
    }
    $('emailApply').onclick = applyFilters; $('emailSearch').onkeydown = function (event) { if (event.key === 'Enter') applyFilters(); }; $('emailRefresh').onclick = function () { $('emailError').hidden = true; load(); }; $('emailPageSize').onchange = function () { pageSize = parseInt(this.value, 10); currentPage = 1; load(); };
    $('emailPrev').onclick = function () { if (currentPage > 1) { currentPage--; load(); } }; $('emailNext').onclick = function () { if (currentPage < totalPages) { currentPage++; load(); } };
    $('emailReportDownload').onclick = downloadReport; $('emailReportSend').onclick = sendReport;
    $('emailDetailClose').onclick = closeDetail; $('emailDetailBackdrop').onclick = function (event) { if (event.target === this) closeDetail(); };
    load(1);
}());
