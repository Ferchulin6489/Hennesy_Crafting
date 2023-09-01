// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.SyncAwaiter`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Threading.Tasks;


#nullable enable
namespace ExileCore.Shared
{
  public class SyncAwaiter<T> : SyncAwaiter
  {
    public bool IsCompleted => this.ResultTask.Task.IsCompleted;

    public 
    #nullable disable
    T GetResult() => this.ResultTask.Task.GetAwaiter().GetResult();

    internal TaskCompletionSource<T> ResultTask { get; } = new TaskCompletionSource<T>();

    public override void OnCompleted(Action completion) => this.ResultTask.Task.ContinueWith((Action<Task<T>>) (_ => completion()), TaskContinuationOptions.ExecuteSynchronously);
  }
}
