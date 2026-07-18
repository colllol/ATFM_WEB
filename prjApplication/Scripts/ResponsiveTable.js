(function () {
    'use strict';

    var layouts = [];
    var refreshFrame = null;

    function isEligible(container) {
        if (!container || container.getAttribute('data-atfm-responsive-table') === 'off') return false;
        if (container.closest && (container.closest('.modal') || container.closest('#table-container'))) return false;
        return !!container.querySelector('table');
    }

    function ensureLegacyContainers() {
        var tables = document.querySelectorAll(
            'table#tblSource, table[id$="_tblSource"], table[id$="_grdSource"], table#grdSource, ' +
            'table[id*="GridView"], table[id^="gv"], table[id*="_gv"], ' +
            'table.table.table-bordered, table.table.table-condensed, table.table.table-hover'
        );
        for (var i = 0; i < tables.length; i++) {
            var table = tables[i];
            if (table.getAttribute('data-atfm-responsive-table') === 'off') continue;
            if (table.closest && (table.closest('.modal') || table.closest('#table-container'))) continue;
            if (table.parentElement && table.parentElement.closest && table.parentElement.closest('table')) continue;

            // Một số trang cũ gắn nhầm .table-responsive trực tiếp lên table.
            // Chuẩn hóa lại thành div bao ngoài để thanh cuộn hoạt động đúng.
            var responsiveParent = table.parentElement && table.parentElement.closest
                ? table.parentElement.closest('.table-responsive')
                : null;
            if (responsiveParent) continue;
            if ((' ' + table.className + ' ').indexOf(' table-responsive ') >= 0) {
                table.classList.remove('table-responsive');
            }

            var container = document.createElement('div');
            container.className = 'table-responsive atfm-generated-table-responsive';
            table.parentNode.insertBefore(container, table);
            container.appendChild(table);
        }
    }

    function getColumnStep(container) {
        var table = container.querySelector('table');
        var row = table && table.rows && table.rows.length ? table.rows[0] : null;
        if (!row || !row.cells || !row.cells.length) return 120;
        var visibleCell = null;
        for (var i = 0; i < row.cells.length; i++) {
            if (row.cells[i].offsetWidth > 0) {
                visibleCell = row.cells[i];
                break;
            }
        }
        return visibleCell ? Math.max(60, Math.round(visibleCell.getBoundingClientRect().width)) : 120;
    }

    function bindButton(button, container, direction) {
        var holdTimer = null;
        var holdFrame = null;
        var holding = false;

        function scrollOneColumn() {
            var distance = direction * getColumnStep(container);
            if (container.scrollBy) container.scrollBy({ left: distance, behavior: 'smooth' });
            else container.scrollLeft += distance;
        }

        function continuousScroll() {
            container.scrollLeft += direction * 4;
            holdFrame = window.requestAnimationFrame(continuousScroll);
        }

        function start(event) {
            if (event.button !== undefined && event.button !== 0) return;
            event.preventDefault();
            holding = false;
            holdTimer = window.setTimeout(function () {
                holding = true;
                button.classList.add('is-holding');
                continuousScroll();
            }, 280);
        }

        function stop(event) {
            if (holdTimer === null && holdFrame === null) return;
            window.clearTimeout(holdTimer);
            holdTimer = null;
            if (holdFrame !== null) window.cancelAnimationFrame(holdFrame);
            holdFrame = null;
            button.classList.remove('is-holding');
            if (!holding) scrollOneColumn();
            holding = false;
            if (event) event.preventDefault();
        }

        button.addEventListener('pointerdown', start);
        button.addEventListener('pointerup', stop);
        button.addEventListener('pointercancel', stop);
        button.addEventListener('lostpointercapture', stop);
        button.addEventListener('click', function (event) {
            if (event.detail === 0) scrollOneColumn();
        });
    }

    function createControls(layout, container) {
        var controls = document.createElement('div');
        controls.className = 'atfm-responsive-table-controls';
        controls.setAttribute('aria-label', 'Điều hướng bảng theo chiều ngang');

        var left = document.createElement('button');
        left.type = 'button';
        left.className = 'atfm-responsive-table-arrow';
        left.setAttribute('title', 'Cuộn bảng sang trái');
        left.setAttribute('aria-label', 'Cuộn bảng sang trái');
        left.setAttribute('data-atfm-loading-ignore', 'true');
        left.innerHTML = '&#10094;';

        var right = document.createElement('button');
        right.type = 'button';
        right.className = 'atfm-responsive-table-arrow';
        right.setAttribute('title', 'Cuộn bảng sang phải');
        right.setAttribute('aria-label', 'Cuộn bảng sang phải');
        right.setAttribute('data-atfm-loading-ignore', 'true');
        right.innerHTML = '&#10095;';

        controls.appendChild(left);
        controls.appendChild(right);
        layout.appendChild(controls);
        bindButton(left, container, -1);
        bindButton(right, container, 1);
        return controls;
    }

    function ensureLayout(container) {
        if (!isEligible(container) || container.getAttribute('data-atfm-responsive-bound') === 'true') return;
        var layout = document.createElement('div');
        layout.className = 'atfm-responsive-table-layout';
        container.parentNode.insertBefore(layout, container);
        layout.appendChild(container);
        container.setAttribute('data-atfm-responsive-bound', 'true');
        layouts.push({ layout: layout, container: container, controls: null });
    }

    function refreshLayout(item) {
        if (!document.documentElement.contains(item.container)) return false;
        var table = item.container.querySelector('table');
        if (!table) return false;

        var containerTop = item.container.getBoundingClientRect().top;
        var availableHeight = window.innerHeight - Math.max(0, containerTop) - 86;
        var maxTableHeight = Math.max(320, Math.min(620, availableHeight));
        item.container.style.setProperty('--atfm-table-max-height', Math.round(maxTableHeight) + 'px');

        var rowCount = table.rows ? table.rows.length : table.querySelectorAll('tr').length;
        var hasVerticalOverflow = rowCount > 12 && item.container.scrollHeight > maxTableHeight + 2;
        item.layout.classList.toggle('has-vertical-overflow', hasVerticalOverflow);

        item.layout.classList.remove('has-horizontal-overflow');
        var overflow = item.container.scrollWidth > item.container.clientWidth + 2;
        item.layout.classList.toggle('has-horizontal-overflow', overflow);
        if (overflow && !item.controls) item.controls = createControls(item.layout, item.container);
        if (!overflow && item.controls) {
            item.controls.parentNode.removeChild(item.controls);
            item.controls = null;
        }
        return true;
    }

    function refreshAll() {
        refreshFrame = null;
        ensureLegacyContainers();
        var containers = document.querySelectorAll('.table-responsive');
        for (var i = 0; i < containers.length; i++) ensureLayout(containers[i]);
        var activeLayouts = [];
        for (var j = 0; j < layouts.length; j++) {
            if (refreshLayout(layouts[j])) activeLayouts.push(layouts[j]);
        }
        layouts = activeLayouts;
    }

    function scheduleRefresh() {
        if (refreshFrame !== null) return;
        refreshFrame = window.requestAnimationFrame(refreshAll);
    }

    function initialize() {
        refreshAll();
        window.addEventListener('resize', scheduleRefresh);
        if (window.MutationObserver) {
            new MutationObserver(scheduleRefresh).observe(document.body, { childList: true, subtree: true });
        }
        if (window.Sys && window.Sys.WebForms && window.Sys.WebForms.PageRequestManager) {
            window.Sys.WebForms.PageRequestManager.getInstance().add_endRequest(scheduleRefresh);
        }
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', initialize);
    else initialize();
})();
