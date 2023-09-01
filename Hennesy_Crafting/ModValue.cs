using ExileCore.PoEMemory;
using ExileCore.PoEMemory.FilesInMemory;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hennesy_Crafting
{
    public class ModValue
    {
        public ModValue(ItemMod mod, FilesContainer fs, int iLvl, BaseItemType baseItem)
        {
            string key1 = baseItem.ClassName.ToLower().Replace(' ', '_');
            this.Record = fs.Mods.records[mod.RawName];
            this.AffixType = this.Record.AffixType;
            this.AffixText = string.IsNullOrEmpty(this.Record.UserFriendlyName) ? this.Record.Key : this.Record.UserFriendlyName;
            this.IsCrafted = (int)this.Record.Domain == 10;
            this.StatValue = new int[4] { mod.Value1, mod.Value2, mod.Value3, mod.Value4 };
            this.Tier = -1;
            int val1 = 0;
            List<ModsDat.ModRecord> source;
            if (fs.Mods.recordsByTier.TryGetValue(Tuple.Create<string, ModType>(this.Record.Group, this.Record.AffixType), out source))
            {
                bool flag = false;
                this.TotalTiers = 0;
                char[] keyRcd = this.Record.Key.Where<char>(new Func<char, bool>(char.IsLetter)).ToArray<char>();
                List<ModsDat.ModRecord> list = ((IEnumerable<ModsDat.ModRecord>)source).Where<ModsDat.ModRecord>((Func<ModsDat.ModRecord, bool>)(x => x.Key.StartsWith(new string(keyRcd), StringComparison.Ordinal))).ToList<ModsDat.ModRecord>();
                foreach (ModsDat.ModRecord modRecord in list)
                {
                    if (((IEnumerable<char>)modRecord.Key.Where<char>(new Func<char, bool>(char.IsLetter)).ToArray<char>()).SequenceEqual<char>((IEnumerable<char>)keyRcd))
                    {
                        int num1;
                        if (!modRecord.TagChances.TryGetValue(key1, out num1))
                            num1 = -1;
                        int num2;
                        if (!modRecord.TagChances.TryGetValue("default", out num2))
                            num2 = 0;
                        int num3 = -1;
                        foreach (string tag in baseItem.Tags)
                        {
                            if (modRecord.TagChances.ContainsKey(tag))
                                num3 = modRecord.TagChances[tag];
                        }
                        int num4 = -1;
                        foreach (string key2 in baseItem.MoreTagsFromPath)
                        {
                            if (modRecord.TagChances.ContainsKey(key2))
                                num4 = modRecord.TagChances[key2];
                        }
                        switch (num1)
                        {
                            case -1:
                                switch (num3)
                                {
                                    case -1:
                                        switch (num4)
                                        {
                                            case -1:
                                                if (num2 > 0)
                                                {
                                                    this.TotalTiers = this.TotalTiers + 1;
                                                    if (((object)modRecord).Equals((object)this.Record))
                                                    {
                                                        this.Tier = this.TotalTiers;
                                                        flag = true;
                                                    }
                                                    if (!flag && modRecord.MinLevel <= iLvl)
                                                    {
                                                        ++val1;
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                continue;
                                            case 0:
                                                continue;
                                            default:
                                                this.TotalTiers = this.TotalTiers + 1;
                                                if (((object)modRecord).Equals((object)this.Record))
                                                {
                                                    this.Tier = this.TotalTiers;
                                                    flag = true;
                                                }
                                                if (!flag && modRecord.MinLevel <= iLvl)
                                                {
                                                    ++val1;
                                                    continue;
                                                }
                                                continue;
                                        }
                                    case 0:
                                        continue;
                                    default:
                                        this.TotalTiers = this.TotalTiers + 1;
                                        if (((object)modRecord).Equals((object)this.Record))
                                        {
                                            this.Tier = this.TotalTiers;
                                            flag = true;
                                        }
                                        if (!flag && modRecord.MinLevel <= iLvl)
                                        {
                                            ++val1;
                                            continue;
                                        }
                                        continue;
                                }
                            case 0:
                                continue;
                            default:
                                this.TotalTiers = this.TotalTiers + 1;
                                if (((object)modRecord).Equals((object)this.Record))
                                {
                                    this.Tier = this.TotalTiers;
                                    flag = true;
                                }
                                if (!flag && modRecord.MinLevel <= iLvl)
                                {
                                    ++val1;
                                    continue;
                                }
                                continue;
                        }
                    }
                }
                int result;
                if (this.Tier == -1 && !string.IsNullOrEmpty(this.Record.Tier) && int.TryParse(new string(this.Record.Tier.Where<char>(new Func<char, bool>(char.IsDigit)).ToArray<char>()), out result))
                {
                    this.Tier = result;
                    this.TotalTiers = list.Count;
                }
            }
            this.Color = ConvertHelper.ColorFromHsv(this.TotalTiers == 1 ? 180.0 : (double)(120 - Math.Min(val1, 3) * 40), this.TotalTiers == 1 ? 0.0 : 1.0, 1.0);
        }

        public ModType AffixType { get; }

        public bool IsCrafted { get; }

        public string AffixText { get; }

        public Color Color { get; }

        public ModsDat.ModRecord Record { get; }

        public int[] StatValue { get; }

        public int Tier { get; }

        public int TotalTiers { get; } = 1;

        public bool CouldHaveTiers() => this.TotalTiers > 0;
    }
}
