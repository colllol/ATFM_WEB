const fs = require('fs'), vm = require('vm'), assert = require('assert');
const source = fs.readFileSync('prjApplication/ReportNew/FlightOperationOverview.js', 'utf8');
function section(start, end) { return source.slice(source.indexOf(start), source.indexOf(end, source.indexOf(start))); }
let checks = 0;
function ok(value, message) { assert.ok(value, message); checks++; }
const ctx = { Date, Math, Number, String, JSON, Object, Array, Error };
vm.createContext(ctx);
vm.runInContext(section('    function airportSeries(', '    function esc('), ctx);
const flights = [
 {flightDate:'2026-06-01',fromAirp:'VVNB',toAirp:'VVTS',status:'FINISHED'},
 {flightDate:'2026-06-01',fromAirp:'VVTS',toAirp:'VVNB',status:'DELAY_16_30'},
 {flightDate:'2026-06-03',fromAirp:'VVNB',toAirp:'VVDN',status:'DELAY_60_PLUS'},
 {flightDate:'2026-07-01',fromAirp:'VVNB',toAirp:'VVTS',status:'FINISHED'},
 {flightDate:'2026-06-01',fromAirp:'VVTS',toAirp:'VVDN',status:'FINISHED'}
];
let result=ctx.airportSeries(flights,'VVNB','2026-06-01','2026-06-03');
ok(result.daily,'same month groups by day');
ok(result.buckets.length===3,'zero days retained');
ok(result.buckets[0].departures===1 && result.buckets[0].arrivals===1,'both movements counted');
ok(result.buckets[1].departures===0,'zero day count');
ok(result.buckets[2].departures===1,'delay flight retained');
result=ctx.airportSeries(flights,'VVNB','2026-06-01','2026-07-02');
ok(!result.daily && result.buckets.length===2,'cross month groups monthly');
ok(result.buckets[0].departures===2 && result.buckets[1].departures===1,'monthly totals');
result=ctx.airportSeries([], 'VVNB','2025-12-30','2026-01-02');
ok(!result.daily && result.buckets[1].key==='2026-01','year crossing');
result=ctx.airportSeries([], 'VVNB','2024-02-28','2024-02-29');
ok(result.daily && result.buckets.length===2,'leap day');
result=ctx.airportSeries([], 'VVNB','2026-06-01','2026-06-01');
ok(result.buckets.length===1,'one day');
const host={};
ctx.app={querySelector:()=>host}; ctx.selectedBar={code:'VVNB',movement:'departure'};
ctx.data={flights};ctx.appliedFrom='2026-06-01';ctx.appliedTo='2026-06-03';
ctx.esc=x=>String(x);ctx.airportName=x=>x;ctx.number=x=>String(x);
vm.runInContext(section('        function renderAirportTrend()', '        function renderPage()'),ctx);
ctx.renderAirportTrend();
ok(host.hidden===false && host.innerHTML.includes('<svg'),'click renders a chart');
ok((host.innerHTML.match(/<rect /g)||[]).length===6,'two bars for each day');
ok(!/NaN|Infinity/.test(host.innerHTML),'valid chart coordinates');
ctx.selectedBar=null;ctx.renderAirportTrend();ok(host.hidden && host.innerHTML==='','deselect clears chart');
ctx.selectedBar={code:'VVNB',movement:'departure'};
vm.runInContext(section('        function detailFlights()', '        function renderKpis()'),ctx);
ok(ctx.detailFlights().some(x=>x.status==='DELAY_60_PLUS'),'detail preserves delay');
ctx.selectedBar=null;ok(ctx.detailFlights().length===5,'all detail rows retained');
let request;
ctx.window={location:{pathname:'/atfm_web/ReportNew/FlightOperationOverview.aspx'}};
ctx.camelize=x=>x;
ctx.fetch=(url,opts)=>{request={url,opts};return Promise.resolve({ok:true,text:()=>Promise.resolve(JSON.stringify({d:{total:5}}))});};
vm.runInContext(section('    function post(', '    function airportSeries('),ctx);
(async()=>{
 const response=await ctx.post('GetData',{fromDate:'2026-06-01',toDate:'2026-06-03',airport:'ALL'});
 ok(response.total===5,'WebMethod response unwrapped');
 ok(request.url.endsWith('.aspx/GetData'),'same-origin WebMethod');
 ctx.fetch=()=>Promise.resolve({ok:false,text:()=>Promise.resolve('<html>login</html>')});
 await assert.rejects(()=>ctx.post('GetData',{}),/JSON/);checks++;
 ok(!source.includes('templateKpis.remove()'),'KPI cards retained');
 ok(!source.includes('renderDonut('),'no missing donut DOM references');
 ok(!/\\\\u[0-9a-f]{4}/i.test(source),'no literal unicode escapes in labels');
 const cs=fs.readFileSync('prjApplication/ReportNew/FlightOperationOverview.aspx.cs','utf8');
 ok(cs.includes('GetData(string fromDate, string toDate, string airport)'),'dashboard compatible signature');
 ok(cs.includes('today.AddDays(-1)') && cs.includes('statusData.Merge(live)'),'history and live split without date overlap');
 ok(cs.includes('flightDate = item.FlightDate'),'dates available for grouping');
 console.log('PASS '+checks+' overview regression checks');
})().catch(e=>{console.error(e);process.exitCode=1;});

const template = fs.readFileSync('prjApplication/ReportNew/ReportNew.js','utf8');
const overview = template.slice(template.indexOf("else if(view==='overview'){"),template.indexOf("else if(view==='military'){"));
assert.ok(!/kpis\(|donut\(|bars\(|table\(/.test(overview),'Overview must not render fixture charts before fetching');
assert.ok(overview.includes('aria-live="polite"'),'loading status is accessible');
assert.ok(source.includes("loadState.textContent = 'Không tải được dữ liệu: '"),'load error remains visible');
console.log('PASS 3 Overview initial-loading checks');

const zeroHost = {};
const zeroCtx = { data: {}, app: { querySelector: () => zeroHost }, number: x => String(Number(x || 0)) };
vm.createContext(zeroCtx);
vm.runInContext(section('        function renderKpis()', '        function renderBars()'), zeroCtx);
zeroCtx.renderKpis();
assert.strictEqual((zeroHost.innerHTML.match(/<strong>0<\/strong>/g) || []).length, 4, 'all empty KPIs show zero');
zeroCtx.data = { total: 5, finished: 3, delay: 2, airportCount: 4 };
zeroCtx.renderKpis();
for (const value of [5, 3, 2, 4]) assert.ok(zeroHost.innerHTML.includes('<strong>' + value + '</strong>'), 'KPI uses actual response');
Object.assign(zeroCtx, { data: { airports: [] }, airport: { value: 'ALL' }, airportNames: { VVNB: 'Noi Bai', VVTS: 'Tan Son Nhat' }, selectedBar: null, esc: String, airportName: String });
zeroCtx.app.querySelectorAll = () => [];
vm.runInContext(section('        function renderBars()', '        function renderAirportTrend()'), zeroCtx);
zeroCtx.renderBars();
assert.strictEqual((zeroHost.innerHTML.match(/height:0%/g) || []).length, 4, 'empty chart retains zero bars for both airports');
assert.ok(!/NaN|Infinity/.test(zeroHost.innerHTML), 'zero bars have valid coordinates');
zeroCtx.airport.value = 'VVNB';
zeroCtx.renderBars();
assert.strictEqual((zeroHost.innerHTML.match(/height:0%/g) || []).length, 2, 'empty chart respects selected airport');
zeroCtx.data.airports = [{ code: 'VVNB', departures: 3, arrivals: 2 }];
zeroCtx.renderBars();
assert.ok(zeroHost.innerHTML.includes('<span>3</span>') && zeroHost.innerHTML.includes('<span>2</span>'), 'chart displays actual counts');
assert.ok(!fs.readFileSync('prjApplication/ReportNew/FlightOperationOverview.aspx', 'utf8').includes('.rn-kpis{display:none}'), 'page does not hide KPIs');
console.log('PASS 10 Overview zero-data checks');
