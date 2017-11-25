using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cribo.Objects
{
    public static class SimpleHash
    {
        public static byte[] Create(string input, byte[] salt)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(salt);
            bytes.AddRange(UnicodeEncoding.UTF8.GetBytes(input));

            int missing = 8 - (bytes.Count % 8);
            bytes.AddRange(new byte[missing]);
            List<ulong> integers = new List<ulong>();
            for (int i = 0; i < bytes.Count; i += 8)
            {
                integers.Add(BitConverter.ToUInt64(bytes.ToArray(), i));
            }

            ulong crosssum = 0;
            for(int i = 0; i < integers.Count; i++)
            {
                crosssum += integers[i];
            }

            return BitConverter.GetBytes(crosssum);
        }
    }
}
