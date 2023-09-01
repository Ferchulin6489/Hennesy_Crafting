// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Player
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Enums;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Components
{
  public class Player : Component
  {
    private const int LevelOffset = 428;
    private const int AttributeOffset = 412;

    public string PlayerName => NativeStringReader.ReadString(this.Address + 360L, this.M);

    public uint XP => this.Address == 0L ? 0U : this.M.Read<uint>(this.Address + 396L);

    public int Strength => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 412L);

    public int Dexterity => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 412L + 4L);

    public int Intelligence => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 412L + 8L);

    public int Level => this.Address == 0L ? 1 : (int) this.M.Read<byte>(this.Address + 428L);

    public int AllocatedLootId => this.Address == 0L ? 1 : (int) this.M.Read<byte>(this.Address + 404L);

    public int HideoutLevel => (int) this.M.Read<byte>(this.Address + 910L);

    public HideoutWrapper Hideout => this.ReadObject<HideoutWrapper>(this.Address + 488L);

    public PantheonGod PantheonMinor => (PantheonGod) this.M.Read<byte>(this.Address + 420L);

    public PantheonGod PantheonMajor => (PantheonGod) this.M.Read<byte>(this.Address + 421L);

    private IList<PassiveSkill> AllocatedPassivesM()
    {
      List<PassiveSkill> passiveSkillList = new List<PassiveSkill>();
      foreach (ushort passiveSkillId in (IEnumerable<ushort>) this.TheGame.IngameState.ServerData.PassiveSkillIds)
      {
        PassiveSkill skillByPassiveId = this.TheGame.Files.PassiveSkills.GetPassiveSkillByPassiveId((int) passiveSkillId);
        if (skillByPassiveId == null)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
          interpolatedStringHandler.AppendLiteral("Can't find passive with id: ");
          interpolatedStringHandler.AppendFormatted<ushort>(passiveSkillId);
          DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 10f, Color.Red);
        }
        else
          passiveSkillList.Add(skillByPassiveId);
      }
      return (IList<PassiveSkill>) passiveSkillList;
    }

    public bool IsTrialCompleted(string trialId) => this.TrialPassStates.Get((this.TheGame.Files.LabyrinthTrials.GetLabyrinthTrialByAreaId(trialId) ?? throw new ArgumentException("Trial with id '" + trialId + "' is not found. Use WorldArea.Id or LabyrinthTrials.LabyrinthTrialAreaIds[]")).Id - 1);

    public bool IsTrialCompleted(LabyrinthTrial trialWrapper)
    {
      if (trialWrapper == null)
        throw new ArgumentException("Argument trialWrapper should not be null");
      return this.TrialPassStates.Get(trialWrapper.Id - 1);
    }

    public bool IsTrialCompleted(WorldArea area)
    {
      if (area == null)
        throw new ArgumentException("Argument area should not be null");
      return this.TrialPassStates.Get((this.TheGame.Files.LabyrinthTrials.GetLabyrinthTrialByArea(area) ?? throw new ArgumentException("Can't find trial wrapper for area '" + area.Name + "' (seems not a trial area).")).Id - 1);
    }

    [HideInReflection]
    private BitArray TrialPassStates => new BitArray(this.M.ReadBytes(this.Address + 692L, 36));

    public IList<Player.TrialState> TrialStates
    {
      get
      {
        List<Player.TrialState> trialStates = new List<Player.TrialState>();
        BitArray trialPassStates = this.TrialPassStates;
        foreach (string labyrinthTrialAreaId in LabyrinthTrials.LabyrinthTrialAreaIds)
        {
          LabyrinthTrial labyrinthTrialByAreaId = this.TheGame.Files.LabyrinthTrials.GetLabyrinthTrialByAreaId(labyrinthTrialAreaId);
          trialStates.Add(new Player.TrialState()
          {
            TrialAreaId = labyrinthTrialAreaId,
            TrialArea = labyrinthTrialByAreaId,
            IsCompleted = trialPassStates.Get(labyrinthTrialByAreaId.Id - 1)
          });
        }
        return (IList<Player.TrialState>) trialStates;
      }
    }

    public class TrialState
    {
      public LabyrinthTrial TrialArea { get; internal set; }

      public string TrialAreaId { get; internal set; }

      public bool IsCompleted { get; internal set; }

      public string AreaAddr => this.TrialArea.Address.ToString("x");

      public override string ToString()
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
        interpolatedStringHandler.AppendLiteral("Completed: ");
        interpolatedStringHandler.AppendFormatted<bool>(this.IsCompleted);
        interpolatedStringHandler.AppendLiteral(", Trial ");
        interpolatedStringHandler.AppendFormatted(this.TrialArea.Area.Name);
        interpolatedStringHandler.AppendLiteral(", AreaId: ");
        interpolatedStringHandler.AppendFormatted<int>(this.TrialArea.Id);
        return interpolatedStringHandler.ToStringAndClear();
      }
    }
  }
}
