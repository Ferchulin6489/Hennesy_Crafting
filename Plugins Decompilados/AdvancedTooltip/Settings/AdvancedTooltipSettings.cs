// Decompiled with JetBrains decompiler
// Type: AdvancedTooltip.Settings.AdvancedTooltipSettings
// Assembly: AdvancedTooltip, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 655408A3-EFFF-42CC-9A14-FFB321F3C387
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\AdvancedTooltip\AdvancedTooltip.dll

using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace AdvancedTooltip.Settings
{
  public class AdvancedTooltipSettings : ISettings
  {
    public ItemLevelSettings ItemLevel { get; set; } = new ItemLevelSettings();

    public ItemModsSettings ItemMods { get; set; } = new ItemModsSettings();

    public WeaponDpsSettings WeaponDps { get; set; } = new WeaponDpsSettings();

    public ToggleNode Enable { get; set; } = new ToggleNode(false);
  }
}
