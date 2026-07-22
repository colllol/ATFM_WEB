using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLB.API.Data.Perm;
using System.Data;
namespace QLB.API.Controllers.Perm
{
    public class RestorePermController : ApiController
    {
        RestorePermRepository RestorePermRepository = null;
        public RestorePermController()
        {
            if(RestorePermRepository == null)
            {
                RestorePermRepository = new RestorePermRepository();
            }
        }
        
        [AcceptVerbs("Get")]
        public bool RestorePermMasterNo(Int64 Id, int Version, int idUser)
        {
            return RestorePermRepository.RestorePermMasterNo(Id, Version, idUser);
        }

        [AcceptVerbs("Get")]
        public bool RestorePermMasterSc(Int64 Id, int Version, int idUser)
        {
            return RestorePermRepository.RestorePermMasterSc(Id, Version, idUser);
        }
        [AcceptVerbs("Get")]
        public bool RestorePermDetailNo(Int64 Id, int Version, int idUser)
        {
            return RestorePermRepository.RestorePermDetailNo(Id, Version, idUser);
        }
        [AcceptVerbs("Get")]
        public bool RestorePermDetailSc (Int64 Id, int Version, int idUser)
        {
            return RestorePermRepository.RestorePermDetailSc(Id, Version, idUser);
        }
    }
}
