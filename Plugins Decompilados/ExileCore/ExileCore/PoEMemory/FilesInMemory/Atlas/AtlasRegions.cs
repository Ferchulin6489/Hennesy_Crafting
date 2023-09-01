// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Atlas.AtlasRegions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.FilesInMemory.Atlas
{
  public class AtlasRegions : UniversalFileWrapper<AtlasRegion>
  {
    public int IndexCounter;

    public AtlasRegions(IMemory mem, Func<long> address)
      : base(mem, address)
    {
    }

    public Dictionary<int, AtlasRegion> RegionIndexDictionary { get; } = new Dictionary<int, AtlasRegion>();

    protected override void EntryAdded(long addr, AtlasRegion entry)
    {
      entry.Index = this.IndexCounter++;
      this.RegionIndexDictionary.Add(entry.Index, entry);
    }
  }
}
