using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class FlyPurposeDAL
    {
        public bool InsertObject(FlyPurpose obj)
        {
            return new clsResuftAPI<FlyPurpose>().InsertObj(obj, "api/FlyPurpose/CreateFlyPurpose");
        }
        public bool UpdateObject(FlyPurpose obj)
        {
            return new clsResuftAPI<FlyPurpose>().UpdateObj(obj, "api/FlyPurpose/UpdateFlyPurpose");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<FlyPurpose>().DeleteObj(id, "api/FlyPurpose/DeleteFlyPurpose/");
        }

        public FlyPurpose GetOneFlyPurpose(string id)
        {
            return new clsResuftAPI<FlyPurpose>().GetOneObj(id, "api/FlyPurpose/GetByIdFlyPurpose/");
        }
        public List<FlyPurpose> GetAllObject()
        {
            return new clsResuftAPI<FlyPurpose>().GetListObj("api/FlyPurpose/GetAllFlyPurpose");
        }
        public List<FlyPurpose> GetPageFlyPurpose(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<FlyPurpose>().GetListObj("api/FlyPurpose/GetPageFlyPurpose/", pageSize, pageIndex, where);
        }
        public List<FlyPurpose> GetPageFlyPurposeExport(string where)
        {
            return new clsResuftAPI<FlyPurpose>().GetListObjExportData("api/FlyPurpose/GetPageFlyPurposeExport/", where);
        }
    }
}
