// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.SyncTaskMethodBuilder`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class SyncTaskMethodBuilder<T>
  {
    private IAsyncStateMachine _stateMachine;

    public static SyncTaskMethodBuilder<T> Create() => new SyncTaskMethodBuilder<T>();

    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
      this._stateMachine = (IAsyncStateMachine) stateMachine;
      this._stateMachine.SetStateMachine((IAsyncStateMachine) stateMachine);
      this.Task.Awaiter.EnqueueItem(new Action(this._stateMachine.MoveNext));
    }

    public void SetStateMachine(IAsyncStateMachine stateMachine) => this._stateMachine = stateMachine;

    public void SetException(Exception exception) => this.Task.Awaiter.ResultTask.SetException(exception);

    public void SetResult(T result) => this.Task.Awaiter.ResultTask.SetResult(result);

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
      ref TAwaiter awaiter,
      ref TStateMachine stateMachine)
      where TAwaiter : INotifyCompletion
      where TStateMachine : IAsyncStateMachine
    {
      this._stateMachine = (IAsyncStateMachine) stateMachine;
      this._stateMachine.SetStateMachine((IAsyncStateMachine) stateMachine);
      ref TAwaiter local = ref awaiter;
      if ((object) default (TAwaiter) == null)
      {
        TAwaiter awaiter1 = local;
        local = ref awaiter1;
      }
      Action continuation = (Action) (() => this.Task.Awaiter.EnqueueItem(new Action(this._stateMachine.MoveNext)));
      local.OnCompleted(continuation);
      if (!(awaiter is SyncAwaiter syncAwaiter))
        return;
      IDisposable disposable = syncAwaiter.RedirectExecutionQueue((SyncAwaiter) this.Task.Awaiter);
      syncAwaiter.OnCompleted(new Action(disposable.Dispose));
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
      ref TAwaiter awaiter,
      ref TStateMachine stateMachine)
      where TAwaiter : ICriticalNotifyCompletion
      where TStateMachine : IAsyncStateMachine
    {
      this.AwaitOnCompleted<TAwaiter, TStateMachine>(ref awaiter, ref stateMachine);
    }

    public SyncTask<T> Task { get; } = new SyncTask<T>();
  }
}
