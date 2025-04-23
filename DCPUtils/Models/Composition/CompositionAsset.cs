using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Misc;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.Composition {
    public class CompositionAsset : DCPNode {
        /// <summary>
        /// The framerate of the composition asset, stored as a <see cref="FrameRate"/>
        /// </summary>
        public FrameRate EditRate { get; set; }

        /// <summary>
        /// The full length of the <see cref="CompositionAsset"/>, measured in edit units
        /// </summary>
        public int IntrinsicDuration { get; set; }

        /// <summary>
        /// The <see cref="KDM.KDM"/> decryption key's <see cref="Guid"/> (if the DCP isn't encrypted, this is left null)
        /// </summary>
        public Nullable<FKeyId> KeyId { get; set; }
    }
}