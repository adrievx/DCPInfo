using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Enum {
    public enum EContentKind {
        Feature, // stored as 'feature', FTR
        Trailer, // stored as 'trailer', TLR
        Teaser, // stored as 'teaser', TSR
        Rating, // stored as 'rating', RTG
        Policy, // stored as 'policy', POL
        Advert, // stored as 'advertisement', ADV
        Short, // stored as 'short', EPS/CLP
        PSA, // stored as 'psa', PSA
        Test, // stored as 'test', TST
        Other, // anything that doesn't conform to the above
    }
}
