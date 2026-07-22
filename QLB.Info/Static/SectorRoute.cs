using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class SectorRoute
    {
        public int ID { get; set; }
        public int SECTOR_ID { get; set; }
        public int ROUTE_ID { get; set; }
        public string STATUS { get; set; }
        public string SECTOR_NAME { get; set; }
        public string ROUTE_NAME { get; set; }
    }
}
