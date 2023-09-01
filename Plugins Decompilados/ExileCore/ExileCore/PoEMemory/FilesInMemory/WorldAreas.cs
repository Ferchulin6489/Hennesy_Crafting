// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.WorldAreas
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
  public class WorldAreas : UniversalFileWrapper<WorldArea>
  {
    private int _indexCounter;

    public WorldAreas(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    public Dictionary<int, WorldArea> AreasIndexDictionary { get; } = new Dictionary<int, WorldArea>();

    public Dictionary<int, WorldArea> AreasWorldIdDictionary { get; } = new Dictionary<int, WorldArea>();

    public WorldArea GetAreaByAreaId(int index)
    {
      this.CheckCache();
      WorldArea areaByAreaId;
      this.AreasIndexDictionary.TryGetValue(index, out areaByAreaId);
      return areaByAreaId;
    }

    public WorldArea GetAreaByAreaId(string id)
    {
      this.CheckCache();
      return this.AreasIndexDictionary.First<KeyValuePair<int, WorldArea>>((Func<KeyValuePair<int, WorldArea>, bool>) (area => area.Value.Id == id)).Value;
    }

    public WorldArea GetAreaByWorldId(int id)
    {
      this.CheckCache();
      WorldArea areaByWorldId;
      this.AreasWorldIdDictionary.TryGetValue(id, out areaByWorldId);
      return areaByWorldId;
    }

    protected override void EntryAdded(long addr, WorldArea entry)
    {
      entry.Index = this._indexCounter++;
      this.AreasIndexDictionary.Add(entry.Index, entry);
      this.AreasWorldIdDictionary.Add(entry.WorldAreaId, entry);
    }
  }
}
