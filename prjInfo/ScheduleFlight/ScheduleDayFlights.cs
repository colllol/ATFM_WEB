using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class ScheduleDayFlights:DayFlights
    {
        public int ISRENDER { get; set; }
        public int TOTALRECORDS { get; set; }
    }
    public class ScheduleDayFlight_BAK : ScheduleDayFlights
    {
        public string CONTENT { get; set; }
        public int NOVERSION { get; set; }
        public string ACTION { get; set; }
        public DateTime? LASTMODIFY { get; set; }
    }
}
