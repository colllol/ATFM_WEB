using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers
{
    public class AtklController : ApiController
    {
        AtklRepository AtklRepos = new AtklRepository();

        [AcceptVerbs("Put", "Post")]
        public ReponseEntity GetPage(clsSearchAtkl obj)
        {
            return new AtklRepository().GetPage(obj);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 Id)
        {
            return new AtklRepository().GetByID(Id);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity UpdateAtkl(Atkl oAtkl)
        {
            return AtklRepos.UpdateAtkl(oAtkl);
        }
    }
}
