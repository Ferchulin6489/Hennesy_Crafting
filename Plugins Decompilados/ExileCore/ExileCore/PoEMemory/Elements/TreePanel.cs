// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.TreePanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class TreePanel : Element
  {
    private const int CanvasElementOffset = 1024;

    public Element CanvasElement => this.ReadObjectAt<Element>(1024);
  }
}
