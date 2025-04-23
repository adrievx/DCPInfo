using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class AuthenticatedPublic {
        /// <summary>
        /// The <see cref="Guid"/> of the Key Delivery Message
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// The display name of the <see cref="KDM"/>, typically the name of the DCP
        /// </summary>
        public string AnnotationText { get; set; }

        /// <summary>
        /// The date the <see cref="KDM"/> was issued
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// The <see cref="Crypto.X509Certificate"/> of the signing entity
        /// </summary>
        public Crypto.X509Certificate Signer { get; set; }

        /// <summary>
        /// The list of <see cref="KDMRequiredExtension"/> objects that the TMS recipient must enforce
        /// </summary>
        public List<KDMRequiredExtension> RequiredExtensions { get; set; }
        //public List<KDMNonCriticalExtension> NonCriticalExtensions { get; set; } // TODO: figure this out
    }
}
