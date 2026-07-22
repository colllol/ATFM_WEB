using QLB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using QLB.API.Data;
using System.Web.Http;
using QLB.API.Data.Perm;
using System.Data;

namespace QLB.API.Controllers.Perm
{
    public class PermHistoryController : ApiController
    {

        PermHistoryRepository PermHistoryRepository = null;
        
        public PermHistoryController()
        {
            if (PermHistoryRepository == null)
            {
                PermHistoryRepository = new PermHistoryRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdPermMasterNoBk (Int64 Id)
        {
            return PermHistoryRepository.GetByIdPermMasterNoBk(Id);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdPermMasterScBk (Int64 Id)
        {
            return PermHistoryRepository.GetByIdPermMasterScBk(Id);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetbyIdPermDetailNoBk(Int64 Id)
        {
            return PermHistoryRepository.GetByIdPermDetailNoBk(Id);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdPermDetailScBk (Int64 Id)
        {
            return PermHistoryRepository.GetByIdPermDetailScBk(Id);
        }
    }
}
