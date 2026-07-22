using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class PermNoImpController : ApiController
    {
        
        PermNoRepository PermNoRepository = null;

        public PermNoImpController()
        {
            if (PermNoRepository == null)
            {
                PermNoRepository = new PermNoRepository();
            }
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermNoIMP(int page_size, int page_index, string where)
        {
            return PermNoRepository.GetPagePermNoIMP(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPagePermScIMPExport(string where)
        {
            return PermNoRepository.GetPagePermNoIMPExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllPermNOIMP()
        {
            return PermNoRepository.GetAllPermNOIMP();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdPermNOIMP(int Id)
        {
            return PermNoRepository.GetByIdPermNOIMP(Id);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermNOIMP(PermNoIMP oAlarm)
        {
            return PermNoRepository.UpdatePermNOIMP(oAlarm);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeletePermNOIMP(int Id)
        {
            return PermNoRepository.DeletePermNOIMP(Id);
        }
    }
}