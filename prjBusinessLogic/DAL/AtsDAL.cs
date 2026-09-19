using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public  class AtsDAL
    {
        public List<ATSROUTE> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<ATSROUTE>().GetListObj("api/Ats/GetPageAtsRoute/", pageSize, pageIndex, where);
        }

        public bool DeleteAtsRoute(int id)
        {
            return new clsResuftAPI<ATSROUTE>().DeleteObj(id.ToString(), "api/Ats/DeleteAtsRoute/");
        }

        public List<AtsFIR> GetListPageFir(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<AtsFIR>().GetListObj("api/Ats/GetPageAtsFir/", pageSize, pageIndex, where);
        }

        public bool DeleteAtsFir(int id)
        {
            return new clsResuftAPI<AtsFIR>().DeleteObj(id.ToString(), "api/Ats/DeleteAtsFir/");
        }

        public List<ATSWAYPOINT> GetListPageWay(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<ATSWAYPOINT>().GetListObj("api/Ats/GetPageAtsWay/", pageSize, pageIndex, where);
        }

        public bool DeleteAtsWay(int id)
        {
            return new clsResuftAPI<ATSWAYPOINT>().DeleteObj(id.ToString(), "api/Ats/DeleteAtsWay/");
        }

        public List<AREODROME> GetListPageAreo(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<AREODROME>().GetListObj("api/Ats/GetPageAtsAreo/", pageSize, pageIndex, where);
        }

        public bool DeleteAtsAreo(int id)
        {
            return new clsResuftAPI<AREODROME>().DeleteObj(id.ToString(), "api/Ats/DeleteAtsAreo/");
        }

    }
}
