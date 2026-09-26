const fs = require('node:fs');
const vm = require('node:vm');
const assert = require('node:assert/strict');
const path = require('node:path');
const source = fs.readFileSync(path.join(__dirname, '../prjApplication/SLOTS/EmailReports.js'), 'utf8');
let count = 0;
function equal(actual, expected, message) { assert.equal(actual, expected, message); count++; }
function ok(value, message) { assert.ok(value, message); count++; }
function harness() {
    const elements = {}, requests = [];
    const tabs = ['email', 'incoming'].map(mode => ({ getAttribute: () => mode, setAttribute() {}, classList: { toggle() {} } }));
    function element(id) {
        if (!elements[id]) elements[id] = {
            value: '', textContent: '', innerHTML: '', disabled: false, hidden: true,
            classList: { add() {}, remove() {}, toggle() {} },
            getAttribute(name) { return { 'data-email-endpoint': 'email', 'data-incoming-endpoint': 'incoming', 'data-job-endpoint': 'jobs', 'data-send-report-endpoint': 'send' }[name]; },
            setAttribute() {}, removeAttribute() {}, querySelector() { return element('live'); }, checkValidity() { return true; }
        };
        return elements[id];
    }
    element('emailFileType').value = 'word'; element('emailPageSize').value = '50';
    const h = { el: element, requests, tabs, rows: [], job: () => ({ hasDatabaseData: true, state: 'found', targetPermId: 123 }), jobsActive: 0, maxJobs: 0 };
    const context = {
        console, Promise, Date, JSON, Number, String, Array, Math, Object, isFinite, parseInt, encodeURIComponent,
        window: { setTimeout },
        document: {
            getElementById: element,
            querySelectorAll(selector) { return selector === '[data-report-source]' ? tabs : []; },
            createElement() { let text = ''; return { set textContent(value) { text = String(value); }, get innerHTML() { return text.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;'); } }; }
        },
        fetch: async (url, options) => {
            const request = JSON.parse(options.body); requests.push({ url, request });
            if (url === 'jobs') {
                h.jobsActive++; h.maxJobs = Math.max(h.maxJobs, h.jobsActive);
                try {
                    await new Promise(resolve => setImmediate(resolve));
                    const payload = await h.job(request.syncJobId);
                    return { ok: true, json: async () => ({ d: payload }) };
                } finally { h.jobsActive--; }
            }
            if (h.respond) return h.respond(url, request);
            let rows = h.rows;
            // Model the upstream contract: Email filters server-side; Incoming only paginates.
            if (url === 'email') rows = rows.filter(item => (!request.query || (item.subject || '').includes(request.query)) &&
                (!request.processingStatus || item.processingStatus === request.processingStatus) &&
                (!request.fromDate || item.receivedAt >= request.fromDate) && (!request.toDate || item.receivedAt <= request.toDate));
            const size = h.apiSize || request.size;
            const data = { content: rows.slice(request.page * size, (request.page + 1) * size), totalElements: rows.length,
                totalPages: Math.ceil(rows.length / size), page: request.page, size };
            return response(data);
        }
    };
    vm.createContext(context);
    const replaced = source.replace(/    load\(1\);\s*\}\(\)\);\s*$/, '    globalThis.api = { load, loadReportItems, showDetail, getState: function () { return { filteredItems, summary, currentPage, totalPages, listReady }; } };\n}());');
    assert.notEqual(replaced, source);
    vm.runInContext(replaced, context);
    h.api = context.api;
    h.rowCount = () => (element('emailRows').innerHTML.match(/data-index=/g) || []).length;
    return h;
}
function response(data) { return { ok: true, json: async () => ({ d: JSON.stringify(data) }) }; }
function row(i, ext = 'docx') { return { id: i, syncJobId: 'job-' + i, storedFileName: 'file-' + i + '.' + ext,
    subject: 'match-' + i, sender: 'sender@example.test', processingStatus: 'SAVED', receivedAt: '2026-09-20T10:00:00' }; }
async function until(fn) { for (let i = 0; i < 200; i++) { if (fn()) return; await new Promise(resolve => setImmediate(resolve)); } throw new Error('Timed out waiting for test condition'); }
(async () => {
    const h = harness(); h.rows = Array.from({ length: 274 }, (_, i) => row(i, i % 2 ? 'pdf' : 'docx'));
    h.job = id => Number(id.slice(4)) % 4 === 0 ? { state: 'found', hasDatabaseData: true, targetPermId: 123 }
        : { state: 'missing', hasDatabaseData: false, targetPermId: 123 };
    await h.api.load();
    equal(h.el('emailTotal').textContent, '137', 'total is entire filtered dataset');
    equal(h.el('emailSent').textContent, '69'); equal(h.el('emailFailed').textContent, '68');
    equal(h.rowCount(), 50); equal(h.api.getState().totalPages, 3);
    equal(h.requests.filter(r => r.url === 'email').length, 3);
    ok(h.maxJobs <= 4, 'database concurrency bounded');
    const requestCount = h.requests.length;
    h.el('emailNext').onclick(); equal(h.rowCount(), 50); ok(h.el('emailRows').innerHTML.includes('data-index="50"')); ok(h.el('emailRows').innerHTML.includes('<td>51</td>'));
    equal(h.el('emailTotal').textContent, '137'); equal(h.el('emailSent').textContent, '69'); equal(h.el('emailFailed').textContent, '68');
    h.el('emailNext').onclick(); equal(h.rowCount(), 37); equal(h.el('emailNext').disabled, true);
    h.el('emailPageSize').value = '100'; h.el('emailPageSize').onchange(); equal(h.rowCount(), 100); equal(h.api.getState().currentPage, 1);
    h.el('emailNext').onclick(); equal(h.rowCount(), 37);
    h.el('emailPageSize').value = '25'; h.el('emailPageSize').onchange(); equal(h.rowCount(), 25); equal(h.api.getState().totalPages, 6);
    equal(h.requests.length, requestCount, 'pagination never re-fetches/recounts a different dataset');
    equal((await h.api.loadReportItems()).length, 137, 'export includes all filtered pages');
    h.el('emailReportSender').value = 'other@example.test'; await assert.rejects(h.api.loadReportItems()); count++;
    h.el('emailReportSender').value = ''; h.el('emailSearch').value = 'not-applied';
    equal((await h.api.loadReportItems()).length, 137, 'export uses applied search snapshot');

    const large = harness(); large.rows = Array.from({ length: 1206 }, (_, i) => ({ ...row(i, i % 2 ? 'pdf' : 'docx'), syncJobId: '' }));
    await large.api.load(); equal(large.el('emailTotal').textContent, '603'); equal(large.requests.length, 13, 'no five-page/500-row truncation for list');
    equal((await large.api.loadReportItems()).length, 500, 'export cap preserved separately');

    for (const n of [0, 1, 49, 50, 51, 100, 101]) {
        const t = harness(); t.rows = Array.from({ length: n }, (_, i) => ({ ...row(i), syncJobId: '' })); await t.api.load();
        equal(t.el('emailTotal').textContent, String(n)); equal(t.rowCount(), Math.min(n, 50));
        equal(t.api.getState().totalPages, Math.max(1, Math.ceil(n / 50)));
        if (n > 50) { t.el('emailNext').onclick(); equal(t.rowCount(), Math.min(n - 50, 50)); }
    }
    const states = harness(); states.rows = Array.from({ length: 5 }, (_, i) => row(i));
    states.job = id => {
        const i = Number(id.slice(4));
        if (i === 4) throw new Error('offline');
        return { state: ['found','missing','error','pending'][i], hasDatabaseData: i === 0, targetPermId: i < 2 ? 123 : null };
    };
    await states.api.load(); equal(states.el('emailTotal').textContent, '5'); equal(states.el('emailSent').textContent, '1'); equal(states.el('emailFailed').textContent, '2');
    ok(states.el('emailStatsInfo').textContent.includes('Đang chờ xử lý: 1'));
    ok(states.el('emailStatsInfo').textContent.includes('Không kiểm tra được DB: 1'));
    equal(states.el('emailReportDownload').disabled, false, 'pending does not lock reports');
    states.job = () => ({ state: 'found', hasDatabaseData: true, targetPermId: 789 });
    await states.el('emailRefresh').onclick(); equal(states.el('emailSent').textContent, '5'); equal(states.el('emailFailed').textContent, '0');
    equal(states.requests.filter(r => r.url === 'jobs').length, 10, 'refresh invalidates job cache');

    const cached = harness(); cached.rows = [row(1), { ...row(2), syncJobId: 'job-1' }];
    cached.job = () => ({ state: 'missing', hasDatabaseData: false, targetPermId: 123 }); await cached.api.load();
    equal(cached.el('emailSent').textContent, '0'); equal(cached.el('emailFailed').textContent, '2');
    equal(cached.requests.filter(r => r.url === 'jobs').length, 1, 'duplicate jobs share in-flight lookup');

    const incoming = harness(); incoming.rows = Array.from({ length: 240 }, (_, i) => ({ ...row(i, i % 2 ? 'pdf' : 'docx'), syncJobId: '',
        subject: i >= 100 ? 'needle' : 'other', processingStatus: i < 180 ? 'FAILED' : 'SAVED',
        receivedAt: i < 120 ? '2026-09-19T10:00:00' : '2026-09-20T10:00:00' }));
    incoming.el('emailSearch').value = 'needle'; incoming.el('emailStatus').value = 'FAILED';
    incoming.el('emailFrom').value = '2026-09-20'; incoming.el('emailTo').value = '2026-09-20';
    incoming.tabs[1].onclick(); await until(() => incoming.el('emailApply').disabled === false);
    equal(incoming.el('emailTotal').textContent, '30', 'Incoming applies query/status/date/file type across all pages');
    equal(incoming.requests.filter(r => r.url === 'incoming').length, 3);
    incoming.el('emailFrom').value = '2026-09-21'; const beforeInvalid = incoming.requests.length;
    await incoming.api.load(); equal(incoming.requests.length, beforeInvalid); equal(incoming.el('emailError').hidden, false);

    const slow = harness(); slow.rows = [row(1)]; let finish;
    slow.job = () => new Promise(resolve => { finish = resolve; });
    const slowLoad = slow.api.load(); await until(() => finish);
    equal(slow.el('emailTotal').textContent, '1'); equal(slow.el('emailSent').textContent, '--'); equal(slow.el('emailFailed').textContent, '--');
    slow.rows = [{ ...row(2), syncJobId: '' }]; await slow.api.load();
    finish({ state: 'found', hasDatabaseData: true, targetPermId: 123 }); await slowLoad;
    equal(slow.el('emailTotal').textContent, '1'); equal(slow.el('emailSent').textContent, '0'); equal(slow.el('emailFailed').textContent, '1', 'old DB response cannot contaminate new search');

    const race = harness(); let releaseOld;
    race.respond = (url, req) => req.query === 'old' ? new Promise(resolve => { releaseOld = () => resolve(response({ content: [row(1)], totalElements: 1, totalPages: 1, page: 0 })); })
        : response({ content: [{ ...row(2), syncJobId: '' }], totalElements: 1, totalPages: 1, page: 0 });
    race.el('emailSearch').value = 'old'; const oldLoad = race.api.load(); await until(() => releaseOld);
    race.el('emailSearch').value = 'new'; await race.api.load(); releaseOld(); await oldLoad;
    equal(race.api.getState().filteredItems[0].id, 2, 'old list response cannot replace newer results');

    for (const mode of ['http-error','missing-page','changed-total','invalid-json-shape','duplicate','wrong-page']) {
        const t = harness();
        t.respond = (url, req) => {
            if (mode === 'invalid-json-shape') return response({ error: 'bad' });
            if (req.page === 0) return response({ content: [row(1)], totalElements: 2, totalPages: 2, page: 0 });
            if (mode === 'http-error') return { ok: false, status: 503 };
            return response({ content: mode === 'missing-page' ? [] : [{ ...row(mode === 'duplicate' ? 1 : 2), syncJobId: '' }],
                totalElements: mode === 'changed-total' ? 3 : 2, totalPages: 2, page: mode === 'wrong-page' ? 0 : 1 });
        };
        await t.api.load(); equal(t.el('emailTotal').textContent, '--', mode + ' cannot publish a partial total'); equal(t.el('emailError').hidden, false); equal(t.rowCount(), 0);
    }
    const legacy = harness(); legacy.respond = (url, req) => response({ content: req.page === 0 ? [{ ...row(1), syncJobId: '' }] : [], hasNext: req.page === 0, size: 1 });
    await legacy.api.load(); equal(legacy.el('emailTotal').textContent, '1'); equal(legacy.requests.length, 2);
    const capped = harness(); capped.apiSize = 25; capped.rows = Array.from({ length: 101 }, (_, i) => ({ ...row(i), syncJobId: '' }));
    await capped.api.load(); equal(capped.el('emailTotal').textContent, '101'); equal(capped.rowCount(), 50); equal(capped.requests.length, 5);
    console.log('PASS: ' + count + ' assertions covering filtered totals, full pages, all-record KPI, cache, exports, Incoming, races and incomplete API data.');
})().catch(error => { console.error(error); process.exitCode = 1; });
