// Decompiled with JetBrains decompiler
// Type: ExileCore.Time
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Diagnostics;

namespace ExileCore
{
  public class Time
  {
    private static Stopwatch Stopwatch { get; } = Stopwatch.StartNew();

    public static double TotalMilliseconds => Time.Stopwatch.Elapsed.TotalMilliseconds;

    public static long ElapsedMilliseconds => Time.Stopwatch.ElapsedMilliseconds;

    public static TimeSpan Elapsed => Time.Stopwatch.Elapsed;
  }
}
