using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Enum;

namespace DCPUtils.Models.Composition {
    public class Marker {
        /// <summary>
        /// The marker's identifier (e.g. FFOC), see the Markers section in <see href="https://hpaonline.com/wp-content/uploads/2022/07/Deluxe_Source-and-DCP_Delivery_Specifications_v5-11_20220314-2.pdf">Recommended Guidelines for DCP Content Delivery</see>
        /// </summary>
        public EMarkerType Label { get; set; }

        /// <summary>
        /// The frame offset of the marker
        /// </summary>
        public long Offset { get; set; }
    }
}
