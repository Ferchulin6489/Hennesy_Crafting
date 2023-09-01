// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ItemOnGroundTooltip
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class ItemOnGroundTooltip : Element
  {
    public Element ItemFrame => this.TooltipUI?.GetChildAtIndex(1);

    public Element TooltipUI => this.GetChildAtIndex(0)?.GetChildAtIndex(0);

    public Element Item2DIcon => this.TooltipUI?.GetChildAtIndex(0);

    public new Element Tooltip => this.TooltipUI;
  }
}
