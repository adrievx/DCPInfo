using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models.KDM {
    public class AuthenticatedPublic {
        public Guid MessageId { get; set; }
        public string AnnotationText { get; set; }
        public DateTime IssueDate { get; set; }
        public Crypto.X509Certificate Signer { get; set; }
        public List<KDMRequiredExtension> RequiredExtensions { get; set; }
        //public List<KDMNonCriticalExtension> NonCriticalExtensions { get; set; } // TODO: figure this out
    }
}
