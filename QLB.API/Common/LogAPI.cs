using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace QLB.API.Common
{
    public class LogAPI
    {
        private static string _logPath = string.Empty;

        public static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                    _logPath = ConfigurationManager.AppSettings["LOG_PATH"];
                return _logPath;
            }
            set
            {
                _logPath = value;
            }
        }

        public static void LogToFile(LogFileType logFileType, string message)
        {
            LoggerGeneral logger = new LoggerGeneral();
            message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
            FormatterGeneral formatter = new FormatterGeneral(message);
            string logPath = string.Empty;
            logPath = LogPath + @"\" + DateTime.Now.ToString("yyyyMMdd") + " " + DateTime.Now.ToString("HH");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            switch(logFileType)
            {
                case LogFileType.TRACE:
                    logPath = logPath + @"\TRACE.log";
                    break;
                case LogFileType.MESSAGE:
                    logPath = logPath + @"\MESSAGE.log";
                    break;
                default:
                    logPath = logPath + @"\EXCEPTION.log";
                    break;
            }
            logger.write(formatter, logPath);
        }
    }
    public enum LogFileType
    {
        TRACE,
        MESSAGE,
        EXCEPTION
    }
}