using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class Alarm
    {
        private int _STATUS;
        public int ID { get; set; }
        public string EVENT_NAME { get; set; }
        public DateTime EVENT_DATE { get; set; }
        public DateTime BEGIN_VALID { get; set; }
        public DateTime END_VALID { get; set; }
        public int STATUS {
            get
            {
                if (_STATUS == 1) return 1; return 0;
            }
            set
            {
                _STATUS = value;
            }
        }
    }
}
