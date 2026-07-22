using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLB.API.Models;
using QLB.API.Data.Cant;
using System.Web.Http.Cors;

namespace QLB.API.Controllers.Cant
{
    public class CantSplitController : ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate)
        {
            return new CantSplitRepository().GetPage(page_size, page_index, where, Fromdate, Todate);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 Id)
        {
            return new CantSplitRepository().GetByID(Id);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity Get_Message_ByNbr(string Nbr)
        {
            return new CantSplitRepository().GetByNBR(Nbr);
        }
    }
}
