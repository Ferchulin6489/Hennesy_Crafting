// Decompiled with JetBrains decompiler
// Type: AdvancedTooltip.Settings.ItemModsSettings
// Assembly: AdvancedTooltip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 655408A3-EFFF-42CC-9A14-FFB321F3C387
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\AdvancedTooltip\AdvancedTooltip.dll

using ExileCore.Shared.Attributes;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace AdvancedTooltip.Settings
{
  [Submenu]
  public class ItemModsSettings
  {
    public ToggleNode Enable { get; set; } = new ToggleNode(true);

    public ToggleNode EnableFastMods { get; set; } = new ToggleNode(true);

    public ToggleNode EnableFastModsTags { get; set; } = new ToggleNode(true);

    public ToggleNode ShowModNames { get; set; } = new ToggleNode(true);

    public ToggleNode StartStatsOnSameLine { get; set; } = new ToggleNode(false);

    public ColorNode BackgroundColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 0, (byte) 0, (byte) 0, (byte) 220));

    public ColorNode PrefixColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 136, (byte) 136, byte.MaxValue, byte.MaxValue));

    public ColorNode SuffixColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 0, (byte) 206, (byte) 209, byte.MaxValue));

    public ColorNode T1Color { get; set; } = ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue));

    public ColorNode T2Color { get; set; } = ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue));

    public ColorNode T3Color { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 0, byte.MaxValue, (byte) 0, byte.MaxValue));
  }
}
