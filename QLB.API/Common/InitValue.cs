using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace QLB.API.Common
{
    public class InitValue
    {
        public static string QueryConnString = string.Empty;
        public static string strSchemaName = string.Empty;
        public static int maxSizeRecord = 1000;

        public void Init()
        {
            QueryConnString = System.Configuration.ConfigurationManager.ConnectionStrings["fdp"].ConnectionString;
            strSchemaName = System.Configuration.ConfigurationManager.AppSettings["SCHEMA_NAME"].ToString();
            maxSizeRecord = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MAX_RECORD"].ToString());
        }
    }
}