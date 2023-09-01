// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.SyndicatePanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class SyndicatePanel : Element
  {
    public Element EventElement
    {
      get
      {
        Element childAtIndex1 = this.GetChildAtIndex(0);
        if (childAtIndex1.ChildCount < 25L)
          return (Element) null;
        Element childAtIndex2 = childAtIndex1.GetChildAtIndex(24);
        return childAtIndex2.GetChildFromIndices(8, 1) == null ? childAtIndex1.GetChildAtIndex(25) : childAtIndex2;
      }
    }

    public Element TextElement => this.EventElement?.GetChildFromIndices(8, 1);

    public string EventText => this.TextElement.Text;
  }
}
