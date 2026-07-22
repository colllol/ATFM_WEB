using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
  public class Calendar
    {
        public Int64 ID { get; set; }
        public int CALENDAR_ID { get; set; }
        public string CALENDAR_NAME { get; set; }
        public string LASTUSER { get; set; }
        public DateTime MAKING_DATE { get; set; }
        public string USER_NAME { get; set; }
        public DateTime BEGIN_DATE { get; set; }
        public DateTime FINISH_DATE { get; set; }
        public string SEASON { get; set; }
        public string YEAR { get; set; }
        public string TYPE { get; set; }
    }
}
