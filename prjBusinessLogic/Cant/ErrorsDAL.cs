using System;
using System.Collections.Generic;
using prjInfo;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjBusinessLogic.Cant
{
    public class ErrorsDAL
    {
        public List<Errors> GetPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Errors>().GetListObj("api/Errors/GetPageErrors", pageSize, pageIndex, where);
        }
        //public Errors GetById(string id)
        //{
        //    return new clsResuftAPI<Errors>().GetOneObj(id, "api/Errors/GetById/");
        //}
    }
}
