using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class AftnDAL
    {
        public bool InsertObject(Aftn obj)
        {
            return new clsResuftAPI<Aftn>().InsertObj(obj, "api/Gam/CreateGam");
        }
        public bool UpdateObject(Aftn obj)
        {
            return new clsResuftAPI<Aftn>().UpdateObj(obj, "api/Gam/UpdateGam");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<Aftn>().DeleteObj(id, "api/Gam/DeleteGam/");
        }

        public Aftn GetOneGam(string id)
        {
            return new clsResuftAPI<Aftn>().GetOneObj(id, "api/Gam/GetByIdGam/");
        }
        public List<Aftn> GetAllObject()
        {
            return new clsResuftAPI<Aftn>().GetListObj("api/Gam/GetAllGam");
        }
        public List<Aftn> GetPageObject(int pageSize, int pageIndex,string where)
        {
            return new clsResuftAPI<Aftn>().GetListObj("api/Gam/GetPageGam/", pageSize,pageIndex, where);
        }

        public List<Aftn> GetPageGamExport(string where) 
        {
            return new clsResuftAPI<Aftn>().GetListObjExportData("api/Gam/GetPageGamExport/", where);
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Gam/GetAllGam");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Gam/GetPageGam/", pageSize, pageIndex, where);
        }
    }
}
