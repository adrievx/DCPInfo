using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCPUtils.Models;

namespace DCPInfo.Controls {
    internal class AssetListViewItem : ListViewItem {
        public DCPAssetChunk Asset { get; }

        public AssetListViewItem(DCPAssetChunk chunk, Guid uuid) : base(uuid.ToString()) {
            SubItems.Add(chunk.Path);
            SubItems.Add(chunk.VolumeIndex.ToString());
            SubItems.Add(chunk.Offset.ToString());
            SubItems.Add(chunk.Length.ToString());
        }
    }
}
