// Decompiled with JetBrains decompiler
// Type: GameOffsets.Objects.GameStateOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets.Objects
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct GameStateOffsets
  {
    [FieldOffset(8)]
    public StdVector CurrentStatePtr;
    [FieldOffset(72)]
    public long State0;
    [FieldOffset(88)]
    public long State1;
    [FieldOffset(104)]
    public long State2;
    [FieldOffset(120)]
    public long State3;
    [FieldOffset(136)]
    public long State4;
    [FieldOffset(152)]
    public long State5;
    [FieldOffset(168)]
    public long State6;
    [FieldOffset(184)]
    public long State7;
    [FieldOffset(200)]
    public long State8;
    [FieldOffset(216)]
    public long State9;
    [FieldOffset(232)]
    public long State10;
    [FieldOffset(248)]
    public long State11;
  }
}
