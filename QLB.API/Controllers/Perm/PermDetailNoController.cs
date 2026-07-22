using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;

namespace QLB.API.Controllers
{
    public class PermDetailNoController : ApiController
    {
        PermDetailNoRepository PermDetailNoRepository = null;

        public PermDetailNoController()
        {
            if (PermDetailNoRepository == null)
            {
                PermDetailNoRepository = new PermDetailNoRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermDetailNo(int page_size, int page_index, string where)
        {
            return PermDetailNoRepository.GetPagePermDetailNo(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermDetailNoExport(string where)
        {
            return PermDetailNoRepository.GetPagePermDetailNoExport(where);
        }

        [AcceptVerbs("Get")]


        public ReponseEntity GetAllPermDetailNo()
        {
            return PermDetailNoRepository.GetAllPermDetailNo();
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdPermDetailNo(int ID)
        {

            return PermDetailNoRepository.GetByIdPermDetailNo(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreatePermDetailNo(PermDetailNo oPERMDETAILNO)
        {
            return PermDetailNoRepository.CreatePermDetailNo(oPERMDETAILNO);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermDetailNo(PermDetailNo oPERMDETAILNO)
        {
            return PermDetailNoRepository.UpdatePermDetailNo(oPERMDETAILNO);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermDetailNo_GenBack(PermDetailNo oPERMDETAILNO)
        {
            return PermDetailNoRepository.UpdatePermDetailNo_GenBack(oPERMDETAILNO);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeletePermDetailNo(Int64 ID)
        {
            return PermDetailNoRepository.DeletePermDetailNo(ID);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetRecordDeleted(string where)
        {
            return new PermDetailNoRepository().GetDeleted(where);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity GetBySearch(PermDetailNo_Search obj)
        {
            return PermDetailNoRepository.GetBySearch(obj);
        }
    }
}
