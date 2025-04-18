using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.Composition {
    public class CompositionMetadata : CompositionAsset {
        public string FullContentTitleText { get; set; }
        public int VersionNumber { get; set; }
        public string Facility {  get; set; }
        public FSoundConfiguration MainSoundConfiguration { get; set; }
        public FSampleRate MainSoundSampleRate { get; set; }
        public Point MainPictureStoredArea { get; set; }
        public Point MainPictureActiveArea { get; set; }
    }
}
