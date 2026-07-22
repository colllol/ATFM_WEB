using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class PermScImpController : ApiController
    {
        PermScRepository PermScRepository = null;


        public PermScImpController()
        {
            if (PermScRepository == null)
            {
                PermScRepository = new PermScRepository();
            }
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermScIMP(int page_size, int page_index, string where)
        {
            return PermScRepository.GetPagePermScIMP(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPagePermScIMPExport(string where)
        {
            return PermScRepository.GetPagePermScIMPExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllPermSCIMP()
        {
            return PermScRepository.GetAllPermSCIMP();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdPermSCIMP(int Id)
        {
            return PermScRepository.GetByIdPermSCIMP(Id);
        }
       
        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermSCIMP(PermScIMP oAlarm)
        {
            return PermScRepository.UpdatePermSCIMP(oAlarm);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeletePermSCIMP(int Id)
        {
            return PermScRepository.DeletePermSCIMP(Id);
        }
    }
}