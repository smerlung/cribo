using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shade.Objects
{
    public static class Crypto
    {
        public static byte[] Encrypt(byte[] data, byte[] hash)
        {
            List<byte> result = new List<byte>();
            int hashindex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                result.Add((byte)(data[i] - hash[hashindex]));
                hashindex++;
                hashindex %= hash.Length;
            }

            return result.ToArray();
        }

        public static byte[] Decrypt(byte[] data, byte[] hash)
        {
            List<byte> result = new List<byte>();
            int hashindex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                result.Add((byte)(data[i] + hash[hashindex]));
                hashindex++;
                hashindex %= hash.Length;
            }

            return result.ToArray();
        }

        public static string Encrypt(string plaintext, byte[] hash)
        {
            byte[] data = UnicodeEncoding.UTF8.GetBytes(plaintext);
            byte[] encrypted = Crypto.Encrypt(data, hash);
            return UnicodeEncoding.UTF8.GetString(encrypted);
        }

        public static string Decrypt(string ciphertext, byte[] hash)
        {
            byte[] data = UnicodeEncoding.UTF8.GetBytes(ciphertext);
            byte[] decrypted = Crypto.Decrypt(data, hash);
            return UnicodeEncoding.UTF8.GetString(decrypted);
        }
    }
}
