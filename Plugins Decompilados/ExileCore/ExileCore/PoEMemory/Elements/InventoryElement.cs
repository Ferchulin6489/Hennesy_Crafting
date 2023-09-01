// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.InventoryElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class InventoryElement : Element
  {
    private InventoryList _allInventories;

    private InventoryList AllInventories => this._allInventories = this._allInventories ?? this.GetObjectAt<InventoryList>(872);

    public Inventory this[InventoryIndex k] => this.AllInventories[k];

    public IList<Element> GetItemsInInventory()
    {
      IList<Element> children = this.GetElementSlot(InventoryIndex.PlayerInventory).Children;
      children.RemoveAt(0);
      return children;
    }

    public Element GetElementSlot(InventoryIndex inventoryIndex)
    {
      switch (inventoryIndex)
      {
        case InventoryIndex.None:
          throw new ArgumentOutOfRangeException(nameof (inventoryIndex));
        case InventoryIndex.Helm:
          return this.EquippedItems.GetChildAtIndex(12);
        case InventoryIndex.Amulet:
          return this.EquippedItems.GetChildAtIndex(13);
        case InventoryIndex.Chest:
          return this.EquippedItems.GetChildAtIndex(19);
        case InventoryIndex.LWeapon:
          return this.EquippedItems.GetChildAtIndex(16);
        case InventoryIndex.RWeapon:
          return this.EquippedItems.GetChildAtIndex(15);
        case InventoryIndex.LWeaponSwap:
          return this.EquippedItems.GetChildAtIndex(18);
        case InventoryIndex.RWeaponSwap:
          return this.EquippedItems.GetChildAtIndex(17);
        case InventoryIndex.LRing:
          return this.EquippedItems.GetChildAtIndex(20);
        case InventoryIndex.RRing:
          return this.EquippedItems.GetChildAtIndex(21);
        case InventoryIndex.Gloves:
          return this.EquippedItems.GetChildAtIndex(22);
        case InventoryIndex.Belt:
          return this.EquippedItems.GetChildAtIndex(23);
        case InventoryIndex.Boots:
          return this.EquippedItems.GetChildAtIndex(24);
        case InventoryIndex.PlayerInventory:
          return this.EquippedItems.GetChildAtIndex(26);
        case InventoryIndex.Flask:
          return this.EquippedItems.GetChildAtIndex(25);
        default:
          throw new ArgumentOutOfRangeException(nameof (inventoryIndex));
      }
    }

    private Element EquippedItems => this.GetChildAtIndex(3);
  }
}
