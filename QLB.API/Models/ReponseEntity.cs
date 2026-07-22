using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLB.API.Models
{
    public class ReponseEntity
    {
        public string SumRecord { get; set; }
        public string Code { get; set; }

        public string Message { get; set; }

        public object Value { get; set; }

        public List<object> ListValue { get; set; }
    }
    public class ReponseReportEntity
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public object Value { get; set; }

        public object ListValue { get; set; }

        public string SumRecord { get; set; }
    }
}