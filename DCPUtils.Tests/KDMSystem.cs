using System;
using System.Diagnostics;
using DCPUtils.Models;
using DCPUtils.Models.KDM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DCPUtils.Tests {
    [TestClass]
    public class KDMSystem {
        [TestMethod]
        public void TestKDM() {
            var kdm = KDM.Read(Statics.KdmPath);

            Assert.IsNotNull(kdm);
            Assert.IsNotNull(kdm.AuthenticatedPublic);
            Assert.IsNotNull(kdm.AuthenticatedPrivate);
            Assert.IsNotNull(kdm.Signature);

            Assert.IsNotNull(kdm.AuthenticatedPublic.RequiredExtensions);
            Assert.IsNotNull(kdm.AuthenticatedPublic.Signer);
            Assert.AreNotEqual(DateTime.MinValue, kdm.AuthenticatedPublic.IssueDate);

            Debug.WriteLine(JsonConvert.SerializeObject(kdm, Formatting.Indented));
        }

        [TestMethod]
        public void VerifyKDM() {
            var dcp = DCP.Read(Statics.DcpPath);
            var kdm = KDM.Read(Statics.KdmPath);

            Assert.IsNotNull(dcp);
            Assert.IsNotNull(kdm);

            bool result = kdm.Verify(dcp, Statics.TmsCert, Statics.TmsKey);

            Assert.IsTrue(result);
        }
    }
}
