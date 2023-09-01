// Decompiled with JetBrains decompiler
// Type: GameOffsets.CameraOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Numerics;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct CameraOffsets
  {
    [FieldOffset(664)]
    public int Width;
    [FieldOffset(668)]
    public int Height;
    [FieldOffset(320)]
    public Matrix4x4 MatrixBytes;
    [FieldOffset(436)]
    public Vector3 Position;
    [FieldOffset(596)]
    public float ZFar;
  }
}
