using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class AuthorizedDevice {
        public Guid DeviceListIdentifier { get; set; }
        public string DeviceListDescription { get; set; }
        public List<string> DeviceList { get; set; } // list of SHA-1 hashes of the X509 certs used by the recipient TMS (encoded as base64 in the actual XML but we convert back to hex, e.g. '<CertificateThumbprint>2jmj7l5rSw0yVb/vlWAYkK/YBwk=</CertificateThumbprint>')
    }
}
