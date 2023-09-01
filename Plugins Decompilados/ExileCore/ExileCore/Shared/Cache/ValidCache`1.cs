// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.ValidCache`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System;

namespace ExileCore.Shared.Cache
{
  public class ValidCache<T> : CachedValue<T>
  {
    private readonly Entity _entity;

    public ValidCache(Entity entity, Func<T> func)
      : base(func)
    {
      this._entity = entity;
    }

    protected override bool Update(bool force) => this._entity.IsValid | force;
  }
}
