using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class SectorRoute
    {
        private int _SECTOR_ID;
        private string _SECTOR_NAME;
        private string _NOTE;
        public int ROUTE_ID { get; set; }
        public string MARK { get; set; }
        public string ROUTE_NAME { get; set; }
        public int SECTOR_ID
        {
            get
            {
                return _SECTOR_ID;
            }
            set
            {
                _SECTOR_ID = value;
            }
        }

        public string SECTOR_NAME
        {
            get
            {
                return _SECTOR_NAME;
            }
            set
            {
                _SECTOR_NAME = value;
            }
        }
        public string NOTE
        {
            get
            {
                return _NOTE;
            }
            set
            {
                _NOTE = value;
            }
        }


    }
}
