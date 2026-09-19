using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    [Serializable]
    public class M_FLIGHT_FIXES
    {
       
        public Int64? STT { get; set; }
        public Int64? ID { get; set; }
        public string CALLSIGN { get; set; }
        public string ADEP { get; set; }
        public string ADES { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string FIXKIND { get; set; }
        public string FIXNAME { get; set; }
        public string CURRENTSECTOR { get; set; }
        public string FLIGHTRULEAFTERFIX { get; set; }
        public string TIMESTAMP { get; set; }
        public string DISPLAYEDETO { get; set; }
        public string REFERENCEETO { get; set; }
        
    }
}
