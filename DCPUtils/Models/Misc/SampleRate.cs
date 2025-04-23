using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Interfaces;

namespace DCPUtils.Models.Misc {
    public class SampleRate : IDenominatorModel {
        /// <summary>
        /// The sample rate of the audio, see <see href="https://en.wikipedia.org/wiki/Sampling_(signal_processing)#Audio_sampling">Sampling (signal processing)<see/>
        /// </summary>
        public int Rate {  get; set; }

        /// <summary>
        /// The denominator used for the sample rate
        /// </summary>
        public int Denominator { get; set; }

        /// <summary>
        /// Return the real sample rate of the audio stream
        /// </summary>
        /// <returns></returns>
        public float GetRealValue() {
            return this.Rate / this.Denominator;
        }
    }
}
