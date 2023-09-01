// Decompiled with JetBrains decompiler
// Type: GameOffsets.PathfindingComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct PathfindingComponentOffsets
  {
    public static int PathNodeStart = 212;
    [FieldOffset(1304)]
    public int DestinationNodes;
    [FieldOffset(1360)]
    public Vector2i WantMoveToPosition;
    [FieldOffset(1372)]
    public float StayTime;
  }
}
