using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
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
                return _Ngay;
            }

            set
            {
                _Ngay = value;
            }
        }

        private DateTime? _Ngay;

        public string Month { get; set; }
    }
}
