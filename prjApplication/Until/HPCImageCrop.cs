using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
namespace prjApplication.Until
{
    public class HPCImageCrop
    {
        private static Dictionary<string, ImageCodecInfo> encoders = null;
        private static System.Drawing.Image ProcessCrop(System.Drawing.Image imgPhoto, int Width, int Height, int X, int Y)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = X;
            int destY = Y;
            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(
                Width,Height,PixelFormat.Format32bppRgb);
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
        private static void SaveCrop(string pathDest, System.Drawing.Image image, int Width, int Height, int X, int Y, int quality)
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
        private static Dictionary<string, ImageCodecInfo> Encoders
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
        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
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
        
        public static void Crop(string pathSource,string pathDest,int _Width, int _Height, int _X, int _Y)
        {
            System.Drawing.Image obj_InImage;
            System.Drawing.Image obj_OutImage;
            
            obj_InImage = System.Drawing.Image.FromFile(pathSource);
            obj_OutImage = ProcessCrop(obj_InImage, _Width,_Height,_X,_Y);
            //obj_OutImage.Save(@"C:\Documents and Settings\Admin\Desktop\avatar-crop-jquery-source\images\AB_40x27_Crop.jpg", ImageFormat.Jpeg);
            SaveCrop(pathDest,obj_OutImage, _Width,_Height,_X,_Y, 100);
            obj_InImage.Dispose();
            obj_OutImage.Dispose();
        }
    }
}
