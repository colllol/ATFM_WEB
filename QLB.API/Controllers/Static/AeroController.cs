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
    public class AeroController:ApiController
    {
        AeroRepository AeroRepository = null;
        const string _App01 = "*";
        public AeroController()
        {
            if (AeroRepository == null)
            {
                AeroRepository = new AeroRepository();
            }
        }

        [AcceptVerbs("Get")]
        
        public ReponseEntity GetPageAero(int page_size, int page_index,string where)
        {
            return AeroRepository.GetPageAero(page_size, page_index,where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageAeroExport(string where)
        {
            return AeroRepository.GetPageAeroExport(where);
        }

        [AcceptVerbs("Get")]
        [EnableCors(_App01, "*", "*")]
        
        public ReponseEntity GetAllAero()
        {
            return AeroRepository.GetAllAero();
        }
        [AcceptVerbs("Get")]
        [EnableCors(_App01, "*", "*")]

        public ReponseEntity GetAeroVV()
        {
            return AeroRepository.GetAeroVV();
        }

        [AcceptVerbs("Get")]
        
        public ReponseEntity GetByIdAero(int ID)
        {
            
            return AeroRepository.GetByIdAero(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateAero(Aero oAERO)
        {
            return AeroRepository.CreateAero(oAERO);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateAero(Aero oAERO)
        {
            return AeroRepository.UpdateAero(oAERO);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAero(int ID)
        {
            return AeroRepository.DeleteAero(ID);
        }
    }
}