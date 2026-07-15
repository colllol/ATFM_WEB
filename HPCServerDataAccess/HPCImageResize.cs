using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace HPCServerDataAccess
{
    public class HPCImageResize
    {
        #region "Resize image"

        protected static Bitmap GetScaledPicture(Bitmap source, int maxWidth, int maxHeight)
        {
            int width = 0, height = 0;
            float aspectRatio = (float)source.Width / (float)source.Height;

            if ((maxHeight > 0) && (maxWidth > 0))
            {
                if ((source.Width < maxWidth) && (source.Height < maxHeight))
                {
                    return source;
                }
                else if (aspectRatio > 1)
                {
                    width = maxWidth;
                    height = (int)(width / aspectRatio);
                    if (height > maxHeight)
                    {
                        height = maxHeight;
                        width = (int)(height * aspectRatio);
                    }
                }
                else
                {
                    height = maxHeight;
                    width = (int)(height * aspectRatio);
                    if (width > maxWidth)
                    {
                        width = maxWidth;
                        height = (int)(width / aspectRatio);
                    }
                }
            }
            else if ((maxHeight == 0) && (maxWidth > 0))
            {
                width = maxWidth;
                height = (int)(width / aspectRatio);
            }
            else if ((maxWidth == 0) && (maxHeight > 0))
            {
                height = maxHeight;
                width = (int)(height * aspectRatio);
            }
            else if ((maxWidth == 0) && (maxHeight == 0))
            {
                return source;
            }

            Bitmap newImage = GetResizedImage(source, width, height);
            return newImage;
        }
        protected static Bitmap GetResizedImage(Bitmap source, int width, int height)
        {
            //This function creates the thumbnail image.
            //The logic is to create a blank image and to 
            // draw the source image onto it
            Bitmap thumb = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(thumb);
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.CompositingQuality = CompositingQuality.HighQuality;
            Rectangle rectDestination = new Rectangle(0, 0, width, height);
            gr.DrawImage(source, rectDestination, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
            gr.Dispose();
            return thumb;
        }
        protected static Bitmap getImageSource(string strPhysicalPath, int maxWidth, int maxHeight)
        {
            Bitmap bmpImageSource;
            bmpImageSource = new Bitmap(strPhysicalPath);
            return bmpImageSource;
        }
        protected static bool DeleteImageSourceAfterResize(string physicalPath2File)
        {
            bool bolDeleted = false;
            try
            {
                if (System.IO.File.Exists((physicalPath2File)))
                {
                    System.IO.File.Delete(physicalPath2File);
                    bolDeleted = true;
                }
            }
            catch (Exception ex) { bolDeleted = false; }
            return bolDeleted;
        }        
        public static bool SaveImage2Server(string strPhysicalPath, string oldFileName, string newFileName, int maxWidth, int maxHeight)
        {
            bool bolResize = false;
            string strFullPath2Oldfilename = strPhysicalPath + @"\" + oldFileName;
            string strFullPath2Newfilename = strPhysicalPath + @"\" + newFileName;
            Bitmap bmpSource = null;
            Bitmap bmpAfterResize = null;
            try
            {
                bmpSource = getImageSource(strFullPath2Oldfilename, maxWidth, maxHeight);
                bmpAfterResize = GetScaledPicture(bmpSource, maxWidth, maxHeight);
                bmpAfterResize.Save(strFullPath2Newfilename);
                bmpSource.Dispose();
                bolResize = DeleteImageSourceAfterResize(strFullPath2Oldfilename);
                bmpAfterResize.Dispose();
                bolResize = true;
            }
            catch //(Exception ex)
            {
                bolResize = false;
            }   
            return bolResize;            
        }
        public static bool SaveImage2Server(string strPhysicalPath, string oldFileName, string newFileName, string newFileNameWatermark, string logo, int maxWidth, int maxHeight, int WM, int spacevalue)
        {
            bool bolResize = false;
            string strFullPath2Oldfilename = strPhysicalPath + @"\" + oldFileName;
            string strFullPath2Newfilename = strPhysicalPath + @"\" + newFileName;
            Bitmap bmpSource = null;
            Bitmap bmpAfterResize = null;
            try
            {
                bmpSource = getImageSource(strFullPath2Oldfilename, maxWidth, maxHeight);
                bmpAfterResize = GetScaledPicture(bmpSource, maxWidth, maxHeight);
                bmpAfterResize.Save(strFullPath2Newfilename);
                //
                HPCImageResize _until = new HPCImageResize();
                if (WM > 0)
                {
                    _until.WatermarkImages(strPhysicalPath, newFileNameWatermark, strFullPath2Newfilename, logo, WM, spacevalue);
                }else
                    _until.WatermarkImages(strPhysicalPath, newFileNameWatermark, strFullPath2Newfilename, logo);

                _until = null;
                //
                bmpSource.Dispose();
                bolResize = DeleteImageSourceAfterResize(strFullPath2Oldfilename);
                //bmpAfterResize.Dispose();//BOCT EDIT
                //bmpAfterResize = null;//BOCT EDIT
                //bolResize = DeleteImageSourceAfterResize(strFullPath2Newfilename);// BOCT EDIT
                bolResize = true;
            }
            catch //(Exception ex)
            {
                bolResize = false;
            }
            return bolResize;
        }
        public static bool SaveImage2Server(string strPhysicalPath, string oldFileName, string newFileName, string newFileNameWatermark, string logo, int maxWidth, int maxHeight)
        {
            bool bolResize = false;
            string strFullPath2Oldfilename = strPhysicalPath + @"\" + oldFileName;
            string strFullPath2Newfilename = strPhysicalPath + @"\" + newFileName;
            Bitmap bmpSource = null;
            Bitmap bmpAfterResize = null;
            try
            {
                bmpSource = getImageSource(strFullPath2Oldfilename, maxWidth, maxHeight);
                bmpAfterResize = GetScaledPicture(bmpSource, maxWidth, maxHeight);
                bmpAfterResize.Save(strFullPath2Newfilename);
                //
                HPCImageResize _until = new HPCImageResize();
                _until.WatermarkImages(strPhysicalPath, newFileNameWatermark, strFullPath2Newfilename, logo);
                _until = null;
                //
                bmpSource.Dispose();
                bolResize = DeleteImageSourceAfterResize(strFullPath2Oldfilename);
                //bmpAfterResize.Dispose();//BOCT EDIT
                //bmpAfterResize = null;//BOCT EDIT
                //bolResize = DeleteImageSourceAfterResize(strFullPath2Newfilename);// BOCT EDIT
                bolResize = true;
            }
            catch //(Exception ex)
            {
                bolResize = false;
            }   
            return bolResize;            
        }
        protected void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");
            if (jpegCodec == null)
                return;
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
            img.Dispose();
        }
        protected ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        public void WatermarkImages(string strPhysicalPath, string newFileNameWatermark, string _fileName, string LogoDongDau)
        {
            Bitmap _obj;
            double _width, _height;
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(_fileName);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //create a image object containing the watermark
            System.Drawing.Image imgWatermark = new Bitmap(LogoDongDau);
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
            //END
            this.saveJpeg(strPhysicalPath +@"\" + newFileNameWatermark , _obj, 100L);
            semiTransBrush.Dispose();
            imageAttributes.Dispose();
            imgWatermark.Dispose();
            _obj.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();
            //----------------END PHAN DONG DAU ANH-----------------------
        }
        public void WatermarkImages(string strPhysicalPath, string newFileNameWatermark, string _fileName, string LogoDongDau, int wm, int spacevalue)
        {
            Bitmap _obj;
            double _width, _height;
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(_fileName);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //create a image object containing the watermark
            System.Drawing.Image imgWatermark = new Bitmap(LogoDongDau);
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
            int posH = 0, posW = 0;
            if (wm == 1)
            {
                posW = Convert.ToInt32(phWidth * 0.5) - Convert.ToInt32(imgWatermark.Width * 0.5);
                posH = Convert.ToInt32(phHeight * 0.5) - Convert.ToInt32(imgWatermark.Height * 0.5);
            }
            else if (wm == 2)
            {
                posW = spacevalue;
                posH = spacevalue;
            }
            else if (wm == 3)
            {
                posW = Convert.ToInt32(phWidth) - Convert.ToInt32(imgWatermark.Width) - spacevalue;
                posH = spacevalue;
            }
            else if (wm == 4)
            {
                posW = spacevalue;
                posH = Convert.ToInt32(phHeight) - Convert.ToInt32(imgWatermark.Height) - spacevalue;
            }
            else if (wm == 5)
            {
                posW = Convert.ToInt32(phWidth) - Convert.ToInt32(imgWatermark.Width) - spacevalue;
                posH = Convert.ToInt32(phHeight) - Convert.ToInt32(imgWatermark.Height) - spacevalue;
            }
            grWatermark.DrawImage(imgWatermark,
                new System.Drawing.Rectangle(posW, posH, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object
            //Replace the original photgraphs bitmap with the new Bitmap
            _width = _obj.Width;
            _height = _obj.Height;
            //END
            this.saveJpeg(strPhysicalPath + @"\" + newFileNameWatermark, _obj, 100L);
            semiTransBrush.Dispose();
            imageAttributes.Dispose();
            imgWatermark.Dispose();
            _obj.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();
            //----------------END PHAN DONG DAU ANH-----------------------
        }
        #endregion
    }
}
