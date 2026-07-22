using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System.Web.Http;

namespace QLB.API.Controllers
{
    public class UserMenuController : ApiController
    {
        UserMenuRepository UserMenuRepository = null;

        public UserMenuController()
        {
            if (UserMenuRepository == null)
            {
                UserMenuRepository = new UserMenuRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageUserMenu(int page_size, int page_index)
        {
            return UserMenuRepository.GetPageUserMenu(page_size, page_index);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllUserMenu()
        {
            return UserMenuRepository.GetAllUserMenu();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdUserMenu(int ID)
        {
            return UserMenuRepository.GetByIdUserMenu(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateUserMenu(UserMenu oUSERMENU)
        {
            return UserMenuRepository.CreateUserMenu(oUSERMENU);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateUserMenu(UserMenu oUSERMENU)
        {
            return UserMenuRepository.UpdateUserMenu(oUSERMENU);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteUserMenu(int ID)
        {
            return UserMenuRepository.DeleteUserMenu(ID);
        }
    }
}