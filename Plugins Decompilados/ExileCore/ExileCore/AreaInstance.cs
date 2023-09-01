// Decompiled with JetBrains decompiler
// Type: ExileCore.AreaInstance
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;
using System;
using System.Runtime.CompilerServices;

namespace ExileCore
{
  public sealed class AreaInstance
  {
    public static uint CurrentHash;

    public AreaInstance(AreaTemplate area, uint hash, int realLevel)
    {
      this.Area = area;
      this.Hash = hash;
      this.RealLevel = realLevel;
      this.Name = area.Name;
      this.Act = area.Act;
      this.IsTown = area.IsTown || this.Name.Equals("The Rogue Harbour");
      this.IsHideout = this.Name.Contains("Hideout") && !this.Name.Contains("Syndicate Hideout");
      this.HasWaypoint = area.HasWaypoint || this.IsHideout;
    }

    public int RealLevel { get; }

    public string Name { get; }

    public int Act { get; }

    public bool IsTown { get; }

    public bool IsHideout { get; }

    public bool HasWaypoint { get; }

    public uint Hash { get; }

    public AreaTemplate Area { get; }

    public string DisplayName => this.Name + " (" + (object) this.RealLevel + ")";

    public DateTime TimeEntered { get; } = DateTime.UtcNow;

    public Color AreaColorName { get; set; } = Color.Aqua;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<int>(this.RealLevel);
      interpolatedStringHandler.AppendLiteral(") #");
      interpolatedStringHandler.AppendFormatted<uint>(this.Hash);
      return interpolatedStringHandler.ToStringAndClear();
    }

    public static string GetTimeString(TimeSpan timeSpent)
    {
      int totalSeconds = (int) timeSpent.TotalSeconds;
      int num1 = totalSeconds % 60;
      int num2 = totalSeconds / 60;
      int num3 = num2 / 60;
      int num4 = num2 % 60;
      return string.Format(num3 > 0 ? "{0}:{1:00}:{2:00}" : "{1}:{2:00}", (object) num3, (object) num4, (object) num1);
    }
  }
}
