// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.ItemVisualIdentities
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class ItemVisualIdentities : UniversalFileWrapper<ItemVisualIdentity>
  {
    private readonly Dictionary<string, List<ItemVisualIdentity>> _artPathDictionary = new Dictionary<string, List<ItemVisualIdentity>>();

    public ItemVisualIdentities(IMemory mem, Func<long> address)
      : base(mem, address)
    {
    }

    protected override void EntryAdded(long addr, ItemVisualIdentity entry)
    {
      List<ItemVisualIdentity> itemVisualIdentityList;
      if (!this._artPathDictionary.TryGetValue(entry.ArtPath, out itemVisualIdentityList))
        this._artPathDictionary[entry.ArtPath] = itemVisualIdentityList = new List<ItemVisualIdentity>();
      itemVisualIdentityList.Add(entry);
    }

    public List<ItemVisualIdentity> GetByArtPath(string artPath)
    {
      this.CheckCache();
      return this._artPathDictionary.GetValueOrDefault<string, List<ItemVisualIdentity>>(artPath) ?? new List<ItemVisualIdentity>();
    }
  }
}
