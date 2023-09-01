// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.UniqueItemDescriptions
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class UniqueItemDescriptions : UniversalFileWrapper<UniqueItemDescription>
  {
    private readonly Dictionary<ItemVisualIdentity, List<UniqueItemDescription>> _visualIdentityDictionary = new Dictionary<ItemVisualIdentity, List<UniqueItemDescription>>();

    public UniqueItemDescriptions(IMemory mem, Func<long> address)
      : base(mem, address)
    {
    }

    protected override void EntryAdded(long addr, UniqueItemDescription entry)
    {
      if (addr == 0L)
      {
        this.EntriesList.Remove(entry);
        this.EntriesAddressDictionary.Remove(0L);
      }
      else
      {
        if (entry.ItemVisualIdentity == null)
          return;
        List<UniqueItemDescription> uniqueItemDescriptionList;
        if (!this._visualIdentityDictionary.TryGetValue(entry.ItemVisualIdentity, out uniqueItemDescriptionList))
          this._visualIdentityDictionary[entry.ItemVisualIdentity] = uniqueItemDescriptionList = new List<UniqueItemDescription>();
        uniqueItemDescriptionList.Add(entry);
      }
    }

    public List<UniqueItemDescription> GetByVisualIdentity(ItemVisualIdentity itemVisualIdentity)
    {
      this.CheckCache();
      return this._visualIdentityDictionary.GetValueOrDefault<ItemVisualIdentity, List<UniqueItemDescription>>(itemVisualIdentity) ?? new List<UniqueItemDescription>();
    }
  }
}
