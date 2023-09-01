// Decompiled with JetBrains decompiler
// Type: ExileCore.ThreadUnit
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Diagnostics;
using System.Threading;

namespace ExileCore
{
  public class ThreadUnit
  {
    private readonly AutoResetEvent _event;
    private readonly Stopwatch sw;
    private readonly Thread thread;
    private bool _wait = true;
    private bool running = true;

    public ThreadUnit(string name, int number)
    {
      this.Number = number;
      this.Job = new Job("InitJob", (Action) null)
      {
        IsCompleted = true
      };
      this.SecondJob = new Job("InitJob", (Action) null)
      {
        IsCompleted = true
      };
      this._event = new AutoResetEvent(false);
      this.thread = new Thread(new ThreadStart(this.DoWork));
      this.thread.Name = name;
      this.thread.IsBackground = true;
      this.thread.Start();
      this.sw = Stopwatch.StartNew();
    }

    public static int CountJobs { get; set; }

    public static int CountWait { get; set; }

    public int Number { get; }

    public Job Job { get; private set; }

    public Job SecondJob { get; private set; }

    public bool Free => this.Job.IsCompleted || this.SecondJob.IsCompleted;

    public long WorkingTime => this.sw.ElapsedMilliseconds;

    private void DoWork()
    {
      while (this.running)
      {
        if (this.Job.IsCompleted && this.SecondJob.IsCompleted)
        {
          this._event.WaitOne();
          ++ThreadUnit.CountWait;
          this._wait = true;
        }
        if (!this.Job.IsCompleted)
        {
          try
          {
            this.sw.Restart();
            Action work = this.Job.Work;
            if (work != null)
              work();
          }
          catch (Exception ex)
          {
            DebugWindow.LogError(ex.ToString());
            this.Job.IsFailed = true;
          }
          finally
          {
            this.Job.ElapsedMs = this.sw.Elapsed.TotalMilliseconds;
            this.Job.IsCompleted = true;
            this.sw.Restart();
          }
        }
        if (!this.SecondJob.IsCompleted)
        {
          try
          {
            this.sw.Restart();
            Action work = this.SecondJob.Work;
            if (work != null)
              work();
          }
          catch (Exception ex)
          {
            DebugWindow.LogError(ex.ToString());
            this.SecondJob.IsFailed = true;
          }
          finally
          {
            this.SecondJob.ElapsedMs = this.sw.Elapsed.TotalMilliseconds;
            this.SecondJob.IsCompleted = true;
            this.sw.Restart();
          }
        }
      }
    }

    public bool AddJob(Job job)
    {
      job.WorkingOnThread = this;
      bool flag = false;
      if (this.Job.IsCompleted)
      {
        this.Job = job;
        flag = true;
        ++ThreadUnit.CountJobs;
      }
      else if (this.SecondJob.IsCompleted)
      {
        this.SecondJob = job;
        flag = true;
        ++ThreadUnit.CountJobs;
      }
      if (this._wait & flag)
      {
        this._wait = false;
        this._event.Set();
      }
      return flag;
    }

    public void Abort()
    {
      this.Job.IsCompleted = true;
      this.SecondJob.IsCompleted = true;
      this.Job.IsFailed = true;
      this.Job.IsFailed = true;
      if (this._wait)
        this._event.Set();
      this.running = false;
    }
  }
}
