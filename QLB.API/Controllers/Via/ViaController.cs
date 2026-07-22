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
using QLB.BusinessLogic;
namespace QLB.API.Controllers
{
    public class ViaController : ApiController
    {
        ViaRepository ViaRepository = null;

        public ViaController()
        {
            if (ViaRepository == null)
            {
                ViaRepository = new ViaRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageVia(int page_size, int page_index, string where)
        {
            return ViaRepository.GetPageVia(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageViaExport(string where)
        {
            return ViaRepository.GetPageViaExport(where);
        }

        [AcceptVerbs("Get")]


        public ReponseEntity GetAllVia()
        {
            return ViaRepository.GetAllVia();
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdVia(int ID)
        {

            return ViaRepository.GetByIdVia(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateVia(Via oVia)
        {
            return ViaRepository.CreateVia(oVia);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateVia(Via oVia)
        {
            return ViaRepository.UpdateVia(oVia);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteVia(int ID)
        {
            return ViaRepository.DeleteVia(ID);
        }

        [AcceptVerbs("Put","POST")]
        public ReponseReportEntity GetViaBySearch(ViaSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new ViaDAL().GetBySearch(obj));
        }
        [AcceptVerbs("PUT", "POST")]
        public ReponseEntity InsertViaImport(ViaSearch obj)
        {
            return new ReponseEntityHelper().Insert<Via, ViaDAL>(obj, "InsertViaImport");
        }
        [AcceptVerbs("PUT", "POST")]
        public ReponseEntity DeleteViaImport(ViaSearch obj)
        {
            return ViaRepository.DeleteViaImport(obj);
        }


        ////////////////////////////////////////////////

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageM_VIA_ARIPORT(int page_size, int page_index, string where)
        {
            return ViaRepository.GetPageM_VIA_ARIPORT(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageM_VIA_ARIPORTExport(string where)
        {
            return ViaRepository.GetPageM_VIA_ARIPORTExport(where);
        }

       

        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdM_VIA_ARIPORT(int ID)
        {

            return ViaRepository.GetByIdM_VIA_ARIPORT(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateM_VIA_ARIPORT(M_VIA_ARIPORT oVia)
        {
            return ViaRepository.CreateM_VIA_ARIPORT(oVia);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateM_VIA_ARIPORT(M_VIA_ARIPORT oVia)
        {
            return ViaRepository.UpdateM_VIA_ARIPORT(oVia);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteM_VIA_ARIPORT(int ID)
        {
            return ViaRepository.DeleteM_VIA_ARIPORT(ID);
        }


    }
}