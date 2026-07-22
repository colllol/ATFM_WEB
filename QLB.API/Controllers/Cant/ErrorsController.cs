using System.Web.Http;
using QLB.API.Data.Cant;
using QLB.API.Models;
using QLB.API.Data;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers.Cant
{
    public class ErrorsController : ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageErrors(int page_size, int page_index, string where)
        {
            return new ErrorsRepository().GetPageErrors(page_size, page_index, where);
        }
    }
}
