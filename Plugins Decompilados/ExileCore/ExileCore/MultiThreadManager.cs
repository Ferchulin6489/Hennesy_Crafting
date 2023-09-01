// Decompiled with JetBrains decompiler
// Type: ExileCore.MultiThreadManager
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExileCore
{
  public class MultiThreadManager
  {
    private const long CriticalWorkTimeMs = 750;
    private readonly object locker = new object();
    private int _lock;
    private object _objectInitWork;
    private readonly List<ThreadUnit> BrokenThreads = new List<ThreadUnit>();
    private readonly ConcurrentQueue<ThreadUnit> FreeThreads = new ConcurrentQueue<ThreadUnit>();
    private readonly ConcurrentQueue<Job> Jobs = new ConcurrentQueue<Job>();
    private readonly Queue<Job> processJobs = new Queue<Job>();
    private volatile bool ProcessWorking;
    private SpinWait spinWait;
    private ThreadUnit[] threads;

    public MultiThreadManager(int countThreads)
    {
      this.spinWait = new SpinWait();
      this.ChangeNumberThreads(countThreads);
    }

    public int FailedThreadsCount { get; private set; }

    public int ThreadsCount { get; private set; }

    public void ChangeNumberThreads(int countThreads)
    {
      lock (this.locker)
      {
        if (countThreads == this.ThreadsCount)
          return;
        this.ThreadsCount = countThreads;
        if (this.threads != null)
        {
          foreach (ThreadUnit thread in this.threads)
            thread?.Abort();
          while (!this.FreeThreads.IsEmpty)
            this.FreeThreads.TryDequeue(out ThreadUnit _);
        }
        if (countThreads > 0)
        {
          this.threads = new ThreadUnit[this.ThreadsCount];
          for (int number = 0; number < this.ThreadsCount; ++number)
          {
            ThreadUnit[] threads = this.threads;
            int index = number;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
            interpolatedStringHandler.AppendLiteral("Thread #");
            interpolatedStringHandler.AppendFormatted<int>(number);
            ThreadUnit threadUnit = new ThreadUnit(interpolatedStringHandler.ToStringAndClear(), number);
            threads[index] = threadUnit;
            this.FreeThreads.Enqueue(this.threads[number]);
          }
        }
        else
          this.threads = (ThreadUnit[]) null;
      }
    }

    public Job AddJob(Job job)
    {
      job.IsStarted = true;
      bool flag = false;
      if (!this.FreeThreads.IsEmpty)
      {
        ThreadUnit result;
        this.FreeThreads.TryDequeue(out result);
        if (result != null)
        {
          flag = result.AddJob(job);
          if (result.Free)
            this.FreeThreads.Enqueue(result);
        }
      }
      if (!flag)
        this.Jobs.Enqueue(job);
      return job;
    }

    public Job AddJob(Action action, string name) => this.AddJob(new Job(name, action));

    public void Process(object o)
    {
      if (this.threads == null || Interlocked.CompareExchange(ref this._lock, 1, 0) == 1)
        return;
      DefaultInterpolatedStringHandler interpolatedStringHandler;
      if (this.ProcessWorking)
      {
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
        interpolatedStringHandler.AppendLiteral("WTF ");
        interpolatedStringHandler.AppendFormatted<Type>(this._objectInitWork.GetType());
        DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear());
      }
      this._objectInitWork = o;
      this.ProcessWorking = true;
      this.spinWait.Reset();
      Job result1;
      while (this.Jobs.TryDequeue(out result1))
        this.processJobs.Enqueue(result1);
      if (this.ThreadsCount > 1)
      {
        while (this.processJobs.Count > 0)
        {
          if (!this.FreeThreads.IsEmpty)
          {
            ThreadUnit result2;
            this.FreeThreads.TryDequeue(out result2);
            Job job = this.processJobs.Dequeue();
            if (!result2.AddJob(job))
              this.processJobs.Enqueue(job);
            else if (result2.Free)
              this.FreeThreads.Enqueue(result2);
          }
          else
          {
            this.spinWait.SpinOnce();
            bool flag = true;
            for (int index = 0; index < this.threads.Length; ++index)
            {
              ThreadUnit thread = this.threads[index];
              if (thread.Free)
              {
                flag = false;
                this.FreeThreads.Enqueue(thread);
              }
            }
            if (flag)
            {
              for (int index = 0; index < this.threads.Length; ++index)
              {
                ThreadUnit thread = this.threads[index];
                long workingTime = thread.WorkingTime;
                if (workingTime > 750L)
                {
                  interpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 9);
                  interpolatedStringHandler.AppendLiteral("Repair thread #");
                  interpolatedStringHandler.AppendFormatted<int>(thread.Number);
                  interpolatedStringHandler.AppendLiteral(" with Job1: ");
                  interpolatedStringHandler.AppendFormatted(thread.Job.Name);
                  interpolatedStringHandler.AppendLiteral(" (C: ");
                  interpolatedStringHandler.AppendFormatted<bool>(thread.Job.IsCompleted);
                  interpolatedStringHandler.AppendLiteral(" F: ");
                  interpolatedStringHandler.AppendFormatted<bool>(thread.Job.IsFailed);
                  interpolatedStringHandler.AppendLiteral(") && Job2:");
                  interpolatedStringHandler.AppendFormatted(thread.SecondJob.Name);
                  interpolatedStringHandler.AppendLiteral(" (C: ");
                  interpolatedStringHandler.AppendFormatted<bool>(thread.SecondJob.IsCompleted);
                  interpolatedStringHandler.AppendLiteral(" F: ");
                  interpolatedStringHandler.AppendFormatted<bool>(thread.SecondJob.IsFailed);
                  interpolatedStringHandler.AppendLiteral(") Time: ");
                  interpolatedStringHandler.AppendFormatted<long>(workingTime);
                  interpolatedStringHandler.AppendLiteral(" > ");
                  interpolatedStringHandler.AppendFormatted<bool>(workingTime >= 750L);
                  DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 5f);
                  thread.Abort();
                  this.BrokenThreads.Add(thread);
                  interpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
                  interpolatedStringHandler.AppendLiteral("Repair critical time ");
                  interpolatedStringHandler.AppendFormatted<int>(thread.Number);
                  ThreadUnit threadUnit = new ThreadUnit(interpolatedStringHandler.ToStringAndClear(), thread.Number);
                  this.threads[thread.Number] = threadUnit;
                  this.FreeThreads.Enqueue(threadUnit);
                  Thread.Sleep(5);
                  ++this.FailedThreadsCount;
                  break;
                }
              }
            }
          }
        }
      }
      else
      {
        ThreadUnit threadUnit = this.threads[0];
        while (this.processJobs.Count > 0)
        {
          if (threadUnit.Free)
          {
            Job job = this.processJobs.Dequeue();
            threadUnit.AddJob(job);
          }
          else
          {
            this.spinWait.SpinOnce();
            if (threadUnit.WorkingTime > 750L)
            {
              MultiThreadManager.LogThreadOvertime(threadUnit);
              threadUnit.Abort();
              this.BrokenThreads.Add(threadUnit);
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
              interpolatedStringHandler.AppendLiteral("Repair critical time ");
              interpolatedStringHandler.AppendFormatted<int>(threadUnit.Number);
              threadUnit = new ThreadUnit(interpolatedStringHandler.ToStringAndClear(), threadUnit.Number);
              Thread.Sleep(5);
              ++this.FailedThreadsCount;
            }
          }
        }
      }
      if (this.BrokenThreads.Count > 0)
      {
        long num = 1500;
        for (int index = 0; index < this.BrokenThreads.Count; ++index)
        {
          ThreadUnit brokenThread = this.BrokenThreads[index];
          if (brokenThread != null && brokenThread.WorkingTime > num)
          {
            MultiThreadManager.LogThreadOvertime(brokenThread);
            DebugWindow.LogError("Thread does not respond to stop requests. Usually this indicates a broken plugin", 10f);
            this.BrokenThreads[index] = (ThreadUnit) null;
          }
        }
        if (this.BrokenThreads.AllF<ThreadUnit>((Predicate<ThreadUnit>) (x => x == null)))
          this.BrokenThreads.Clear();
      }
      Interlocked.CompareExchange(ref this._lock, 0, 1);
      this.ProcessWorking = false;
    }

    private static void LogThreadOvertime(ThreadUnit threadUnit)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 9);
      interpolatedStringHandler.AppendLiteral("Repair thread #");
      interpolatedStringHandler.AppendFormatted<int>(threadUnit.Number);
      interpolatedStringHandler.AppendLiteral(" with Unit Job1: ");
      interpolatedStringHandler.AppendFormatted(threadUnit.Job.Name);
      interpolatedStringHandler.AppendLiteral(" (C: ");
      interpolatedStringHandler.AppendFormatted<bool>(threadUnit.Job.IsCompleted);
      interpolatedStringHandler.AppendLiteral(" F: ");
      interpolatedStringHandler.AppendFormatted<bool>(threadUnit.Job.IsFailed);
      interpolatedStringHandler.AppendLiteral(") && Job2:");
      interpolatedStringHandler.AppendFormatted(threadUnit.SecondJob.Name);
      interpolatedStringHandler.AppendLiteral(" (C: ");
      interpolatedStringHandler.AppendFormatted<bool>(threadUnit.SecondJob.IsCompleted);
      interpolatedStringHandler.AppendLiteral(" F: ");
      interpolatedStringHandler.AppendFormatted<bool>(threadUnit.SecondJob.IsFailed);
      interpolatedStringHandler.AppendLiteral(") Time: ");
      interpolatedStringHandler.AppendFormatted<long>(threadUnit.WorkingTime);
      interpolatedStringHandler.AppendLiteral(" > ");
      interpolatedStringHandler.AppendFormatted<long>(750L);
      DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 5f);
    }

    public void Close()
    {
      foreach (ThreadUnit thread in this.threads)
        thread.Abort();
    }
  }
}
