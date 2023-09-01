// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.IntRange
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class IntRange
  {
    public IntRange(int min, int max)
    {
      this.Min = min;
      this.Max = max;
    }

    public IntRange()
    {
      this.Min = int.MaxValue;
      this.Max = int.MinValue;
    }

    public int Min { get; private set; }

    public int Max { get; private set; }

    public void Include(int value)
    {
      if (value < this.Min)
        this.Min = value;
      if (value <= this.Max)
        return;
      this.Max = value;
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
      interpolatedStringHandler.AppendFormatted<int>(this.Min);
      interpolatedStringHandler.AppendLiteral(" - ");
      interpolatedStringHandler.AppendFormatted<int>(this.Max);
      return interpolatedStringHandler.ToStringAndClear();
    }

    public float GetPercentage(int val) => this.Min == this.Max ? 1f : (float) (val - this.Min) / (float) (this.Max - this.Min);

    public bool HasSpread() => this.Max != this.Min;
  }
}
