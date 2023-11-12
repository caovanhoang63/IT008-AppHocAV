using System.Security.Cryptography;
using System.Text;

namespace IT008_AppHocAV.Util
{
    public class Hashing
    {
        
        /// <summary>
        /// Hashing a string to a byte array with SHA256
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] CalculateSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));

            return hashValue;
        }
    }
}