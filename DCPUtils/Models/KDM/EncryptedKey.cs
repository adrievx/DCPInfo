using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    // uses XMLENC spec
    public class EncryptedKey {
        public string EncryptionMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum
        public string DigestMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum
        public string CipherValue {  get; set; } // stored as base64 (we convert it back to hex here), this is the RSA-encrypted session key,
                                                 // encrypted using the public key from the recipient (TMS)’s certificate, used to decrypt the actual DCP
    }
}
