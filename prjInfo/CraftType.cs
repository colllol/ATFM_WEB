using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class CraftType
    {
        private int _ID;
        private string _MA;
        private string _SOHIEU;
        private decimal _TAITRONG;
        private int _CRAFT_ID;
        //public int _NUMBER_CHAIRS;
        //public string _YEAR_MANUFACTURE;

        //public int NUMBER_CHAIRS
        //{
        //    get
        //    {
        //        return _NUMBER_CHAIRS;
        //    }

        //    set
        //    {
        //        _NUMBER_CHAIRS = value;
        //    }
        //}
        //public string YEAR_MANUFACTURE
        //{
        //    get
        //    {
        //        return _YEAR_MANUFACTURE;
        //    }

        //    set
        //    {
        //        _YEAR_MANUFACTURE = value;
        //    }
        //}
        public int CRAFT_ID
        {
            get
            {
                return _CRAFT_ID;
            }

            set
            {
                _CRAFT_ID = value;
            }
        }

        public decimal TAITRONG
        {
            get
            {
                return _TAITRONG;
            }

            set
            {
                _TAITRONG = value;
            }
        }

        public string SOHIEU
        {
            get
            {
                return _SOHIEU;
            }

            set
            {
                _SOHIEU = value;
            }
        }

        public string MA
        {
            get
            {
                return _MA;
            }

            set
            {
                _MA = value;
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
    }
}
