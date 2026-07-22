using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Jwt.Filters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers
{
    public class SectorController : ApiController
    {
        SectorRepository AeroRepository = null;
        const string _App01 = "*";
        public SectorController()
        {
            if (AeroRepository == null)
            {
                AeroRepository = new SectorRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageSector(int page_size, int page_index, string where)
        {
            return AeroRepository.GetPageSector(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageSectorExport(string where)
        {
            return AeroRepository.GetPageSectorExport(where);
        }

        [AcceptVerbs("Get")]
        [EnableCors(_App01, "*", "*")]

        public ReponseEntity GetAllSector()
        {
            return AeroRepository.GetAllSectors();
        }
        

        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdSector(int ID)
        {

            return AeroRepository.GetByIdSector(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateSector(Sectors oAERO)
        {
            return AeroRepository.CreateSector(oAERO);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateSector(Sectors oAERO)
        {
            return AeroRepository.UpdateSector(oAERO);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteSectors(int ID)
        {
            return AeroRepository.DeleteSector(ID);
        }
    }
}