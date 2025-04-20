using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Utils;

namespace DCPUtils.Models.Composition {
    public class PrimaryCompositionAsset : CompositionAsset {
        public long EntryPoint { get; set; }
        public long Duration { get; set; } // in frames
        public string Hash { get; set; } // in an actual CPL this is base64 encoded SHA1, we decode it for ease of use

        /// <summary>
        /// Verifies the SHA1 hash of a specified MXF.
        /// </summary>
        /// <param name="packList"></param>
        /// <param name="mxfPath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool Verify(string packList, string mxfPath) {
            if(File.Exists(mxfPath)) {
                var expectedHash = PackListUtils.GetHashFromPackagingList(packList, UUID);
                var resultHash = CryptoUtils.CalculateSHA1(mxfPath);

                return expectedHash.ToLower() == resultHash.ToLower();
            }
            else {
                throw new FileNotFoundException("Unable to find specified MXF file.");
            }
        }
    }
}
