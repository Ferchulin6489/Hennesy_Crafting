// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.NpcLine
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.PoEMemory.Elements
{
  public class NpcLine
  {
    public NpcLine(Element element)
    {
      this.Element = element ?? throw new ArgumentNullException(nameof (element));
      this.Text = this.Element.GetChildAtIndex(0)?.Text ?? throw new ArgumentOutOfRangeException(nameof (element));
    }

    public Element Element { get; }

    public string Text { get; }

    public override string ToString() => this.Text;
  }
}
