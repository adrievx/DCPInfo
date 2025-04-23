using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM.Crypto {
    public class X509Certificate {
        /// <summary>
        /// Qualifier for the <see href="https://eclipse.dev/amlen/docs/Reference/_Topics/sy10570_.html">Distinguished Name (DN)</see>
        /// </summary>
        public string DnQualifier { get; }

        /// <summary>
        /// The certificate's Common Name, typically the TMS server's serial number (e.g. RMB SPB MDE FMA.Dolby-CP850-F4945017)
        /// </summary>
        public string CommonName { get; }

        /// <summary>
        /// The certificate's OU, in the case of Dolby systems this is the model of the TMS server (e.g. CP850)
        /// </summary>
        public string OrganizationalUnit { get; }

        /// <summary>
        /// The certificate's Organization, typically the issuer
        /// </summary>
        public string Organization { get; }

        /// <summary>
        /// The serial number of the certificate
        /// </summary>
        public string SerialNumber { get; }

        public X509Certificate(string dn, string serial) {
            var spl = dn.Split(',');

            foreach (var item in spl) {
                var index = item.IndexOf('=');
                if (index < 0) {
                    continue;
                }

                var key = item.Substring(0, index).Trim();
                var value = item.Substring(index + 1).Trim();

                switch (key) {
                    case "dnQualifier":
                        this.DnQualifier = value;
                        break;
                    case "CN":
                        this.CommonName = value;
                        break;
                    case "OU":
                        this.OrganizationalUnit = value;
                        break;
                    case "O":
                        this.Organization = value;
                        break;
                }
            }

            this.SerialNumber = serial;
        }
    }
}
