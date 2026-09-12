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
                (options.watermark ? '<Row><Cell ss:MergeAcross="' + (selected.length - 1) + '"><Data ss:Type="String">' + escapeHtml(options.watermark) + '</Data></Cell></Row>' : '') +
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

    function sheetXml(name, title, watermark, columns, rows) {
        var safeName = String(name || 'Report').replace(/[\[\]:*?\/\\]/g, ' ').replace(/\s+/g, ' ').trim().slice(0, 31) || 'Report';
        return '<Worksheet ss:Name="' + escapeHtml(safeName) + '"><Table>' +
            (watermark ? '<Row><Cell ss:MergeAcross="' + (columns.length - 1) + '"><Data ss:Type="String">' + escapeHtml(watermark) + '</Data></Cell></Row>' : '') +
            (title ? '<Row><Cell ss:MergeAcross="' + (columns.length - 1) + '"><Data ss:Type="String">' + escapeHtml(title) + '</Data></Cell></Row>' : '') +
            '<Row>' + columns.map(function (column) { return '<Cell><Data ss:Type="String">' + escapeHtml(column.label) + '</Data></Cell>'; }).join('') + '</Row>' +
            rows.map(function (row, rowIndex) { return '<Row>' + columns.map(function (column) {
                var value = column.key === '__no' ? rowIndex + 1 : row[column.key];
                if (column.format) value = column.format(value, row);
                return '<Cell><Data ss:Type="String">' + escapeHtml(value) + '</Data></Cell>';
            }).join('') + '</Row>'; }).join('') + '</Table></Worksheet>';
    }

    function downloadBlob(content, mime, fileName) {
        var url = URL.createObjectURL(new Blob([content], { type: mime }));
        var link = document.createElement('a');
        link.href = url; link.download = fileName;
        document.body.appendChild(link); link.click(); document.body.removeChild(link);
        setTimeout(function () { URL.revokeObjectURL(url); }, 0);
    }

    // Xuất .xls nhiều worksheet: options = {fileName, watermark, sheets:[{name,title,columns,rows}]}.
    function exportExcelSheets(options) {
        var xml = '<?xml version="1.0" encoding="UTF-8"?><?mso-application progid="Excel.Sheet"?>' +
            '<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">' +
            (options.sheets || []).map(function (sheet) {
                return sheetXml(sheet.name, sheet.title, options.watermark, sheet.columns || [], sheet.rows || []);
            }).join('') + '</Workbook>';
        downloadBlob(xml, 'application/vnd.ms-excel;charset=utf-8', (options.fileName || 'Report') + '.xls');
    }

    // Xuất CSV (UTF-8 BOM để Excel đọc đúng tiếng Việt): options = {fileName, columns, rows}.
    function exportCsv(options) {
        var columns = options.columns || [];
        function cell(value) {
            value = String(value == null ? '' : value);
            return /[",\r\n]/.test(value) ? '"' + value.replace(/"/g, '""') + '"' : value;
        }
        var lines = [columns.map(function (column) { return cell(column.label); }).join(',')];
        (options.rows || []).forEach(function (row, rowIndex) {
            lines.push(columns.map(function (column) {
                var value = column.key === '__no' ? rowIndex + 1 : row[column.key];
                if (column.format) value = column.format(value, row);
                return cell(value);
            }).join(','));
        });
        downloadBlob('﻿' + lines.join('\r\n'), 'text/csv;charset=utf-8', (options.fileName || 'Report') + '.csv');
    }

    // Nhân bản SVG và chuyển style từ CSS thành thuộc tính inline để vẽ được ngoài trang (in/ảnh).
    function inlineSvgStyles(svg) {
        var clone = svg.cloneNode(true);
        var props = ['fill', 'stroke', 'stroke-width', 'stroke-dasharray', 'stroke-dashoffset', 'stroke-linecap',
            'opacity', 'font-size', 'font-family', 'font-weight', 'text-anchor'];
        var sourceNodes = [svg].concat([].slice.call(svg.querySelectorAll('*')));
        var targetNodes = [clone].concat([].slice.call(clone.querySelectorAll('*')));
        sourceNodes.forEach(function (node, index) {
            if (!(node instanceof SVGElement)) return;
            var computed = window.getComputedStyle(node);
            props.forEach(function (prop) {
                var value = computed.getPropertyValue(prop);
                if (value && value !== 'none' || prop === 'fill' || prop === 'stroke') targetNodes[index].setAttribute(prop, value || 'none');
            });
            targetNodes[index].removeAttribute('class');
        });
        // CSS transform trên thẻ svg (vd donut xoay -90deg) không theo được khi serialize:
        // chuyển thành <g transform="rotate(...)"> quanh tâm viewBox.
        var matrix = window.getComputedStyle(svg).transform;
        var parsed = /^matrix\(([-0-9.e]+),\s*([-0-9.e]+),/.exec(matrix || '');
        if (parsed) {
            var angle = Math.round(Math.atan2(parseFloat(parsed[2]), parseFloat(parsed[1])) * 180 / Math.PI);
            if (angle) {
                var viewBox = (svg.getAttribute('viewBox') || '0 0 0 0').split(/\s+/).map(Number);
                var wrapper = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                wrapper.setAttribute('transform', 'rotate(' + angle + ' ' + (viewBox[0] + viewBox[2] / 2) + ' ' + (viewBox[1] + viewBox[3] / 2) + ')');
                while (clone.firstChild) wrapper.appendChild(clone.firstChild);
                clone.appendChild(wrapper);
            }
        }
        clone.removeAttribute('style');
        // width="100%" không cho ra kích thước thật khi vẽ lên canvas — thay bằng số px đo được.
        var widthAttr = clone.getAttribute('width');
        var heightAttr = clone.getAttribute('height');
        if (!widthAttr || widthAttr.indexOf('%') >= 0 || !heightAttr || heightAttr.indexOf('%') >= 0) {
            var box = (svg.getAttribute('viewBox') || '0 0 600 360').split(/\s+/).map(Number);
            var realWidth = svg.clientWidth || box[2] || 600;
            clone.setAttribute('width', realWidth);
            clone.setAttribute('height', svg.clientHeight || (box[2] ? Math.round(realWidth * box[3] / box[2]) : 360));
        }
        return clone;
    }

    // Tải các SVG về một file PNG (xếp dọc): options = {fileName, title, svgs:[SVGElement], scale}.
    function exportSvgsAsImage(options) {
        var scale = options.scale || 2;
        var pad = 24, titleHeight = options.title ? 40 : 0;
        var items = (options.svgs || []).filter(Boolean).map(function (svg) {
            var clone = inlineSvgStyles(svg);
            var width = parseFloat(clone.getAttribute('width')) || 600;
            var height = parseFloat(clone.getAttribute('height')) || 360;
            return { width: width, height: height, uri: 'data:image/svg+xml;charset=utf-8,' + encodeURIComponent(new XMLSerializer().serializeToString(clone)) };
        });
        if (!items.length) { alert('Không có biểu đồ để xuất ảnh.'); return; }
        var totalWidth = Math.max.apply(null, items.map(function (item) { return item.width; })) + pad * 2;
        var totalHeight = items.reduce(function (sum, item) { return sum + item.height + pad; }, pad + titleHeight);
        var canvas = document.createElement('canvas');
        canvas.width = totalWidth * scale; canvas.height = totalHeight * scale;
        var ctx = canvas.getContext('2d');
        ctx.scale(scale, scale);
        ctx.fillStyle = '#ffffff'; ctx.fillRect(0, 0, totalWidth, totalHeight);
        if (options.title) {
            ctx.fillStyle = '#173f59'; ctx.font = '700 18px Roboto, Arial, sans-serif'; ctx.textAlign = 'center';
            ctx.fillText(options.title, totalWidth / 2, pad + 6);
        }
        var loaded = 0, offsetY = pad + titleHeight;
        items.forEach(function (item) {
            var image = new Image();
            item.y = offsetY; offsetY += item.height + pad;
            image.onload = function () {
                ctx.drawImage(image, (totalWidth - item.width) / 2, item.y, item.width, item.height);
                if (++loaded === items.length) canvas.toBlob(function (blob) {
                    var url = URL.createObjectURL(blob);
                    var link = document.createElement('a');
                    link.href = url; link.download = (options.fileName || 'Chart') + '.png';
                    document.body.appendChild(link); link.click(); document.body.removeChild(link);
                    setTimeout(function () { URL.revokeObjectURL(url); }, 0);
                });
            };
            image.onerror = function () { if (++loaded === items.length) alert('Không tạo được ảnh biểu đồ.'); };
            image.src = item.uri;
        });
    }

    // Mở cửa sổ in để người dùng Lưu thành PDF: options = {title, subtitle, meta:[{label,value}], watermark, sections:[{heading, html}]}.
    function printReport(options) {
        var win = window.open('', '_blank');
        if (!win) { alert('Trình duyệt chặn cửa sổ mới. Vui lòng cho phép popup để xuất PDF.'); return; }
        var meta = (options.meta || []).map(function (item) {
            return '<span><b>' + escapeHtml(item.label) + ':</b> ' + escapeHtml(item.value) + '</span>';
        }).join('');
        var sections = (options.sections || []).map(function (section) {
            return '<section>' + (section.heading ? '<h2>' + escapeHtml(section.heading) + '</h2>' : '') + section.html + '</section>';
        }).join('');
        win.document.write('<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + escapeHtml(options.title || 'Báo cáo') + '</title><style>' +
            'body{margin:24px;color:#1c3a52;font-family:Roboto,Arial,sans-serif;font-size:12px}' +
            'h1{margin:0 0 4px;color:#173f59;font-size:20px}' +
            'h2{margin:22px 0 8px;color:#20507a;font-size:14px}' +
            '.rpt-subtitle{margin:0 0 10px;color:#5d768c;font-size:12px}' +
            '.rpt-meta{display:flex;gap:18px;flex-wrap:wrap;margin:0 0 14px;padding:8px 0;border-top:1px solid #d9e4ee;border-bottom:1px solid #d9e4ee;color:#3d5a74}' +
            'table{width:100%;border-collapse:collapse;font-size:11px}' +
            'th,td{padding:6px 7px;border:1px solid #c4d5e2;text-align:left}' +
            'th{background:#eaf3fa;color:#1d4c72}' +
            'tr:nth-child(even) td{background:#f7fafc}' +
            'svg{max-width:100%;height:auto}' +
            'section{page-break-inside:avoid}section.rpt-page-break{page-break-before:always}' +
            '.rpt-watermark{position:fixed;inset:0;z-index:99;display:flex;align-items:center;justify-content:center;pointer-events:none}' +
            '.rpt-watermark span{color:rgba(31,79,120,.09);font-size:72px;font-weight:800;letter-spacing:4px;text-transform:uppercase;transform:rotate(-28deg);white-space:nowrap}' +
            '@media print{.rpt-noprint{display:none}}' +
            '</style></head><body>' +
            (options.watermark ? '<div class="rpt-watermark"><span>' + escapeHtml(options.watermark) + '</span></div>' : '') +
            '<h1>' + escapeHtml(options.title || 'Báo cáo') + '</h1>' +
            (options.subtitle ? '<p class="rpt-subtitle">' + escapeHtml(options.subtitle) + '</p>' : '') +
            (meta ? '<div class="rpt-meta">' + meta + '</div>' : '') +
            sections +
            '<script>window.onload=function(){window.focus();window.print()}<\/script></body></html>');
        win.document.close();
        return win;
    }

    window.ReportControls = { enhance: enhance, enhanceAll: enhanceAll, exportExcel: exportExcel, exportExcelSheets: exportExcelSheets, exportCsv: exportCsv, exportSvgsAsImage: exportSvgsAsImage, inlineSvgStyles: inlineSvgStyles, printReport: printReport };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', function () { enhanceAll(document); });
    else enhanceAll(document);
}());
