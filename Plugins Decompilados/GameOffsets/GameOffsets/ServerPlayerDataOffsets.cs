// Decompiled with JetBrains decompiler
// Type: GameOffsets.ServerPlayerDataOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ServerPlayerDataOffsets
  {
    [FieldOffset(400)]
    public NativePtrArray PassiveSkillIds;
    [FieldOffset(608)]
    public byte PlayerClass;
    [FieldOffset(612)]
    public int CharacterLevel;
    [FieldOffset(616)]
    public int PassiveRefundPointsLeft;
    [FieldOffset(620)]
    public int QuestPassiveSkillPoints;
    [FieldOffset(624)]
    public int FreePassiveSkillPointsLeft;
    [FieldOffset(620)]
    public int TotalAscendencyPoints;
    [FieldOffset(632)]
    public int SpentAscendencyPoints;
  }
}
