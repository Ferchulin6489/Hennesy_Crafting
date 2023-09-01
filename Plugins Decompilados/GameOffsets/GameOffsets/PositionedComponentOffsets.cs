// Decompiled with JetBrains decompiler
// Type: GameOffsets.PositionedComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Numerics;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct PositionedComponentOffsets
  {
    [FieldOffset(8)]
    public long OwnerAddress;
    [FieldOffset(480)]
    public byte Reaction;
    [FieldOffset(580)]
    public Vector2 PrevPosition;
    [FieldOffset(592)]
    public Vector2 RelativeCoord;
    [FieldOffset(656)]
    public Vector2i GridPosition;
    [FieldOffset(664)]
    public float Rotation;
    [FieldOffset(684)]
    public float Scale;
    [FieldOffset(485)]
    public int Size;
    [FieldOffset(692)]
    public Vector2 WorldPosition;
  }
}
