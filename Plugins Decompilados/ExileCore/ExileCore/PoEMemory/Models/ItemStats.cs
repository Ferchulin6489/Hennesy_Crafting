// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Models.ItemStats
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System;

namespace ExileCore.PoEMemory.Models
{
  public sealed class ItemStats
  {
    private static StatTranslator translate;
    private readonly Entity item;
    private readonly float[] stats;

    public ItemStats(Entity item)
    {
      this.item = item;
      if (ItemStats.translate == null)
        ItemStats.translate = new StatTranslator();
      this.stats = new float[Enum.GetValues<ItemStatEnum>().Length];
      this.ParseSockets();
      this.ParseExplicitMods();
      if (!item.HasComponent<Weapon>())
        return;
      this.ParseWeaponStats();
    }

    private void ParseWeaponStats()
    {
      Weapon component = this.item.GetComponent<Weapon>();
      float num1 = ((float) (component.DamageMin + component.DamageMax) / 2f + this.GetStat(ItemStatEnum.LocalPhysicalDamage)) * (float) (1.0 + ((double) this.GetStat(ItemStatEnum.LocalPhysicalDamagePercent) + (double) this.item.GetComponent<Quality>().ItemQuality) / 100.0);
      this.AddToMod(ItemStatEnum.AveragePhysicalDamage, num1);
      float num2 = (float) (1.0 / ((double) component.AttackTime / 1000.0)) * (float) (1.0 + (double) this.GetStat(ItemStatEnum.LocalAttackSpeed) / 100.0);
      this.AddToMod(ItemStatEnum.AttackPerSecond, num2);
      this.AddToMod(ItemStatEnum.WeaponCritChance, (float) component.CritChance / 100f * (float) (1.0 + (double) this.GetStat(ItemStatEnum.LocalCritChance) / 100.0));
      float num3 = this.GetStat(ItemStatEnum.LocalAddedColdDamage) + this.GetStat(ItemStatEnum.LocalAddedFireDamage) + this.GetStat(ItemStatEnum.LocalAddedLightningDamage);
      this.AddToMod(ItemStatEnum.AverageElementalDamage, num3);
      this.AddToMod(ItemStatEnum.DPS, (num1 + num3) * num2);
      this.AddToMod(ItemStatEnum.PhysicalDPS, num1 * num2);
    }

    private void ParseExplicitMods()
    {
      foreach (ItemMod itemMod in this.item.GetComponent<Mods>().ItemMods)
        ItemStats.translate.Translate(this, itemMod);
      this.AddToMod(ItemStatEnum.ElementalResistance, this.GetStat(ItemStatEnum.LightningResistance) + this.GetStat(ItemStatEnum.FireResistance) + this.GetStat(ItemStatEnum.ColdResistance));
      this.AddToMod(ItemStatEnum.TotalResistance, this.GetStat(ItemStatEnum.ElementalResistance) + this.GetStat(ItemStatEnum.TotalResistance));
    }

    private void ParseSockets()
    {
    }

    public void AddToMod(ItemStatEnum stat, float value) => this.stats[(int) stat] += value;

    public float GetStat(ItemStatEnum stat) => this.stats[(int) stat];
  }
}
