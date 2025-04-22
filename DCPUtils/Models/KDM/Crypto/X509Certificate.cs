using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM.Crypto {
    public class X509Certificate {
        public string DnQualifier { get; }
        public string CommonName { get; }
        public string OrganizationalUnit { get; }
        public string Organization { get; }
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
