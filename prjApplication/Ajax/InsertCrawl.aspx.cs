using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using prjBusinessLogic;

namespace prjApplication.Ajax
{
    public partial class InsertCrawl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    string _urlEnd = Request["link"];
                    string _cateID = Request["id"];
                    this.litContent.Text = ExtractContent(_urlEnd, _cateID);
                }
            }
        }
        protected string ExtractContent(string url, string _cateID)
        {
            string extractedContent = string.Empty;
            if (_cateID != "")
            {
                string _xpath = UltilFunc.GetXPathByID(Convert.ToInt32(_cateID));
                if (_xpath != "")
                {
                    extractedContent = GetContentByXpath(url, _xpath);                   
                }
            }
            return extractedContent;

        }
        protected string RetrieveContent(string webPage)
        {
            HttpWebResponse response = null;//used to get response
            StreamReader respStream = null;//used to read response into string
            string content = "";
            try
            {
                //create a request object using the url passed in
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webPage);
                request.Timeout = 100000;
                //go get a response from the page
                response = (HttpWebResponse)request.GetResponse();
                //create a streamreader object from the response
                respStream = new StreamReader(response.GetResponseStream());
                //get the contents of the page as a string and return it
                content = respStream.ReadToEnd();
            }
            catch //(Exception ex)//houston we have a problem!
            {
                //throw ex;
            }
            finally
            {
                //close it down, we're going home!
                response.Close();
                respStream.Close();
            }
            return content;
        }

        protected string SearchImgTag(string str,string _imgHref)
        {
            System.Text.StringBuilder _strB = new System.Text.StringBuilder();
            try
            {
                Regex regex = new Regex(
                    @"(?<=<img[^<]+?src=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    string _url = matchCollect[i].Value.Trim();
                    if (_url.Length > 0)
                    {
                        if (!_url.StartsWith("http"))
                        {
                            string _urlImg = matchCollect[i].Value;
                            string _urlImgReplate = _imgHref + _urlImg;
                            str = System.Text.RegularExpressions.Regex.Replace(str, _urlImg, _urlImgReplate, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            catch { }

            return str;
        }
        public string GetContentByXpath(string _url,string _xpath)
        {
            string _content = "";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(_url);
            char[] sep = { ',' };
            string[] sArrXpath = null;
            sArrXpath = _xpath.Split(sep);
            int value = int.Parse(sArrXpath.Length.ToString());
            switch (value)
            {
                case 2:
                    for (int i = 0; i < sArrXpath.Length; i++)
                    {
                        try
                        {
                            foreach (HtmlNode link in doc.DocumentNode.SelectNodes(sArrXpath[0].ToString()))
                            {
                                _content = link.InnerHtml;
                            }
                        }
                        catch
                        {
                            try
                            {
                                foreach (HtmlNode link in doc.DocumentNode.SelectNodes(sArrXpath[1].ToString()))
                                {
                                    _content = link.InnerHtml;
                                }
                            }
                            catch
                            {
                                _content = "Lỗi ! Hãy kiểm tra lại xpath .";
                            }
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < sArrXpath.Length; i++)
                    {
                        try
                        {
                            foreach (HtmlNode link in doc.DocumentNode.SelectNodes(sArrXpath[0].ToString()))
                            {
                                _content = link.InnerHtml;
                            }
                        }
                        catch
                        {
                            try
                            {
                                foreach (HtmlNode link in doc.DocumentNode.SelectNodes(sArrXpath[1].ToString()))
                                {
                                    _content = link.InnerHtml;
                                }
                            }
                            catch
                            {
                                try
                                {
                                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes(sArrXpath[2].ToString()))
                                    {
                                        _content = link.InnerHtml;
                                    }
                                }
                                catch
                                {
                                    _content = "Lỗi ! Hãy kiểm tra lại xpath .";
                                }
                            }
                        }
                    }
                    break;
                default:
                    try
                    {
                        foreach (HtmlNode link in doc.DocumentNode.SelectNodes(_xpath.ToString().Trim()))
                        {
                            _content = link.InnerHtml;

                        }
                    }
                    catch
                    {
                        _content = "Lỗi ! Hãy kiểm tra lại xpath .";                       
                    }
                    break;
            }
            
            return _content;
        }
        
    }
}
