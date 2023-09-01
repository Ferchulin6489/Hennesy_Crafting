// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.BanditDialog
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.PoEMemory.Elements
{
  public class BanditDialog : Element
  {
    public Element HelpButton => this.GetChildAtIndex(2)?.GetChildAtIndex(0);

    public Element KillButton => this.GetChildAtIndex(2)?.GetChildAtIndex(1);

    public BanditType BanditType => this.GetBanditType();

    private BanditType GetBanditType()
    {
      if (this.HelpButton == null)
        DebugWindow.LogError("BanditDialog.HelpButton is null, either window is not open or check offsets");
      string lower = this.HelpButton?.GetChildAtIndex(0)?.Text?.ToLower();
      if (lower == null)
        throw new ArgumentException();
      if (lower.Contains("kraityn"))
        return BanditType.Kraityn;
      if (lower.Contains("alira"))
        return BanditType.Alira;
      if (lower.Contains("oak"))
        return BanditType.Oak;
      throw new ArgumentException();
    }
  }
}
