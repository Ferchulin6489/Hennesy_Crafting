// Decompiled with JetBrains decompiler
// Type: GameOffsets.ActorAnimationStageOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ActorAnimationStageOffsets
  {
    [FieldOffset(4)]
    public float StageStart;
    [FieldOffset(8)]
    public int ActorAnimationListIndex;
    [FieldOffset(24)]
    public NativeUtf8Text StageName;
  }
}
