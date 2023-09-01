// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HarvestInfrastructureMod
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ExileCore.PoEMemory.Components
{
  public class HarvestInfrastructureMod
  {
    internal HarvestInfrastructureMod(HarvestInfrastructureModUnmanaged data, IMemory m)
    {
      this.ModLevel = data.ModLevel;
      long addr = m.Read<long>(data.DatEntryPtr + 8L);
      this.ModName = Regex.Replace(m.ReadStringU(addr, 1000), "\\<(.*?)\\>|\\{|\\}", string.Empty);
    }

    public int ModLevel { get; }

    public string ModName { get; }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
      interpolatedStringHandler.AppendFormatted(this.ModName);
      interpolatedStringHandler.AppendLiteral(". (");
      interpolatedStringHandler.AppendFormatted<int>(this.ModLevel);
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
