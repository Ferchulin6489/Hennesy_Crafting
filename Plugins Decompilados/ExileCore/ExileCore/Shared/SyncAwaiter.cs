// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.SyncAwaiter
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


#nullable enable
namespace ExileCore.Shared
{
  public abstract class SyncAwaiter : INotifyCompletion
  {
    private readonly 
    #nullable disable
    Queue<Action> _methodExecutionQueue = new Queue<Action>();
    private readonly ConcurrentDictionary<SyncAwaiter, bool> _childAwaiters = new ConcurrentDictionary<SyncAwaiter, bool>();

    public abstract void OnCompleted(Action completion);

    internal IDisposable RedirectExecutionQueue(SyncAwaiter target)
    {
      target._childAwaiters.TryAdd(this, true);
      SyncAwaiter.Unsubscribe unsubscribe = new SyncAwaiter.Unsubscribe(target, this);
      this.OnCompleted(new Action(unsubscribe.Dispose));
      return (IDisposable) unsubscribe;
    }

    internal void EnqueueItem(Action item) => this._methodExecutionQueue.Enqueue(item);

    public bool PumpEvents()
    {
      bool flag1 = false;
      try
      {
        bool flag2;
        do
        {
          flag2 = false;
          Action result;
          while (this._methodExecutionQueue.TryDequeue(out result))
          {
            flag1 = flag2 = true;
            if (result != null)
              result();
          }
          foreach (SyncAwaiter key in (IEnumerable<SyncAwaiter>) this._childAwaiters.Keys)
            flag1 |= (flag2 |= key.PumpEvents());
        }
        while (flag2);
      }
      catch
      {
      }
      return flag1;
    }

    private record Unsubscribe(SyncAwaiter Parent, SyncAwaiter Child) : IDisposable
    {
      public void Dispose() => this.Parent._childAwaiters.TryRemove(this.Child, out bool _);
    }
  }
}
