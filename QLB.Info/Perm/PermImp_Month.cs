using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info.Perm
{
    
    public class PermImp_Month
    {
        private Int64 _ID;
        private string _CRAFT_TYPE;
        private string _CRAFT_VERSION;
        private string _CRAFT_LOGICAL_NO;
        private string _REGISTER_CRAFT;
        private string _CALLSIGN;
        private string _SECTOR;
        private string _FROM_AIRP;
        private string _TO_AIRP;
        private string _ETD;
        private string _ETA;
        private DateTime _FLIGHT_DATE;
        private string _OPER;
        private int _STT;
        public DateTime FLIGHT_DATE
        {
            get
            {
                return _FLIGHT_DATE;
            }

            set
            {
                _FLIGHT_DATE = value;
            }
        }

        public string ETA
        {
            get
            {
                return _ETA;
            }

            set
            {
                _ETA = value;
            }
        }

        public string ETD
        {
            get
            {
                return _ETD;
            }

            set
            {
                _ETD = value;
            }
        }

        public string TO_AIRP
        {
            get
            {
                return _TO_AIRP;
            }

            set
            {
                _TO_AIRP = value;
            }
        }

        public string FROM_AIRP
        {
            get
            {
                return _FROM_AIRP;
            }

            set
            {
                _FROM_AIRP = value;
            }
        }

        public string SECTOR
        {
            get
            {
                return _SECTOR;
            }

            set
            {
                _SECTOR = value;
            }
        }

        public string CALLSIGN
        {
            get
            {
                return _CALLSIGN;
            }

            set
            {
                _CALLSIGN = value;
            }
        }

        public string REGISTER_CRAFT
        {
            get
            {
                return _REGISTER_CRAFT;
            }

            set
            {
                _REGISTER_CRAFT = value;
            }
        }

        public string CRAFT_LOGICAL_NO
        {
            get
            {
                return _CRAFT_LOGICAL_NO;
            }

            set
            {
                _CRAFT_LOGICAL_NO = value;
            }
        }

        public string CRAFT_VERSION
        {
            get
            {
                return _CRAFT_VERSION;
            }

            set
            {
                _CRAFT_VERSION = value;
            }
        }

        public string CRAFT_TYPE
        {
            get
            {
                return _CRAFT_TYPE;
            }

            set
            {
                _CRAFT_TYPE = value;
            }
        }

        public Int64 Id
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }

        public string OPER
        {
            get
            {
                return _OPER;
            }

            set
            {
                _OPER = value;
            }
        }

        public int STT
        {
            get
            {
                return _STT;
            }

            set
            {
                _STT = value;
            }
        }
    }
    public class perm_imp_month_search : PermImp_Month
    {
        private int _pageSize;
        private int _pagaIndex;
        private DateTime _startDate;
        private DateTime _finishDate;

        public DateTime FinishDate
        {
            get
            {
                return _finishDate;
            }

            set
            {
                _finishDate = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value;
            }
        }

        public int PagaIndex
        {
            get
            {
                return _pagaIndex;
            }

            set
            {
                _pagaIndex = value;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }
    }
}
