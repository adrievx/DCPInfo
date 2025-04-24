using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCPInfo.Util {
    internal class ListUtils {
        public static ListViewItem ToListViewItem(IEnumerable<string> values) {
            var list = values.ToList();

            if (list.Count == 0) {
                return new ListViewItem();
            }

            var item = new ListViewItem(list.First());
            for (int i = 1; i < list.Count; i++) {
                item.SubItems.Add(list[i]);
            }

            return item;
        }
    }
}
