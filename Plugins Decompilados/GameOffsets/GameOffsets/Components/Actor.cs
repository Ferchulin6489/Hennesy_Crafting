// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.Actor
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct Actor
  {
    [FieldOffset(0)]
    public ComponentHeader Header;
    [FieldOffset(248)]
    public short ActionId;
    [FieldOffset(288)]
    public int AnimationId;
    [FieldOffset(1000)]
    public NativePtrArray ActorSkillsArray;
    [FieldOffset(1104)]
    public NativePtrArray DeployedObjectArray;
  }
}
