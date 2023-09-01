// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.RitualWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.InventoryElements;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class RitualWindow : Element
  {
    public Element InventoryElement => this.ReadObjectAt<Element>(704);

    public List<NormalInventoryItem> Items => this.InventoryElement.GetChildrenAs<NormalInventoryItem>();
  }
}
