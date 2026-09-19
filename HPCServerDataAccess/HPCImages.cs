using System;
using System.Collections.Generic;
using System.Web;
using prjInfo;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using HPCServerDataAccess;


namespace HPCServerDataAccess
{
    public class HPCImages
    {
        public static Bitmap GetScaledPicture(Bitmap source, int maxWidth, int maxHeight)
        {
            int width = 0, height = 0;
            maxHeight = 0;
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
                if (source.Width >= maxWidth)
                {
                    width = maxWidth;
                    height = (int)(width / aspectRatio);
                }
                else
                {
                    width = source.Width;
                    height = source.Height;
                }

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
        public static Bitmap GetResizedImage(Bitmap source, int width, int height)
        {
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

        public static Bitmap SaveImage2Server(Bitmap bmpSource, int maxWidth, int maxHeight, bool Resize, Bitmap PathImageResize)
        {
            Bitmap _return = null;
            try
            {
                //using ()
                //{
                Bitmap bmpAfterResize = GetScaledPicture(bmpSource, maxWidth, maxHeight);
                if (Resize)
                    _return = WatermarkImages(bmpAfterResize, PathImageResize);
                else
                    _return = bmpAfterResize;
                //}

                ////bmpAfterResize.Dispose();
            }
            catch
            {

                _return = null;
            }

            return _return;
        }
        public static void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            if (jpegCodec == null)
                return;
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
            img.Dispose();
        }

        public static Bitmap WatermarkImages(Bitmap bmpSource, Bitmap ImageMark)
        {
            Bitmap _obj;
            double _width, _height;
            System.Drawing.Image imgPhoto = bmpSource;

            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            System.Drawing.Image imgWatermark = ImageMark;
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;


            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;


            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new System.Drawing.Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            System.Drawing.Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new System.Drawing.Font("arial", sizes[i], FontStyle.Bold);
                crSize = grPhoto.MeasureString("", crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            int xPixlesFromBottom = (int)(phWidth * .05);
            int yPixlesFromBottom = (int)(phHeight * .05);

            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            float xCenterOfImg = ((phWidth - xPixlesFromBottom) - (crSize.Width / 2));

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);                               //Text alignment
            _obj = new Bitmap(bmPhoto);
            _obj.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics grWatermark = Graphics.FromImage(_obj);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(255, 0, 255, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);


            float[][] colorMatrixElements = { 
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},    
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            int xPosOfWm = 0;

            int yPosOfWm = (phHeight - wmHeight);
            grWatermark.DrawImage(imgWatermark,
                new System.Drawing.Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            _width = _obj.Width;
            _height = _obj.Height;

            imgPhoto.Dispose();
            semiTransBrush.Dispose();
            imageAttributes.Dispose();
            imgWatermark.Dispose();
            //_obj.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();

            return _obj;
        }

        public static Bitmap WatermarkImages_RemByDung(Bitmap bmpSource, Bitmap ImageMark, int style, int Spacevalue)
        {
            Bitmap _obj;
            double _width, _height;
            System.Drawing.Image imgPhoto = bmpSource;

            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            System.Drawing.Image imgWatermark = ImageMark;
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;


            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;


            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new System.Drawing.Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            System.Drawing.Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new System.Drawing.Font("arial", sizes[i], FontStyle.Bold);
                crSize = grPhoto.MeasureString("", crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            int xPixlesFromBottom = (int)(phWidth * .05);
            int yPixlesFromBottom = (int)(phHeight * .05);

            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            float xCenterOfImg = ((phWidth - xPixlesFromBottom) - (crSize.Width / 2));

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);                               //Text alignment
            _obj = new Bitmap(bmPhoto);
            _obj.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics grWatermark = Graphics.FromImage(_obj);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(255, 0, 255, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);


            float[][] colorMatrixElements = { 
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},    
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            int xPosOfWm = 0;

            int yPosOfWm = (phHeight - wmHeight);


            int count_long = (phWidth / wmWidth) + 1;
            //double percent = 0;

            int posH = 0, posW = 0;
            if (style == 1)
            {
                posW = Convert.ToInt32(phWidth * 0.5) - Convert.ToInt32(ImageMark.Width * 0.5);
                posH = Convert.ToInt32(phHeight * 0.5) - Convert.ToInt32(ImageMark.Height * 0.5);
            }
            else if (style == 2)
            {
                posW = Spacevalue;
                posH = Spacevalue;
            }
            else if (style == 3)
            {
                posW = Convert.ToInt32(phWidth) - Convert.ToInt32(ImageMark.Width) - Spacevalue;
                posH = Spacevalue;
            }
            else if (style == 4)
            {
                posW = Spacevalue;
                posH = Convert.ToInt32(phHeight) - Convert.ToInt32(ImageMark.Height) - Spacevalue;
            }
            else if (style == 5)
            {
                posW = Convert.ToInt32(phWidth) - Convert.ToInt32(ImageMark.Width) - Spacevalue;
                posH = Convert.ToInt32(phHeight) - Convert.ToInt32(ImageMark.Height) - Spacevalue;
            }
            //int posH = Convert.ToInt32(percent) + wmHeight;

            grWatermark.DrawImage(imgWatermark,
                new System.Drawing.Rectangle(posW, posH, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            _width = _obj.Width;
            _height = _obj.Height;

            imgPhoto.Dispose();
            semiTransBrush.Dispose();
            imageAttributes.Dispose();
            imgWatermark.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();

            return _obj;
        }

        public static Bitmap WatermarkImages(Bitmap bmpSource, Bitmap ImageMark, int intX, int intY)
        {
            Bitmap _obj;
            double _width, _height;
            System.Drawing.Image imgPhoto = bmpSource;

            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            System.Drawing.Image imgWatermark = ImageMark;
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;


            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;


            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new System.Drawing.Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            System.Drawing.Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new System.Drawing.Font("arial", sizes[i], FontStyle.Bold);
                crSize = grPhoto.MeasureString("", crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            int xPixlesFromBottom = (int)(phWidth * .05);
            int yPixlesFromBottom = (int)(phHeight * .05);

            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            float xCenterOfImg = ((phWidth - xPixlesFromBottom) - (crSize.Width / 2));

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);                               //Text alignment
            _obj = new Bitmap(bmPhoto);
            _obj.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics grWatermark = Graphics.FromImage(_obj);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(255, 0, 255, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);


            float[][] colorMatrixElements = { 
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},    
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            int xPosOfWm = 0;

            int yPosOfWm = (phHeight - wmHeight);


            int count_long = (phWidth / wmWidth) + 1;
            //double percent = 0;

            int posH = intY, posW = intX;
            //if (style == 1)
            //{
            //    posW = Convert.ToInt32(phWidth * 0.5) - Convert.ToInt32(ImageMark.Width * 0.5);
            //    posH = Convert.ToInt32(phHeight * 0.5) - Convert.ToInt32(ImageMark.Height * 0.5);
            //}
            //else if (style == 2)
            //{
            //    posW = Spacevalue;
            //    posH = Spacevalue;
            //}
            //else if (style == 3)
            //{
            //    posW = Convert.ToInt32(phWidth) - Convert.ToInt32(ImageMark.Width) - Spacevalue;
            //    posH = Spacevalue;
            //}
            //else if (style == 4)
            //{
            //    posW = Spacevalue;
            //    posH = Convert.ToInt32(phHeight) - Convert.ToInt32(ImageMark.Height) - Spacevalue;
            //}
            //else if (style == 5)
            //{
            //    posW = Convert.ToInt32(phWidth) - Convert.ToInt32(ImageMark.Width) - Spacevalue;
            //    posH = Convert.ToInt32(phHeight) - Convert.ToInt32(ImageMark.Height) - Spacevalue;
            //}
            //int posH = Convert.ToInt32(percent) + wmHeight;

            grWatermark.DrawImage(imgWatermark,
                new System.Drawing.Rectangle(posW, posH, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            _width = _obj.Width;
            _height = _obj.Height;

            imgPhoto.Dispose();
            semiTransBrush.Dispose();
            imageAttributes.Dispose();
            imgWatermark.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();

            return _obj;
        }

        public static Bitmap WatermarkImagesold(Bitmap bmpSource, Bitmap ImageMark, int style)
        {
            Bitmap _obj;
            double _width, _height;
            System.Drawing.Image imgPhoto = bmpSource;

            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            System.Drawing.Image imgWatermark = ImageMark;
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;


            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;


            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new System.Drawing.Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            System.Drawing.Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new System.Drawing.Font("arial", sizes[i], FontStyle.Bold);
                crSize = grPhoto.MeasureString("", crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            int xPixlesFromBottom = (int)(phWidth * .05);
            int yPixlesFromBottom = (int)(phHeight * .05);

            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            float xCenterOfImg = ((phWidth - xPixlesFromBottom) - (crSize.Width / 2));

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            grPhoto.DrawString("",                 //string of text
                crFont,                                   //font
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);                               //Text alignment
            _obj = new Bitmap(bmPhoto);
            _obj.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics grWatermark = Graphics.FromImage(_obj);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(255, 0, 255, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);


            float[][] colorMatrixElements = { 
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},    
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},        
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            int xPosOfWm = 0;

            int yPosOfWm = (phHeight - wmHeight);


            int count_long = (phWidth / wmWidth) + 1;
            double percent = 0;
            if (style == 1)
                percent = phHeight * 0.25 - ImageMark.Width * 0.5;
            else if (style == 2)
                percent = phHeight * 0.5 - ImageMark.Width * 0.5;
            else if (style == 3)
                percent = phHeight * 0.75 - ImageMark.Width * 0.5;
            int posH = Convert.ToInt32(percent) + wmHeight;
            for (int i = 0; i < count_long; i++)
            {
                grWatermark.DrawImage(imgWatermark,
                    new System.Drawing.Rectangle(i * wmWidth, posH, wmWidth, wmHeight),  //Set the detination Position
                    0,                  // x-coordinate of the portion of the source image to draw. 
                    0,                  // y-coordinate of the portion of the source image to draw. 
                    wmWidth,            // Watermark Width
                    wmHeight,		    // Watermark Height
                    GraphicsUnit.Pixel, // Unit of measurment
                    imageAttributes);   //ImageAttributes Object
            }
            _width = _obj.Width;
            _height = _obj.Height;

            imgPhoto.Dispose();
            semiTransBrush.Dispose();
            imageAttributes.Dispose();
            imgWatermark.Dispose();
            //_obj.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();

            return _obj;
        }

        public System.Drawing.Image WatermarkImages111(Bitmap ImageSCR, Bitmap ImageMark, int style)
        {
            System.Drawing.Bitmap imgPhoto = ImageSCR;
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            using (Graphics grPhoto = Graphics.FromImage(imgPhoto))
            {
                Bitmap imgWatermark = ImageMark;// new Bitmap(LogoDongDau);
                int Img_W = imgPhoto.Width / 20, Img_H = imgPhoto.Height / 20;
                Decimal a = Img_W / Img_H;
                Decimal b = (imgWatermark.Width / imgWatermark.Height);
                //if (a >= b)
                //{
                //    Img_H = Convert.ToInt32(Math.Round(Img_H * b));
                //}
                //else
                //{
                //    Img_W = Convert.ToInt32(Math.Round(Img_W / b));
                //}
                //HPCAppBMDL.ResizedImage resize = new ResizedImage();
                //imgWatermark = resize.GetResizedImage(ImageMark, Img_W, Img_H);

                int wmWidth = imgWatermark.Width;
                int wmHeight = imgWatermark.Height;
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                int count_long = (phWidth / wmWidth) + 1;
                int count_height = (phHeight / wmHeight) + 1;
                for (int i = 0; i < count_long; i++)
                {
                    //for (int j = 0; j < count_height; j++)
                    //{
                    //grPhoto.DrawImage(imgWatermark,
                    //    new System.Drawing.Rectangle(i * wmWidth, j * wmHeight, wmWidth, wmHeight),  //Set the detination Position
                    //    0,                  // x-coordinate of the portion of the source image to draw. 
                    //    0,                  // y-coordinate of the portion of the source image to draw. 
                    //    wmWidth,            // Watermark Width
                    //    wmHeight,		    // Watermark Height
                    //    GraphicsUnit.Pixel);//, // Unit of measurment
                    //}
                }
                imgWatermark.Dispose();
            }
            return imgPhoto;
        }


        public static Dictionary<string, ImageCodecInfo> encoders = null;
        public static System.Drawing.Image ProcessCrop(System.Drawing.Image imgPhoto, int Width, int Height, int X, int Y)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = X;
            int destY = Y;
            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(
                Width, Height, PixelFormat.Format32bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            System.Drawing.Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //InterpolationMode.HighQualityBicubic;
            // grPhoto.CompositingQuality =CompositingQuality.HighQuality ;
            //grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.CompositingMode = CompositingMode.SourceOver;
            //grPhoto.DrawImage(imgPhoto,
            //    new System.Drawing.Rectangle(destX, destY, Width, Height),
            //    new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            //    System.Drawing.GraphicsUnit.Pixel);
            grPhoto.DrawImage(imgPhoto,
                new System.Drawing.Rectangle(sourceX, sourceY, Width, Height),
                new System.Drawing.Rectangle(destX, destY, Width, Height),
                System.Drawing.GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        public static void SaveCrop(string pathDest, System.Drawing.Image image, int Width, int Height, int X, int Y, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(pathDest, jpegCodec, encoderParams);
        }
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return encoders;
            }
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }

        public static void Crop(string pathSource, string pathDest, int _Width, int _Height, int _X, int _Y)
        {
            System.Drawing.Image obj_InImage;
            System.Drawing.Image obj_OutImage;

            obj_InImage = System.Drawing.Image.FromFile(pathSource);
            obj_OutImage = ProcessCrop(obj_InImage, _Width, _Height, _X, _Y);
            //obj_OutImage.Save(@"C:\Documents and Settings\Admin\Desktop\avatar-crop-jquery-source\images\AB_40x27_Crop.jpg", ImageFormat.Jpeg);
            SaveCrop(pathDest, obj_OutImage, _Width, _Height, _X, _Y, 100);
            obj_InImage.Dispose();
            obj_OutImage.Dispose();
        }
    }
}
