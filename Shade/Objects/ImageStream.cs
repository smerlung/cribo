using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shade.Objects
{
    public static class ImageStream
    {
        public static byte[] GetImageBytes(string imagepath)
        {
            using (Bitmap bitmap = new Bitmap(imagepath))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.GetBuffer();
                }
            }
        }

        public static void SetImageBytes(string imagepath, IList<byte> bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes.ToArray()))
            {
                using (Bitmap bitmap = new Bitmap(stream))
                {
                    bitmap.Save(imagepath, ImageFormat.Png);
                }
            }
        }
    }
}
