using System;
using System.Diagnostics;
using DCPUtils.Models.KDM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DCPUtils.Tests {
    [TestClass]
    public class KDMSystem {
        [TestMethod]
        public void TestKDM() {
            string kdmPath = "D:\\work\\DCPInfo\\temp\\EncryptTest\\KDM\\KDM_DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV_SYNDEXTEST_DCPInfo.xml";

            var kdm = KDM.Read(kdmPath);

            Assert.IsNotNull(kdm);
            Assert.IsNotNull(kdm.AuthenticatedPublic);
            Assert.IsNotNull(kdm.AuthenticatedPrivate);
            Assert.IsNotNull(kdm.Signature);

            Assert.IsNotNull(kdm.AuthenticatedPublic.RequiredExtensions);
            Assert.IsNotNull(kdm.AuthenticatedPublic.Signer);
            Assert.AreNotEqual(DateTime.MinValue, kdm.AuthenticatedPublic.IssueDate);

            Debug.WriteLine(JsonConvert.SerializeObject(kdm, Formatting.Indented));
        }
    }
}
