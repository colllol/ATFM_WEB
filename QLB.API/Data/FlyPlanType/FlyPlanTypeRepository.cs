using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data.FlyPlanType
{
    public class FlyPlanTypeRepository
    {
        public ReponseEntity GetPage(clsSearchFlyplanType obj)
        {
            return new ReponseEntityHelper().GetPage<FlyplanType>(new FlyplanTypeDAL().GetPage(obj));
        }

        public ReponseEntity GetByID(string Id)
        {
            return new ReponseEntityHelper().GetByID<FlyplanType>(new FlyplanTypeDAL().GetByNBR(Id));
        }
    }
}