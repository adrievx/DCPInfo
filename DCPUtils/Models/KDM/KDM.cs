using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class KDM {
        public AuthenticatedPublic AuthenticatedPublic { get; set; }
        public List<EncryptedKey> AuthenticatedPrivate { get; set; }
        public KDMSignature Signature { get; set; }

        public bool Verify(DCP dcp, X509Certificate tmsCertificate) {
            // TODO: implement
            throw new NotImplementedException();
        }

        public static KDM Read(string kdmPath) {

        }
    }
}
