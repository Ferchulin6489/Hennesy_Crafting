// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Coroutine
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class Coroutine
  {
    private IEnumerator _enumerator;

    private Coroutine(string name, IPlugin owner)
    {
      this.Name = name ?? MathHepler.GetRandomWord(13);
      this.Owner = owner;
      this.OwnerName = this.Owner == null ? "Free" : this.Owner.GetType().Namespace;
    }

    public Coroutine(
      Action action,
      IYieldBase condition,
      IPlugin owner,
      string name = null,
      bool infinity = true,
      bool autoStart = true)
      : this(name, owner)
    {
      this.Running = autoStart;
      this.Started = DateTime.Now;
      string str;
      switch (condition)
      {
        case WaitTime waitTime:
          str = waitTime.Milliseconds.ToString();
          break;
        case WaitRender waitRender:
          str = waitRender.HowManyRenderCountWait.ToString();
          break;
        case WaitRandom waitRandom:
          str = waitRandom.Timeout;
          break;
        case WaitFunction _:
          str = "Function -1";
          break;
        default:
          str = this.TimeoutForAction;
          break;
      }
      this.TimeoutForAction = str;
      this.Action = action;
      this.Condition = condition;
      if (infinity)
      {
        this._enumerator = CoroutineAction(action);

        IEnumerator CoroutineAction(Action a)
        {
          yield return YieldBase.RealWork;
          while (true)
          {
            try
            {
              Action action = a;
              if (action != null)
                action();
            }
            catch (Exception ex)
            {
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
              interpolatedStringHandler.AppendLiteral("Coroutine ");
              interpolatedStringHandler.AppendFormatted(this.Name);
              interpolatedStringHandler.AppendLiteral(" in ");
              interpolatedStringHandler.AppendFormatted(this.OwnerName);
              interpolatedStringHandler.AppendLiteral(" error -> ");
              interpolatedStringHandler.AppendFormatted<Exception>(ex);
              Console.WriteLine(interpolatedStringHandler.ToStringAndClear());
            }
            this.Ticks++;
            yield return (object) this.Condition.GetEnumerator();
          }
        }
      }
      else
      {
        this._enumerator = CoroutineAction(action);

        IEnumerator CoroutineAction(Action a)
        {
          yield return (object) this.Condition.GetEnumerator();
          Action action = a;
          if (action != null)
            action();
          this.Ticks++;
        }
      }
    }

    public Coroutine(
      Action action,
      int waitMilliseconds,
      IPlugin owner,
      string name = null,
      bool autoStart = true)
      : this(action, (IYieldBase) new WaitTime(waitMilliseconds), owner, name, autoStart)
    {
    }

    public Coroutine(IEnumerator enumerator, IPlugin owner, string name = null, bool autoStart = true)
      : this(name, owner)
    {
      this.Running = autoStart;
      this.Started = DateTime.Now;
      this.TimeoutForAction = "Not simple -1";
      this._enumerator = enumerator;
    }

    public bool IsDone { get; private set; }

    public string Name { get; set; }

    public IPlugin Owner { get; private set; }

    public string OwnerName { get; }

    public bool Running { get; private set; }

    public bool AutoResume { get; set; } = true;

    public string TimeoutForAction { get; private set; }

    public long Ticks { get; private set; } = -1;

    public CoroutinePriority Priority { get; set; }

    public DateTime Started { get; set; }

    public Action Action { get; private set; }

    public IYieldBase Condition { get; private set; }

    public bool ThisIsSimple => this.Action != null;

    public bool NextIterRealWork { get; set; }

    public bool SyncModWork { get; set; }

    public event Action OnAutoRestart;

    public event EventHandler WhenDone;

    public void UpdateCondtion(IYieldBase condition)
    {
      string str;
      switch (condition)
      {
        case WaitTime waitTime:
          str = waitTime.Milliseconds.ToString();
          break;
        case WaitRender waitRender:
          str = waitRender.HowManyRenderCountWait.ToString();
          break;
        case WaitFunction _:
          str = "Function";
          break;
        default:
          str = this.TimeoutForAction;
          break;
      }
      this.TimeoutForAction = str;
      this.Condition = condition;
    }

    public IEnumerator GetEnumerator() => this._enumerator;

    public void UpdateTicks(uint tick) => this.Ticks = (long) tick;

    public void Resume() => this.Running = true;

    public void AutoRestart()
    {
      Action onAutoRestart = this.OnAutoRestart;
      if (onAutoRestart == null)
        return;
      onAutoRestart();
    }

    public void Pause(bool force = false)
    {
      if (this.Priority == CoroutinePriority.Critical && !force)
        return;
      this.Running = false;
    }

    public bool Done(bool force = false)
    {
      if (this.Priority == CoroutinePriority.Critical)
        return false;
      this.Running = false;
      this.IsDone = true;
      EventHandler whenDone = this.WhenDone;
      if (whenDone != null)
        whenDone((object) this, EventArgs.Empty);
      return this.IsDone;
    }

    public void UpdateAction(Action action)
    {
      if (this.Action == null)
        return;
      this.Action = action;
    }

    public void UpdateAction(IEnumerator action)
    {
      if (this._enumerator == null)
        return;
      this._enumerator = action;
    }

    public bool MoveNext() => this.MoveNext(this._enumerator);

    private bool MoveNext(IEnumerator enumerator)
    {
      if (this.IsDone)
        return false;
      bool flag;
      if (enumerator.Current is IEnumerator current && this.MoveNext(current))
      {
        flag = true;
      }
      else
      {
        flag = enumerator.MoveNext();
        this.NextIterRealWork = enumerator.Current == YieldBase.RealWork;
      }
      return flag;
    }
  }
}
