using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class FlightPermissionDAL
    {
        public string InsertObjectReturn(FlightPermission obj)
        {
            return new clsResuftAPI<FlightPermission>().InsertReturnId(obj, "api/Calendar/CreateCalendar/");
        }
        public bool InsertObject(FlightPermission obj)
        {
            return new clsResuftAPI<FlightPermission>().InsertObj(obj, "api/Calendar/CreateCalendar/");
        }
        public bool UpdateObject(FlightPermission obj)
        {
            return new clsResuftAPI<FlightPermission>().UpdateObj(obj, "api/Calendar/UpdateCalendar/");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<FlightPermission>().DeleteObj(id, "api/Calendar/DeleteCalendar/");
        }

        public FlightPermission GetOneObject(int id)
        {
            return new clsResuftAPI<FlightPermission>().GetOneObj(id.ToString(), "api/Calendar/GetByIdCalendar/");
        }
        public List<FlightPermission> GetAllObject()
        {
            return new clsResuftAPI<FlightPermission>().GetListObj("api/Calendar/GetAllCalendar/");
        }
        public List<FlightPermission> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<FlightPermission>().GetListObj("api/Calendar/GetPageCalendar", pageSize, pageIndex, where);
        }
    }
}
