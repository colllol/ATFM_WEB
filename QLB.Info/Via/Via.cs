using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class Via
    {
        public Int64 ID { get; set; }        
        public string SOHIEU { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string VIA { get; set; }
        public string POINT_IN { get; set; }
        public string POINT_OUT { get; set; }
        public string MUCBAY { get; set; }
        public string QUYTAC { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string OPER { get; set; }
        public string FlightType { get; set; }
        public string PermType { get; set; }
        public string ReMark { get; set; }

    }
    public class ViaSearch:Via
    {
        public Int64 PageSize { get; set; }
        public Int64 PageIndex { get; set; }
        public Int64 Record_Sum { get; set; }
        
    }
}
