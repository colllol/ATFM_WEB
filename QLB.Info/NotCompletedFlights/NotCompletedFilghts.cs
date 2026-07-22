using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class NotCompletedFlights
    {
        public int FLIGHT_ID { get; set; }
        public int FLIGHT_PK { get; set; }
        public int PERM_ID { get; set; }
        public string PERMNBR { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PURPOSE { get; set; }
        public int CRAFT_ID { get; set; }
        public int MTOW { get; set; }
        public int VALIDHOURS { get; set; }
        public DateTime DATE_OLD { get; set; }
        public DateTime FLIGHTDATE { get; set; }
        public string FLIGHTNBR { get; set; }
        public string REGISTRATION { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string VIA { get; set; }
        public DateTime LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string OPER_ID { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string REMARK { get; set; }
    }
}
