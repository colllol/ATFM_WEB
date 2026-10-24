using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class DayFlights
    {
        private Int64? _FLIGHT_PK;
        private Int64? _PERM_ID;
        public Int64 ID { get; set; }
        public Int64 FLIGHT_ID { get; set; }
        public Int64? FLIGHT_PK { get { if (_FLIGHT_PK == null) return 0; return _FLIGHT_PK; } set { _FLIGHT_PK = value; } }
        public Int64? PERM_ID { get { if (_PERM_ID == null) return 0; return _PERM_ID; } set { _PERM_ID = value; } }
        public string PERMNBR{get;set;}
        public string PERMTYPE{get;set;}
        public string FLIGHT_TYPE{get;set;}
        public string PURPOSE{get;set;}
        public Int64? CRAFT_ID{get;set;}
        public Int64? MTOW{get;set;}
        public int VALIDHOURS{get;set;}
        private DateTime? _DATE_OLD;
        private DateTime? _FLIGHTDATE;
        public string FLIGHTNBR{get;set;}
        public string REGISTRATION{get;set;}
        public string FROM_AIRP{get;set;}
        public string TO_AIRP{get;set;}
        public string ETD{get;set;}
        public string ETA{get;set;}
        public string ATD{get;set;}
        public string ATA{get;set;}
        public string VIA{get;set;}
        public string STATUS{get;set;}
        private DateTime? _LETTERNBR_PK;
        public string NBR{get;set;}
        public string LASTUSER{get;set;}
        public string OPER_ID{get;set;}
        public string CRAFT_TYPE{get;set;}
        public string PLAN_STATUS{get;set;}
        public string REMARK{get;set;}
        public Int64? STT{get;set;}
        public string CODE{get;set;}
        private DateTime? _DOF;
         public DateTime? DOF { get { if (_DOF == DateTime.MinValue) return null; return _DOF; }set { _DOF = value; } }
         public DateTime? LETTERNBR_PK { get { if (_LETTERNBR_PK == DateTime.MinValue) return null; return _LETTERNBR_PK; }set { _LETTERNBR_PK = value; } }
        public DateTime? FLIGHTDATE { get { if (_FLIGHTDATE == DateTime.MinValue) return null; return _FLIGHTDATE; }set { _FLIGHTDATE = value; } }
        public DateTime? DATE_OLD { get { if (_DATE_OLD == DateTime.MinValue) return null; return _DATE_OLD; }set { _DATE_OLD = value; } }
        public Int64 HASPERM { get; set; }
        public Int64 STATUSFLIGHT { get; set; }
        public Int64 HASTIMEVALID { get; set; }
        public string CONTENTCHANGE { get; set; }
        public string CHANGEVALUE { get; set; }
        public Int64 ISACCESS { get; set; }
        public string LETTER_TYPE { get; set; }
    }
    public class clsDaylyFlightSearch
    {
        private Int64 _RowStart;
        private Int64 _RowFinish;
        public string PERMNBR { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PURPOSE { get; set; }
        public Int64? CRAFT_ID { get; set; }
        public Int64? MTOW { get; set; }
        public Int64? VALIDHOURS { get; set; }
        public DateTime? DATE_OLD { get; set; }
        public DateTime? FLIGHTDATE { get; set; }
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
        public DateTime? LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string LASTUSER { get; set; }
        public string OPER_ID { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string PLAN_STATUS { get; set; }
        public string REMARK { get; set; }
        public Int64? STT { get; set; }
        public string CODE { get; set; }
        public DateTime? DOF { get; set; }
        public string KHUNGGIO1 { get; set; }
        public string KHUNGGIO2 { get; set; }
        public string CRAFT_TP { get; set; }
        public string PTD { get; set; }
        public Int64? RowStart { get; set; }
        public Int64? RowFinish { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string FPL_VIA { get; set; }
        public string REAL_CRAFT_TYPE { get; set; }
        public int TypeNumber { get; set; }
        public string LETTER_TYPE { get; set; }
        public string ROUTE { get; set; }
        public string ROUTE_TT { get; set; }
        
        public string OptionDate { get; set; }
        public Int64 ISACCESS { get; set; }


    }

    public class clsFinishedFlightSearch
    {
        private Int64 _RowStart;
        private Int64 _RowFinish;
        public string PERMNBR { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PURPOSE { get; set; }
        public Int64? CRAFT_ID { get; set; }
        public Int64? MTOW { get; set; }
        public Int64? VALIDHOURS { get; set; }
        public DateTime? DATE_OLD { get; set; }
        public DateTime? FLIGHTDATE { get; set; }
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
        public DateTime? LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string LASTUSER { get; set; }
        public string OPER_ID { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string PLAN_STATUS { get; set; }
        public string REMARK { get; set; }
        public Int64? STT { get; set; }
        public string CODE { get; set; }
        public DateTime? DOF { get; set; }
        public string KHUNGGIO1 { get; set; }
        public string KHUNGGIO2 { get; set; }
        public string CRAFT_TP { get; set; }
        public string PTD { get; set; }
        public Int64? RowStart { get; set; }
        public Int64? RowFinish { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string FPL_VIA { get; set; }
        public string REAL_CRAFT_TYPE { get; set; }
        public int TypeNumber { get; set; }
        public string LETTER_TYPE { get; set; }
        public string ROUTE { get; set; }
        public string OptionDate { get; set; }
        public Int64 ISACCESS { get; set; }
    }
}
