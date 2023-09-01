// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.SetParts.RingItemsSetPart
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace FullRareSetManager.SetParts
{
  public class RingItemsSetPart : BaseSetPart
  {
    private StashItem[] _currentSetItems;
    public List<StashItem> HighLvlItems = new List<StashItem>();
    public List<StashItem> LowLvlItems = new List<StashItem>();

    public RingItemsSetPart(string partName)
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

    public override int TotalSetsCount() => (this.HighLvlItems.Count + this.LowLvlItems.Count) / 2;

    public override int LowSetsCount()
    {
      int num = this.LowLvlItems.Count - this.HighLvlItems.Count;
      return num <= 0 ? this.LowLvlItems.Count : this.HighLvlItems.Count + num / 2;
    }

    public override int HighSetsCount() => this.HighLvlItems.Count / 2;

    public override string GetInfoString() => this.PartName + ": " + this.TotalSetsCount().ToString() + " (" + this.LowSetsCount().ToString() + "L / " + this.HighSetsCount().ToString() + "H)";

    public override PrepareItemResult PrepareItemForSet(FullRareSetManagerSettings settings)
    {
      bool flag1 = this.HighLvlItems.Count >= 1 && this.HighLvlItems[0].BInPlayerInventory;
      bool flag2 = this.LowLvlItems.Count >= 1 && this.LowLvlItems[0].BInPlayerInventory;
      if (flag1 & flag2)
      {
        this._currentSetItems = new StashItem[2]
        {
          this.HighLvlItems[0],
          this.LowLvlItems[0]
        };
        return new PrepareItemResult()
        {
          AllowedReplacesCount = this.LowLvlItems.Count - 1,
          LowSet = true,
          BInPlayerInvent = true
        };
      }
      if (flag1)
      {
        if ((this.HighLvlItems.Count < 2 ? 0 : (this.HighLvlItems[1].BInPlayerInventory ? 1 : 0)) != 0)
        {
          this._currentSetItems = new StashItem[2]
          {
            this.HighLvlItems[0],
            this.HighLvlItems[1]
          };
          return new PrepareItemResult()
          {
            AllowedReplacesCount = this.LowLvlItems.Count,
            LowSet = false,
            BInPlayerInvent = true
          };
        }
        PrepareItemResult prepareItemResult1 = this.PrepareHigh();
        if (prepareItemResult1 != null)
          return prepareItemResult1;
        PrepareItemResult prepareItemResult2 = this.PrepareMixedHl();
        if (prepareItemResult2 != null)
          return prepareItemResult2;
      }
      else if (flag2)
      {
        if ((this.LowLvlItems.Count < 2 ? 0 : (this.LowLvlItems[1].BInPlayerInventory ? 1 : 0)) != 0)
        {
          this._currentSetItems = new StashItem[2]
          {
            this.LowLvlItems[0],
            this.LowLvlItems[1]
          };
          return new PrepareItemResult()
          {
            AllowedReplacesCount = this.LowLvlItems.Count - 2,
            LowSet = true,
            BInPlayerInvent = true
          };
        }
        PrepareItemResult prepareItemResult = this.PrepareMixedHl() ?? this.PrepareLow();
        if (prepareItemResult != null)
          return prepareItemResult;
      }
      else
      {
        PrepareItemResult prepareItemResult3 = this.PrepareHigh();
        if (prepareItemResult3 != null)
          return prepareItemResult3;
        PrepareItemResult prepareItemResult4 = this.PrepareMixedHl();
        if (prepareItemResult4 != null)
          return prepareItemResult4;
        PrepareItemResult prepareItemResult5 = this.PrepareLow();
        if (prepareItemResult5 != null)
          return prepareItemResult5;
      }
      return new PrepareItemResult();
    }

    private PrepareItemResult PrepareHigh()
    {
      if (this.HighLvlItems.Count < 2)
        return (PrepareItemResult) null;
      if ((this.HighLvlItems[0].BInPlayerInventory ? 1 : (this.HighLvlItems[1].BInPlayerInventory ? 1 : 0)) == 0)
        this.HighLvlItems = this.HighLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
      this._currentSetItems = new StashItem[2]
      {
        this.HighLvlItems[0],
        this.HighLvlItems[1]
      };
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowLvlItems.Count,
        LowSet = false,
        BInPlayerInvent = false
      };
    }

    private PrepareItemResult PrepareMixedHl()
    {
      if (this.HighLvlItems.Count < 1 || this.LowLvlItems.Count < 1)
        return (PrepareItemResult) null;
      if (!this.HighLvlItems[0].BInPlayerInventory)
      {
        this.HighLvlItems = this.HighLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
        if ((this.LowLvlItems.Count <= 1 ? 0 : (this.LowLvlItems[1].BInPlayerInventory ? 1 : 0)) == 0)
          this.HighLvlItems = this.HighLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
      }
      this._currentSetItems = new StashItem[2]
      {
        this.HighLvlItems[0],
        this.LowLvlItems[0]
      };
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowLvlItems.Count - 1,
        LowSet = true,
        BInPlayerInvent = false
      };
    }

    private PrepareItemResult PrepareLow()
    {
      if (this.LowLvlItems.Count < 2)
        return (PrepareItemResult) null;
      if ((this.LowLvlItems[0].BInPlayerInventory ? 1 : (this.LowLvlItems[1].BInPlayerInventory ? 1 : 0)) == 0)
        this.LowLvlItems = this.LowLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
      this._currentSetItems = new StashItem[2]
      {
        this.LowLvlItems[0],
        this.LowLvlItems[1]
      };
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowLvlItems.Count - 2,
        LowSet = true,
        BInPlayerInvent = false
      };
    }

    public override void DoLowItemReplace()
    {
      if (this.HighLvlItems.Count >= 1 && this.LowLvlItems.Count >= 1)
      {
        if (!this.LowLvlItems[0].BInPlayerInventory)
          this.LowLvlItems = this.LowLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
        if (!this.HighLvlItems[0].BInPlayerInventory)
          this.HighLvlItems = this.HighLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
        this._currentSetItems = new StashItem[2]
        {
          this.HighLvlItems[0],
          this.LowLvlItems[0]
        };
      }
      else
      {
        if (this.LowLvlItems.Count < 2)
          return;
        if (!this.LowLvlItems[0].BInPlayerInventory)
          this.LowLvlItems = this.LowLvlItems.OrderByDescending<StashItem, int>((Func<StashItem, int>) (x => x.InventPosX + x.InventPosY * 12)).ToList<StashItem>();
        this._currentSetItems = new StashItem[2]
        {
          this.LowLvlItems[0],
          this.LowLvlItems[1]
        };
      }
    }

    public override StashItem[] GetPreparedItems() => this._currentSetItems;

    public override void RemovePreparedItems()
    {
      this.RemoveItem(this._currentSetItems[0]);
      this.RemoveItem(this._currentSetItems[1]);
    }

    private void RemoveItem(StashItem item)
    {
      if (item.LowLvl)
        this.LowLvlItems.Remove(item);
      else
        this.HighLvlItems.Remove(item);
    }

    public override int PlayerInventItemsCount() => this.HighLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory)) + this.LowLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory));
  }
}
