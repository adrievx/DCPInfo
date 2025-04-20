using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Utils {
    public class CryptoUtils {
        /// <summary>
        /// Calculates the <see cref="SHA1"/> hash of a file and outputs it as a string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string CalculateSHA1(string filePath) {
            using (var stream = File.OpenRead(filePath)) {
                using (var sha = SHA1.Create()) {
                    byte[] hashBytes = sha.ComputeHash(stream);
                    var sb = new StringBuilder();

                    foreach (byte b in hashBytes) {
                        sb.Append(b.ToString("x2"));
                    }

                    return sb.ToString();
                }
            }
        }
    }
}
