// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.StashItem
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

namespace FullRareSetManager
{
  public class StashItem
  {
    public bool BIdentified;
    public int InventPosX;
    public int InventPosY;
    public string ItemClass;
    public string ItemName;
    public StashItemType ItemType;
    public bool LowLvl;
    public string StashName;

    public bool BInPlayerInventory { get; set; }
  }
}
