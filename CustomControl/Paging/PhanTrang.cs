using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControl
{
    [ToolboxData("<{0}:PhanTrang runat=\"server\" PageSize=10></{0}:PhanTrang>")]
    public class PhanTrang : CompositeControl
    {

        #region properties
        private bool isStartVisible = true;
        private bool isFinishVisible = true;
        private int _NumBerViewPage;
        [Bindable(true), Category("Appearance"), DefaultValue(7), Description("Số lượng phân trang hiển thị.")]
        public int NumberViewPage
        {
            get
            {
                EnsureChildControls();
                return (_NumBerViewPage == 0) ? 7 : _NumBerViewPage;
            }
            set
            {
                EnsureChildControls();
                _NumBerViewPage = value;
                _eventChangeIndex(PageIndex);
            }
        }

        private int _pageSize;
        [Bindable(true), Category("Appearance"), DefaultValue(10), Description("Số lượng bản ghi trong 1 trang.")]
        public int PageSize
        {
            get
            {
                EnsureChildControls();                
                return _pageSize == 0 ? 10 : _pageSize;
            }
            set
            {
                EnsureChildControls();
                _pageSize = value;
                _eventChangeIndex(PageIndex);
            }
        }
        private int _pageIndex;
        [Bindable(true), Category("Appearance"), DefaultValue(0), Description("Trang hiện tại.")]
        public int PageIndex
        {
            get
            {
                EnsureChildControls();
                return (_pageIndex == 0) ? 0 : _pageIndex;
            }
            set
            {
                EnsureChildControls();
                _pageIndex = value;
                _eventChangeIndex(PageIndex);
            }
        }
        private int _totalRecord;
        [Bindable(true), Category("Appearance"), DefaultValue(0), Description("Tổng số bản ghi.")]
        public int TotalRecord
        {
            get
            {
                EnsureChildControls();
                return (_totalRecord == 0) ? 0 : _totalRecord;
            }
            set
            {
                EnsureChildControls();
                _totalRecord = value;
                _eventChangeIndex(PageIndex);
            }
        }
        private LinkButton[] lisPage;
        private LinkButton likS;
        private LinkButton likF;
        #endregion

        #region function
        private void CreatePage()
        {
            if (TotalRecord == 0)
                return;
            int TP = TotalRecord % PageSize == 0 ? TotalRecord / PageSize : (TotalRecord / PageSize) + 1;
            int TR = TotalRecord;
            int PS = PageSize;
            int PI = PageIndex;
            int NVP = NumberViewPage;
            int TB = NVP % 2 == 0 ? NVP / 2 : (NVP - 1) / 2;
            int start = PI - TB;
            int fnish = PI + TB;
            while (start < 1) { start++; fnish++; }
            while (fnish > TP) { fnish--; start--; }
            start = start < 1 ? 1 : start;
            fnish = fnish < 1 ? 1 : fnish;
            if ((fnish - start) > TP)
                fnish = start + NVP;
            lisPage = new LinkButton[(fnish - start) + 1];
            for (int i = start; i <= fnish; i++)
            {
                LinkButton li = new LinkButton();
                li.Text = i.ToString();
                li.ID = "lik" + i.ToString();
                li.CommandArgument = (i - 1).ToString();
                li.Click += new EventHandler(_linkbuttom_Click);
                lisPage[(i - start)] = li;

            }
        }
        #endregion
        #region event
        [Category("Action")]
        public event EventHandler Paging_IndexChange;
        private void _linkbuttom_Click(object source, EventArgs e)
        {
            LinkButton li = (LinkButton)source;
            //PageIndex = Convert.ToInt32(li.CommandArgument);
            PageIndex = Convert.ToInt32(li.Attributes["data-pageIndex"].ToString());
            if (Paging_IndexChange != null)
            {
                Paging_IndexChange(this, e);
            }
            _eventChangeIndex(PageIndex);
        }
        private void _eventChangeIndex(int iPageIndex)
        {
            if (TotalRecord == 0) return;
            int TP = TotalRecord % PageSize == 0 ? TotalRecord / PageSize : (TotalRecord / PageSize) + 1;
            int TR = TotalRecord;
            int PS = PageSize;
            int PI = iPageIndex;
            int NVP = NumberViewPage;
            int TB = NVP % 2 == 0 ? NVP / 2 : (NVP - 1) / 2;
            int start = PI - TB;
            int fnish = PI + TB;
            while (start < 1) { start++; fnish++; }
            while (fnish > TP) { fnish--; start--; }
            start = start < 1 ? 1 : start;
            fnish = fnish < 1 ? 1 : fnish;
            if ((fnish - start) >= TP)
                fnish = start + NVP;

            for (int i = start; i <= fnish; i++)
            {
                lisPage[(i - start)].CommandArgument = (i - 1).ToString();
                lisPage[(i - start)].Text = (i).ToString();
                lisPage[(i - start)].Attributes.Add("data-pageIndex", (i - 1).ToString());
                lisPage[(i - start)].Visible = true;                
            }
            likF.CommandArgument = (TP - 1).ToString();
            likF.Attributes.Add("data-pageIndex", (TP - 1).ToString());
        }
        #endregion
        #region override
        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            likS = new LinkButton();
            likS.Text = "<<";
            likS.ID = "likStart";
            likS.CommandArgument = "0";
            likS.Attributes.Add("data-pageIndex", (0).ToString());
            likS.Click += new EventHandler(_linkbuttom_Click);
            likF = new LinkButton();
            likF.Text = ">>";
            likF.ID = "likFinish";
            likF.Click += new EventHandler(_linkbuttom_Click);
            lisPage = new LinkButton[NumberViewPage];
            for (int i = 0; i < lisPage.Length; i++)
            {
                LinkButton li = new LinkButton();
                li.Text = i.ToString();
                li.ID = "lik" + i.ToString();
                li.CommandArgument = (i - 1).ToString();
                li.Click += new EventHandler(_linkbuttom_Click);
                li.Attributes.Add("data-pageIndex", (i - 1).ToString());
                lisPage[i] = li;
                lisPage[i].Visible = false;
                this.Controls.Add(lisPage[i]);
            }
            this.Controls.Add(likS);
            this.Controls.Add(likF);
            if (TotalRecord == 0) return;
            if (PageSize == 0) return;
            int TP = TotalRecord % PageSize == 0 ? TotalRecord / PageSize : (TotalRecord / PageSize) + 1;            
            likF.CommandArgument = (TP - 1).ToString();            
            likF.Attributes.Add("data-pageIndex", (TP - 1).ToString());
            _eventChangeIndex(PageIndex);
                        
        }



        protected override void Render(HtmlTextWriter writer)
        {
            CreateChildControls();


            if (TotalRecord == 0 || PageSize==0)
            {
                writer.Write("0 record");
                return;
            }
            int TP = TotalRecord % PageSize == 0 ? TotalRecord / PageSize : (TotalRecord / PageSize) + 1;
            if (TP < 2)
            {
                writer.Write($"{TotalRecord} record");
                return;
            }
            string clsCSS = "page";
            writer.Write($"<ul id={UniqueID} class=\"pagination\">");
            writer.Write("<li style=\"float:left;margin-top: 15px;padding-right: 10px;\">");
            writer.Write($"Page {PageIndex+1}/{TP}({TotalRecord} records)");
            writer.Write("</li>");
            if (isStartVisible)
            {
                writer.Write("<li  class=\"paginate_button previous\" aria-controls=\"dynamic-table\">");
                likS.RenderControl(writer);
                writer.Write("</li>");
            }
            for (int i = 0; i < lisPage.Length; i++)
            {
                int act = Convert.ToInt32(lisPage[i].CommandArgument);
                if (act == PageIndex) clsCSS = "paginate_button active";
                else clsCSS = "paginate_button";
                writer.Write($"<li class=\"{clsCSS}\" aria-controls=\"dynamic-table\">");
                lisPage[i].RenderControl(writer);
                writer.Write("</li>");
            }
            if (isFinishVisible)
            {
                writer.Write("<li class=\"paginate_button next\" aria-controls=\"dynamic-table\">");
                likF.RenderControl(writer);
                writer.Write("</li>");
            }
            writer.Write("</ul");

        }

        #endregion
    }
}

