using System;
using System.Diagnostics;
using DCPUtils.Models;
using DCPUtils.Models.KDM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DCPUtils.Tests {
    [TestClass]
    public class KDMSystem {
        private string kdmPath = "D:\\work\\DCPInfo\\temp\\EncryptTest\\KDM\\KDM_DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV_SYNDEXTEST_DCPInfo.xml";
        private string dcpPath = "D:\\work\\DCPInfo\\temp\\EncryptTest\\DIEncryptTest_TST-1-Temp-Pre_F-178_EN-XX_INT-TL_20_2K_SYN_20250421_SYN_SMPTE_OV";
        private string tmsKeyPub = "D:\\work\\DCPInfo\\keys\\dev-250421\\tms-certificate.crt";
        private string tmsKeyPrv = "D:\\work\\DCPInfo\\keys\\dev-250421\\tms-private.key";

        [TestMethod]
        public void TestKDM() {
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

        [TestMethod]
        public void VerifyKDM() {
            var dcp = DCP.Read(dcpPath);
            var kdm = KDM.Read(kdmPath);

            Assert.IsNotNull(dcp);
            Assert.IsNotNull(kdm);

            bool result = kdm.Verify(dcp, tmsKeyPub, tmsKeyPrv);

            Assert.IsTrue(result);
        }
    }
}
