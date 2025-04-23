using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class AuthorizedDevice {
        /// <summary>
        /// The <see cref="Guid"/> of the authorized device
        /// </summary>
        public Guid DeviceListIdentifier { get; set; }

        /// <summary>
        /// The description of the playback device, typically the model number or other identifier
        /// </summary>
        public string DeviceListDescription { get; set; }

        /// <summary>
        /// The list of SHA-1 hashes of the X509 certs used by the recipient TMS
        /// </summary>
        public List<string> DeviceList { get; set; }
    }
}
