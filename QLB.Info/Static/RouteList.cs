using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class RouteList
    {
        private string _IS_OVERSEA;
        public int ID { get; set; }
        public string SOKM { get; set; }
        public int ROUTE_ID { get; set; }
        public string ROUTE_NAME { get; set; }
        public string DESCRIPTION { get; set; }
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

    }
}
