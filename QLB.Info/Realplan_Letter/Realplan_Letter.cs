using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class Realplan_Letter
    {
        public DateTime? LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string LETTER_TYPE { get; set; }
        public string STATUS { get; set; }
        public int FLIGHT_ID { get; set; }
        public string FLIGHTNBR { get; set; }
        public string REGISTRATION { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string VIA { get; set; }
        public string LASTUSER { get; set; }
        public string CNL_TYPE { get; set; }
        public string CRAFT_TYPE { get; set; }
        public DateTime? FLIGHTDATE { get; set; }
        public string CODE { get; set; }
        public string DOF { get; set; }
        public string TEXT { get; set; }
        public int ISFIR { get; set; }
        public string ROUTE { get; set; }
        public string ROUTE_TT { get; set; }
        public int HASPERM { get; set; }
        public int FLIGHT_ID_CUS { get; set; }
        public int STT { get; set; }
        public int ISSELECT { get; set; }
        public int RECORD_SUM { get; set; }
    }
    public class clsSearchRealplanLetter : Realplan_Letter
    {
        public int PAGE_INDEX { get; set; }
        public int PAGE_SIZE { get; set; }
    }
}
