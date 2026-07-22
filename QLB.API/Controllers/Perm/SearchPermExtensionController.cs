using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;
using System.Net.Http;
using QLB.BusinessLogic;
using System.Data;

namespace QLB.API.Controllers.Perm
{
    public class SearchPermExtensionController : ApiController
    {
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetPermBySearch(SearchPermExtension obj)
        {
            return new ReponseEntityHelper().GetTable(new SearchPermExtensionDAL().GetTableValue(obj));
        }
    }
}
