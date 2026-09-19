using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;

namespace CustomControl
{
    [ToolboxData("<{0}:PagingForNet runat=server NumberViewPage=7 PageSize=20 ></{0}:PagingForNet>")]
    public class PagingForNet : WebControl, INamingContainer
    {
        private static readonly object EventSubmitKey =
            new object();
        public PagingForNet()
        {
            lis = new List<LinkButton>();
            S = new LinkButton();
            S.Text = "<<";
            S.Click += new EventHandler(ButtonPaging_Onclick);
            S.CommandArgument = "0";
            S.ID = "likStart";


            for (int i = 1; i <= NumberViewPage; i++)
            {
                LinkButton btn = new LinkButton();
                btn.ID = "lik" + i.ToString();
                //btn.Text = (i + 1).ToString();
                btn.Click += new EventHandler(ButtonPaging_Onclick);
                lis.Add(btn);

            }
            F = new LinkButton();
            F.Text = ">>";
            F.Click += new EventHandler(ButtonPaging_Onclick);
            //F.CommandArgument = "100";
            F.ID = "likFinish";

        }
        #region properties
        private bool _isFirst = false;
        private bool _isEnd = false;
        private int _pageIndex;
        private int _pageSize;
        private int _totalRecord;
        private int _numberViewPage;
        private List<LinkButton> lis;
        private LinkButton S;
        private LinkButton F;
        private List<object> _Control = new List<object>();
        [Category("Appearance"), Bindable(true)]
        public int NumberViewPage
        {
            get
            {
                //EnsureChildControls();
                if (_numberViewPage < 1)
                    return 7;
                return _numberViewPage;
            }

            set
            {
                if (value != 0)
                {
                    //EnsureChildControls();
                    _numberViewPage = value;
                }
            }
        }
        [Category("Appearance"), Bindable(true)]
        public int TotalRecord
        {
            get
            {
                //EnsureChildControls();
                return _totalRecord;
            }

            set
            {
                if (value != 0)
                {
                    //EnsureChildControls();
                    _totalRecord = value;
                }
            }
        }
        [Category("Appearance"), Bindable(true)]
        public int PageSize
        {
            get
            {
                //EnsureChildControls();
                return _pageSize;
            }

            set
            {
                if (value != 0)
                {
                    //EnsureChildControls();
                    _pageSize = value;
                }

            }
        }
        [Category("Appearance"), Bindable(true)]
        public int PageIndex
        {
            get
            {
                EnsureChildControls();
                return _pageIndex;
            }

            set
            {
                if (value != 0)
                {
                    EnsureChildControls();
                    _pageIndex = value;
                }

            }
        }
        #endregion
        private List<LinkButton> _s = new List<LinkButton>();
        #region function
        void RenderControlHtml()
        {

            if (TotalRecord == 0) return;
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
            CreatHtmlControl(start, fnish, PI);
        }
        void CreatHtmlControl(int start, int fnish, int index)
        {
            Controls.Clear();
            _Control = new List<object>();
            if (TotalRecord == 0) return;
            if (start == 0)
            {
                start++; fnish++; index++;
            }
            string clsCSS = "page";
            _Control.Add(new Literal() { Text = "<ul class=\"pagination\">\r\n1" });
            _Control.Add(new Literal() { Text = "<li style=\"float:left;margin-top: 15px;padding-right: 10px;\">\r\naa<li>" });
            int TB = NumberViewPage % 2 == 0 ? NumberViewPage / 2 : (NumberViewPage - 1) / 2;

            if (index - TB > 0)
            {
                Literal li = new Literal();
                li.Text = "<li class=\"paginate_button previous\" aria-controls=\"dynamic-table\">\r\n aaaaaa";
                _Control.Add(li);
                S.Text = "<<";
                S.Attributes.Add("data-paging", "0");
                _Control.Add(S);
                _Control.Add(new Literal() { Text = "</li>" });
            }


            for (int i = start; i <= fnish; i++)
            {
                if (i == index + 1) clsCSS = "paginate_button active";
                else clsCSS = "paginate_button";
                _Control.Add(new Literal() { Text = $"<li class=\"{clsCSS}\" aria-controls=\"dynamic-table\"> \r\n axc" });
                lis[(i - start)].Attributes.Add("data-paging", (i-1).ToString());
                lis[(i - start)].Text = i.ToString();
                lis[(i - start)].CommandArgument = (i-1).ToString();
                _Control.Add(lis[(i - start)]);
                _Control.Add(new Literal() { Text = "</li>" });
            }
            int TP = TotalRecord % PageSize == 0 ? TotalRecord / PageSize : (TotalRecord / PageSize) + 1;
            if (index + TB < TP)
            {
                _isEnd = true;
            _Control.Add(new LiteralControl("<li class='paginate_button next' aria-controls='dynamic-table'>"));
            LinkButton lik = new LinkButton();
            //lik.ID = "likFinish";
            //lik.CommandArgument = TP.ToString();
            //lik.Attributes.Add("data-paging", TP.ToString()); 
            //lik.Click += new EventHandler(ButtonPaging_Onclick);
            //lik.Text = ">>";
            F.Attributes.Add("data-paging", (TP-1).ToString());
            F.CommandArgument = (TP-1).ToString();
            F.Text = ">>";
            F.Visible = true;
            _Control.Add(F);
            _Control.Add(new LiteralControl("</li>"));
            }
            else { F.Visible = false; }
            _Control.Add(new LiteralControl("</ul>"));
        }


        #endregion


        #region event

        public event EventHandler Paging_Click;
        protected void ButtonPaging_Onclick(object sender, EventArgs e)
        {
            //EnsureChildControls();
            Page.Response.Write(": " + PageIndex + " : __");
            if (sender is Button)
                PageIndex = Convert.ToInt32(((Button)sender).Attributes["data-paging"]);
            if (sender is LinkButton)
            {
                var ax = (LinkButton)sender;
                var bx = ax.CommandArgument;
                var cx = ax.Attributes["data-paging"];
                PageIndex = Convert.ToInt32(cx);
            }            
            CreateChildControls();
            if (Paging_Click != null)
            {
                Paging_Click(this, e);
            }
            //CreateChildControls();
        }


        protected override void Render(HtmlTextWriter writer)
        {
            ////EnsureChildControls();    
            foreach (object item in Controls)
            {
                if (item is LiteralControl)
                    ((LiteralControl)item).RenderControl(writer);
                if (item is LinkButton)
                    ((LinkButton)item).RenderControl(writer);
                if (item is Button)
                    ((Button)item).RenderControl(writer);
                if (item is Literal)
                    ((Literal)item).RenderControl(writer);
            }

        }


        //protected override void RecreateChildControls()
        //{
        //    EnsureChildControls();
        //}
        protected override void CreateChildControls()
        {
            Controls.Clear();
            _Control.Clear();
            RenderControlHtml();
            foreach (object item in _Control)
            {
                if (item is LiteralControl)
                    this.Controls.Add((LiteralControl)item);
                if (item is LinkButton)
                    this.Controls.Add((LinkButton)item);
                if (item is Button)
                    this.Controls.Add((Button)item);
                if (item is Literal)
                    this.Controls.Add(((Literal)item));
            }

        }



        #endregion

    }
    [ToolboxData("<{0}:CustomCalendar runat=server></{0}:CustomCalendar>")]
    public class CustomCalendar : CompositeControl
    {
        TextBox textBox;
        ImageButton imageButton;
        Calendar calendar;

        protected override void CreateChildControls()
        {
            Controls.Clear();

            textBox = new TextBox();
            textBox.ID = "dateTextBox";
            textBox.Width = Unit.Pixel(80);

            imageButton = new ImageButton();
            imageButton.ID = "calendarImageButton";
            imageButton.Click += new ImageClickEventHandler(imageButton_Click);

            calendar = new Calendar();
            calendar.ID = "calendarControl";
            calendar.SelectionChanged += new EventHandler(calendar_SelectionChanged);
            calendar.Visible = false;

            this.Controls.Add(textBox);
            this.Controls.Add(imageButton);
            this.Controls.Add(calendar);
        }

        void calendar_SelectionChanged(object sender, EventArgs e)
        {
            textBox.Text = calendar.SelectedDate.ToShortDateString();

            DateSelectedEventArgs dateSelectedEventData = new DateSelectedEventArgs(calendar.SelectedDate);
            OnDateSelection(dateSelectedEventData);

            calendar.Visible = false;
        }

        void imageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (calendar.Visible)
            {
                calendar.Visible = false;
            }
            else
            {
                calendar.Visible = true;
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    calendar.VisibleDate = DateTime.Today;
                }
                else
                {
                    DateTime output = DateTime.Today;
                    bool isDateTimeConverionSuccessful = DateTime.TryParse(textBox.Text, out output);
                    calendar.VisibleDate = output;
                }
            }
        }

        [Category("Appearance")]
        [Description("Sets the image icon for the calendar control")]
        public string ImageButtonImageUrl
        {
            get
            {
                EnsureChildControls();
                return imageButton.ImageUrl != null ? imageButton.ImageUrl : string.Empty;
            }
            set
            {
                EnsureChildControls();
                imageButton.ImageUrl = value;
            }
        }

        [Category("Appearance")]
        [Description("Gets or sets the selected date of custom calendar control")]
        public DateTime SelectedDate
        {
            get
            {
                EnsureChildControls();
                return string.IsNullOrEmpty(textBox.Text) ? DateTime.MinValue : Convert.ToDateTime(textBox.Text);
            }

            set
            {
                if (value != null)
                {
                    EnsureChildControls();
                    textBox.Text = value.ToShortDateString();
                }
                else
                {
                    EnsureChildControls();
                    textBox.Text = "";
                }
            }
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            textBox.RenderControl(writer);
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            imageButton.RenderControl(writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();

            calendar.RenderControl(writer);
        }

        public event DateSelectedEventHandler DateSelected;

        protected virtual void OnDateSelection(DateSelectedEventArgs e)
        {
            if (DateSelected != null)
            {
                DateSelected(this, e);
            }
        }
    }

    public class DateSelectedEventArgs : EventArgs
    {
        private DateTime _selectedDate;

        public DateSelectedEventArgs(DateTime selectedDate)
        {
            this._selectedDate = selectedDate;
        }

        public DateTime SelectedDate
        {
            get
            {
                return this._selectedDate;
            }
        }
    }

    public delegate void DateSelectedEventHandler(object sender, DateSelectedEventArgs e);
}
