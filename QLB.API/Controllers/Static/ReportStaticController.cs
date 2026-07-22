using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Jwt.Filters;

namespace QLB.API.Controllers.Static
{
    public class ReportStaticController : ApiController
    {
        ReportStaticRepository ReportStaticRepository = null;

        public ReportStaticController()
        {
            if (ReportStaticRepository == null)
            {
                ReportStaticRepository = new ReportStaticRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageReportStatic(int page_size, int page_index, string where)
        {
            return ReportStaticRepository.GetPageReportStatic(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageReportStaticExport(string where)
        {
            return ReportStaticRepository.GetPageReportStaticExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllReportStatic()
        {
            return ReportStaticRepository.GetAllReportStatic();
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetAllReportNameStatic()
        {
            return ReportStaticRepository.GetAllReportNameStatic();
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdReportStatic(int ID)
        {

            return ReportStaticRepository.GetByIdReportStatic(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateReportStatic(ReportStatic oREPORT)
        {
            return ReportStaticRepository.CreateReportStatic(oREPORT);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateReportStatic(ReportStatic oREPORT)
        {
            return ReportStaticRepository.UpdateReportStatic(oREPORT);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteReportStatic(int ID)
        {
            return ReportStaticRepository.DeleteReportStatic(ID);
        }
    }
}
