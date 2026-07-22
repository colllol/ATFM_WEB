using System.Web.Http;
using QLB.API.Data.System;
using QLB.API.Models;
using QLB.Info.System;

namespace QLB.API.Controllers.System
{
    public class ActionHistoryController : ApiController
    {
        ActionHistoryRepository ActionHistoryRepository = null;

        public ActionHistoryController()
        {
            if (ActionHistoryRepository == null)
            {
                ActionHistoryRepository = new ActionHistoryRepository();
            }
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate, string FullName)
        {
            return ActionHistoryRepository.GetPage(page_size, page_index, where, Fromdate, Todate, FullName);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageActionHistory(int page_size, int page_index, string where)
        {
            return ActionHistoryRepository.GetPageActionHistory(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllActionHistory()
        {
            return ActionHistoryRepository.GetAllActionHistory();
        }
         [AcceptVerbs("Post")]
        public ReponseEntity CreateActionHistory(ActionHistory oACTIONHISTORY)
        {
            return ActionHistoryRepository.CreateActionHistory(oACTIONHISTORY);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateActionHistory(ActionHistory oACTIONHISTORY)
        {
            return ActionHistoryRepository.UpdateActionHistory(oACTIONHISTORY);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteActionHistory(int ID)
        {
            return ActionHistoryRepository.DeleteActionHistory(ID);
        }
    }
}
