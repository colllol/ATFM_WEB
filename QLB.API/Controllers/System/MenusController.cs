using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System.Web.Http;

namespace QLB.API.Controllers
{
    public class MenusController : ApiController
    {
        MenusRepository MenusRepository = null;

        public MenusController()
        {
            if (MenusRepository == null)
            {
                MenusRepository = new MenusRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageMenus(int page_size, int page_index, string where)
        {
            return MenusRepository.GetPageMenus(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllMenus()
        {
            return MenusRepository.GetAllMenus();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdMenus(int ID)
        {
            return MenusRepository.GetByIdMenus(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateMenus(Menus oMENUS)
        {
            return MenusRepository.CreateMenus(oMENUS);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateMenus(Menus oMENUS)
        {
            return MenusRepository.UpdateMenus(oMENUS);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteMenus(int ID)
        {
            return MenusRepository.DeleteMenus(ID);
        }
    }
}