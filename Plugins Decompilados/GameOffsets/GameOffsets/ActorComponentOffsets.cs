// Decompiled with JetBrains decompiler
// Type: GameOffsets.ActorComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ActorComponentOffsets
  {
    [FieldOffset(416)]
    public long AnimationControllerPtr;
    [FieldOffset(432)]
    public long ActionPtr;
    [FieldOffset(528)]
    public short ActionId;
    [FieldOffset(572)]
    public int AnimationId;
    [FieldOffset(1728)]
    public NativePtrArray ActorSkillsArray;
    [FieldOffset(1752)]
    public NativePtrArray ActorSkillsCooldownArray;
    [FieldOffset(1776)]
    public NativePtrArray ActorVaalSkills;
    [FieldOffset(1800)]
    public NativePtrArray DeployedObjectArray;
  }
}
