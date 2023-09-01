// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.WaitRandom
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections;

namespace ExileCore.Shared
{
  public class WaitRandom : YieldBase
  {
    private static readonly Random rnd = new Random();
    private readonly int _maxWait;
    private readonly int _minWait;

    public WaitRandom(int minWait, int maxWait)
    {
      this._minWait = minWait;
      this._maxWait = maxWait;
      this.Current = (object) this.GetEnumerator();
    }

    public string Timeout => this._minWait.ToString() + "-" + this._maxWait.ToString();

    public override sealed IEnumerator GetEnumerator()
    {
      long wait = YieldBase.sw.ElapsedMilliseconds + (long) WaitRandom.rnd.Next(this._minWait, this._maxWait);
      while (YieldBase.sw.ElapsedMilliseconds < wait)
        yield return (object) null;
      yield return YieldBase.RealWork;
    }
  }
}
