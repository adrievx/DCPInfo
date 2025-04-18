using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    public enum ESoundType {
        Mono,           // 10, center only
        Stereo,         // 20, standard stereo
        StereoCent,     // 21, stereo with center
        StereoCentLFE,  // 31, stereo with center and LFE
        Dolby51,        // 51, 5.1 surround
        Dolby51HI,      // 61, 5.1 surround with hearing impairment track
        Dolby71,        // 71, 7.1 surround
        Dolby51HIViN,   // 911, 5.1 surround with hearing impairment/visual impairment tracks
        Atmos           // 714, dolby atmos
    }
}
