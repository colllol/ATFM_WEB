using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    
   public class CalendarDetail
    {
        public Int64 ID { get; set; }
        public Int64 CALENDAR_ID { get; set; }
        public Int64 PERM_ID { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public Int64 FLIGHT_PK { get; set; }
        public Int64 CRAFT_ID { get; set; }
        public string FLIGHTNBR { get; set; }
        public string PURPOSE_ID { get; set; }
        public string DAY1 { get; set; }
        public string DAY2 { get; set; }
        public string DAY3 { get; set; }
        public string DAY4 { get; set; }
        public string DAY5 { get; set; }
        public string DAY6 { get; set; }
        public string DAY7 { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public Int64 MTOW { get; set; }
        public string REGISTRATION { get; set; }
        public string VIA { get; set; }
        public string REMARK { get; set; }
        public DateTime? BEGINDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public string USER_NAME { get; set; }
        public string LAST_USER { get; set; }
        public DateTime? TIME_UPDATE { get; set; }
        public string PURPOSE_NAME { get; set; }
        public string CRAFT_NAME { get; set; }

    }
}
