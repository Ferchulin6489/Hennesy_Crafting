// Decompiled with JetBrains decompiler
// Type: GameOffsets.ServerInventoryOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ServerInventoryOffsets
  {
    [FieldOffset(320)]
    public byte InventType;
    [FieldOffset(324)]
    public byte InventSlot;
    [FieldOffset(320)]
    public byte IsRequested;
    [FieldOffset(332)]
    public int Columns;
    [FieldOffset(336)]
    public int Rows;
    [FieldOffset(368)]
    public long InventoryItemsPtr;
    [FieldOffset(392)]
    public long InventorySlotItemsPtr;
    [FieldOffset(400)]
    public long ItemCount;
    [FieldOffset(488)]
    public int ServerRequestCounter;
    [FieldOffset(504)]
    public long Hash;
  }
}
