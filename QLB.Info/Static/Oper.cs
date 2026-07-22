using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info.Perm
{
    public class Oper
    {
        private string _IS_DOMESTIC;
        public Int64 ID { get; set; }
        public string OPER_ICAO { get; set; }
        public string OPER_IATA { get; set; }
        public string OPER_NAME { get; set; }
        public string OPER_ADDRESS { get; set; }
        public string IS_DOMESTIC {
            get
            {
                if (_IS_DOMESTIC == "1") return "1"; return "0";
            }
            set
            {
                _IS_DOMESTIC = value;
            }
        }

    }
}
