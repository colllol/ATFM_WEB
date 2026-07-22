using QLB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using QLB.API.Data;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class ReportBTCController : ApiController
    {

        #region ReportBTC_QuaCanh

        [AcceptVerbs("Put")]
        public ReponseEntity QC01 (clsSearchValue obj)
        {
            return new ReportBTCRepository().QC01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC02(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC02(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC03(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC03(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC04(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC04(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC05(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC05(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC06(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC06(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC07(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC07(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC08(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC08(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC09(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC09(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC10(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC10(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC11(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC11(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC12(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC12(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC13(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC13(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC14(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC14(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC15(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC15(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC16(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC16(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC17(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC17(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC18(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC18(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity QC19(clsSearchValue obj)
        {
            return new ReportBTCRepository().QC19(obj);
        }        
        #endregion
        #region ReportBTC_QuocNoi

        [AcceptVerbs("Put")]
        public ReponseEntity QN01 (clsSearchValue obj)
        {
            return new ReportBTCRepository().QN01(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN02(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN02(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN03(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN03(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN04(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN04(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN05(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN05(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN06(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN06(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN07(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN07(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN08(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN08(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity QN09(clsSearchValue obj)
        {
            return new ReportBTCRepository().QN09(obj);
        }
        #endregion
        #region ReportBTC_QuocTeDiDen

        [AcceptVerbs("Put")]
        public ReponseEntity BCMienThu01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity BCMienThu02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity BCThuTMTS01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity BCThuTMTS02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity CTMThu01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity CTMThu02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity CTTSan01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity CTTSan02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity CTTsau01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity CTTsau02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity ListOffFlights01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity ListOffFLights02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity TongHop01(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity TongHop02(clsSearchValue obj)
        {
            return new ReportBTCRepository().BCMienThu01(obj);
        }
        #endregion
    }
}
