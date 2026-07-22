using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.Info.Cant;
using QLB.API.Models;
using QLB.BusinessLogic.Cant;

namespace QLB.API.Data.Cant
{
    public class CantSplitRepository
    {
        public ReponseEntity GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetPage<CantSplit>(new CantSplitDAL().GetPage(page_size, page_index, where, FromDate, ToDate));
        }
        public ReponseEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<CantSplit>(new CantSplitDAL().GetById(Id));
        }

        public ReponseReportEntity GetByNBR(string Nbr)
        {
            return new ReponseEntityHelper().GetTable(new CantSplitDAL().GetContentByNBR(Nbr));
        }
    }
}