using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class SearchPermExtension
    {
        private DateTime? _sDate;
        private DateTime? _fDate;
        public string FLIGHTNBR { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string CRAFT { get; set; }
        public string VIA { get; set; }
        public string PERMNBR { get; set; }
        public string PERMDATE { get; set; }
        public string OPER { get; set; }
        public string SEASION { get; set; }
        public string FLIGHT_TYPE { get; set; }
        public string PERMTYPE { get; set; }
        public string ETD { get; set; }
        public string Remark { get; set; }
        public string Purpose { get; set; }
        public string ETA { get; set; }
        public DateTime? sDate { get { if (_sDate == DateTime.MinValue) return null; return _sDate; } set { _sDate = value; } }
        public DateTime? fDate { get { if (_fDate == DateTime.MinValue) return null; return _fDate; } set { _fDate = value; } }
        public Int64 PAGESIZE { get; set; }
        public Int64 PAGEINDEX { get; set; }
    }
}
