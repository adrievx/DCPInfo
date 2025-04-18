using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Models {
    public class DCPNodeExtended : DCPNode {
        public string AnnotationText { get; set; }
        public DateTime IssueDate { get; set; }
        public string Issuer { get; set; }
        public string Creator { get; set; }
    }
}
