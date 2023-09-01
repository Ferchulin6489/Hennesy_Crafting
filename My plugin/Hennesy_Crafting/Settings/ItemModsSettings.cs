using ExileCore.Shared.Attributes;
using ExileCore.Shared.Nodes;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hennesy_Crafting.Settings
{
    [Submenu]
    public class ItemModsSettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);

        public ToggleNode EnableFastMods { get; set; } = new ToggleNode(true);

        public ToggleNode EnableFastModsTags { get; set; } = new ToggleNode(true);

        public ToggleNode ShowModNames { get; set; } = new ToggleNode(true);

        public ToggleNode StartStatsOnSameLine { get; set; } = new ToggleNode(false);

        public ColorNode BackgroundColor { get; set; } = new ColorBGRA((byte)0, (byte)0, (byte)0, (byte)220);

        public ColorNode PrefixColor { get; set; } = new ColorBGRA((byte)136, (byte)136, byte.MaxValue, byte.MaxValue);

        public ColorNode SuffixColor { get; set; } = new ColorBGRA((byte)0, (byte)206, (byte)209, byte.MaxValue);

        public ColorNode T1Color { get; set; } = new ColorBGRA(byte.MaxValue, (byte)0, byte.MaxValue, byte.MaxValue);

        public ColorNode T2Color { get; set; } = new ColorBGRA(byte.MaxValue, byte.MaxValue, (byte)0, byte.MaxValue);

        public ColorNode T3Color { get; set; } = new ColorBGRA((byte)0, byte.MaxValue, (byte)0, byte.MaxValue);
    }
}
