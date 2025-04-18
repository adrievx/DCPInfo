using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Enum;

namespace DCPUtils.Models.Structs {
    public struct FSoundConfiguration {
        public ESoundType Type { get; set; } // 5.1, 7.1, etc.
        public List<ESoundChannel> Channels { get; set; } // e.g. L, R, C, LFE, Ls, Rs
    }
}
