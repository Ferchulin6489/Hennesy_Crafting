// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.PropheciesDat
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class PropheciesDat : UniversalFileWrapper<ProphecyDat>
  {
    private int IndexCounter;
    private bool loaded;
    private readonly Dictionary<int, ProphecyDat> ProphecyIndexDictionary = new Dictionary<int, ProphecyDat>();

    public PropheciesDat(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    public IList<ProphecyDat> EntriesList => (IList<ProphecyDat>) base.EntriesList.ToList<ProphecyDat>();

    public ProphecyDat GetProphecyById(int index)
    {
      this.CheckCache();
      if (!this.loaded)
      {
        foreach (ProphecyDat entries in (IEnumerable<ProphecyDat>) this.EntriesList)
          this.EntryAdded(entries.Address, entries);
        this.loaded = true;
      }
      ProphecyDat prophecyById;
      this.ProphecyIndexDictionary.TryGetValue(index, out prophecyById);
      return prophecyById;
    }

    protected new void EntryAdded(long addr, ProphecyDat entry)
    {
      entry.Index = this.IndexCounter++;
      this.ProphecyIndexDictionary.Add(entry.ProphecyId, entry);
    }

    public new ProphecyDat GetByAddress(long address) => base.GetByAddress(address);
  }
}
