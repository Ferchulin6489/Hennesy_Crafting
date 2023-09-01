// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ActorSkill
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ActorSkill : RemoteMemoryObject
  {
    private readonly CachedValue<ActorSkillCooldown> _actorSkillCooldown;
    private readonly CachedValue<ActorSkillOffsets> _cache;
    private readonly CachedValue<Dictionary<GameStat, int>> _statsCache;
    private readonly CachedValue<ActorVaalSkill> _actorVaalSkill;

    public ActorSkill()
    {
      this._actorSkillCooldown = (CachedValue<ActorSkillCooldown>) new AreaCache<ActorSkillCooldown>((Func<ActorSkillCooldown>) (() => this.Actor.ActorSkillsCooldowns.FirstOrDefault<ActorSkillCooldown>((Func<ActorSkillCooldown, bool>) (x => (int) x.Id == (int) this.Id && (long) x.SkillSubId == this.EffectsPerLevel.SkillGemWrapper.ActiveSkillSubId))));
      this._actorVaalSkill = (CachedValue<ActorVaalSkill>) new AreaCache<ActorVaalSkill>((Func<ActorVaalSkill>) (() => this.Actor.ActorVaalSkills.FirstOrDefault<ActorVaalSkill>((Func<ActorVaalSkill, bool>) (x => x.VaalSkillInternalName == this.InternalName))));
      this._cache = (CachedValue<ActorSkillOffsets>) new FrameCache<ActorSkillOffsets>((Func<ActorSkillOffsets>) (() => this.M.Read<ActorSkillOffsets>(this.Address)));
      this._statsCache = (CachedValue<Dictionary<GameStat, int>>) new FrameCache<Dictionary<GameStat, int>>((Func<Dictionary<GameStat, int>>) (() => this.ReadStats(this.Struct.SubData.StatsPtr)));
    }

    private ActorSkillOffsets Struct => this._cache.Value;

    public ushort Id => this.Struct.SubData.Id;

    public GrantedEffectsPerLevel EffectsPerLevel => this.GetObject<GrantedEffectsPerLevel>(this.Struct.SubData.EffectsPerLevelPtr);

    public bool CanBeUsedWithWeapon => this.Struct.SubData.CanBeUsedWithWeapon > (byte) 0;

    public bool CanBeUsed => this.Struct.SubData.CannotBeUsed == (byte) 0 && !this.IsOnCooldown && this.HasEnoughSouls;

    public int Cost => this.GetStat(GameStat.ManaCost);

    public bool IsChanneling => this.Struct.CastType == (byte) 10;

    public int TotalUses => this.Struct.SubData.TotalUses;

    public float Cooldown => (float) ((double) this.Struct.SubData.Cooldown / 1000.0 / ((double) (100 + this.GetStat(GameStat.VirtualCooldownSpeedPct)) * 0.0099999997764825821));

    public int SoulsPerUse => this.Struct.SubData.SoulsPerUse;

    public int TotalVaalUses => this.Struct.SubData.TotalVaalUses;

    public bool IsOnSkillBar => this.SkillSlotIndex != -1;

    public int SkillSlotIndex => this.TheGame.IngameState.Data.ServerData.SkillBarIds.IndexOf(this.Id);

    public byte SkillUseStage => this.Struct.SkillUseStage;

    internal int SlotIdentifier => (int) this.Id >> 8 & (int) byte.MaxValue;

    public int SocketIndex => this.SlotIdentifier >> 2 & 15;

    public bool IsUserSkill => (this.SlotIdentifier & 128) > 0;

    public bool AllowedToCast => this.CanBeUsedWithWeapon && this.CanBeUsed;

    public bool IsUsingOrCharging => this.SkillUseStage >= (byte) 2;

    public bool IsUsing => this.SkillUseStage > (byte) 1;

    public bool PrepareForUsage => this.SkillUseStage == (byte) 1;

    public float Dps => (float) this.GetStat((GameStat) (677 + (this.IsUsing ? 4 : 0))) / 100f;

    public bool IsSpell => this.GetStat(GameStat.CastingSpell) == 1;

    public bool IsAttack => this.GetStat(GameStat.SkillIsAttack) == 1;

    public bool IsCry => this.InternalName.EndsWith("_cry");

    public bool IsBallistaTotem => this.InternalName.EndsWith("_ballista_totem");

    public bool IsInstant => this.GetStat(GameStat.SkillIsInstant) == 1;

    public bool IsMine => this.GetStat(GameStat.IsRemoteMine) == 1 || this.GetStat(GameStat.SkillIsMined) == 1;

    public bool IsTotem => this.GetStat(GameStat.IsTotem) == 1 || this.GetStat(GameStat.SkillIsTotemified) == 1;

    public bool IsTrap => this.GetStat(GameStat.IsTrap) == 1 || this.GetStat(GameStat.SkillIsTrapped) == 1;

    public bool IsVaalSkill => this.SoulsPerUse > 0;

    public Dictionary<GameStat, int> Stats => this._statsCache.Value;

    public Actor Actor { get; private set; }

    public string Name
    {
      get
      {
        ushort id = this.Id;
        GrantedEffectsPerLevel effectsPerLevel = this.EffectsPerLevel;
        if (effectsPerLevel != null && effectsPerLevel.Address != 0L)
        {
          SkillGemWrapper skillGemWrapper = effectsPerLevel.SkillGemWrapper;
          if (!string.IsNullOrEmpty(skillGemWrapper.Name))
            return skillGemWrapper.Name;
          return !string.IsNullOrEmpty(skillGemWrapper.ActiveSkill.InternalName) ? this.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture) : skillGemWrapper.ActiveSkill.InternalName;
        }
        switch (id)
        {
          case 614:
            return "Interaction";
          case 10505:
            return "Move";
          case 14297:
            return "WashedUp";
          default:
            return this.InternalName;
        }
      }
    }

    public TimeSpan CastTime
    {
      get
      {
        if (this.IsBallistaTotem)
          return TimeSpan.FromMilliseconds(350.0);
        if (this.IsTotem)
          return TimeSpan.FromMilliseconds(600.0);
        int attacksPerSecond = this.HundredTimesAttacksPerSecond;
        return TimeSpan.FromMilliseconds(this.IsInstant || attacksPerSecond == 0 ? 0.0 : (double) (int) Math.Ceiling(1000.0 / ((double) attacksPerSecond * 0.0099999997764825821)));
      }
    }

    public int HundredTimesAttacksPerSecond
    {
      get
      {
        if (this.IsInstant)
          return 0;
        int attacksPerSecond = !this.IsSpell ? (!this.IsAttack ? this.GetStat(GameStat.HundredTimesNonSpellCastsPerSecond) : this.GetStat(GameStat.HundredTimesAttacksPerSecond)) : this.GetStat(GameStat.HundredTimesCastsPerSecond);
        if (attacksPerSecond == 0 && this.IsCry)
          attacksPerSecond = 60;
        return attacksPerSecond;
      }
    }

    public ActorSkill SetActor(Actor actor)
    {
      this.Actor = actor;
      return this;
    }

    public bool IsOnCooldown
    {
      get
      {
        ActorSkillCooldown actorSkillCooldown = this._actorSkillCooldown.Value;
        return actorSkillCooldown != null && actorSkillCooldown.SkillCooldowns.Count >= actorSkillCooldown.MaxUses;
      }
    }

    public bool HasEnoughSouls
    {
      get
      {
        if (!this.IsVaalSkill)
          return true;
        ActorVaalSkill actorVaalSkill = this._actorVaalSkill.Value;
        return actorVaalSkill == null || actorVaalSkill.CurrVaalSouls >= actorVaalSkill.VaalMaxSouls;
      }
    }

    public int RemainingUses
    {
      get
      {
        ActorSkillCooldown actorSkillCooldown = this._actorSkillCooldown.Value;
        return actorSkillCooldown == null ? 0 : actorSkillCooldown.MaxUses - actorSkillCooldown.SkillCooldowns.Count;
      }
    }

    public string InternalName
    {
      get
      {
        GrantedEffectsPerLevel effectsPerLevel = this.EffectsPerLevel;
        if (effectsPerLevel != null)
          return effectsPerLevel.SkillGemWrapper.ActiveSkill.InternalName;
        switch (this.Id)
        {
          case 614:
            return "Interaction";
          case 10505:
            return "Move";
          case 14297:
            return "WashedUp";
          default:
            return this.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }

    private Dictionary<GameStat, int> ReadStats(long address) => ((IEnumerable<(GameStat, int)>) this.M.ReadStdVector<(GameStat, int)>(this.M.Read<SubStatsComponentOffsets>(address).Stats)).ToDictionary<(GameStat, int), GameStat, int>((Func<(GameStat, int), GameStat>) (x => x.Stat), (Func<(GameStat, int), int>) (x => x.Value));

    public int GetStat(GameStat stat)
    {
      int num;
      return this.Stats.TryGetValue(stat, out num) ? num : 0;
    }

    public List<DeployedObject> DeployedObjects => this.Actor.DeployedObjects.FindAll((Predicate<DeployedObject>) (x => (int) x.SkillKey == (int) this.Id));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 5);
      interpolatedStringHandler.AppendLiteral("IsUsing: ");
      interpolatedStringHandler.AppendFormatted<bool>(this.IsUsing);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(", Id: ");
      interpolatedStringHandler.AppendFormatted<ushort>(this.Id);
      interpolatedStringHandler.AppendLiteral(", InternalName: ");
      interpolatedStringHandler.AppendFormatted(this.InternalName);
      interpolatedStringHandler.AppendLiteral(", CanBeUsed: ");
      interpolatedStringHandler.AppendFormatted<bool>(this.CanBeUsed);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
