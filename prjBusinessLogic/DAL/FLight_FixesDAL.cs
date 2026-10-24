using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class FLight_FixesDAL
    {
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Flight_Fixes/GetPage/", pageSize, pageIndex, where);
        }

        public List<M_FLIGHT_FIXES> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<M_FLIGHT_FIXES>().GetListObj("api/Flight_Fixes/GetPage/", pageSize, pageIndex, where);
        }
    }
}
