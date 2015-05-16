using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shade.Objects
{
    public class ShadeStream
    {
        public static byte[] GetShadeBytes(IList<byte> data)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i <= data.Count - 8; i += 8)
            {
                byte value = GetShadeByte(data, i);
                result.Add(value);
            }

            return result.ToArray();
        }

        public static void SetShadeBytes(IList<byte> data, IList<byte> shadebytes)
        {
            for (int i = 0; i < shadebytes.Count; i++)
            {
                SetShadeByte(data, i * 8, shadebytes[i]);
            }
        }

        public static byte GetShadeByte(IList<byte> data, int index)
        {
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                byte bit = (byte)(data[index + i] & 0x1);
                result <<= 1;
                result |= bit;
            }

            return result;
        }

        public static void SetShadeByte(IList<byte> data, int index, byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                byte clearedbyte = (byte)(data[index + i] & 0xfe);
                byte bit = (byte)((value >> 7 - i) & 0x1);
                clearedbyte |= bit;
                data[index + i] = clearedbyte;
            }
        }
    }
}
