using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class clsSearchValue
    {
        private DateTime? _FromDate;
        private DateTime? _ToDate;

        public DateTime? FromDate
        {
            get
            {
                if (_FromDate == DateTime.MinValue) return null; return _FromDate;
            }

            set
            {
                _FromDate = value;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                if (_ToDate == DateTime.MinValue) return null; return _ToDate;
            }

            set
            {
                _ToDate = value;
            }
        }
        public string Oper { get; set; }

        public DateTime? Ngay
        {
            get
            {
                return ngay;
            }
            
            set
            {
                ngay = value;
            }
        }

        private DateTime? ngay;

        public string Month { get; set; }
        public int DOMISTIC { get; set; }
        public int FIRHN { get; set; }
        public int FIRHCM { get; set; }
        public int FIRALL { get; set; }
        public string OPER { get; set; }
        public string PUPOSE { get; set; }

    }
}
