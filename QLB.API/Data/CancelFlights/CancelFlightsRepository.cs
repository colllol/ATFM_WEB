using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class CancelFlightsRepository
    {
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<CancelFlights>(new CancelFlightsDAL().GetPage(page_size, page_index, where));
        }

        public ReponseEntity GET_CANCEL_FLIGHTS(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new CancelFlightsDAL().GET_CANCEL_FLIGHTS(finished_flight));
        }
    }
}