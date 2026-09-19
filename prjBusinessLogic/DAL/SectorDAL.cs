using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class SectorDAL
    {
        public List<Sectors> GetListAll()
        {
            return new clsResuftAPI<Sectors>().GetListObj("api/Sector/GetAllSector");
        }
       
        public List<Sectors> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Sectors>().GetListObj("api/Sector/GetPageSector/", pageSize, pageIndex, where);
        }
        public List<Sectors> GetListPageExport(string where)
        {
            return new clsResuftAPI<Sectors>().GetListObjExportData("api/Sector/GetPageSectorExport", where);
        }
        public Sectors GetSectorsById(string id)
        {
            return new clsResuftAPI<Sectors>().GetOneObj(id, "api/Sector/GetByIdSector/");
        }

        public bool InsertSectors(Sectors obj)
        {
            return new clsResuftAPI<Sectors>().InsertObj(obj, "api/Sector/CreateSector");
        }
        public bool UpdateSectors(Sectors obj)
        {
            return new clsResuftAPI<Sectors>().UpdateObj(obj, "api/Sector/UpdateSector");
        }
        public bool DeleteSectors(int id)
        {
            return new clsResuftAPI<Sectors>().DeleteObj(id.ToString(), "api/Sector/DeleteSectors/");
        }
               
    }
}
