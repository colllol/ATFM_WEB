const fs=require('fs'),vm=require('vm'),assert=require('assert/strict');
const source=fs.readFileSync('prjApplication/SLOTS/EmailReports.js','utf8');let count=0;
function eq(a,b,m){assert.equal(a,b,m);count++;} function ok(v,m){assert.ok(v,m);count++;}
function response(data,success=true,status=200){return{ok:success,status,json:async()=>({d:typeof data==='string'?data:JSON.stringify(data)})};}
function harness(){
 const els={},requests=[],buttons=['email','incoming'].map(x=>({getAttribute:()=>x,setAttribute(){},classList:{toggle(){}}}));
 function el(id){return els[id]||(els[id]={value:'',textContent:'',innerHTML:'',disabled:false,hidden:true,className:'',getAttribute(name){return {'data-email-endpoint':'email','data-incoming-endpoint':'incoming','data-email-summary-endpoint':'emailSummary','data-incoming-summary-endpoint':'incomingSummary','data-email-detail-endpoint':'emailDetail','data-incoming-detail-endpoint':'incomingDetail','data-job-endpoint':'job','data-send-report-endpoint':'send'}[name]},classList:{add(){},remove(){},toggle(){}},setAttribute(){},removeAttribute(){},querySelector(){return el('live')},checkValidity(){return true}})}
 el('emailFileType').value='word';el('emailPageSize').value='50';
 const h={el,requests,buttons,list:[],summary:{totalFiles:0,successFiles:0,errorFiles:0,pendingFiles:0,unknownFiles:0,snapshotId:'snap'},details:{},summaryFails:false};
 const ctx={setTimeout,clearTimeout,Map,console,Promise,Date,JSON,Number,String,Array,Math,Object,Blob,isFinite,parseInt,encodeURIComponent,URL:{createObjectURL:()=>'',revokeObjectURL(){}},window:{setTimeout,AbortController},document:{getElementById:el,querySelectorAll:q=>q==='[data-report-source]'?buttons:[],createElement(tag){if(tag==='a')return{click(){},style:{}};let t='';return{set textContent(v){t=String(v)},get innerHTML(){return t.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;')}}},body:{appendChild(){},removeChild(){}}},fetch:async(url,opt)=>{let body=JSON.parse(opt.body);requests.push({url,body});if(h.respond){const r=await h.respond(url,body,opt);if(r)return r;}if(/Summary/.test(url)){if(h.summaryFails)return response({Message:'summary failed'},false,500);return response(h.summary)}if(/Detail/.test(url))return response(h.details[body.id]||h.list.find(x=>x.id===body.id));if(url==='job')return response({state:'found',targetPermId:9,targetTable:'SC'});if(url==='send')return response({message:'sent'});let start=body.page*body.size;return response({content:h.list.slice(start,start+body.size),page:body.page,size:body.size,totalElements:h.list.length,totalPages:Math.ceil(h.list.length/body.size),hasNext:start+body.size<h.list.length});}};
 vm.createContext(ctx);let modified=source.replace(/    loadPage\(\);\s*\}\(\)\);\s*$/,'    globalThis.api={loadPage,showDetail,selectSource,cacheGet,cachePut,getCacheSize:()=>viewCacheOrder.length,getState:function(){return{rows,totalItems,totalPages,currentPage,summary,snapshotId,sourceMode}}};\n}());');assert.notEqual(modified,source);vm.runInContext(modified,ctx);h.api=ctx.api;h.rowCount=()=>((el('emailRows').innerHTML.match(/data-index=/g))||[]).length;return h;
}
function row(i,state='found'){return{id:i,syncJobId:'j'+i,storedFileName:'f'+i+'.docx',subject:'s'+i,sender:'a@b.c',receivedAt:'2026-09-27T10:00:00',processingStatus:'SAVED',databaseState:state,hasDatabaseData:state==='found'};}
(async()=>{
 let h=harness();h.list=Array.from({length:121},(_,i)=>row(i,i%2?'found':'missing'));h.summary={totalFiles:121,successFiles:60,errorFiles:61,pendingFiles:0,unknownFiles:0,snapshotId:'S1'};await h.api.loadPage();
 eq(h.requests.length,2,'one summary plus one list only');eq(h.requests[0].url,'emailSummary');eq(h.requests[1].url,'email');eq(h.requests[1].body.fileType,'word');eq(h.requests[1].body.size,50);eq(h.requests[1].body.snapshotId,'S1');eq(h.rowCount(),50);eq(h.el('emailTotal').textContent,'121');eq(h.el('emailSent').textContent,'60');eq(h.el('emailFailed').textContent,'61');
 h.el('emailNext').onclick();await new Promise(r=>setImmediate(r));eq(h.requests.length,3,'next calls one list only');eq(h.requests[2].body.page,1);eq(h.requests[2].body.snapshotId,'S1');eq(h.rowCount(),50);ok(h.el('emailRows').innerHTML.includes('<td>51</td>'));
 h.el('emailNext').onclick();await new Promise(r=>setImmediate(r));eq(h.rowCount(),21);eq(h.el('emailNext').disabled,true);
 h.el('emailPageSize').value='100';h.el('emailPageSize').onchange();await new Promise(r=>setImmediate(r));eq(h.requests.at(-1).body.size,100);eq(h.requests.at(-1).body.page,0);eq(h.rowCount(),100);
 let f=harness();f.list=Array.from({length:70},(_,i)=>row(i,'unknown'));f.summaryFails=true;await f.api.loadPage();eq(f.requests.length,2,'failed summary never triggers full scan');eq(f.rowCount(),50);eq(f.el('emailTotal').textContent,'70');eq(f.el('emailSent').textContent,'--');eq(f.el('emailFailed').textContent,'--');eq(f.el('emailError').hidden,true,'summary failure does not break list');
 let d=harness();d.list=[row(7)];d.summary={totalFiles:1,successFiles:1,errorFiles:0,pendingFiles:0,unknownFiles:0,snapshotId:'S'};d.details[7]={...row(7),body:'full body'};await d.api.loadPage();let before=d.requests.length;d.api.showDetail(d.list[0]);await new Promise(r=>setImmediate(r));await new Promise(r=>setImmediate(r));eq(d.requests[before].url,'emailDetail');eq(d.requests[before].body.id,7);eq(d.requests[before+1].url,'job');eq(d.el('emailDetailBody').textContent,'full body');
 let race=harness(),release;race.summaryFails=true;race.list=[row(1)];race.respond=null;let originalFetch=undefined;
 // Contract-level source assertions prevent regression to all-page scanning/per-row inspection.
 ok(!source.includes('function loadAllItems'),'no all-page loader');ok(!source.includes('inspectItems('),'no per-row DB scan');ok(!source.includes('size: 100 };'),'no forced page size 100');ok(source.includes('snapshotId: snapshot ||'),'list carries snapshot');

 const tick=()=>new Promise(r=>setImmediate(r));
 const c=harness();c.list=Array.from({length:121},(_,i)=>row(i));c.summary={totalFiles:121,successFiles:121,errorFiles:0,pendingFiles:0,unknownFiles:0,snapshotId:'C'};
 await c.api.loadPage(); c.el('emailNext').onclick(); await tick();
 c.api.selectSource('incoming');await tick();const n=c.requests.length;
 c.api.selectSource('email');await tick();eq(c.requests.length,n,'tab return is zero requests');eq(c.api.getState().currentPage,2,'tab restores page');eq(c.el('emailPageSize').value,'50');
 let fail=false;c.respond=(url)=>fail&&url==='email'?response({Message:'offline'},false,500):null;
 fail=true;c.el('emailRefresh').onclick();await tick();eq(c.rowCount(),50,'failed refresh retains rows');eq(c.api.getState().currentPage,2,'failed refresh retains page');eq(c.el('emailError').hidden,false);
 fail=false;c.summary.snapshotId='NEW';c.el('emailRefresh').onclick();await tick();const rn=c.requests.length;c.el('emailNext').onclick();await tick();eq(c.requests.length,rn+1,'old snapshot page cannot hit cache');eq(c.requests.at(-1).body.snapshotId,'NEW');
 for(let i=0;i<25;i++) c.api.cachePut('k'+i,{createdAt:Date.now()});eq(c.api.getCacheSize(),20,'LRU capped');eq(c.api.cacheGet('k0'),null,'evicted oldest');
 c.api.cachePut('expired',{createdAt:Date.now()-400000});eq(c.api.cacheGet('expired'),null,'TTL expired');
 const dn=d.requests.length;d.api.showDetail(d.list[0]);await tick();eq(d.requests.length,dn,'detail and job cache hit');
 const q=harness();q.list=[row(1)];q.summary={totalFiles:1,successFiles:1,errorFiles:0,pendingFiles:0,unknownFiles:0,snapshotId:'Q'};
 let releaseQ;q.respond=(url)=>url==='emailSummary'?new Promise(r=>releaseQ=()=>r(response(q.summary))):null;
 const qa=q.api.loadPage();const qb=q.api.loadPage();eq(q.requests.length,1,'same in-flight search deduped');releaseQ();await Promise.all([qa,qb]);eq(q.requests.length,2);
 const r=harness();r.list=[row(8)];r.summary={totalFiles:1,successFiles:1,errorFiles:0,pendingFiles:0,unknownFiles:0,snapshotId:'R'};
 let releaseOld,oldSignal;r.respond=(url,body,opt)=>body.query==='old'?new Promise(resolve=>{oldSignal=opt.signal;releaseOld=()=>resolve(response(r.summary));}):null;
 r.el('emailSearch').value='old';const old=r.api.loadPage();r.el('emailSearch').value='new';await r.api.loadPage();eq(oldSignal.aborted,true,'superseded fetch aborted');releaseOld();await old;eq(r.api.getState().rows[0].id,8);eq(r.requests.filter(x=>x.url==='email').length,1,'old summary cannot launch list');
 ok(!/Hi\?n|KH\?NG|Kh\?ng/.test(source),'Vietnamese text not corrupted');
 console.log('PASS: '+count+' assertions for one-page loading, summary fallback, snapshot paging and on-demand detail.');
})().catch(e=>{console.error(e);process.exitCode=1});
