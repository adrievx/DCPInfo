using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPUtils.Utils {
    public class UuidUtils {
        public static Guid ToGuid(string uuid) {
            return Guid.Parse(uuid.Split(':').Last());
        }
    }
}
