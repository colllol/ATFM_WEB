using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class QlbOutBox
    {
        public Int64 OT_ID { get; set; }
        public Int64 NT_ID { get; set; }
        public DateTime? OT_GTIME { get; set; }
        public DateTime? OT_STIME { get; set; }
        public string OT_SENT { get; set; }
        public Int64 OT_NUM { get; set; }
        public string OT_CONT { get; set; }
    }
}
