using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.Structs {
    public struct FContentVersion {
        /// <summary>
        /// The <see cref="Guid"/> of the content
        /// </summary>
        public Guid UUID { get; set; }

        /// <summary>
        /// The version number of the content
        /// </summary>
        public int Version { get; set; } // stored as LabelText
    }
}
