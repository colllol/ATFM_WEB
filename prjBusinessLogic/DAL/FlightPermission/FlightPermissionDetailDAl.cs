using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class FlightPermissionDetailDAL
    {
        public bool InsertObject(FlightPermissionDetail obj)
        {
            return new clsResuftAPI<FlightPermissionDetail>().InsertObj(obj, "api/CalendarDetail/CreateCalendarDetail/");
        }
        //public string InsertReturnId(FlightPermissionDetail obj)
        //{
        //    return new clsResuftAPI<FlightPermissionDetail>().InsertReturnId(obj, "api/CalendarDetail/CreateCalendarDetail/"); 
        //}
        public string InsertReturnId(FlightPermissionDetail obj)
        {
            return new clsResuftAPI<FlightPermissionDetail>().InsertReturnId(obj, "api/CalendarDetail/CreateCalendarDetail/");
        }
        public bool UpdateObject(FlightPermissionDetail obj)
        {
            return new clsResuftAPI<FlightPermissionDetail>().UpdateObj(obj, "api/CalendarDetail/UpdateCalendarDetail/");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<FlightPermissionDetail>().DeleteObj(id, "api/CalendarDetail/DeleteCalendarDetail/");
        }

        public FlightPermissionDetail GetOneObject(int id)
        {
            return new clsResuftAPI<FlightPermissionDetail>().GetOneObj(id.ToString(), "api/CalendarDetail/GetByIdCalendarDetail/");
        }
        public List<FlightPermissionDetail> GetAllObject()
        {
            return new clsResuftAPI<FlightPermissionDetail>().GetListObj("api/CalendarDetail/GetAllCalendarDetail/");
        }
        public List<FlightPermissionDetail> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<FlightPermissionDetail>().GetListObj("api/CalendarDetail/GetPageCalendarDetail/", pageSize, pageIndex, where);
        }
        public System.Data.DataTable GetHistoryById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/CalendarDetail/GetHistoryById", id);
        }
        public bool RestoreHis(string id, string iduser, string version)
        {
            return new clsResuftAPI().RestoreRecord("api/CalendarDetail/RestoreHis", id, version, iduser);
        }
    }
}
