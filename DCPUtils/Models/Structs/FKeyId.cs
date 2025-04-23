using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Enum;

namespace DCPUtils.Models.Structs {
    public struct FKeyId {
        /// <summary>
        /// The type of key
        /// </summary>
        public EKdmKeyType KeyType { get; set; }

        /// <summary>
        /// The <see cref="Guid"/> of the key
        /// </summary>
        public Guid KeyId { get; set; }
    }
}
