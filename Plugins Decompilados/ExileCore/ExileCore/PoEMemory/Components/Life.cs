// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Life
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class Life : Component
  {
    private readonly CachedValue<LifeComponentOffsets> _life;

    public Life() => this._life = (CachedValue<LifeComponentOffsets>) new FrameCache<LifeComponentOffsets>((Func<LifeComponentOffsets>) (() => this.Address != 0L ? this.M.Read<LifeComponentOffsets>(this.Address) : new LifeComponentOffsets()));

    public new long OwnerAddress => this.LifeComponentOffsetsStruct.Owner;

    private LifeComponentOffsets LifeComponentOffsetsStruct => this._life.Value;

    public VitalStruct Health => this._life.Value.Health;

    public VitalStruct Mana => this._life.Value.Mana;

    public VitalStruct EnergyShield => this._life.Value.EnergyShield;

    public int MaxHP => this.Address == 0L ? 1 : this.LifeComponentOffsetsStruct.Health.Max;

    public int CurHP => this.Address == 0L ? 0 : this.LifeComponentOffsetsStruct.Health.Current;

    public int ReservedFlatHP => this.LifeComponentOffsetsStruct.Health.ReservedFlat;

    public int ReservedPercentHP => this.LifeComponentOffsetsStruct.Health.ReservedFraction / 100;

    public int MaxMana => this.Address == 0L ? 1 : this.LifeComponentOffsetsStruct.Mana.Max;

    public int CurMana => this.Address == 0L ? 1 : this.LifeComponentOffsetsStruct.Mana.Current;

    public int ReservedFlatMana => this.LifeComponentOffsetsStruct.Mana.ReservedFlat;

    public int ReservedPercentMana => this.LifeComponentOffsetsStruct.Mana.ReservedFraction / 100;

    public int MaxES => this.LifeComponentOffsetsStruct.EnergyShield.Max;

    public int CurES => this.LifeComponentOffsetsStruct.EnergyShield.Current;

    public float HPPercentage => (float) this.CurHP / ((float) (this.MaxHP - this.ReservedFlatHP) - (float) Math.Round((double) this.ReservedPercentHP * 0.01 * (double) this.MaxHP));

    public float MPPercentage => (float) this.CurMana / ((float) (this.MaxMana - this.ReservedFlatMana) - (float) Math.Round((double) this.ReservedPercentMana * 0.01 * (double) this.MaxMana));

    public float ESPercentage => this.MaxES != 0 ? (float) this.CurES / (float) this.MaxES : 0.0f;
  }
}
