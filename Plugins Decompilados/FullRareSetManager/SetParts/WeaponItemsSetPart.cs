// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.SetParts.WeaponItemsSetPart
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace FullRareSetManager.SetParts
{
  public class WeaponItemsSetPart : BaseSetPart
  {
    private StashItem[] _currentSetItems;
    public List<StashItem> OneHandedHighLvlItems = new List<StashItem>();
    public List<StashItem> OneHandedLowLvlItems = new List<StashItem>();
    public List<StashItem> TwoHandedHighLvlItems = new List<StashItem>();
    public List<StashItem> TwoHandedLowLvlItems = new List<StashItem>();

    public WeaponItemsSetPart(string partName)
      : base(partName)
    {
    }

    public override void AddItem(StashItem item)
    {
      if (item.ItemType == StashItemType.TwoHanded)
      {
        if (item.LowLvl)
          this.TwoHandedLowLvlItems.Add(item);
        else
          this.TwoHandedHighLvlItems.Add(item);
      }
      else if (item.LowLvl)
        this.OneHandedLowLvlItems.Add(item);
      else
        this.OneHandedHighLvlItems.Add(item);
    }

    public override int LowSetsCount()
    {
      int count = this.TwoHandedLowLvlItems.Count;
      int num1 = this.OneHandedLowLvlItems.Count - this.OneHandedHighLvlItems.Count;
      int num2 = num1 > 0 ? this.OneHandedHighLvlItems.Count + num1 / 2 : this.OneHandedLowLvlItems.Count;
      return count + num2;
    }

    public override int HighSetsCount() => this.TwoHandedHighLvlItems.Count + this.OneHandedHighLvlItems.Count / 2;

    public override int TotalSetsCount() => this.TwoHandedLowLvlItems.Count + this.TwoHandedHighLvlItems.Count + (this.OneHandedHighLvlItems.Count + this.OneHandedLowLvlItems.Count) / 2;

    public override string GetInfoString()
    {
      string infoString = "Weapons: " + this.TotalSetsCount().ToString() + " (" + this.LowSetsCount().ToString() + "L / " + this.HighSetsCount().ToString() + "H)";
      int num1 = this.TwoHandedLowLvlItems.Count + this.TwoHandedHighLvlItems.Count;
      if (num1 > 0)
      {
        string[] strArray = new string[8]
        {
          infoString,
          "\r\n     Two Handed: ",
          num1.ToString(),
          " (",
          null,
          null,
          null,
          null
        };
        int count = this.TwoHandedLowLvlItems.Count;
        strArray[4] = count.ToString();
        strArray[5] = "L / ";
        count = this.TwoHandedHighLvlItems.Count;
        strArray[6] = count.ToString();
        strArray[7] = "H)";
        infoString = string.Concat(strArray);
      }
      int num2 = this.OneHandedLowLvlItems.Count + this.OneHandedHighLvlItems.Count;
      if (num2 > 0)
      {
        string[] strArray = new string[8]
        {
          infoString,
          "\r\n     One Handed: ",
          (num2 / 2).ToString(),
          " (",
          null,
          null,
          null,
          null
        };
        int count = this.OneHandedLowLvlItems.Count;
        strArray[4] = count.ToString();
        strArray[5] = "L / ";
        count = this.OneHandedHighLvlItems.Count;
        strArray[6] = count.ToString();
        strArray[7] = "H)";
        infoString = string.Concat(strArray);
      }
      return infoString;
    }

    public override PrepareItemResult PrepareItemForSet(FullRareSetManagerSettings settings)
    {
      bool flag = settings.WeaponTypePriority.Value == "One handed";
      if (!flag)
      {
        if (this.OneHandedHighLvlItems.Count > 0 && this.OneHandedHighLvlItems[0].BInPlayerInventory)
          flag = true;
        else if (this.OneHandedLowLvlItems.Count > 0 && this.OneHandedLowLvlItems[0].BInPlayerInventory)
          flag = true;
      }
      else if (this.TwoHandedHighLvlItems.Count > 0 && this.TwoHandedHighLvlItems[0].BInPlayerInventory)
        flag = false;
      else if (this.TwoHandedLowLvlItems.Count > 0 && this.TwoHandedLowLvlItems[0].BInPlayerInventory)
        flag = false;
      Func<PrepareItemResult>[] funcArray = new Func<PrepareItemResult>[5];
      if (flag)
      {
        funcArray[0] = new Func<PrepareItemResult>(this.Prepahe_OH);
        funcArray[1] = new Func<PrepareItemResult>(this.Prepahe_OHOL);
        funcArray[2] = new Func<PrepareItemResult>(this.Prepahe_OL);
        funcArray[3] = new Func<PrepareItemResult>(this.Prepahe_TH);
        funcArray[4] = new Func<PrepareItemResult>(this.Prepahe_TL);
      }
      else
      {
        funcArray[0] = new Func<PrepareItemResult>(this.Prepahe_TH);
        funcArray[1] = new Func<PrepareItemResult>(this.Prepahe_TL);
        funcArray[2] = new Func<PrepareItemResult>(this.Prepahe_OHOL);
        funcArray[3] = new Func<PrepareItemResult>(this.Prepahe_OH);
        funcArray[4] = new Func<PrepareItemResult>(this.Prepahe_OL);
      }
      List<Tuple<PrepareItemResult, Func<PrepareItemResult>>> tupleList = new List<Tuple<PrepareItemResult, Func<PrepareItemResult>>>();
      foreach (Func<PrepareItemResult> func1 in funcArray)
      {
        PrepareItemResult prepareItemResult = func1();
        if (prepareItemResult != null)
        {
          Func<PrepareItemResult> func2 = func1;
          tupleList.Add(new Tuple<PrepareItemResult, Func<PrepareItemResult>>(prepareItemResult, func2));
        }
      }
      if (tupleList.Count <= 0)
        return (PrepareItemResult) null;
      Tuple<PrepareItemResult, Func<PrepareItemResult>> tuple = tupleList.Find((Predicate<Tuple<PrepareItemResult, Func<PrepareItemResult>>>) (x => x.Item1.BInPlayerInvent));
      if (tuple != null)
      {
        PrepareItemResult prepareItemResult = tuple.Item2();
        return tuple.Item1;
      }
      PrepareItemResult prepareItemResult1 = tupleList[0].Item2();
      return tupleList[0].Item1;
    }

    private PrepareItemResult Prepahe_TH()
    {
      if (this.TwoHandedHighLvlItems.Count < 1)
        return (PrepareItemResult) null;
      this._currentSetItems = new StashItem[1]
      {
        this.TwoHandedHighLvlItems[0]
      };
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowSetsCount(),
        LowSet = false,
        BInPlayerInvent = this._currentSetItems[0].BInPlayerInventory
      };
    }

    private PrepareItemResult Prepahe_OH()
    {
      if (this.OneHandedHighLvlItems.Count < 2)
        return (PrepareItemResult) null;
      this._currentSetItems = new StashItem[2]
      {
        this.OneHandedHighLvlItems[0],
        this.OneHandedHighLvlItems[1]
      };
      bool flag = this._currentSetItems[0].BInPlayerInventory || this._currentSetItems[1].BInPlayerInventory;
      return new PrepareItemResult()
      {
        AllowedReplacesCount = this.LowSetsCount(),
        LowSet = false,
        BInPlayerInvent = flag
      };
    }

    private PrepareItemResult Prepahe_OHOL()
    {
      if (this.OneHandedHighLvlItems.Count < 1 || this.OneHandedLowLvlItems.Count < 1)
        return (PrepareItemResult) null;
      this._currentSetItems = new StashItem[2]
      {
        this.OneHandedHighLvlItems[0],
        this.OneHandedLowLvlItems[0]
      };
      int count = this.TwoHandedLowLvlItems.Count;
      int num1 = this.OneHandedLowLvlItems.Count - 1;
      int num2 = num1 - this.OneHandedHighLvlItems.Count;
      int num3 = num2 > 0 ? this.OneHandedHighLvlItems.Count + num2 / 2 : num1;
      int num4 = count + num3;
      bool flag = this._currentSetItems[0].BInPlayerInventory || this._currentSetItems[1].BInPlayerInventory;
      return new PrepareItemResult()
      {
        AllowedReplacesCount = num4,
        LowSet = true,
        BInPlayerInvent = flag
      };
    }

    private PrepareItemResult Prepahe_TL()
    {
      if (this.TwoHandedLowLvlItems.Count < 1)
        return (PrepareItemResult) null;
      this._currentSetItems = new StashItem[1]
      {
        this.TwoHandedLowLvlItems[0]
      };
      int num = this.LowSetsCount() - 1;
      return new PrepareItemResult()
      {
        AllowedReplacesCount = num,
        LowSet = true,
        BInPlayerInvent = this._currentSetItems[0].BInPlayerInventory
      };
    }

    private PrepareItemResult Prepahe_OL()
    {
      if (this.OneHandedLowLvlItems.Count < 2)
        return (PrepareItemResult) null;
      this._currentSetItems = new StashItem[2]
      {
        this.OneHandedLowLvlItems[0],
        this.OneHandedLowLvlItems[1]
      };
      int num = this.LowSetsCount() - 2;
      bool flag = this._currentSetItems[0].BInPlayerInventory || this._currentSetItems[1].BInPlayerInventory;
      return new PrepareItemResult()
      {
        AllowedReplacesCount = num,
        LowSet = true,
        BInPlayerInvent = flag
      };
    }

    public override void DoLowItemReplace()
    {
      if (this.TwoHandedLowLvlItems.Count >= 1)
        this._currentSetItems = new StashItem[1]
        {
          this.TwoHandedLowLvlItems[0]
        };
      else if (this.OneHandedHighLvlItems.Count >= 1 && this.OneHandedLowLvlItems.Count >= 1)
      {
        this._currentSetItems = new StashItem[2]
        {
          this.OneHandedHighLvlItems[0],
          this.OneHandedLowLvlItems[0]
        };
      }
      else
      {
        if (this.OneHandedLowLvlItems.Count < 2)
          return;
        this._currentSetItems = new StashItem[2]
        {
          this.OneHandedLowLvlItems[0],
          this.OneHandedLowLvlItems[1]
        };
      }
    }

    public override StashItem[] GetPreparedItems() => this._currentSetItems;

    public override void RemovePreparedItems()
    {
      this.RemoveItem(this._currentSetItems[0]);
      if (this._currentSetItems.Length <= 1)
        return;
      this.RemoveItem(this._currentSetItems[1]);
    }

    private void RemoveItem(StashItem item)
    {
      if (item.LowLvl)
      {
        this.TwoHandedLowLvlItems.Remove(item);
        this.OneHandedLowLvlItems.Remove(item);
      }
      else
      {
        this.TwoHandedHighLvlItems.Remove(item);
        this.OneHandedHighLvlItems.Remove(item);
      }
    }

    public override int PlayerInventItemsCount() => this.TwoHandedHighLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory)) + this.TwoHandedLowLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory)) + this.OneHandedHighLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory)) + this.OneHandedLowLvlItems.Count<StashItem>((Func<StashItem, bool>) (x => x.BInPlayerInventory));
  }
}
