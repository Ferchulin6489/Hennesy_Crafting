// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.SetParts.SingleItemSetPart
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace FullRareSetManager.SetParts
{
  public class SingleItemSetPart : BaseSetPart
  {
    private StashItem _currentSetItem;
    public List<StashItem> HighLvlItems = new List<StashItem>();
    public List<StashItem> LowLvlItems = new List<StashItem>();

    public SingleItemSetPart(string partName)
      : base(partName)
    {
    }

    public override void AddItem(StashItem item)
    {
      if (item.LowLvl)
        this.LowLvlItems.Add(item);
      else
        this.HighLvlItems.Add(item);
    }

    public override int TotalSetsCount() => this.HighLvlItems.Count + this.LowLvlItems.Count;

    public override int LowSetsCount() => this.LowLvlItems.Count;

    public override int HighSetsCount() => this.HighLvlItems.Count;

    public override string GetInfoString() => this.PartName + ": " + this.TotalSetsCount().ToString() + " (" + this.LowSetsCount().ToString() + "L / " + this.HighSetsCount().ToString() + "H)";

    public override PrepareItemResult PrepareItemForSet(FullRareSetManagerSettings settings)
    {
      if ((this.LowLvlItems.Count <= 0 ? 0 : (this.LowLvlItems[0].BInPlayerInventory ? 1 : 0)) != 0)
      {
        PrepareItemResult prepareItemResult1 = this.LowProcess();
        if (prepareItemResult1 != null)
          return prepareItemResult1;
        PrepareItemResult prepareItemResult2 = this.HighProcess();
        if (prepareItemResult2 != null)
          return prepareItemResult2;
      }
      else
      {
        PrepareItemResult prepareItemResult3 = this.HighProcess();
        if (prepareItemResult3 != null)
          return prepareItemResult3;
        PrepareItemResult prepareItemResult4 = this.LowProcess();
        if (prepareItemResult4 != null)
          return prepareItemResult4;
      }
      return (PrepareItemResult) null;
    }

    private PrepareItemResult HighProcess()
    {
      if (this.HighLvlItems.Count <= 0)
        return (PrepareItemResult) null;
      if (!this.HighLvlItems[0].BInPlayerInventory)
        this.HighLvlItems = this.HighLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
      this._currentSetItem = this.HighLvlItems[0];
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowLvlItems.Count,
        LowSet = false,
        BInPlayerInvent = this._currentSetItem.BInPlayerInventory
      };
    }

    private PrepareItemResult LowProcess()
    {
      if (this.LowLvlItems.Count <= 0)
        return (PrepareItemResult) null;
      if (!this.LowLvlItems[0].BInPlayerInventory)
        this.LowLvlItems = this.LowLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
      this._currentSetItem = this.LowLvlItems[0];
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowLvlItems.Count - 1,
        LowSet = true,
        BInPlayerInvent = this._currentSetItem.BInPlayerInventory
      };
    }

    public override void DoLowItemReplace()
    {
      if (this.LowLvlItems.Count <= 0)
        return;
      this.LowLvlItems = this.LowLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
      this._currentSetItem = this.LowLvlItems[0];
    }

    public override StashItem[] GetPreparedItems() => new StashItem[1]
    {
      this._currentSetItem
    };

    public override void RemovePreparedItems()
    {
      if (this._currentSetItem.LowLvl)
        this.LowLvlItems.Remove(this._currentSetItem);
      else
        this.HighLvlItems.Remove(this._currentSetItem);
    }

    public override int PlayerInventItemsCount() => this.HighLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory)) + this.LowLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory));
  }
}
