using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    [Serializable]
    public class Aero
    {
        #region properties
        private int _ID;
        private Int64 _AE_ID;
        private string _CTR_CODE;
        private int _FR_ID;
        private string _AE_CODE;
        private string _AE_NAME;
        private string _AE_ZONE;
        private string _AE_INTER;
        private string _AE_IATA;
        private string _AE_LAT;
        private string _AE_LONG;
        private string _AD_ISOVERSEA;
        private string _SUMMER_TIME;
        private string _WINTER_TIME;
        private string _TM;
        private string _TN;
        public string _MIEN;
        public string CTR_ENAME { get; set; }
         
        public string TN
        {
            get
            {
                return _TN;
            }

            set
            {
                _TN = value;
            }
        }

        public string TM
        {
            get
            {
                return _TM;
            }

            set
            {
                _TM = value;
            }
        }

        public string WINTER_TIME
        {
            get
            {
                return _WINTER_TIME;
            }

            set
            {
                _WINTER_TIME = value;
            }
        }

        public string SUMMER_TIME
        {
            get
            {
                return _SUMMER_TIME;
            }

            set
            {
                _SUMMER_TIME = value;
            }
        }

        public string AD_ISOVERSEA
        {
            get
            {
                return _AD_ISOVERSEA;
            }

            set
            {
                _AD_ISOVERSEA = value;
            }
        }

        public string AE_LONG
        {
            get
            {
                return _AE_LONG;
            }

            set
            {
                _AE_LONG = value;
            }
        }

        public string AE_LAT
        {
            get
            {
                return _AE_LAT;
            }

            set
            {
                _AE_LAT = value;
            }
        }

        public string AE_IATA
        {
            get
            {
                return _AE_IATA;
            }

            set
            {
                _AE_IATA = value;
            }
        }

        public string AE_INTER
        {
            get
            {
                return _AE_INTER;
            }

            set
            {
                _AE_INTER = value;
            }
        }

        public string AE_ZONE
        {
            get
            {
                return _AE_ZONE;
            }

            set
            {
                _AE_ZONE = value;
            }
        }

        public string AE_NAME
        {
            get
            {
                return _AE_NAME;
            }

            set
            {
                _AE_NAME = value;
            }
        }

        public string AE_CODE
        {
            get
            {
                return _AE_CODE;
            }

            set
            {
                _AE_CODE = value;
            }
        }

        public int FR_ID
        {
            get
            {
                return _FR_ID;
            }

            set
            {
                _FR_ID = value;
            }
        }

        public string CTR_CODE
        {
            get
            {
                return _CTR_CODE;
            }

            set
            {
                _CTR_CODE = value;
            }
        }

        public Int64 AE_ID
        {
            get
            {
                return _AE_ID;
            }

            set
            {
                _AE_ID = value;
            }
        }
        public string MIEN
        {
            get
            {
                return _MIEN;
            }

            set
            {
                _MIEN = value;
            }
        }
        public int ID
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

        #endregion

        #region contructor
        public Aero()
        {

        }
        public Aero(Aero obj)
        {
            this._AD_ISOVERSEA = obj.AD_ISOVERSEA;
            this._AE_CODE = obj.AE_CODE;
            this._AE_IATA = obj.AE_IATA;
            this._AE_ID = obj.AE_ID;
            this._AE_INTER = obj.AE_INTER;
            this._AE_LAT = obj.AE_LAT;
            this._AE_LONG = obj.AE_LONG;
            this._AE_NAME = obj.AE_NAME;
            this._AE_ZONE = obj.AE_ZONE;
            this._CTR_CODE = obj.CTR_CODE;
            this._FR_ID = obj.FR_ID;
            this._ID = obj.ID;
            this._SUMMER_TIME = obj.SUMMER_TIME;
            this._TM = obj.TM;
            this._TN = obj.TN;
            this._MIEN = obj.MIEN;
        }
        #endregion

    }
}
