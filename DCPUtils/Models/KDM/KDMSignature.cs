using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class KDMSignature {
        public SignedInfo SignedInfo { get; set; }
        public string SignatureValue { get; set; } // stored as base64 in the XML, we convert to hex. this contains the actual digital signature
                                                   // that was generated over <ds:SignedInfo>, which allows the recipient TMS to verify the integrity
                                                   // and authenticity of the KDM.
    }
}
