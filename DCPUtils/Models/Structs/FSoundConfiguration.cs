using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Enum;

namespace DCPUtils.Models.Structs {
    public struct FSoundConfiguration {
        /// <summary>
        /// The <see cref="ESoundType"/> that's used for the audio
        /// </summary>
        public ESoundType Type { get; set; } // 5.1, 7.1, etc.

        /// <summary>
        /// The list of sound channels present in the audio stream, see <see href="https://en.wikipedia.org/wiki/Surround_sound#Standard_speaker_channels">Surround sound - Standard speaker channels</see>
        /// </summary>
        public List<ESoundChannel> Channels { get; set; } // e.g. L, R, C, LFE, Ls, Rs
    }
}
