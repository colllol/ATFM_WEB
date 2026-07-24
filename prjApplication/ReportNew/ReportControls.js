(function () {
    'use strict';

    function escapeHtml(value) {
        return String(value == null ? '' : value).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    function enhance(select) {
        if (!select || select.dataset.searchableReady === '1') return;
        select.dataset.searchableReady = '1';
        var wrapper = document.createElement('div');
        wrapper.className = 'rn-search-select';
        var input = document.createElement('input');
        input.type = 'text';
        input.className = 'rn-search-select-input';
        input.autocomplete = 'off';
        input.placeholder = 'Nhập để tìm...';
        var list = document.createElement('div');
        list.className = 'rn-search-select-list';
        select.parentNode.insertBefore(wrapper, select);
        wrapper.appendChild(input);
        wrapper.appendChild(select);
        wrapper.appendChild(list);

        function selectedText() {
            return select.selectedIndex >= 0 ? select.options[select.selectedIndex].text : '';
        }
        function draw(query) {
            query = String(query || '').trim().toLocaleLowerCase('vi');
            var html = [];
            Array.prototype.forEach.call(select.options, function (option) {
                if (!query || option.text.toLocaleLowerCase('vi').indexOf(query) >= 0 || option.value.toLocaleLowerCase('vi').indexOf(query) >= 0) {
                    html.push('<button type="button" data-value="' + escapeHtml(option.value) + '">' + escapeHtml(option.text) + '</button>');
                }
            });
            list.innerHTML = html.length ? html.join('') : '<span>Không tìm thấy dữ liệu</span>';
            Array.prototype.forEach.call(list.querySelectorAll('button'), function (button) {
                button.onclick = function () {
                    select.value = this.getAttribute('data-value');
                    input.value = selectedText();
                    list.classList.remove('is-open');
                    select.dispatchEvent(new Event('change', { bubbles: true }));
                };
            });
        }
        function sync() { input.value = selectedText(); }
        input.onfocus = function () { draw(''); list.classList.add('is-open'); this.select(); };
        input.oninput = function () { draw(this.value); list.classList.add('is-open'); };
        input.onkeydown = function (event) {
            if (event.key === 'Escape') { list.classList.remove('is-open'); sync(); }
        };
        select.addEventListener('change', sync);
        new MutationObserver(sync).observe(select, { childList: true });
        document.addEventListener('click', function (event) {
            if (!wrapper.contains(event.target)) { list.classList.remove('is-open'); sync(); }
        });
        sync();
    }

    function enhanceAll(root) {
        Array.prototype.forEach.call((root || document).querySelectorAll(
            '.rn-filters .rn-field select,.rn-military-filters .rn-field select,.adsb-filter-card select,.airport-filter select[data-searchable="true"]'
        ), enhance);
    }

    function exportExcel(options) {
        var columns = options.columns || [];
        var rows = options.rows || [];
        var backdrop = document.createElement('div');
        backdrop.className = 'rn-export-backdrop is-open';
        backdrop.innerHTML = '<div class="rn-export-popup" role="dialog" aria-modal="true">' +
            '<div class="rn-export-header"><h4><i class="fa fa-file-excel-o"></i> Chọn trường xuất Excel</h4><button type="button" data-close>&times;</button></div>' +
            '<div class="rn-export-body"><label class="rn-export-title">Tiêu đề báo cáo<input type="text" maxlength="200" placeholder="Nhập tiêu đề hiển thị trong file Excel"></label>' +
            '<label class="rn-export-all"><input type="checkbox" checked> Chọn tất cả</label><div class="rn-export-grid">' +
            columns.map(function (column, index) { return '<label><input type="checkbox" data-index="' + index + '" checked> ' + escapeHtml(column.label) + '</label>'; }).join('') +
            '</div></div><div class="rn-export-footer"><button type="button" data-close>Hủy</button><button type="button" class="rn-export-submit"><i class="fa fa-download"></i> Export Excel</button></div></div>';
        document.body.appendChild(backdrop);
        var checks = backdrop.querySelectorAll('.rn-export-grid input');
        var all = backdrop.querySelector('.rn-export-all input');
        function close() { if (backdrop.parentNode) backdrop.parentNode.removeChild(backdrop); }
        Array.prototype.forEach.call(backdrop.querySelectorAll('[data-close]'), function (button) { button.onclick = close; });
        all.onchange = function () { Array.prototype.forEach.call(checks, function (check) { check.checked = all.checked; }); };
        Array.prototype.forEach.call(checks, function (check) {
            check.onchange = function () { all.checked = backdrop.querySelectorAll('.rn-export-grid input:checked').length === checks.length; };
        });
        backdrop.querySelector('.rn-export-submit').onclick = function () {
            var selected = [];
            Array.prototype.forEach.call(backdrop.querySelectorAll('.rn-export-grid input:checked'), function (check) {
                selected.push(columns[parseInt(check.getAttribute('data-index'), 10)]);
            });
            if (!selected.length) { alert('Vui lòng chọn ít nhất một trường để xuất Excel.'); return; }
            var title = backdrop.querySelector('.rn-export-title input').value.trim();
            var xml = '<?xml version="1.0" encoding="UTF-8"?><?mso-application progid="Excel.Sheet"?>' +
                '<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"><Worksheet ss:Name="Report"><Table>' +
                (title ? '<Row><Cell ss:MergeAcross="' + (selected.length - 1) + '"><Data ss:Type="String">' + escapeHtml(title) + '</Data></Cell></Row>' : '') +
                '<Row>' + selected.map(function (column) { return '<Cell><Data ss:Type="String">' + escapeHtml(column.label) + '</Data></Cell>'; }).join('') + '</Row>' +
                rows.map(function (row, rowIndex) { return '<Row>' + selected.map(function (column) {
                    var value = column.key === '__no' ? rowIndex + 1 : row[column.key];
                    if (column.format) value = column.format(value, row);
                    return '<Cell><Data ss:Type="String">' + escapeHtml(value) + '</Data></Cell>';
                }).join('') + '</Row>'; }).join('') + '</Table></Worksheet></Workbook>';
            var url = URL.createObjectURL(new Blob([xml], { type: 'application/vnd.ms-excel;charset=utf-8' }));
            var link = document.createElement('a');
            link.href = url; link.download = (options.fileName || 'Report') + '.xls';
            document.body.appendChild(link); link.click(); document.body.removeChild(link);
            setTimeout(function () { URL.revokeObjectURL(url); }, 0);
            close();
        };
    }

    window.ReportControls = { enhance: enhance, enhanceAll: enhanceAll, exportExcel: exportExcel };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', function () { enhanceAll(document); });
    else enhanceAll(document);
}());
