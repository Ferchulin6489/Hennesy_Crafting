// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Helpers.PerformanceTimer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Serilog;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExileCore.Shared.Helpers
{
  public struct PerformanceTimer : IDisposable
  {
    private readonly string DebugText;
    private readonly Action<string, TimeSpan> FinishedCallback;
    private readonly int TriggerMs;
    private readonly bool Log;
    public static bool IgnoreTimer;
    public static ILogger Logger;
    private readonly Stopwatch sw;

    public PerformanceTimer(
      string debugText,
      int triggerMs = 0,
      Action<string, TimeSpan> callback = null,
      bool log = true)
    {
      this.FinishedCallback = callback;
      this.DebugText = debugText;
      this.TriggerMs = triggerMs;
      this.Log = log;
      this.sw = Stopwatch.StartNew();
    }

    public void Dispose() => this.StopAndPrint();

    public void StopAndPrint()
    {
      if (!this.sw.IsRunning)
        return;
      this.sw.Stop();
      if (this.sw.ElapsedMilliseconds < (long) this.TriggerMs || PerformanceTimer.IgnoreTimer)
        return;
      TimeSpan elapsed = this.sw.Elapsed;
      if (this.Log)
      {
        ILogger logger = PerformanceTimer.Logger;
        if (logger != null)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
          interpolatedStringHandler.AppendLiteral("PerfTimer =-> ");
          interpolatedStringHandler.AppendFormatted(this.DebugText);
          interpolatedStringHandler.AppendLiteral(" (");
          interpolatedStringHandler.AppendFormatted<double>(elapsed.TotalMilliseconds);
          interpolatedStringHandler.AppendLiteral(" ms) Thread #[");
          interpolatedStringHandler.AppendFormatted<int>(Thread.CurrentThread.ManagedThreadId);
          interpolatedStringHandler.AppendLiteral("]");
          logger.Information(interpolatedStringHandler.ToStringAndClear());
        }
      }
      Action<string, TimeSpan> finishedCallback = this.FinishedCallback;
      if (finishedCallback == null)
        return;
      finishedCallback(this.DebugText, elapsed);
    }
  }
}
