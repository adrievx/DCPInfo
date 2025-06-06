﻿using System;
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
        /// <summary>
        /// The title of the content, typically the DCP name
        /// </summary>
        public string ContentTitleText { get; set; }

        /// <summary>
        /// The type of content present (feature, trailer, etc.)
        /// </summary>
        public EContentKind ContentKind { get; set; }

        /// <summary>
        /// The version of the content being played (if not OV)
        /// </summary>
        public FContentVersion ContentVersion { get; set; }

        /// <summary>
        /// The ratings data for the movie (optional)
        /// </summary>
        public List<FRating> RatingList { get; set; }

        /// <summary>
        /// The list of <see cref="CompositionReel"/> reels present in the CPL
        /// </summary>
        public List<CompositionReel> ReelList { get; set; }

        public CompositionPlaylist() {
            RatingList = new List<FRating>();
            ReelList = new List<CompositionReel>();
        }

        /// <summary>
        /// Reads a <see cref="CompositionPlaylist"/> from the given <see cref="XDocument"/> and outputs a <see cref="FContentVersion"/> structure
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

        /// <summary>
        /// Reads the rating list from the given <see cref="XDocument"/> and outputs a collection of <see cref="FRating"/>s
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public List<FRating> ReadRatingList(XDocument doc, XNamespace ns) {
            List<FRating> ratings = doc.Descendants(ns + "Rating").Select(ratingElem => new FRating {
                Agency = RatingUtils.FromUrl(ratingElem.Element(ns + "Agency")?.Value ?? string.Empty),
                Label = ratingElem.Element(ns + "Label")?.Value ?? string.Empty,
                Region = ratingElem.Element(ns + "Region")?.Value ?? string.Empty
            }).ToList();

            return ratings;
        }
    }
}
