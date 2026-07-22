using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data.Receive_LogFile
{
    public class ReceiveLogFileRepository
    {
        public ReponseEntity GetPage(clsSearchReceiveLogFile obj)
        {
            return new ReponseEntityHelper().GetPage<ReceiveLogFile>(new ReceiveLogFileDAL().GetPage(obj));
        }
        public ReponseEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<ReceiveLogFile>(new ReceiveLogFileDAL().GetById(Id));
        }
    }
}