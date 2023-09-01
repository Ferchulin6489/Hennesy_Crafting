// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HarvestInfrastructure
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Components
{
  public class HarvestInfrastructure : Component
  {
    public List<HarvestInfrastructureMod> CraftMods => this.M.ReadStructsArray<HarvestInfrastructureModUnmanaged>(this.M.Read<long>(this.Address + 32L), this.M.Read<long>(this.Address + 40L), sizeof (HarvestInfrastructureModUnmanaged)).Select<HarvestInfrastructureModUnmanaged, HarvestInfrastructureMod>((Func<HarvestInfrastructureModUnmanaged, HarvestInfrastructureMod>) (x => new HarvestInfrastructureMod(x, this.M))).ToList<HarvestInfrastructureMod>();
  }
}
