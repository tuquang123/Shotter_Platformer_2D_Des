using System.Security.Cryptography;
using System.Text;

namespace G2.Sdk.SecurityHelper
{
    public class HashHelper
    {
        const string stringFormat = "x2";

        public static string HashMD5(string input)
        {
            if (input != null)
            {
                MD5 md5Helper = MD5.Create();
                byte[] crypto = md5Helper.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < crypto.Length; i++)
                {
                    sBuilder.Append(crypto[i].ToString(stringFormat));
                }

                return sBuilder.ToString();
            }
            else
            {
                return null;
            }
        }

        public static string HashSHA256(string input)
        {
            if (input != null)
            {
                SHA256Managed sha256Helper = new SHA256Managed();
                byte[] crypto = sha256Helper.ComputeHash(Encoding.UTF8.GetBytes(input), 0, Encoding.UTF8.GetByteCount(input));

                StringBuilder hash = new StringBuilder();
                for (int i = 0; i < crypto.Length; i++)
                {
                    hash.Append(crypto[i].ToString(stringFormat));
                }

                return hash.ToString().ToLower();
            }
            else
            {
                return null;
            }
        }
    }
}