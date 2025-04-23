using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPAssetChunk {
        /// <summary>
        /// The filename of the chunk
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The volume index, typically 1 unless the DCP has multiple volumes
        /// </summary>
        public int VolumeIndex { get; set; }

        /// <summary>
        /// The chunk's offset (if there's more than one chunk in a single asset, otherwise this is 0)
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// The length (in bytes) of the chunk
        /// </summary>
        public long Length { get; set; }
    }
}
