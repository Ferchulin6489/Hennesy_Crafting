// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HarvestSeedSpawnDescriptor
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory.Harvest;

namespace ExileCore.PoEMemory.Components
{
  public class HarvestSeedSpawnDescriptor : RemoteMemoryObject
  {
    private HarvestSeed _seed;

    public HarvestSeed Seed => this._seed ?? (this._seed = this.TheGame.Files.HarvestSeeds.GetByAddress(this.M.Read<long>(this.Address)));

    public int Count => this.M.Read<int>(this.Address + 16L);
  }
}
