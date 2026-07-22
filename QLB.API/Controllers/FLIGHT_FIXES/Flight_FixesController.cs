using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers.FLIGHT_FIXES
{
    public class Flight_FixesController: ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new FlightFixesRepository().GetPage(page_size, page_index, where);
        }
    }
}