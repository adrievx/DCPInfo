using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.Composition {
    public class MainPicture : CompositionAsset {
        public FFrameRate FrameRate { get; set; }
        public Point ScreenAspectRatio { get; set; }
    }
}
