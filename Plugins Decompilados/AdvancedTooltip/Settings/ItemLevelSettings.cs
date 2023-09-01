// Decompiled with JetBrains decompiler
// Type: AdvancedTooltip.Settings.ItemLevelSettings
// Assembly: AdvancedTooltip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 655408A3-EFFF-42CC-9A14-FFB321F3C387
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\AdvancedTooltip\AdvancedTooltip.dll

using ExileCore.Shared.Attributes;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace AdvancedTooltip.Settings
{
  [Submenu]
  public class ItemLevelSettings
  {
    public ToggleNode Enable { get; set; } = new ToggleNode(true);

    public RangeNode<int> TextSize { get; set; } = new RangeNode<int>(16, 10, 50);

    public ColorNode TextColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue));

    public ColorNode BackgroundColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 0, (byte) 0, (byte) 0, (byte) 230));
  }
}
