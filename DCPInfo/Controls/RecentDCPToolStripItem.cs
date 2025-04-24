using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCPInfo.Models;

namespace DCPInfo.Controls {
    internal class RecentDCPToolStripItem : ToolStripMenuItem {
        public RecentDCP Data { get; }

        public RecentDCPToolStripItem(RecentDCP entry) {
            this.Data = entry;

            var di = new DirectoryInfo(entry.DCPFolder);

            this.Text = di.Name;
        }
    }
}
