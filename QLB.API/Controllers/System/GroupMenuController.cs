using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System.Web.Http;

namespace QLB.API.Controllers
{
    public class GroupMenuController : ApiController
    {
        GroupMenuRepository GroupMenuRepository = null;

        public GroupMenuController()
        {
            if (GroupMenuRepository == null)
            {
                GroupMenuRepository = new GroupMenuRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGroupMenu(int page_size, int page_index)
        {
            return GroupMenuRepository.GetPageGroupMenu(page_size, page_index);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllGroupMenu()
        {
            return GroupMenuRepository.GetAllGroupMenu();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdGroupMenu(int GROUPMENU_ID)
        {
            return GroupMenuRepository.GetByIdGroupMenu(GROUPMENU_ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateGroupMenu(GroupMenu oGROUPMENU)
        {
            return GroupMenuRepository.CreateGroupMenu(oGROUPMENU);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateGroupMenu(GroupMenu oGROUPMENU)
        {
            return GroupMenuRepository.UpdateGroupMenu(oGROUPMENU);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteGroupMenu(int GROUPMENU_ID)
        {
            return GroupMenuRepository.DeleteGroupMenu(GROUPMENU_ID);
        }
    }
}