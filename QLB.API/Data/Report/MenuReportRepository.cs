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

namespace QLB.API.Data
{
    public class MenuReportRepository
    {
        public ReponseEntity ReportSS_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportSS_Get_All().Tables[0]);
        }
        public ReponseEntity ReportVP_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportVP_Get_All().Tables[0]);
        }
        public ReponseEntity ReportDHB_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportDHB_Get_All().Tables[0]);
        }
        public ReponseEntity ReportATFM_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportATFM_Get_All().Tables[0]);
        }
        public ReponseEntity ReportBTC_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportBTC_Get_All().Tables[0]);
        }
        public ReponseEntity ReportStatistic_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportStatistic_Get_All().Tables[0]);
        }
        public ReponseEntity ReportScheduled_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ReportScheduled_Get_All().Tables[0]);
        }
        public ReponseEntity FlightsGeneral_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().FlightsGeneral_Get_All().Tables[0]);
        }
        public ReponseEntity ForeignerFlights_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().ForeignerFlights_Get_All().Tables[0]);
        }
        public ReponseEntity FlightsSummarize_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().FlightsSummarize_Get_All().Tables[0]);
        }
        public ReponseEntity FlightsStatistic_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().FlightsStatistic_Get_All().Tables[0]);
        }
        public ReponseEntity NoneScheduledFlights_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().NoneScheduledFlights_Get_All().Tables[0]);
        }
        public ReponseEntity OverFlights_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().OverFlights_Get_All().Tables[0]);
        }
        public ReponseEntity VietNameseFlights_Get_All()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new MenuReportDAL().VietNameseFlights_Get_All().Tables[0]);
        }
    }
}