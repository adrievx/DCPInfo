using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Enum;

namespace DCPUtils.Models.Structs {
    public struct FRating {
        /// <summary>
        /// The rating agency, see <see href="https://en.wikipedia.org/wiki/Category:Motion_picture_rating_systems">Motion picture rating systems</see>
        /// </summary>
        public ERatingAgency Agency;

        /// <summary>
        /// The rating given by the <see cref="Agency"/>
        /// </summary>
        public string Label;

        /// <summary>
        /// The region that the rating is applicable in
        /// </summary>
        public string Region; // TOOD: make this an enum
    }
}
