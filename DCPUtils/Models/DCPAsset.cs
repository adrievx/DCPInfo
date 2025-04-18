using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPAsset : DCPNode {
        public bool PackingList { get; set; }
        public List<DCPAssetChunk> Chunks { get; set; }
    }
}
