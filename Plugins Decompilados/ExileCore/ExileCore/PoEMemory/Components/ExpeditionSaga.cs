// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.ExpeditionSaga
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public class ExpeditionSaga : Component
  {
    private readonly CachedValue<ExpeditionSagaOffsets> _cachedValue;

    public ExpeditionSaga() => this._cachedValue = (CachedValue<ExpeditionSagaOffsets>) new FrameCache<ExpeditionSagaOffsets>((Func<ExpeditionSagaOffsets>) (() => this.M.Read<ExpeditionSagaOffsets>(this.Address)));

    public ExpeditionSagaOffsets SagaStruct => this._cachedValue.Value;

    public int AreaLevel => (int) this.SagaStruct.AreaLevel;

    public List<ExpeditionAreaData> Areas
    {
      get
      {
        long first = this.SagaStruct.AreasData.First;
        long last = this.SagaStruct.AreasData.Last;
        return first == 0L || (last - first) / 192L > 1024L ? new List<ExpeditionAreaData>() : this.M.ReadStructsArray<ExpeditionAreaData>(first, last, 192, (RemoteMemoryObject) this).ToList<ExpeditionAreaData>();
      }
    }
  }
}
