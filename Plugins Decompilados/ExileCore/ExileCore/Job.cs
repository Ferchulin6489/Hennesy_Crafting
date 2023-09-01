// Decompiled with JetBrains decompiler
// Type: ExileCore.Job
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Diagnostics;

namespace ExileCore
{
  [DebuggerDisplay("Name: {Name}, Elapsed: {ElapsedMs}, Completed: {IsCompleted}, Failed: {IsFailed}")]
  public class Job
  {
    public volatile bool IsCompleted;
    public volatile bool IsFailed;
    public volatile bool IsStarted;

    public Job(string name, Action work)
    {
      this.Name = name;
      this.Work = work;
    }

    public Action Work { get; set; }

    public string Name { get; set; }

    public ThreadUnit WorkingOnThread { get; set; }

    public double ElapsedMs { get; set; }
  }
}
