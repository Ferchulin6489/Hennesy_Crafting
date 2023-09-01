// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Heist.HeistNpcRecord
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects.Heist
{
  public class HeistNpcRecord : RemoteMemoryObject
  {
    private long _JobCount => Math.Max(0L, Math.Min(10L, this.M.Read<long>(this.Address + 32L)));

    public List<HeistJobRecord> Jobs => this.GetJobs(this.M.Read<long>(this.Address + 40L));

    public string PortraitFile => this.M.ReadStringU(this.M.Read<long>(this.Address + 48L));

    private int _StatCount => Math.Clamp(this.M.Read<int>(this.Address + 56L), 0, 32);

    public List<StatsDat.StatRecord> Stats => this.GetStats(this.M.Read<long>(this.Address + 64L), this._StatCount);

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 108L));

    private List<StatsDat.StatRecord> GetStats(long start, int count)
    {
      List<StatsDat.StatRecord> stats = new List<StatsDat.StatRecord>();
      int num = 0;
      while (num < count)
      {
        stats.Add(this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(start, new int[1])));
        ++num;
        start += 16L;
      }
      return stats;
    }

    private List<HeistJobRecord> GetJobs(long source)
    {
      List<HeistJobRecord> jobs = new List<HeistJobRecord>();
      if ((source += 8L) == 0L)
        return jobs;
      int num = 0;
      while ((long) num < this._JobCount)
      {
        jobs.Add(this.TheGame.Files.HeistJobs.GetByAddress(this.M.Read<long>(source)));
        ++num;
        source += 16L;
      }
      return jobs;
    }

    public override string ToString() => this.Name;
  }
}
