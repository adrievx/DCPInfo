using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCPUtils.Models.Structs;
using System.Xml.Linq;
using DCPUtils.Enum;

namespace DCPUtils.Utils {
    public class XmlParserUtils {
        /// <summary>
        /// Parses a sound configuration string into a <see cref="FSoundConfiguration"/> object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static FSoundConfiguration ParseSoundConfiguration(string value) {
            var spl = value.Split('/');

            if (spl.Length != 2) {
                throw new FormatException("Invalid sound configuration format. Expected 'typecode/channels'");
            }

            int typeCode;
            if (!int.TryParse(spl[0], out typeCode)) {
                throw new FormatException("Invalid typecode.");
            }

            ESoundType soundType;
            switch (typeCode) {
                case 10: soundType = ESoundType.Mono; break;
                case 20: soundType = ESoundType.Stereo; break;
                case 21: soundType = ESoundType.StereoCent; break;
                case 31: soundType = ESoundType.StereoCentLFE; break;
                case 51: soundType = ESoundType.Dolby51; break;
                case 61: soundType = ESoundType.Dolby51HI; break;
                case 71: soundType = ESoundType.Dolby71; break;
                case 911: soundType = ESoundType.Dolby51HIViN; break;
                case 714: soundType = ESoundType.Atmos; break;
                default:
                    throw new NotSupportedException("Unsupported sound type code was specified: " + typeCode);
                    break;
            }

            var channelStrings = spl[1].Split(',');
            var channels = new List<ESoundChannel>();

            foreach (var raw in channelStrings) {
                var ch = raw.Trim();

                if (ch == "L") channels.Add(ESoundChannel.Left);
                else if (ch == "R") channels.Add(ESoundChannel.Right);
                else if (ch == "C") channels.Add(ESoundChannel.Center);
                else if (ch == "LFE") channels.Add(ESoundChannel.LFE);
                else if (ch == "Ls") channels.Add(ESoundChannel.LeftSurround);
                else if (ch == "Rs") channels.Add(ESoundChannel.RightSurround);
                else if (ch == "Lrs") channels.Add(ESoundChannel.LeftRearSurround);
                else if (ch == "Rrs") channels.Add(ESoundChannel.RightRearSurround);
                else if (ch == "HI") channels.Add(ESoundChannel.HearingImpairment);
                else if (ch == "VI") channels.Add(ESoundChannel.VisualImpairmment);
                else if (ch == "AD") channels.Add(ESoundChannel.AudioDescription);
                else if (ch == "OHI") channels.Add(ESoundChannel.OtherHearingImpairment);
                else if (ch == "OVI") channels.Add(ESoundChannel.OtherVisionImpairment);
                else {
                    throw new NotSupportedException("Unsupported channel specification: " + ch);
                }
            }

            return new FSoundConfiguration {
                Type = soundType,
                Channels = channels
            };
        }

        /// <summary>
        /// Parses a sample rate string into a <see cref="FSampleRate"/> object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FSampleRate ParseSampleRate(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return default;
            }

            var data = ParseSplitNumerator(value);

            return new FSampleRate {
                SampleRate = data.numerator,
                Denominator = data.denominator
            };
        }

        /// <summary>
        /// Parses a string into a <see cref="Tuple{T1, T2, T3}"/> of a numerator, denominator and the length of its parts.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static (int numerator, int denominator, int partsLen) ParseSplitNumerator(string value) {
            var parts = value.Split(' ');

            if(parts.Length == 2) {
                return (int.Parse(parts.FirstOrDefault()), int.Parse(parts.Last()), parts.Length);
            }
            else if(parts.Length == 1) {
                return (int.Parse(parts.FirstOrDefault()), 1, parts.Length); // only parse denominator if not present
            }
            else {
                throw new IndexOutOfRangeException($"Unable to parse {parts.Length} value array, expected 1 or 2 values.");
            }
        }

        /// <summary>
        /// Parses a <see cref="Point"/> from an <see cref="XElement"/>.
        /// </summary>
        /// <param name="pointElem"></param>
        /// <param name="meta"></param>
        /// <returns></returns>
        public static Point ParsePoint(XElement pointElem, XNamespace meta) {
            if (pointElem == null) {
                return default;
            }

            return new Point {
                X = int.Parse(pointElem.Element(meta + "Width")?.Value ?? "0"),
                Y = int.Parse(pointElem.Element(meta + "Height")?.Value ?? "0")
            };
        }
    }
}
