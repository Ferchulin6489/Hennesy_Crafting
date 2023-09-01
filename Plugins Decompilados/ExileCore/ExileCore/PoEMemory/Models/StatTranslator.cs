// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Models.StatTranslator
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Models
{
  public class StatTranslator
  {
    private readonly Dictionary<string, StatTranslator.AddStat> mods;

    public StatTranslator() => this.mods = new Dictionary<string, StatTranslator.AddStat>()
    {
      {
        "Dexterity",
        this.Single(ItemStatEnum.Dexterity)
      },
      {
        "Strength",
        this.Single(ItemStatEnum.Strength)
      },
      {
        "Intelligence",
        this.Single(ItemStatEnum.Intelligence)
      },
      {
        "IncreasedMana",
        this.Single(ItemStatEnum.AddedMana)
      },
      {
        "IncreasedLife",
        this.Single(ItemStatEnum.AddedHP)
      },
      {
        "IncreasedEnergyShield",
        this.Single(ItemStatEnum.AddedES)
      },
      {
        "IncreasedEnergyShieldPercent",
        this.Single(ItemStatEnum.AddedESPercent)
      },
      {
        "ColdResist",
        this.Single(ItemStatEnum.ColdResistance)
      },
      {
        "FireResist",
        this.Single(ItemStatEnum.FireResistance)
      },
      {
        "LightningResist",
        this.Single(ItemStatEnum.LightningResistance)
      },
      {
        "ChaosResist",
        this.Single(ItemStatEnum.ChaosResistance)
      },
      {
        "AllResistances",
        this.MultipleSame(ItemStatEnum.ColdResistance, ItemStatEnum.FireResistance, ItemStatEnum.LightningResistance)
      },
      {
        "CriticalStrikeChance",
        this.Single(ItemStatEnum.CritChance)
      },
      {
        "LocalCriticalMultiplier",
        this.Single(ItemStatEnum.CritMultiplier)
      },
      {
        "MovementVelocity",
        this.Single(ItemStatEnum.MovementSpeed)
      },
      {
        "ItemFoundRarityIncrease",
        this.Single(ItemStatEnum.Rarity)
      },
      {
        "ItemFoundQuantityIncrease",
        this.Single(ItemStatEnum.Quantity)
      },
      {
        "ManaLeech",
        this.Single(ItemStatEnum.ManaLeech)
      },
      {
        "LifeLeech",
        this.Single(ItemStatEnum.LifeLeech)
      },
      {
        "AddedLightningDamage",
        this.Average(ItemStatEnum.AddedLightningDamage)
      },
      {
        "AddedColdDamage",
        this.Average(ItemStatEnum.AddedColdDamage)
      },
      {
        "AddedFireDamage",
        this.Average(ItemStatEnum.AddedFireDamage)
      },
      {
        "AddedPhysicalDamage",
        this.Average(ItemStatEnum.AddedPhysicalDamage)
      },
      {
        "WeaponElementalDamage",
        this.Single(ItemStatEnum.WeaponElementalDamagePercent)
      },
      {
        "FireDamagePercent",
        this.Single(ItemStatEnum.FireDamagePercent)
      },
      {
        "ColdDamagePercent",
        this.Single(ItemStatEnum.ColdDamagePercent)
      },
      {
        "LightningDamagePercent",
        this.Single(ItemStatEnum.LightningDamagePercent)
      },
      {
        "SpellDamage",
        this.Single(ItemStatEnum.SpellDamage)
      },
      {
        "SpellDamageAndMana",
        this.Dual(ItemStatEnum.SpellDamage, ItemStatEnum.AddedMana)
      },
      {
        "SpellCriticalStrikeChance",
        this.Single(ItemStatEnum.SpellCriticalChance)
      },
      {
        "IncreasedCastSpeed",
        this.Single(ItemStatEnum.CastSpeed)
      },
      {
        "ProjectileSpeed",
        this.Single(ItemStatEnum.ProjectileSpeed)
      },
      {
        "LocalIncreaseSocketedMinionGemLevel",
        this.Single(ItemStatEnum.MinionSkillLevel)
      },
      {
        "LocalIncreaseSocketedFireGemLevel",
        this.Single(ItemStatEnum.FireSkillLevel)
      },
      {
        "LocalIncreaseSocketedColdGemLevel",
        this.Single(ItemStatEnum.ColdSkillLevel)
      },
      {
        "LocalIncreaseSocketedLightningGemLevel",
        this.Single(ItemStatEnum.LightningSkillLevel)
      },
      {
        "LocalAddedPhysicalDamage",
        this.Average(ItemStatEnum.LocalPhysicalDamage)
      },
      {
        "LocalIncreasedPhysicalDamagePercent",
        this.Single(ItemStatEnum.LocalPhysicalDamagePercent)
      },
      {
        "LocalAddedColdDamage",
        this.Average(ItemStatEnum.LocalAddedColdDamage)
      },
      {
        "LocalAddedFireDamage",
        this.Average(ItemStatEnum.LocalAddedFireDamage)
      },
      {
        "LocalAddedLightningDamage",
        this.Average(ItemStatEnum.LocalAddedLightningDamage)
      },
      {
        "LocalCriticalStrikeChance",
        this.Single(ItemStatEnum.LocalCritChance)
      },
      {
        "LocalIncreasedAttackSpeed",
        this.Single(ItemStatEnum.LocalAttackSpeed)
      },
      {
        "LocalIncreasedEnergyShield",
        this.Single(ItemStatEnum.LocalES)
      },
      {
        "LocalIncreasedEvasionRating",
        this.Single(ItemStatEnum.LocalEV)
      },
      {
        "LocalIncreasedPhysicalDamageReductionRating",
        this.Single(ItemStatEnum.LocalArmor)
      },
      {
        "LocalIncreasedEvasionRatingPercent",
        this.Single(ItemStatEnum.LocalEVPercent)
      },
      {
        "LocalIncreasedEnergyShieldPercent",
        this.Single(ItemStatEnum.LocalESPercent)
      },
      {
        "LocalIncreasedPhysicalDamageReductionRatingPercent",
        this.Single(ItemStatEnum.LocalArmorPercent)
      },
      {
        "LocalIncreasedArmourAndEvasion",
        this.MultipleSame(ItemStatEnum.LocalArmorPercent, ItemStatEnum.LocalEVPercent)
      },
      {
        "LocalIncreasedArmourAndEnergyShield",
        this.MultipleSame(ItemStatEnum.LocalArmorPercent, ItemStatEnum.LocalESPercent)
      },
      {
        "LocalIncreasedEvasionAndEnergyShield",
        this.MultipleSame(ItemStatEnum.LocalEVPercent, ItemStatEnum.LocalESPercent)
      }
    };

    public void Translate(ItemStats stats, ItemMod m)
    {
      if (!this.mods.ContainsKey(m.Name))
        return;
      this.mods[m.Name](stats, m);
    }

    private StatTranslator.AddStat Single(ItemStatEnum stat) => (StatTranslator.AddStat) ((x, m) => x.AddToMod(stat, (float) m.Value1));

    private StatTranslator.AddStat Average(ItemStatEnum stat) => (StatTranslator.AddStat) ((x, m) => x.AddToMod(stat, (float) (m.Value1 + m.Value2) / 2f));

    private StatTranslator.AddStat Dual(ItemStatEnum s1, ItemStatEnum s2) => (StatTranslator.AddStat) ((x, m) =>
    {
      x.AddToMod(s1, (float) m.Value1);
      x.AddToMod(s2, (float) m.Value2);
    });

    private StatTranslator.AddStat MultipleSame(params ItemStatEnum[] stats) => (StatTranslator.AddStat) ((x, m) =>
    {
      foreach (ItemStatEnum stat in stats)
        x.AddToMod(stat, (float) m.Value1);
    });

    private delegate void AddStat(ItemStats stats, ItemMod m);
  }
}
