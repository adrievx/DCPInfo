using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;

namespace DCPUtils.Models.Composition {
    public class CompositionMetadata : CompositionAsset {
        /// <summary>
        /// The title text of the <see cref="DCP"/>
        /// </summary>
        public string FullContentTitleText { get; set; }

        /// <summary>
        /// Version number of the <see href="https://cinepedia.com/packaging/composition-playlist/">Composition Playlist</see> (for OV this is typically '1')
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// The <see href="https://registry-page.isdcf.com/facilities/">tag of the facility</see> that mastered the DCP
        /// </summary>
        public string Facility {  get; set; }

        /// <summary>
        /// The <see cref="FSoundConfiguration"/> of the <see cref="CompositionPlaylist"/>
        /// </summary>
        public FSoundConfiguration MainSoundConfiguration { get; set; }

        /// <summary>
        /// The <see href="https://www.travsonic.com/preparing-audio-for-dcp/">sample rate</see> of the <see cref="CompositionPlaylist"/>
        /// </summary>
        public FSampleRate MainSoundSampleRate { get; set; }

        /// <summary>
        /// The resolution of the stored image in the video MXF
        /// </summary>
        public Point MainPictureStoredArea { get; set; }

        /// <summary>
        /// The resolution of the displayed image
        /// </summary>
        public Point MainPictureActiveArea { get; set; }
    }
}
