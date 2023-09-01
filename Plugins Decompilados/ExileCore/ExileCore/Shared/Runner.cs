// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Runner
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ExileCore.Shared
{
  public class Runner
  {
    private readonly HashSet<Coroutine> _autorestartCoroutines = new HashSet<Coroutine>();
    private readonly List<CoroutineDetails> _finishedCoroutines = new List<CoroutineDetails>();
    private readonly object locker = new object();
    private readonly Stopwatch sw;
    private readonly List<Job> jobs = new List<Job>(16);
    private double time;

    public Runner(string name)
    {
      this.Name = name;
      this.sw = Stopwatch.StartNew();
    }

    public MultiThreadManager MultiThreadManager { get; set; }

    public string Name { get; }

    public int CriticalTimeWork { get; set; } = 150;

    public bool IsRunning => this.Coroutines.Count > 0;

    public int CoroutinesCount => this.Coroutines.Count;

    public List<CoroutineDetails> FinishedCoroutines => this._finishedCoroutines.ToList<CoroutineDetails>();

    public int FinishedCoroutineCount { get; private set; }

    public IList<Coroutine> Coroutines { get; } = (IList<Coroutine>) new List<Coroutine>();

    public IEnumerable<Coroutine> WorkingCoroutines => this.Coroutines.Where<Coroutine>((Func<Coroutine, bool>) (x => x.Running));

    public int CountAddCoroutines { get; private set; }

    public int CountFalseAddCoroutines { get; private set; }

    public int IterationPerFrame { get; set; } = 2;

    public Dictionary<string, double> CoroutinePerformance { get; } = new Dictionary<string, double>();

    public int Count => this.Coroutines.Count;

    public Coroutine Run(IEnumerator enumerator, IPlugin owner, string name = null)
    {
      if (enumerator == null)
        throw new NullReferenceException("Coroutine cant not be null.");
      Coroutine routine = new Coroutine(enumerator, owner, name);
      lock (this.locker)
      {
        Coroutine coroutine = this.Coroutines.FirstOrDefault<Coroutine>((Func<Coroutine, bool>) (x => x.Name == routine.Name && x.Owner == routine.Owner));
        if (coroutine != null)
        {
          ++this.CountFalseAddCoroutines;
          return coroutine;
        }
        this.Coroutines.Add(routine);
        double num;
        this.CoroutinePerformance.TryGetValue(routine.Name, out num);
        this.CoroutinePerformance[routine.Name] = num;
        ++this.CountAddCoroutines;
        return routine;
      }
    }

    public Coroutine Run(Coroutine routine)
    {
      if (routine == null)
        throw new NullReferenceException("Coroutine cant not be null.");
      lock (this.locker)
      {
        Coroutine coroutine = this.Coroutines.FirstOrDefault<Coroutine>((Func<Coroutine, bool>) (x => x.Name == routine.Name && x.Owner == routine.Owner));
        if (coroutine != null)
        {
          ++this.CountFalseAddCoroutines;
          return coroutine;
        }
        this.Coroutines.Add(routine);
        double num;
        this.CoroutinePerformance.TryGetValue(routine.Name, out num);
        this.CoroutinePerformance[routine.Name] = num;
        ++this.CountAddCoroutines;
        return routine;
      }
    }

    public void PauseCoroutines(IList<Coroutine> coroutines)
    {
      foreach (Coroutine coroutine in (IEnumerable<Coroutine>) coroutines)
        coroutine.Pause();
    }

    public void ResumeCoroutines(IList<Coroutine> coroutines)
    {
      foreach (Coroutine coroutine in (IEnumerable<Coroutine>) coroutines)
      {
        if (coroutine.AutoResume)
          coroutine.Resume();
      }
    }

    public Coroutine FindByName(string name) => this.Coroutines.FirstOrDefault<Coroutine>((Func<Coroutine, bool>) (x => x.Name == name));

    public Coroutine ByFuncName(Func<string, bool> predicate) => this.Coroutines.FirstOrDefault<Coroutine>((Func<Coroutine, bool>) (x => predicate(x.Name)));

    public void Update()
    {
      if (this.Coroutines.Count <= 0)
        return;
      for (int index = 0; index < this.Coroutines.Count; ++index)
      {
        Coroutine coroutine = this.Coroutines[index];
        DefaultInterpolatedStringHandler interpolatedStringHandler;
        if (!coroutine.IsDone)
        {
          if (coroutine.Running)
          {
            try
            {
              TimeSpan elapsed = this.sw.Elapsed;
              this.time = elapsed.TotalMilliseconds;
              if (!coroutine.MoveNext())
                coroutine.Done();
              elapsed = this.sw.Elapsed;
              double num = elapsed.TotalMilliseconds - this.time;
              this.CoroutinePerformance[coroutine.Name] += num;
              if (num > (double) this.CriticalTimeWork)
              {
                interpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 4);
                interpolatedStringHandler.AppendLiteral("Coroutine ");
                interpolatedStringHandler.AppendFormatted(coroutine.Name);
                interpolatedStringHandler.AppendLiteral(" (");
                interpolatedStringHandler.AppendFormatted(coroutine.OwnerName);
                interpolatedStringHandler.AppendLiteral(") [");
                interpolatedStringHandler.AppendFormatted(this.Name);
                interpolatedStringHandler.AppendLiteral("] ");
                interpolatedStringHandler.AppendFormatted<double>(num);
                interpolatedStringHandler.AppendLiteral(" $Performance coroutine");
                Console.WriteLine(interpolatedStringHandler.ToStringAndClear());
              }
            }
            catch (Exception ex)
            {
              Dictionary<string, double> coroutinePerformance = this.CoroutinePerformance;
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
              interpolatedStringHandler.AppendFormatted(coroutine.Name);
              interpolatedStringHandler.AppendLiteral(" | (");
              interpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now);
              interpolatedStringHandler.AppendLiteral(")");
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              double num = this.CoroutinePerformance[coroutine.Name] + (this.sw.Elapsed.TotalMilliseconds - this.time);
              coroutinePerformance[stringAndClear] = num;
              this.CoroutinePerformance[coroutine.Name] = 0.0;
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 3);
              interpolatedStringHandler.AppendLiteral("Coroutine ");
              interpolatedStringHandler.AppendFormatted(coroutine.Name);
              interpolatedStringHandler.AppendLiteral(" (");
              interpolatedStringHandler.AppendFormatted(coroutine.OwnerName);
              interpolatedStringHandler.AppendLiteral(") error: ");
              interpolatedStringHandler.AppendFormatted<Exception>(ex);
              Console.WriteLine(interpolatedStringHandler.ToStringAndClear());
            }
          }
        }
        else
        {
          this._finishedCoroutines.Add(new CoroutineDetails(coroutine.Name, coroutine.OwnerName, coroutine.Ticks, coroutine.Started, DateTime.Now));
          ++this.FinishedCoroutineCount;
          this.Coroutines.Remove(coroutine);
        }
      }
    }

    public void ParallelUpdate()
    {
      if (this.MultiThreadManager == null || this.MultiThreadManager.ThreadsCount < 1)
      {
        this.Update();
      }
      else
      {
        if (this.Coroutines.Count <= 0)
          return;
        this.jobs.Clear();
        for (int index = 0; index < this.Coroutines.Count; ++index)
        {
          Coroutine coroutine = this.Coroutines[index];
          if (!coroutine.IsDone)
          {
            if (coroutine.Running)
            {
              if (coroutine.NextIterRealWork && !coroutine.SyncModWork)
              {
                this.jobs.Add(this.MultiThreadManager.AddJob((Action) (() =>
                {
                  if (coroutine.MoveNext())
                    return;
                  coroutine.Done();
                }), coroutine.Name));
              }
              else
              {
                this.time = this.sw.Elapsed.TotalMilliseconds;
                if (!coroutine.MoveNext())
                  coroutine.Done();
                double num = this.sw.Elapsed.TotalMilliseconds - this.time;
                this.CoroutinePerformance[coroutine.Name] += num;
                if (num > (double) this.CriticalTimeWork)
                {
                  DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 4);
                  interpolatedStringHandler.AppendLiteral("Coroutine ");
                  interpolatedStringHandler.AppendFormatted(coroutine.Name);
                  interpolatedStringHandler.AppendLiteral(" (");
                  interpolatedStringHandler.AppendFormatted(coroutine.OwnerName);
                  interpolatedStringHandler.AppendLiteral(") [");
                  interpolatedStringHandler.AppendFormatted(this.Name);
                  interpolatedStringHandler.AppendLiteral("] ");
                  interpolatedStringHandler.AppendFormatted<double>(num);
                  interpolatedStringHandler.AppendLiteral(" $Performance coroutine");
                  Console.WriteLine(interpolatedStringHandler.ToStringAndClear());
                }
              }
            }
          }
          else
          {
            this._finishedCoroutines.Add(new CoroutineDetails(coroutine.Name, coroutine.OwnerName, coroutine.Ticks, coroutine.Started, DateTime.Now));
            ++this.FinishedCoroutineCount;
            this.Coroutines.Remove(coroutine);
          }
        }
        this.MultiThreadManager.Process((object) this);
        SpinWait.SpinUntil((Func<bool>) (() => this.jobs.AllF<Job>((Predicate<Job>) (job => job.IsCompleted))), 500);
        foreach (Job job in this.jobs)
          this.CoroutinePerformance[job.Name] += job.ElapsedMs;
      }
    }
  }
}
