using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;


namespace QLB.API.Controllers
{
    public class AirlinesController : ApiController
    {
        AirlinesRepository AirlinesRepository = null;

        public AirlinesController()
        {
            if (AirlinesRepository == null)
            {
                AirlinesRepository = new AirlinesRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageAirlines(int page_size, int page_index, string where)
        {
            return AirlinesRepository.GetPageAirlines(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetPageAirlinesExport(string where)
        {
            return AirlinesRepository.GetPageAirlinesExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllAirlines()
        {
            return AirlinesRepository.GetAllAirlines();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdAirlines(int Id)
        {
            return AirlinesRepository.GetByIdAirlines(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateAirlines(Airlines oAIRLINES)
        {
            return AirlinesRepository.CreateAirlines(oAIRLINES);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateAirlines(Airlines oAIRLINES)
        {
            return AirlinesRepository.UpdateAirlines(oAIRLINES);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteAirlines(int Id)
        {
            return AirlinesRepository.DeleteAirlines(Id);
        }
    }
}
