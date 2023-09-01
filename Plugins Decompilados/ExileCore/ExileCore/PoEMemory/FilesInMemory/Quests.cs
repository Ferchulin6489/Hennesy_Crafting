// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Quests
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
  public class Quests : UniversalFileWrapper<Quest>
  {
    public Quests(IMemory game, Func<long> address)
      : base(game, address)
    {
    }

    public IList<Quest> EntriesList
    {
      get
      {
        this.CheckCache();
        return (IList<Quest>) this.CachedEntriesList.ToList<Quest>();
      }
    }

    public new Quest GetByAddress(long address) => base.GetByAddress(address);
  }
}
