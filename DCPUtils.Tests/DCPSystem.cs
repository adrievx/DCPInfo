using System;
using DCPUtils.Models.Composition;
using DCPUtils.Models;
using DCPUtils.Utils;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace DCPUtils.Tests {
    [TestClass]
    public class DCPSystem {
        [TestMethod]
        public void DCP_Get_GetMainPictureFilename() {
            var dcp = DCP.Read(Statics.DcpPath);

            var uuid = dcp.CompositionPlaylist.ReelList.First().MainPicture.UUID;
            string filename = PackListUtils.GetFileNameFromPackagingList(dcp.PackListPath, uuid);
            var filePath = Path.Combine(Statics.DcpPath, filename);

            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsTrue(File.Exists(filePath));

            Debug.WriteLine(filename);
        }

        [TestMethod]
        public void DCP_Get_GetMainSoundFilename() {
            var dcp = DCP.Read(Statics.DcpPath);

            var uuid = dcp.CompositionPlaylist.ReelList.First().MainSound.UUID;
            string filename = PackListUtils.GetFileNameFromPackagingList(dcp.PackListPath, uuid);
            var filePath = Path.Combine(Statics.DcpPath, filename);

            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsTrue(File.Exists(filePath));

            Debug.WriteLine(filename);
        }

        [TestMethod]
        public void DCP_Get_GetAssetMapFilename() {
            var dcp = DCP.Read(Statics.DcpPath);

            string filename = dcp.AssetMapPath;
            var filePath = Path.Combine(Statics.DcpPath, filename);

            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsTrue(File.Exists(filePath));

            Debug.WriteLine(filename);
        }

        [TestMethod]
        public void DCP_Get_GetCompositionPlaylistFilename() {
            var dcp = DCP.Read(Statics.DcpPath);

            string filename = dcp.CompositionPlaylistPath;
            var filePath = Path.Combine(Statics.DcpPath, filename);

            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsTrue(File.Exists(filePath));

            Debug.WriteLine(filename);
        }

        [TestMethod]
        public void DCP_Get_GetPackListPath() {
            var dcp = DCP.Read(Statics.DcpPath);

            string filename = dcp.PackListPath;
            var filePath = Path.Combine(Statics.DcpPath, filename);

            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsTrue(File.Exists(filePath));

            Debug.WriteLine(filename);
        }

        [TestMethod]
        public void DCP_Get_GetCplPathFromPackList() {
            var dcp = DCP.Read(Statics.DcpPath);

            string filename = dcp.PackListPath;
            string packListPath = Path.Combine(Statics.DcpPath, filename);

            var cplUuid = PackListUtils.GetCplUuid(packListPath);
            string cplFilename = PackListUtils.GetFileNameFromPackagingList(packListPath, cplUuid);
            string cplPath = Path.Combine(Statics.DcpPath, cplFilename);

            Assert.IsFalse(string.IsNullOrEmpty(cplPath));
            Assert.IsTrue(File.Exists(cplPath));

            Debug.WriteLine(cplFilename);
        }

        [TestMethod]
        public void DCP_VerifyChecksum() {
            var dcp = DCP.Read(Statics.DcpPath);

            Assert.IsTrue(dcp.Verify());
        }
    }
}
