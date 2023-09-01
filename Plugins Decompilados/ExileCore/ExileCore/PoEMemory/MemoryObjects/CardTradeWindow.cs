// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.CardTradeWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.InventoryElements;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class CardTradeWindow : Element
  {
    public Element CardSlotElement => this.GetChildAtIndex(5);

    public NormalInventoryItem CardSlotItem => this.CardSlotElement.GetChildFromIndices(1)?.AsObject<NormalInventoryItem>();

    public Element TradeButton => this.GetChildAtIndex(4);
  }
}
