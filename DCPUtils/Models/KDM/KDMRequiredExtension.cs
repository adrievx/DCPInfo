using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.KDM {
    public class KDMRequiredExtension {
        public Crypto.X509Certificate Recipient { get; set; }
        public Guid CompositionPlaylistId { get; set; }
        public string ContentTitleText { get; set; } // typically the name of the DCP itself (e.g. Miraculous_FTR-1_S...)
        public DateTime ContentKeysNotValidBefore { get; set; }
        public DateTime ContentKeysNotValidAfter { get; set; }
        public AuthorizedDevice AuthorizedDeviceInfo { get; set; }
        public List<FKeyId> KeyIdList { get; set; }
        public List<string> ForensicMarkFlagList { get; set; } // see https://app.box.com/s/gjf3xtan24qvcwknfkz4s6cuo7eaejnl
    }
}
