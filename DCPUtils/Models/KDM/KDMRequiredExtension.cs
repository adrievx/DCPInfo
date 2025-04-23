using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.KDM {
    public class KDMRequiredExtension {
        /// <summary>
        /// The <see cref="Crypto.X509Certificate"/> of the recipient TMS
        /// </summary>
        public Crypto.X509Certificate Recipient { get; set; }

        /// <summary>
        /// The <see cref="Guid"/> of the <see cref="CompositionPlaylist"/> that the KDM is for
        /// </summary>
        public Guid CompositionPlaylistId { get; set; }

        /// <summary>
        /// The <see cref="KDM"/>'s title, typically the name of the DCP itself (e.g. Miraculous_FTR-1_S...)
        /// </summary>
        public string ContentTitleText { get; set; }

        /// <summary>
        /// The earliest date the <see cref="KDM"/> can be used
        /// </summary>
        public DateTime ContentKeysNotValidBefore { get; set; }

        /// <summary>
        /// The last date the <see cref="KDM"/> can be used
        /// </summary>
        public DateTime ContentKeysNotValidAfter { get; set; }

        /// <summary>
        /// Information about the TMS that's authorized to use the current <see cref="KDM"/> on a specific <see cref="DCP"/>
        /// </summary>
        public AuthorizedDevice AuthorizedDeviceInfo { get; set; }

        /// <summary>
        /// The list of decryption keys used to decrypt a specific <see cref="DCP"/>
        /// </summary>
        public List<FKeyId> KeyIdList { get; set; }

        /// <summary>
        /// List of forensic watermarks present in the <see cref="DCP"/>
        /// </summary>
        public List<string> ForensicMarkFlagList { get; set; } // see https://app.box.com/s/gjf3xtan24qvcwknfkz4s6cuo7eaejnl
    }
}
