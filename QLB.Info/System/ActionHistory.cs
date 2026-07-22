using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info.System
{
    public class ActionHistory
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string FullName { get; set; }
        public string HostIP { get; set; }
        public DateTime DateModify { get; set; }
        public string ActionsCode { get; set; }
        public Int64 News_ID { get; set; }
        public string Notes { get; set; }
        public Int64 Menu_ID { get; set; }
    }
}
