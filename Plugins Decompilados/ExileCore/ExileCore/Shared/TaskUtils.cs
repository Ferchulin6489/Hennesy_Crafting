// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.TaskUtils
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Threading;


#nullable enable
namespace ExileCore.Shared
{
  public static class TaskUtils
  {
    public static async 
    #nullable disable
    SyncTask<bool> CheckEveryFrame(Func<bool> condition, CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        if (condition())
          return true;
        await TaskUtils.NextFrame();
      }
      return false;
    }

    public static async SyncTask<bool> CheckEveryFrameWithThrow(
      Func<bool> condition,
      CancellationToken cancellationToken)
    {
      while (true)
      {
        cancellationToken.ThrowIfCancellationRequested();
        if (!condition())
          await TaskUtils.NextFrame();
        else
          break;
      }
      return true;
    }

    public static SyncTask<T> RunOrRestart<T>(
      ref SyncTask<T> oldTask,
      Func<SyncTask<T>> taskProvider)
    {
      oldTask?.GetAwaiter().PumpEvents();
      TaskUtils.ClearIfCompleted<T>(ref oldTask, taskProvider);
      return oldTask;
    }

    private static void ClearIfCompleted<T>(ref SyncTask<T> oldTask, Func<SyncTask<T>> taskProvider)
    {
      SyncTask<T> syncTask1 = oldTask;
      if ((syncTask1 != null ? (syncTask1.GetAwaiter().IsCompleted ? 1 : 0) : 1) == 0)
        return;
      if (oldTask != null)
      {
        SyncTask<T> syncTask2 = oldTask;
        oldTask = (SyncTask<T>) null;
        syncTask2.GetAwaiter().GetResult();
      }
      oldTask = taskProvider();
    }

    public static NextFrameTask NextFrame() => new NextFrameTask();
  }
}
