using System.Web.Http;
using QLB.API.Data.FlyPlanType;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers.FlyPlanType
{
    public class FlyPlanTypeController : ApiController
    {
        [AcceptVerbs("Put")]
        public ReponseEntity GetPage(clsSearchFlyplanType obj)
        {
            return new FlyPlanTypeRepository().GetPage(obj);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(string Id)
        {
            return new FlyPlanTypeRepository().GetByID(Id);
        }
    }
}