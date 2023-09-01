// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.InventoryHolder
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class InventoryHolder : RemoteMemoryObject
  {
    public const int StructSize = 32;

    public int Id => this.M.Read<int>(this.Address);

    public InventoryNameE TypeId => (InventoryNameE) this.Id;

    public ServerInventory Inventory => this.ReadObject<ServerInventory>(this.Address + 8L);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
      interpolatedStringHandler.AppendLiteral("InventoryType: ");
      interpolatedStringHandler.AppendFormatted<InventoryTypeE>(this.Inventory.InventType);
      interpolatedStringHandler.AppendLiteral(", InventorySlot: ");
      interpolatedStringHandler.AppendFormatted<InventorySlotE>(this.Inventory.InventSlot);
      interpolatedStringHandler.AppendLiteral(", Items.Count: ");
      interpolatedStringHandler.AppendFormatted<int>(this.Inventory.Items.Count);
      interpolatedStringHandler.AppendLiteral(" ItemCount: ");
      interpolatedStringHandler.AppendFormatted<long>(this.Inventory.ItemCount);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
