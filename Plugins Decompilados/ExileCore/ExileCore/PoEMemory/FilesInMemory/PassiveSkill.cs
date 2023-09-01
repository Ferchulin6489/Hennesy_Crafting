// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.PassiveSkill
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class PassiveSkill : RemoteMemoryObject
  {
    private string id;
    private string name;
    private int passiveId = -1;
    private List<(StatsDat.StatRecord, int)> stats;

    public int PassiveId => this.passiveId == -1 ? (this.passiveId = this.M.Read<int>(this.Address + 48L)) : this.passiveId;

    public string Id => this.id ?? (this.id = this.M.ReadStringU(this.M.Read<long>(this.Address), (int) byte.MaxValue));

    public string Name => this.name ?? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 52L), (int) byte.MaxValue));

    public string Icon => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L), (int) byte.MaxValue);

    public IEnumerable<(StatsDat.StatRecord, int)> Stats
    {
      get
      {
        if (this.stats == null)
        {
          this.stats = new List<(StatsDat.StatRecord, int)>();
          int size = this.M.Read<int>(this.Address + 16L);
          this.stats = ((IEnumerable<(long, long)>) this.M.ReadMem<(long, long)>(this.M.Read<long>(this.Address + 24L), size)).Select<(long, long), (StatsDat.StatRecord, int)>((Func<(long, long), int, (StatsDat.StatRecord, int)>) ((x, i) => (this.TheGame.Files.Stats.GetStatByAddress(x.Item1), this.ReadStatValue(i)))).ToList<(StatsDat.StatRecord, int)>();
        }
        return (IEnumerable<(StatsDat.StatRecord, int)>) this.stats;
      }
    }

    private int ReadStatValue(int index) => this.M.Read<int>(this.Address + 32L + (long) (index * 4));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 3);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(", Id: ");
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(", PassiveId: ");
      interpolatedStringHandler.AppendFormatted<int>(this.PassiveId);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
