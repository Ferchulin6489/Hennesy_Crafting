// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.WaitFunction
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections;

namespace ExileCore.Shared
{
  [Obsolete("Use WaitFunctionTimed instead to prevent unwanted endless loops")]
  public class WaitFunction : YieldBase
  {
    private readonly Func<bool> fn;

    public WaitFunction(Func<bool> fn)
    {
      this.fn = fn;
      this.Current = (object) this.GetEnumerator();
    }

    public override sealed IEnumerator GetEnumerator()
    {
      while (!this.fn())
        yield return (object) null;
      yield return YieldBase.RealWork;
    }
  }
}
