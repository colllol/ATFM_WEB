using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Jwt.Filters;
using System.Web.Http.Cors;

namespace QLB.API.Controllers
{
    public class AtsController : ApiController
    {
        AtsRepository AtsRepository = null;
        const string _App01 = "*";
        public AtsController()
        {
            if (AtsRepository == null)
            {
                AtsRepository = new AtsRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageAtsRoute(int page_size, int page_index, string where)
        {
            return AtsRepository.GetPageAtsRoute(page_size, page_index, where);
        }

        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAtsRoute(int ID)
        {
            return AtsRepository.DeleteAtsRoute(ID);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageAtsFir(int page_size, int page_index, string where)
        {
            return AtsRepository.GetPageAtsFir(page_size, page_index, where);
        }

        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAtsFir(int ID)
        {
            return AtsRepository.DeleteAtsFir(ID);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageAtsWay(int page_size, int page_index, string where)
        {
            return AtsRepository.GetPageAtsWay(page_size, page_index, where);
        }

        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAtsWay(int ID)
        {
            return AtsRepository.DeleteAtsWay(ID);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageAtsAreo(int page_size, int page_index, string where)
        {
            return AtsRepository.GetPageAtsAreo(page_size, page_index, where);
        }

        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAtsAreo(int ID)
        {
            return AtsRepository.DeleteAtsAreo(ID);
        }
    }
}