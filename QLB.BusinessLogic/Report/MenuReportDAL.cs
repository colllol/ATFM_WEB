using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace QLB.BusinessLogic
{
    public class MenuReportDAL
    {
        public DataSet ReportSS_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportSS_Get_All");
        }
        public DataSet ReportVP_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportVP_Get_All");
        }
        public DataSet ReportDHB_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportDHB_Get_All");
        }
        public DataSet ReportATFM_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportATFM_Get_All");
        }
        public DataSet ReportBTC_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportBTC_Get_All");
        }
        public DataSet ReportStatistic_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportStatistic_Get_All");
        }
        public DataSet ReportScheduled_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ReportScheduled_Get_All");
        }

        public DataSet FlightsGeneral_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "FlightsGeneral_Get_All");
        }
        public DataSet ForeignerFlights_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "ForeignerFlights_Get_All");
        }
        public DataSet FlightsSummarize_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "FlightsSummarize_Get_All");
        }
        public DataSet FlightsStatistic_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "FlightsStatistic_Get_All");
        }
        public DataSet NoneScheduledFlights_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "NoneScheduledFlights_Get_All");
        }
        public DataSet OverFlights_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "OverFlights_Get_All");
        }
        public DataSet VietNameseFlights_Get_All()
        {
            return new oDataProvider().ExecuteDatase("MENU_REPORT_PKG", "VietNameseFlights_Get_All");
        }
    }
}
