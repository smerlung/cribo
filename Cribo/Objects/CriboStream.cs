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

namespace Cribo.Objects
{
    public class CriboStream
    {
        public static byte[] GetCriboBytes(IList<byte> data)
        {
            byte[] firstbytes = new byte[]{
                GetCriboByte(data,0),
                GetCriboByte(data,8)
            };

            ushort count = BitConverter.ToUInt16(firstbytes, 0);

            List<byte> result = new List<byte>();
            for (int i = 2; i < 2 + count; i++)
            {
                byte value = GetCriboByte(data, i * 8);
                result.Add(value);
            }

            return result.ToArray();
        }

        public static void SetCriboBytes(IList<byte> data, IList<byte> Cribobytes)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(BitConverter.GetBytes(Convert.ToUInt16(Cribobytes.Count)));
            buffer.AddRange(Cribobytes);
            for (int i = 0; i < buffer.Count; i++)
            {
                SetCriboByte(data, i * 8, buffer[i]);
            }
        }

        public static byte GetCriboByte(IList<byte> data, int index)
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

        public static void SetCriboByte(IList<byte> data, int index, byte value)
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
