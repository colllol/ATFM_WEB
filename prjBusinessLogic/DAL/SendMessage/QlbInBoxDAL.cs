using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjBusinessLogic
{
    public class QlbInBoxDAL
    {
        public bool Insert(object obj)
        {
            var ax = Int64.Parse(new clsResuftAPI().GetPostValueApiExtension("MESSAGE_PKG", "SP_INSERT_QLBINBOX", obj).ToString());
            return ax == -1 ? false : true;
        }
    }
}
