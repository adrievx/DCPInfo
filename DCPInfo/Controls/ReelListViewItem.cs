using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCPUtils.Models.Composition;
using DCPUtils.Models.Misc;
using DCPUtils.Models.Structs;

namespace DCPInfo.Controls {
    internal class ReelListViewItem : ListViewItem {
        public CompositionReel Reel { get; }

        public ReelListViewItem(CompositionReel reel) : base(reel.UUID.ToString()) {
            Reel = reel;

            SubItems.Add(reel.MainPicture.EditRate.GetRealValue().ToString());
            SubItems.Add(reel.Metadata.IntrinsicDuration.ToString());
            SubItems.Add(reel.MainMarkers.MarkerList.Count.ToString());
            SubItems.Add(resolutionToString(reel.Metadata.MainPictureStoredArea));
            SubItems.Add(resolutionToString(reel.Metadata.MainPictureActiveArea));
            SubItems.Add($"{reel.Metadata.MainSoundSampleRate.GetRealValue().ToString()}Hz");
            SubItems.Add(soundConfigurationToString(reel.Metadata.MainSoundConfiguration));
        }

        private string soundConfigurationToString(FSoundConfiguration soundConfig) {
            var list = new List<string>();

            var channelMap = new Dictionary<DCPUtils.Enum.ESoundChannel, string>() {
                { DCPUtils.Enum.ESoundChannel.Left, "L" },
                { DCPUtils.Enum.ESoundChannel.Right, "R" },
                { DCPUtils.Enum.ESoundChannel.Center, "C" },
                { DCPUtils.Enum.ESoundChannel.LFE, "LFE" },
                { DCPUtils.Enum.ESoundChannel.LeftSurround, "Ls" },
                { DCPUtils.Enum.ESoundChannel.RightSurround, "Rs" },
                { DCPUtils.Enum.ESoundChannel.LeftRearSurround, "Lrs" },
                { DCPUtils.Enum.ESoundChannel.RightRearSurround, "Rrs" },
                { DCPUtils.Enum.ESoundChannel.HearingImpairment, "HI" },
                { DCPUtils.Enum.ESoundChannel.VisualImpairmment, "VI" },
                { DCPUtils.Enum.ESoundChannel.AudioDescription, "AD" },
                { DCPUtils.Enum.ESoundChannel.OtherHearingImpairment, "OHI" },
                { DCPUtils.Enum.ESoundChannel.OtherVisionImpairment, "OVI" }
            };

            return string.Join(", ", soundConfig.Channels.Where(c => channelMap.ContainsKey(c)).Select(c => channelMap[c]));
        }

        private string resolutionToString(Point v) {
            return $"{v.X}x{v.Y}";
        }
    }
}
