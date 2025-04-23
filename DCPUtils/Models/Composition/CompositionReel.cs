using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DCPUtils.Models.Misc;
using DCPUtils.Models.Structs;
using DCPUtils.Utils;

namespace DCPUtils.Models.Composition {
    public class CompositionReel : DCPNode {
        /// <summary>
        /// The list of <see cref="Marker"/>s set for the video
        /// </summary>
        public MainMarker MainMarkers { get; set; }

        /// <summary>
        /// Data about the video MXF for the current reel
        /// </summary>
        public MainPicture MainPicture { get; set; }

        /// <summary>
        /// Data about the audio MXF for the current reel
        /// </summary>
        public MainSound MainSound { get; set; }

        /// <summary>
        /// Metadata for the current reel
        /// </summary>
        public CompositionMetadata Metadata { get; set; } // this may or may not exist

        /// <summary>
        /// Returns a collection of <see cref="CompositionReel"/> objects in document order 
        /// from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static List<CompositionReel> ReadReels(XDocument doc, XNamespace ns) {
            List<CompositionReel> reels = doc.Descendants(ns + "Reel").Select(reelElem => {
                var assetList = reelElem.Element(ns + "AssetList");

                var reel = new CompositionReel();

                // MainMarkers
                var mainMarkersElem = assetList?.Element(ns + "MainMarkers");
                if (mainMarkersElem != null) {
                    var markerList = mainMarkersElem.Element(ns + "MarkerList")?.Elements(ns + "Marker").Select(m => new Marker {
                        Label = m.Element(ns + "Label")?.Value ?? string.Empty,
                        Offset = long.TryParse(m.Element(ns + "Offset")?.Value, out var offset) ? offset : 0
                    }).ToList() ?? new List<Marker>();

                    var idElement = mainMarkersElem.Element(ns + "Id");
                    var val = idElement?.Value;
                    var uuid = UuidUtils.ToGuid(val);

                    reel.MainMarkers = new MainMarker {
                        UUID = uuid,
                        EditRate = parseFramerate(mainMarkersElem.Element(ns + "EditRate")?.Value),
                        IntrinsicDuration = int.Parse(mainMarkersElem.Element(ns + "IntrinsicDuration")?.Value ?? "0"),
                        MarkerList = markerList
                    };
                }

                // MainPicture
                var mainPicElem = assetList?.Element(ns + "MainPicture");
                if (mainPicElem != null) {
                    Nullable<FKeyId> keyId = null;

                    var keyIdElem = mainPicElem.Element(ns + "KeyId");
                    if (keyIdElem != null) {
                        keyId = new FKeyId() { KeyId = UuidUtils.ToGuid(keyIdElem.Value) };
                    }

                    reel.MainPicture = new MainPicture {
                        UUID = UuidUtils.ToGuid(mainPicElem.Element(ns + "Id")?.Value),
                        EditRate = parseFramerate(mainPicElem.Element(ns + "EditRate")?.Value),
                        KeyId = keyId,
                        IntrinsicDuration = int.Parse(mainPicElem.Element(ns + "IntrinsicDuration")?.Value ?? "0"),
                        FrameRate = parseFramerate(mainPicElem.Element(ns + "FrameRate")?.Value),
                        ScreenAspectRatio = parsePoint(mainPicElem.Element(ns + "ScreenAspectRatio")?.Value)
                    };
                }

                // MainSound
                var mainSoundElem = assetList?.Element(ns + "MainSound");
                if (mainSoundElem != null) {
                    Nullable<FKeyId> keyId = null;

                    var keyIdElem = mainSoundElem.Element(ns + "KeyId");
                    if (keyIdElem != null) {
                        keyId = new FKeyId() { KeyId = UuidUtils.ToGuid(keyIdElem.Value) };
                    }

                    reel.MainSound = new MainSound {
                        UUID = UuidUtils.ToGuid(mainSoundElem.Element(ns + "Id")?.Value),
                        EditRate = parseFramerate(mainSoundElem.Element(ns + "EditRate")?.Value),
                        KeyId = keyId,
                        IntrinsicDuration = int.Parse(mainSoundElem.Element(ns + "IntrinsicDuration")?.Value ?? "0"),
                        EntryPoint = long.TryParse(mainSoundElem.Element(ns + "EntryPoint")?.Value, out var ep) ? ep : 0,
                        Duration = long.TryParse(mainSoundElem.Element(ns + "Duration")?.Value, out var dur) ? dur : 0,
                        Hash = mainSoundElem.Element(ns + "Hash")?.Value
                    };
                }

                // CompositionMetadata (may or may not exist)
                XNamespace cplMeta = "http://www.smpte-ra.org/schemas/429-16/2014/CPL-Metadata";

                var metadataElem = assetList?.Element(cplMeta + "CompositionMetadataAsset");
                if (metadataElem != null) {
                    reel.Metadata = new CompositionMetadata {
                        UUID = UuidUtils.ToGuid(metadataElem.Element(ns + "Id")?.Value),
                        EditRate = parseFramerate(metadataElem.Element(cplMeta + "EditRate")?.Value),
                        IntrinsicDuration = int.Parse(metadataElem.Element(cplMeta + "IntrinsicDuration")?.Value ?? "0"),
                        FullContentTitleText = metadataElem.Element(cplMeta + "FullContentTitleText")?.Value,
                        VersionNumber = int.Parse(metadataElem.Element(cplMeta + "VersionNumber")?.Value ?? "0"),
                        Facility = metadataElem.Element(cplMeta + "Facility")?.Value,
                        MainSoundConfiguration = XmlParserUtils.ParseSoundConfiguration(metadataElem.Element(cplMeta + "MainSoundConfiguration")?.Value),
                        MainSoundSampleRate = XmlParserUtils.ParseSampleRate(metadataElem.Element(cplMeta + "MainSoundSampleRate")?.Value),
                        MainPictureStoredArea = XmlParserUtils.ParsePoint(metadataElem.Element(cplMeta + "MainPictureStoredArea"), cplMeta),
                        MainPictureActiveArea = XmlParserUtils.ParsePoint(metadataElem.Element(cplMeta + "MainPictureActiveArea"), cplMeta),
                    };
                }

                return reel;
            }).ToList();

            return reels;
        }

        private static FrameRate parseFramerate(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                // shouldn't be, but we check regardless
                return default;
            }

            var parts = value.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[0], out int numerator) && int.TryParse(parts[1], out int denominator)) {
                return new FrameRate { Rate = numerator, Denominator = denominator };
            }

            return default;
        }

        private static Point parsePoint(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                // shouldn't be, but we check regardless
                return Point.Empty;
            }

            var parts = value.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y)) {
                return new Point(x, y);
            }

            return Point.Empty;
        }
    }
}
