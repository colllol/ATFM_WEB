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
    var appliedFilters = null;
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
    function postJson(url, body) {
        return fetch(url, {
            method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(body)
        }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(errorMessage(payload, 'HTTP ' + response.status));
                return unwrap(payload);
            });
        });
    }
    function status(item) { return text(item, ['processingStatus','status','state','mailStatus','result']) || 'Kh?ng x?c ??nh'; }
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
            return text(item, ['errorMessage']).trim() === 'File Word ch?a c? format YAML ph? h?p' ? 'error-format' : 'error';
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

    function normalizeSummary(value) {
        if (!value || value.totalFiles == null) return null;
        var result = {
            totalFiles: Number(value.totalFiles), successFiles: Number(value.successFiles),
            errorFiles: Number(value.errorFiles), pendingFiles: Number(value.pendingFiles),
            unknownFiles: Number(value.unknownFiles), snapshotId: text(value, ['snapshotId','SnapshotId'])
        };
        if (![result.totalFiles,result.successFiles,result.errorFiles,result.pendingFiles,result.unknownFiles].every(isFinite)) return null;
        return result;
    }
    function draw() {
        var start = (currentPage - 1) * pageSize;
        $('emailRows').innerHTML = rows.length ? rows.map(function (item, index) {
            var state = status(item), subject = text(item,['subject','title']) || '(Kh?ng c? ti?u ??)';
            var sender = text(item,['sender','from','senderEmail']), errorMsg = text(item,['errorMessage']);
            var dbState = databaseState(item);
            var rowClass = dbState === 'found' ? 'email-row--has-data' : /^(missing|error)$/.test(dbState) ? 'email-row--no-data' : '';
            var dbTitle = dbState === 'found' ? 'C? d? li?u trong database' : dbState === 'pending' ? '?ang ch? x? l?' : dbState === 'unknown' ? 'Ch?a x?c minh ???c database' : 'Kh?ng c? d? li?u trong database';
            return '<tr class="' + rowClass + '" title="' + dbTitle + '"><td>' + (start + index + 1) + '</td>' +
                '<td><span class="email-cell-clamp" title="' + esc(date(item)) + '">' + esc(date(item)) + '</span></td>' +
                '<td class="email-subject" title="' + esc(subject) + '"><span class="email-cell-clamp">' + esc(subject) + '</span></td>' +
                '<td><span class="email-cell-clamp" title="' + esc(sender) + '">' + esc(sender) + '</span></td>' +
                '<td><span class="email-cell-clamp" title="' + esc(errorMsg) + '">' + esc(errorMsg) + '</span></td>' +
                '<td><span class="email-status ' + statusClass(state) + '">' + esc(state) + '</span></td>' +
                '<td><button type="button" class="email-detail-button" data-index="' + index + '" title="Xem chi ti?t email"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="7" class="email-empty-cell">' + (loading ? '?ang t?i d? li?u...' : 'Kh?ng c? email ph? h?p.') + '</td></tr>';
        Array.prototype.forEach.call(document.querySelectorAll('.email-detail-button'), function (button) {
            button.onclick = function () { showDetail(rows[parseInt(this.getAttribute('data-index'), 10)]); };
        });
        $('emailTotal').textContent = totalItems.toLocaleString('vi-VN');
        $('emailSent').textContent = summary ? summary.successFiles.toLocaleString('vi-VN') : '--';
        $('emailFailed').textContent = summary ? summary.errorFiles.toLocaleString('vi-VN') : '--';
        $('emailPageLabel').textContent = currentPage;
        $('emailPageInfo').textContent = 'Trang ' + currentPage + '/' + totalPages + ' ? ' + totalItems.toLocaleString('vi-VN') + ' ' + sourceLabel();
        $('emailTableInfo').textContent = totalItems ? 'Hi?n th? ' + (start + 1) + '?' + (start + rows.length) + ' / ' + totalItems.toLocaleString('vi-VN') + ' b?n ghi sau l?c' : '0 b?n ghi sau l?c';
        $('emailPrev').disabled = loading || currentPage <= 1;
        $('emailNext').disabled = loading || currentPage >= totalPages;
        $('emailReportDownload').disabled = loading || !rows.length || reportBusy;
        $('emailReportSend').disabled = loading || !rows.length || reportBusy;
        if (summary) $('emailStatsInfo').textContent = 'KPI tr?n to?n b? ' + summary.totalFiles.toLocaleString('vi-VN') + ' b?n ghi sau l?c.' +
            (summary.pendingFiles ? ' ?ang ch?: ' + summary.pendingFiles + '.' : '') +
            (summary.unknownFiles ? ' Ch?a x?c minh DB: ' + summary.unknownFiles + '.' : '');
        else $('emailStatsInfo').textContent = 'Danh s?ch ?? ph?n trang. KPI th?nh c?ng/l?i ch?a kh? d?ng do API summary ch?a ph?n h?i h?p l?.';
    }

    function parseList(value) {
        var items = value && (value.content || value.items || value.data);
        if (!Array.isArray(items)) throw new Error('API tr? d? li?u danh s?ch kh?ng h?p l?.');
        var total = Number(value.totalElements), pages = Number(value.totalPages);
        if (!isFinite(total) || total < 0) throw new Error('API kh?ng tr? t?ng s? b?n ghi h?p l?.');
        if (!isFinite(pages) || pages < 0) pages = Math.ceil(total / pageSize);
        return { items: items, total: total, pages: Math.max(1, pages) };
    }
    function loadPage(options) {
        options = options || {};
        var requestId = ++loadRequestId, filters = options.keepFilters && appliedFilters ? appliedFilters : readFilters();
        if (filters.fromDate && filters.toDate && filters.fromDate > filters.toDate) {
            $('emailError').textContent = 'T? ng?y kh?ng ???c l?n h?n ??n ng?y.'; $('emailError').hidden = false;
            return Promise.resolve();
        }
        if (!options.keepFilters) { currentPage = 1; appliedFilters = filters; summary = null; snapshotId = ''; }
        loading = true; rows = []; draw(); $('emailError').hidden = true;
        $('emailApply').disabled = true; $('emailApply').innerHTML = '<i class="fa fa-spinner fa-spin"></i> ?ang t?m...';
        setConnection(false, '?ang t?i d? li?u...');

        var summaryPromise = options.keepSnapshot || !summaryEndpoint() ? Promise.resolve(summary) :
            postJson(summaryEndpoint(), summaryBody(filters)).then(function (value) {
                var parsed = normalizeSummary(value);
                if (!parsed) throw new Error('API summary ch?a tr? schema KPI m?i.');
                return parsed;
            }).catch(function () { return null; });

        return summaryPromise.then(function (summaryResult) {
            if (requestId !== loadRequestId) return null;
            if (summaryResult) { summary = summaryResult; snapshotId = summaryResult.snapshotId || ''; }
            return postJson(listEndpoint(), requestBody(filters, currentPage - 1, pageSize, snapshotId));
        }).then(function (value) {
            if (requestId !== loadRequestId || value == null) return;
            var list = parseList(value);
            rows = list.items; totalItems = list.total; totalPages = list.pages;
            if (currentPage > totalPages) { currentPage = totalPages; return loadPage({ keepFilters: true, keepSnapshot: true }); }
            if (summary && summary.totalFiles !== totalItems) {
                summary = null;
                $('emailError').textContent = 'Summary v? danh s?ch kh?ng c?ng t?ng d? li?u; ?ang hi?n th? t?ng t? danh s?ch.';
                $('emailError').hidden = false;
            }
            loading = false; draw(); setConnection(true, '?? k?t n?i');
            $('emailLastUpdated').textContent = 'C?p nh?t ' + new Date().toLocaleTimeString('vi-VN');
        }).catch(function (error) {
            if (requestId !== loadRequestId) return;
            loading = false; rows = []; totalItems = 0; totalPages = 1; summary = null; draw();
            setConnection(false, 'Kh?ng t?i ???c d? li?u');
            $('emailError').textContent = 'Kh?ng th? t?i d? li?u ' + sourceLabel() + ': ' + error.message;
            $('emailError').hidden = false;
        }).then(function () {
            if (requestId !== loadRequestId) return;
            $('emailApply').disabled = false; $('emailApply').innerHTML = '<i class="fa fa-search"></i> T?m ki?m';
        });
    }

    function selectSource(mode) {
        sourceMode = mode === 'incoming' ? 'incoming' : 'email';
        currentPage = 1; appliedFilters = null; summary = null; snapshotId = '';
        Array.prototype.forEach.call(document.querySelectorAll('[data-report-source]'), function (button) {
            var active = button.getAttribute('data-report-source') === sourceMode;
            button.classList.toggle('is-active', active); button.setAttribute('aria-pressed', active ? 'true' : 'false');
        });
        $('emailTableTitle').textContent = sourceMode === 'incoming' ? 'Danh s?ch Incoming' : 'Danh s?ch email';
        loadPage();
    }

    function showDetail(item) {
        if (!item) return;
        var requestId = ++detailRequestId, id = Number(text(item, ['id','ID']));
        $('emailDetailSubject').textContent = text(item,['subject','title']) || '(Kh?ng c? ti?u ??)';
        $('emailDetailMeta').textContent = '?ang t?i chi ti?t...'; $('emailDetailBody').textContent = '';
        $('emailDetailBackdrop').hidden = false;
        var detailPromise = id > 0 ? postJson(detailEndpoint(), { id: id }).catch(function () { return item; }) : Promise.resolve(item);
        detailPromise.then(function (detail) {
            if (requestId !== detailRequestId) return;
            detail = detail || item;
            var syncJobId = text(detail, ['syncJobId']) || text(item, ['syncJobId']);
            var fileName = attachmentName(detail) || attachmentName(item), fileUrl = downloadUrl(detail) || downloadUrl(item);
            $('emailDetailSubject').textContent = text(detail,['subject','title']) || '(Kh?ng c? ti?u ??)';
            $('emailDetailMeta').innerHTML = '<b>Ng??i g?i:</b> ' + esc(text(detail,['sender','from','senderEmail'])) + '<br><b>T?p ??nh k?m:</b> ' + esc(fileName) + '<br><b>Th?i gian:</b> ' + esc(date(detail)) + '<br><b>Tr?ng th?i x? l?:</b> ' + esc(status(detail)) + '<br><b>Th?ng b?o l?i:</b> ' + esc(text(detail,['errorMessage'])) + '<br><b>Sync Job ID:</b> ' + esc(syncJobId);
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
        if (!syncJobId) { message.textContent = 'Email n?y kh?ng c? syncJobId.'; message.classList.add('is-error'); return; }
        message.textContent = '?ang l?y s? ph?p bay...';
        postJson(endpoints.job, { syncJobId: syncJobId }).then(function (result) {
            if (requestId !== detailRequestId) return;
            if (!result || result.state !== 'found' || !result.targetPermId) {
                message.textContent = result && result.message || 'Ch?a c? d? li?u s? ph?p bay trong database.';
                message.classList.add(result && result.state === 'pending' ? 'is-pending' : 'is-error'); return;
            }
            var editPage = result.targetTable === 'NO' ? 'Edit_PermNO4Mail.aspx' : 'Edit_PermSC4Mail.aspx';
            link.href = '../Permission/' + editPage + '?Menu_ID=51&ID=' + encodeURIComponent(result.targetPermId);
            link.classList.remove('is-disabled'); link.removeAttribute('aria-disabled');
            message.textContent = 'S? ph?p bay: ' + result.targetPermId + (result.targetTable === 'NO' ? ' (ng?y ??n)' : '');
        }).catch(function (error) {
            if (requestId !== detailRequestId) return;
            message.textContent = 'Kh?ng th? l?y s? ph?p bay: ' + error.message; message.classList.add('is-error');
        });
    }
    function closeDetail() { detailRequestId++; $('emailDetailBackdrop').hidden = true; }

    function reportRows(items) {
        return items.map(function (item) { var state = databaseState(item); return { Oper: oper(item), FileName: attachmentName(item) || '--', Status: status(item), HasDatabaseData: state === 'found', DatabaseState: state }; });
    }
    function reportHtml(items) {
        var body = reportRows(items).map(function (row,index) {
            var state = row.HasDatabaseData ? row.Status + ' - C? D? LI?U DB' : row.DatabaseState === 'pending' ? '[~] ?ANG CH? X? L?' : row.DatabaseState === 'unknown' ? '[?] CH?A X?C MINH DB' : '[!] KH?NG C? D? LI?U DB';
            return '<tr><td>'+(index+1)+'</td><td>'+esc(row.Oper)+'</td><td>'+esc(row.FileName)+'</td><td>'+esc(state)+'</td></tr>';
        }).join('');
        return '<html><head><meta charset="utf-8"><style>body{font-family:Arial}table{border-collapse:collapse;width:100%}th,td{border:1px solid #777;padding:7px}</style></head><body><h2>B?O C?O TR?NG TH?I D? LI?U EMAIL</h2><table><tr><th>STT</th><th>OPER</th><th>T?n file</th><th>Tr?ng th?i</th></tr>'+body+'</table></body></html>';
    }
    function filteredReportRows() {
        var sender = $('emailReportSender').value.trim().toLowerCase();
        return rows.filter(function (item) { return !sender || text(item,['sender','from','senderEmail']).toLowerCase().indexOf(sender) >= 0; });
    }
    function validateReport() {
        if ($('emailReportSender').value.trim() && !$('emailReportSender').checkValidity()) { $('emailReportMessage').textContent='Email ng??i g?i kh?ng h?p l?.'; return false; }
        return true;
    }
    function downloadReport() {
        if (!validateReport()) return;
        var items=filteredReportRows(), message=$('emailReportMessage'); if(!items.length){message.textContent='Kh?ng c? d? li?u tr?n trang hi?n t?i.';return;}
        var format=$('emailReportFormat').value, blob=new Blob(['\ufeff',reportHtml(items)],{type:format==='word'?'application/msword':'application/vnd.ms-excel'});
        var link=document.createElement('a'); link.href=URL.createObjectURL(blob); link.download='Bao_cao_'+sourceLabel()+'_trang_'+currentPage+(format==='word'?'.doc':'.xls'); document.body.appendChild(link); link.click(); document.body.removeChild(link); URL.revokeObjectURL(link.href);
        message.className='email-report-message is-success'; message.textContent='?? t?o b?o c?o '+items.length+' d?ng c?a trang hi?n t?i.';
    }
    function sendReport() {
        var recipient=$('emailReportRecipient').value.trim(), items=filteredReportRows(), message=$('emailReportMessage');
        if(!validateReport() || !recipient || !$('emailReportRecipient').checkValidity()){message.textContent='Vui l?ng nh?p Gmail ng??i nh?n h?p l?.';return;}
        if(!items.length){message.textContent='Kh?ng c? d? li?u tr?n trang hi?n t?i.';return;}
        reportBusy=true; draw();
        postJson(endpoints.sendReport,{recipient:recipient,format:$('emailReportFormat').value,rows:reportRows(items)}).then(function(result){message.className='email-report-message is-success';message.textContent=result.message||'?? g?i b?o c?o.';}).catch(function(error){message.className='email-report-message is-error';message.textContent='Kh?ng th? g?i b?o c?o: '+error.message;}).then(function(){reportBusy=false;draw();});
    }

    $('emailApply').onclick = function () { loadPage(); };
    $('emailSearch').onkeydown = function (event) { if (event.key === 'Enter') { event.preventDefault(); loadPage(); } };
    $('emailFileType').onchange = function () { loadPage(); };
    $('emailRefresh').onclick = function () { loadPage(); };
    $('emailPageSize').onchange = function () { pageSize = parseInt(this.value,10)||50; currentPage=1; loadPage({keepFilters:true,keepSnapshot:true}); };
    $('emailPrev').onclick = function () { if(currentPage>1){currentPage--;loadPage({keepFilters:true,keepSnapshot:true});} };
    $('emailNext').onclick = function () { if(currentPage<totalPages){currentPage++;loadPage({keepFilters:true,keepSnapshot:true});} };
    $('emailReportDownload').onclick=downloadReport; $('emailReportSend').onclick=sendReport;
    Array.prototype.forEach.call(document.querySelectorAll('[data-report-source]'),function(button){button.onclick=function(){if(this.getAttribute('data-report-source')!==sourceMode)selectSource(this.getAttribute('data-report-source'));};});
    $('emailDetailClose').onclick=closeDetail; $('emailDetailBackdrop').onclick=function(event){if(event.target===this)closeDetail();};
    loadPage();
}());
