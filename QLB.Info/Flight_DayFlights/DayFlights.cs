using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLB.Info
{
    public class DayFlights
    {
        public Int64 ID { get; set; }
        public Int64 FLIGHT_ID { get; set; }
        public Int64 FLIGHT_PK { get; set; }
        public Int64 PERM_ID { get; set; }
        public string PERMNBR { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PURPOSE { get; set; }
        public Int64 CRAFT_ID { get; set; }
        public Int64 MTOW { get; set; }
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
        public string STATUS { get; set; }
        public DateTime LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string LASTUSER { get; set; }
        public string OPER_ID { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string PLAN_STATUS { get; set; }
        public string REMARK { get; set; }
        public Int64 STT { get; set; }
        public string CODE { get; set; }        
        public DateTime DOF { get; set; }
        public string CHANGEVALUE { get; set; }
        public Int64 ISACCESS { get; set; }
        public string LETTER_TYPE { get; set; }


    }
    public class DayFlights_BAK: DayFlights
    {
        public string CONTENT { get; set; }
        public int NOVERSION { get; set; }
        public string ACTION { get; set; }
        public DateTime LASTMODIFY { get; set; }
    }

    public class DayFlights_GoingOn : DayFlights
    {
        public string EOBT { get; set; }
        public string ATDDATE { get; set; }
        public string ATADATE { get; set; }
        public string EOBTDATE { get; set; }
        public string ROUTE_TT { get; set; }
    }
}
