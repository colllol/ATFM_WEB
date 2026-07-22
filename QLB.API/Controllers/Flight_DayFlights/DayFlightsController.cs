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
    public class DayFlightsController : ApiController
    {
        const string _App01 = "*";
        #region funtion dayflight default
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new DayFlightsRepository().GetPage(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageTrungLap(int page_size, int page_index, string where)
        {
            return new DayFlightsRepository().GetPageTrungLap(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity GetAll()
        {
            return new DayFlightsRepository().GetAll();
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity GetById(Int64 Id)
        {
            return new DayFlightsRepository().GetByID(Id);
        }
        [AcceptVerbs("Post","Put")]
        public ReponseEntity Insert(DayFlights obj)
        {
            //return new DayFlightsRepository().Insert(obj);
            //var axxxx = JsonConvert.DeserializeObject<DayFlights>(Request.Content.ReadAsStringAsync().Result);
            //var json = Request.Content.ReadAsStreamAsync().Result;
            //Stream stream = new MemoryStream();
            //json.Seek(0, SeekOrigin.Begin);
            //StreamReader rd = new StreamReader(json);
            //var ax = rd.ReadToEnd();
            //obj = JsonConvert.DeserializeObject<DayFlights>(ax, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddThh:mm:ss" });
            return new DayFlightsRepository().Insert(obj);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseEntity InsertManual(DayFlights obj)
        {
            return new DayFlightsRepository().Insert_Manual(obj);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseEntity InsertManual_GoingOn(DayFlights_GoingOn obj)
        {
            return new DayFlightsRepository().Insert_Manual_GoingOn(obj);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseEntity UpdateManual_GoingOn(DayFlights_GoingOn obj)
        {
            return new DayFlightsRepository().Update_Manual_GoingOn(obj);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseEntity UpdateManual_GoingOn_DB(DayFlights_GoingOn obj)
        {
            return new DayFlightsRepository().Update_Manual_GoingOn_DB(obj);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseEntity Update(DayFlights obj)
        {
            return new DayFlightsRepository().Update(obj);
        }
        [AcceptVerbs("Delete")]
        [EnableCors(_App01, "*", "*")]
        public ReponseEntity Delete(Int64 Id)
        {
            return new DayFlightsRepository().Delete(Id);
        }


        [AcceptVerbs("Delete")]
        [EnableCors(_App01, "*", "*")]
        public ReponseEntity WaitDelete(Int64 Id, Int64 Status)
        {
            return new DayFlightsRepository().WaitDelete(Id, Status);
        }



        [AcceptVerbs("Delete")]
        [EnableCors(_App01, "*", "*")]
        public ReponseEntity MoveDate(Int64 Id)
        {
            return new DayFlightsRepository().MoveDate(Id);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseEntity MoveDate(DayFlights obj)
        {
            return new DayFlightsRepository().Update(obj);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetHistoryById(Int64 id)
        {
            return new DayFlightsRepository().GetHistoryByID(id);
        }
        [AcceptVerbs("Get")]
        [EnableCors(_App01, "*", "*")]
        public ReponseEntity RestoreHis(Int64 id, int version, string iduser)
        {
            return new DayFlightsRepository().RestoreHis(id, version, iduser);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetRecordDeleted(string where)
        {
            return new DayFlightsRepository().GetRecordDeleted(where);
        }
        #endregion


        #region funtion for dien bien hoat dong bay
        [AcceptVerbs("Get")]
        public ReponseEntity GetDaylyFlightInfo()
        {
            return new DayFlightsRepository().GetDaylyFlightInfo();
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetDaylyFlightBySearch(clsDayFlightValueSearch obj)
        {            
            return new DayFlightsRepository().GetDayFlightByValueSearch(obj);
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetDaylyFlightBySearch_New(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().GetDayFlightByValueSearch_New(obj);
        }


        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetDaylyFlightNgayNTru(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().GetDayFlightByNgayNtru1(obj);
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetDaylyFlightNgayNCong(clsDayFlightValueSearch obj)
        {
            return new DayFlightsRepository().GetDayFlightByNgayNCong1(obj);
        }

        [AcceptVerbs("GET")]
        public ReponseReportEntity GetPermBy(Int64 id)
        {
            return new DayFlightsRepository().GetPermById(id);
        }

        [AcceptVerbs("GET")]
        public ReponseReportEntity GetPerm_GoingOnBy(Int64 id)
        {
            return new DayFlightsRepository().GetPerm_GoingOnById(id);
        }


        [AcceptVerbs("GET")]
        public ReponseReportEntity GetLinkFile(Int64 id, string permtype)
        {
            return new DayFlightsRepository().GetLinkFile(id, permtype);
        }
        #endregion
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetDaylyFlightBySearch_HCM(clsDayFlightValueSearch obj)
        {

            return new DayFlightsRepository().GetDayFlightByValueSearch_HCM(obj);
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetDaylyFlightBySearch_DNG(clsDayFlightValueSearch obj)
        {

            return new DayFlightsRepository().GetDayFlightByValueSearch_DNG(obj);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetDaylyFlightInfo_Hcm(string dsSanBayHcm)
        {
            return new DayFlightsRepository().GetDaylyFlightInfo_Hcm(dsSanBayHcm);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetDaylyFlightInfo_Dng(string dsSanBayDng)
        {
            return new DayFlightsRepository().GetDaylyFlightInfo_Dng(dsSanBayDng);
        }

        #region unkown
        [AcceptVerbs("Put")]
        public DataTable GetDaylyFlightBy(clsDayFlightValueSearch obj)
        {
            return new DayFlightsDAL().GetAllForValueSearch(obj);
        }
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetMessage(DateTime date, string callSign)
        {
            return new DayFlightsRepository().GetMessage(date, callSign);
        }
        #endregion

        #region flight change
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetListFlightChange(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetFlightChangeBySearch(obj));
        }
        #endregion
        #region flight not perm
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetListFlightNotPerm(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetFlightNotPermBySearch(obj));
        }
        #endregion

        #region flight atd not null
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetFlightAtdNotNull(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetFlightAtdNotNullBySearch(obj));
        }
        #endregion


        #region flight not route
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetListFlightNotRoute(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetFlightNotRouteBySearch(obj));
        }
        #endregion
        #region flight by search
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetListFlightBySearch(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetFlightBySearch(obj));
        }
        #endregion
        #region Calendar draft
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageDraft(int page_size, int page_index, string where)
        {
            return new DayFlightsRepository().GetPageDraft(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageDraftTrungLap(int page_size, int page_index, string where)
        {
            return new DayFlightsRepository().GetPageDraftTrungLap(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdDraft(Int64 id)
        {
            return new DayFlightsRepository().GetByIdDraft(id);
        }
        [AcceptVerbs("Post", "Put")]
        public ReponseEntity InsertDraft(DayFlights obj)
        {
            return new DayFlightsRepository().InsertDraft(obj);
        }
        [AcceptVerbs("Post", "Put")]
        public ReponseEntity UpdateDraft(DayFlights obj)
        {
            return new DayFlightsRepository().UpdateDraft(obj);
        }
        
        #endregion
    }
}
