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

namespace QLB.API.Controllers
{
    public class DocumentController : ApiController
    {
        DocumentRepository DocumentRepository = null;

        public DocumentController()
        {
            if (DocumentRepository == null)
            {
                DocumentRepository = new DocumentRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageDocument(int page_size, int page_index, string where)
        {
            return DocumentRepository.GetPageDocument(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageDocumentExport(string where)
        {
            return DocumentRepository.GetPageDocumentExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllDocument()
        {
            return DocumentRepository.GetAllDocument();
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdDocument(int ID)
        {

            return DocumentRepository.GetByIdDocument(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateDocument(Document oDocument)
        {
            return DocumentRepository.CreateDocument(oDocument);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateDocument(Document oDocument)
        {
            return DocumentRepository.UpdateDocument(oDocument);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteDocument(int ID)
        {
            return DocumentRepository.DeleteDocument(ID);
        }

    }
}