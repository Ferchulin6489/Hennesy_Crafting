// Decompiled with JetBrains decompiler
// Type: GameOffsets.ActiveAnimationOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ActiveAnimationOffsets
  {
    [FieldOffset(16)]
    public int AnimationId;
    [FieldOffset(72)]
    public float SlowAnimationSpeed;
    [FieldOffset(76)]
    public float NormalAnimationSpeed;
    [FieldOffset(56)]
    public long SlowAnimationStartStagePtr;
    [FieldOffset(64)]
    public long SlowAnimationEndStagePtr;
  }
}
