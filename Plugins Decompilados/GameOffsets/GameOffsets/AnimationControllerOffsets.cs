// Decompiled with JetBrains decompiler
// Type: GameOffsets.AnimationControllerOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct AnimationControllerOffsets
  {
    [FieldOffset(24)]
    public NativePtrArray ActiveAnimationsArrayPtr;
    [FieldOffset(376)]
    public long ActorAnimationArrayPtr;
    [FieldOffset(392)]
    public int AnimationInActorId;
    [FieldOffset(408)]
    public float AnimationProgress;
    [FieldOffset(412)]
    public int CurrentAnimationStage;
    [FieldOffset(416)]
    public float NextAnimationPoint;
    [FieldOffset(420)]
    public float AnimationSpeedMultiplier1;
    [FieldOffset(464)]
    public float AnimationSpeedMultiplier2;
    [FieldOffset(428)]
    public float MaxAnimationProgressOffset;
    [FieldOffset(432)]
    public float MaxAnimationProgress;
  }
}
