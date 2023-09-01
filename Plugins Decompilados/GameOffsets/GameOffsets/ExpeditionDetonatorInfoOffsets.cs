// Decompiled with JetBrains decompiler
// Type: GameOffsets.ExpeditionDetonatorInfoOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit)]
  public struct ExpeditionDetonatorInfoOffsets
  {
    public const int ExpeditionDetonatorInfoOffset = 632;
    [FieldOffset(432)]
    public long PlacementMarkerPtr;
    [FieldOffset(488)]
    public StdVector PlacedExplosives;
    [FieldOffset(680)]
    public Vector2i DetonatorGridPosition;
    [FieldOffset(696)]
    public Vector2i PlacementIndicatorGridPosition;
    [FieldOffset(720)]
    public byte TotalExplosiveCount;
  }
}
