using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using QLB.BusinessLogic;

namespace QLB.API
{
    public class CancelFlightsController : ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new CancelFlightsRepository().GetPage(page_size, page_index, where);
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_CANCEL_FLIGHTS(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new CancelFlightsDAL().GET_CANCEL_FLIGHTS(_obj));
        }
    }
}