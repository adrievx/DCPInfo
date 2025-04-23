using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FKeyId {
        /// <summary>
        /// The type of key
        /// </summary>
        public string KeyType { get; set; } // TODO: make this an enum instead

        /// <summary>
        /// The <see cref="Guid"/> of the key
        /// </summary>
        public Guid KeyId { get; set; }
    }
}
