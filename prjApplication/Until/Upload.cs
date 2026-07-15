using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using Word;
using prjComponents;
using prjBusinessLogic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

/// <summary>
/// Upload handler for uploading files.
/// </summary>
public class Upload : IHttpHandler
{
    public Upload()
    {
    }
    #region IHttpHandler Members

    public bool IsReusable
    {
        get { return true; }
    }
    public string GetConfig()
    {
        return ConfigurationManager.AppSettings["ServerPathDis"].ToString();
    }
    public void ProcessRequest(HttpContext context)
    {
        Bitmap _obj;
        double _width, _height;
        // Example of using a passed in value in the query string to set a categoryId
        // Now you can do anything you need to witht the file.
        //int categoryId = 0;
        //if (!string.IsNullOrEmpty(context.Request.QueryString["CategoryID"]))
        //{
        //    int.TryParse(context.Request.QueryString["CategoryID"],out categoryId);
        //}
        //if (categoryId > 0)
        //{
        //}
        //string temp = context.Session["temp"].ToString();
        //string temp = context.Session["itemp"].ToString();
        // string temp = context.Session["temp"].ToString();

        string EncryptString = context.Request.QueryString["User"];
        // FormsAuthenticationTicket UserTicket = FormsAuthentication.Decrypt(EncryptString);

        if (EncryptString != "" && context.Request.Files.Count > 0)
        //if (context.Request.Files.Count > 0)
        {
            // get the applications path

            //Directory.CreateDirectory("E://LAM VIEC/Flex Builder/FlashUpload1/Item");
            //COMMENT BY BOCT
            //string uploadPath = context.Server.MapPath(context.Request.ApplicationPath + "/Upload");
            string TempFile = context.Server.MapPath("../upload/TempFile");
            if (Directory.Exists(TempFile) == false)
                Directory.CreateDirectory(TempFile);

            string vType = context.Request["s"];
            string vCheckDongDau = context.Request["sdongdau"];
            string uploadPath = "";
            if (vType == "1")
                uploadPath = DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString();
            if (vType == "2")
                uploadPath = prjComponents.Global.UploadPhotoAlbum + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString();
            if (vType == "3")
                uploadPath = prjComponents.Global.UploadPhotoEvent + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString();
            //string strRootPath = context.Server.MapPath(uploadPath);
            string strRootPath = ConfigurationManager.AppSettings["ServerPathDis"].ToString() + uploadPath;
            if (Directory.Exists(strRootPath) == false)
                Directory.CreateDirectory(strRootPath);
            // loop through all the uploaded files
            //string str = context.Server.MapPath();
            //Directory.CreateDirectory("D:/VNPMarket/Backend/upload/124");
            //string uploadPath = "D:/VNPMarket/Backend/upload/124";
            //Directory.CreateDirectory(
            for (int j = 0; j < context.Request.Files.Count; j++)
            {
                // get the current file
                HttpPostedFile uploadFile = context.Request.Files[j];
                // if there was a file uploded
                if (uploadFile.ContentLength > 0)
                {
                    // save the file to the upload directory

                    //use this if testing from a classic style upload, ie. 

                    // <form action="Upload.axd" method="post" enctype="multipart/form-data">
                    //    <input type="file" name="fileUpload" />
                    //    <input type="submit" value="Upload" />
                    //</form>

                    // this is because flash sends just the filename, where the above 
                    //will send the file path, ie. c:\My Pictures\test1.jpg
                    //you can use Test.thm to test this page.
                    //string filename = uploadFile.FileName.Substring(uploadFile.FileName.LastIndexOf("\\"));
                    //uploadFile.SaveAs(string.Format("{0}{1}{2}", tempFile, "Upload\\", filename));

                    // use this if using flash to upload
                    //uploadFile.SaveAs(Path.Combine(uploadPath, uploadFile.FileName));
                    string strtemp = DateTime.Now.Millisecond + uploadFile.FileName;
                    uploadFile.SaveAs(Path.Combine(TempFile, strtemp));
                    if (vCheckDongDau == "0")
                    {
                        // XU LY DONG DAU
                        char[] chfile = { '.' };
                        string[] strFileTemEnd = uploadFile.FileName.Split(chfile);
                        //END
                        uploadFile.SaveAs(Path.Combine(strRootPath, strFileTemEnd[0].Split(' ')[0].ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "." + strFileTemEnd[strFileTemEnd.Length - 1].ToString()));
                    }
                    //uploadFile.SaveAs(Path.Combine(strRootPath, uploadFile.FileName));
                    else
                    {
                        string _exten = Path.GetExtension(uploadFile.FileName);
                        #region ResizeImages
                        string _tenGoc = uploadFile.FileName;
                        System.Drawing.Image objImage;
                        objImage = System.Drawing.Image.FromFile(TempFile + @"\" + strtemp);
                        string _tenResize = "";
                        string strPathPhotos = "";
                        if (objImage.Width > Convert.ToInt32(prjComponents.Global.VNPResizeImages))
                        {
                            _tenResize = "re_" + DateTime.Now.Millisecond.ToString() + uploadFile.FileName;
                            //phan resize anh
                            //resizeimage(strRootPath + @"\" + _tenGoc, strRootPath + @"\" + _tenResize);
                            resizeimage(TempFile + @"\" + strtemp, TempFile + @"\" + _tenResize);
                            //strPathPhotos = strRootPath + @"\" + _tenResize;
                            strPathPhotos = TempFile + @"\" + _tenResize;
                        }
                        else strPathPhotos = TempFile + @"\" + strtemp;
                        //else strPathPhotos = strRootPath + @"\" + uploadFile.FileName;
                        #endregion ResizeImages


                        #region DONG DAU IMAGES
                        System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(strPathPhotos);
                        int phWidth = imgPhoto.Width;
                        int phHeight = imgPhoto.Height;

                        //create a Bitmap the Size of the original photograph
                        Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

                        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                        //load the Bitmap into a Graphics object 
                        Graphics grPhoto = Graphics.FromImage(bmPhoto);

                        //create a image object containing the watermark
                        //@"F:\HoangPhat\Bao_Anh\BackEnd\prjApplicationNews\prjApplication\prjApplication\Images\IconHPC\LoGoBaoAnhDong.png"
                        System.Drawing.Image imgWatermark = new Bitmap(context.Server.MapPath("../Images/IconHPC/LoGoBaoAnhDong.png"));
                        int wmWidth = imgWatermark.Width;
                        int wmHeight = imgWatermark.Height;

                        //------------------------------------------------------------
                        //Step #1 - Insert Copyright message
                        //------------------------------------------------------------

                        //Set the rendering quality for this Graphics object
                        grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

                        //Draws the photo Image object at original size to the graphics object.
                        grPhoto.DrawImage(
                            imgPhoto,                               // Photo Image object
                            new System.Drawing.Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                            0,                                      // x-coordinate of the portion of the source image to draw. 
                            0,                                      // y-coordinate of the portion of the source image to draw. 
                            phWidth,                                // Width of the portion of the source image to draw. 
                            phHeight,                               // Height of the portion of the source image to draw. 
                            GraphicsUnit.Pixel);                    // Units of measure 

                        //-------------------------------------------------------
                        //to maximize the size of the Copyright message we will 
                        //test multiple Font sizes to determine the largest posible 
                        //font we can use for the width of the Photograph
                        //define an array of point sizes you would like to consider as possiblities
                        //-------------------------------------------------------
                        int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

                        System.Drawing.Font crFont = null;
                        SizeF crSize = new SizeF();

                        //Loop through the defined sizes checking the length of the Copyright string
                        //If its length in pixles is less then the image width choose this Font size.
                        for (int i = 0; i < 7; i++)
                        {
                            //set a Font object to Arial (i)pt, Bold
                            crFont = new System.Drawing.Font("arial", sizes[i], FontStyle.Bold);
                            //Measure the Copyright string in this Font
                            crSize = grPhoto.MeasureString("", crFont);

                            if ((ushort)crSize.Width < (ushort)phWidth)
                                break;
                        }

                        //Since all photographs will have varying heights, determine a 
                        //position 5% from the bottom of the image
                        //int yPixlesFromBottom = (int)(phHeight * .05);
                        int xPixlesFromBottom = (int)(phWidth * .05);
                        int yPixlesFromBottom = (int)(phHeight * .05);

                        //Now that we have a point size use the Copyrights string height 
                        //to determine a y-coordinate to draw the string of the photograph
                        float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

                        //Determine its x-coordinate by calculating the center of the width of the image
                        //float xCenterOfImg = (phWidth / 2);
                        float xCenterOfImg = ((phWidth - xPixlesFromBottom) - (crSize.Width / 2));

                        //Define the text layout by setting the text alignment to centered
                        StringFormat StrFormat = new StringFormat();
                        StrFormat.Alignment = StringAlignment.Center;

                        //define a Brush which is semi trasparent black (Alpha set to 153)
                        SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

                        //Draw the Copyright string
                        grPhoto.DrawString("",                 //string of text
                            crFont,                                   //font
                            semiTransBrush2,                           //Brush
                            new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                            StrFormat);

                        //define a Brush which is semi trasparent white (Alpha set to 153)
                        SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

                        //Draw the Copyright string a second time to create a shadow effect
                        //Make sure to move this text 1 pixel to the right and down 1 pixel
                        grPhoto.DrawString("",                 //string of text
                            crFont,                                   //font
                            semiTransBrush,                           //Brush
                            new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                            StrFormat);                               //Text alignment


                        //------------------------------------------------------------
                        //Step #2 - Insert Watermark image
                        //------------------------------------------------------------

                        //Create a Bitmap based on the previously modified photograph Bitmap
                        _obj = new Bitmap(bmPhoto);
                        _obj.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
                        //Load this Bitmap into a new Graphic Object
                        Graphics grWatermark = Graphics.FromImage(_obj);

                        //To achieve a transulcent watermark we will apply (2) color 
                        //manipulations by defineing a ImageAttributes object and 
                        //seting (2) of its properties.
                        ImageAttributes imageAttributes = new ImageAttributes();
                        //The first step in manipulating the watermark image is to replace 
                        //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
                        //to do this we will use a Colormap and use this to define a RemapTable
                        ColorMap colorMap = new ColorMap();

                        //My watermark was defined with a background of 100% Green this will
                        //be the color we search for and replace with transparency
                        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                        //colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                        colorMap.NewColor = Color.FromArgb(255, 0, 255, 0);

                        ColorMap[] remapTable = { colorMap };

                        imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                        //The second color manipulation is used to change the opacity of the 
                        //watermark.  This is done by applying a 5x5 matrix that contains the 
                        //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
                        //to 0.3f we achive a level of opacity
                        float[][] colorMatrixElements = { 
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},       
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
                        ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

                        imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                            ColorAdjustType.Bitmap);

                        //For this example we will place the watermark in the upper right
                        //hand corner of the photograph. offset down 10 pixels and to the 
                        //left 10 pixles

                        //int xPosOfWm = (phWidth - wmWidth) / 2;
                        int xPosOfWm = 0;
                        //int xPosOfWm = (phWidth - wmWidth) / 2;
                        //int yPosOfWm = (phHeight - wmHeight) / 2;//((phHeight - wmHeight) - xPosOfWm);
                        int yPosOfWm = (phHeight - wmHeight);

                        grWatermark.DrawImage(imgWatermark,
                            new System.Drawing.Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                            0,                  // x-coordinate of the portion of the source image to draw. 
                            0,                  // y-coordinate of the portion of the source image to draw. 
                            wmWidth,            // Watermark Width
                            wmHeight,		    // Watermark Height
                            GraphicsUnit.Pixel, // Unit of measurment
                            imageAttributes);   //ImageAttributes Object

                        //Replace the original photgraphs bitmap with the new Bitmap
                        _width = _obj.Width;
                        _height = _obj.Height;

                        //_obj = this.resizeImage(_obj, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                        //this.saveJpeg(tempFile + @"" + strRootPath + @"/" + _tenfile, _obj, 85L);
                        //XU LY DAU CHAM
                        char[] chfile = { '.' };
                        string[] strFileTemEnd = uploadFile.FileName.Split(chfile);
                        //END
                        this.saveJpeg(strRootPath + "\\" + strFileTemEnd[0].Split(' ')[0].ToString() + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond.ToString() + "." + strFileTemEnd[strFileTemEnd.Length - 1].ToString(), _obj, 100L);

                        semiTransBrush.Dispose();
                        imageAttributes.Dispose();
                        imgWatermark.Dispose();
                        _obj.Dispose();
                        objImage.Dispose();
                        bmPhoto.Dispose();
                        grPhoto.Dispose();
                        grWatermark.Dispose();
                        //----------------END PHAN DONG DAU ANH-----------------------
                        #endregion END DONG DAU IMAGES
                    }
                    //Xóa file
                    //context.Response.End();
                    //DeleteFile(TempFile + "\\" + strtemp);
                    // HttpPostedFile has an InputStream also.  You can pass this to 
                    // a function, or business logic. You can save it a database:
                    //byte[] fileData = new byte[uploadFile.ContentLength];
                    //uploadFile.InputStream.Write(fileData, 0, fileData.Length);
                    // save byte array into database.

                    // something I do is extract files from a zip file by passing
                    // the inputStream to a function that uses SharpZipLib found here:
                    // http://www.icsharpcode.net/OpenSource/SharpZipLib/
                    // and then save the files to disk.                    
                }
            }
        }
        // Used as a fix for a bug in mac flash player that makes the 
        // onComplete event not fire
        HttpContext.Current.Response.Write(" ");
    }
    #endregion
    #region DELETE FILE
    public void DeleteFile(string _pathFile)
    {
        try
        {
            if (File.Exists(_pathFile))
                File.Delete(_pathFile);

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion END DELETE FILE
    #region Resize Image
    private void resizeimage(string fileImage, string _strpath)
    {
        int _path = fileImage.IndexOf(":");
        if (_path < 1)
            fileImage = HttpContext.Current.Server.MapPath("../" + fileImage);
        ViewResize(fileImage, Convert.ToInt32(prjComponents.Global.VNPResizeImages), _strpath);
    }
    private void ViewResize(string ImagePath, int Width, string imgPath)
    {
        string path = ImagePath;
        //System.Drawing.Image objThumbnail;
        Bitmap _obj;
        System.Drawing.Image objImage;
        int imgwidth = 0;
        int imgheight = 0;
        decimal lnRatio;
        try
        {
            objImage = System.Drawing.Image.FromFile(path);
            if (objImage.Width > objImage.Height)
            {
                lnRatio = (decimal)Width / objImage.Width;
                imgwidth = Width;
                decimal lnTemp = objImage.Height * lnRatio;
                imgheight = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)Width / objImage.Width;
                imgwidth = Width;
                decimal lnTemp = objImage.Height * lnRatio;
                imgheight = (int)lnTemp;

            }
            // Create thumbnail
            _obj = new Bitmap(imgwidth, imgheight);
            Graphics grWatermark = Graphics.FromImage(_obj);
            grWatermark.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grWatermark.SmoothingMode = SmoothingMode.HighQuality;
            grWatermark.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grWatermark.CompositingQuality = CompositingQuality.HighQuality;

            System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, imgwidth, imgheight);

            grWatermark.DrawImage(objImage, imageRectangle, 0, 0, objImage.Width, objImage.Height, GraphicsUnit.Pixel);
            //_obj.Save(imgPath, objImage.RawFormat);
            this.saveJpeg(imgPath, _obj, 100L);
            grWatermark.Dispose();
            _obj.Dispose();
            objImage.Dispose();
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }

    #endregion

    #region DONG DAU IMAGES
    private void saveJpeg(string path, Bitmap img, long quality)
    {
        // Encoder parameter for image quality
        EncoderParameter qualityParam =
            new EncoderParameter(Encoder.Quality, quality);

        // Jpeg image codec
        ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

        if (jpegCodec == null)
            return;

        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;

        img.Save(path, jpegCodec, encoderParams);
        img.Dispose();

    }
    private ImageCodecInfo getEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        // Find the correct image codec
        for (int i = 0; i < codecs.Length; i++)
            if (codecs[i].MimeType == mimeType)
                return codecs[i];
        return null;
    }
    #endregion ----------------
}
