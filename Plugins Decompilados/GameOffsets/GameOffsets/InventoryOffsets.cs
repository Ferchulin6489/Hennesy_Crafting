// Decompiled with JetBrains decompiler
// Type: GameOffsets.InventoryOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct InventoryOffsets
  {
    public const int DefaultServerInventoryOffset = 1248;
    public const int ComplexStashFirstLevelServerInventoryOffset = 96;
    public const int DivinationServerInventoryOffset = 872;
    public const int BlightServerInventoryOffset = 960;
    [FieldOffset(584)]
    public long HoverItem;
    [FieldOffset(592)]
    public Vector2i FakePos;
    [FieldOffset(616)]
    public int XFake;
    [FieldOffset(620)]
    public int YFake;
    [FieldOffset(600)]
    public Vector2i RealPos;
    [FieldOffset(624)]
    public int XReal;
    [FieldOffset(628)]
    public int YReal;
    [FieldOffset(616)]
    public int CursorInInventory;
    [FieldOffset(936)]
    public long ItemCount;
    [FieldOffset(1280)]
    public Vector2i InventorySize;
    [FieldOffset(1148)]
    public int TotalBoxesInInventoryRow;
  }
}
