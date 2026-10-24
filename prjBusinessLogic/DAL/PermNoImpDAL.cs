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
    public class PermNoImpDAL
    {
        public List<PermNoIMP> GetPagePermNoIMP(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<PermNoIMP>().GetListObj("api/PermNoImp/GetPagePermNoIMP", pageSize, pageIndex, where);
        }

        public List<PermNoIMP> GetPagePermNoIMPExport(string where)
        {
            return new clsResuftAPI<PermNoIMP>().GetListObjExportData("api/PermNoImp/GetPagePermScIMPExport", where);
        }

        public List<PermNoIMP> GetAllPermNOIMP()
        {
            return new clsResuftAPI<PermNoIMP>().GetListObj("api/PermNoImp/GetAllPermNOIMP");
        }

        public bool UpdatePermNoIMPAsync(PermNoIMP obj)
        {
            return new clsResuftAPI<PermNoIMP>().UpdateObj(obj, "api/PermNoImp/UpdatePermNOIMP");
        }

        public bool DeletePermNoIMP(string id)
        {
            return new clsResuftAPI<PermNoIMP>().DeleteObj(id, "api/PermNoImp/DeletePermNOIMP/");
        }

        public PermNoIMP GetOnePermNoIMP(int id)
        {
            return new clsResuftAPI<PermNoIMP>().GetOneObj(id.ToString(), "api/PermNoImp/GetByIdPermNOIMP/");
        }
    }
}
