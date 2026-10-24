using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class AddressAftnDAL
    {
        public bool InsertObject(AddressAftn obj)
        {
            return new clsResuftAPI<AddressAftn>().InsertObj(obj, "api/Gad/CreateGad");
        }
        public bool UpdateObject(AddressAftn obj)
        {
            return new clsResuftAPI<AddressAftn>().UpdateObj(obj, "api/Gad/UpdateGad");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<AddressAftn>().DeleteObj(id, "api/Gad/DeleteGad/");
        }

        public AddressAftn GetOneGad(string id)
        {
            return new clsResuftAPI<AddressAftn>().GetOneObj(id, "api/Gad/GetByIdGad/");
        }
        public List<AddressAftn> GetAllObject()
        {
            return new clsResuftAPI<AddressAftn>().GetListObj("api/Gad/GetAllGad");
        }
        public List<AddressAftn> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<AddressAftn>().GetListObj("api/Gad/GetPageGad", pageSize, pageIndex, where);
        }

        public List<AddressAftn> GetPageGadExport(string where)
        {
            return new clsResuftAPI<AddressAftn>().GetListObjExportData("api/Gad/GetPageGadExport/", where);
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Gad/GetAllGad");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Gad/GetPageGad/", pageSize, pageIndex, where);
        }
    }
}
