using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;

namespace QLB.API.Controllers
{
    public class PermMasterNoController : ApiController
    {
        PermMasterNoRepository PermMasterNoRepository = null;

        public PermMasterNoController()
        {
            if (PermMasterNoRepository == null)
            {
                PermMasterNoRepository = new PermMasterNoRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermMasterNo(int page_size, int page_index, string where)
        {
            return PermMasterNoRepository.GetPagePermMasterNo(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermMasterNoExport(string where)
        {
            return PermMasterNoRepository.GetPagePermMasterNoExport(where);
        }

        [AcceptVerbs("Get")]


        public ReponseEntity GetAllPermMasterNo()
        {
            return PermMasterNoRepository.GetAllPermMasterNo();
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdPermMasterNo(int ID)

        {

            return PermMasterNoRepository.GetByIdPermMasterNo(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreatePermMasterNo(PermMasterNo oPERMMASTERNO)
         {
            return PermMasterNoRepository.CreatePermMasterNo(oPERMMASTERNO);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermMasterNo(PermMasterNo oPERMMASTERNO)
        {
            return PermMasterNoRepository.UpdatePermMasterNo(oPERMMASTERNO);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeletePermMasterNo(int ID)
        {
            return PermMasterNoRepository.DeletePermMasterNo(ID);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetRecordDeleted(string where)
        {
            return new PermMasterNoRepository().GetDeleted(where);
        }
    }
}
