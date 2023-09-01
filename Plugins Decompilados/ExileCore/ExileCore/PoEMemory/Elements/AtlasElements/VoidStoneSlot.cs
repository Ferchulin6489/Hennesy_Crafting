// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.AtlasElements.VoidStoneSlot
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Elements.AtlasElements
{
  public class VoidStoneSlot : Element
  {
    public bool isEmpty => this.GetChildAtIndex(1) == null;

    public NormalInventoryItem Voidstone => this[1].AsObject<NormalInventoryItem>();

    public bool hasSextantApplied => this.Voidstone.Item.HasComponent<Mods>();

    public ItemMod SextantMod => !this.hasSextantApplied ? (ItemMod) null : this.Voidstone.Item.GetComponent<Mods>().ItemMods[0];

    public int RemainingSextantCharges
    {
      get
      {
        ItemMod sextantMod = this.SextantMod;
        return sextantMod == null ? 0 : sextantMod.Values[0];
      }
    }
  }
}
