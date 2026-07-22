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
    public class GamController : ApiController
    {
        GamRepository GamRepository = null;

        public GamController()
        {
            if (GamRepository == null)
            {
                GamRepository = new GamRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGam(int page_size, int page_index,string where)
        {
            return GamRepository.GetPageGam(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGamExport(string where)
        {
            return GamRepository.GetPageGamExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllGam()
        {
            return GamRepository.GetAllGam();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdGam(int ID)
        {
            return GamRepository.GetByIdGam(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateGam(Gam oGAM)
        {
            return GamRepository.CreateGam(oGAM);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateGam(Gam oGAM)
        {
            return GamRepository.UpdateGam(oGAM);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteGam(int ID)
        {
            return GamRepository.DeleteGam(ID);
        }
    }
}