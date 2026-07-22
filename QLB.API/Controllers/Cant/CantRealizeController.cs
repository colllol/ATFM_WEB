using System.Web.Http;
using QLB.API.Data.Cant;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers.Cant
{
    public class CantRealizeController: ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new CantRealizeRepository().GetPage(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate)
        {
            return new CantRealizeRepository().GetPage(page_size, page_index, where, Fromdate, Todate);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 Id)
        {
            return new CantRealizeRepository().GetByID(Id);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity Get_Message_ByNbr(string Nbr)
        {
            return new CantRealizeRepository().GetByNBR(Nbr);
        }
    }
}