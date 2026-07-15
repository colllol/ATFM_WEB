using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjBusinessLogic
{
    public interface IDAL
    {
        bool InsertObject(object obj);
        bool UpdateObject(object obj);
        bool DeleteObject(string id);
        object GetOneObject(string id);
        List<object> GetAllObject();
        List<object> GetPageObject(int pageSize, int pageIndex, string where);
        string InsertReturnId(object obj);
        System.Data.DataTable GetTableObject();
        System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where);
        System.Data.DataTable GetHistoryById(string id);
        bool RestoreRecode(string id, string version, string idUser);
        System.Data.DataTable GetTableDelete(string where);
    }
}
