using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class FlightPemissionDetailHistory: FlightPermissionDetail
    {
        public FlightPemissionDetailHistory():base()
        {

        }
        public int Log_History { get; set; }

    }
}
