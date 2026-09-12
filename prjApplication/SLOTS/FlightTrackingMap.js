(function () {
    'use strict';
    var page = document.querySelector('.track-map-page');
    if (!page) return;

    var flightsEndpoint = page.getAttribute('data-flights-endpoint');
    var firEndpoint = page.getAttribute('data-fir-endpoint');
    var svg = document.getElementById('trackMapSvg');
    var baseLayer = document.getElementById('trackBaseLayer');
    var firLayer = document.getElementById('trackFirLayer');
    var planeLayer = document.getElementById('trackPlaneLayer');
    var gridLayer = document.getElementById('trackGridLayer');
    var popup = document.getElementById('trackPopup');
    var viewport = document.getElementById('trackMapViewport');
    var refreshButton = document.getElementById('trackRefreshButton');
    var geoJson = null;
    var baseGeoJson = null;
    var bounds = null;
    var refreshing = false;
    var markerNodes = {};
    var selectedMarker = null;
    var svgNs = 'http://www.w3.org/2000/svg';
    var defaultView = { x: 0, y: 0, width: 1100, height: 680 };
    var mapView = { x: 0, y: 0, width: 1100, height: 680 };
    var dragging = false;
    var dragPoint = null;

    function byId(id) { return document.getElementById(id); }
    function text(value) { return value == null ? '' : String(value); }
    function escapeHtml(value) {
        var node = document.createElement('div');
        node.appendChild(document.createTextNode(text(value)));
        return node.innerHTML;
    }
    function post(endpoint, body) {
        return fetch(endpoint, {
            method: 'POST',
            credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify(body || {}),
            atfmSilent: true
        }).then(function (response) {
            if (!response.ok) throw new Error('Máy chủ trả về HTTP ' + response.status + '.');
            return response.json();
        }).then(function (response) {
            var data = response.d;
            return typeof data === 'string' ? JSON.parse(data) : data;
        });
    }
    function eachPoint(coordinates, callback) {
        if (!Array.isArray(coordinates)) return;
        if (coordinates.length >= 2 && typeof coordinates[0] === 'number' && typeof coordinates[1] === 'number') {
            callback(coordinates[0], coordinates[1]);
            return;
        }
        coordinates.forEach(function (item) { eachPoint(item, callback); });
    }
    function computeBounds() {
        return { minLon: 84, minLat: -5, maxLon: 136, maxLat: 37 };
    }
    function project(lon, lat) {
        return {
            x: 45 + (lon - bounds.minLon) / (bounds.maxLon - bounds.minLon) * 1010,
            y: 635 - (lat - bounds.minLat) / (bounds.maxLat - bounds.minLat) * 590
        };
    }
    function geometryPolygons(geometry) {
        if (!geometry) return [];
        if (geometry.type === 'Polygon') return [geometry.coordinates];
        if (geometry.type === 'MultiPolygon') return geometry.coordinates;
        return [];
    }
    function ringPath(ring) {
        return ring.map(function (point, index) {
            var p = project(point[0], point[1]);
            return (index ? 'L' : 'M') + p.x.toFixed(2) + ',' + p.y.toFixed(2);
        }).join(' ') + ' Z';
    }
    function featurePath(feature) {
        var parts = [];
        geometryPolygons(feature.geometry).forEach(function (polygon) {
            polygon.forEach(function (ring) { parts.push(ringPath(ring)); });
        });
        return parts.join(' ');
    }
    function pointInRing(lon, lat, ring) {
        var inside = false;
        for (var i = 0, j = ring.length - 1; i < ring.length; j = i++) {
            var xi = ring[i][0], yi = ring[i][1], xj = ring[j][0], yj = ring[j][1];
            var cross = (lon - xi) * (yj - yi) - (lat - yi) * (xj - xi);
            var onSegment = Math.abs(cross) < 1e-9 && lon >= Math.min(xi, xj) - 1e-9 && lon <= Math.max(xi, xj) + 1e-9 &&
                lat >= Math.min(yi, yj) - 1e-9 && lat <= Math.max(yi, yj) + 1e-9;
            if (onSegment) return true;
            var intersects = ((yi > lat) !== (yj > lat)) &&
                (lon < (xj - xi) * (lat - yi) / ((yj - yi) || 1e-30) + xi);
            if (intersects) inside = !inside;
        }
        return inside;
    }
    function pointInPolygon(lon, lat, polygon) {
        if (!polygon.length || !pointInRing(lon, lat, polygon[0])) return false;
        for (var index = 1; index < polygon.length; index++) {
            if (pointInRing(lon, lat, polygon[index])) return false;
        }
        return true;
    }
    function insideFir(flight) {
        var lon = Number(flight.longitude), lat = Number(flight.latitude), hit = false;
        (geoJson.features || []).some(function (feature) {
            return geometryPolygons(feature.geometry).some(function (polygon) {
                if (pointInPolygon(lon, lat, polygon)) { hit = true; return true; }
                return false;
            });
        });
        return hit;
    }
    function renderGrid() {
        gridLayer.innerHTML = '';
        var lonStart = Math.ceil(bounds.minLon / 2) * 2;
        var latStart = Math.ceil(bounds.minLat / 2) * 2;
        var lon, lat, p1, p2, line, label;
        for (lon = lonStart; lon <= bounds.maxLon; lon += 2) {
            p1 = project(lon, bounds.minLat); p2 = project(lon, bounds.maxLat);
            line = document.createElementNS(svgNs, 'line'); line.setAttribute('class', 'track-grid-line');
            line.setAttribute('x1', p1.x); line.setAttribute('y1', p1.y); line.setAttribute('x2', p2.x); line.setAttribute('y2', p2.y); gridLayer.appendChild(line);
            label = document.createElementNS(svgNs, 'text'); label.setAttribute('class', 'track-grid-text');
            label.setAttribute('x', p1.x + 4); label.setAttribute('y', 667); label.textContent = lon + '°E'; gridLayer.appendChild(label);
        }
        for (lat = latStart; lat <= bounds.maxLat; lat += 2) {
            p1 = project(bounds.minLon, lat); p2 = project(bounds.maxLon, lat);
            line = document.createElementNS(svgNs, 'line'); line.setAttribute('class', 'track-grid-line');
            line.setAttribute('x1', p1.x); line.setAttribute('y1', p1.y); line.setAttribute('x2', p2.x); line.setAttribute('y2', p2.y); gridLayer.appendChild(line);
            label = document.createElementNS(svgNs, 'text'); label.setAttribute('class', 'track-grid-text');
            label.setAttribute('x', 7); label.setAttribute('y', p1.y - 4); label.textContent = lat + '°N'; gridLayer.appendChild(label);
        }
    }
    function addMapLabel(label) {
        var location = project(label.lon, label.lat);
        var dot = document.createElementNS(svgNs, 'circle');
        dot.setAttribute('class', 'base-city-dot'); dot.setAttribute('cx', location.x); dot.setAttribute('cy', location.y); dot.setAttribute('r', 2.5);
        baseLayer.appendChild(dot);
        var textNode = document.createElementNS(svgNs, 'text');
        textNode.setAttribute('class', label.country ? 'base-country-label' : 'base-city-label');
        textNode.setAttribute('x', location.x + (label.dx || 6)); textNode.setAttribute('y', location.y + (label.dy || -5));
        textNode.textContent = label.name; baseLayer.appendChild(textNode);
    }
    function renderBaseMap() {
        baseLayer.innerHTML = '';
        (baseGeoJson.features || []).forEach(function (feature) {
            var path = document.createElementNS(svgNs, 'path');
            path.setAttribute('d', featurePath(feature));
            path.setAttribute('class', 'base-country');
            path.setAttribute('fill-rule', 'evenodd');
            baseLayer.appendChild(path);
        });
        [
            { name: 'Vietnam', lon: 106.1, lat: 16.1, country: true },
            { name: 'Laos', lon: 103.8, lat: 18.3, country: true },
            { name: 'Cambodia', lon: 104.9, lat: 12.7, country: true },
            { name: 'Thailand', lon: 101.0, lat: 15.2, country: true },
            { name: 'Myanmar', lon: 96.9, lat: 20.7, country: true },
            { name: 'Malaysia', lon: 102.1, lat: 4.2, country: true },
            { name: 'Philippines', lon: 122.5, lat: 12.8, country: true },
            { name: 'China', lon: 111.5, lat: 29.0, country: true },
            { name: 'Hà Nội', lon: 105.84, lat: 21.03 },
            { name: 'Đà Nẵng', lon: 108.2, lat: 16.05 },
            { name: 'TP. Hồ Chí Minh', lon: 106.7, lat: 10.78 },
            { name: 'Bangkok', lon: 100.5, lat: 13.75 },
            { name: 'Manila', lon: 120.98, lat: 14.6 }
        ].forEach(addMapLabel);
    }
    function renderFir() {
        bounds = computeBounds(); renderBaseMap(); renderGrid(); firLayer.innerHTML = '';
        (geoJson.features || []).forEach(function (feature) {
            var code = text(feature.properties && feature.properties.id).toUpperCase();
            var path = document.createElementNS(svgNs, 'path');
            path.setAttribute('d', featurePath(feature));
            path.setAttribute('class', 'fir-shape ' + code.toLowerCase());
            path.setAttribute('fill-rule', 'evenodd'); firLayer.appendChild(path);
            var points = [];
            eachPoint(feature.geometry.coordinates, function (lon, lat) { points.push([lon, lat]); });
            if (points.length) {
                var lonAvg = points.reduce(function (sum, p) { return sum + p[0]; }, 0) / points.length;
                var latAvg = points.reduce(function (sum, p) { return sum + p[1]; }, 0) / points.length;
                var location = project(lonAvg, latAvg);
                var label = document.createElementNS(svgNs, 'text'); label.setAttribute('class', 'fir-label');
                label.setAttribute('x', location.x); label.setAttribute('y', location.y); label.setAttribute('text-anchor', 'middle');
                label.textContent = code; firLayer.appendChild(label);
            }
        });
    }
    function markerClass(permType) {
        var value = text(permType).trim().toUpperCase();
        return value === 'LD' ? 'ld' : (value === 'O/F' ? 'of' : 'other');
    }
    function createMarker(flight) {
        var group = document.createElementNS(svgNs, 'g');
        group.setAttribute('class', 'track-plane ' + markerClass(flight.permType));
        group.innerHTML = '<circle class="plane-hit" r="20"></circle><path class="plane-body" d="M0,-16 L4,-5 L15,2 L15,7 L4,4 L3,15 L-3,15 L-4,4 L-15,7 L-15,2 L-4,-5 Z"></path>';
        group.addEventListener('click', function (event) {
            selectMarker(group);
            showPopup(group._flight, event);
        });
        planeLayer.appendChild(group); return group;
    }
    function selectMarker(marker) {
        if (selectedMarker && selectedMarker !== marker) selectedMarker.classList.remove('is-selected');
        selectedMarker = marker || null;
        if (selectedMarker) selectedMarker.classList.add('is-selected');
    }
    function markerScale() {
        var viewRatio = mapView.width / defaultView.width;
        return Math.max(.28, Math.pow(viewRatio, 1.15));
    }
    function positionMarker(node, flight) {
        var p = project(Number(flight.longitude), Number(flight.latitude));
        node.setAttribute('transform', 'translate(' + p.x.toFixed(2) + ' ' + p.y.toFixed(2) + ') rotate(' +
            Number(flight.heading || 0).toFixed(1) + ') scale(' + markerScale().toFixed(3) + ')');
    }
    function showPopup(flight, event) {
        var markerType = markerClass(flight.permType);
        var permClass = markerType === 'ld' ? 'perm-ld' : (markerType === 'of' ? 'perm-of' : 'perm-other');
        popup.innerHTML = '<h3>' + escapeHtml(flight.callsign || flight.flightIdCurrent) + '</h3><dl>' +
            '<dt>Flight ID</dt><dd>' + escapeHtml(flight.flightIdCurrent) + '</dd>' +
            '<dt>OPER_ID</dt><dd>' + escapeHtml(flight.operId || 'Chưa đối chiếu') + '</dd>' +
            '<dt>PERMTYPE</dt><dd class="' + permClass + '">' + escapeHtml(flight.permType || 'Chưa xác định') + '</dd>' +
            '<dt>Tọa độ</dt><dd>' + Number(flight.latitude).toFixed(4) + ', ' + Number(flight.longitude).toFixed(4) + '</dd>' +
            '<dt>Cập nhật</dt><dd>' + escapeHtml(flight.updatedAt) + '</dd></dl>';
        popup.hidden = false;
        var rect = viewport.getBoundingClientRect();
        var left = Math.min(rect.width - 276, Math.max(12, event.clientX - rect.left - 25));
        var top = Math.max(12, event.clientY - rect.top - popup.offsetHeight - 20);
        popup.style.left = left + 'px'; popup.style.top = top + 'px';
    }
    function renderFlights(allFlights) {
        var flights = (allFlights || []).filter(insideFir);
        var active = {}, ld = 0, of = 0, other = 0;
        flights.forEach(function (flight) {
            var key = text(flight.flightIdCurrent || flight.callsign);
            active[key] = true;
            var node = markerNodes[key] || createMarker(flight);
            markerNodes[key] = node;
            node._flight = flight;
            node.setAttribute('class', 'track-plane ' + markerClass(flight.permType) + (node === selectedMarker ? ' is-selected' : ''));
            positionMarker(node, flight);
            if (markerClass(flight.permType) === 'ld') ld++;
            if (markerClass(flight.permType) === 'of') of++;
            if (markerClass(flight.permType) === 'other') other++;
        });
        Object.keys(markerNodes).forEach(function (key) {
            if (!active[key]) {
                if (markerNodes[key] === selectedMarker) selectedMarker = null;
                markerNodes[key].parentNode.removeChild(markerNodes[key]); delete markerNodes[key];
            }
        });
        byId('trackTotal').textContent = flights.length;
        byId('trackLd').textContent = ld; byId('trackOf').textContent = of;
        byId('trackOther').textContent = other;
        byId('trackEmpty').hidden = flights.length !== 0;
    }
    function refreshFlights(initial) {
        if (refreshing || !geoJson) return;
        refreshing = true; refreshButton.disabled = true;
        if (initial) byId('trackMessage').textContent = 'Đang tải vị trí chuyến bay...';
        post(flightsEndpoint).then(function (data) {
            renderFlights(data.flights || []);
            byId('trackDayLabel').textContent = 'Dữ liệu ngày ' + text(data.day);
            byId('trackUpdatedAt').textContent = text(data.serverTime);
            byId('trackMessage').textContent = 'Vị trí được cập nhật tự động mỗi 5 giây';
        }).catch(function (error) {
            byId('trackMessage').textContent = error.message || 'Không thể cập nhật vị trí chuyến bay.';
        }).then(function () { refreshing = false; refreshButton.disabled = false; });
    }
    function applyView() {
        svg.setAttribute('viewBox', [mapView.x, mapView.y, mapView.width, mapView.height].join(' '));
        Object.keys(markerNodes).forEach(function (key) {
            var node = markerNodes[key];
            if (node && node._flight) positionMarker(node, node._flight);
        });
        popup.hidden = true;
    }
    function zoom(factor) {
        var nextWidth = Math.max(360, Math.min(defaultView.width, mapView.width * factor));
        var nextHeight = nextWidth * defaultView.height / defaultView.width;
        mapView.x += (mapView.width - nextWidth) / 2;
        mapView.y += (mapView.height - nextHeight) / 2;
        mapView.width = nextWidth; mapView.height = nextHeight;
        applyView();
    }
    document.getElementById('trackZoomIn').addEventListener('click', function () { zoom(.72); });
    document.getElementById('trackZoomOut').addEventListener('click', function () { zoom(1.38); });
    document.getElementById('trackResetView').addEventListener('click', function () {
        mapView = { x: 0, y: 0, width: 1100, height: 680 }; applyView();
    });
    svg.addEventListener('wheel', function (event) {
        event.preventDefault(); zoom(event.deltaY < 0 ? .84 : 1.19);
    }, { passive: false });
    svg.addEventListener('pointerdown', function (event) {
        if (event.target.closest && event.target.closest('.track-plane')) return;
        dragging = true; dragPoint = { x: event.clientX, y: event.clientY }; svg.setPointerCapture(event.pointerId); svg.classList.add('is-dragging');
    });
    svg.addEventListener('pointermove', function (event) {
        if (!dragging || !dragPoint || mapView.width >= defaultView.width) return;
        var rect = svg.getBoundingClientRect();
        mapView.x -= (event.clientX - dragPoint.x) * mapView.width / rect.width;
        mapView.y -= (event.clientY - dragPoint.y) * mapView.height / rect.height;
        mapView.x = Math.max(0, Math.min(defaultView.width - mapView.width, mapView.x));
        mapView.y = Math.max(0, Math.min(defaultView.height - mapView.height, mapView.y));
        dragPoint = { x: event.clientX, y: event.clientY }; applyView();
    });
    function stopDragging() { dragging = false; dragPoint = null; svg.classList.remove('is-dragging'); }
    svg.addEventListener('pointerup', stopDragging); svg.addEventListener('pointercancel', stopDragging);
    refreshButton.addEventListener('click', function () { refreshFlights(false); });
    viewport.addEventListener('click', function (event) {
        if (!event.target.closest || !event.target.closest('.track-plane')) {
            popup.hidden = true;
            selectMarker(null);
        }
    });
    post(firEndpoint).then(function (data) {
        geoJson = JSON.parse(data.geoJson); baseGeoJson = JSON.parse(data.baseGeoJson); renderFir(); refreshFlights(true);
        window.setInterval(function () { refreshFlights(false); }, 5000);
    }).catch(function (error) {
        byId('trackMessage').textContent = error.message || 'Không thể tải dữ liệu vùng FIR.';
    });
}());
