// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.ItemInfoData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class ItemInfoData : RemoteMemoryObject
  {
    private readonly CachedValue<ItemInfoOffsets> _CachedValue;

    public ItemInfoData() => this._CachedValue = (CachedValue<ItemInfoOffsets>) new FrameCache<ItemInfoOffsets>((Func<ItemInfoOffsets>) (() => this.M.Read<ItemInfoOffsets>(this.Address)));

    public ItemInfoOffsets ItemInfoDataStruct => this._CachedValue.Value;

    public byte ItemCellsSizeX => this.ItemInfoDataStruct.ItemCellsSizeX;

    public byte ItemCellsSizeY => this.ItemInfoDataStruct.ItemCellsSizeY;

    public string Name => this.ItemInfoDataStruct.Name.ToString(this.M);

    public string FlavourText => this.ItemInfoDataStruct.FlavourText.ToString(this.M);

    public long BaseItemType => this.ItemInfoDataStruct.BaseItemType;

    public NativePtrArray Tags => this.ItemInfoDataStruct.Tags;
  }
}
