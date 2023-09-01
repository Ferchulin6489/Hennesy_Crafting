// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.AreaLoadingState
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class AreaLoadingState : GameState
  {
    private AreaLoadingStateOffsets Data => this.M.Read<AreaLoadingStateOffsets>(this.Address);

    public bool IsLoading => this.Data.IsLoading == 1L;

    public uint TotalLoadingScreenTimeMs => this.Data.TotalLoadingScreenTimeMs;

    public string AreaName => this.M.ReadStringU(this.Data.AreaName);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
      interpolatedStringHandler.AppendFormatted(this.AreaName);
      interpolatedStringHandler.AppendLiteral(", IsLoading: ");
      interpolatedStringHandler.AppendFormatted<bool>(this.IsLoading);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
