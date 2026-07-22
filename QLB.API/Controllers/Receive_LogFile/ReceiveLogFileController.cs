using System.Web.Http;
using QLB.API.Data.Receive_LogFile;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers.Receive_LogFile
{
    public class ReceiveLogFileController : ApiController
    {
        //[AcceptVerbs("Get")]
        //public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate)
        //{
        //    return new ReceiveLogFileRepository().GetPage(page_size, page_index, where, Fromdate, Todate);
        //}
        [AcceptVerbs("Put")]
        public ReponseEntity GetPage(clsSearchReceiveLogFile obj)
        {
            return new ReceiveLogFileRepository().GetPage(obj);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 Id)
        {
            return new ReceiveLogFileRepository().GetByID(Id);
        }
    }
}
