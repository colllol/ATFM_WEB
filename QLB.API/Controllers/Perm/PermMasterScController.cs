using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class PermMasterScController : ApiController
    {
        PermMasterScRepository PermMasterScRepository = null;

        public PermMasterScController()
        {
            if (PermMasterScRepository == null)
            {
                PermMasterScRepository = new PermMasterScRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermMasterSc(int page_size, int page_index, string where)
        {
            return PermMasterScRepository.GetPagePermMasterSc(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermMasterScExport(string where)
        {
            return PermMasterScRepository.GetPagePermMasterScExport(where);
        }


        [AcceptVerbs("Get")]


        public ReponseEntity GetAllPermMasterSc()
        {
            return PermMasterScRepository.GetAllPermMasterSc();
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdPermMasterSc(int ID)
        {

            return PermMasterScRepository.GetByIdPermMasterSc(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreatePermMasterSc(PermMasterSc oPERMMASTER_SC)
        {
            return PermMasterScRepository.CreatePermMasterSc(oPERMMASTER_SC);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermMasterSc(PermMasterSc oPERMMASTER_SC)
        {
            return PermMasterScRepository.UpdatePermMasterSc(oPERMMASTER_SC);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeletePermMasterSc(int ID)
        {
            return PermMasterScRepository.DeletePermMasterSc(ID);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetRecordDeleted(string where)
        {
            return new PermMasterScRepository().GetDeleted(where);
        }
    }
}
