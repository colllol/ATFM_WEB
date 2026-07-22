using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class RouteListController : ApiController
    {
        RouteListRepository RouteListRepository = null;

        public RouteListController()
        {
            if (RouteListRepository == null)
            {
                RouteListRepository = new RouteListRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageRouteList(int page_size, int page_index, string where)
        {
            return RouteListRepository.GetPageRouteList(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageRouteListExport(string where)
        {
            return RouteListRepository.GetPageRouteListExport(where);
        }


        [AcceptVerbs("Get")]
        public ReponseEntity GetAllRouteList()
        {
            return RouteListRepository.GetAllRouteList();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdRouteList(int Id)
        {
            return RouteListRepository.GetByIdRouteList(Id);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdRoutePoint(int Id)
        {
            return RouteListRepository.GetByIdRoutePoint(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateRouteList(RouteList oROUTELIST)
        {
            return RouteListRepository.CreateRouteList(oROUTELIST);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateRouteList(RouteList oROUTELIST)
        {
            return RouteListRepository.UpdateRouteList(oROUTELIST);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteRouteList(int Id)
        {
            return RouteListRepository.DeleteRouteList(Id);
        }
    }
}
