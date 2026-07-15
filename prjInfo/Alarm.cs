using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    
    public class Alarm
    {
        public int ID { get; set; }
        public string EVENT_NAME { get; set; }
        public DateTime EVENT_DATE { get; set; }
        public DateTime BEGIN_VALID { get; set; }
        public DateTime END_VALID { get; set; }
        public int STATUS { get; set; }
    }
    
}
