// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.PassiveSkills
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class PassiveSkills : UniversalFileWrapper<PassiveSkill>
  {
    private List<PassiveSkill> _EntriesList;
    private bool loaded;

    public PassiveSkills(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    public Dictionary<int, PassiveSkill> PassiveSkillsDictionary { get; } = new Dictionary<int, PassiveSkill>();

    public IList<PassiveSkill> EntriesList => (IList<PassiveSkill>) this._EntriesList ?? (IList<PassiveSkill>) (this._EntriesList = base.EntriesList.ToList<PassiveSkill>());

    public PassiveSkill GetPassiveSkillByPassiveId(int index)
    {
      this.CheckCache();
      if (!this.loaded)
      {
        foreach (PassiveSkill entries in (IEnumerable<PassiveSkill>) this.EntriesList)
          this.EntryAdded(entries.Address, entries);
        this.loaded = true;
      }
      PassiveSkill skillByPassiveId;
      this.PassiveSkillsDictionary.TryGetValue(index, out skillByPassiveId);
      return skillByPassiveId;
    }

    public PassiveSkill GetPassiveSkillById(string id) => this.EntriesList.FirstOrDefault<PassiveSkill>((Func<PassiveSkill, bool>) (x => x.Id == id));

    protected new void EntryAdded(long addr, PassiveSkill entry) => this.PassiveSkillsDictionary.Add(entry.PassiveId, entry);

    public new PassiveSkill GetByAddress(long address) => base.GetByAddress(address);
  }
}
