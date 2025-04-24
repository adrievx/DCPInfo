using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCPInfo.Util {
    internal class ExceptionHandler {
        public static void Handle(Exception ex) {
            MessageBox.Show(ex.Message, "DCPInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}