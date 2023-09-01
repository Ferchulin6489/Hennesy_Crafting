// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.PluginWrapper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;


#nullable enable
namespace ExileCore.Shared
{
  public class PluginWrapper
  {
    private readonly 
    #nullable disable
    Lazy<FileSystemWatcher> _fileSystemWatcher;

    public DateTime LastWrite { get; set; } = DateTime.MinValue;

    public PluginWrapper(IPlugin plugin, string pathOnDisk)
    {
      this.Plugin = plugin;
      this.PathOnDisk = pathOnDisk;
      this._fileSystemWatcher = new Lazy<FileSystemWatcher>((Func<FileSystemWatcher>) (() => new FileSystemWatcher()
      {
        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime,
        Path = this.Plugin.DirectoryFullName,
        EnableRaisingEvents = true
      }));
      this.TickDebugInformation = new DebugInformation(this.Name + " [P]", nameof (plugin));
      this.RenderDebugInformation = new DebugInformation(this.Name + " [R]", nameof (plugin));
    }

    public double InitialiseTime { get; private set; }

    public bool Force => this.Plugin.Force;

    public string Name => this.Plugin.Name;

    public int Order => this.Plugin.Order;

    public IPlugin Plugin { get; private set; }

    public string PathOnDisk { get; }

    public bool CanRender { get; set; }

    public bool CanBeMultiThreading => this.Plugin.CanUseMultiThreading;

    public DebugInformation TickDebugInformation { get; }

    public DebugInformation RenderDebugInformation { get; }

    public bool IsEnable
    {
      get
      {
        try
        {
          return (bool) this.Plugin._Settings.Enable;
        }
        catch (Exception ex)
        {
          this.LogError(ex, nameof (IsEnable));
          return false;
        }
      }
    }

    public void CorrectThisTick(float val) => this.TickDebugInformation.CorrectAfterTick(val);

    public void Onload()
    {
      try
      {
        this.Plugin.OnLoad();
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (Onload));
      }
    }

    public void Initialise(GameController _gameController)
    {
      try
      {
        if (this.Plugin._Settings == null)
          throw new NullReferenceException("Cant load plugin (" + this.Plugin.Name + ") because settings is null.");
        if (this.Plugin.Initialized)
          throw new InvalidOperationException("Already initialized.");
        this.Plugin._Settings.Enable.OnValueChanged += (EventHandler<bool>) ((obj, value) =>
        {
          try
          {
            if (this.Plugin.Initialized)
            {
              List<Coroutine> list = Core.MainRunner.Coroutines.Concat<Coroutine>((IEnumerable<Coroutine>) Core.ParallelRunner.Coroutines).Where<Coroutine>((Func<Coroutine, bool>) (x => x.Owner == this.Plugin)).ToList<Coroutine>();
              if (value)
              {
                foreach (Coroutine coroutine in list)
                  coroutine.Resume();
              }
              else
              {
                foreach (Coroutine coroutine in list)
                  coroutine.Pause();
              }
            }
            if (value && !this.Plugin.Initialized)
            {
              this.Plugin.Initialized = this.pluginInitialise();
              if (this.Plugin.Initialized)
                this.Plugin.AreaChange(_gameController.Area.CurrentArea);
            }
            if (!value || this.Plugin.Initialized)
              return;
            this.Plugin._Settings.Enable.Value = false;
          }
          catch (Exception ex)
          {
            this.LogError(ex, nameof (Initialise));
          }
        });
        if (!(bool) this.Plugin._Settings.Enable)
          return;
        this.Plugin.Initialized = !this.Plugin.Initialized ? this.pluginInitialise() : throw new InvalidOperationException("Already initialized.");
        if (this.Plugin.Initialized)
          return;
        this.Plugin._Settings.Enable.Value = false;
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (Initialise));
      }
    }

    private bool pluginInitialise()
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      int num = this.Plugin.Initialise() ? 1 : 0;
      stopwatch.Stop();
      if (num == 0)
        return num != 0;
      double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
      this.InitialiseTime = totalMilliseconds;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(" -> Initialise time: ");
      interpolatedStringHandler.AppendFormatted<double>(totalMilliseconds);
      interpolatedStringHandler.AppendLiteral(" ms.");
      DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 1f, Color.Yellow);
      return num != 0;
    }

    public void SubscrideOnFile(Action<PluginWrapper, FileSystemEventArgs> action) => this._fileSystemWatcher.Value.Changed += (FileSystemEventHandler) ((_, args) =>
    {
      Action<PluginWrapper, FileSystemEventArgs> action1 = action;
      if (action1 == null)
        return;
      action1(this, args);
    });

    public void TurnOnOffPlugin(bool state) => this.Plugin._Settings.Enable.Value = state;

    public void AreaChange(AreaInstance area)
    {
      try
      {
        this.Plugin.AreaChange(area);
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (AreaChange));
      }
    }

    public Job PerfomanceTick()
    {
      try
      {
        using (this.TickDebugInformation.Measure())
          return this.Plugin.Tick();
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (PerfomanceTick));
        return (Job) null;
      }
    }

    public Job Tick()
    {
      try
      {
        return this.Plugin.Tick();
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (Tick));
        return (Job) null;
      }
    }

    public void PerfomanceRender()
    {
      try
      {
        using (this.RenderDebugInformation.Measure())
          this.Plugin.Render();
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (PerfomanceRender));
      }
    }

    public void Render()
    {
      try
      {
        this.Plugin.Render();
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (Render));
      }
    }

    private void LogError(Exception e, [CallerMemberName] string methodName = null)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
      interpolatedStringHandler.AppendFormatted(this.Plugin.Name);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(methodName);
      interpolatedStringHandler.AppendLiteral(" -> ");
      interpolatedStringHandler.AppendFormatted<Exception>(e);
      DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 3f);
    }

    public void EntityIgnored(Entity entity)
    {
      try
      {
        this.Plugin.EntityIgnored(entity);
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (EntityIgnored));
      }
    }

    public void EntityAddedAny(Entity entity)
    {
      try
      {
        this.Plugin.EntityAddedAny(entity);
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (EntityAddedAny));
      }
    }

    public void EntityAdded(Entity entity)
    {
      try
      {
        this.Plugin.EntityAdded(entity);
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (EntityAdded));
      }
    }

    public void EntityRemoved(Entity entity)
    {
      try
      {
        this.Plugin.EntityRemoved(entity);
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (EntityRemoved));
      }
    }

    public void ReceiveEvent(string eventId, object args)
    {
      try
      {
        this.Plugin.ReceiveEvent(eventId, args);
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (ReceiveEvent));
      }
    }

    public void SetApi(
      GameController gameController,
      Graphics graphics,
      PluginManager pluginManager)
    {
      this.Plugin.SetApi(gameController, graphics, pluginManager);
    }

    public void LoadSettings() => this.Plugin._LoadSettings();

    public void Close()
    {
      try
      {
        if (this._fileSystemWatcher.IsValueCreated)
          this._fileSystemWatcher.Value.Dispose();
        this.Plugin._SaveSettings();
        this.Plugin.OnPluginDestroyForHotReload();
        this.Plugin.OnClose();
        this.Plugin.OnUnload();
        this.Plugin.Dispose();
      }
      catch (Exception ex)
      {
        this.LogError(ex, nameof (Close));
      }
    }

    public void DrawSettings() => this.Plugin.DrawSettings();

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
      interpolatedStringHandler.AppendFormatted(this.Name);
      interpolatedStringHandler.AppendLiteral(" [");
      interpolatedStringHandler.AppendFormatted<int>(this.Order);
      interpolatedStringHandler.AppendLiteral("]");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
