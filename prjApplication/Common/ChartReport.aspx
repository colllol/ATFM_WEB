<%@ Page Title="Flight Dashboard" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ChartReport.aspx.cs" Inherits="prjApplication.Common.ChartReport" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="prjApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .flight-dashboard { --finished:#20b486; --cancel:#ef5b5b; --delay:#f5a623; --wait:#4d8df7; color:#243b53; }
        .dashboard-heading { display:flex; align-items:flex-end; justify-content:space-between; gap:16px; margin:0 0 20px; }
        .dashboard-heading h2 { margin:0 0 5px; color:#163f63; font-size:25px; font-weight:700; }
        .dashboard-heading p { margin:0; color:#718096; }
        .dashboard-date { padding:8px 13px; border:1px solid #dce7f0; border-radius:20px; background:#fff; color:#52677a; box-shadow:0 4px 14px rgba(31,55,78,.07); }
        .dashboard-grid { display:grid; grid-template-columns:minmax(360px,1.05fr) minmax(420px,1.45fr); gap:20px; }
        .dashboard-card { border:1px solid #dce7f0; border-radius:16px; background:#fff; box-shadow:0 8px 26px rgba(31,55,78,.09); }
        .chart-card { min-height:410px; padding:22px; }
        .card-title { margin:0; color:#294c68; font-size:17px; font-weight:700; }
        .card-subtitle { margin:5px 0 0; color:#8393a3; font-size:12px; }
        .donut-wrap { position:relative; width:310px; max-width:100%; margin:18px auto 0; }
        #flightDonut { display:block; width:100%; height:auto; overflow:visible; transform:rotate(-90deg); }
        .donut-track { fill:none; stroke:#edf2f7; stroke-width:28; }
        .donut-segment { fill:none; stroke-width:28; cursor:pointer; transition:opacity .2s, stroke-width .2s, filter .2s; }
        .donut-segment:hover, .donut-segment.is-active { stroke-width:34; filter:drop-shadow(0 3px 4px rgba(24,50,74,.25)); opacity:1 !important; }
        .donut-wrap.has-active .donut-segment:not(.is-active) { opacity:.35; }
        .donut-center { position:absolute; inset:0; display:flex; flex-direction:column; align-items:center; justify-content:center; pointer-events:none; }
        .donut-center strong { color:#173f5f; font-size:38px; line-height:1; }
        .donut-center span { margin-top:7px; color:#8493a2; font-size:12px; font-weight:600; text-transform:uppercase; }
        .stats-panel { display:grid; grid-template-columns:repeat(2,minmax(0,1fr)); gap:13px; padding:22px; align-content:start; }
        .stat-card { position:relative; min-height:112px; padding:18px 18px 15px 22px; overflow:hidden; border:1px solid #e0e9f1; border-radius:13px; background:linear-gradient(145deg,#fff,#f8fbfd); cursor:pointer; transition:.2s ease; }
        .stat-card:hover, .stat-card.is-active { transform:translateY(-3px); border-color:var(--accent); box-shadow:0 9px 20px rgba(31,55,78,.13); }
        .stat-card.is-active { background:var(--accent); color:#fff; box-shadow:0 12px 24px rgba(31,55,78,.22); }
        .stat-card.is-active .stat-label, .stat-card.is-active .stat-value { color:#fff; }
        .stat-card.is-active .stat-percent { background:rgba(255,255,255,.2); color:#fff; }
        .stat-card:before { content:""; position:absolute; top:0; bottom:0; left:0; width:5px; background:var(--accent); }
        .stat-card.total { grid-column:1/-1; min-height:96px; --accent:#244b74; background:linear-gradient(120deg,#173f63,#286b9d); color:#fff; }
        .stat-label { display:block; color:#718096; font-size:12px; font-weight:700; letter-spacing:.45px; text-transform:uppercase; }
        .total .stat-label { color:#cfe4f5; }
        .stat-value { display:block; margin-top:8px; color:#223f59; font-size:30px; font-weight:700; line-height:1; }
        .total .stat-value { color:#fff; font-size:34px; }
        .stat-percent { position:absolute; right:15px; bottom:14px; padding:4px 8px; border-radius:13px; background:#eef5fa; color:var(--accent); font-size:11px; font-weight:700; }
        .details-card { margin-top:20px; overflow:hidden; }
        .details-header { display:flex; align-items:center; justify-content:space-between; gap:15px; padding:18px 20px; border-bottom:1px solid #e4ebf2; }
        .details-header h3 { margin:0; color:#294c68; font-size:17px; font-weight:700; }
        .status-badge { display:inline-block; padding:5px 10px; border-radius:14px; color:#fff; font-size:11px; font-weight:700; text-transform:uppercase; }
        .dashboard-table-wrap { width:100%; overflow-x:auto; }
        .dashboard-table { width:100%; min-width:800px; border-collapse:collapse; }
        .dashboard-table th { padding:12px 14px; border:0; background:#eef5fa; color:#456078; font-size:11px; text-align:left; text-transform:uppercase; }
        .dashboard-table td { padding:12px 14px; border-top:1px solid #edf1f5; color:#40566b; }
        .dashboard-table tbody tr:hover { background:#f7fbfe; }
        .empty-row { padding:34px !important; color:#8795a3 !important; text-align:center; }
        .dashboard-pagination { display:flex; align-items:center; justify-content:space-between; gap:12px; padding:14px 20px; border-top:1px solid #e4ebf2; color:#718096; font-size:12px; }
        .dashboard-pagination-control { display:flex; align-items:center; gap:7px; }
        .dashboard-pagination select, .dashboard-pagination button { min-height:32px; padding:5px 10px; border:1px solid #d5e1eb; border-radius:7px; background:#fff; color:#36536c; }
        .dashboard-pagination button { cursor:pointer; font-weight:700; }
        .dashboard-pagination button:hover:not(:disabled), .dashboard-pagination button.is-current { border-color:#337ab7; background:#337ab7; color:#fff; }
        .dashboard-pagination button:disabled { cursor:not-allowed; opacity:.45; }
        #dashboardPageNumbers { display:flex; gap:4px; }
        .dashboard-error { padding:30px; border:1px solid #f4cccc; border-radius:12px; background:#fff5f5; color:#b33; text-align:center; }
        @media (max-width:991px) { .dashboard-grid { grid-template-columns:1fr; } }
        @media (max-width:600px) { .dashboard-heading { align-items:flex-start; flex-direction:column; } .stats-panel { grid-template-columns:1fr; } .stat-card.total { grid-column:auto; } .chart-card,.stats-panel { padding:16px; } }
    </style>

    <section class="flight-dashboard" aria-labelledby="dashboardTitle">
        <div class="dashboard-heading">
            <div><h2 id="dashboardTitle">Flight Operations Dashboard</h2><p>Tổng quan trạng thái khai thác chuyến bay</p></div>
            <div id="dashboardDate" class="dashboard-date"></div>
        </div>

        <div id="dashboardContent">
            <div class="dashboard-grid">
                <article class="dashboard-card chart-card">
                    <h3 class="card-title">Tỷ lệ trạng thái chuyến bay</h3>
                    <p class="card-subtitle">Nhấn vào một phần biểu đồ để xem dữ liệu chi tiết</p>
                    <div id="donutWrap" class="donut-wrap">
                        <svg id="flightDonut" viewBox="0 0 240 240" role="img" aria-label="Biểu đồ trạng thái chuyến bay">
                            <circle class="donut-track" cx="120" cy="120" r="82"></circle>
                            <g id="donutSegments"></g>
                        </svg>
                        <div class="donut-center"><strong id="donutTotal">0</strong><span>Tổng chuyến bay</span></div>
                    </div>
                </article>
                <aside id="statsPanel" class="dashboard-card stats-panel"></aside>
            </div>

            <article class="dashboard-card details-card">
                <div class="details-header"><h3 id="detailTitle">Danh sách tất cả chuyến bay</h3><span id="detailBadge" class="status-badge" style="background:#244b74">Tất cả</span></div>
                <div class="dashboard-table-wrap">
                    <table class="dashboard-table">
                        <thead><tr><th>STT</th><th>Chuyến bay</th><th>Đăng ký</th><th>Hành trình</th><th>Ngày bay</th><th>Giờ dự kiến</th><th>Trạng thái</th></tr></thead>
                        <tbody id="flightTableBody"></tbody>
                    </table>
                </div>
                <div class="dashboard-pagination">
                    <div class="dashboard-pagination-control"><label for="dashboardRowsPerPage">Số hàng/trang</label><select id="dashboardRowsPerPage" onchange="window.dashboardSetPageSize(this.value)"><option value="5">5</option><option value="10" selected>10</option><option value="20">20</option><option value="50">50</option></select></div>
                    <span id="dashboardPageSummary"></span>
                    <div id="dashboardPageNumbers" class="dashboard-pagination-control"></div>
                </div>
            </article>
        </div>
    </section>

    <script>
        (function () {
            var dataUrl = '<%= ResolveUrl("~/Data/dashboard-flights.json") %>';
            var flights = [];
            var selectedStatus = 'all', currentPage = 1, pageSize = 10;
            var statuses = [
                { key:'finished', label:'Finished', color:'#20b486' },
                { key:'cancel', label:'Cancel', color:'#ef5b5b' },
                { key:'delay', label:'Delay', color:'#f5a623' },
                { key:'wait', label:'Wait', color:'#4d8df7' }
            ];

            function escapeHtml(value) { var node=document.createElement('div'); node.appendChild(document.createTextNode(value == null ? '' : String(value))); return node.innerHTML; }
            function getStatus(key) { for(var i=0;i<statuses.length;i++) if(statuses[i].key===key) return statuses[i]; return {key:key,label:key,color:'#718096'}; }
            function countStatus(key) { var count=0; for(var i=0;i<flights.length;i++) if(flights[i].status===key) count++; return count; }

            function renderDonut() {
                var total=flights.length, radius=82, circumference=2*Math.PI*radius, offset=0, html='';
                for(var i=0;i<statuses.length;i++) {
                    var item=statuses[i], count=countStatus(item.key), length=total ? count/total*circumference : 0;
                    html += '<circle class="donut-segment" data-status="'+item.key+'" cx="120" cy="120" r="'+radius+'" stroke="'+item.color+'" stroke-dasharray="'+length+' '+(circumference-length)+'" stroke-dashoffset="'+(-offset)+'"><title>'+item.label+': '+count+' ('+(total?Math.round(count*100/total):0)+'%)</title></circle>';
                    offset += length;
                }
                document.getElementById('donutSegments').innerHTML=html;
                document.getElementById('donutTotal').innerHTML=total;
                var segments=document.querySelectorAll('.donut-segment');
                for(var j=0;j<segments.length;j++) segments[j].onclick=function(){ var status=this.getAttribute('data-status'); selectStatus(selectedStatus===status?'all':status); };
            }

            function renderStats() {
                var total=flights.length;
                var html='<div class="stat-card total" data-status="all"><span class="stat-label">Tổng số chuyến bay</span><strong class="stat-value">'+total+'</strong></div>';
                for(var i=0;i<statuses.length;i++) {
                    var item=statuses[i], count=countStatus(item.key), percent=total?Math.round(count*100/total):0;
                    html += '<div class="stat-card" data-status="'+item.key+'" style="--accent:'+item.color+'"><span class="stat-label">Tổng số '+item.label+'</span><strong class="stat-value">'+count+'</strong><span class="stat-percent">'+percent+'%</span></div>';
                }
                document.getElementById('statsPanel').innerHTML=html;
                var cards=document.querySelectorAll('.stat-card');
                for(var j=0;j<cards.length;j++) cards[j].onclick=function(){ selectStatus(this.getAttribute('data-status')); };
            }

            function renderTable() {
                var filtered=selectedStatus==='all'?flights:flights.filter(function(f){return f.status===selectedStatus;});
                var totalPages=Math.max(1,Math.ceil(filtered.length/pageSize)); currentPage=Math.min(currentPage,totalPages);
                var start=(currentPage-1)*pageSize, pageRows=filtered.slice(start,start+pageSize), rows='';
                for(var i=0;i<pageRows.length;i++) { var f=pageRows[i], s=getStatus(f.status); rows+='<tr><td>'+(start+i+1)+'</td><td><strong>'+escapeHtml(f.flightNo)+'</strong></td><td>'+escapeHtml(f.registration)+'</td><td>'+escapeHtml(f.from)+' → '+escapeHtml(f.to)+'</td><td>'+escapeHtml(f.flightDate)+'</td><td>'+escapeHtml(f.scheduledTime)+'</td><td><span class="status-badge" style="background:'+s.color+'">'+s.label+'</span></td></tr>'; }
                document.getElementById('flightTableBody').innerHTML=rows||'<tr><td colspan="7" class="empty-row">Không có dữ liệu</td></tr>';
                document.getElementById('dashboardPageSummary').innerHTML=filtered.length?'Hiển thị '+(start+1)+'–'+Math.min(start+pageSize,filtered.length)+' / '+filtered.length:'Không có dữ liệu';
                var pages='', from=Math.max(1,currentPage-2), to=Math.min(totalPages,from+4); from=Math.max(1,to-4);
                pages+='<button type="button" onclick="window.dashboardSetPage('+(currentPage-1)+' )" '+(currentPage===1?'disabled':'')+'>‹</button>';
                for(var p=from;p<=to;p++) pages+='<button type="button" class="'+(p===currentPage?'is-current':'')+'" onclick="window.dashboardSetPage('+p+')">'+p+'</button>';
                pages+='<button type="button" onclick="window.dashboardSetPage('+(currentPage+1)+' )" '+(currentPage===totalPages?'disabled':'')+'>›</button>';
                document.getElementById('dashboardPageNumbers').innerHTML=pages;
            }

            function selectStatus(status) {
                selectedStatus=status; currentPage=1;
                var filtered=status==='all'?flights:flights.filter(function(f){return f.status===status;});
                var meta=status==='all'?{label:'Tất cả',color:'#244b74'}:getStatus(status);
                document.getElementById('detailTitle').innerHTML='Danh sách '+(status==='all'?'tất cả chuyến bay':meta.label)+' ('+filtered.length+')';
                var badge=document.getElementById('detailBadge'); badge.innerHTML=meta.label; badge.style.background=meta.color;
                renderTable();
                var segments=document.querySelectorAll('.donut-segment'), cards=document.querySelectorAll('.stat-card');
                document.getElementById('donutWrap').classList.toggle('has-active',status!=='all');
                for(var j=0;j<segments.length;j++) segments[j].classList.toggle('is-active',segments[j].getAttribute('data-status')===status);
                for(var k=0;k<cards.length;k++) cards[k].classList.toggle('is-active',cards[k].getAttribute('data-status')===status);
            }

            window.dashboardSetPage=function(page){ currentPage=Math.max(1,parseInt(page,10)||1); renderTable(); };
            window.dashboardSetPageSize=function(size){ pageSize=Math.max(1,parseInt(size,10)||10); currentPage=1; renderTable(); };

            function start() {
                document.getElementById('dashboardDate').innerHTML=new Date().toLocaleDateString('vi-VN',{weekday:'long',day:'2-digit',month:'2-digit',year:'numeric'});
                fetch(dataUrl,{cache:'no-store'}).then(function(response){if(!response.ok) throw new Error('HTTP '+response.status); return response.json();}).then(function(data){flights=data.flights||[];renderDonut();renderStats();selectStatus('all');}).catch(function(error){document.getElementById('dashboardContent').innerHTML='<div class="dashboard-error">Không thể tải dữ liệu dashboard: '+escapeHtml(error.message)+'</div>';});
            }
            if(document.readyState==='loading') document.addEventListener('DOMContentLoaded',start); else start();
        })();
    </script>
</asp:Content>
