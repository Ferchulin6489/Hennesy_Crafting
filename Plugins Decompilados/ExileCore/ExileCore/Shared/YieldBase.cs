// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.YieldBase
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections;
using System.Diagnostics;

namespace ExileCore.Shared
{
  public abstract class YieldBase : IYieldBase, IEnumerable, IEnumerator
  {
    protected static readonly Stopwatch sw = Stopwatch.StartNew();

    public static object RealWork { get; } = new object();

    public bool MoveNext()
    {
      if (((IEnumerator) this.Current).MoveNext())
        return true;
      this.Current = (object) this.GetEnumerator();
      return false;
    }

    public void Reset()
    {
    }

    public object Current { get; protected set; }

    public abstract IEnumerator GetEnumerator();
  }
}
