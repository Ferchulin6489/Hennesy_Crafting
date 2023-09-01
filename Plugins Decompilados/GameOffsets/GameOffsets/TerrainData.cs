// Decompiled with JetBrains decompiler
// Type: GameOffsets.TerrainData
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct TerrainData
  {
    [FieldOffset(24)]
    public ushort NumCols;
    [FieldOffset(32)]
    public ushort NumRows;
    [FieldOffset(40)]
    public NativePtrArray TgtArray;
    [FieldOffset(208)]
    public NativePtrArray LayerMelee;
    [FieldOffset(232)]
    public NativePtrArray LayerRanged;
    [FieldOffset(256)]
    public int BytesPerRow;
    [FieldOffset(260)]
    public int TileHeightMultiplier;

    private int Cols => (int) this.NumCols;

    private int Rows => (int) this.NumRows;
  }
}
