// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.UniversalFileWrapper`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class UniversalFileWrapper<RecordType> : FileInMemory where RecordType : RemoteMemoryObject, new()
  {
    public UniversalFileWrapper(IMemory mem, Func<long> address)
      : base(mem, address)
    {
    }

    public bool ExcludeZeroAddresses { get; set; }

    protected Dictionary<long, RecordType> EntriesAddressDictionary { get; set; } = new Dictionary<long, RecordType>();

    protected List<RecordType> CachedEntriesList { get; set; } = new List<RecordType>();

    public List<RecordType> EntriesList
    {
      get
      {
        this.CheckCache();
        return this.CachedEntriesList;
      }
    }

    public RecordType GetByAddress(long address)
    {
      this.CheckCache();
      RecordType byAddress;
      this.EntriesAddressDictionary.TryGetValue(address, out byAddress);
      return byAddress;
    }

    public void CheckCache()
    {
      if (this.EntriesAddressDictionary.Count != 0)
        return;
      foreach (long num in this.RecordAddresses().Where<long>((Func<long, bool>) (x => !this.ExcludeZeroAddresses || x != 0L)))
      {
        if (!this.EntriesAddressDictionary.ContainsKey(num))
        {
          RecordType entry = RemoteMemoryObject.pTheGame.GetObject<RecordType>(num);
          this.EntriesAddressDictionary.Add(num, entry);
          this.EntriesList.Add(entry);
          this.EntryAdded(num, entry);
        }
      }
    }

    protected virtual void EntryAdded(long addr, RecordType entry)
    {
    }
  }
}
