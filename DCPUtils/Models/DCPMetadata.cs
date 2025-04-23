using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPMetadata : DCPNodeExtended {
        /// <summary>
        /// The volume count of the DCP (this is typically 1, unless the DCP is split across multiple volumes)
        /// </summary>
        public int VolumeCount { get; internal set; }
    }
}
