using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities.Encoders;

namespace DCPUtils.Utils {
    public class EncodingUtils {
        /// <summary>
        /// Decodes a string from base 64 to a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="convertHex"></param>
        /// <returns></returns>
        public static string Base64Decode(string input, bool convertHex = true) {
            byte[] data = Convert.FromBase64String(input);

            if (convertHex) {
                var output = new StringBuilder(data.Length * 2);
                
                foreach (byte b in data) {
                    output.Append(b.ToString("x2"));
                }

                return output.ToString();
            }
            else {
                return Encoding.UTF8.GetString(data);
            }
        }

        public static byte[] HexToBytes(string hex) {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }
    }
}
