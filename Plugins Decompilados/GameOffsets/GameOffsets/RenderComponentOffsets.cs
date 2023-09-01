// Decompiled with JetBrains decompiler
// Type: GameOffsets.RenderComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct RenderComponentOffsets
  {
    [FieldOffset(160)]
    public Vector3 Pos;
    [FieldOffset(172)]
    public Vector3 Bounds;
    [FieldOffset(200)]
    public NativeStringU Name;
    [FieldOffset(232)]
    public Vector3 Rotation;
    [FieldOffset(260)]
    public float Height;
  }
}
