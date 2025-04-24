using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPInfo.Models;
using Newtonsoft.Json;

namespace DCPInfo {
    internal class Statics {
        public static string RecentFilePath {
            get {
                string path = Path.Combine(Statics.DataPath, "recents.json");

                if (!File.Exists(path)) {
                    var empty = new List<RecentDCP>();

                    File.WriteAllText(path, JsonConvert.SerializeObject(empty));
                }

                return path;
            }
        }

        public static string DataPath {
            get {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dcpinfo");

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
    }
}
