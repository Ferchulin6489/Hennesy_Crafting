// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.Sanctum.SanctumFloorWindowDataSelector
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using GameOffsets;

namespace ExileCore.PoEMemory.Elements.Sanctum
{
  public class SanctumFloorWindowDataSelector : RemoteMemoryObject
  {
    private readonly CachedValue<SanctumFloorWindowDataOffsets> _cachedValue;

    public SanctumFloorWindowDataSelector() => this._cachedValue = (CachedValue<SanctumFloorWindowDataOffsets>) this.CreateStructFrameCache<SanctumFloorWindowDataOffsets>();

    public SanctumFloorData FloorData
    {
      get
      {
        long address = this.Address;
        SanctumFloorWindowDataOffsets windowDataOffsets = this._cachedValue.Value;
        long num = !windowDataOffsets.Flag1 ? 352L : (windowDataOffsets.Flag2 ? 480L : 408L);
        return this.GetObject<SanctumFloorData>(address + num);
      }
    }
  }
}
