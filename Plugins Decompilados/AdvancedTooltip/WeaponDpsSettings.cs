// Decompiled with JetBrains decompiler
// Type: AdvancedTooltip.WeaponDpsSettings
// Assembly: AdvancedTooltip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 655408A3-EFFF-42CC-9A14-FFB321F3C387
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\AdvancedTooltip\AdvancedTooltip.dll

using ExileCore.Shared.Attributes;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace AdvancedTooltip
{
  [Submenu]
  public class WeaponDpsSettings
  {
    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    public ColorNode TextColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 254, (byte) 192, (byte) 118, byte.MaxValue));

    public RangeNode<int> DpsTextSize { get; set; } = new RangeNode<int>(16, 10, 50);

    public RangeNode<int> DpsNameTextSize { get; set; } = new RangeNode<int>(13, 10, 50);

    public ColorNode BackgroundColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue));

    public ColorNode DmgFireColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 150, (byte) 0, (byte) 0, byte.MaxValue));

    public ColorNode DmgColdColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 54, (byte) 100, (byte) 146, byte.MaxValue));

    public ColorNode DmgLightningColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, (byte) 215, (byte) 0, byte.MaxValue));

    public ColorNode DmgChaosColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 208, (byte) 31, (byte) 144, byte.MaxValue));

    public ColorNode PhysicalDamageColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));

    public ColorNode ElementalDamageColor { get; set; } = ColorNode.op_Implicit(new ColorBGRA((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue));
  }
}
