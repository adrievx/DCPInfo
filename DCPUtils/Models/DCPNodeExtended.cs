using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPNodeExtended : DCPNode {
        /// <summary>
        /// The annotation text (title) of the node
        /// </summary>
        public string AnnotationText { get; set; }

        /// <summary>
        /// The date the node was issued
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// The node's issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The node's creator
        /// </summary>
        public string Creator { get; set; }
    }
}
