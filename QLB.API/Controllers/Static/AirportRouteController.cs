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
    public class AirportRouteController : ApiController
    {
        AirportRouteRepository AirportRouteRepository = null;

        public AirportRouteController()
        {
            if (AirportRouteRepository == null)
            {
                AirportRouteRepository = new AirportRouteRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageAirportRoute(int page_size, int page_index,string where)
        {
            return AirportRouteRepository.GetPageAirportRoute(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageAirportRouteExport(string where)
        {
            return AirportRouteRepository.GetPageAirportRouteExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllAirportRoute()
        {
            return AirportRouteRepository.GetAllAirportRoute();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdAirportRoute(int ID)
        {
            return AirportRouteRepository.GetByIdAirportRoute(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateAirportRoute(AirportRoute oAIRPORTROUTE)
        {
            return AirportRouteRepository.CreateAirportRoute(oAIRPORTROUTE);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateAirportRoute(AirportRoute oAIRPORTROUTE)
        {
            return AirportRouteRepository.UpdateAirportRoute(oAIRPORTROUTE);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAirportRoute(int ID)
        {
            return AirportRouteRepository.DeleteAirportRoute(ID);
        }
    }
}