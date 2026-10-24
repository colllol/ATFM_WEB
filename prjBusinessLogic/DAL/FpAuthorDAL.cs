using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class FpAuthorDAL
    {
        public bool InsertObject(FpAuthor obj)
        {
            return new clsResuftAPI<FpAuthor>().InsertObj(obj, "api/Fpauthor/CreateFpauthor");
        }
        public bool UpdateObject(FpAuthor obj)
        {
            return new clsResuftAPI<FpAuthor>().UpdateObj(obj, "api/Fpauthor/UpdateFpauthor");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<FpAuthor>().DeleteObj(id, "api/Fpauthor/DeleteFpauthor/");
        }

        public FpAuthor GetOneFpauthor(string id)
        {
            return new clsResuftAPI<FpAuthor>().GetOneObj(id, "api/Fpauthor/GetByIdFpauthor/");
        }
        public List<FpAuthor> GetAllObject()
        {
            return new clsResuftAPI<FpAuthor>().GetListObj("api/Fpauthor/GetAllFpauthor");
        }
        public List<FpAuthor> GetPageFpauthor(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<FpAuthor>().GetListObj("api/Fpauthor/GetPageFpauthor/", pageSize, pageIndex, where);
        }
        public List<FpAuthor> GetPageFpauthorExport(string where)
        {
            return new clsResuftAPI<FpAuthor>().GetListObjExportData("api/Fpauthor/GetPageFpauthorExport/", where);
        }
    }
}
