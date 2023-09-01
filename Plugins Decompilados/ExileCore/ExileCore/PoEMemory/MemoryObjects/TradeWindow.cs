// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.TradeWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.InventoryElements;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class TradeWindow : Element
  {
    public Element SellDialog => this.GetChildAtIndex(3)?.GetChildAtIndex(1)?.GetChildAtIndex(0)?.GetChildAtIndex(0);

    public Element YourOfferElement => this.SellDialog?.GetChildAtIndex(0);

    public IList<NormalInventoryItem> YourOffer => this.ExtractNormalInventoryItems(this.YourOfferElement?.Children);

    public Element OtherOfferElement => this.SellDialog?.GetChildAtIndex(1);

    public IList<NormalInventoryItem> OtherOffer => this.ExtractNormalInventoryItems(this.OtherOfferElement?.Children);

    public string NameSeller => this.SellDialog?.GetChildAtIndex(2).Text.Replace("'s Offer", "");

    public Element AcceptButton => this.SellDialog?.GetChildAtIndex(5);

    public bool SellerAccepted => this.AcceptButton?.GetChildAtIndex(0).Text == "cancel accept";

    public Element CancelButton => this.SellDialog?.GetChildAtIndex(6);

    private IList<NormalInventoryItem> ExtractNormalInventoryItems(IList<Element> children)
    {
      List<NormalInventoryItem> normalInventoryItems = new List<NormalInventoryItem>();
      for (int index = 1; index < children.Count; ++index)
        normalInventoryItems.Add(children[index].AsObject<NormalInventoryItem>());
      return (IList<NormalInventoryItem>) normalInventoryItems;
    }
  }
}
