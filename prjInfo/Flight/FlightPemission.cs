using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class FlightPermission
    {
        private DateTime? _MAKING_DATE;
        private DateTime? _FINISH_DATE;
        private DateTime? _BEGIN_DATE;
        public Int64 ID { get; set; }
        public int CALENDAR_ID { get; set; }
        public string CALENDAR_NAME { get; set; }
        public string LASTUSER { get; set; }
        public DateTime? MAKING_DATE {
            get
            {
                if (_MAKING_DATE == DateTime.MinValue) return null;
                else return _MAKING_DATE;
            }
            set
            { _MAKING_DATE = value;
            }
        }
        public string USER_NAME { get; set; }
        public DateTime? BEGIN_DATE
        {
            get
            {
                if (_BEGIN_DATE == DateTime.MinValue) return null;
                else return _BEGIN_DATE;
            }
            set
            {
                _BEGIN_DATE = value;
            }
        }
        public DateTime? FINISH_DATE {
            get
            {
                if (_FINISH_DATE == DateTime.MinValue) return null;
                else return _FINISH_DATE;
            }
            set
            {
                _FINISH_DATE = value;
            }
        }
        public string SEASON { get; set; }
        public string YEAR { get; set; }
        public string TYPE { get; set; }
    }
}
