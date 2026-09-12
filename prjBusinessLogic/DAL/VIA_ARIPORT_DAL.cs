using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class VIA_ARIPORT_DAL
    {
        public List<M_VIA_ARIPORT> GetListPageM_VIA_ARIPORT(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<M_VIA_ARIPORT>().GetListObj("api/Via/GetPageM_VIA_ARIPORT/", pageSize, pageIndex, where);
        }
        public List<M_VIA_ARIPORT> GetListPageM_VIA_ARIPORTExport(string where)
        {
            return new clsResuftAPI<M_VIA_ARIPORT>().GetListObjExportData("api/Via/GetPageM_VIA_ARIPORTExport", where);
        }
        public M_VIA_ARIPORT GetViaById(string id)
        {
            return new clsResuftAPI<M_VIA_ARIPORT>().GetOneObj(id, "api/Via/GetByIdM_VIA_ARIPORT/");
        }

        public bool InsertM_VIA_ARIPORT(M_VIA_ARIPORT obj)
        {
            return new clsResuftAPI<M_VIA_ARIPORT>().InsertObj(obj, "api/Via/CreateM_VIA_ARIPORT");
        }
        public bool UpdateM_VIA_ARIPORT(M_VIA_ARIPORT obj)
        {
            return new clsResuftAPI<M_VIA_ARIPORT>().UpdateObj(obj, "api/Via/UpdateM_VIA_ARIPORT");
        }
        public bool DeleteM_VIA_ARIPORT(int id)
        {
            return new clsResuftAPI<M_VIA_ARIPORT>().DeleteObj(id.ToString(), "api/Via/DeleteM_VIA_ARIPORT/");
        }

    }
}
