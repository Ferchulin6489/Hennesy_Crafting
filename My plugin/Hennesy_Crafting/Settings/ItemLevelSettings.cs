using ExileCore.Shared.Attributes;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace Hennesy_Crafting.Settings
{
    [Submenu]
    public class ItemLevelSettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);

        public RangeNode<int> TextSize { get; set; } = new RangeNode<int>(16, 10, 50);

        public ColorNode TextColor { get; set; } = new ColorBGRA(byte.MaxValue, byte.MaxValue, (byte)0, byte.MaxValue); // ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, byte.MaxValue, (byte)0, byte.MaxValue));

        public ColorNode BackgroundColor { get; set; } = new ColorBGRA((byte)0, (byte)0, (byte)0, (byte)230);
    }
}
