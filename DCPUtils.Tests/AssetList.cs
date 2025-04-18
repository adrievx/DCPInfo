using System;
using DCPUtils.Models;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DCPUtils.Tests {
    [TestClass]
    public class AssetList {
        [TestMethod]
        public void GetAssetList() {
            var dcp = Models.DCP.Read(Statics.DcpPath);

            Debug.WriteLine(JsonConvert.SerializeObject(dcp.Assets, Formatting.Indented)); // serialized for ease of reading
        }
    }
}
