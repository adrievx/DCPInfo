using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.Composition {
    public class CompositionAsset : DCPNode {
        public FFrameRate EditRate { get; set; }
        public int IntrinsicDuration { get; set; }
    }
}
