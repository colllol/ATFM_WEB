using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using prjInfo;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
//using System.Net.Http.Formatting;

namespace prjBusinessLogic
{
    public  class PermScImpDAL
    {
        public List<PermScIMP> GetPagePermScIMP(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<PermScIMP>().GetListObj("api/PermScImp/GetPagePermScIMP", pageSize, pageIndex, where);
        }

        public List<PermScIMP> GetPagePermScIMPExport(string where)
        {
            return new clsResuftAPI<PermScIMP>().GetListObjExportData("api/PermScImp/GetPagePermScIMPExport", where);
        }

        public List<PermScIMP> GetAllPermSCIMP()
        {
            return new clsResuftAPI<PermScIMP>().GetListObj("api/PermScImp/GetAllPermSCIMP");
        }

        public bool UpdatePermSCIMPAsync(PermScIMP obj)
        {
            return new clsResuftAPI<PermScIMP>().UpdateObj(obj, "api/PermScImp/UpdatePermSCIMP");
        }

        public bool DeletePermScIMP(string id)
        {
            return new clsResuftAPI<PermScIMP>().DeleteObj(id, "api/PermScImp/DeletePermSCIMP/");
        }

        public PermScIMP GetOnePermScIMP(int id)
        {
            return new clsResuftAPI<PermScIMP>().GetOneObj(id.ToString(), "api/PermScImp/GetByIdPermSCIMP/");
        }
    }
}
