using QLB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLB.API.Data;

namespace QLB.API.Controllers.Report
{
    public class MenuReportController : ApiController
    {
        [AcceptVerbs("Get")]
        public ReponseEntity ReportSS_Get_All()
        {
            return new MenuReportRepository().ReportSS_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity ReportVP_Get_All()
        {
            return new MenuReportRepository().ReportVP_Get_All();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity ReportDHB_Get_All()
        {
            return new MenuReportRepository().ReportDHB_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity ReportATFM_Get_All()
        {
            return new MenuReportRepository().ReportATFM_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity ReportBTC_Get_All()
        {
            return new MenuReportRepository().ReportBTC_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity ReportStatistic_Get_All()
        {
            return new MenuReportRepository().ReportStatistic_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity ReportScheduled_Get_All()
        {
            return new MenuReportRepository().ReportScheduled_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity FlightsGeneral_Get_All()
        {
            return new MenuReportRepository().FlightsGeneral_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity ForeignerFlights_Get_All()
        {
            return new MenuReportRepository().ForeignerFlights_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity FlightsSummarize_Get_All()
        {
            return new MenuReportRepository().FlightsSummarize_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity FlightsStatistic_Get_All()
        {
            return new MenuReportRepository().FlightsStatistic_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity NoneScheduledFlights_Get_All()
        {
            return new MenuReportRepository().NoneScheduledFlights_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity OverFlights_Get_All()
        {
            return new MenuReportRepository().OverFlights_Get_All();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity VietNameseFlights_Get_All()
        {
            return new MenuReportRepository().VietNameseFlights_Get_All();
        }
    }
}
