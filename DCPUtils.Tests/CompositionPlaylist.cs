using System;
using DCPUtils.Models;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCPUtils.Tests {
    [TestClass]
    public class CompositionPlaylist {
        [TestMethod]
        public void ReadContentTitleText() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.CompositionPlaylist.ContentTitleText;

            Assert.IsFalse(string.IsNullOrEmpty(value));

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadContentKind() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.CompositionPlaylist.ContentKind;

            Assert.IsNotNull(value);

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadContentVersion() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.CompositionPlaylist.ContentVersion;

            Assert.IsNotNull(value);

            Assert.IsNotNull(value.UUID);
            Assert.AreNotEqual(value.Version, 0);

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadRatingList() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.CompositionPlaylist.RatingList;

            Assert.IsNotNull(value);

            foreach (var rating in value) {
                Assert.IsFalse(string.IsNullOrEmpty(rating.Label));
                Assert.IsFalse(string.IsNullOrEmpty(rating.Agency));
                Assert.IsFalse(string.IsNullOrEmpty(rating.Region));
            }

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadReelList() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.CompositionPlaylist.ReelList;

            Assert.IsNotNull(value);

            foreach (var item in value) {
                Assert.IsNotNull(item.UUID);
                Assert.IsNotNull(item.MainMarkers);
                Assert.IsNotNull(item.MainPicture);
                Assert.IsNotNull(item.MainSound);
                Assert.IsNotNull(item.Metadata);
            }

            Debug.WriteLine(value);
        }
    }
}
