using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace QLB.API.Common
{
    public class LoggerGeneral
    {
        public void write(FormatterBase formatter, string logFileName)
        {
            TextWriterTraceListener listener = new TextWriterTraceListener(logFileName);
            listener.WriteLine(formatter.Message);
            listener.Flush();
            listener.Close();
        }
    }
}