// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.InventoryList
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class InventoryList : RemoteMemoryObject
  {
    public static int InventoryCount => 53;

    public Inventory this[InventoryIndex inv]
    {
      get
      {
        int num = (int) inv;
        return num < 0 || num >= InventoryList.InventoryCount ? (Inventory) null : (Inventory) this.ReadObjectAt<PlayerInventory>(num * 8);
      }
    }

    public List<Inventory> DebugInventories => this._debug();

    private List<Inventory> _debug()
    {
      List<Inventory> inventoryList = new List<Inventory>();
      foreach (int num in Enum.GetValues<InventoryIndex>())
      {
        if (num < 0 || num >= InventoryList.InventoryCount)
          return (List<Inventory>) null;
        inventoryList.Add((Inventory) this.ReadObjectAt<PlayerInventory>(num * 8));
      }
      return inventoryList;
    }
  }
}
