// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.SetParts.BaseSetPart
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

namespace FullRareSetManager.SetParts
{
  public abstract class BaseSetPart
  {
    public int ItemCellsSize = 1;
    public string PartName;
    public int StashTabItemsCount;

    protected BaseSetPart(string partName) => this.PartName = partName;

    public abstract int LowSetsCount();

    public abstract int HighSetsCount();

    public abstract int TotalSetsCount();

    public abstract void AddItem(StashItem item);

    public abstract string GetInfoString();

    public abstract int PlayerInventItemsCount();

    public abstract PrepareItemResult PrepareItemForSet(FullRareSetManagerSettings settings);

    public abstract void DoLowItemReplace();

    public abstract StashItem[] GetPreparedItems();

    public abstract void RemovePreparedItems();
  }
}
