using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Utils;

namespace DCPUtils.Models.Composition {
    public class PrimaryCompositionAsset : CompositionAsset {
        /// <summary>
        /// The frame at which the asset starts at
        /// </summary>
        public long EntryPoint { get; set; }

        /// <summary>
        /// The duration of the asset in frames
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Hex-encoded SHA1 hash of the asset
        /// </summary>
        public string Hash { get; set; }
    }
}
