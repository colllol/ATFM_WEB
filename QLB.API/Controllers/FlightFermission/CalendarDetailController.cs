using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Web.Http.Results;
using System;

namespace QLB.API.Controllers
{
    public class CalendarDetailController : ApiController
    {
        CalendarDetailRepository CalendarDetailRepository = null;

        public CalendarDetailController()
        {
            if (CalendarDetailRepository == null)
            {
                CalendarDetailRepository = new CalendarDetailRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageCalendarDetail(int page_size, int page_index, string where)
        {
            return CalendarDetailRepository.GetPageCalendarDetail(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetCountCalendarDetail(string where)
        {
            return CalendarDetailRepository.GetCountCalendarDetail(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllCalendarDetail()
        {
            return CalendarDetailRepository.GetAllCalendarDetail();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdCalendarDetail(int Id)
        {
            return CalendarDetailRepository.GetByIdCalendarDetail(Id);
        }
        [AcceptVerbs("Post")]
        [HttpPost]
        public ReponseEntity CreateCalendarDetail([FromBody]CalendarDetail oCALENDARDETAIL)
        {            
            var axxxx = JsonConvert.DeserializeObject<CalendarDetail>(Request.Content.ReadAsStringAsync().Result);
            var json = Request.Content.ReadAsStreamAsync().Result;            
            Stream stream = new MemoryStream();
            json.Seek(0, SeekOrigin.Begin);
            StreamReader rd = new StreamReader(json);
            var ax = rd.ReadToEnd();          
            oCALENDARDETAIL = JsonConvert.DeserializeObject<CalendarDetail>(ax);            
            return CalendarDetailRepository.CreateCalendarDetail(oCALENDARDETAIL);
        }        
        
        [AcceptVerbs("Put")]
        public ReponseEntity UpdateCalendarDetail(CalendarDetail oCALENDARDETAIL)
        {
            return CalendarDetailRepository.UpdateCalendarDetail(oCALENDARDETAIL);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteCalendarDetail(Int64 Id)
        {
            return CalendarDetailRepository.DeleteCalendarDetail(Id);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetHistoryById(string id)
        {
            return new CalendarDetailRepository().GetHis(id);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity RestoreHis(Int64 id, int version, string iduser)
        {
            return new CalendarDetailRepository().RestoreHis(id, iduser, version);
        }

    }
}
