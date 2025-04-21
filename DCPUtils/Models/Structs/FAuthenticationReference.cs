using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FAuthenticationReference {
        public string DigestMethod { get; set; } // stored as Algorithm tag, TODO: convert this to an enum
        public string DigestValue { get; set; } // stored as base64 in the actual XML, we convert to hex
    }
}
