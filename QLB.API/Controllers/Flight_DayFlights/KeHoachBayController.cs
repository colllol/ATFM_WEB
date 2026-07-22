using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;
using System.Net.Http;
using QLB.BusinessLogic;
using System.Data;

namespace QLB.API.Controllers.Flight_DayFlights
{
    public class KeHoachBayController : ApiController
    {
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetKeHoachBay(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachBayNgay(obj);
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetKeHoachBayFull(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachBayNgayFull(obj);
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetKeHoachBayOrigin(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachBayNgayOrigin(obj);
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetKeHoachBayTrungLap(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachBayNgayTrungLap(obj);
        }
        [AcceptVerbs("Put", "Post","Get")]
        public ReponseEntity AccessKeHoachBayNgay(string user, DateTime date)
        {
            return new DayFlightsRepository().AccessKeHoachBayNgay(user, date);
        }

        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseEntity AccessKeHoachBayNgayNew(string user, DateTime date)
        {
            return new DayFlightsRepository().AccessKeHoachBayNgayNew(user, date);
        }

        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseEntity KeHoachBayToMessage(DateTime date)
        {
            return new DayFlightsRepository().KeHoachBayToMessage(date);
        }

        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseEntity KeHoachBayToMessageExt(DateTime date)
        {
            return new DayFlightsRepository().KeHoachBayToMessageExt(date);
        }

        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseEntity KeHoachBayToMessageByAirport(DateTime date)
        {
            return new DayFlightsRepository().KeHoachBayToMessageByAirport(date);
        }
        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseReportEntity KeHoachBayDelete(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachBayNgayDelete(obj);
        }
        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseReportEntity KeHoachBayVenhTheoLichMua(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachVenhTheoMua(obj);
        }
        [AcceptVerbs("Put", "Post", "Get")]
        public ReponseReportEntity KeHoachBayVenhTheoLichThang(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().LayKeHoachVenhTheoThang(obj);
        }
    }
}
