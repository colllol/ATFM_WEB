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
    public class SubdivisionController : ApiController
    {
        SubdivisionRepository SubdivisionRepository = null;

        public SubdivisionController()
        {
            if (SubdivisionRepository == null)
            {
                SubdivisionRepository = new SubdivisionRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageSubdivision(int page_size, int page_index, string where)
        {
            return SubdivisionRepository.GetPageSubdivision(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageSubdivisionExport(string where)
        {
            return SubdivisionRepository.GetPageSubdivisionExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllSubdivision()
        {
            return SubdivisionRepository.GetAllSubdivision();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdSubdivision(int Id)
        {
            return SubdivisionRepository.GetByIdSubdivision(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateSubdivision(Subdivision oSUBDIVISION)
        {
            return SubdivisionRepository.CreateSubdivision(oSUBDIVISION);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateSubdivision(Subdivision oSUBDIVISION)
        {
            return SubdivisionRepository.UpdateSubdivision(oSUBDIVISION);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteSubdivision(int Id)
        {
            return SubdivisionRepository.DeleteSubdivision(Id);
        }
    }
}
