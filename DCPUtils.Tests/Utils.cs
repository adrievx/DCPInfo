using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DCPUtils.Enum;
using DCPUtils.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DCPUtils.Tests {
    [TestClass]
    public class Utils {
        [TestMethod]
        public void UuidUtils_ToGuid() {
            string guid = "bd72f449-9acc-477f-bd44-09b0603e58a3";
            string input = $"urn:uuid:{guid}";

            var guidOutStr = UuidUtils.ToGuid(input);
            Assert.AreEqual(guid, guidOutStr.ToString());
        }

        [TestMethod]
        public void XmlParserUtils_ParseSoundConfiguration() {
            string input = $"20/L,R";

            var output = XmlParserUtils.ParseSoundConfiguration(input);

            Assert.AreEqual(output.Type, ESoundType.Stereo);
            Assert.AreEqual(output.Channels.First(), ESoundChannel.Left);
            Assert.AreEqual(output.Channels.Last(), ESoundChannel.Right);
        }

        [TestMethod]
        public void XmlParserUtils_ParseSampleRate() {
            string input = $"44100 1";

            var output = XmlParserUtils.ParseSampleRate(input);

            Assert.AreEqual(output.Rate, 44100);
            Assert.AreEqual(output.Denominator, 1);

            Assert.AreEqual(44100, output.GetRealValue());
        }

        [TestMethod]
        public void XmlParserUtils_ParseSplitNumerator() {
            string input = $"44100 1";

            var output = XmlParserUtils.ParseSplitNumerator(input);

            Assert.AreEqual(output.numerator, 44100);
            Assert.AreEqual(output.denominator, 1);
            Assert.AreEqual(output.partsLen, 2);
        }

        [TestMethod]
        public void XmlParserUtils_ParsePoint() {
            XNamespace meta = "http://example.com/meta";
            string input = @"
                <MainPictureStoredArea xmlns='http://example.com/meta'>
                  <Width>1998</Width>
                  <Height>1080</Height>
                </MainPictureStoredArea>";

            var output = XmlParserUtils.ParsePoint(XDocument.Parse(input).Root, meta);

            Assert.AreEqual(1998, output.X);
            Assert.AreEqual(1080, output.Y);
        }

        [TestMethod]
        public void PackListUtils_GetFileNameFromPackagingList() {
            string file = Path.Combine(Statics.DcpPath, "pkl_5dd41f3b-f395-4576-aca5-1023e5b2614b.xml");

            string output = PackListUtils.GetFileNameFromPackagingList(file, Guid.Parse("b8162be0-6091-4b66-9ca3-9abeffd3d925"));

            Assert.IsFalse(string.IsNullOrEmpty(output));

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void PackListUtils_GetCplUuid() {
            string file = Path.Combine(Statics.DcpPath, "pkl_5dd41f3b-f395-4576-aca5-1023e5b2614b.xml");

            var output = PackListUtils.GetCplUuid(file);

            Assert.IsNotNull(output);

            Debug.WriteLine(output.ToString());
        }

        [TestMethod]
        public void EncodingUtils_Base64Decode() {
            string input = "sDLxQtiGzY8CNBU82g3/6J90GYw=";

            string output = EncodingUtils.Base64Decode(input);

            Assert.AreEqual(output, "b032f142d886cd8f0234153cda0dffe89f74198c");

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void CryptoUtils_CalculateSHA1() {
            string inputFile = Path.Combine(Statics.DcpPath, "j2c_72d1e32a-d33f-467f-9c80-2cd11c037312.mxf");

            string output = CryptoUtils.CalculateSHA1(inputFile);

            Assert.AreEqual(output.ToLower(), "c6d175a75142f47b22f4163a69f015d12646af2a");

            Debug.WriteLine(output);
        }
    }
}
