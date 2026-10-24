using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.SessionState;

namespace CustomControl
{
    [ToolboxData("<{0}:CustomPaging runat=server NumberViewPage=7 pageSize=20 ></{0}:CustomPaging>")]
    public class CustomPaging : CompositeControl
    {
        const string _where = " 1=1";
        public CustomPaging()
        {
            EnsureChildControls();
            SetStatusProperties();
        }
        #region properties       
        private int pageSize;
        private int pageIndex;
        private int totalsRecord;
        private int numberViewPage;
        private Uri uRL;
        private bool _IsSearch;
        private string _RedirectFirstSearch;
        private string _LabelTotal;
        private string _DisplayPage;
        private string _ValueSearch;
       
        public string LabelTotal
        {
            get
            {
                int ax = 1;
                if (totalsRecord % pageSize == 0) ax = totalsRecord / pageSize;
                else ax = (totalsRecord / pageSize) + 1;
                return $"Page {pageIndex.ToString()}/{ax.ToString()} ({totalsRecord} records)";
            }

        }

        public int NumberViewPage
        {
            get
            {
                //EnsureChildControls();
                return numberViewPage;
            }

            set
            {
                //EnsureChildControls();
                numberViewPage = value;
            }
        }

        public int TotalsRecord
        {
            get
            {
                return totalsRecord;
            }

            set
            {
                totalsRecord = value;
            }
        }

        public int PageIndex
        {
            get
            {
                if (pageIndex < 0)
                    return 0;
                return pageIndex;
            }
            set
            {
                if (value < 0)
                    pageIndex = 0;
                pageIndex = value;
            }
        }
        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                pageSize = value;
            }
        }

        public string DisplayPage
        {
            get
            {
                return _DisplayPage;
            }

        }



        public string ValueSearch
        {
            set
            {
                if (_ValueSearch != value)
                    PageIndex = 0;
                _ValueSearch = value;
            }
            get
            {
                return string.IsNullOrEmpty(_ValueSearch) ? _where : _ValueSearch;
            }
        }

        public bool IsSearch
        {
            get
            {
                return _IsSearch;
            }

            set
            {
                _IsSearch = value;
            }
        }

        public string RedirectFirstSearch
        {
            get
            {
                //return _RedirectFirstSearch;
                if (uRL == null)
                    uRL = new Uri(HttpContext.Current.Request.Url.ToString());
                UriBuilder builder = new UriBuilder(uRL.ToString());
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["s"] = Encrypt(_ValueSearch);
                query["page"] = 0.ToString();
                builder.Query = query.ToString();
                return builder.ToString();
            }

            set
            {
                _RedirectFirstSearch = value;
            }
        }

        
        #endregion

        #region function

        private void SetStatusProperties()
        {

            try
            {
                PageIndex = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"].ToString()) ? 0 : Convert.ToInt32(HttpContext.Current.Request.QueryString["page"].ToString());
            }
            catch { PageIndex = 0; }
            try
            {
                _ValueSearch = Decrypt(HttpContext.Current.Request.QueryString["s"].ToString());
            }
            catch { _ValueSearch = _where; }

        }
        private string getParam(string valueString, string paramName)
        {
            string[] tu = valueString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in tu)
            {
                if (item.Substring(0, paramName.Length + 1) == paramName + "=")
                {
                    return item.Substring(paramName.Length + 1);
                }
            }

            return "";
        }
        private void RenderPages()
        {
            int ax = 1;
            if (totalsRecord % pageSize == 0) ax = totalsRecord / pageSize;
            else ax = (totalsRecord / pageSize) + 1;
            _LabelTotal = "Page " +(PageIndex+1).ToString()+ "/"+ ax.ToString() +  "(" +totalsRecord.ToString()+ "records)";
        }

        private string RenderLabelTotalsRecord()
        {
            RenderPages();
            int TP = totalsRecord % pageSize == 0 ? totalsRecord / pageSize : (totalsRecord / pageSize) + 1;
            int TR = totalsRecord;
            int PS = pageSize;
            int PI = pageIndex;
            int NVP = numberViewPage;
            int TB = NVP % 2 == 0 ? NVP / 2 : (NVP - 1) / 2;
            int start = PI - TB;
            int fnish = PI + TB;
            while (start < 1) { start++; fnish++; }
            while (fnish > TP) { fnish--; start--; }
            start = start < 1 ? 1 : start;
            fnish = fnish < 1 ? 1 : fnish;
            _DisplayPage = ResulfPaging(start, fnish, PI);
            return $"<div style=\"display: block;\">{_DisplayPage}</div>";
        }
        private string ResulfPaging(int start, int fnish, int index)
        {
            if (start == 0)
            {
                start++;fnish++;index++;
            }
              if (uRL == null)
                uRL = new Uri(HttpContext.Current.Request.Url.ToString());
            UriBuilder builder = new UriBuilder(uRL.ToString());
            var query = HttpUtility.ParseQueryString(builder.Query);
            // where
            //if (_ValueSearch != _where && _ValueSearch != null)
            query["s"] = Encrypt(_ValueSearch);
            string kq = "";
            string clsCSS = "page";
            kq += $"<ul class=\"pagination\">";
            kq += $"<li style=\"float:left;margin-top: 15px;padding-right: 10px;\">{_LabelTotal}  {_DisplayPage}</li>";
            int TB = numberViewPage % 2 == 0 ? numberViewPage / 2 : (numberViewPage - 1) / 2;
            if (index - TB > 0)
            {
                query["page"] = "0";
                builder.Query = query.ToString();
                kq += $"<li class=\"paginate_button previous\" aria-controls=\"dynamic-table\"><a href=\"{builder.ToString()}\"><<</a></li>";
            }
            for (int i = start; i <= fnish; i++)
            {
                query["page"] = (i-1).ToString();
                uRL = builder.Uri;
                if (i == index+1) clsCSS = "paginate_button active";
                else clsCSS = "paginate_button";
                builder.Query = query.ToString();
                kq += $"<li class=\"{clsCSS}\" aria-controls=\"dynamic-table\">";
                kq += $"<a href=\"{builder.ToString()}\">{i}</a>";
                kq += "</li>";
            }
            int TP = totalsRecord % pageSize == 0 ? totalsRecord / pageSize : (totalsRecord / pageSize) + 1;
            if (index + TB < TP)
            {
                query["page"] = (TP-1).ToString();
                uRL = builder.Uri;
                builder.Query = query.ToString();
                kq += "<li class=\"paginate_button next\" aria-controls=\"dynamic-table\">";

                kq += $"<a class=\"page\" href=\"{builder.ToString()}\">>></a>";
                kq += "</li>";
            }
            kq += "</ul>";
            return kq;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        #endregion function
        #region override        


        protected override void Render(HtmlTextWriter writer)
        {
            if (totalsRecord == 0)
            {
                writer.Write("");
                RenderChildren(writer);
            }
            else
            {
                writer.Write(RenderLabelTotalsRecord());
                RenderChildren(writer);
            }

        }
        public override string ToString()
        {
            return _LabelTotal + " " + _DisplayPage;
        }
        #endregion


        #region new
        
        #endregion
    }

}
