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
    public class PermissionInboxController : ApiController
    {
        [AcceptVerbs("Put")]
        public ReponseEntity GetPage(clsSearchPermissionInbox obj)
        {
            return new PermissionInboxRepository().GetPage(obj);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(string Id)
        {
            return new PermissionInboxRepository().GetByID(Id);
        }
    }
}