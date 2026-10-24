using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using prjInfo;

namespace prjBusinessLogic
{
   public class ReportBTCDAL
    {
        #region ReportBTC_QuaCanh
        public DataTable QC01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC01", obj);
        }
        public DataTable QC02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC02", obj);
        }
        public DataTable QC03(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC03", obj);
        }
        public DataTable QC04(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC04", obj);
        }
        public DataTable QC05(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC05", obj);
        }
        public DataTable QC06(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC06", obj);
        }
        public DataTable QC07(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC07", obj);
        }
        public DataTable QC08(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC08", obj);
        }
        public DataTable QC09(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC09", obj);
        }
        public DataTable QC10(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC10", obj);
        }
        public DataTable QC11(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC11", obj);
        }
        public DataTable QC12(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC12", obj);
        }
        public DataTable QC13(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC13", obj);
        }
        public DataTable QC14(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC14", obj);
        }
        public DataTable QC15(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC15", obj);
        }
        public DataTable QC16(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC16", obj);
        }
        public DataTable QC17(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC17", obj);
        }
        public DataTable QC18(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC18", obj);
        }
        public DataTable QC19(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QC19", obj);
        }
        #endregion
        #region ReportBTC_QuocNoi
        public DataTable QN01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN01", obj);
        }
        public DataTable QN02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN02", obj);
        }
        public DataTable QN03(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN03", obj);
        }
        public DataTable QN04(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN04", obj);
        }
        public DataTable QN05(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN05", obj);
        }
        public DataTable QN06(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN06", obj);
        }
        public DataTable QN07(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN07", obj);
        }
        public DataTable QN08(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN08", obj);
        }
        public DataTable QN09(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/QN09", obj);
        }
        #endregion
        #region ReportBTC_QuocTeDiDen
        public DataTable BCMienThu01 (clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/BCMienThu01", obj);
        }
        public DataTable BCMienThu02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/BCMienThu02", obj);
        }
        public DataTable BCThuTMTS01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/BCThuTMTS01", obj);
        }
        public DataTable BCThuTMTS02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/BCThuTMTS02", obj);
        }
        public DataTable CTMThu01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/CTMThu01", obj);
        }
        public DataTable CTMThu02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/CTMThu02", obj);
        }
        public DataTable CTTSan01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/CTTSan01", obj);
        }
        public DataTable CTTSan02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/CTTSan02", obj);
        }
        public DataTable CTTsau01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/CTTsau01", obj);
        }
        public DataTable CTTsau02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/CTTsau02", obj);
        }
        public DataTable ListOffFlights01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/ListOffFlights01", obj);
        }
        public DataTable ListOffFLights02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/ListOffFLights02", obj);
        }
        public DataTable TongHop01(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/TongHop01", obj);
        }
        public DataTable TongHop02(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportBTC/TongHop02", obj);
        }
        #endregion


       
    }
}
