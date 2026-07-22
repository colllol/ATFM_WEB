using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;


namespace QLB.API.Data
{
    public class ErrorsRepository
    {
        public ReponseEntity GetPageErrors(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<Errors>(new ErrorsDAL().GetPageErrors(page_size, page_index, where));
        }
    }
}