// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ExpeditionElements.ExpeditionVendorElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements.ExpeditionElements
{
  public class ExpeditionVendorElement : Element
  {
    public SearchBarElement HighlightSearchbarElement => this.GetChildFromIndices(4, 3).AsObject<SearchBarElement>();

    public Element VendorResponseTextBox => this.GetChildFromIndices(6, 1);

    public string VendorWindowTitle => this.GetChildFromIndices(6, 2, 0).Text;

    public Element RefreshItemsButton => this.GetChildFromIndices(7, 0);

    public Element RefreshCurrencyInfoBox => this.GetChildFromIndices(7, 1);

    public List<NormalInventoryItem> InventoryItems => this.GetChildFromIndices(8, 1, 0, 0).GetChildrenAs<NormalInventoryItem>().Skip<NormalInventoryItem>(1).ToList<NormalInventoryItem>();

    public ExpeditionVendorCurrencyInfoElement CurrencyInfo => this[9]?.AsObject<ExpeditionVendorCurrencyInfoElement>();

    public List<Entity> OfferedItems => this.InventoryItems.Select<NormalInventoryItem, Entity>((Func<NormalInventoryItem, Entity>) (elem => elem.Item)).ToList<Entity>();

    public TujenHaggleWindowElement TujenHaggleWindow => this.GetChildFromIndices(11, 0).AsObject<TujenHaggleWindowElement>();
  }
}
