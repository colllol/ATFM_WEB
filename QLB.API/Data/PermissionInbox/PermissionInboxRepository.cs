using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class PermissionInboxRepository
    {
        public ReponseEntity GetPage(clsSearchPermissionInbox obj)
        {
            return new ReponseEntityHelper().GetPage<PermissionInbox>(new PermissionInboxDAL().GetPage(obj));            
        }

        public ReponseEntity GetByID(string Id)
        {
            return new ReponseEntityHelper().GetByID<PermissionInbox>(new PermissionInboxDAL().GetByNBR(Id));
        }
    }
}