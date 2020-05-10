using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ImageController
{
    public class ImageOptimizer
    {
        public static byte[] Resize(byte[] inputImage, int pixelsSmallestSideAmount)
        {
            using (var ms = new MemoryStream(inputImage))
            {
                Image image = Image.FromStream(ms);

                var resultImage = ImageOptimizer.Resize(image, pixelsSmallestSideAmount);

                using (var resultMS = new MemoryStream())
                {
                    resultImage.Save(resultMS, resultImage.RawFormat);
                    return ms.ToArray();
                }
            }
        }


        public static Image Resize(Image inputImage, int pixelsSmallestSideAmount)
        {
            int newWidth;
            int newHeight;

            Image result;

            if (inputImage.Width < inputImage.Height)
            {
                if (inputImage.Width < pixelsSmallestSideAmount)
                {
                    return inputImage;
                }

                newWidth = pixelsSmallestSideAmount;
                newHeight = (int)(inputImage.Height * (Convert.ToDouble(newWidth) / inputImage.Width));
            }
            else
            {
                if (inputImage.Height < pixelsSmallestSideAmount)
                {
                    return inputImage;
                }

                newHeight = pixelsSmallestSideAmount;
                newWidth = (int)(inputImage.Width * (Convert.ToDouble(newHeight) / inputImage.Height));
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(inputImage, imageRectangle);

            using (MemoryStream toStream = new MemoryStream())
            {
                thumbnailBitmap.Save(toStream, inputImage.RawFormat);
                result = Image.FromStream(toStream);

                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
            }

            return result;
        }


        public static byte[] Resize(byte[] inputImage, int pixelsWigthAmount, int pixelsHeightAmount)
        {
            using (var ms = new MemoryStream(inputImage))
            {
                Image image = Image.FromStream(ms);

                var resultImage = ImageOptimizer.Resize(image, pixelsWigthAmount, pixelsHeightAmount);

                using (var resultMS = new MemoryStream())
                {
                    resultImage.Save(resultMS, resultImage.RawFormat);
                    return ms.ToArray();
                }
            }
        }

        public static Image Resize(Image inputImage, int pixelsWigthAmount, int pixelsHeightAmount)
        {
            Image result;

            var thumbnailBitmap = new Bitmap(pixelsWigthAmount, pixelsHeightAmount);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, pixelsWigthAmount, pixelsHeightAmount);
            thumbnailGraph.DrawImage(inputImage, imageRectangle);

            using (MemoryStream toStream = new MemoryStream())
            {
                thumbnailBitmap.Save(toStream, inputImage.RawFormat);
                result = Image.FromStream(toStream);


                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();

                return result;
            }
        }


        public static byte[] MakeSquare(byte[] inputImage)
        {
            using (var ms = new MemoryStream(inputImage))
            {
                Image image = Image.FromStream(ms);

                var resultImage = ImageOptimizer.MakeSquare(image);

                using (var resultMS = new MemoryStream())
                {
                    resultImage.Save(resultMS, resultImage.RawFormat);
                    return ms.ToArray();
                }
            }
        }


        public static Image MakeSquare(Image inputImage) 
        {
            int smallestDimension = Math.Min(inputImage.Height, inputImage.Width);

            Size squareSize = new Size(smallestDimension, smallestDimension);
            Bitmap bmp = new Bitmap(squareSize.Width, squareSize.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, squareSize.Width, squareSize.Height);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(inputImage, (squareSize.Width / 2) - (inputImage.Width / 2), (squareSize.Height / 2) - (inputImage.Height / 2), inputImage.Width, inputImage.Height);
            }

            using (MemoryStream newImStream = new MemoryStream())
            {
                bmp.Save(newImStream, ImageFormat.Jpeg);

                return Image.FromStream(newImStream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputImage"></param>
        /// <param name="qualityValue">Has to be biggest yhan zero, less than 100 and the remainder of the division on 10 equals zero.</param>
        /// <returns></returns>
        public static byte[] Compres(byte[] inputImage, long qualityValue)
        {
            using (var ms = new MemoryStream(inputImage))
            {
                Image image = Image.FromStream(ms);

                var resultImage = ImageOptimizer.Compres(image, qualityValue);

                using (var resultMS = new MemoryStream())
                {
                    resultImage.Save(resultMS, resultImage.RawFormat);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputImage"></param>
        /// <param name="qualityValue">Has to be biggest yhan zero, less than 100 and the remainder of the division on 10 equals zero.</param>
        /// <returns></returns>
        public static Image Compres(Image inputImage, long qualityValue)
        {
            if (qualityValue > 100 || qualityValue < 0 || qualityValue % 10 != 0)
            {
                throw new ArgumentException("Input 'qualityValue' has to be less than 100, bigger than 0 and the remainder of the division on 10 equals zero.");
            }


            Bitmap bm = (Bitmap)inputImage;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == "image/jpeg")
                {
                    ici = codec;
                }
            }

            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(Encoder.Quality, qualityValue);

            using (MemoryStream ms2 = new MemoryStream())
            {
                bm.Save(ms2, ici, ep);

                return Image.FromStream(ms2);
            }
        }


    }
}
