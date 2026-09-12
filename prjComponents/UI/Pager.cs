using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;

using System.ComponentModel;

namespace prjComponents.UI.WebControls
{
	
	public class Pager : Label, INamingContainer 
	{
		#region Member variables and constructor
        //PortalSettings portal = (PortalSettings)HttpContext.Current.Application["PortalSettings"];
		LinkButton previousButton;
		LinkButton nextButton;
		LinkButton firstButton;
		LinkButton lastButton;

		//ImageButton previousButton;
//		ImageButton nextButton;
//		ImageButton firstButton;
//		ImageButton lastButton;
		LinkButton[] pagingLinkButtons;
		
		public override ControlCollection Controls 
		{
			get 
			{
				EnsureChildControls();
				return base.Controls;
			}

		}
		// *********************************************************************
		//  CreateChildControls
		//
		/// <summary>
		/// This event handler adds the children controls and is resonsible
		/// for determining the template type used for the control.
		/// </summary>
		/// 
		// ********************************************************************/ 
		protected override void CreateChildControls() 
		{
			Controls.Clear();

			// Add Page buttons
			//
			AddPageButtons();

			// Add Previous Next child controls
			//
			AddPreviousNextLinkButtons();

			// Add First Last child controls
			//
			AddFirstLastLinkButtons();

		}
		#endregion

		#region Render functions
		protected override void Render(HtmlTextWriter writer) 
		{

			int totalPages = CalculateTotalPages();

			// Do we have data?
			//
			if (totalPages <= 1)
				return;

			AddAttributesToRender(writer);


			// Render the paging buttons
			//
			writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, false);
            //writer.RenderBeginTag(HtmlTextWriterTag.Div);
            
			// Render the first button
			//
			RenderFirst(writer);

			// Render the previous button
			//
			RenderPrevious(writer);
            
			// Render the page button(s)
			//
			RenderPagingButtons(writer);

			// Render the next button
			//
			RenderNext(writer);

			// Render the last button
			//
			RenderLast(writer);

			//writer.RenderEndTag();

		}

		void RenderFirst (HtmlTextWriter writer) 
		{

			int totalPages = CalculateTotalPages();

			if ((PageIndex >= 3) && (totalPages > 5)) 
			{
				firstButton.RenderControl(writer);

				LiteralControl l = new LiteralControl("&nbsp;...&nbsp;");
				l.RenderControl(writer);
			}
		}

		void RenderLast (HtmlTextWriter writer) 
		{

			int totalPages = CalculateTotalPages();

			if (((PageIndex + 3) < totalPages) && (totalPages > 5)) 
			{
				LiteralControl l = new LiteralControl("&nbsp;...&nbsp;");
				l.RenderControl(writer);

				lastButton.RenderControl(writer);
			}

		}

		void RenderPrevious (HtmlTextWriter writer) 
		{
			Literal l;

			if (HasPrevious) 
			{
				previousButton.RenderControl(writer);

				l = new Literal();
				l.Text = "&nbsp;";
				l.RenderControl(writer);
			}

		}

		void RenderNext(HtmlTextWriter writer) 
		{
			Literal l;

			if (HasNext) 
			{

				l = new Literal();
				l.Text = "&nbsp;";
				l.RenderControl(writer);

				nextButton.RenderControl(writer);
			}

		}
        /// <summary>
        /// Luu y tai day
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="writer"></param>
		void RenderButtonRange(int start, int end, HtmlTextWriter writer,bool _zen) 
		{            
            
			for (int i = start; i < end; i++) 
			{

				// Are we working with the selected index?
				//
                if (_zen == false)
                {
                    if (PageIndex == i)
                    {

                        // Render different output
                        //
                        Literal l = new Literal();
                        l.Text = "<span class=\"current\">" + (i + 1).ToString() + "</span>";
                        //l.Text = (i + 1).ToString();

                        l.RenderControl(writer);
                    }
                    else
                    {
                        pagingLinkButtons[i].RenderControl(writer);
                    }
                    
                }
                else
                {
                    if (PageIndex ==(i+1))
                    {
                        // Render different output
                        //
                        Literal l = new Literal();
                        l.Text = "<span class=\"current\">" + (i + 1).ToString() + "</span>";
                        //l.Text = (i + 1).ToString();

                        l.RenderControl(writer);
                    }
                    else
                    {
                        pagingLinkButtons[i].RenderControl(writer);
                    }
                    
                }
                if (i < (end - 1))
                    writer.Write(" ");

			}

		}

		void RenderPagingButtons(HtmlTextWriter writer) 
		{
			int totalPages;

			// Get the total pages available
			//
			totalPages = CalculateTotalPages();

			// If we have <= 5 pages display all the pages and exit
			//
			if ( totalPages <= 5) 
			{
                RenderButtonRange(0, totalPages, writer,false);                
			} 
			else 
			{

				int lowerBound = (PageIndex - 2);
				int upperBound = (PageIndex + 3);

				if (lowerBound <= 0) 
					lowerBound = 0;

				if (PageIndex == 0)
					RenderButtonRange(0, 5, writer,false);

				else if (PageIndex == 1)
					RenderButtonRange(0, (PageIndex + 4), writer,false);

				else if (PageIndex < (totalPages - 2))
					RenderButtonRange(lowerBound, (PageIndex + 3), writer,false);

				else if (PageIndex == (totalPages - 2))
					RenderButtonRange((totalPages - 5), (PageIndex + 2), writer,false);

				else if (PageIndex == (totalPages - 1))
					RenderButtonRange((totalPages - 5), (PageIndex + 1), writer,false);
                ////Ad by nvthai
                else if (PageIndex == (totalPages))
                    RenderButtonRange((totalPages - 5), (PageIndex), writer, true);
                else
                {
                    if (totalPages < PageIndex)
                    {
                        PageIndex = totalPages;
                        RenderButtonRange((totalPages - 5), (PageIndex), writer, true);
                    }
                }
			}
		}
		#endregion

		#region ControlTree functions
		void AddPageButtons() 
		{

			// First add the lower page buttons
			//
			pagingLinkButtons = new LinkButton[CalculateTotalPages()];

			// Create the buttons and add them to 
			// the Controls collection
			//
			for (int i = 0; i < pagingLinkButtons.Length; i++) 
			{

				pagingLinkButtons[i] = new LinkButton();
				pagingLinkButtons[i].EnableViewState = false;
				pagingLinkButtons[i].Text = (i + 1).ToString();
				pagingLinkButtons[i].ID = i.ToString();
				pagingLinkButtons[i].CommandArgument = i.ToString();
				pagingLinkButtons[i].Click += new System.EventHandler(PageIndex_Click);
				//pagingLinkButtons[i].CssClass="Pages";
				Controls.Add(pagingLinkButtons[i]);

			}


		}

		void AddFirstLastLinkButtons() 
		{

			// Create the previous button if necessary
			//
			firstButton = new LinkButton();
			//firstButton = new ImageButton(); 
			firstButton.ID = "First";
            firstButton.Text = CommonLib.ReadXML("Utility_Pager_firstButton");//"<img border='0' src=\"" + Globals.GetSkinPath() + "/images/navigate_left2.png\">";
			//

			//firstButton.AlternateText = ResourceManager.GetString("Utility_Pager_firstButton");
			//firstButton.ImageUrl= Globals.GetSkinPath() +  "/images/navigate_left2.png";
			firstButton.CommandArgument = "0";
			firstButton.Click += new System.EventHandler(PageIndex_Click);
			//firstButton.Click +=new ImageClickEventHandler(PageIndexImages_Click);
			//firstButton.CssClass="Pages";
			Controls.Add(firstButton);

			// Create the next button if necessary
			//
			lastButton = new LinkButton();
			//lastButton = new ImageButton(); 
			lastButton.ID = "Last";
            lastButton.Text = CommonLib.ReadXML("Utility_Pager_lastButton");//"<img border='0' src=\"" + Globals.GetSkinPath() + "/images/navigate_right2.png\">";
				//
			//lastButton.AlternateText = ResourceManager.GetString("Utility_Pager_lastButton");
			//lastButton.ImageUrl= Globals.GetSkinPath() +  "/images/navigate_right2.png";
			lastButton.CommandArgument = (CalculateTotalPages() - 1).ToString();

			lastButton.Click += new System.EventHandler(PageIndex_Click);
			//lastButton.Click +=new ImageClickEventHandler(PageIndexImages_Click);
			//lastButton.CssClass="Pages";
			Controls.Add(lastButton);

		}

		void AddPreviousNextLinkButtons() 
		{

			// Create the previous button if necessary
			//
			previousButton = new LinkButton();
			previousButton.ID = "Prev";
            previousButton.Text = CommonLib.ReadXML("Utility_Pager_previousButton");
			

			//previousButton.AlternateText = ResourceManager.GetString("Utility_Pager_previousButton");
			//previousButton.ImageUrl= Globals.GetSkinPath() +  "/images/navigate_left.png";
			previousButton.CommandArgument = (PageIndex - 1).ToString();
			previousButton.Click += new System.EventHandler(PageIndex_Click);
			//previousButton.Click +=new ImageClickEventHandler(PageIndexImages_Click);
			//previousButton.CssClass="Pages";
			Controls.Add(previousButton);

			// Create the next button if necessary
			//
			nextButton = new LinkButton();
			//nextButton = new ImageButton();
			nextButton.ID = "Next";
            nextButton.Text = CommonLib.ReadXML("Utility_Pager_nextButton");
			//nextButton.AlternateText = ResourceManager.GetString("Utility_Pager_nextButton");
			//nextButton.ImageUrl= Globals.GetSkinPath() +  "/images/navigate_right.png";
			nextButton.CommandArgument = (PageIndex + 1).ToString();
			nextButton.Click += new System.EventHandler(PageIndex_Click);
			//nextButton.Click +=new ImageClickEventHandler(PageIndexImages_Click);
			//nextButton.CssClass="Pages";
			Controls.Add(nextButton);

		}
		#endregion

		#region Private Properties
		private bool HasPrevious 
		{
			get 
			{
				if (PageIndex > 0)
					return true;

				return false;
			}
		}

		private bool HasNext 
		{
			get 
			{
				if (PageIndex + 1 < CalculateTotalPages() )
					return true;

				return false;
			}
		}
		#endregion

		#region Events
		// *********************************************************************
		//  IndexChanged
		//
		/// <summary>
		/// Event raised when a an index has been selected by the end user
		/// </summary>
		/// 
		// ********************************************************************/
		public event System.EventHandler IndexChanged;

		// *********************************************************************
		//  PageIndex_Click
		//
		/// <summary>
		/// Event raised when a new index is selected from the paging control
		/// </summary>
		/// 
		// ********************************************************************/
		
		void PageIndexImages_Click(object sender, ImageClickEventArgs e) 
		{

				PageIndex = Convert.ToInt32(((ImageButton) sender).CommandArgument);

				if (null != IndexChanged)
					IndexChanged(sender, e);

			}
		void PageIndex_Click(Object sender, EventArgs e) 
		{

			PageIndex = Convert.ToInt32(((LinkButton) sender).CommandArgument);

			if (null != IndexChanged)
				IndexChanged(sender, e);

		}
		#endregion

		#region Helper methods and Public Properties
		// *********************************************************************
		//  CalculateTotalPages
		//
		/// <summary>
		/// Static that caculates the total pages available.
		/// </summary>
		/// 
		// ********************************************************************/
		public int CalculateTotalPages() 
		{
			int totalPagesAvailable;

			if (TotalRecords == 0)
				return 0;

			// First calculate the division
			//
			totalPagesAvailable = TotalRecords / PageSize;

			// Now do a mod for any remainder
			//
			if ((TotalRecords % PageSize) > 0)
				totalPagesAvailable++;

			return totalPagesAvailable;

		}

		public int PageIndex 
		{
            get
            {
                int _pageIndex = 0;

                //if ((!Page.IsPostBack) && (.PageIndex > 0))
                //    _pageIndex = portal.PageIndex;
                //else
                //if (Convert.ToInt32(ViewState["PageIndex"]) > CalculateTotalPages())
                //{
                //    _pageIndex = CalculateTotalPages() - 1;
                //    //ViewState["PageIndex"] = _pageIndex
                //}
                //else
                _pageIndex = Convert.ToInt32(ViewState["PageIndex"]);


                if (_pageIndex < 0)
                    return 0;
                else
                    return _pageIndex;
            }
            set
            {
                ViewState["PageIndex"] = value;
            }

           
		}

		public int PageSize 
		{
			get 
			{
				int pageSize = Convert.ToInt32(ViewState["PageSize"]);

				//if (pageSize == 0) 
//				{
//					if (_context.PostID > 0) 
//					{
//						return Globals.GetSiteSettings().PostsPerPage;
//					} 
//					else 
//					{
//						return Globals.GetSiteSettings().ThreadsPerPage;
//					}
//				}

				return pageSize;
			}
			set 
			{
				ViewState["PageSize"] = value;
			}

		}

		public int TotalRecords 
		{
			get 
			{
				return Convert.ToInt32(ViewState["TotalRecords"]);
			}
			set 
			{
				ViewState["TotalRecords"] = value;
			}
		}
		#endregion		
	}
}