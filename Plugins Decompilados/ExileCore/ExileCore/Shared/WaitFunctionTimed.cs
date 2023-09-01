// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.WaitFunctionTimed
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using Serilog;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class WaitFunctionTimed : YieldBase
  {
    private readonly Func<bool> fn;

    public int Milliseconds { get; }

    public bool StopCode { get; }

    public string ErrorMessage { get; }

    public WaitFunctionTimed(Func<bool> fn, bool stopCode = false, int maxWait = 1000, string errorMessage = "")
    {
      this.fn = fn;
      this.Milliseconds = maxWait;
      this.StopCode = stopCode;
      this.ErrorMessage = errorMessage;
      this.Current = (object) this.GetEnumerator();
    }

    public override sealed IEnumerator GetEnumerator()
    {
      WaitFunctionTimed waitFunctionTimed = this;
      TimeSpan elapsed = YieldBase.sw.Elapsed;
      double wait = elapsed.TotalMilliseconds + (double) waitFunctionTimed.Milliseconds;
      while (!waitFunctionTimed.fn())
      {
        elapsed = YieldBase.sw.Elapsed;
        if (elapsed.TotalMilliseconds < wait)
          yield return (object) null;
        else
          break;
      }
      if (!waitFunctionTimed.fn() && waitFunctionTimed.StopCode)
      {
        if (waitFunctionTimed.ErrorMessage != "")
          DebugWindow.LogMsg(waitFunctionTimed.ErrorMessage);
        ILogger log = Logger.Log;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
        interpolatedStringHandler.AppendLiteral("Code Stopped in ");
        interpolatedStringHandler.AppendFormatted<WaitFunctionTimed>(waitFunctionTimed);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        log.Error(stringAndClear);
      }
      else
        yield return YieldBase.RealWork;
    }
  }
}
