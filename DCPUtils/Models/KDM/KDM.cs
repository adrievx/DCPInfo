using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DCPUtils.Models.Structs;
using DCPUtils.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Math;
using static System.Net.Mime.MediaTypeNames;

namespace DCPUtils.Models.KDM {
    public class KDM {
        /// <summary>
        /// The public authentication block of the KDM, also containing additional metadata (annotation text, issue date, etc.)
        /// </summary>
        public AuthenticatedPublic AuthenticatedPublic { get; set; }

        /// <summary>
        /// The private authentication block of the KDM, a list of <see cref="EncryptedKey"/>s, which are used to decrypt the DCP
        /// </summary>
        public List<EncryptedKey> AuthenticatedPrivate { get; set; }

        /// <summary>
        /// The digital signature of the KDM
        /// </summary>
        public KDMSignature Signature { get; set; }

        /// <summary>
        /// Verify the <see cref="KDM"/> against a specified <see cref="DCP"/> and TMS signature
        /// </summary>
        /// <param name="dcp">The DCP to compare against</param>
        /// <param name="tmsCertificatePath">The file path for the TMS's public key</param>
        /// <param name="tmsPrivateKeyPath">The file path for the TMS's private key</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Verify(DCP dcp, string tmsCertificatePath, string tmsPrivateKeyPath) {
            #region Load TMS certificate
            var cert = new X509Certificate2(tmsCertificatePath);

            string prvKeyPem = File.ReadAllText(tmsPrivateKeyPath);
            object keyObject;

            using (var reader = new StringReader(prvKeyPem)) {
                var pemReader = new PemReader(reader);
                keyObject = pemReader.ReadObject();
            }

            RSA privateKey;
            AsymmetricKeyParameter keyParam;

            if (keyObject is AsymmetricKeyParameter k && k.IsPrivate) {
                keyParam = k;
            }
            else if (keyObject is PrivateKeyInfo pkInfo) {
                keyParam = PrivateKeyFactory.CreateKey(pkInfo);
            }
            else {
                throw new InvalidOperationException("The specified PEM does not contain a valid private key");
            }

            privateKey = DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters)keyParam);
            #endregion

            #region Recipient check
            bool isRecipient = false;
            foreach(var ext in AuthenticatedPublic.RequiredExtensions) {
                var recipient = ext.Recipient;
                string certSerialHex = cert.SerialNumber;

                if (certSerialHex.Length % 2 == 1) {
                    certSerialHex = "0" + certSerialHex;
                }

                byte[] certSerialBytes = Enumerable.Range(0, certSerialHex.Length / 2).Select(i => Convert.ToByte(certSerialHex.Substring(i * 2, 2), 16)).Reverse().ToArray();
                var certSerialDecimal = new System.Numerics.BigInteger(certSerialBytes);

                System.Numerics.BigInteger recipientSerialDecimal;
                try {
                    recipientSerialDecimal = System.Numerics.BigInteger.Parse(recipient.SerialNumber);
                }
                catch {
                    continue; // skip invalid
                }

                string certCommonName = cert.GetNameInfo(X509NameType.SimpleName, false);

                if (certSerialDecimal == recipientSerialDecimal & string.Equals(recipient.CommonName, certCommonName, StringComparison.OrdinalIgnoreCase)) {
                    isRecipient = true;
                    break;
                }
            }

            if (!isRecipient) {
                return false;
            }
            #endregion

            #region Date check
            if (!this.AuthenticatedPublic.RequiredExtensions.All(item => DateTime.UtcNow >= item.ContentKeysNotValidBefore && DateTime.UtcNow <= item.ContentKeysNotValidAfter)) {
                return false;
            }
            #endregion

            #region CPL check
            if (!AuthenticatedPublic.RequiredExtensions.Any(ext => ext.CompositionPlaylistId == dcp.CompositionPlaylist.UUID)) {
                return false;
            }
            #endregion

            #region Decryption check
            // attempt decryption of at least a single key
            bool canDecrypt = false;
            foreach (var encryptedKey in AuthenticatedPrivate) {
                try {
                    byte[] cipher = EncodingUtils.HexToBytes(encryptedKey.CipherValue);
                    byte[] sessionKey = privateKey.Decrypt(cipher, RSAEncryptionPadding.OaepSHA1);
                    canDecrypt = true;
                    break;
                }
                catch (CryptographicException ex) {
                    continue;
                }
            }

            if (!canDecrypt) {
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// Read a Key Delivery Message into a new <see cref="KDM"/> instance
        /// </summary>
        /// <param name="kdmPath">The file path for the KDM's XML file</param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static KDM Read(string kdmPath) {
            if (File.Exists(kdmPath)) {
                var kdm = new KDM();

                kdm.AuthenticatedPublic = new AuthenticatedPublic();
                kdm.AuthenticatedPrivate = new List<EncryptedKey>();
                kdm.Signature = new KDMSignature();

                var kdmXml = XDocument.Load(kdmPath);

                if (kdmXml != null) {
                    XNamespace etmNs = "http://www.smpte-ra.org/schemas/430-3/2006/ETM";
                    XNamespace ds = "http://www.w3.org/2000/09/xmldsig#";
                    XNamespace kdmNs = "http://www.smpte-ra.org/schemas/430-1/2006/KDM";
                    XNamespace encNs = "http://www.w3.org/2001/04/xmlenc#";

                    #region AuthenticatedPublic block
                    var authenticatedPublic = kdmXml.Root.Element(etmNs + "AuthenticatedPublic");

                    if (authenticatedPublic != null) {
                        kdm.AuthenticatedPublic.MessageId = UuidUtils.ToGuid(authenticatedPublic.Element(etmNs + "MessageId")?.Value);
                        kdm.AuthenticatedPublic.AnnotationText = authenticatedPublic.Element(etmNs + "AnnotationText")?.Value;
                        kdm.AuthenticatedPublic.IssueDate = DateTime.Parse(authenticatedPublic.Element(etmNs + "IssueDate")?.Value);

                        // <Signer> block
                        var signer = authenticatedPublic.Element(etmNs + "Signer");
                        kdm.AuthenticatedPublic.Signer = new Crypto.X509Certificate(signer?.Element(ds + "X509IssuerName")?.Value, signer?.Element(ds + "X509SerialNumber")?.Value);

                        // <RequiredExtensions> block
                        kdm.AuthenticatedPublic.RequiredExtensions = new List<KDMRequiredExtension>();

                        var requiredExtensions = kdmXml.Descendants(etmNs + "RequiredExtensions").Elements(kdmNs + "KDMRequiredExtensions");
                        foreach (var ext in requiredExtensions) {
                            var recipientElem = ext.Element(kdmNs + "Recipient");
                            Crypto.X509Certificate recipientCert = null;

                            if (recipientElem != null) {
                                var issuerName = recipientElem.Element(kdmNs + "X509IssuerSerial")?.Element(ds + "X509IssuerName")?.Value;
                                var serialNumber = recipientElem.Element(kdmNs + "X509IssuerSerial")?.Element(ds + "X509SerialNumber")?.Value;

                                if (!string.IsNullOrEmpty(issuerName) && !string.IsNullOrEmpty(serialNumber)) {
                                    var cert = new Crypto.X509Certificate(issuerName, serialNumber);
                                    recipientCert = cert;
                                }
                            }

                            var extension = new KDMRequiredExtension {
                                Recipient = recipientCert,
                                CompositionPlaylistId = UuidUtils.ToGuid(ext.Element(kdmNs + "CompositionPlaylistId")?.Value),
                                ContentTitleText = ext.Element(kdmNs + "ContentTitleText")?.Value,
                                ContentKeysNotValidBefore = DateTime.Parse(ext.Element(kdmNs + "ContentKeysNotValidBefore")?.Value),
                                ContentKeysNotValidAfter = DateTime.Parse(ext.Element(kdmNs + "ContentKeysNotValidAfter")?.Value),
                                AuthorizedDeviceInfo = new AuthorizedDevice {
                                    DeviceListIdentifier = UuidUtils.ToGuid(ext.Element(kdmNs + "AuthorizedDeviceInfo")?.Element(kdmNs + "DeviceListIdentifier")?.Value),
                                    DeviceListDescription = ext.Element(kdmNs + "AuthorizedDeviceInfo")?.Element(kdmNs + "DeviceListDescription")?.Value,
                                    DeviceList = ext.Element(kdmNs + "AuthorizedDeviceInfo")?
                                                    .Element(kdmNs + "DeviceList")?
                                                    .Elements(kdmNs + "CertificateThumbprint")
                                                    .Select(e => EncodingUtils.Base64Decode(e.Value))
                                                    .ToList()
                                },
                                KeyIdList = ext.Element(kdmNs + "KeyIdList")?
                                                .Elements(kdmNs + "TypedKeyId")
                                                .Select(k => new FKeyId {
                                                    KeyType = k.Element(kdmNs + "KeyType")?.Value,
                                                    KeyId = UuidUtils.ToGuid(k.Element(kdmNs + "KeyId")?.Value ?? Guid.Empty.ToString())
                                                }).ToList(),
                                ForensicMarkFlagList = ext.Element(kdmNs + "ForensicMarkFlagList")?
                                                .Elements(kdmNs + "ForensicMarkFlag")
                                                .Select(e => e.Value).ToList()
                            };

                            kdm.AuthenticatedPublic.RequiredExtensions.Add(extension);
                        }
                    }
                    else {
                        throw new FileLoadException("Unable to find AuthenticatedPublic block in KDM");
                    }
                    #endregion

                    #region AuthenticatedPrivate block
                    kdm.AuthenticatedPrivate = kdmXml.Descendants(etmNs + "AuthenticatedPrivate").Elements(encNs + "EncryptedKey").Select(e => new EncryptedKey {
                        EncryptionMethod = e.Element(encNs + "EncryptionMethod")?.Attribute("Algorithm")?.Value,
                        DigestMethod = e.Element(encNs + "EncryptionMethod")?.Element(ds + "DigestMethod")?.Attribute("Algorithm")?.Value,
                        CipherValue = EncodingUtils.Base64Decode(e.Element(encNs + "CipherData")?.Element(encNs + "CipherValue")?.Value)
                    }).ToList();
                    #endregion

                    #region Signature block
                    var sigElement = kdmXml.Descendants(ds + "Signature").FirstOrDefault();
                    kdm.Signature = new KDMSignature {
                        SignatureValue = EncodingUtils.Base64Decode(sigElement?.Element(ds + "SignatureValue")?.Value),
                        SignedInfo = new SignedInfo {
                            CanonicalizationMethod = sigElement?.Element(ds + "SignedInfo")?.Element(ds + "CanonicalizationMethod")?.Attribute("Algorithm")?.Value,
                            SignatureMethod = sigElement?.Element(ds + "SignedInfo")?.Element(ds + "SignatureMethod")?.Attribute("Algorithm")?.Value,
                            AuthenticatedPublic = parseAuthenticationReference(sigElement, "#ID_AuthenticatedPublic"),
                            AuthenticatedPrivate = parseAuthenticationReference(sigElement, "#ID_AuthenticatedPrivate")
                        }
                    };
                    #endregion

                    return kdm;
                }
                else {
                    throw new SerializationException("Unable to deserialize KDM");
                }
            }
            else {
                throw new FileNotFoundException("Unable to find KDM from path specified");
            }
        }

        private static FAuthenticationReference parseAuthenticationReference(XElement signatureElement, string uri) {
            XNamespace ds = "http://www.w3.org/2000/09/xmldsig#";

            var reference = signatureElement?.Element(ds + "SignedInfo")?.Elements(ds + "Reference").FirstOrDefault(r => r.Attribute("URI")?.Value == uri);

            return new FAuthenticationReference {
                DigestMethod = reference?.Element(ds + "DigestMethod")?.Attribute("Algorithm")?.Value,
                DigestValue = EncodingUtils.Base64Decode(reference?.Element(ds + "DigestValue")?.Value)
            };
        }
    }
}
