using System.Web.Http;
using QLB.API.Data.Perm;
using QLB.API.Models;
using QLB.Info.Perm;
using System.Web.Http.Cors;

namespace QLB.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OperController : ApiController
    {
        OperRepository OperRepository = null;

        public OperController()
        {
            if (OperRepository == null)
            {
                OperRepository = new OperRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageOper(int page_size, int page_index, string where)
        {
            return OperRepository.GetPageOper(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageOperExport(string where)
        {
            return OperRepository.GetPageOperExport(where);
        }

        [AcceptVerbs("Get")]


        public ReponseEntity GetAllOper()
        {
            return OperRepository.GetAllOper();
        }


        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdOper(int ID)
        {

            return OperRepository.GetByIdOper(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateOper(Oper oOPER)
        {
            return OperRepository.CreateOper(oOPER);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateOper(Oper oOPER)
        {
            return OperRepository.UpdateOper(oOPER);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteOper(int ID)
        {
            return OperRepository.DeleteOper(ID);
        }
    }
}
