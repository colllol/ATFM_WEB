using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class AlarmController : ApiController
    {
        AlarmRepository AlarmRepository = null;

        public AlarmController()
        {
            if (AlarmRepository == null)
            {
                AlarmRepository = new AlarmRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageAlarm(int page_size, int page_index, string where)
        {
            return AlarmRepository.GetPageAlarm(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageAlarmExport(string where)
        {
            return AlarmRepository.GetPageAlarmExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllAlarm()
        {
            return AlarmRepository.GetAllAlarm();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdAlarm(int Id)
        {
            return AlarmRepository.GetByIdAlarm(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateAlarm(Alarm oAlarm)
        {
            return AlarmRepository.CreateAlarm(oAlarm);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateAlarm(Alarm oAlarm)
        {
            return AlarmRepository.UpdateAlarm(oAlarm);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAlarm(int Id)
        {
            return AlarmRepository.DeleteAlarm(Id);
        }

        
        [AcceptVerbs("Get")]
        public ReponseReportEntity GetCountAlarmByDate()
        {
            return new AlarmRepository().GetCountAlarmByDate();
        }
    }
}