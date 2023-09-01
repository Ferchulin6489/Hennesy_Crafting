// Decompiled with JetBrains decompiler
// Type: GameOffsets.IngameDataOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct IngameDataOffsets
  {
    [FieldOffset(136)]
    public long CurrentArea;
    [FieldOffset(168)]
    public byte CurrentAreaLevel;
    [FieldOffset(236)]
    public uint CurrentAreaHash;
    [FieldOffset(256)]
    public NativePtrArray MapStats;
    [FieldOffset(280)]
    public long LabDataPtr;
    [FieldOffset(656)]
    public long IngameStatePtr;
    [FieldOffset(848)]
    public long IngameStatePtr2;
    [FieldOffset(2000)]
    public long ServerData;
    [FieldOffset(2008)]
    public long LocalPlayer;
    [FieldOffset(2184)]
    public long EntityList;
    [FieldOffset(2192)]
    public long EntitiesCount;
    [FieldOffset(2600)]
    public TerrainData Terrain;
    [FieldOffset(2504)]
    public NativePtrArray TgtArray;
  }
}
