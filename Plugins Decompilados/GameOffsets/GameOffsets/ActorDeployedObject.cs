// Decompiled with JetBrains decompiler
// Type: GameOffsets.ActorDeployedObject
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ActorDeployedObject
  {
    [FieldOffset(0)]
    public ushort Unknown1;
    [FieldOffset(2)]
    public ushort SkillId;
    [FieldOffset(4)]
    public ushort ObjectId;
    [FieldOffset(6)]
    public ushort Unknown2;
  }
}
