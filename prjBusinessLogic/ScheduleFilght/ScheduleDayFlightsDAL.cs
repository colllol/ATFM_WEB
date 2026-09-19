using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class ScheduleDayFlightsDAL
    {
        public System.Data.DataTable GetExportFull(string date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("SCHEDULEDAYFLIGHTS_2020_PKG", "sp_ExpDHBFull", new { P_DATE = date });
        }
        public System.Data.DataTable GetExportSchedule(string date,string pdate)
        {
            return new clsResuftAPI().GetPostTableApiExtension("SCHEDULEDAYFLIGHTS_2020_PKG", "sp_ExpDHBScheule", new { P_DATE = date,P_EDATE=pdate });
        }
        public System.Data.DataTable GetExport(string date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("SCHEDULEDAYFLIGHTS_2020_PKG", "sp_ExpDHB", new { P_DATE = date });
        }
        public System.Data.DataTable GetTableWithValueSearch(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ScheduleFlights/GetBySearchAll", obj);
        }
        public ScheduleDayFlights GetObjectById(string id)
        {
            return new clsResuftAPI<ScheduleDayFlights>().GetOneObj(id.ToString(), "api/ScheduleFlights/GetById");
        }
        public Int64 InsertReturnId(ScheduleDayFlights obj)
        {
            return Convert.ToInt64(new clsResuftAPI<ScheduleDayFlights>().InsertReturnId(obj, "api/ScheduleFlights/Insert").ToString());
        }
        public bool Update(ScheduleDayFlights obj)
        {
            return new clsResuftAPI<ScheduleDayFlights>().UpdateObj(obj, "api/ScheduleFlights/Update").ToString().ToString() == "-1" ? false : true;
        }
        public bool Delete(string id)
        {
            return new clsResuftAPI<ScheduleDayFlights>().DeleteObj(id, "api/ScheduleFlights/Delete").ToString() == "-1" ? false : true;
        }
        public Int64 RenderTo_DayFlight(DateTime date, string userId)
        {
            return Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("SCHEDULEDAYFLIGHTS_2020_PKG", "RenderKeHoachBayNgay", new { P_DATEFLIGHT = date, P_USER = userId }).ToString());
        }
        public System.Data.DataTable GetHisById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/ScheduleFlights/GetHistoryById", id);
        }
        public bool RetoryHis(string id, string version, string iduser)
        {
            return new clsResuftAPI().RestoreRecord("api/ScheduleFlights/RestoreHis", id, version, iduser);
        }
        public System.Data.DataTable GetTableDelete(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ScheduleFlights/GetTableDelete", obj);
        }
    }
}
