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
    public class CraftTypeController : ApiController
    {
        CraftTypeRepository CraftTypeRepository = null;

        public CraftTypeController()
        {
            if (CraftTypeRepository == null)
            {
                CraftTypeRepository = new CraftTypeRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageCraftType(int page_size, int page_index,string where)
        {
            return CraftTypeRepository.GetPageCraftType(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageCraftTypeExport(string where)
        {
            return CraftTypeRepository.GetPageCraftTypeExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllCraftType()
        {
            return CraftTypeRepository.GetAllCraftType();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdCraftType(int ID)
        {
            return CraftTypeRepository.GetByIdCraftType(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateCraftType(CraftType oCRAFTTYPE)
        {
            return CraftTypeRepository.CreateCraftType(oCRAFTTYPE);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateCraftType(CraftType oCRAFTTYPE)
        {
            return CraftTypeRepository.UpdateCraftType(oCRAFTTYPE);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteCraftType(int ID)
        {
            return CraftTypeRepository.DeleteCraftType(ID);
        }
    }
}