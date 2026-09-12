using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class CraftTypeDAL
    {
        public bool InsertObject(CraftType obj)
        {
            return new clsResuftAPI<CraftType>().InsertObj(obj, "api/CraftType/CreateCraftType");
        }
        public bool UpdateObject(CraftType obj)
        {
            return new clsResuftAPI<CraftType>().UpdateObj(obj, "api/CraftType/UpdateCraftType");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<CraftType>().DeleteObj(id, "api/CraftType/DeleteCraftType/");
        }

        public CraftType GetOneCraftType(int id)
        {
            return new clsResuftAPI<CraftType>().GetOneObj(id.ToString(), "api/CraftType/GetByIdCraftType/");
        }
        public List<CraftType> GetAllCraftType()
        {
            return new clsResuftAPI<CraftType>().GetListObj("api/CraftType/GetAllCraftType");
        }
        public List<CraftType> GetPageCraftType(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<CraftType>().GetListObj("api/CraftType/GetPageCraftType/", pageSize, pageIndex, where);
        }
        public List<CraftType> GetPageCraftTypeExport(string where)
        {
            return new clsResuftAPI<CraftType>().GetListObjExportData("api/CraftType/GetPageCraftTypeExport/", where);
        }
    }
}
