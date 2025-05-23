﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPInfo.Models;
using Newtonsoft.Json;

namespace DCPInfo.Util {
    internal class RecentDCPManager {
        private static string recentsPath => Statics.RecentFilePath;
        private const int maxEntries = 10;

        public static List<RecentDCP> Load() {
            if (!File.Exists(recentsPath)) {
                return new List<RecentDCP>();
            }

            return JsonConvert.DeserializeObject<List<RecentDCP>>(File.ReadAllText(recentsPath));
        }

        public static void Save(List<RecentDCP> data) {
            File.WriteAllText(recentsPath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void Update(RecentDCP entry) {
            var data = Load();
            int index = data.FindIndex(e => e.DCPFolder == entry.DCPFolder);

            if (index != -1) {
                data[index] = entry;
            }

            Save(data);
        }

        public static void Remove(RecentDCP entry) {
            var data = Load();

            data.RemoveAll(e => e.DCPFolder == entry.DCPFolder);

            Save(data);
        }

        public static void MoveToEnd(RecentDCP entry) {
            var data = Load();

            data.RemoveAll(e => e.DCPFolder == entry.DCPFolder);
            data.Add(entry);

            if (data.Count > maxEntries) {
                data = data.Skip(data.Count - maxEntries).ToList();
            }

            Save(data);
        }

        public static RecentDCP Get(string dcpFolder) {
            return Load().FirstOrDefault(e => e.DCPFolder == dcpFolder);
        }

        public static void Add(RecentDCP entry) {
            var data = Load();

            data.RemoveAll(e => e.DCPFolder == entry.DCPFolder);
            data.Add(entry);

            if (data.Count > maxEntries) {
                data = data.Skip(data.Count - maxEntries).ToList();
            }

            Save(data);
        }
    }
}
