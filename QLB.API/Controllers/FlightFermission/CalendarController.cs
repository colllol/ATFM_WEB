using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class CalendarController : ApiController
    {
        CalendarRepository CalendarRepository = null;

        public CalendarController()
        {
            if (CalendarRepository == null)
            {
                CalendarRepository = new CalendarRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageCalendar(int page_size, int page_index, string where)
        {
            return CalendarRepository.GetPageCalendar(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetCountCalendar(string where)
        {
            return CalendarRepository.GetCountCalendar(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllCalendar()
        {
            return CalendarRepository.GetAllCalendar();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdCalendar(int Id)
        {
            return CalendarRepository.GetByIdCalendar(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateCalendar(Calendar oCALENDAR)
        {
            //return CalendarRepository.CreateCalendar(oCALENDAR);
            return CalendarRepository.InsertObject(oCALENDAR);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateCalendar(Calendar oCALENDAR)
        {
            //return CalendarRepository.UpdateCalendar(oCALENDAR);
            return CalendarRepository.UpdateObject(oCALENDAR);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteCalendar(int Id)
        {
            return CalendarRepository.DeleteCalendar(Id);
        }
    }
}
