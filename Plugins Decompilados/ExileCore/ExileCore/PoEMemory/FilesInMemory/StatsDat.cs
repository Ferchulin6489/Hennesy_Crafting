// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.StatsDat
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class StatsDat : FileInMemory
  {
    private readonly Dictionary<long, StatsDat.StatRecord> _recordsByAddress = new Dictionary<long, StatsDat.StatRecord>();

    public StatsDat(IMemory m, Func<long> address)
      : base(m, address)
    {
      this.loadItems();
    }

    public IDictionary<string, StatsDat.StatRecord> records { get; } = (IDictionary<string, StatsDat.StatRecord>) new Dictionary<string, StatsDat.StatRecord>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public IDictionary<int, StatsDat.StatRecord> recordsById { get; } = (IDictionary<int, StatsDat.StatRecord>) new Dictionary<int, StatsDat.StatRecord>();

    public StatsDat.StatRecord GetStatByAddress(long address) => this._recordsByAddress.GetValueOrDefault<long, StatsDat.StatRecord>(address);

    private void loadItems()
    {
      int num = 1;
      foreach (long recordAddress in this.RecordAddresses())
      {
        StatsDat.StatRecord statRecord = new StatsDat.StatRecord(this.M, recordAddress, num++);
        this.records[statRecord.Key] = statRecord;
        this.recordsById[statRecord.ID] = statRecord;
        this._recordsByAddress[statRecord.Address] = statRecord;
      }
    }

    public class StatRecord
    {
      public StatRecord(IMemory m, long addr, int iCounter)
      {
        this.Address = addr;
        IStaticCache<string> stringCache1 = RemoteMemoryObject.Cache.StringCache;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (StatsDat));
        interpolatedStringHandler.AppendFormatted<long>(addr);
        string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func1 = (Func<string>) (() => m.ReadStringU(m.Read<long>(addr), 512));
        this.Key = stringCache1.Read(stringAndClear1, func1);
        this.Flag0 = m.Read<byte>(addr + 8L) > (byte) 0;
        this.IsLocal = m.Read<byte>(addr + 9L) > (byte) 0;
        this.IsWeaponLocal = m.Read<byte>(addr + 10L) > (byte) 0;
        this.Type = this.Key.Contains("%") ? StatType.Percents : (StatType) m.Read<int>(addr + 11L);
        this.Flag3 = m.Read<byte>(addr + 15L) > (byte) 0;
        IStaticCache<string> stringCache2 = RemoteMemoryObject.Cache.StringCache;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (StatsDat));
        interpolatedStringHandler.AppendFormatted<long>(addr + 16L);
        string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
        Func<string> func2 = (Func<string>) (() => m.ReadStringU(m.Read<long>(addr + 16L), 512));
        this.UserFriendlyName = stringCache2.Read(stringAndClear2, func2);
        this.ID = iCounter;
      }

      public string Key { get; }

      public long Address { get; }

      public StatType Type { get; }

      public bool Flag0 { get; }

      public bool IsLocal { get; }

      public bool IsWeaponLocal { get; }

      public bool Flag3 { get; }

      public string UserFriendlyName { get; }

      public int ID { get; }

      public override string ToString() => !string.IsNullOrWhiteSpace(this.UserFriendlyName) ? this.UserFriendlyName : this.Key;

      public string ValueToString(int val)
      {
        switch (this.Type)
        {
          case StatType.Percents:
          case StatType.Precents5:
            return val.ToString("+#;-#") + "%";
          case StatType.Value2:
          case StatType.IntValue:
            return val.ToString("+#;-#");
          case StatType.Boolean:
            return val == 0 ? "False" : "True";
          default:
            return "";
        }
      }
    }
  }
}
