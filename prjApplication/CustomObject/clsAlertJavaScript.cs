using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using TuesPechkin;

namespace prjApplication
{
    public static class clsAlertJavaScript
    {
        public static void AlertMessage(this System.Web.UI.Page page, string message)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, typeof(string), "Message", $"alert(\"{message}\");", true);
        }
        public static void ExcuteJavascript(this System.Web.UI.Page page, string textJs)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, typeof(string), "Excute", $"{textJs}", true);
        }
    }
    public static class clsRenderToHTML
    {
        public static string RenderToHTML(this System.Web.UI.Page page, System.Web.UI.Control ctrl)
        {            
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            ctrl.RenderControl(hw);
            var ax = sb.ToString();
            hw.Dispose();
            tw.Dispose();
            sb.Clear();
            
            return ax;           
        }
        public static void CreatePDF(this System.Web.UI.Page page, string html, string fileName, string pathCSS, System.Drawing.Printing.PaperKind pageSize)
        {
            if (File.Exists(pathCSS))
            {
                html = html.Insert(0, string.Format("<style type=\"text/css\">{0}</style>", File.ReadAllText(pathCSS)));
            }
            page.Response.ClearContent();
            page.Response.Clear();
            page.Response.ClearHeaders();
            page.Response.ContentType = "application/pdf";
            page.Response.AddHeader("content-disposition",
             "attachment;filename=" + fileName);
            var tempFolderDeployment = new TempFolderDeployment();
            var win32EmbeddedDeployment = new WinAnyCPUEmbeddedDeployment(new TempFolderDeployment());
            var remotingToolset = new RemotingToolset<PdfToolset>(win32EmbeddedDeployment);

            var converter =
                 new ThreadSafeConverter(remotingToolset);
            HtmlToPdfDocument Document = new HtmlToPdfDocument
            {
                Objects =
            {

                new ObjectSettings
                {
                    HtmlText = html,
                    WebSettings = new WebSettings {
                        DefaultEncoding = "UTF-8",
                        MinimumFontSize = 8,
                        EnableIntelligentShrinking = true,
                        PrintBackground = true
                        ,LoadImages=true
                        //,PrintMediaType=true

                    }
                }
            },
                GlobalSettings =
            {
                    OutputFormat = GlobalSettings.DocumentOutputFormat.PDF,
                    ColorMode = GlobalSettings.DocumentColorMode.Color,
                    ProduceOutline = true,
                    DocumentTitle = "BAO CAO",
                    PaperSize = pageSize,
                    Margins =
                    {
                        All = 0.375,
                        Unit = TuesPechkin.Unit.Centimeters
                    }
            }
            };
            byte[] result = converter.Convert(Document);
            remotingToolset.Unload();
            page.Response.ContentEncoding = System.Text.Encoding.UTF8;
            page.Response.OutputStream.Write(result, 0, result.Length);
            page.Response.End();
        }
        public static void CreateExcel(this System.Web.UI.Page page, string html, string fileName)
        {
            
            page.Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
            page.Response.Charset = "";
            page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            page.Response.ContentType = "application/vnd.ms-excel";
            page.EnableViewState = false;
            page.Response.Write(html);
            page.Response.End();
            
        }

        public static void CreateExcel(this System.Web.UI.Page page, string html, string fileName, string pathCSS)
        {
            string _style = null;
            if (File.Exists(pathCSS))
                 _style = string.Format("<style type=\"text/css\">{0}</style>", File.ReadAllText(pathCSS));           
            page.Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
            page.Response.Charset = "";
            page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            page.Response.ContentType = "application/vnd.ms-excel";
            page.EnableViewState = false;
            page.Response.Write("<head>");
            page.Response.Write(_style);
            page.Response.Write("</head>");
            page.Response.Write(string.Format("<body>{0}</body></html>", html));
            page.Response.End();

        }

        public static void CreateWord(this System.Web.UI.Page page, string html, string fileName, string pathCSS)
        {
            string _style = null;
            if (File.Exists(pathCSS))
                _style = string.Format("<style type=\"text/css\">{0}</style>", File.ReadAllText(pathCSS));
            page.Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
            page.Response.Charset = "";
            page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            page.Response.ContentType = "application/vnd.ms-word";
            page.EnableViewState = false;
            page.Response.Write("<head>");
            page.Response.Write(_style);
            page.Response.Write("</head>");
            page.Response.Write(string.Format("<body>{0}</body></html>", html));
            page.Response.End();

        }

    }
    public class DateTimeConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>() { typeof(DateTime), typeof(DateTime?) }; }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null) return result;
            result["DateTime"] = ((DateTime)obj).ToString("dd/MM/yyyy");
            return result;
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary.ContainsKey("DateTime"))
                return new DateTime(long.Parse(dictionary["DateTime"].ToString()), DateTimeKind.Unspecified);
            return null;
        }
    }
}