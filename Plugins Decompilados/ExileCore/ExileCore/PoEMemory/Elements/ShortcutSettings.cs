// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ShortcutSettings
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class ShortcutSettings : Element
  {
    public StdVector ShortcutArray => this.M.Read<StdVector>(this.Address + 760L);

    public IList<Shortcut> Shortcuts => (IList<Shortcut>) this.M.ReadStdVector<Shortcut>(this.ShortcutArray);

    public Shortcut LeagueInterfaceShortcut => this.Shortcuts.FirstOrDefault<Shortcut>((Func<Shortcut, bool>) (x => x.Usage == ShortcutUsage.LeagueInterface));

    public Shortcut LeaguePanelShortcut => this.Shortcuts.FirstOrDefault<Shortcut>((Func<Shortcut, bool>) (x => x.Usage == ShortcutUsage.LeaguePanel));

    public Shortcut StalkerSentinelShortcut => this.Shortcuts.FirstOrDefault<Shortcut>((Func<Shortcut, bool>) (x => x.Usage == ShortcutUsage.StalkerSentinel));

    public Shortcut PandemoniumSentinelShortcut => this.Shortcuts.FirstOrDefault<Shortcut>((Func<Shortcut, bool>) (x => x.Usage == ShortcutUsage.PandemoniumSentinel));

    public Shortcut ApexSentinelShortcut => this.Shortcuts.FirstOrDefault<Shortcut>((Func<Shortcut, bool>) (x => x.Usage == ShortcutUsage.ApexSentinel));
  }
}
