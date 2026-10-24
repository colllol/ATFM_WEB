using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
namespace prjBusinessLogic.DAL
{
   public class HPC_GenerateHtml
    {
        public int GenerateHTML(string Url, string physicalFullPath)
        {
            int intResult = 0;
            try
            {
                // Read Source of Web page
                WebRequest req = HttpWebRequest.Create(Url);
                req.Method = "GET";

                string source;
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }
                // Write HTML File            
                using (StreamWriter sw = new StreamWriter(physicalFullPath))
                {
                    sw.WriteLine(source);
                }

            }
            catch (Exception ex)
            {
                intResult = 1;
            }
            return intResult;
        }
    }
}
