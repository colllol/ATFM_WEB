using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
  public  class Aero
    {
        private string _AE_INTER;
        private string _AD_ISOVERSEA;
        public int ID { get; set; }
        public Int64 AE_ID { get; set; }
        public string CTR_CODE { get; set; }
        public int FR_ID { get; set; }
        public string AE_CODE { get; set; }
        public string AE_NAME { get; set; }
        public string AE_ZONE { get; set; }
        public string AE_INTER {
            get
            {
                if (_AE_INTER == "Y") return "Y"; return "N";
            }
            set
            {
                _AE_INTER = value;
            }
        }
        public string AE_IATA { get; set; }
        public string AE_LAT { get; set; }
        public string AE_LONG { get; set; }
        public string SUMMER_TIME { get; set; }
        public string WINTER_TIME { get; set; }
        public string TM { get; set; }
        public string TN { get; set; }
        public string CTR_ENAME { get; set; }
        public string AD_ISOVERSEA
        {
            get 
            {
                { if (_AD_ISOVERSEA == "Y") return "Y"; return "N"; }
            }

            set
            {
                _AD_ISOVERSEA = value;
            }
        }
       
        public string MIEN { get; set; }
    }
}
