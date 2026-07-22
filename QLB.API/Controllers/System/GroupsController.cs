using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System.Web.Http;

namespace QLB.API.Controllers
{
    public class GroupsController : ApiController
    {
        GroupsRepository GroupsRepository = null;

        public GroupsController()
        {
            if (GroupsRepository == null)
            {
                GroupsRepository = new GroupsRepository();
            }
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity BindGridMenuByGroup(int Parrent_ID, int GroupId)
        {
            return GroupsRepository.BindGridMenuByGroup(Parrent_ID, GroupId);

        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGroupExport(string where)
        {
            return GroupsRepository.GetPageGroupExport(where);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageGroups(int page_size, int page_index)
        {
            return GroupsRepository.GetPageGroups(page_size, page_index);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllGroups()
        {
            return GroupsRepository.GetAllGroups();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdGroups(int GROUP_ID)
        {
            return GroupsRepository.GetByIdGroups(GROUP_ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateGroups(Groups oGROUPS)
        {
            return GroupsRepository.CreateGroups(oGROUPS);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateGroups(Groups oGROUPS)
        {
            return GroupsRepository.UpdateGroups(oGROUPS);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteGroups(int GROUP_ID)
        {
            return GroupsRepository.DeleteGroups(GROUP_ID);
        }
    }
}