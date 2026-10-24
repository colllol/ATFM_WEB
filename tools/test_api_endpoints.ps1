# Goi thu cac package qua API remote y het cach cac trang web se goi.
$base = "http://172.29.79.49:5176/api/ApiExtension/ExcuteTable"

function Call-Pkg($pkg, $store, $body, $timeoutSec = 120) {
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    try {
        $r = Invoke-WebRequest -Uri "$base`?packageName=$pkg&storeName=$store" -Method Put -Body $body -ContentType 'application/json' -TimeoutSec $timeoutSec -UseBasicParsing
        $j = $r.Content | ConvertFrom-Json
        $n = if ($j.ListValue) { $j.ListValue.Count } else { 0 }
        "{0,-45} Code={1} rows={2,-6} {3}ms" -f "$pkg.$store", $j.Code, $n, $sw.ElapsedMilliseconds
    } catch {
        "{0,-45} LOI: {1}" -f "$pkg.$store", $_.Exception.Message
    }
}

# FLIGHT_STATUS_PKG - kich ban FlightStatusRate/Overview/TakeoffLanding/Trend
Call-Pkg "FLIGHT_STATUS_PKG" "GET_STATUS"    '{"P_FROM_DATE":"2026-07-10","P_TO_DATE":"2026-07-10","P_OPER":null,"P_AIRPORT":null,"P_CURRENT_DAY":0}' 300
Call-Pkg "FLIGHT_STATUS_PKG" "GET_STATUS"    ('{"P_FROM_DATE":"' + (Get-Date -Format yyyy-MM-dd) + '","P_TO_DATE":"' + (Get-Date -Format yyyy-MM-dd) + '","P_OPER":null,"P_AIRPORT":null,"P_CURRENT_DAY":1}')
Call-Pkg "FLIGHT_STATUS_PKG" "GET_CANCELLED" '{"P_FROM_DATE":"2026-07-10","P_TO_DATE":"2026-07-10"}'
# DAYPLAN_COMPARE_PKG - kich ban FlightPlanDailyComparison
Call-Pkg "DAYPLAN_COMPARE_PKG" "GET_DAY_FLIGHTS" '{"P_FLIGHT_DATE":"2026-07-10","P_AIRPORT":null,"P_OPER":null}'
# TRACKING_MAP_PKG - kich ban FlightTrackingMap
Call-Pkg "TRACKING_MAP_PKG" "GET_FLIGHT_META" ('{"P_FLIGHT_DATE":"' + (Get-Date -Format yyyy-MM-dd) + '","P_CALLSIGNS":null}')
# SLOT_COMPARE_PKG - kich ban SlotComparison
Call-Pkg "SLOT_COMPARE_PKG" "GET_DEFAULT_DATE" '{}'
Call-Pkg "SLOT_COMPARE_PKG" "GET_OPERATORS"    '{}'
Call-Pkg "SLOT_COMPARE_PKG" "GET_OPERATOR_MAP" '{}'
Call-Pkg "SLOT_COMPARE_PKG" "GET_AIRPORT_MAP"  '{}'
