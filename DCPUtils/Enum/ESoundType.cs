using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    /// <summary>
    /// All the different sound types that are supported in DCPs.
    /// </summary>
    public enum ESoundType {
        /// <summary>
        /// 10, Center channel only
        /// </summary>
        Mono,

        /// <summary>
        /// 20, Standard stereo
        /// </summary>
        Stereo,

        /// <summary>
        /// 21, Stereo with Center channel
        /// </summary>
        StereoCent,

        /// <summary>
        /// 31, Stereo with Center and LFE
        /// </summary>
        StereoCentLFE,

        /// <summary>
        /// 51, 5.1 Surround
        /// </summary>
        Dolby51,

        /// <summary>
        /// 61, 5.1 Surround with hearing impairment track
        /// </summary>
        Dolby51HI,

        /// <summary>
        /// 71, 7.1 Surround
        /// </summary>
        Dolby71,

        /// <summary>
        /// 911, 5.1 Surround with hearing impairment/visual impairment tracks
        /// </summary>
        Dolby51HIViN,

        /// <summary>
        /// 714, Dolby Atmos
        /// </summary>
        Atmos
    }
}
