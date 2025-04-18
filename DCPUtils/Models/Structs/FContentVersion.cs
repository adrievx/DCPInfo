using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FContentVersion {
        public Guid UUID { get; set; }
        public int Version { get; set; } // stored as LabelText
    }
}
