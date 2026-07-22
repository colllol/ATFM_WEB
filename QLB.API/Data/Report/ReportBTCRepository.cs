using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class ReportBTCRepository
    {
        #region ReportBTC_QuaCanh
        public ReponseEntity QC01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC01(obj));
        }
        public ReponseEntity QC02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC02(obj));
        }
        public ReponseEntity QC03(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC03(obj));
        }
        public ReponseEntity QC04(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC04(obj));
        }
        public ReponseEntity QC05(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC05(obj));
        }
        public ReponseEntity QC06(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC06(obj));
        }
        public ReponseEntity QC07(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC07(obj));
        }
        public ReponseEntity QC08(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC08(obj));
        }
        public ReponseEntity QC09(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC09(obj));
        }
        public ReponseEntity QC10(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC10(obj));
        }
        public ReponseEntity QC11(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC11(obj));
        }
        public ReponseEntity QC12(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC12(obj));
        }
        public ReponseEntity QC13(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC13(obj));
        }
        public ReponseEntity QC14(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC14(obj));
        }
        public ReponseEntity QC15(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC15(obj));
        }
        public ReponseEntity QC16(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC16(obj));
        }
        public ReponseEntity QC17(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC17(obj));
        }
        public ReponseEntity QC18(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC18(obj));
        }
        public ReponseEntity QC19(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QC19(obj));
        }        
        #endregion
        #region ReportBTC_QuocNoi
        public ReponseEntity QN01 (clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN01(obj));
        }
        public ReponseEntity QN02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN02(obj));
        }
        public ReponseEntity QN03(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN03(obj));
        }
        public ReponseEntity QN04(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN04(obj));
        }
        public ReponseEntity QN05(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN05(obj));
        }
        public ReponseEntity QN06(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN06(obj));
        }
        public ReponseEntity QN07(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN07(obj));
        }
        public ReponseEntity QN08(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN08(obj));
        }
        public ReponseEntity QN09(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().QN09(obj));
        }
        #endregion
        #region ReportBTC_QuocTeDiDen
        public ReponseEntity BCMienThu01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().BCMienThu01(obj));
        }
        public ReponseEntity BCMienThu02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().BCMienThu02(obj));
        }
        public ReponseEntity BCThuTMTS01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().BCThuTMTS01(obj));
        }
        public ReponseEntity BCThuTMTS02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().BCThuTMTS02(obj));
        }
        public ReponseEntity CTMThu01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().CTMThu01(obj));
        }
        public ReponseEntity CTMThu02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().CTMThu02(obj));
        }
        public ReponseEntity CTTSan01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().CTTSan01(obj));
        }
        public ReponseEntity CTTSan02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().CTTSan02(obj));
        }
        public ReponseEntity CTTsau01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().CTTsau01(obj));
        }
        public ReponseEntity CTTsau02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().CTTsau02(obj));
        }
        public ReponseEntity ListOffFlights01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().ListOffFlights01(obj));
        }
        public ReponseEntity ListOffFLights02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().ListOffFLights02(obj));
        }
        public ReponseEntity TongHop01(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().TongHop01(obj));
        }
        public ReponseEntity TongHop02(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportBTCDAL().TongHop02(obj));
        }
        #endregion
    }
}