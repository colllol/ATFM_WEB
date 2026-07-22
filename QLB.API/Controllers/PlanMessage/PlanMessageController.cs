using System.Web.Http;
using QLB.API.Data.FlyPlanType;
using QLB.API.Models;
using QLB.API.Data;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers
{
    public class PlanMessageController : ApiController
    {
        [AcceptVerbs("Put")]
        public ReponseEntity GetPage(clsSearchPlanMessageNew obj)
        {
            return new PlanMessageRepository().GetPage(obj);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(string date,int part,string mestype)
        {
            return new PlanMessageRepository().GetByID(date, part, mestype);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity GetPageRealplanLetter(clsSearchRealplanLetter obj)
        {
            return new PlanMessageRepository().GetPageRealplanLetter(obj);
        }
    }
}