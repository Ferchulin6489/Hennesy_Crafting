﻿// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.MapSubInventoryInfo
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class MapSubInventoryInfo
  {
    public int Tier { get; set; }

    public int Count { get; set; }

    public string MapName { get; set; }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 3);
      interpolatedStringHandler.AppendLiteral("Tier:");
      interpolatedStringHandler.AppendFormatted<int>(this.Tier);
      interpolatedStringHandler.AppendLiteral(" Count:");
      interpolatedStringHandler.AppendFormatted<int>(this.Count);
      interpolatedStringHandler.AppendLiteral(" MapName:");
      interpolatedStringHandler.AppendFormatted(this.MapName);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
