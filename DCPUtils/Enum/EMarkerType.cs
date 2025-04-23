using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    public enum EMarkerType {
        /// <summary>
        /// First Frame Of Composition
        /// </summary>
        FFOC,

        /// <summary>
        /// Last Frame Of Composition
        /// </summary>
        LFOC,

        /// <summary>
        /// First Frame of Title Credits
        /// </summary>
        FFTC,

        /// <summary>
        /// Last Frame of Title Credits
        /// </summary>
        LFTC,

        /// <summary>
        /// First Frame Of Intermission
        /// </summary>
        FFOI,

        /// <summary>
        /// Last Frame Of Intermission
        /// </summary>
        LFOI,

        /// <summary>
        /// First Frame Of Ratings Band
        /// </summary>
        FFOB,

        /// <summary>
        /// Last Frame Of Ratings Band
        /// </summary>
        LFOB,

        /// <summary>
        /// First Frame Of End Credits
        /// </summary>
        FFEC,

        /// <summary>
        /// Last Frame Of End Credits
        /// </summary>
        LFEC,

        /// <summary>
        /// First Frame Of Moving Credits
        /// </summary>
        FFMC,

        /// <summary>
        /// Last Frame Of Moving Credits
        /// </summary>
        LFMC
    }
}
