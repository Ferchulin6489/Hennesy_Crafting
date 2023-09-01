// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.ModsDat
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class ModsDat : FileInMemory
  {
    public ModsDat(IMemory m, Func<long> address, StatsDat sDat, TagsDat tagsDat)
      : base(m, address)
    {
      this.loadItems(sDat, tagsDat);
    }

    public IDictionary<string, ModsDat.ModRecord> records { get; } = (IDictionary<string, ModsDat.ModRecord>) new Dictionary<string, ModsDat.ModRecord>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public IDictionary<long, ModsDat.ModRecord> DictionaryRecords { get; } = (IDictionary<long, ModsDat.ModRecord>) new Dictionary<long, ModsDat.ModRecord>();

    public IDictionary<Tuple<string, ModType>, List<ModsDat.ModRecord>> recordsByTier { get; } = (IDictionary<Tuple<string, ModType>, List<ModsDat.ModRecord>>) new Dictionary<Tuple<string, ModType>, List<ModsDat.ModRecord>>();

    public ModsDat.ModRecord GetModByAddress(long address)
    {
      ModsDat.ModRecord modByAddress;
      this.DictionaryRecords.TryGetValue(address, out modByAddress);
      return modByAddress;
    }

    private void loadItems(StatsDat sDat, TagsDat tagsDat)
    {
      foreach (long recordAddress in this.RecordAddresses())
      {
        ModsDat.ModRecord modRecord;
        try
        {
          modRecord = new ModsDat.ModRecord(this.M, sDat, tagsDat, recordAddress);
        }
        catch (Exception ex)
        {
          Logger.Log.Warning(ex, "Error load ModRecord");
          continue;
        }
        if (!this.records.ContainsKey(modRecord.Key))
        {
          this.DictionaryRecords.Add(recordAddress, modRecord);
          this.records.Add(modRecord.Key, modRecord);
          if (modRecord.Domain != ModDomain.Monster)
          {
            Tuple<string, ModType> key = Tuple.Create<string, ModType>(modRecord.Group, modRecord.AffixType);
            List<ModsDat.ModRecord> modRecordList;
            if (!this.recordsByTier.TryGetValue(key, out modRecordList))
            {
              modRecordList = new List<ModsDat.ModRecord>();
              this.recordsByTier[key] = modRecordList;
            }
            modRecordList.Add(modRecord);
          }
        }
      }
      foreach (List<ModsDat.ModRecord> modRecordList in (IEnumerable<List<ModsDat.ModRecord>>) this.recordsByTier.Values)
        modRecordList.Sort(ModsDat.ModRecord.ByLevelComparer);
    }

    public class ModRecord
    {
      public const int NumberOfStats = 4;
      public static IComparer<ModsDat.ModRecord> ByLevelComparer = (IComparer<ModsDat.ModRecord>) new ModsDat.ModRecord.LevelComparer();

      public ModRecord(IMemory m, StatsDat sDat, TagsDat tagsDat, long addr)
      {
        this.Address = addr;
        ModsRecordOffsets ModsRecord = m.Read<ModsRecordOffsets>(addr);
        IStaticCache<string> stringCache1 = RemoteMemoryObject.Cache.StringCache;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
        interpolatedStringHandler.AppendFormatted<long>(ModsRecord.Key.buf);
        string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func1 = (Func<string>) (() => ModsRecord.Key.ToString(m));
        this.Key = stringCache1.Read(stringAndClear1, func1);
        this.Hash = ModsRecord.Hash;
        this.MinLevel = ModsRecord.MinLevel;
        long read = m.Read<long>(ModsRecord.TypeName);
        IStaticCache<string> stringCache2 = RemoteMemoryObject.Cache.StringCache;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
        interpolatedStringHandler.AppendFormatted<long>(read);
        string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func2 = (Func<string>) (() => m.ReadStringU(read, (int) byte.MaxValue));
        this.TypeName = stringCache2.Read(stringAndClear2, func2);
        long s1 = ModsRecord.StatNames1 == 0L ? 0L : m.Read<long>(ModsRecord.StatNames1);
        long s2 = ModsRecord.StatNames2 == 0L ? 0L : m.Read<long>(ModsRecord.StatNames2);
        long s3 = ModsRecord.StatNames3 == 0L ? 0L : m.Read<long>(ModsRecord.StatNames3);
        long s4 = ModsRecord.StatName4 == 0L ? 0L : m.Read<long>(ModsRecord.StatName4);
        StatsDat.StatRecord[] statRecordArray = new StatsDat.StatRecord[4];
        StatsDat.StatRecord statRecord1;
        if (ModsRecord.StatNames1 != 0L)
        {
          IDictionary<string, StatsDat.StatRecord> records = sDat.records;
          IStaticCache<string> stringCache3 = RemoteMemoryObject.Cache.StringCache;
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
          interpolatedStringHandler.AppendFormatted<long>(s1);
          string stringAndClear3 = interpolatedStringHandler.ToStringAndClear();
          Func<string> func3 = (Func<string>) (() => m.ReadStringU(s1));
          string key = stringCache3.Read(stringAndClear3, func3);
          statRecord1 = records[key];
        }
        else
          statRecord1 = (StatsDat.StatRecord) null;
        statRecordArray[0] = statRecord1;
        StatsDat.StatRecord statRecord2;
        if (ModsRecord.StatNames2 != 0L)
        {
          IDictionary<string, StatsDat.StatRecord> records = sDat.records;
          IStaticCache<string> stringCache4 = RemoteMemoryObject.Cache.StringCache;
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
          interpolatedStringHandler.AppendFormatted<long>(s2);
          string stringAndClear4 = interpolatedStringHandler.ToStringAndClear();
          Func<string> func4 = (Func<string>) (() => m.ReadStringU(s2));
          string key = stringCache4.Read(stringAndClear4, func4);
          statRecord2 = records[key];
        }
        else
          statRecord2 = (StatsDat.StatRecord) null;
        statRecordArray[1] = statRecord2;
        StatsDat.StatRecord statRecord3;
        if (ModsRecord.StatNames3 != 0L)
        {
          IDictionary<string, StatsDat.StatRecord> records = sDat.records;
          IStaticCache<string> stringCache5 = RemoteMemoryObject.Cache.StringCache;
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
          interpolatedStringHandler.AppendFormatted<long>(s3);
          string stringAndClear5 = interpolatedStringHandler.ToStringAndClear();
          Func<string> func5 = (Func<string>) (() => m.ReadStringU(s3));
          string key = stringCache5.Read(stringAndClear5, func5);
          statRecord3 = records[key];
        }
        else
          statRecord3 = (StatsDat.StatRecord) null;
        statRecordArray[2] = statRecord3;
        StatsDat.StatRecord statRecord4;
        if (ModsRecord.StatName4 != 0L)
        {
          IDictionary<string, StatsDat.StatRecord> records = sDat.records;
          IStaticCache<string> stringCache6 = RemoteMemoryObject.Cache.StringCache;
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
          interpolatedStringHandler.AppendFormatted<long>(s4);
          string stringAndClear6 = interpolatedStringHandler.ToStringAndClear();
          Func<string> func6 = (Func<string>) (() => m.ReadStringU(s4));
          string key = stringCache6.Read(stringAndClear6, func6);
          statRecord4 = records[key];
        }
        else
          statRecord4 = (StatsDat.StatRecord) null;
        statRecordArray[3] = statRecord4;
        this.StatNames = statRecordArray;
        this.Domain = (ModDomain) ModsRecord.Domain;
        IStaticCache<string> stringCache7 = RemoteMemoryObject.Cache.StringCache;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
        interpolatedStringHandler.AppendFormatted<long>(ModsRecord.UserFriendlyName);
        string stringAndClear7 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func7 = (Func<string>) (() => m.ReadStringU(ModsRecord.UserFriendlyName));
        this.UserFriendlyName = stringCache7.Read(stringAndClear7, func7);
        this.AffixType = (ModType) ModsRecord.AffixType;
        IStaticCache<string> stringCache8 = RemoteMemoryObject.Cache.StringCache;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
        interpolatedStringHandler.AppendFormatted<(long, long)>(ModsRecord.Group);
        string stringAndClear8 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func8 = (Func<string>) (() => string.Join(",", (IEnumerable<string>) ((IEnumerable<(long, long)>) m.ReadMem<(long, long)>(ModsRecord.Group.Ptr, (int) ModsRecord.Group.Count)).Select<(long, long), string>(closure_1 ?? (closure_1 = (Func<(long, long), string>) (x => m.ReadStringU(m.Read<long>(x.Ptr))))).OrderBy<string, string>((Func<string, string>) (x => x))));
        this.Group = stringCache8.Read(stringAndClear8, func8);
        this.Groups = ((IEnumerable<string>) this.Group.Split(",")).ToList<string>();
        this.StatRange = new IntRange[4]
        {
          new IntRange(ModsRecord.StatRange1, ModsRecord.StatRange2),
          new IntRange(ModsRecord.StatRange3, ModsRecord.StatRange4),
          new IntRange(ModsRecord.StatRange5, ModsRecord.StatRange6),
          new IntRange(ModsRecord.StatRange7, ModsRecord.StatRange8)
        };
        this.Tags = new TagsDat.TagRecord[ModsRecord.Tags];
        long ta = ModsRecord.ta;
        for (int index = 0; index < this.Tags.Length; ++index)
        {
          long addr1 = ta + (long) (16 * index);
          long l = m.Read<long>(addr1, new int[1]);
          IStaticCache<string> stringCache9 = RemoteMemoryObject.Cache.StringCache;
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
          interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
          interpolatedStringHandler.AppendFormatted<long>(l);
          string stringAndClear9 = interpolatedStringHandler.ToStringAndClear();
          Func<string> func9 = (Func<string>) (() => m.ReadStringU(l, (int) byte.MaxValue));
          string key = stringCache9.Read(stringAndClear9, func9);
          this.Tags[index] = tagsDat.Records[key];
        }
        this.TagChances = (IDictionary<string, int>) new Dictionary<string, int>();
        long tc = ModsRecord.tc;
        for (int index = 0; index < this.Tags.Length; ++index)
          this.TagChances[this.Tags[index].Key] = m.Read<int>(tc + (long) (4 * index));
        this.IsEssence = ModsRecord.IsEssence == (byte) 1;
        IStaticCache<string> stringCache10 = RemoteMemoryObject.Cache.StringCache;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (ModsDat));
        interpolatedStringHandler.AppendFormatted<long>(ModsRecord.Tier);
        string stringAndClear10 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func10 = (Func<string>) (() => m.ReadStringU(ModsRecord.Tier));
        this.Tier = stringCache10.Read(stringAndClear10, func10);
      }

      public long Address { get; }

      public string Key { get; }

      public ModType AffixType { get; }

      public ModDomain Domain { get; }

      public string Group { get; }

      public List<string> Groups { get; }

      public int MinLevel { get; }

      public StatsDat.StatRecord[] StatNames { get; }

      public IntRange[] StatRange { get; }

      public IDictionary<string, int> TagChances { get; }

      public TagsDat.TagRecord[] Tags { get; }

      public int Hash { get; }

      public string UserFriendlyName { get; }

      public bool IsEssence { get; }

      public string Tier { get; }

      public string TypeName { get; }

      public override string ToString()
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 3);
        interpolatedStringHandler.AppendLiteral("Name: ");
        interpolatedStringHandler.AppendFormatted(this.UserFriendlyName);
        interpolatedStringHandler.AppendLiteral(", Key: ");
        interpolatedStringHandler.AppendFormatted(this.Key);
        interpolatedStringHandler.AppendLiteral(", MinLevel: ");
        interpolatedStringHandler.AppendFormatted<int>(this.MinLevel);
        return interpolatedStringHandler.ToStringAndClear();
      }

      private class LevelComparer : IComparer<ModsDat.ModRecord>
      {
        public int Compare(ModsDat.ModRecord x, ModsDat.ModRecord y) => -x.MinLevel + y.MinLevel;
      }
    }
  }
}
