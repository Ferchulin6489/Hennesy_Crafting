// Decompiled with JetBrains decompiler
// Type: GameOffsets.VitalStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct VitalStruct
  {
    [FieldOffset(40)]
    public float Regen;
    [FieldOffset(44)]
    public int Max;
    [FieldOffset(48)]
    public int Current;
    [FieldOffset(16)]
    public int ReservedFlat;
    [FieldOffset(20)]
    public int ReservedFraction;

    public int Reserved => (int) Math.Ceiling((double) this.ReservedFraction / 10000.0 * (double) this.Max) + this.ReservedFlat;

    public int Unreserved => this.Max - this.Reserved;
  }
}
