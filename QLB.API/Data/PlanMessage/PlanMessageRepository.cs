using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class PlanMessageRepository
    {
        public ReponseEntity GetPage(clsSearchPlanMessageNew obj)
        {
            return new ReponseEntityHelper().GetPage<PlanMessageNew>(new PlanMessageDAL().GetPage(obj));
        }

        public ReponseEntity GetByID(string date, int part, string mestype)
        {
            return new ReponseEntityHelper().GetByID<PlanMessageNew>(new PlanMessageDAL().GetByDatePart(date, part, mestype));
        }
        public ReponseEntity GetPageRealplanLetter(clsSearchRealplanLetter obj)
        {
            return new ReponseEntityHelper().GetPage<Realplan_Letter>(new PlanMessageDAL().GetPageRealplanLetter(obj));
        }
    }
}