// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Map
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class Map : Component
  {
    private readonly Lazy<MapComponentBase> mapBase;
    private readonly Lazy<MapComponentInner> mapInner;
    private readonly CachedValue<WorldArea> _area;

    public Map()
    {
      this.mapBase = new Lazy<MapComponentBase>((Func<MapComponentBase>) (() => this.M.Read<MapComponentBase>(this.Address)));
      this.mapInner = new Lazy<MapComponentInner>((Func<MapComponentInner>) (() => this.M.Read<MapComponentInner>(this.mapBase.Value.Base)));
      this._area = (CachedValue<WorldArea>) new StaticValueCache<WorldArea>((Func<WorldArea>) (() => this.TheGame.Files.WorldAreas.GetByAddress(this.MapInformation.Area)));
    }

    public MapComponentInner MapInformation => this.mapInner.Value;

    public WorldArea Area => this._area.Value;

    public byte Tier => this.mapBase.Value.Tier;

    public InventoryTabMapSeries MapSeries => (InventoryTabMapSeries) this.MapInformation.MapSeries;
  }
}
