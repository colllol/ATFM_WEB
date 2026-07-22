using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;

namespace QLB.API.Controllers
{
    public class NotCompletedFlightsController : ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate)
        {
            return new NotCompletedFlightsRepository().GetPage(page_size, page_index, where, Fromdate, Todate);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 Id)
        {
            return new NotCompletedFlightsRepository().GetByID(Id);
        }
    }
}