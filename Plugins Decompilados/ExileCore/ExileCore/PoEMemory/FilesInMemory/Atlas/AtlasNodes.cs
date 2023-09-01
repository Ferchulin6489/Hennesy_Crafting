// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Atlas.AtlasNodes
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory.Atlas
{
  public class AtlasNodes : UniversalFileWrapper<AtlasNode>
  {
    public AtlasNodes(IMemory mem, Func<long> address)
      : base(mem, address)
    {
    }

    public IList<AtlasNode> EntriesList
    {
      get
      {
        this.CheckCache();
        return (IList<AtlasNode>) this.CachedEntriesList.ToList<AtlasNode>();
      }
    }

    public new AtlasNode GetByAddress(long address) => base.GetByAddress(address);
  }
}
