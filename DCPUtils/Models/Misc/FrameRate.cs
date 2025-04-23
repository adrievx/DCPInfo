using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Interfaces;

namespace DCPUtils.Models.Misc {
    public class FrameRate : IDenominatorModel {
        /// <summary>
        /// The framerate, stored as an integer
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// The framerate's denominator, used to support non-integer framerates
        /// </summary>
        public int Denominator { get; set; }

        /// <summary>
        /// Return the real framerate of the video stream
        /// </summary>
        /// <returns></returns>
        public float GetRealValue() {
            return this.Rate / this.Denominator;
        }
    }
}
