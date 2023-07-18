using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoctorGu.Encryption
{
    // https://stackoverflow.com/a/9031886/2958717
    public class CSymmetric
    {
        private static byte[] key = new byte[8] { 2, 0, 2, 3, 0, 2, 2, 1 };
        private static byte[] iv = new byte[8] { 2, 0, 2, 3, 0, 2, 2, 1 };

        public static string Encrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputBuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}
