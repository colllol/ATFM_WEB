(function () {
    'use strict';
    var page = document.getElementById('emailReportsPage');
    if (!page) return;
    var endpoint = 'EmailReports.aspx/GetEmails';
    var allItems = [], filteredItems = [], currentPage = 1, pageSize = 50, totalItems = 0, totalPages = 1;
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
    function statusClass(value) { var normalized = String(value).toLowerCase(); return /fail|error|reject/.test(normalized) ? 'failed' : /pending|wait|queue/.test(normalized) ? 'pending' : /sent|success|deliver|complete|ok/.test(normalized) ? 'success' : ''; }
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
            return '<tr><td>' + (start + index + 1) + '</td><td>' + esc(date(item)) + '</td><td class="email-subject" title="' + esc(subject) + '">' + esc(subject) + '</td><td>' + esc(text(item,['sender','from','senderEmail'])) + '</td><td>' + esc(text(item,['attachmentName'])) + '</td><td><span class="email-status ' + statusClass(state) + '">' + esc(state) + '</span></td><td><button type="button" class="email-detail-button" data-index="' + index + '"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="7" class="email-empty-cell">Không có email phù hợp.</td></tr>';
        Array.prototype.forEach.call(document.querySelectorAll('.email-detail-button'), function (button) { button.onclick = function () { showDetail(filteredItems[parseInt(this.getAttribute('data-index'), 10)]); }; });
        $('emailTotal').textContent = totalItems.toLocaleString('vi-VN'); $('emailPageLabel').textContent = currentPage;
        $('emailPageInfo').textContent = 'Trang ' + currentPage + '/' + pages + ' · ' + totalItems.toLocaleString('vi-VN') + ' email';
        $('emailTableInfo').textContent = rows.length.toLocaleString('vi-VN') + ' bản ghi trên trang'; $('emailPrev').disabled = currentPage <= 1; $('emailNext').disabled = currentPage >= pages;
    }
    function showDetail(item) {
        if (!item) return;
        var subject = text(item,['subject','title']) || '(Không có tiêu đề)';
        $('emailDetailSubject').textContent = subject;
        $('emailDetailMeta').innerHTML = '<b>Người gửi:</b> ' + esc(text(item,['sender','from','senderEmail'])) + '<br><b>Tệp đính kèm:</b> ' + esc(text(item,['attachmentName'])) + '<br><b>Thời gian:</b> ' + esc(date(item)) + '<br><b>Trạng thái xử lý:</b> ' + esc(status(item)) + '<br><b>Trạng thái xác nhận:</b> ' + esc(text(item,['acknowledgementStatus']));
        $('emailDetailBody').textContent = text(item,['body','content','message','text','html']) || JSON.stringify(item, null, 2);
        $('emailDetailBackdrop').hidden = false;
    }
    function load() {
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
            $('emailSent').textContent = sent.toLocaleString('vi-VN'); $('emailFailed').textContent = failed.toLocaleString('vi-VN'); $('emailLastUpdated').textContent = 'Cập nhật ' + new Date().toLocaleTimeString('vi-VN'); setConnection(true, 'Đã kết nối'); draw();
        }).catch(function (error) { allItems = []; filteredItems = []; totalItems = 0; totalPages = 1; draw(); setConnection(false, 'Không kết nối'); $('emailError').textContent = 'Không thể tải dữ liệu email: ' + error.message; $('emailError').hidden = false; }).then(function () {
            apply.disabled = false;
            apply.innerHTML = '<i class="fa fa-search"></i> Tìm kiếm';
        });
    }
    $('emailApply').onclick = applyFilters; $('emailSearch').onkeydown = function (event) { if (event.key === 'Enter') applyFilters(); }; $('emailRefresh').onclick = function () { $('emailError').hidden = true; load(); }; $('emailPageSize').onchange = function () { pageSize = parseInt(this.value, 10); currentPage = 1; load(); };
    $('emailPrev').onclick = function () { if (currentPage > 1) { currentPage--; load(); } }; $('emailNext').onclick = function () { if (currentPage < totalPages) { currentPage++; load(); } };
    $('emailDetailClose').onclick = function () { $('emailDetailBackdrop').hidden = true; }; $('emailDetailBackdrop').onclick = function (event) { if (event.target === this) this.hidden = true; };
    load();
}());
