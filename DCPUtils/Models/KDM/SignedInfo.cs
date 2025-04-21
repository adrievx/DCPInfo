using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.KDM {
    public class SignedInfo {
        public string CanonicalizationMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum
        public string SignatureMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum
        public FAuthenticationReference AuthenticatedPublic { get; set; }
        public FAuthenticationReference AuthenticatedPrivate { get; set; }
    }
}
