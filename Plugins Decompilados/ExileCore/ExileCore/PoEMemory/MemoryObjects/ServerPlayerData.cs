// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ServerPlayerData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using GameOffsets;
using GameOffsets.Native;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ServerPlayerData : StructuredRemoteMemoryObject<ServerPlayerDataOffsets>
  {
    public CharacterClass Class => (CharacterClass) ((int) this.Structure.PlayerClass & 15);

    public int Level => this.Structure.CharacterLevel;

    public int PassiveRefundPointsLeft => this.Structure.PassiveRefundPointsLeft;

    public int QuestPassiveSkillPoints => this.Structure.QuestPassiveSkillPoints;

    public int FreePassiveSkillPointsLeft => this.Structure.FreePassiveSkillPointsLeft;

    public int TotalAscendencyPoints => this.Structure.TotalAscendencyPoints;

    public int SpentAscendencyPoints => this.Structure.SpentAscendencyPoints;

    public NativePtrArray AllocatedPassivesIds => this.Structure.PassiveSkillIds;
  }
}
