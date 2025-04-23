using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FFrameRate {
        /// <summary>
        /// The framerate, stored as an integer
        /// </summary>
        public int FrameRate { get; set; }

        /// <summary>
        /// The framerate's denominator, used to support non-integer framerates
        /// </summary>
        public int Denominator { get; set; }
    }
}
