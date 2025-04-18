using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPAssetChunk {
        public string Path { get; set; }
        public int VolumeIndex { get; set; }
        public long Offset { get; set; }
        public long Length { get; set; }
    }
}
