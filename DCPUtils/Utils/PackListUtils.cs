using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DCPUtils.Utils {
    public class PackListUtils {
        /// <summary>
        /// Gets the filename from the packaging list for the specified <see cref="Guid"/>
        /// </summary>
        /// <param name="packListFile">The PKL's file path</param>
        /// <param name="uuid">The UUID of the file to obtain</param>
        /// <returns></returns>
        public static string GetFileNameFromPackagingList(string packListFile, Guid uuid) {
            if(string.IsNullOrEmpty(packListFile)) {
                return null;
            }

            XNamespace ns = "http://www.smpte-ra.org/schemas/429-8/2007/PKL";
            XDocument doc = XDocument.Load(packListFile);

            var asset = doc.Descendants(ns + "Asset").FirstOrDefault(a => (string)a.Element(ns + "Id") == $"urn:uuid:{uuid.ToString().ToLower()}");

            if (asset != null) {
                return (string)asset.Element(ns + "OriginalFileName");
            }
            else {
                return null; // uuid not found
            }
        }

        /// <summary>
        /// Gets the SHA1 hash from the packaging list for the specified <see cref="Guid"/>
        /// </summary>
        /// <param name="packListFile">The PKL's file path</param>
        /// <param name="uuid">The UUID of the file to obtain</param>
        /// <returns></returns>
        public static string GetHashFromPackagingList(string packListFile, Guid uuid) {
            if (string.IsNullOrEmpty(packListFile)) {
                return null;
            }

            XNamespace ns = "http://www.smpte-ra.org/schemas/429-8/2007/PKL";
            XDocument doc = XDocument.Load(packListFile);

            var asset = doc.Descendants(ns + "Asset").FirstOrDefault(a => (string)a.Element(ns + "Id") == $"urn:uuid:{uuid.ToString().ToLower()}");

            if (asset != null) {
                return EncodingUtils.Base64Decode((string)asset.Element(ns + "Hash"));
            }
            else {
                return null; // uuid not found
            }
        }

        /// <summary>
        /// Gets the <see cref="Guid"/> of the <see cref="Models.CompositionPlaylist"/> from the PKL
        /// </summary>
        /// <param name="packListPath">The PKL's file path</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Guid GetCplUuid(string packListPath) {
            XNamespace ns = "http://www.smpte-ra.org/schemas/429-8/2007/PKL";

            var doc = XDocument.Load(packListPath);

            var cplAssetId = doc.Descendants(ns + "Asset").Where(asset => asset.Element(ns + "Type")?.Value == "text/xml" && asset.Element(ns + "OriginalFileName")?.Value.StartsWith("cpl_", StringComparison.OrdinalIgnoreCase) == true).Select(asset => asset.Element(ns + "Id")?.Value).FirstOrDefault();
            if (cplAssetId == null) {
                throw new InvalidOperationException("A CPL could not found in the packing list.");
            }

            return UuidUtils.ToGuid(cplAssetId);
        }
    }
}
