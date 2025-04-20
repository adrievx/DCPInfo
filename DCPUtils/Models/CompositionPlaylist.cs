using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DCPUtils.Enum;
using DCPUtils.Models.Composition;
using DCPUtils.Models.Structs;
using DCPUtils.Utils;

namespace DCPUtils.Models {
    public class CompositionPlaylist : DCPNodeExtended {
        public string ContentTitleText { get; set; }
        public EContentKind ContentKind { get; set; }
        public FContentVersion ContentVersion { get; set; }
        public List<FRating> RatingList { get; set; } // may or may not exist, is optional
        public List<CompositionReel> ReelList { get; set; }

        public CompositionPlaylist() {
            RatingList = new List<FRating>();
            ReelList = new List<CompositionReel>();
        }

        /// <summary>
        /// Reads a <see cref="CompositionPlaylist"/> from the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public FContentVersion ReadCplContentVersion(XDocument doc, XNamespace ns) {
            XElement versionElement = doc.Root?.Element(ns + "ContentVersion");

            if (versionElement == null) {
                throw new Exception("ContentVersion couldn't be found in the CPL.");
            }

            var idStr = versionElement.Element(ns + "Id")?.Value;
            var labelStr = versionElement.Element(ns + "LabelText")?.Value;

            return new FContentVersion {
                UUID = UuidUtils.ToGuid(idStr),
                Version = int.TryParse(labelStr, out var version) ? version : 1
            };
        }

        public List<FRating> ReadRatingList(XDocument doc, XNamespace ns) {
            var ratings = doc.Descendants(ns + "Rating").Select(ratingElem => new FRating {
                Agency = ratingElem.Element(ns + "Agency")?.Value ?? string.Empty,
                Label = ratingElem.Element(ns + "Label")?.Value ?? string.Empty,
                Region = ratingElem.Element(ns + "Region")?.Value ?? string.Empty
            }).ToList();

            return ratings;
        }
    }
}
