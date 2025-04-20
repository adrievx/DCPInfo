using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
