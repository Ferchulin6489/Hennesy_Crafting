// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.LabyrinthSectionAreas
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class LabyrinthSectionAreas
  {
    private List<WorldArea> cruelAreas;
    private List<WorldArea> endgameAreas;
    private List<WorldArea> mercilesAreas;
    private List<WorldArea> normalAreas;

    public LabyrinthSectionAreas(WorldAreas filesWorldAreas)
    {
      this.FilesWorldAreas = filesWorldAreas;
      this.NormalAreasPtrs = (IList<long>) new List<long>();
      this.CruelAreasPtrs = (IList<long>) new List<long>();
      this.MercilesAreasPtrs = (IList<long>) new List<long>();
      this.EndgameAreasPtrs = (IList<long>) new List<long>();
    }

    public WorldAreas FilesWorldAreas { get; }

    public string Name { get; set; }

    public IList<long> NormalAreasPtrs { get; set; }

    public IList<long> CruelAreasPtrs { get; set; }

    public IList<long> MercilesAreasPtrs { get; set; }

    public IList<long> EndgameAreasPtrs { get; set; }

    public IList<WorldArea> NormalAreas
    {
      get
      {
        if (this.normalAreas == null)
          this.normalAreas = this.NormalAreasPtrs.Select<long, WorldArea>((Func<long, WorldArea>) (x => this.FilesWorldAreas.GetByAddress(x))).ToList<WorldArea>();
        return (IList<WorldArea>) this.normalAreas;
      }
    }

    public IList<WorldArea> CruelAreas
    {
      get
      {
        if (this.cruelAreas == null)
          this.cruelAreas = this.CruelAreasPtrs.Select<long, WorldArea>((Func<long, WorldArea>) (x => this.FilesWorldAreas.GetByAddress(x))).ToList<WorldArea>();
        return (IList<WorldArea>) this.cruelAreas;
      }
    }

    public IList<WorldArea> MercilesAreas
    {
      get
      {
        if (this.mercilesAreas == null)
          this.mercilesAreas = this.MercilesAreasPtrs.Select<long, WorldArea>((Func<long, WorldArea>) (x => this.FilesWorldAreas.GetByAddress(x))).ToList<WorldArea>();
        return (IList<WorldArea>) this.mercilesAreas;
      }
    }

    public IList<WorldArea> EndgameAreas
    {
      get
      {
        if (this.endgameAreas == null)
          this.endgameAreas = this.EndgameAreasPtrs.Select<long, WorldArea>((Func<long, WorldArea>) (x => this.FilesWorldAreas.GetByAddress(x))).ToList<WorldArea>();
        return (IList<WorldArea>) this.endgameAreas;
      }
    }
  }
}
