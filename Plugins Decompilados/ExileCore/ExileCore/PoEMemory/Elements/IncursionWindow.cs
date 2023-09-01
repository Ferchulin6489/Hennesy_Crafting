// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.IncursionWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class IncursionWindow : Element
  {
    public Element AcceptElement
    {
      get
      {
        try
        {
          Element childFromIndices = this.GetChildFromIndices(3, 13, 2);
          if (childFromIndices.GetChildAtIndex(0).Text == "enter incursion")
            return childFromIndices;
        }
        catch
        {
        }
        return (Element) null;
      }
    }

    public string Reward1 => this.GetChildFromIndices(3, 13, 3).Text;

    public string Reward2 => this.GetChildFromIndices(3, 13, 4).Text;
  }
}
