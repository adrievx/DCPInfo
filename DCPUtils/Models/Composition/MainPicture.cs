using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Misc;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.Composition {
    public class MainPicture : CompositionAsset {
        /// <summary>
        /// The framerate of the video MXF, stored as a <see cref="FrameRate"/>
        /// </summary>
        public FrameRate FrameRate { get; set; }

        /// <summary>
        /// The aspect ratio of the video MXF
        /// </summary>
        public Point ScreenAspectRatio { get; set; }
    }
}
