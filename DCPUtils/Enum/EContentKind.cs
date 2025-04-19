using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    public enum EContentKind {
        /// <summary>
        /// Content stored as 'feature', FTR
        /// </summary>
        Feature,
        /// <summary>
        /// Content stored as 'trailer', TLR
        /// </summary>
        Trailer,
        /// <summary>
        /// Content stored as 'teaser', TSR
        /// </summary>
        Teaser,
        /// <summary>
        /// Content stored as 'rating', RTG
        /// </summary>
        Rating,
        /// <summary>
        /// Content stored as 'policy', POL
        /// </summary>
        Policy,
        /// <summary>
        /// Content stored as 'advertisement', ADV
        /// </summary>
        Advert,
        /// <summary>
        /// Content stored as 'short', EPS/CLP
        /// </summary>
        Short,
        /// <summary>
        /// Content stored as 'psa', PSA
        /// </summary>
        PSA,
        /// <summary>
        /// Content stored as 'test', TST
        /// </summary>
        Test,
        /// <summary>
        /// Anything that doesn't conform to the standard content types.
        /// </summary>
        Other,
    }
}
