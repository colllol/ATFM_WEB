using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class clsDayFlightValueSearch
    {
        private DateTime? _DATE_OLD;
        private DateTime? _FLIGHTDATE;
        private DateTime? _LETTERNBR_PK;
        private DateTime? _DOF;
        public string PERMNBR { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PURPOSE { get; set; }
        public Int64 CRAFT_ID { get; set; }
        public Int64 MTOW { get; set; }
        public Int64 VALIDHOURS { get; set; }
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
        public string STATUS { get; set; }
        public DateTime? LETTERNBR_PK { get { if (_LETTERNBR_PK == DateTime.MinValue) return null; return _LETTERNBR_PK; } set { _LETTERNBR_PK = value; } }
        public string NBR { get; set; }
        public string LASTUSER { get; set; }
        public string OPER_ID { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string PLAN_STATUS { get; set; }
        public string REMARK { get; set; }
        public Int64 STT { get; set; }
        public string CODE { get; set; }
        public DateTime? DOF { get { if (_DOF == DateTime.MinValue) return null; return _DOF; } set { _DOF = value; } }
        public string KHUNGGIO1 { get; set; }
        public string KHUNGGIO2 { get; set; }
        public string CRAFT_TP { get; set; }
        public string PTD { get; set; }
        public Int64 RowStart { get; set; }
        public Int64 RowFinish { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string FPL_VIA { get; set; }
        public string REAL_CRAFT_TYPE { get; set; }
        public string LETTER_TYPE { get; set; }
        public Int64 ISACCESS { get; set; }
        public string ROUTE { get; set; }
        public string ROUTE_TT { get; set; }
    }
}
