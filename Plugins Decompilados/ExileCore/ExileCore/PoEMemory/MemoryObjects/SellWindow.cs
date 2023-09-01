// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.SellWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.InventoryElements;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class SellWindow : Element
  {
    public virtual Element SellDialog => this.GetChildAtIndex(3);

    public virtual Element YourOffer => this.SellDialog?.GetChildAtIndex(0);

    public List<NormalInventoryItem> YourOfferItems
    {
      get
      {
        Element yourOffer = this.YourOffer;
        return (yourOffer != null ? yourOffer.GetChildrenAs<NormalInventoryItem>().Skip<NormalInventoryItem>(2).ToList<NormalInventoryItem>() : (List<NormalInventoryItem>) null) ?? new List<NormalInventoryItem>();
      }
    }

    public virtual Element OtherOffer => this.SellDialog?.GetChildAtIndex(1);

    public List<NormalInventoryItem> OtherOfferItems
    {
      get
      {
        Element otherOffer = this.OtherOffer;
        return (otherOffer != null ? otherOffer.GetChildrenAs<NormalInventoryItem>().Skip<NormalInventoryItem>(1).ToList<NormalInventoryItem>() : (List<NormalInventoryItem>) null) ?? new List<NormalInventoryItem>();
      }
    }

    public string NameSeller => this.SellDialog?.GetChildAtIndex(2)?.Text ?? "";

    public Element AcceptButton => this.SellDialog?.GetChildAtIndex(5);

    public Element CancelButton => this.SellDialog?.GetChildAtIndex(6);
  }
}
