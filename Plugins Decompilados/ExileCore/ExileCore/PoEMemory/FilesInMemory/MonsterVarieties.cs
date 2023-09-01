// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.MonsterVarieties
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
  public class MonsterVarieties : UniversalFileWrapper<MonsterVariety>
  {
    private readonly Dictionary<string, MonsterVariety> MonsterVarietyMetadataDictionary = new Dictionary<string, MonsterVariety>();

    public MonsterVarieties(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    public IList<MonsterVariety> EntriesList => (IList<MonsterVariety>) base.EntriesList.ToList<MonsterVariety>();

    public MonsterVariety TranslateFromMetadata(string path)
    {
      this.CheckCache();
      MonsterVariety monsterVariety;
      this.MonsterVarietyMetadataDictionary.TryGetValue(path, out monsterVariety);
      return monsterVariety;
    }

    protected override void EntryAdded(long addr, MonsterVariety entry) => this.MonsterVarietyMetadataDictionary.Add(entry.VarietyId, entry);
  }
}
