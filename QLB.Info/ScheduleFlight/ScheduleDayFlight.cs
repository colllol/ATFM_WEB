using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class ScheduleDayFlight: DayFlights
    {
        public int ISRENDER { get; set; }
        public int TOTALRECORDS { get; set; }
    }
    public class ScheduleDayFlight_BAK: ScheduleDayFlight
    {
        public string CONTENT { get; set; }
        public int NOVERSION { get; set; }
        public string ACTION { get; set; }
        public DateTime LASTMODIFY { get; set; }
    }
}
