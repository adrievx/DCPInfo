using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    // uses XMLENC spec
    public class EncryptedKey {
        /// <summary>
        /// The algorithm used for the encryption (typically RSA)
        /// </summary>
        public string EncryptionMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum

        /// <summary>
        /// The algorithm used for the digest (typically SHA1)
        /// </summary>
        public string DigestMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum

        /// <summary>
        /// The RSA-encrypted session key, encrypted using the public key from the recipient TMS’s certificate, used to decrypt the actual DCP
        /// </summary>
        public string CipherValue {  get; set; } // stored as base64 (we convert it back to hex here)
    }
}
