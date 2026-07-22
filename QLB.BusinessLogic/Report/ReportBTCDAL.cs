using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class ReportBTCDAL
    {
        #region ReportBTC_QuaCanh
        public DataTable QC01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC01", obj).Tables[0];
        }
        public DataTable QC02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC02", obj).Tables[0];
        }
        public DataTable QC03(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC03", obj).Tables[0];
        }
        public DataTable QC04(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC04", obj).Tables[0];
        }
        public DataTable QC05(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC05", obj).Tables[0];
        }
        public DataTable QC06(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC06", obj).Tables[0];
        }
        public DataTable QC07(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC07", obj).Tables[0];
        }
        public DataTable QC08(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC08", obj).Tables[0];
        }
        public DataTable QC09(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC09", obj).Tables[0];
        }
        public DataTable QC10(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC10", obj).Tables[0];
        }
        public DataTable QC11(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC11", obj).Tables[0];
        }
        public DataTable QC12(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC12", obj).Tables[0];
        }
        public DataTable QC13(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC13", obj).Tables[0];
        }
        public DataTable QC14(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC14", obj).Tables[0];
        }
        public DataTable QC15(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC15", obj).Tables[0];
        }
        public DataTable QC16(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC16", obj).Tables[0];
        }
        public DataTable QC17(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC17", obj).Tables[0];
        }
        public DataTable QC18(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC18", obj).Tables[0];
        }
        public DataTable QC19(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QC19", obj).Tables[0];
        }

        #endregion
        #region ReportBTC_QuocNoi
        public DataTable QN01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN01", obj).Tables[0];
        }
        public DataTable QN02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN02", obj).Tables[0];
        }
        public DataTable QN03(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN03", obj).Tables[0];
        }
        public DataTable QN04(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN04", obj).Tables[0];
        }
        public DataTable QN05(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN05", obj).Tables[0];
        }
        public DataTable QN06(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN06", obj).Tables[0];
        }
        public DataTable QN07(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN07", obj).Tables[0];
        }
        public DataTable QN08(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN08", obj).Tables[0];
        }
        public DataTable QN09(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "QN09", obj).Tables[0];
        }
        
        #endregion
        #region ReportBTC_QuocTeDiDen
        public DataTable BCMienThu01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCMienThu01", obj).Tables[0];
        }
        public DataTable BCMienThu02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCMienThu02", obj).Tables[0];
        }
        public DataTable BCThuTMTS01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCThuTMTS01", obj).Tables[0];
        }
        public DataTable BCThuTMTS02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCThuTMTS02", obj).Tables[0];
        }
        public DataTable CTMThu01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "CTMThu01", obj).Tables[0];
        }
        public DataTable CTMThu02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "CTMThu02", obj).Tables[0];
        }
        public DataTable CTTSan01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "CTTSan01", obj).Tables[0];
        }
        public DataTable CTTSan02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "CTTSan02", obj).Tables[0];
        }
        public DataTable CTTsau01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "CTTsau01", obj).Tables[0];
        }
        public DataTable CTTsau02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "CTTsau02", obj).Tables[0];
        }
        public DataTable ListOffFlights01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "ListOffFlights01", obj).Tables[0];
        }
        public DataTable ListOffFLights02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "ListOffFLights02", obj).Tables[0];
        }
        public DataTable TongHop01(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "TongHop01", obj).Tables[0];
        }
        public DataTable TongHop02(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "TongHop02", obj).Tables[0];
        }
        #endregion
    }
}
