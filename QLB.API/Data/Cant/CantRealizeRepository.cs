using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info.Cant;
using QLB.BusinessLogic.Cant;

namespace QLB.API.Data.Cant
{
    public class CantRealizeRepository
    {
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<CantRealize>(new CantRealizeDAL().GetPage(page_size, page_index, where));
        }
        public ReponseEntity GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetPage<CantRealize>(new CantRealizeDAL().GetPage(page_size, page_index, where, FromDate, ToDate));
        }
        public ReponseEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<CantRealize>(new CantRealizeDAL().GetById(Id));
        }
        public ReponseReportEntity GetByNBR(string Nbr)
        {
            return new ReponseEntityHelper().GetTable(new CantRealizeDAL().GetContentByNBR(Nbr));
        }
    }
}