using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using prjInfo;
using System.Threading.Tasks;

namespace prjBusinessLogic
{
    public class MenuReportDAL
    {
        public List<ReportStatic> ReportSS_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportSS_Get_All");
        }
        public List<ReportStatic> ReportVP_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportVP_Get_All");
        }
        public List<ReportStatic> ReportDHB_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportDHB_Get_All");
        }
        public List<ReportStatic> ReportATFM_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportATFM_Get_All");
        }
        public List<ReportStatic> ReportBTC_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportBTC_Get_All");
        }
        public List<ReportStatic> ReportStatistic_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportStatistic_Get_All");
        }
        public List<ReportStatic> ReportScheduled_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ReportScheduled_Get_All");
        }
        public List<ReportStatic> FlightsGeneral_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/FlightsGeneral_Get_All");
        }
        public List<ReportStatic> ForeignerFlights_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/ForeignerFlights_Get_All");
        }
        public List<ReportStatic> FlightsSummarize_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/FlightsSummarize_Get_All");
        }
        public List<ReportStatic> FlightsStatistic_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/FlightsStatistic_Get_All");
        }
        public List<ReportStatic> NoneScheduledFlights_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/NoneScheduledFlights_Get_All");
        }
        public List<ReportStatic> OverFlights_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/OverFlights_Get_All");
        }
        public List<ReportStatic> VietNameseFlights_Get_All()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/MenuReport/VietNameseFlights_Get_All");
        }

    }
}
