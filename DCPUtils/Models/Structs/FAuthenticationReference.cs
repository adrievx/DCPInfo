using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FAuthenticationReference {
        /// <summary>
        /// Specifies which hashing algorithm is used to compute the digest
        /// </summary>
        public string DigestMethod { get; set; } // stored as Algorithm tag, TODO: convert this to an enum

        /// <summary>
        /// The actual hash value, computed with the algorithm specified in <see cref="DigestMethod"/>
        /// </summary>
        public string DigestValue { get; set; } // stored as base64 in the actual XML, we convert to hex
    }
}
