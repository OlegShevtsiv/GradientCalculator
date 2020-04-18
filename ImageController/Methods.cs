using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageController
{
    public class Methods : IDisposable
    {
        public static byte[] ResizeImage(int toSize, byte[] input)
        {

            MemoryStream fromStream = new MemoryStream(input);

            var image = Image.FromStream(fromStream);

            int newWidth;
            int newHeight;

            if (image.Width < image.Height)
            {
                if (image.Width < toSize)
                {
                    return input;
                }

                newWidth = toSize;
                newHeight = (int)(image.Height * (Convert.ToDouble(newWidth) / image.Width));
            }
            else
            {
                if (image.Height < toSize)
                {
                    return input;
                }

                newHeight = toSize;
                newWidth = (int)(image.Width * (Convert.ToDouble(newHeight) / image.Height));
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            byte[] res;
            MemoryStream toStream = new MemoryStream();
            thumbnailBitmap.Save(toStream, image.RawFormat);
            res = toStream.ToArray();

            toStream.Dispose();
            fromStream.Dispose();

            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();

            return res;
        }

        public static byte[] ResizeImage(int wigth, int height, byte[] input)
        {

            MemoryStream fromStream = new MemoryStream(input);

            var image = Image.FromStream(fromStream);

            var thumbnailBitmap = new Bitmap(wigth, height);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, wigth, height);
            thumbnailGraph.DrawImage(image, imageRectangle);

            byte[] res;
            MemoryStream toStream = new MemoryStream();
            thumbnailBitmap.Save(toStream, image.RawFormat);
            res = toStream.ToArray();

            toStream.Dispose();
            fromStream.Dispose();

            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();

            return res;
        }


        public static byte[] MakeImmageSquare(byte[] data)
        {
            try
            {
                Image img;

                byte[] res;
                using (var ms = new MemoryStream(data))
                {
                    img = Image.FromStream(ms, true, true);


                    int smallestDimension = Math.Min(img.Height, img.Width);

                    Size squareSize = new Size(smallestDimension, smallestDimension);
                    Bitmap bmp = new Bitmap(squareSize.Width, squareSize.Height);
                    using (Graphics graphics = Graphics.FromImage(bmp))
                    {
                        graphics.FillRectangle(Brushes.White, 0, 0, squareSize.Width, squareSize.Height);
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        graphics.DrawImage(img, (squareSize.Width / 2) - (img.Width / 2), (squareSize.Height / 2) - (img.Height / 2), img.Width, img.Height);
                    }

                    MemoryStream newImStream = new MemoryStream();

                    bmp.Save(newImStream, ImageFormat.Jpeg);

                    res = newImStream.ToArray();

                    newImStream.Dispose();
                }
                return res;
            }
            catch { return new byte[0]; }
        }

        public static byte[] Compres(byte[] data, long qualityValue)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    var img = Image.FromStream(ms);

                    Bitmap bm = (Bitmap)img;
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
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityValue);

                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        bm.Save(ms2, ici, ep);
                        return ms2.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {

        }
    }
}
