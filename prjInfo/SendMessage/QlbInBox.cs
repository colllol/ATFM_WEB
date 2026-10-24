using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class QlbInBox
    {
        public Int64 IN_ID { get; set; }
        public Int64 ER_ID { get; set; }
        public Int64 NT_ID { get; set; }
        public string IN_STT { get; set; }
        
        public DateTime? IN_TIME { get; set; }
        public Int64? IN_NUM { get; set; }
        public string IN_MSG { get; set; }
        public Int64 IN_CONT { get; set; }
        
    }
}
