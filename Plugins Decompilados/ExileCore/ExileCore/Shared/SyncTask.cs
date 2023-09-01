// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.SyncTask
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.Shared
{
  public static class SyncTask
  {
    public static SyncTask<SyncTask<T>> WhenAny<T>(params SyncTask<T>[] tasks)
    {
      SyncTask<SyncTask<T>> aggregateTask = new SyncTask<SyncTask<T>>();
      SyncTask<T> result = ((IEnumerable<SyncTask<T>>) tasks).FirstOrDefault<SyncTask<T>>((Func<SyncTask<T>, bool>) (x => x.Awaiter.IsCompleted));
      if (result != null)
      {
        aggregateTask.GetAwaiter().ResultTask.SetResult(result);
        return aggregateTask;
      }
      List<IDisposable> disposeList = new List<IDisposable>();
      foreach (SyncTask<T> task in tasks)
        disposeList.Add(task.Awaiter.RedirectExecutionQueue((SyncAwaiter) aggregateTask.Awaiter));
      foreach (SyncTask<T> task in tasks)
      {
        SyncTask<T> childTask = task;
        childTask.Awaiter.OnCompleted((Action) (() =>
        {
          if (!aggregateTask.GetAwaiter().ResultTask.TrySetResult(childTask))
            return;
          foreach (IDisposable disposable in disposeList)
            disposable.Dispose();
        }));
      }
      return aggregateTask;
    }
  }
}
