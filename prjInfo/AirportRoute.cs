using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class AirportRoute
    {
        public int ID { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ROUTE { get; set; }
        public string IS_OVERSEA { get; set; }
        public int DOMESTIC { get; set; }
        public int INTERNATIONAL { get; set; }
        public string IS_DOMESTIC { get; set; }
        public int SUMMARY { get; set; }

    }
}
