// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.NextFrameTask
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class NextFrameTask
  {
    public NextFrameTask.NextFrameAwaiter GetAwaiter() => this.Awaiter;

    private NextFrameTask.NextFrameAwaiter Awaiter { get; } = new NextFrameTask.NextFrameAwaiter();

    public class NextFrameAwaiter : INotifyCompletion
    {
      private static readonly ConcurrentQueue<Action> Continuations = new ConcurrentQueue<Action>();

      public bool IsCompleted => false;

      public void GetResult()
      {
      }

      public static void SetNextFrame()
      {
        Action result;
        while (NextFrameTask.NextFrameAwaiter.Continuations.TryDequeue(out result))
          result();
      }

      public void OnCompleted(Action completion) => NextFrameTask.NextFrameAwaiter.Continuations.Enqueue(completion);
    }
  }
}
