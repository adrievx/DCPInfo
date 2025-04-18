using System;
using System.Diagnostics;
using DCPUtils.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DCPUtils.Tests {
    [TestClass]
    public class DCPMetadata {
        [TestMethod]
        public void ReadUUID() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.Metadata.UUID;

            Assert.IsNotNull(value);

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadAnnotationText() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.Metadata.AnnotationText;

            Assert.IsFalse(string.IsNullOrEmpty(value));

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadVolumeCount() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.Metadata.VolumeCount;

            Assert.AreNotEqual(value, 0);

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadCreator() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.Metadata.Creator;

            Assert.IsFalse(string.IsNullOrEmpty(value));

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadIssueDate() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.Metadata.IssueDate;

            Assert.IsNotNull(value);
            Assert.AreNotEqual(value, DateTime.MinValue);

            Debug.WriteLine(value);
        }

        [TestMethod]
        public void ReadIssuer() {
            var dcp = Models.DCP.Read(Statics.DcpPath);
            var value = dcp.Metadata.Issuer;

            Assert.IsFalse(string.IsNullOrEmpty(value));

            Debug.WriteLine(value);
        }
    }
}
