using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using QLB.BusinessLogic.Perm;

namespace QLB.API.Data.Perm
{
    public class RestorePermRepository
    {
        public bool RestorePermMasterNo(Int64 Id, int Version,int UserId)
        {
            return new RestorePermDAL().RestorePermMasterNo(Id, Version, UserId);
        }
        public bool RestorePermMasterSc(Int64 Id, int Version, int UserId)
        {
            return new RestorePermDAL().RestorePermMasterSc(Id, Version, UserId);
        }
        public bool RestorePermDetailNo (Int64 Id, int Version, int UserId)
        {
            return new RestorePermDAL().RestorePermDetailNo(Id, Version, UserId);
        }
        public bool RestorePermDetailSc (Int64 Id, int Version, int UserId)
        {
            return new RestorePermDAL().RestorePermDetailSc(Id, Version, UserId);
        }
    }
}