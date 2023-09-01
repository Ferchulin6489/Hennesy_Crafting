// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.StashTabElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class StashTabElement : Element
  {
    public Element StashTab => this.Address == 0L ? (Element) null : this.ReadObjectAt<Element>(448);

    public string TabName => this.StashTab?.Text ?? string.Empty;
  }
}
