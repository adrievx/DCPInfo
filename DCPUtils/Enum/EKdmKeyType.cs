using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    public enum EKdmKeyType {
        /// <summary>
        /// Media Decryption Authentication Key, used to authenticate the playback device
        /// </summary>
        MDAK,

        /// <summary>
        /// Media Decryption Initialization Key, used to initialize the process of content decryption
        /// </summary>
        MDIK,

        /// <summary>
        /// Media File Key, the actual key used to decrypt the content (MainPicture, MainSound, etc.)
        /// </summary>
        MFK,

        /// <summary>
        /// Remote Encryption Key, used to encrypt content for remote decryption
        /// </summary>
        REK,

        /// <summary>
        /// Encrypted Session Key, a wrapper key used when decrypting session keys
        /// </summary>
        ESK,

        /// <summary>
        /// Decryption Initialization Key, generalized <see cref="MDIK"/>
        /// </summary>
        DIK
    }
}
