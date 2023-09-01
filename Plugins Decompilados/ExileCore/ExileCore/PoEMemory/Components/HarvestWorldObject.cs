// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HarvestWorldObject
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public class HarvestWorldObject : Component
  {
    private readonly CachedValue<HarvestWorldObjectComponentOffsets> _cacheValue;

    public HarvestWorldObject() => this._cacheValue = (CachedValue<HarvestWorldObjectComponentOffsets>) this.CreateStructFrameCache<HarvestWorldObjectComponentOffsets>();

    public List<HarvestSeedSpawnDescriptor> Seeds => ((IEnumerable<HarvestSeedSpawnDescriptor>) this.M.ReadRMOStdVector<HarvestSeedSpawnDescriptor>(this._cacheValue.Value.Seeds, 24)).ToList<HarvestSeedSpawnDescriptor>();
  }
}
