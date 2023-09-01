// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.DebugInformation
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Diagnostics;

namespace ExileCore.Shared
{
  public class DebugInformation
  {
    public static readonly int SizeArray = 512;
    private readonly Stopwatch sw = Stopwatch.StartNew();
    private double tick;

    public DebugInformation(string name, bool main = true)
    {
      this.Name = name;
      this.Main = main;
      for (int index = 0; index < DebugInformation.SizeArray; ++index)
      {
        this.Ticks[index] = 0.0f;
        this.TicksAverage[index] = 0.0f;
      }
      lock (Core.SyncLocker)
        Core.DebugInformations.Add(this);
    }

    public DebugInformation(string name, string description, bool main = true)
      : this(name, main)
    {
      this.Description = description;
    }

    public string Name { get; }

    public string Description { get; }

    public bool Main { get; }

    public int IndexTickAverage { get; private set; }

    public int Index { get; private set; }

    public float Sum { get; private set; }

    public float Total { get; private set; }

    private float TotalIndex { get; set; }

    public float TotalMaxAverage { get; private set; }

    public float TotalAverage { get; private set; }

    public float Average { get; private set; }

    public bool AtLeastOneFullTick { get; private set; }

    public double Tick
    {
      get => this.tick;
      set
      {
        this.tick = value;
        if (this.Index >= DebugInformation.SizeArray)
        {
          this.Index = 0;
          this.Sum = this.Ticks.SumF();
          this.TotalIndex += (float) DebugInformation.SizeArray;
          this.Total += this.Sum;
          this.Average = this.Sum / (float) DebugInformation.SizeArray;
          this.TotalAverage = this.Total / this.TotalIndex;
          this.TotalMaxAverage = Math.Max(this.TotalMaxAverage, this.Average);
          if (this.IndexTickAverage >= DebugInformation.SizeArray)
            this.IndexTickAverage = 0;
          if (this.IndexTickAverage == 0 && (double) this.Average > 16.0)
          {
            this.Average = 0.0f;
            this.TotalMaxAverage = 0.0f;
          }
          this.TicksAverage[this.IndexTickAverage] = this.Average;
          ++this.IndexTickAverage;
          this.AtLeastOneFullTick = true;
        }
        this.Ticks[this.Index] = (float) value;
        ++this.Index;
      }
    }

    public DebugInformation.MeasureHolder Measure() => new DebugInformation.MeasureHolder(this);

    public float[] Ticks { get; } = new float[DebugInformation.SizeArray];

    public float[] TicksAverage { get; } = new float[DebugInformation.SizeArray];

    public void CorrectAfterTick(float val)
    {
      this.Ticks[this.Index - 1] = val;
      this.tick += (double) val;
    }

    public float TickAction(Action action, bool onlyValue = false)
    {
      double totalMilliseconds = this.sw.Elapsed.TotalMilliseconds;
      action();
      float num = (float) (this.sw.Elapsed.TotalMilliseconds - totalMilliseconds);
      if (!onlyValue)
        this.Tick = (double) num;
      return num;
    }

    public void AddToCurrentTick(float value) => this.Ticks[this.Index] += value;

    public class MeasureHolder : IDisposable
    {
      private readonly DebugInformation _debugInformation;
      private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
      private bool _disposed;

      public MeasureHolder(DebugInformation debugInformation) => this._debugInformation = debugInformation;

      public void Dispose()
      {
        if (this._disposed)
          return;
        this._stopwatch.Stop();
        this._debugInformation.Tick = this._stopwatch.Elapsed.TotalMilliseconds;
        this._disposed = true;
      }

      public TimeSpan Elapsed => this._stopwatch.Elapsed;
    }
  }
}
