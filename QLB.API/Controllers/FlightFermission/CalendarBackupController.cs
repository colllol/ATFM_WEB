using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Controllers
{
    public class CalendarBackupController : ApiController
    {
        CalendarBackupRepository CalendarBackupRepository = null;
        public CalendarBackupController()
        {
            if (CalendarBackupRepository == null)
            {
                CalendarBackupRepository = new CalendarBackupRepository();
            }
           
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdCalendarBackup(int Id)
        {
            return CalendarBackupRepository.GetByIdCalendarBackup(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateCalendarBackup(CalendarBackup oCALENDARBACKUP)
        {
            return CalendarBackupRepository.CreateCalendarBackup(oCALENDARBACKUP);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateCalendarBackup(CalendarBackup oCALENDARBACKUP)
        {
            return CalendarBackupRepository.UpdateCalendarBackup(oCALENDARBACKUP);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteCalendarBackup(int ID)
        {
            return CalendarBackupRepository.DeleteCalendarBackup(ID);
        }
    }
}
