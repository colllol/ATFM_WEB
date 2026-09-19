using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
   public class FinishedFlights
    {
        private DateTime? _DATE_OLD;
        private DateTime? _FLIGHTDATE;
        public Int64? STT { get; set; }
        public Int64? FLIGHT_ID { get; set; }
        public Int64? FLIGHT_PK { get; set; }
        public Int64? PERM_ID { get; set; }
        public string PERMNBR { get; set; }
        public string OPER_ID { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PURPOSE { get; set; }
        public Int64? CRAFT_ID { get; set; }
        public Int64? MTOW { get; set; }
        public Int64? VALIDHOURS { get; set; }
        public DateTime? DATE_OLD { get { if (_DATE_OLD == DateTime.MinValue) return null; return _DATE_OLD; } set { _DATE_OLD = value; } }
        public DateTime? FLIGHTDATE { get { if (_FLIGHTDATE == DateTime.MinValue) return null; return _FLIGHTDATE; } set { _FLIGHTDATE = value; } }
        public string FLIGHTNBR { get; set; }
        public string REGISTRATION { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string VIA { get; set; }
        public DateTime? LETTERNBR_PK { get; set; }
        public string LASTUSER { get; set; }
        public string LASTMODIFY { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string REMARK { get; set; }
        public string FPL_VIA { get; set; }
        public string REAL_CRAFT_TYPE { get; set; }
        public Int64? RowStart { get; set; }
        public Int64? RowFinish { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public string KHUNGGIO1 { get; set; }
        public string KHUNGGIO2 { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TypeOper { get; set; }
        public int TypeFlight { get; set; }
    }
}
