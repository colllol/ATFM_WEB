(function () {
    'use strict';
    var page = document.getElementById('emailReportsPage');
    if (!page) return;
    var endpoint = 'EmailReports.aspx/GetEmails';
    var allItems = [], filteredItems = [], currentPage = 1, pageSize = 50;
    var $ = function (id) { return document.getElementById(id); };
    function esc(value) { var node = document.createElement('div'); node.textContent = value == null ? '' : String(value); return node.innerHTML; }
    function text(item, keys) { for (var i = 0; i < keys.length; i++) if (item && item[keys[i]] != null) return String(item[keys[i]]); return ''; }
    function payloadItems(payload) {
        var data = payload && (payload.data || payload.items || payload.emails || payload.results || payload.ListValue || payload.d || payload);
        if (data && !Array.isArray(data)) data = data.items || data.emails || data.results || data.data;
        return Array.isArray(data) ? data : [];
    }
    function status(item) { return text(item, ['status','state','mailStatus','result']) || 'Không xác định'; }
    function date(item) { return text(item, ['sentAt','createdAt','date','emailDate','created_at','timestamp']); }
    function dateKey(value) { var match = String(value || '').match(/(20\d\d)[-\/]?(\d\d)[-\/]?(\d\d)/); return match ? match[1] + '-' + match[2] + '-' + match[3] : ''; }
    function statusClass(value) { var normalized = String(value).toLowerCase(); return /fail|error|reject/.test(normalized) ? 'failed' : /pending|wait|queue/.test(normalized) ? 'pending' : /sent|success|deliver|complete|ok/.test(normalized) ? 'success' : ''; }
    function setConnection(ok, message) { $('emailConnectionState').textContent = message; page.querySelector('.email-live').className = 'email-live ' + (ok ? 'is-online' : 'is-error'); }
    function applyFilters() {
        var query = $('emailSearch').value.trim().toLowerCase(), selectedStatus = $('emailStatus').value.toLowerCase();
        var from = $('emailFrom').value, to = $('emailTo').value;
        filteredItems = allItems.filter(function (item) {
            var haystack = [text(item,['subject','title']),text(item,['from','sender','senderEmail']),text(item,['to','recipients','recipient']),text(item,['body','content','message'])].join(' ').toLowerCase();
            var key = dateKey(date(item)), state = status(item).toLowerCase();
            return (!query || haystack.indexOf(query) >= 0) && (!selectedStatus || state === selectedStatus) && (!from || !key || key >= from) && (!to || !key || key <= to);
        });
        currentPage = 1; draw();
    }
    function draw() {
        var pages = Math.max(1, Math.ceil(filteredItems.length / pageSize)), start = (currentPage - 1) * pageSize, rows = filteredItems.slice(start, start + pageSize);
        $('emailRows').innerHTML = rows.length ? rows.map(function (item, index) {
            var state = status(item), subject = text(item,['subject','title']) || '(Không có tiêu đề)';
            return '<tr><td>' + (start + index + 1) + '</td><td>' + esc(date(item)) + '</td><td class="email-subject" title="' + esc(subject) + '">' + esc(subject) + '</td><td>' + esc(text(item,['from','sender','senderEmail'])) + '</td><td>' + esc(text(item,['to','recipients','recipient'])) + '</td><td><span class="email-status ' + statusClass(state) + '">' + esc(state) + '</span></td><td><button type="button" class="email-detail-button" data-index="' + (start + index) + '"><i class="fa fa-eye"></i></button></td></tr>';
        }).join('') : '<tr><td colspan="7" class="email-empty-cell">Không có email phù hợp.</td></tr>';
        Array.prototype.forEach.call(document.querySelectorAll('.email-detail-button'), function (button) { button.onclick = function () { showDetail(filteredItems[parseInt(this.getAttribute('data-index'), 10)]); }; });
        $('emailTotal').textContent = filteredItems.length.toLocaleString('vi-VN'); $('emailPageLabel').textContent = currentPage;
        $('emailPageInfo').textContent = 'Trang ' + currentPage + '/' + pages + ' · ' + filteredItems.length.toLocaleString('vi-VN') + ' email';
        $('emailTableInfo').textContent = filteredItems.length.toLocaleString('vi-VN') + ' bản ghi'; $('emailPrev').disabled = currentPage <= 1; $('emailNext').disabled = currentPage >= pages;
    }
    function showDetail(item) {
        if (!item) return;
        var subject = text(item,['subject','title']) || '(Không có tiêu đề)';
        $('emailDetailSubject').textContent = subject;
        $('emailDetailMeta').innerHTML = '<b>Từ:</b> ' + esc(text(item,['from','sender','senderEmail'])) + '<br><b>Đến:</b> ' + esc(text(item,['to','recipients','recipient'])) + '<br><b>Thời gian:</b> ' + esc(date(item)) + '<br><b>Trạng thái:</b> ' + esc(status(item));
        $('emailDetailBody').textContent = text(item,['body','content','message','text','html']) || JSON.stringify(item, null, 2);
        $('emailDetailBackdrop').hidden = false;
    }
    function load() {
        setConnection(false, 'Đang kết nối...');
        fetch(endpoint, { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json; charset=utf-8' }, body: '{}' }).then(function (response) {
            return response.json().catch(function () { return null; }).then(function (payload) {
                if (!response.ok) throw new Error(payload && (payload.Message || payload.message) || ('HTTP ' + response.status));
                return payload;
            });
        }).then(function (payload) {
            if (payload && typeof payload.d === 'string') payload = JSON.parse(payload.d);
            allItems = payloadItems(payload); filteredItems = allItems.slice(); currentPage = 1;
            var statuses = {}; allItems.forEach(function (item) { statuses[status(item)] = true; });
            $('emailStatus').innerHTML = '<option value="">Tất cả trạng thái</option>' + Object.keys(statuses).sort().map(function (value) { return '<option value="' + esc(value) + '">' + esc(value) + '</option>'; }).join('');
            var sent = 0, failed = 0; allItems.forEach(function (item) { var cls = statusClass(status(item)); if (cls === 'success') sent++; if (cls === 'failed') failed++; });
            $('emailSent').textContent = sent.toLocaleString('vi-VN'); $('emailFailed').textContent = failed.toLocaleString('vi-VN'); $('emailLastUpdated').textContent = 'Cập nhật ' + new Date().toLocaleTimeString('vi-VN'); setConnection(true, 'Đã kết nối'); draw();
        }).catch(function (error) { allItems = []; filteredItems = []; draw(); setConnection(false, 'Không kết nối'); $('emailError').textContent = 'Không thể tải dữ liệu email: ' + error.message; $('emailError').hidden = false; });
    }
    $('emailApply').onclick = applyFilters; $('emailRefresh').onclick = function () { $('emailError').hidden = true; load(); }; $('emailPageSize').onchange = function () { pageSize = parseInt(this.value, 10); currentPage = 1; draw(); };
    $('emailPrev').onclick = function () { if (currentPage > 1) { currentPage--; draw(); } }; $('emailNext').onclick = function () { if (currentPage < Math.ceil(filteredItems.length / pageSize)) { currentPage++; draw(); } };
    $('emailDetailClose').onclick = function () { $('emailDetailBackdrop').hidden = true; }; $('emailDetailBackdrop').onclick = function (event) { if (event.target === this) this.hidden = true; };
    load();
}());
