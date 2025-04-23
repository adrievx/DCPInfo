using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPAsset : DCPNode {
        /// <summary>
        /// If the current asset is a PKL or not
        /// </summary>
        public bool PackingList { get; set; }

        /// <summary>
        /// The list of <see cref="DCPAssetChunk"/> chunks present in the asset
        /// </summary>
        public List<DCPAssetChunk> Chunks { get; set; }
    }
}
