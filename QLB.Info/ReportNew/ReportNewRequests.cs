using System.Collections.Generic;

namespace QLB.Info
{
    // Request cho nhom API goi cac store package tao ngay 11-09-2026
    // (FLIGHT_STATUS_PKG, DELAY_ALERT_PKG, DAYPLAN_COMPARE_PKG,
    //  TRACKING_MAP_PKG, SLOT_COMPARE_PKG).

    public class FlightStatusRequest
    {
        public string FromDate { get; set; }   // yyyy-MM-dd hoac dd-MM-yyyy
        public string ToDate { get; set; }     // bao gom ca ngay ket thuc
        public string Oper { get; set; }       // NULL/ALL = khong loc
        public string Airport { get; set; }    // NULL/ALL = khong loc
        public int CurrentDay { get; set; }    // 1 = T_DAY_FLIGHTS_GOINGON, 0 = T_FINISHED_FLIGHTS
    }

    public class DateRangeRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class DayPlanCompareRequest
    {
        public string FlightDate { get; set; } // yyyy-MM-dd
        public string Airport { get; set; }    // NULL/ALL = khong loc
        public string Oper { get; set; }       // NULL/ALL = khong loc
    }

    public class TrackingMapRequest
    {
        public string FlightDate { get; set; } // yyyy-MM-dd
        public string Callsigns { get; set; }  // danh sach callsign phan tach dau phay; NULL = tat ca
    }

    public class SlotCompareRequest
    {
        public string CompareDate { get; set; } // yyyy-MM-dd
        public string Oper { get; set; }        // NULL/ALL = khong loc
        public string ResultType { get; set; }  // KQ1..KQ4
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
