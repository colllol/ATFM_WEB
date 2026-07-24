(function () {
    function camelize(value) { if (Array.isArray(value)) return value.map(camelize); if (!value || typeof value !== 'object') return value; var result = {}; Object.keys(value).forEach(function (key) { result[key.charAt(0).toLowerCase() + key.slice(1)] = camelize(value[key]); }); return result; }
    var airportNames = {
        VVNB: 'Nội Bài', VVTS: 'Tân Sơn Nhất', VVDN: 'Đà Nẵng', VVCR: 'Cam Ranh',
        VVPQ: 'Phú Quốc', VVCI: 'Cát Bi', VVDL: 'Liên Khương', VVPC: 'Phù Cát',
        VVVH: 'Vinh', VVCT: 'Cần Thơ', VVCA: 'Chu Lai', VVDB: 'Điện Biên',
        VVBM: 'Buôn Ma Thuột', VVTH: 'Tuy Hòa', VVPK: 'Pleiku', VVTX: 'Thọ Xuân',
        VVDH: 'Đồng Hới', VVRG: 'Rạch Giá', VVCM: 'Cà Mau', VVCS: 'Côn Đảo', VVVD: 'Vân Đồn'
    };

    var defaultOperators = [
        'AAR', 'APG', 'AXM', 'BAV', 'CAL', 'CEB', 'CES', 'CQH', 'DKH', 'ETD',
        'FDX', 'HVN', 'JAL', 'KAL', 'KHV', 'KLM', 'MAS', 'MKR', 'MXD', 'PIC',
        'QTR', 'SIA', 'THA', 'UAE', 'UAL', 'VAG', 'VJC'
    ];

    function post(method, data) {
        return fetch(window.reportApiBase + 'api/FlightTrendAnalysis/' + method, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(data)
        }).then(function (response) {
            return response.json().then(function (result) {
                if (!response.ok || (result.Code && result.Code !== '00')) throw new Error(result.Message || 'Không thể tải dữ liệu xu hướng.');
                return camelize(Object.prototype.hasOwnProperty.call(result, 'ListValue') ? result.ListValue : result.d);
            });
        });
    }

    function esc(value) {
        var node = document.createElement('div');
        node.textContent = value == null ? '' : value;
        return node.innerHTML;
    }

    function number(value) { return Number(value || 0).toLocaleString('vi-VN'); }
    function signed(value) { value = Number(value || 0); return (value > 0 ? '+' : '') + number(value); }
    function formatDate(value) {
        return value.getFullYear() + '-' + String(value.getMonth() + 1).padStart(2, '0') + '-' + String(value.getDate()).padStart(2, '0');
    }

    function init() {
        var app = document.querySelector('.rn-app[data-report-view="trend"]');
        if (!app) return;
        var filters = app.querySelector('.rn-filters');
        var airport = app.querySelector('#rnAirport');
        var from = app.querySelector('#rnFrom');
        var to = app.querySelector('#rnTo');
        var period = app.querySelector('#rnPeriod');
        var apply = app.querySelector('#rnApply');
        var today = new Date();
        var resizeHandler = null;

        filters.classList.add('rn-trend-filters');
        airport.innerHTML = '<option value="ALL">Tất cả sân bay</option>' + Object.keys(airportNames).map(function (code) {
            return '<option value="' + code + '">' + code + ' - ' + airportNames[code] + '</option>';
        }).join('');
        airport.parentNode.insertAdjacentHTML('afterend', '<div class="rn-field"><label>Hãng bay</label><select id="rnTrendOper"><option value="ALL">Tất cả hãng bay</option></select></div>');
        var oper = app.querySelector('#rnTrendOper');
        if (window.ReportControls) window.ReportControls.enhanceAll(filters);
        from.value = formatDate(today);
        to.value = formatDate(today);

        var heroSubtitle = app.querySelector('.rn-hero p');
        var liveNote = app.querySelector('.rn-live small');
        if (heroSubtitle) heroSubtitle.textContent = 'So sánh chuyến hoàn thành và delay với đúng cùng kỳ năm trước';
        if (liveNote) liveNote.textContent = '';

        function renderOperators(items, selected) {
            var unique = {};
            defaultOperators.concat(items || []).forEach(function (item) {
                var code = String(item || '').trim().toUpperCase();
                if (code && code !== 'ALL') unique[code] = true;
            });
            oper.innerHTML = '<option value="ALL">Tất cả hãng bay</option>' + Object.keys(unique).sort().map(function (item) {
                return '<option value="' + esc(item) + '">' + esc(item) + '</option>';
            }).join('');
            if (oper.querySelector('option[value="' + selected + '"]')) oper.value = selected;
        }

        function loadOperators() {
            var selected = oper.value;
            return post('GetOperators', { fromDate: from.value, toDate: to.value }).then(function (items) {
                renderOperators(items, selected);
            }).catch(function () {
                renderOperators([], selected);
            });
        }

        renderOperators([], 'ALL');

        function renderKpis(data) {
            var changeColor = data.difference >= 0 ? '#20b486' : '#ef5b5b';
            var changeText = (data.changePercent > 0 ? '+' : '') + Number(data.changePercent || 0).toLocaleString('vi-VN') + '%';
            var html = '<div class="rn-kpis">' +
                '<div class="rn-kpi" style="--accent:#2387c8"><span>Kỳ hiện tại</span><strong>' + number(data.currentTotal) + '</strong><small>Finished ' + number(data.currentFinished) + ' • Delay ' + number(data.currentDelay) + '</small></div>' +
                '<div class="rn-kpi" style="--accent:#9bb0c1"><span>Cùng kỳ năm trước</span><strong>' + number(data.previousTotal) + '</strong><small>' + esc(data.previousFrom + ' – ' + data.previousTo) + '</small></div>' +
                '<div class="rn-kpi" style="--accent:' + changeColor + '"><span>Chênh lệch</span><strong>' + signed(data.difference) + '</strong><small>Chuyến bay</small></div>' +
                '<div class="rn-kpi" style="--accent:#8a6ee8"><span>Tỷ lệ thay đổi</span><strong>' + changeText + '</strong><small>So với cùng kỳ</small></div></div>';
            var old = app.querySelector('.rn-kpis');
            if (old) old.outerHTML = html;
        }

        function renderChart(data) {
            var html = '<div class="rn-grid"><article class="rn-card wide rn-trend-card"><div class="rn-trend-head"><div><h2>Xu hướng khai thác theo ' + (data.period === 'month' ? 'tháng' : 'ngày') + '</h2><p class="rn-card-subtitle">Chỉ gồm chuyến Hoàn thành và Delay • Nguồn: ' + esc(data.source) + '</p></div><div class="rn-trend-legend"><span><i style="background:#9bb0c1"></i>Cùng kỳ năm trước</span><span><i style="background:#2387c8"></i>Kỳ hiện tại</span></div></div>' +
                '<div class="rn-trend-chart-wrap"><canvas id="rnTrendInteractive" class="rn-trend-canvas"></canvas><div id="rnTrendTooltip" class="rn-trend-tooltip" role="status"></div></div>' +
                '<p class="rn-trend-hint"><i class="fa fa-mouse-pointer"></i> Di chuột lên từng mốc để xem số lượng chuyến bay.</p></article></div>';
            var oldGrid = app.querySelector('.rn-grid');
            if (oldGrid) oldGrid.outerHTML = html;
            createChart(app.querySelector('#rnTrendInteractive'), app.querySelector('#rnTrendTooltip'), data);
        }

        function createChart(canvas, tooltip, data) {
            if (resizeHandler) window.removeEventListener('resize', resizeHandler);
            var labels = data.labels || [];
            var current = data.current || [];
            var previous = data.previous || [];
            var hoverIndex = -1;
            var progress = 0;
            var startedAt = null;
            var geometry = null;

            function drawLine(ctx, points, color, width) {
                if (!points.length) return;
                ctx.beginPath();
                ctx.moveTo(points[0].x, points[0].y);
                for (var i = 0; i < points.length - 1; i++) {
                    var currentPoint = points[i];
                    var nextPoint = points[i + 1];
                    var middleX = (currentPoint.x + nextPoint.x) / 2;
                    ctx.bezierCurveTo(middleX, currentPoint.y, middleX, nextPoint.y, nextPoint.x, nextPoint.y);
                }
                ctx.strokeStyle = color;
                ctx.lineWidth = width;
                ctx.lineCap = 'round';
                ctx.lineJoin = 'round';
                ctx.stroke();
            }

            function draw(valueProgress) {
                var rect = canvas.getBoundingClientRect();
                var width = Math.max(320, Math.round(rect.width));
                var height = Math.max(320, Math.round(rect.height));
                var dpr = window.devicePixelRatio || 1;
                var targetWidth = Math.round(width * dpr);
                var targetHeight = Math.round(height * dpr);
                if (canvas.width !== targetWidth) canvas.width = targetWidth;
                if (canvas.height !== targetHeight) canvas.height = targetHeight;
                var ctx = canvas.getContext('2d');
                ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
                ctx.clearRect(0, 0, width, height);
                var pad = { left: 58, right: 28, top: 34, bottom: 48 };
                var plotWidth = width - pad.left - pad.right;
                var plotHeight = height - pad.top - pad.bottom;
                var maximum = Math.max.apply(null, current.concat(previous).concat([1]));
                maximum = Math.max(5, Math.ceil(maximum * 1.15 / 5) * 5);
                var stepX = labels.length > 1 ? plotWidth / (labels.length - 1) : 0;
                var xAt = function (index) { return labels.length > 1 ? pad.left + stepX * index : pad.left + plotWidth / 2; };
                var yAt = function (value) { return pad.top + plotHeight - (value * valueProgress / maximum) * plotHeight; };

                ctx.font = '11px Roboto, Arial, sans-serif';
                ctx.textAlign = 'right';
                ctx.textBaseline = 'middle';
                for (var grid = 0; grid <= 5; grid++) {
                    var gridValue = maximum * (5 - grid) / 5;
                    var gridY = pad.top + plotHeight * grid / 5;
                    ctx.beginPath();
                    ctx.moveTo(pad.left, gridY);
                    ctx.lineTo(width - pad.right, gridY);
                    ctx.strokeStyle = '#e4edf4';
                    ctx.lineWidth = 1;
                    ctx.stroke();
                    ctx.fillStyle = '#7b8fa1';
                    ctx.fillText(number(Math.round(gridValue)), pad.left - 10, gridY);
                }

                var previousPoints = previous.map(function (value, index) { return { x: xAt(index), y: yAt(value), value: value }; });
                var currentPoints = current.map(function (value, index) { return { x: xAt(index), y: yAt(value), value: value }; });

                if (currentPoints.length) {
                    var fill = ctx.createLinearGradient(0, pad.top, 0, pad.top + plotHeight);
                    fill.addColorStop(0, 'rgba(35,135,200,.24)');
                    fill.addColorStop(1, 'rgba(35,135,200,0)');
                    ctx.beginPath();
                    ctx.moveTo(currentPoints[0].x, pad.top + plotHeight);
                    ctx.lineTo(currentPoints[0].x, currentPoints[0].y);
                    for (var fillIndex = 0; fillIndex < currentPoints.length - 1; fillIndex++) {
                        var fillCurrent = currentPoints[fillIndex];
                        var fillNext = currentPoints[fillIndex + 1];
                        var fillMiddle = (fillCurrent.x + fillNext.x) / 2;
                        ctx.bezierCurveTo(fillMiddle, fillCurrent.y, fillMiddle, fillNext.y, fillNext.x, fillNext.y);
                    }
                    ctx.lineTo(currentPoints[currentPoints.length - 1].x, pad.top + plotHeight);
                    ctx.closePath();
                    ctx.fillStyle = fill;
                    ctx.fill();
                }

                drawLine(ctx, previousPoints, '#9bb0c1', 3);
                drawLine(ctx, currentPoints, '#2387c8', 3.5);
                [previousPoints, currentPoints].forEach(function (points, seriesIndex) {
                    points.forEach(function (point, index) {
                        var active = index === hoverIndex;
                        ctx.beginPath();
                        ctx.arc(point.x, point.y, active ? 6 : 3.5, 0, Math.PI * 2);
                        ctx.fillStyle = seriesIndex ? '#2387c8' : '#9bb0c1';
                        ctx.fill();
                        if (active) {
                            ctx.lineWidth = 4;
                            ctx.strokeStyle = 'rgba(255,255,255,.95)';
                            ctx.stroke();
                        }
                    });
                });

                if (hoverIndex >= 0 && hoverIndex < labels.length) {
                    var guideX = xAt(hoverIndex);
                    ctx.save();
                    ctx.setLineDash([5, 5]);
                    ctx.beginPath();
                    ctx.moveTo(guideX, pad.top);
                    ctx.lineTo(guideX, pad.top + plotHeight);
                    ctx.strokeStyle = 'rgba(35,135,200,.45)';
                    ctx.stroke();
                    ctx.restore();
                }

                ctx.fillStyle = '#60778b';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'top';
                var labelStep = Math.max(1, Math.ceil(labels.length / 12));
                labels.forEach(function (label, index) {
                    if (index % labelStep === 0 || index === labels.length - 1) ctx.fillText(label, xAt(index), pad.top + plotHeight + 15);
                });
                geometry = { left: pad.left, right: width - pad.right, top: pad.top, bottom: pad.top + plotHeight, stepX: stepX, width: width, points: currentPoints };
            }

            function animate(timestamp) {
                if (!canvas.isConnected) return;
                if (startedAt === null) startedAt = timestamp;
                progress = Math.min(1, (timestamp - startedAt) / 650);
                var eased = 1 - Math.pow(1 - progress, 3);
                draw(eased);
                if (progress < 1) window.requestAnimationFrame(animate);
            }

            canvas.onmousemove = function (event) {
                if (!geometry || !labels.length) return;
                var rect = canvas.getBoundingClientRect();
                var mouseX = event.clientX - rect.left;
                var index = labels.length === 1 ? 0 : Math.round((mouseX - geometry.left) / geometry.stepX);
                index = Math.max(0, Math.min(labels.length - 1, index));
                if (mouseX < geometry.left - 18 || mouseX > geometry.right + 18) return;
                hoverIndex = index;
                draw(1);
                tooltip.innerHTML = '<strong>' + esc(labels[index]) + '</strong><span><i style="background:#2387c8"></i>Kỳ hiện tại: <b>' + number(current[index]) + ' chuyến</b></span><span><i style="background:#9bb0c1"></i>Cùng kỳ năm trước: <b>' + number(previous[index]) + ' chuyến</b></span>';
                tooltip.classList.add('is-visible');
                var tooltipLeft = geometry.points[index] ? geometry.points[index].x : mouseX;
                tooltip.style.left = Math.max(90, Math.min(geometry.width - 90, tooltipLeft)) + 'px';
                tooltip.style.top = Math.max(8, event.clientY - rect.top - 100) + 'px';
            };
            canvas.onmouseleave = function () {
                hoverIndex = -1;
                tooltip.classList.remove('is-visible');
                draw(1);
            };
            resizeHandler = function () { draw(1); };
            window.addEventListener('resize', resizeHandler);
            window.requestAnimationFrame(animate);
        }

        function load() {
            apply.disabled = true;
            apply.textContent = 'Đang tải...';
            post('GetTrend', {
                fromDate: from.value,
                toDate: to.value,
                airport: airport.value,
                oper: oper.value,
                period: period.value
            }).then(function (data) {
                renderKpis(data);
                renderChart(data);
                loadOperators();
            }).catch(function (error) {
                alert(error.message);
            }).then(function () {
                apply.disabled = false;
                apply.innerHTML = '<i class="fa fa-filter"></i> Áp dụng';
            });
        }

        apply.onclick = load;
        from.onchange = loadOperators;
        to.onchange = loadOperators;
        load();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else setTimeout(init, 0);
})();
