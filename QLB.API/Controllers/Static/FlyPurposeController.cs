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
    public class FlyPurposeController : ApiController
    {
        FlyPurposeRepository FlyPurposeRepository = null;

        public FlyPurposeController()
        {
            if (FlyPurposeRepository == null)
            {
                FlyPurposeRepository = new FlyPurposeRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageFlyPurpose(int page_size, int page_index,string where)
        {
            return FlyPurposeRepository.GetPageFlyPurpose(page_size, page_index,where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageFlyPurposeExport(string where)
        {
            return FlyPurposeRepository.GetPageFlyPurposeExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllFlyPurpose()
        {
            return FlyPurposeRepository.GetAllFlyPurpose();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdFlyPurpose(int ID)
        {
            return FlyPurposeRepository.GetByIdFlyPurpose(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateFlyPurpose(FlyPurpose oFLYPURPOSE)
        {
            return FlyPurposeRepository.CreateFlyPurpose(oFLYPURPOSE);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateFlyPurpose(FlyPurpose oFLYPURPOSE)
        {
            return FlyPurposeRepository.UpdateFlyPurpose(oFLYPURPOSE);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteFlyPurpose(int ID)
        {
            return FlyPurposeRepository.DeleteFlyPurpose(ID);
        }
    }
}