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
        /// 10, center only
        /// </summary>
        Mono,
        /// <summary>
        /// 20, standard stereo
        /// </summary>
        Stereo,
        /// <summary>
        /// 21, stereo with center
        /// </summary>
        StereoCent,
        /// <summary>
        /// 31, stereo with center and LFE
        /// </summary>
        StereoCentLFE,
        /// <summary>
        /// 51, 5.1 surround
        /// </summary>
        Dolby51,
        /// <summary>
        /// 61, 5.1 surround with hearing impairment track
        /// </summary>
        Dolby51HI,
        /// <summary>
        /// 71, 7.1 surround
        /// </summary>
        Dolby71,
        /// <summary>
        /// 911, 5.1 surround with hearing impairment/visual impairment tracks
        /// </summary>
        Dolby51HIViN,
        /// <summary>
        /// 714, dolby atmos
        /// </summary>
        Atmos
    }
}
