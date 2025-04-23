using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.KDM {
    public class SignedInfo {
        /// <summary>
        /// The algorithm used to normalize the XML
        /// </summary>
        public string CanonicalizationMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum

        /// <summary>
        /// The signing algorithm used by the <see cref="KDM"/>
        /// </summary>
        public string SignatureMethod { get; set; } // uses the Algorithm tag, TODO: change this to an enum

        /// <summary>
        /// The <see cref="FAuthenticationReference"/> digest for the AuthenticatedPublic block
        /// </summary>
        public FAuthenticationReference AuthenticatedPublic { get; set; }

        /// <summary>
        /// The <see cref="FAuthenticationReference"/> digest for the AuthenticatedPrivate block
        /// </summary>
        public FAuthenticationReference AuthenticatedPrivate { get; set; }
    }
}
