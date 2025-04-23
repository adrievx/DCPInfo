using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    /// <summary>
    /// All the different sound channels that are supported in DCPs.
    /// </summary>
    public enum ESoundChannel {
        /// <summary>
        /// Sound channel for the left front speaker.
        /// </summary>
        Left,

        /// <summary>
        /// Sound channel for the right front speaker.
        /// </summary>
        Right,

        /// <summary>
        /// Sound channel for the center front speaker.
        /// </summary>
        Center,

        /// <summary>
        /// Sound channel for low frequency effects (subwoofer).
        /// </summary>
        LFE,

        /// <summary>
        /// Sound channel for the left surround speaker.
        /// </summary>
        LeftSurround,

        /// <summary>
        /// Sound channel for the right surround speaker.
        /// </summary>
        RightSurround,

        /// <summary>
        /// Sound channel for the left rear surround speaker.
        /// </summary>
        LeftRearSurround,

        /// <summary>
        /// Sound channel for the right rear surround speaker.
        /// </summary>
        RightRearSurround,

        /// <summary>
        /// Sound channel for assistive listening technologies.
        /// </summary>
        HearingImpairment,

        /// <summary>
        /// Sound channel for additional narration.
        /// </summary>
        VisualImpairmment,

        /// <summary>
        /// Channel for the Descriptive Video Service.
        /// </summary>
        AudioDescription,

        /// <summary>
        /// Sound channel for the likes of hearing loops, FM systems and cochlear implants.
        /// </summary>
        OtherHearingImpairment,

        /// <summary>
        /// Sound channel for the likes of sign language interpretation, and other visual aids.
        /// </summary>
        OtherVisionImpairment
    }
}
