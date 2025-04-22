using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DCPUtils.Models.Structs;
using DCPUtils.Utils;

namespace DCPUtils.Models.KDM {
    public class KDM {
        public AuthenticatedPublic AuthenticatedPublic { get; set; }
        public List<EncryptedKey> AuthenticatedPrivate { get; set; }
        public KDMSignature Signature { get; set; }

        public bool Verify(string dcpPath, Crypto.X509Certificate tmsCertificate) {
            // TODO: implement
            throw new NotImplementedException();
        }

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
