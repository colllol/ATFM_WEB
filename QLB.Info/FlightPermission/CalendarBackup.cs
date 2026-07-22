using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class CalendarBackup
    {
        public int ID { get; set; }
        public int CALENDAR_ID { get; set; }
        public int PERM_ID { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public int FLIGHT_PK { get; set; }
        public int CRAFT_ID { get; set; }
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
        public int MTOW { get; set; }
        public string REGISTRATION { get; set; }
        public string VIA { get; set; }
        public string REMARK { get; set; }
        public DateTime BEGINDATE { get; set; }
        public DateTime ENDDATE { get; set; }
        public int LOG_HISTORY { get; set; }
        public string USER_NAME { get; set; }
        public string LAST_USER { get; set; }
        public DateTime TIME_UPDATE { get; set; }
        public string STATUS_DEL { get; set; }
    }
}
