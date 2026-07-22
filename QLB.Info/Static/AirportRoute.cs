using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class AirportRoute
    {
        private string _IS_OVERSEA;
        private string _IS_DOMESTIC;
        public int ID { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ROUTE { get; set; }
        public string IS_OVERSEA
        {
            get
            {
                if (_IS_OVERSEA == "1") return "1"; return "0";
            }
            set
            {
                _IS_OVERSEA = value;
            }
        }
        public int DOMESTIC { get; set; }
        public int INTERNATIONAL { get; set; }
        public string IS_DOMESTIC
        {
            get
            {
                if (_IS_DOMESTIC == "1") return "1"; return "0";
            }
            set
            {
                _IS_DOMESTIC = value;
            }
        }
        public int SUMMARY { get; set; }
    }
}
