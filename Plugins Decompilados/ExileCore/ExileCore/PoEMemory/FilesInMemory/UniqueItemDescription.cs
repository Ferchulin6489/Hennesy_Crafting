// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.UniqueItemDescription
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using System;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class UniqueItemDescription : RemoteMemoryObject
  {
    private ItemVisualIdentity _itemVisualIdentity;
    private WordEntry _uniqueName;
    private CachedValue<UniqueItemDescription.DataStruct> _data;

    public UniqueItemDescription() => this._data = (CachedValue<UniqueItemDescription.DataStruct>) new StaticValueCache<UniqueItemDescription.DataStruct>((Func<UniqueItemDescription.DataStruct>) (() => this.M.Read<UniqueItemDescription.DataStruct>(this.Address)));

    public ItemVisualIdentity ItemVisualIdentity => this._itemVisualIdentity ?? (this._itemVisualIdentity = this.TheGame.Files.ItemVisualIdentities.GetByAddress(this._data.Value.VisualPtr));

    public WordEntry UniqueName => this._uniqueName ?? (this._uniqueName = this.TheGame.Files.Words.GetByAddress(this._data.Value.NamePtr));

    public override string ToString() => this.UniqueName?.Text ?? base.ToString();

    private record struct DataStruct(long NamePtr, long _, long VisualPtr);
  }
}
