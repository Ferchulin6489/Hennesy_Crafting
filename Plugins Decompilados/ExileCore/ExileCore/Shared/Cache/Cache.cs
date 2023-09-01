// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Cache.Cache
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory;
using ExileCore.Shared.Interfaces;

namespace ExileCore.Shared.Cache
{
  public class Cache
  {
    public Cache() => this.CreateCache();

    public IStaticCache<RemoteMemoryObject> StaticCacheElements { get; private set; }

    public IStaticCache<RemoteMemoryObject> StaticCacheComponents { get; private set; }

    public IStaticCache<RemoteMemoryObject> StaticEntityCache { get; private set; }

    public IStaticCache<RemoteMemoryObject> StaticEntityListCache { get; private set; }

    public IStaticCache<RemoteMemoryObject> StaticServerEntityCache { get; private set; }

    public IStaticCache<string> StringCache { get; private set; }

    public void CreateCache()
    {
      this.StaticCacheElements = (IStaticCache<RemoteMemoryObject>) new StaticCache<RemoteMemoryObject>(300, 60, "Elements");
      this.StaticCacheComponents = (IStaticCache<RemoteMemoryObject>) new StaticCache<RemoteMemoryObject>(90, 29, "Components");
      this.StaticEntityCache = (IStaticCache<RemoteMemoryObject>) new StaticCache<RemoteMemoryObject>(60, name: "Entity");
      this.StaticEntityListCache = (IStaticCache<RemoteMemoryObject>) new StaticCache<RemoteMemoryObject>(60, name: "Entities parse");
      this.StaticServerEntityCache = (IStaticCache<RemoteMemoryObject>) new StaticCache<RemoteMemoryObject>(90, name: "Server entities parse");
      this.StringCache = (IStaticCache<string>) new StaticCache<string>(300);
    }

    public void TryClearCache()
    {
      this.StaticCacheElements.UpdateCache();
      this.StaticCacheComponents.UpdateCache();
      this.StaticEntityCache.UpdateCache();
      this.StaticEntityListCache.UpdateCache();
      this.StaticServerEntityCache.UpdateCache();
      this.StringCache.UpdateCache();
    }
  }
}
