// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.WaitRender
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections;

namespace ExileCore.Shared
{
  public class WaitRender : YieldBase
  {
    public WaitRender(long howManyRenderCountWait = 1)
    {
      this.HowManyRenderCountWait = howManyRenderCountWait;
      this.Current = (object) this.GetEnumerator();
    }

    public static int FrameCount { get; private set; }

    public long HowManyRenderCountWait { get; }

    public static void Frame() => ++WaitRender.FrameCount;

    public override sealed IEnumerator GetEnumerator()
    {
      long wait = (long) WaitRender.FrameCount + this.HowManyRenderCountWait;
      while ((long) WaitRender.FrameCount < wait)
        yield return (object) null;
      yield return YieldBase.RealWork;
    }
  }
}
