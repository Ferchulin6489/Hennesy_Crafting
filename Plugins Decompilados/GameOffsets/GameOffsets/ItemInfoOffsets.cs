// Decompiled with JetBrains decompiler
// Type: GameOffsets.ItemInfoOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ItemInfoOffsets
  {
    [FieldOffset(16)]
    public byte ItemCellsSizeX;
    [FieldOffset(17)]
    public byte ItemCellsSizeY;
    [FieldOffset(48)]
    public NativeStringU Name;
    [FieldOffset(56)]
    public NativeStringU FlavourText;
    [FieldOffset(104)]
    public long BaseItemType;
    [FieldOffset(112)]
    public NativePtrArray Tags;
  }
}
