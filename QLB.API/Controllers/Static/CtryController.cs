using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class CtryController : ApiController
    {
        CtryRepository CtryRepository = null;

        public CtryController()
        {
            if (CtryRepository == null)
            {
                CtryRepository = new CtryRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageCtry(int page_size, int page_index,string where)
        {
            return CtryRepository.GetPageCtry(page_size, page_index,where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageCtryExport(string where)
        {
            return CtryRepository.GetPageCtryExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllCtry()
        {
            return CtryRepository.GetAllCtry();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdCtry(int Id)
        {
            return CtryRepository.GetByIdCtry(Id);
        }        
        [AcceptVerbs("Post")]
        public ReponseEntity CreateCtry(Ctry oCTRY)
        {
            return CtryRepository.CreateCtry(oCTRY);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateCtry(Ctry oCTRY)
        {
            return CtryRepository.UpdateCtry(oCTRY);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteCtry(int Id)
        {
            return CtryRepository.DeleteCtry(Id);
        }
    }
}
