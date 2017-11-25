using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Cribo.Objects
{
    public static class ImageStream
    {
        public static byte[] GetImageBytes(string imagepath)
        {
            using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = fs;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                int stride = image.PixelWidth * 4;
                int size = image.PixelHeight * stride;
                byte[] pixels = new byte[size];
                image.CopyPixels(pixels, stride, 0);
                return pixels;
            }

        }

        public static void SetImageBytes(string imagepath, IList<byte> bytes)
        {
            BitmapImage image = new BitmapImage();
            using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            {

                image.BeginInit();
                image.StreamSource = fs;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }

            WriteableBitmap bitmap = new WriteableBitmap(image);
            bitmap.WritePixels(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight), bytes.ToArray(), image.PixelWidth * 4, 0);


            using (FileStream stream = new FileStream(imagepath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
                stream.Close();
            }
        }
    }
}
