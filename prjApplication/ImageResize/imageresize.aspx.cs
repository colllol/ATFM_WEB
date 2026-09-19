using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging; 
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace prjApplication.ImageResize
{
	/// <summary>
	/// Summary description for viewresize.
	/// </summary>
	public class imageresize : System.Web.UI.Page
	{
		int Width =0;
		int Height=0;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			string physPath= Request.Params["imgPath"];
			if(Request.Params["height"] != null)
			{
				try
				{
					Height = int.Parse(Request.Params["height"]);
				}
				catch
				{
					Height = 0;
				}
			} 
			if(Request.Params["width"] != null)
			{
				try
				{
					Width = int.Parse(Request.Params["width"]);
				}
				catch
				{
					Width = 0;
				}
			} 	
			int _path =physPath.IndexOf(":");
			if (_path<1)
				physPath =Server.MapPath("../" + physPath);
			
			//Request.ServerVariables.Item("APPL_PHYSICAL_PATH ");
			Response.Write (physPath);
			ViewResize(physPath,Width,Height);
		}

		private void ViewResize(string ImagePath,int width, int height)
		{
			string path = ImagePath;
			System.Drawing.Image objThumbnail;	
			System.Drawing.Image objImage;
			int imgwidth=0;
			int imgheight=0;							
			decimal lnRatio;
			try
			{
				objImage =	System.Drawing.Image.FromFile(path);				

				if (objImage.Width <= Width && objImage.Height <= Height )
				{
					imgwidth= objImage.Width;
					imgheight= objImage.Height;
				}
				else if (objImage.Width > objImage.Height)
				{
					lnRatio = (decimal) Width / objImage.Width;
					imgwidth=Width;
					decimal lnTemp = objImage.Height * lnRatio;
					imgheight = (int) lnTemp;
				}
				else
				{
					lnRatio = (decimal) Height / objImage.Height;
					imgheight=Height;
					decimal lnTemp = objImage.Width * lnRatio;
					imgwidth = (int) lnTemp;
							
				}
				// Create thumbnail
				objThumbnail = objImage.GetThumbnailImage(imgwidth,imgheight, null, System.IntPtr.Zero);
				Response.Clear();
				Response.ContentType = "image/jpeg";
				objThumbnail.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);				
				objImage.Dispose();
				objThumbnail.Dispose();
				Response.End();
			}
			catch{}		
		}

		string getContentType(String path)
		{
			switch (Path.GetExtension(path)) 
			{
				case ".bmp": return "Image/bmp";
				case ".gif": return "Image/gif";
				case ".jpg": return "Image/jpeg";
				case ".png": return "Image/png";
				case ".tif": return "Image/tiff";
				default : break;
			}
			return "";
		}
		ImageFormat getImageFormat(String path)
		{
			switch (Path.GetExtension(path)) 
			{
				case ".bmp": return ImageFormat.Bmp;
				case ".gif": return ImageFormat.Gif;
				case ".jpg": return ImageFormat.Jpeg;
				case ".png": return ImageFormat.Png;
				case ".tif": return ImageFormat.Tiff ;
				default : break;
			}
			return ImageFormat.Jpeg;
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
