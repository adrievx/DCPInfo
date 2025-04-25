using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DCPUtils.Enum;
using DCPUtils.Models.Composition;
using DCPUtils.Models.KDM;
using DCPUtils.Models.Structs;
using DCPUtils.Utils;

namespace DCPUtils.Models {
    public class DCP {
        /// <summary>
        /// The name of the DCP
        /// </summary>
        public string Name { get; internal set; } // fetched from folder name or ContentTitleText of CPL

        /// <summary>
        /// The DCP's metadata
        /// </summary>
        public DCPMetadata Metadata { get; internal set; }

        /// <summary>
        /// The DCP's Composition Playlist (CPL)
        /// </summary>
        public CompositionPlaylist CompositionPlaylist { get; internal set; }

        /// <summary>
        /// The list of assets present in the DCP, fetched from the ASSETMAP.xml file
        /// </summary>
        public List<DCPAsset> Assets { get; internal set; }

        /// <summary>
        /// The <see cref="KDM.KDM"/> (Key Delivery Message), if present
        /// </summary>
        public KDM.KDM KDMData { get; internal set; }

        /// <summary>
        /// The full path of the ASSETMAP.xml file
        /// </summary>
        public string AssetMapPath = "";

        /// <summary>
        /// The full path of the <see cref="Models.CompositionPlaylist"/> (CPL)
        /// </summary>
        public string CompositionPlaylistPath = "";

        /// <summary>
        /// The full path of the Packaging List (PKL)
        /// </summary>
        public string PackListPath = "";

        /// <summary>
        /// The full path of the primary video MXF
        /// </summary>
        public string MainPicturePath = "";

        /// <summary>
        /// The full path of the primary audio MXF
        /// </summary>
        public string MainSoundPath = "";

        /// <summary>
        /// The root folder of the DCP
        /// </summary>
        public string DcpRoot = "";

        /// <summary>
        /// Whether the DCP is encrypted (requires a <see cref="KDM.KDM"/>) or not
        /// </summary>
        public bool IsEncrypted {
            get {
                bool isEncrypted = false;

                foreach (var reel in this.CompositionPlaylist.ReelList) {
                    if(reel.MainPicture.KeyId != null) {
                        isEncrypted = true;
                    }

                    if (reel.MainSound.KeyId != null) {
                        isEncrypted = true;
                    }
                }

                return isEncrypted;
            }
        }

        /// <summary>
        /// Whether the DCP has closed captions or not
        /// </summary>
        public bool HasClosedCaptions => CompositionPlaylist.ReelList.Any(reel => reel.ClosedCaption != null);

        /// <summary>
        /// Verifies the <see cref="DCP"/> against the packaging list (if present) to ensure that the package hasn't been tampered with.
        /// </summary>
        /// <returns></returns>
        public bool Verify() {
            bool good = true;

            foreach (var asset in Assets) {
                foreach (var reel in CompositionPlaylist.ReelList) {
                    // we don't use MainPicturePath etc incase there's multiple reels

                    var pictureUuid = reel.MainPicture.UUID;
                    var soundUuid = reel.MainPicture.UUID;

                    string pictureFilename = PackListUtils.GetFileNameFromPackagingList(PackListPath, pictureUuid);
                    string soundFilename = PackListUtils.GetFileNameFromPackagingList(PackListPath, soundUuid);

                    string picturePath = Path.Combine(DcpRoot, pictureFilename);
                    string soundPath = Path.Combine(DcpRoot, soundFilename);

                    string pictureHashExpected = PackListUtils.GetHashFromPackagingList(PackListPath, pictureUuid);
                    string soundHashExpected = PackListUtils.GetHashFromPackagingList(PackListPath, soundUuid);
                    string cplHashExpected = PackListUtils.GetHashFromPackagingList(PackListPath, PackListUtils.GetCplUuid(PackListPath));

                    string pictureHashResult = CryptoUtils.CalculateSHA1(picturePath);
                    string soundHashResult = CryptoUtils.CalculateSHA1(soundPath);
                    string cplHashResult = CryptoUtils.CalculateSHA1(CompositionPlaylistPath);

                    string cplFilename = new FileInfo(CompositionPlaylistPath).Name;

                    // check MainPicture
                    if (pictureHashExpected == pictureHashResult) {
                        Debug.WriteLine($"Hash of MainPicture ({pictureFilename}) was good.");
                    }
                    else {
                        good = false;
                        Debug.WriteLine($"Hash of MainPicture ({pictureFilename}) did not line up with expected hash.");
                    }

                    // check MainSound
                    if (soundHashExpected == soundHashResult) {
                        Debug.WriteLine($"Hash of MainSound ({soundFilename}) was good.");
                    }
                    else {
                        good = false;
                        Debug.WriteLine($"Hash of MainSound ({soundFilename}) did not line up with expected hash.");
                    }

                    // check compo playlist
                    if (cplHashExpected == cplHashResult) {
                        Debug.WriteLine($"Hash of Composition Playlist ({cplFilename}) was good.");
                    }
                    else {
                        good = false;
                        Debug.WriteLine($"Hash of Composition Playlist ({cplFilename}) did not line up with expected hash.");
                    }

                    if(HasClosedCaptions) {
                        var captionUuid = reel.ClosedCaption.UUID;
                        string captionFilename = PackListUtils.GetFileNameFromPackagingList(PackListPath, captionUuid);
                        string captionPath = Path.Combine(DcpRoot, captionFilename);

                        string captionHashExpected = PackListUtils.GetHashFromPackagingList(PackListPath, captionUuid);
                        string captionHashResult = CryptoUtils.CalculateSHA1(captionPath);

                        if (captionHashExpected == captionHashResult) {
                            Debug.WriteLine($"Hash of ClosedCaption ({captionFilename}) was good.");
                        }
                        else {
                            good = false;
                            Debug.WriteLine($"Hash of ClosedCaption ({captionFilename}) did not line up with expected hash.");
                        }
                    }
                }
            }

            return good;
        }

        /// <summary>
        /// Checks whether a specified KDM file is for the current DCP or not
        /// </summary>
        /// <param name="kdmFile">The KDM XML file</param>
        /// <returns></returns>
        /// <exception cref="SerializationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public bool FindKDM(string kdmFile) {
            if (File.Exists(kdmFile)) {
                var kdm = KDM.KDM.Read(kdmFile);

                if (kdm != null) {
                    foreach (var item in kdm.AuthenticatedPublic.RequiredExtensions) {
                        if(item.CompositionPlaylistId == this.CompositionPlaylist.UUID) {
                            this.KDMData = kdm;
                            return true;
                        }
                    }

                    return false;
                }
                else {
                    throw new SerializationException("Unable to deserialize KDM");
                }
            }
            else {
                throw new FileNotFoundException("Specified KDM could not be found");
            }
        }

        /// <summary>
        /// Returns a <see cref="DCPAsset"/> from it's UUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public DCPAsset GetAssetByUUID(Guid guid) {
            foreach (var item in this.Assets) {
                if(item.UUID == guid) {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Read a <see cref="DCP"/> into a new model
        /// </summary>
        /// <param name="root"></param>
        /// <param name="loadingFromDriveRoot"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static DCP Read(string root, bool loadingFromDriveRoot = false) {
            var dcp = new DCP();

            dcp.Metadata = new DCPMetadata();

            if (Directory.Exists(root)) {
                dcp.DcpRoot = root;

                XNamespace nsAssetmap = "http://www.smpte-ra.org/schemas/429-9/2007/AM"; // apparently not all DCPs use this schema, RealD bumper uses 'xmlns="http://www.digicine.com/PROTO-ASDCP-AM-20040311#"'
                XNamespace nsCompPlaylist = "http://www.smpte-ra.org/schemas/429-7/2006/CPL";

                #region ASSETMAP.xml
                #region Read ASSETMAP.xml into memory
                if (File.Exists(Path.Combine(root, "ASSETMAP.xml"))) {
                    dcp.AssetMapPath = Path.Combine(root, "ASSETMAP.xml");
                }
                else if (File.Exists(Path.Combine(root, "ASSETMAP"))) {
                    dcp.AssetMapPath = Path.Combine(root, "ASSETMAP");
                }
                else {
                    throw new FileNotFoundException("Unable to find assetmap");
                }

                // read assetmap
                var assetMapXml = XDocument.Load(dcp.AssetMapPath);
                #endregion

                // read assetmap metadata
                DCPNodeExtended tempMetadata = dcp.Metadata;
                readExtendedNodeBase(ref tempMetadata, assetMapXml.Root, nsAssetmap);
                dcp.Metadata = (DCPMetadata)tempMetadata;

                dcp.Metadata.VolumeCount = int.Parse(assetMapXml.Root?.Element(nsAssetmap + "VolumeCount")?.Value);

                // read asset listing
                dcp.Assets = assetMapXml.Descendants(nsAssetmap + "Asset").Select(assetElem => new DCPAsset {
                    UUID = UuidUtils.ToGuid(assetElem.Element(nsAssetmap + "Id")?.Value),
                    PackingList = bool.TryParse(assetElem.Element(nsAssetmap + "PackingList")?.Value, out var pl) && pl,
                    Chunks = assetElem.Descendants(nsAssetmap + "Chunk").Select(chunkElem => new DCPAssetChunk {
                        Path = chunkElem.Element(nsAssetmap + "Path")?.Value,
                        VolumeIndex = int.Parse(chunkElem.Element(nsAssetmap + "VolumeIndex")?.Value ?? "1"),
                        Offset = long.Parse(chunkElem.Element(nsAssetmap + "Offset")?.Value ?? "0"),
                        Length = long.Parse(chunkElem.Element(nsAssetmap + "Length")?.Value ?? "0")
                    }).ToList()
                }).ToList();
                #endregion

                #region Composition Playlist
                #region Read CPL into memory
                dcp.CompositionPlaylist = new CompositionPlaylist();

                // find composition playlist
                foreach (var asset in dcp.Assets) {
                    foreach (var chunk in asset.Chunks) {
                        if(chunk.Path.ToLower().Contains("cpl_")) {
                            dcp.CompositionPlaylistPath = Path.Combine(root, chunk.Path);
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(dcp.CompositionPlaylistPath)) {
                    throw new FileNotFoundException("Unable to find composition playlist.");
                }

                var compPlaylistXml = XDocument.Load(dcp.CompositionPlaylistPath);
                #endregion

                // read composition playlist metadata
                DCPNodeExtended tempComp = dcp.CompositionPlaylist;
                readExtendedNodeBase(ref tempComp, compPlaylistXml.Root, nsCompPlaylist);
                dcp.CompositionPlaylist = (CompositionPlaylist)tempComp;

                dcp.CompositionPlaylist.ContentTitleText = compPlaylistXml.Root?.Element(nsCompPlaylist + "ContentTitleText")?.Value;

                #region Parse <ContentKind>
                var contentKindMap = new Dictionary<string, EContentKind>(StringComparer.OrdinalIgnoreCase) {
                    { "feature", EContentKind.Feature },
                    { "trailer", EContentKind.Trailer },
                    { "teaser", EContentKind.Teaser },
                    { "rating", EContentKind.Rating },
                    { "policy", EContentKind.Policy },
                    { "advertisement", EContentKind.Advert },
                    { "short", EContentKind.Short },
                    { "psa", EContentKind.PSA },
                    { "test", EContentKind.Test },
                    { "cinema", EContentKind.Policy } // alt version of POL, used for cinema bumpers, etc.
                };

                string contentKindValue = compPlaylistXml.Root?.Element(nsCompPlaylist + "ContentKind")?.Value;

                if (!string.IsNullOrWhiteSpace(contentKindValue) && contentKindMap.TryGetValue(contentKindValue, out var kind)) {
                    dcp.CompositionPlaylist.ContentKind = kind;
                }
                else {
                    dcp.CompositionPlaylist.ContentKind = EContentKind.Other;
                }
                #endregion

                dcp.CompositionPlaylist.ContentVersion = dcp.CompositionPlaylist.ReadCplContentVersion(compPlaylistXml, nsCompPlaylist);
                dcp.CompositionPlaylist.RatingList = dcp.CompositionPlaylist.ReadRatingList(compPlaylistXml, nsCompPlaylist);

                dcp.CompositionPlaylist.ReelList = CompositionReel.ReadReels(compPlaylistXml, nsCompPlaylist);
                #endregion

                #region Packaging List
                // find packaging list
                foreach (var asset in dcp.Assets) {
                    foreach (var chunk in asset.Chunks) {
                        if (chunk.Path.ToLower().Contains("pkl_")) {
                            dcp.PackListPath = Path.Combine(root, chunk.Path);
                            break;
                        }
                    }
                }

                if(string.IsNullOrEmpty(dcp.PackListPath)) {
                    throw new FileNotFoundException("Unable to find packaging list.");
                }

                #region Fetch main picture
                var mainPictureUuid = dcp.CompositionPlaylist.ReelList.First().MainPicture.UUID;
                string mainPictureFilename = PackListUtils.GetFileNameFromPackagingList(dcp.PackListPath, mainPictureUuid);
                dcp.MainPicturePath = Path.Combine(root, mainPictureFilename);
                #endregion

                #region Fetch main sound
                var mainSoundUuid = dcp.CompositionPlaylist.ReelList.First().MainSound.UUID;
                string mainSoundFilename = PackListUtils.GetFileNameFromPackagingList(dcp.PackListPath, mainSoundUuid);
                dcp.MainSoundPath = Path.Combine(root, mainSoundFilename);
                #endregion
                #endregion

                #region Fetch DCP name
                if (!string.IsNullOrEmpty(dcp.CompositionPlaylist.ContentTitleText)) {
                    dcp.Name = dcp.CompositionPlaylist.ContentTitleText;
                }
                else {
                    if(!loadingFromDriveRoot) {
                        dcp.Name = new DirectoryInfo(root).Name;
                    }
                    else {
                        throw new Exception("Unable to get DCP name.");
                    }
                }
                #endregion

                return dcp;
            }
            else {
                throw new DirectoryNotFoundException("Unable to find the specified path");
            }
        }

        private static void readExtendedNodeBase(ref DCPNodeExtended dcpNode, XElement root, XNamespace ns) {
            dcpNode.UUID = UuidUtils.ToGuid(root.Element(ns + "Id")?.Value);
            dcpNode.AnnotationText = root.Element(ns + "AnnotationText")?.Value;
            dcpNode.Creator = root.Element(ns + "Creator")?.Value;
            dcpNode.IssueDate = DateTime.Parse(root.Element(ns + "IssueDate")?.Value);
            dcpNode.Issuer = root.Element(ns + "Issuer")?.Value;
        }
    }
}
