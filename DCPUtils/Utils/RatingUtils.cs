using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Enum;

namespace DCPUtils.Utils {
    public class RatingUtils {
        /// <summary>
        /// Converts a URL into the representing <see cref="ERatingAgency"/> enum
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ERatingAgency FromUrl(string url) {
            var dict = new Dictionary<string, ERatingAgency>() {
                { "http://www.movielabs.com/md/ratings/US/MPAA/1", ERatingAgency.MPAA },
                { "http://www.movielabs.com/md/ratings/GB/BBFC/1", ERatingAgency.BBFC },
                { "http://www.movielabs.com/md/ratings/FR/CNC/3", ERatingAgency.CNC },
                { "http://www.movielabs.com/md/ratings/DE/FSK/1", ERatingAgency.FSK },
                { "http://www.movielabs.com/md/ratings/JP/Eirin/1", ERatingAgency.Eirin },
                { "http://www.movielabs.com/md/ratings/CA/QC/1", ERatingAgency.RDC_QC },
                { "http://www.movielabs.com/md/ratings/CA/BC/1", ERatingAgency.RDC_BC },
                { "http://www.movielabs.com/md/ratings/IN/CBFC/1", ERatingAgency.CBFC },
                { "http://www.movielabs.com/md/ratings/AU/ACB/1", ERatingAgency.ACB },
                { "http://www.movielabs.com/md/ratings/US/NR/1", ERatingAgency.NR }
            };

            try {
                return dict[url];
            }
            catch {
                return ERatingAgency.NR;
            }
        }

        /// <summary>
        /// Converts an <see cref="ERatingAgency"/> enum back into a friendly display name for the rating agency
        /// </summary>
        /// <param name="agency"></param>
        /// <returns></returns>
        public static string ToFriendlyName(ERatingAgency agency) {
            var dict = new Dictionary<ERatingAgency, string>() {
                { ERatingAgency.MPAA, "Motion Picture Association of America" },
                { ERatingAgency.BBFC, "British Board of Film Classification" },
                { ERatingAgency.CNC, "Centre National du Cinéma et de l'image animée" },
                { ERatingAgency.FSK, "Freiwillige Selbstkontrolle der Filmwirtschaft" },
                { ERatingAgency.Eirin, "Film Classification and Rating Organization (Eirin)" },
                { ERatingAgency.RDC_QC, "Régie du Cinéma (Quebec)" },
                { ERatingAgency.RDC_BC, "Régie du Cinéma (British Columbia)" },
                { ERatingAgency.CBFC, "Central Board of Film Certification" },
                { ERatingAgency.ACB, "Australian Classification Board" },
                { ERatingAgency.NR, "Not Rated / Unknown Agency" }
            };

            try {
                return dict[agency];
            }
            catch {
                return dict[ERatingAgency.NR];
            }
        }
    }
}
