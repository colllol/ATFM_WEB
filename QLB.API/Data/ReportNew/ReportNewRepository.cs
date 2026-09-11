using System;
using System.Collections.Generic;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Data
{
    // Repository cho nhom API ReportNew/SLOTS goi truc tiep cac store package
    // qua duong ApiHelperRepository (tu dong bind tham so tu USER_ARGUMENTS,
    // tra du lieu tu OUT ref cursor).
    public class ReportNewRepository
    {
        // ===== FLIGHT_STATUS_PKG =====
        public ReponseReportEntity GetStatus(FlightStatusRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("FLIGHT_STATUS_PKG", "GET_STATUS", new Dictionary<string, object>
            {
                ["P_FROM_DATE"] = request.FromDate,
                ["P_TO_DATE"] = request.ToDate,
                ["P_OPER"] = request.Oper,
                ["P_AIRPORT"] = request.Airport,
                ["P_CURRENT_DAY"] = request.CurrentDay
            });
        }

        public ReponseReportEntity GetCancelled(DateRangeRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("FLIGHT_STATUS_PKG", "GET_CANCELLED", new Dictionary<string, object>
            {
                ["P_FROM_DATE"] = request.FromDate,
                ["P_TO_DATE"] = request.ToDate
            });
        }

        public ReponseReportEntity GetStatusOperators(DateRangeRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("FLIGHT_STATUS_PKG", "GET_OPERATORS", new Dictionary<string, object>
            {
                ["P_FROM_DATE"] = request.FromDate,
                ["P_TO_DATE"] = request.ToDate
            });
        }

        // ===== DELAY_ALERT_PKG =====
        public ReponseReportEntity GetTodayDelayFlights()
        {
            return Execute("DELAY_ALERT_PKG", "GET_TODAY_FLIGHTS", new Dictionary<string, object>());
        }

        // ===== DAYPLAN_COMPARE_PKG =====
        public ReponseReportEntity GetDayFlights(DayPlanCompareRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("DAYPLAN_COMPARE_PKG", "GET_DAY_FLIGHTS", new Dictionary<string, object>
            {
                ["P_FLIGHT_DATE"] = request.FlightDate,
                ["P_AIRPORT"] = request.Airport,
                ["P_OPER"] = request.Oper
            });
        }

        public ReponseReportEntity GetDayPlanAirports()
        {
            return Execute("DAYPLAN_COMPARE_PKG", "GET_AIRPORTS", new Dictionary<string, object>());
        }

        public ReponseReportEntity GetDayPlanOperators()
        {
            return Execute("DAYPLAN_COMPARE_PKG", "GET_OPERATORS", new Dictionary<string, object>());
        }

        // ===== TRACKING_MAP_PKG =====
        public ReponseReportEntity GetFlightMeta(TrackingMapRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("TRACKING_MAP_PKG", "GET_FLIGHT_META", new Dictionary<string, object>
            {
                ["P_FLIGHT_DATE"] = request.FlightDate,
                ["P_CALLSIGNS"] = request.Callsigns
            });
        }

        // ===== SLOT_COMPARE_PKG =====
        public ReponseReportEntity GetSlotDefaultDate()
        {
            return Execute("SLOT_COMPARE_PKG", "GET_DEFAULT_DATE", new Dictionary<string, object>());
        }

        public ReponseReportEntity GetSlotOperators()
        {
            return Execute("SLOT_COMPARE_PKG", "GET_OPERATORS", new Dictionary<string, object>());
        }

        public ReponseReportEntity GetSlotSummary(SlotCompareRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("SLOT_COMPARE_PKG", "GET_SUMMARY", new Dictionary<string, object>
            {
                ["P_COMPARE_DATE"] = request.CompareDate,
                ["P_OPER"] = request.Oper
            });
        }

        public ReponseReportEntity GetSlotResults(SlotCompareRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("SLOT_COMPARE_PKG", "GET_RESULTS", new Dictionary<string, object>
            {
                ["P_COMPARE_DATE"] = request.CompareDate,
                ["P_RESULT_TYPE"] = request.ResultType,
                ["P_OPER"] = request.Oper,
                ["P_PAGE_SIZE"] = request.PageSize,
                ["P_PAGE_INDEX"] = request.PageIndex
            });
        }

        public ReponseReportEntity GetSlotSourceKhh(SlotCompareRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("SLOT_COMPARE_PKG", "GET_SOURCE_KHH", new Dictionary<string, object>
            {
                ["P_COMPARE_DATE"] = request.CompareDate
            });
        }

        public ReponseReportEntity GetSlotSourceSlot(SlotCompareRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("SLOT_COMPARE_PKG", "GET_SOURCE_SLOT", new Dictionary<string, object>
            {
                ["P_COMPARE_DATE"] = request.CompareDate
            });
        }

        public ReponseReportEntity GetSlotSourcePerm(SlotCompareRequest request)
        {
            if (request == null) return MissingRequest();
            return Execute("SLOT_COMPARE_PKG", "GET_SOURCE_PERM", new Dictionary<string, object>
            {
                ["P_COMPARE_DATE"] = request.CompareDate
            });
        }

        public ReponseReportEntity GetSlotOperatorMap()
        {
            return Execute("SLOT_COMPARE_PKG", "GET_OPERATOR_MAP", new Dictionary<string, object>());
        }

        public ReponseReportEntity GetSlotAirportMap()
        {
            return Execute("SLOT_COMPARE_PKG", "GET_AIRPORT_MAP", new Dictionary<string, object>());
        }

        private static ReponseReportEntity Execute(string packageName, string storeName, Dictionary<string, object> parameters)
        {
            return new AipHelper.ApiHelperRepository().ExcuteDataTable(packageName, storeName, parameters);
        }

        private static ReponseReportEntity MissingRequest()
        {
            return new ReponseReportEntity { Code = "-1", Message = "Dữ liệu yêu cầu không được để trống." };
        }
    }
}
