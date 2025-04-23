using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class KDMSignature {
        /// <summary>
        /// Information about the <see cref="KDM"/>'s digital signature
        /// </summary>
        public SignedInfo SignedInfo { get; set; }

        /// <summary>
        /// The value of the signature, which allows the recipient TMS to verify the integrity and authenticity of the KDM
        /// </summary>
        public string SignatureValue { get; set; } // stored as base64 in the XML, we convert to hex
    }
}
