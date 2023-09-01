// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.MonsterVariety
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class MonsterVariety : RemoteMemoryObject
  {
    private string _varietyId;

    public int Id { get; internal set; }

    public string VarietyId => this._varietyId ?? (this._varietyId = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public long MonsterTypePtr => this.M.Read<long>(this.Address + 16L);

    public int ObjectSize => this.M.Read<int>(this.Address + 28L);

    public int MinimumAttackDistance => this.M.Read<int>(this.Address + 32L);

    public int MaximumAttackDistance => this.M.Read<int>(this.Address + 36L);

    public string ACTFile => this.M.ReadStringU(this.M.Read<long>(this.Address + 40L));

    public string AOFile => this.M.ReadStringU(this.M.Read<long>(this.Address + 48L));

    public string BaseMonsterTypeIndex => this.M.ReadStringU(this.M.Read<long>(this.Address + 56L));

    public IEnumerable<ModsDat.ModRecord> Mods
    {
      get
      {
        int count = this.M.Read<int>(this.Address + 64L);
        return (IEnumerable<ModsDat.ModRecord>) this.M.ReadSecondPointerArray_Count(this.M.Read<long>(this.Address + 72L), count).Select<long, ModsDat.ModRecord>((Func<long, ModsDat.ModRecord>) (x => this.TheGame.Files.Mods.GetModByAddress(x))).ToList<ModsDat.ModRecord>();
      }
    }

    public int ModelSizeMultiplier => this.M.Read<int>(this.Address + 100L);

    public int ExperienceMultiplier => this.M.Read<int>(this.Address + 140L);

    public int CriticalStrikeChance => this.M.Read<int>(this.Address + 172L);

    public string AISFile => this.M.ReadStringU(this.M.Read<long>(this.Address + 196L));

    public string MonsterName => this.M.ReadStringU(this.M.Read<long>(this.Address + 260L));

    public int DamageMultiplier => this.M.Read<int>(this.Address + 252L);

    public int LifeMultiplier => this.M.Read<int>(this.Address + 256L);

    public int AttackSpeed => this.M.Read<int>(this.Address + 260L);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
      interpolatedStringHandler.AppendLiteral("Name: ");
      interpolatedStringHandler.AppendFormatted(this.MonsterName);
      interpolatedStringHandler.AppendLiteral(", VarietyId: ");
      interpolatedStringHandler.AppendFormatted(this.VarietyId);
      interpolatedStringHandler.AppendLiteral(", BaseMonsterTypeIndex: ");
      interpolatedStringHandler.AppendFormatted(this.BaseMonsterTypeIndex);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
