using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FKeyId {
        public string KeyType { get; set; } // TODO: make this an enum instead
        public Guid Keyid { get; set; }
    }
}
