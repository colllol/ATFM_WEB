using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
//using Oracle.ManagedDataAccess;
using QLB.BusinessLogic;
using System.Reflection;

namespace QLB.API.Data.ScheduleFlights
{
    public class ScheduleDayFlightsRepository
    {
        public ReponseEntity Update(ScheduleDayFlight obj)
        {
            return new ReponseEntityHelper().Update<ScheduleDayFlight, ScheduleDayFlightDAL>(obj, "Update");
        }
    }
}