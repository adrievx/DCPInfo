using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FSampleRate {
        /// <summary>
        /// The sample rate of the audio, see <see href="https://en.wikipedia.org/wiki/Sampling_(signal_processing)#Audio_sampling">Sampling (signal processing)<see/>
        /// </summary>
        public int SampleRate {  get; set; }

        /// <summary>
        /// The denominator used for the sample rate
        /// </summary>
        public int Denominator { get; set; }
    }
}
