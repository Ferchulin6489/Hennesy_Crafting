// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.TagsDat
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class TagsDat : FileInMemory
  {
    public TagsDat(IMemory m, Func<long> address)
      : base(m, address)
    {
      this.loadItems();
    }

    public Dictionary<string, TagsDat.TagRecord> Records { get; } = new Dictionary<string, TagsDat.TagRecord>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    private void loadItems()
    {
      foreach (long recordAddress in this.RecordAddresses())
      {
        TagsDat.TagRecord tagRecord = new TagsDat.TagRecord(this.M, recordAddress);
        if (!this.Records.ContainsKey(tagRecord.Key))
          this.Records.Add(tagRecord.Key, tagRecord);
      }
    }

    public class TagRecord
    {
      public TagRecord(IMemory m, long addr)
      {
        IStaticCache<string> stringCache = RemoteMemoryObject.Cache.StringCache;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
        interpolatedStringHandler.AppendFormatted(nameof (TagsDat));
        interpolatedStringHandler.AppendFormatted<long>(addr);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        Func<string> func = (Func<string>) (() => m.ReadStringU(m.Read<long>(addr), (int) byte.MaxValue));
        this.Key = stringCache.Read(stringAndClear, func);
        this.Hash = m.Read<int>(addr + 8L);
      }

      public string Key { get; }

      public int Hash { get; }
    }
  }
}
