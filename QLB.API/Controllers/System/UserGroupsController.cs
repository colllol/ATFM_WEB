using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System.Web.Http;

namespace QLB.API.Controllers
{
    public class UserGroupsController : ApiController
    {
        UserGroupsRepository UserGroupsRepository = null;

        public UserGroupsController()
        {
            if (UserGroupsRepository == null)
            {
                UserGroupsRepository = new UserGroupsRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageUserGroups(int page_size, int page_index)
        {
            return UserGroupsRepository.GetPageUserGroups(page_size, page_index);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllUserGroups()
        {
            return UserGroupsRepository.GetAllUserGroups();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdUserGroups(int ID)
        {
            return UserGroupsRepository.GetByIdUserGroups(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateUserGroups(UserGroups oUSERGROUPS)
        {
            return UserGroupsRepository.CreateUserGroups(oUSERGROUPS);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateUserGroups(UserGroups oUSERGROUPS)
        {
            return UserGroupsRepository.UpdateUserGroups(oUSERGROUPS);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteUserGroups(int ID)
        {
            return UserGroupsRepository.DeleteUserGroups(ID);
        }
    }
}