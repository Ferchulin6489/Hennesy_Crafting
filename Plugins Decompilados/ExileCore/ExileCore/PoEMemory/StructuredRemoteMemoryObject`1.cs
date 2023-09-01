// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.StructuredRemoteMemoryObject`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;

namespace ExileCore.PoEMemory
{
  public abstract class StructuredRemoteMemoryObject<T> : RemoteMemoryObject where T : unmanaged
  {
    private readonly CachedValue<T> _cachedStructValue;

    public StructuredRemoteMemoryObject() => this._cachedStructValue = (CachedValue<T>) this.CreateStructFrameCache<T>();

    public T Structure => this._cachedStructValue.Value;
  }
}
