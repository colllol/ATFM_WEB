const fs = require('fs');

const assert = require('assert');

const js = fs.readFileSync('prjApplication/ReportNew/FlightTrendAnalysis.js', 'utf8');

const template = fs.readFileSync('prjApplication/ReportNew/ReportNew.js', 'utf8');

const page = fs.readFileSync('prjApplication/ReportNew/FlightTrendAnalysis.aspx', 'utf8');

const css = fs.readFileSync('prjApplication/ReportNew/ReportInteractive.css', 'utf8');

const controls = fs.readFileSync('prjApplication/ReportNew/ReportControls.js', 'utf8');

assert.ok(js.includes('function comparisonPeriods(data)'), 'trend normalizes comparison periods');

assert.ok(js.includes('slice(0, 10)'), 'trend caps comparison years at ten');

assert.ok(js.includes("yearsBack: parseInt(yearsBack.value, 10) || 1"), 'selected years are sent to API');

assert.ok(js.includes('comparisonPeriods(data).map'), 'legend uses all comparison periods');

assert.ok(js.includes('window.ReportControls.enhance(airport)'), 'airport dropdown keeps searchable enhancement');

assert.ok(js.includes('window.ReportControls.enhance(oper)'), 'operator dropdown keeps searchable enhancement');

assert.ok(!js.includes('enhanceAll(filters)'), 'period/year selects keep native dropdown behavior');

assert.ok(template.includes('id="rnPeriod" class="rn-native-select"'), 'period selector is marked native');

assert.ok(template.includes('id="rnYearsBack" class="rn-native-select"'), 'year selector is marked native');

assert.ok(controls.includes('select:not(.rn-native-select)'), 'global searchable enhancer skips native selects');

assert.ok(!js.match(/\\\\u[0-9a-fA-F]{4}/), 'Vietnamese labels do not contain double-escaped unicode');

assert.ok(js.includes('var previous = comparisons.map'), 'chart builds one series per comparison year');

assert.ok(js.includes('periods.map(function (item, index)'), 'KPI row is rendered for every comparison year');

assert.ok(js.includes('Ch\\u00eanh l\\u1ec7ch v\\u1edbi'), 'each KPI row compares current data with its selected year');

assert.ok(js.includes('seriesColor(seriesIndex + 1)'), 'each previous-year line uses its own color');

for (const year of ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10']) assert.ok(template.includes('<option value="' + year + '"'), 'years option ' + year + ' exists');

assert.ok(css.includes('.rn-app[data-report-view="trend"] .rn-trend-year-row'), 'comparison rows occupy full width');

assert.ok(css.includes('grid-template-columns: repeat(4, minmax(0, 1fr))'), 'comparison row has four horizontal KPI cards');

assert.ok(css.includes('grid-template-columns: minmax(150px,1.15fr) minmax(150px,1.15fr) minmax(135px,1fr) minmax(135px,1fr) minmax(115px,.85fr) minmax(115px,.85fr) 112px !important'), 'seven trend controls fit one grid row');

assert.ok(css.includes('> :nth-child(7) { grid-column: 7; grid-row: 1; }'), 'apply button stays on the same row');

assert.ok(page.includes('FlightTrendAnalysis.js?v=20261018-5'), 'trend JS cache-buster updated');

assert.ok(page.includes('ReportInteractive.css?v=20261018-5'), 'trend CSS cache-buster updated');

console.log('PASS trend filter and multi-year regression checks');
