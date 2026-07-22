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
    public class GadController : ApiController
    {
        GadRepository GadRepository = null;

        public GadController()
        {
            if (GadRepository == null)
            {
                GadRepository = new GadRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGad(int page_size, int page_index, string where)
        {
            return GadRepository.GetPageGad(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGadExport(string where)
        {
            return GadRepository.GetPageGadExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllGad()
        {
            return GadRepository.GetAllGad();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdGad(int ID)
        {
            return GadRepository.GetByIdGad(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateGad(Gad oGAD)
        {
            return GadRepository.CreateGad(oGAD);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateGad(Gad oGAD)
        {
            return GadRepository.UpdateGad(oGAD);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteGad(int ID)
        {
            return GadRepository.DeleteGad(ID);
        }
    }
}
