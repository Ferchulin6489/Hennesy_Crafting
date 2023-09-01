﻿// Decompiled with JetBrains decompiler
// Type: GameOffsets.ActorSkillCooldownOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ActorSkillCooldownOffsets
  {
    [FieldOffset(8)]
    public int SkillSubId;
    [FieldOffset(16)]
    public StdVector Cooldowns;
    [FieldOffset(48)]
    public int MaxUses;
    [FieldOffset(60)]
    public ushort SkillId;
  }
}