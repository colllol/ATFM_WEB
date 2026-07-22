using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System.Web.Http.Cors;

namespace QLB.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FpauthorController : ApiController
    {
        FpauthorRepository FpauthorRepository = null;

        public FpauthorController()
        {
            if (FpauthorRepository == null)
            {
                FpauthorRepository = new FpauthorRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageFpauthor(int page_size, int page_index,string where)
        {
            return FpauthorRepository.GetPageFpauthor(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageFpauthorExport(string where)
        {
            return FpauthorRepository.GetPageFpauthorExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllFpauthor()
        {
            return FpauthorRepository.GetAllFpauthor();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdFpauthor(int ID)
        {
            return FpauthorRepository.GetByIdFpauthor(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateFpauthor(Fpauthor oFPAUTHOR)
        {
            return FpauthorRepository.CreateFpauthor(oFPAUTHOR);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateFpauthor(Fpauthor oFPAUTHOR)
        {
            return FpauthorRepository.UpdateFpauthor(oFPAUTHOR);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteFpauthor(int ID)
        {
            return FpauthorRepository.DeleteFpauthor(ID);
        }
    }
}