using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
  public  class UserGroups
    {
        public int ID { get; set; }
        public int USER_ID { get; set; }
        public int GROUP_ID { get; set; }
        public string MSREPL_TRAN_VERSION { get; set; }
        public string ROWGUID { get; set; }
    }
}
