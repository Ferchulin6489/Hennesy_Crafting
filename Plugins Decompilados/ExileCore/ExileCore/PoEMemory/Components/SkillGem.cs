// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.SkillGem
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public class SkillGem : Component
  {
    private readonly CachedValue<SkillGemOffsets> _cachedValue;
    private readonly FrameCache<GemInformation> _cachedValue2;

    public SkillGem()
    {
      this._cachedValue = (CachedValue<SkillGemOffsets>) new FrameCache<SkillGemOffsets>((Func<SkillGemOffsets>) (() => this.M.Read<SkillGemOffsets>(this.Address)));
      this._cachedValue2 = new FrameCache<GemInformation>((Func<GemInformation>) (() => this.M.Read<GemInformation>(this._cachedValue.Value.AdvanceInformation)));
    }

    public int Level => (int) this._cachedValue.Value.Level;

    public uint TotalExpGained => this._cachedValue.Value.TotalExpGained;

    public uint ExperiencePrevLevel => this._cachedValue.Value.TotalExpGained;

    public uint ExperienceMaxLevel => this._cachedValue.Value.ExperienceMaxLevel;

    public uint ExperienceToNextLevel => this.ExperienceMaxLevel - this.ExperiencePrevLevel;

    public int MaxLevel => this._cachedValue2.Value.MaxLevel;

    public int SocketColor => this._cachedValue2.Value.SocketColor;

    public SkillGemQualityTypeE QualityType => (SkillGemQualityTypeE) this._cachedValue.Value.QualityType;

    public GrantedEffect GrantedEffect1 => this.TheGame.Files.GrantedEffects.GetByAddress(this._cachedValue2.Value.GrantedEffect1);

    public GrantedEffect GrantedEffect2 => this.TheGame.Files.GrantedEffects.GetByAddress(this._cachedValue2.Value.GrantedEffect2);

    public GrantedEffect GrantedEffect1HardMode => this.TheGame.Files.GrantedEffects.GetByAddress(this._cachedValue2.Value.GrantedEffect1HardMode);

    public GrantedEffect GrantedEffect2HardMode => this.TheGame.Files.GrantedEffects.GetByAddress(this._cachedValue2.Value.GrantedEffect2HardMode);

    public int RequiredLevel => this.GetRequiredLevel(this.Level);

    public int GetRequiredLevel(int gemLevel)
    {
      GrantedEffectPerLevel grantedEffectPerLevel = this.GrantedEffect1.PerLevelEffects.FirstOrDefault<GrantedEffectPerLevel>((Func<GrantedEffectPerLevel, bool>) (x => x.Level == gemLevel));
      return grantedEffectPerLevel == null ? 0 : grantedEffectPerLevel.RequiredLevel;
    }
  }
}
