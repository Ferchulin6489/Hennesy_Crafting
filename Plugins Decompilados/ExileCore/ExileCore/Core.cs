// Decompiled with JetBrains decompiler
// Type: ExileCore.Core
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ClickableTransparentOverlay;
using ExileCore.PoEMemory;
using ExileCore.RenderQ;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vanara.PInvoke;


#nullable enable
namespace ExileCore
{
  public class Core : IDisposable
  {
    public static 
    #nullable disable
    object SyncLocker = new object();
    private static readonly DebugInformation TotalDebugInformation = new DebugInformation("Total", "Total waste time");
    private static readonly DebugInformation CoroutineTickDebugInformation = new DebugInformation("Coroutine Tick");
    private static readonly DebugInformation CoreDebugInformation = new DebugInformation(nameof (Core));
    private static readonly DebugInformation MenuDebugInformation = new DebugInformation("Menu+Debug");
    private static readonly DebugInformation GcTickDebugInformation = new DebugInformation("GameController Tick");
    private static readonly DebugInformation AllPluginsDebugInformation = new DebugInformation("All plugins");
    private static readonly DebugInformation InterFrameInformation = new DebugInformation("Inter-frame", "Coroutines + sleep");
    private static readonly DebugInformation DeltaTimeDebugInformation = new DebugInformation("Delta Time", false);
    private static readonly DebugInformation FpsCounterDebugInformation = new DebugInformation("Fps counter", false);
    private static readonly DebugInformation ParallelCoroutineTickDebugInformation = new DebugInformation("Parallel Coroutine Tick");
    private const int JOB_TIMEOUT_MS = 200;
    private const int TICKS_BEFORE_SLEEP = 4;
    private readonly ActionOverlay _overlay;
    private readonly CoreSettings _coreSettings;
    private readonly DebugWindow _debugWindow;
    private readonly DX11 _dx11;
    private readonly WaitTime _mainControl = new WaitTime(2000);
    private readonly WaitTime _mainControl2 = new WaitTime(250);
    private readonly MenuWindow _mainMenu;
    private readonly SettingsContainer _settings;
    private readonly SoundController _soundController;
    private readonly List<(PluginWrapper plugin, Job job)> WaitingJobs = new List<(PluginWrapper, Job)>(20);
    private Memory _memory;
    private bool _memoryValid = true;
    private double _targetParallelFpsTime;
    private double ForeGroundTime;
    private int frameCounter;
    private System.Drawing.Rectangle lastClientBound;
    private double lastCounterTime;
    private double NextCoroutineTime;
    private double NextRender;
    private readonly TaskCompletionSource _readyToRun = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
    private IDisposable _totalTick;
    public static Core Current;

    public static ObservableCollection<DebugInformation> DebugInformations { get; } = new ObservableCollection<DebugInformation>();

    public Core(CancellationToken cancellationToken)
    {
      try
      {
        this._overlay = new ActionOverlay("ExileApi")
        {
          PostInitializedAction = (Func<Task>) (() =>
          {
            User32.MONITORINFO windowMonitorInfo = this._overlay.WindowMonitorInfo;
            this.lastClientBound = new System.Drawing.Rectangle((System.Drawing.Point) windowMonitorInfo.rcWork.Location, windowMonitorInfo.rcWork.Size);
            await this._readyToRun.Task;
          })
        };
        cancellationToken.Register(new Action(((Overlay) this._overlay).Close));
        string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "textures\\icon.ico");
        if (File.Exists(str))
          this._overlay.SetIcon(str);
        this._overlay.Start().Wait();
        this._settings = new SettingsContainer();
        this._coreSettings = this._settings.CoreSettings;
        this._coreSettings.PerformanceSettings.Threads = new RangeNode<int>(this._coreSettings.PerformanceSettings.Threads.Value, 0, Environment.ProcessorCount);
        this.CoroutineRunner = new Runner("Main Coroutine");
        this.CoroutineRunnerParallel = new Runner("Parallel Coroutine");
        using (new PerformanceTimer("DX11 Load"))
          this._dx11 = new DX11(this._overlay, this._coreSettings);
        if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
          Core.Logger.Information("SoundController init skipped because win7 issue.");
        else
          this._soundController = new SoundController("Sounds");
        this._coreSettings.Volume.OnValueChanged += (EventHandler<int>) ((_, i) => this._soundController.SetVolume((float) i / 100f));
        this._overlay.VSync = this._coreSettings.EnableVSync.Value;
        this._coreSettings.EnableVSync.OnValueChanged += (EventHandler<bool>) ((_1, _2) => this._overlay.VSync = this._coreSettings.EnableVSync.Value);
        this.Graphics = new Graphics(this._dx11, this._coreSettings);
        Core.MainRunner = this.CoroutineRunner;
        Core.ParallelRunner = this.CoroutineRunnerParallel;
        new System.Threading.Thread(new ThreadStart(this.ParallelCoroutineManualThread))
        {
          Name = "Parallel Coroutine",
          IsBackground = true
        }.Start();
        this._mainMenu = new MenuWindow(this, this._settings);
        this._debugWindow = new DebugWindow(this.Graphics, this._coreSettings);
        this.MultiThreadManager = new MultiThreadManager((int) this._coreSettings.PerformanceSettings.Threads);
        this.CoroutineRunner.MultiThreadManager = this.MultiThreadManager;
        this._coreSettings.PerformanceSettings.Threads.OnValueChanged += (EventHandler<int>) ((_, i) =>
        {
          if (this.MultiThreadManager == null)
            this.MultiThreadManager = new MultiThreadManager(i);
          else
            Core.ParallelRunner.Run(new Coroutine((Action) (() => this.MultiThreadManager.ChangeNumberThreads((int) this._coreSettings.PerformanceSettings.Threads)), (ExileCore.Shared.IYieldBase) new WaitTime(2000), (IPlugin) null, "Change Threads Number", false)
            {
              SyncModWork = true
            });
        });
        this.TargetPcFrameTime = 1000.0 / (double) (int) this._coreSettings.PerformanceSettings.TargetFps;
        this._targetParallelFpsTime = 1000.0 / (double) (int) this._coreSettings.PerformanceSettings.TargetParallelCoroutineFps;
        this._coreSettings.PerformanceSettings.TargetFps.OnValueChanged += (EventHandler<int>) ((_, i) => this.TargetPcFrameTime = 1000.0 / (double) i);
        this._coreSettings.PerformanceSettings.TargetParallelCoroutineFps.OnValueChanged += (EventHandler<int>) ((_, i) => this._targetParallelFpsTime = 1000.0 / (double) i);
        if (this._memory == null)
          this._memory = Core.FindPoe();
        if (this.GameController == null && this._memory != null)
          this.Inject();
        this.CoroutineRunnerParallel.Run(new Coroutine(this.MainControl(), (IPlugin) null, "Render control")
        {
          Priority = CoroutinePriority.Critical
        });
        this.NextCoroutineTime = Time.TotalMilliseconds;
        this.NextRender = Time.TotalMilliseconds;
        PluginManager pluginManager = this.pluginManager;
        if ((pluginManager != null ? (pluginManager.Plugins.Count == 0 ? 1 : 0) : 0) != 0)
          this._coreSettings.Enable.Value = true;
        this.Graphics.InitImage("missing_texture.png");
        Core.Current = this;
      }
      catch (Exception ex)
      {
        ILogger logger = Core.Logger;
        DefaultInterpolatedStringHandler interpolatedStringHandler;
        if (logger != null)
        {
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
          interpolatedStringHandler.AppendLiteral("Core constructor -> ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          logger.Error(interpolatedStringHandler.ToStringAndClear());
        }
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
        interpolatedStringHandler.AppendLiteral("Error in Core constructor -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        int num = (int) MessageBox.Show(interpolatedStringHandler.ToStringAndClear(), "Oops... Program failed to launch");
        throw;
      }
    }

    public static ILogger Logger { get; set; }

    public static Runner MainRunner { get; set; }

    public static Runner ParallelRunner { get; set; }

    public static uint FramesCount { get; private set; }

    public double TargetPcFrameTime { get; private set; }

    public MultiThreadManager MultiThreadManager { get; private set; }

    public PluginManager pluginManager { get; private set; }

    public Runner CoroutineRunner { get; set; }

    public Runner CoroutineRunnerParallel { get; set; }

    public GameController GameController { get; private set; }

    public bool GameStarted { get; private set; }

    public Graphics Graphics { get; }

    public bool IsForeground { get; private set; }

    public void Dispose()
    {
      this._memory?.Dispose();
      this._mainMenu?.Dispose();
      this.GameController?.Dispose();
      this._dx11?.Dispose();
      this.pluginManager?.CloseAllPlugins();
    }

    private IEnumerator MainControl()
    {
      while (true)
      {
        do
        {
          while (this._memory != null)
          {
            if (this.GameController == null && this._memory != null)
            {
              this.Inject();
              if (this.GameController == null)
                yield return (object) this._mainControl;
            }
            else
            {
              System.Drawing.Rectangle clientRectangle = WinApi.GetClientRectangle(this._memory.Process.MainWindowHandle);
              if (this.lastClientBound != clientRectangle && clientRectangle.Width > 2 && clientRectangle.Height > 2)
              {
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
                interpolatedStringHandler.AppendLiteral("Resize from: ");
                interpolatedStringHandler.AppendFormatted<System.Drawing.Rectangle>(this.lastClientBound);
                interpolatedStringHandler.AppendLiteral(" to ");
                interpolatedStringHandler.AppendFormatted<System.Drawing.Rectangle>(clientRectangle);
                DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 5f, SharpDX.Color.Magenta);
                this.lastClientBound = clientRectangle;
              }
              this._memoryValid = !this._memory.IsInvalid();
              if (!this._memoryValid)
              {
                this.GameController.Dispose();
                this.GameController = (GameController) null;
                this._memory = (Memory) null;
              }
              else
              {
                int num;
                if (!WinApi.IsForegroundWindow(this._memory.Process.MainWindowHandle))
                {
                  IntPtr? windowHandle = this._overlay.WindowHandle;
                  if (!windowHandle.HasValue || !WinApi.IsForegroundWindow(windowHandle.GetValueOrDefault()))
                  {
                    num = (bool) this._coreSettings.ForceForeground ? 1 : 0;
                    goto label_15;
                  }
                }
                num = 1;
label_15:
                bool flag = num != 0;
                this.IsForeground = flag;
                this.GameController.IsForeGroundCache = flag;
              }
              yield return (object) this._mainControl2;
            }
          }
          this._memory = Core.FindPoe();
        }
        while (this._memory != null);
        yield return (object) this._mainControl;
      }
    }

    public static Memory FindPoe()
    {
      (System.Diagnostics.Process process, Offsets offsets)? poeProcess = Core.FindPoeProcess();
      if (poeProcess.HasValue && poeProcess.Value.process.Id != 0)
        return new Memory(poeProcess.Value);
      DebugWindow.LogMsg("Game not found");
      return (Memory) null;
    }

    private void Inject()
    {
      try
      {
        if (this._memory == null)
          return;
        this.GameController = new GameController(this._memory, this._soundController, this._settings, this.MultiThreadManager);
        using (new PerformanceTimer("Plugin loader"))
          this.pluginManager = new PluginManager(this.GameController, this.Graphics, this.MultiThreadManager);
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
        interpolatedStringHandler.AppendLiteral("Inject -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
    }

    private void OnFocusLoss(object sender, EventArgs eventArgs)
    {
      IntPtr? mainWindowHandle = this._memory?.Process?.MainWindowHandle;
      if (!mainWindowHandle.HasValue)
        return;
      IntPtr valueOrDefault = mainWindowHandle.GetValueOrDefault();
      if (!(valueOrDefault != IntPtr.Zero) || WinApi.IsIconic(valueOrDefault))
        return;
      WinApi.SetForegroundWindow(valueOrDefault);
    }

    public void Tick()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler;
      try
      {
        using (Core.CoreDebugInformation.Measure())
        {
          using ((bool) this._coreSettings.ApplySelectedFontGlobally ? this.Graphics.UseCurrentFont() : (IDisposable) null)
          {
            ImGui.GetStyle().MouseCursorScale = this._coreSettings.MouseCursorScale.Value;
            this._memory?.NotifyFrame();
            Input.Update(this._overlay.WindowHandle);
            using (Core.MenuDebugInformation.Measure())
            {
              ++Core.FramesCount;
              NextFrameTask.NextFrameAwaiter.SetNextFrame();
              if (!this.IsForeground)
                this.ForeGroundTime += Core.DeltaTimeDebugInformation.Tick;
              else
                this.ForeGroundTime = 0.0;
              if (this.ForeGroundTime <= 100.0)
              {
                try
                {
                  this._debugWindow?.Render();
                }
                catch (Exception ex)
                {
                  interpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
                  interpolatedStringHandler.AppendLiteral("DebugWindow Tick -> ");
                  interpolatedStringHandler.AppendFormatted<Exception>(ex);
                  DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
                }
                try
                {
                  this._mainMenu?.Render(this.GameController);
                }
                catch (Exception ex)
                {
                  interpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
                  interpolatedStringHandler.AppendLiteral("Core Tick Menu -> ");
                  interpolatedStringHandler.AppendFormatted<Exception>(ex);
                  DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
                }
              }
            }
            if (this.GameController == null)
              return;
            using (Core.GcTickDebugInformation.Measure())
              this.GameController.Tick();
            using (Core.AllPluginsDebugInformation.Measure())
            {
              if (this.ForeGroundTime > 150.0 || this.pluginManager == null)
                return;
              this.WaitingJobs.Clear();
              if ((bool) this._coreSettings.PerformanceSettings.CollectDebugInformation)
              {
                foreach (PluginWrapper plugin in this.pluginManager.Plugins)
                {
                  if (plugin.IsEnable && (this.GameController.InGame || plugin.Force))
                  {
                    plugin.CanRender = true;
                    Job job = plugin.PerfomanceTick();
                    if (job != null)
                    {
                      if (this.MultiThreadManager.ThreadsCount > 0)
                      {
                        if (!job.IsStarted)
                          this.MultiThreadManager.AddJob(job);
                        this.WaitingJobs.Add((plugin, job));
                      }
                      else
                      {
                        double num = (double) plugin.TickDebugInformation.TickAction(job.Work);
                      }
                    }
                  }
                }
              }
              else
              {
                foreach (PluginWrapper plugin in this.pluginManager.Plugins)
                {
                  if (plugin.IsEnable && (this.GameController.InGame || plugin.Force))
                  {
                    plugin.CanRender = true;
                    Job job = plugin.Tick();
                    if (job != null)
                    {
                      if (this.MultiThreadManager.ThreadsCount > 0)
                      {
                        if (!job.IsStarted)
                          this.MultiThreadManager.AddJob(job);
                        this.WaitingJobs.Add((plugin, job));
                      }
                      else
                        job.Work();
                    }
                  }
                }
              }
              if (this.WaitingJobs.Count > 0)
              {
                this.MultiThreadManager.Process((object) this);
                SpinWait.SpinUntil((Func<bool>) (() => this.WaitingJobs.AllF<(PluginWrapper, Job)>((Predicate<(PluginWrapper, Job)>) (x => x.job.IsCompleted))), 200);
                if ((bool) this._coreSettings.PerformanceSettings.CollectDebugInformation)
                {
                  foreach ((PluginWrapper plugin, Job job) waitingJob in this.WaitingJobs)
                  {
                    waitingJob.plugin.TickDebugInformation.CorrectAfterTick((float) waitingJob.job.ElapsedMs);
                    if (waitingJob.job.IsFailed && waitingJob.job.IsCompleted)
                    {
                      waitingJob.plugin.CanRender = false;
                      interpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 3);
                      interpolatedStringHandler.AppendFormatted(waitingJob.plugin.Name);
                      interpolatedStringHandler.AppendLiteral(" job timeout: ");
                      interpolatedStringHandler.AppendFormatted<double>(waitingJob.job.ElapsedMs);
                      interpolatedStringHandler.AppendLiteral(" ms. Thread# ");
                      interpolatedStringHandler.AppendFormatted<ThreadUnit>(waitingJob.job.WorkingOnThread);
                      DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear());
                    }
                  }
                }
                else
                {
                  foreach ((PluginWrapper plugin, Job job) waitingJob in this.WaitingJobs)
                  {
                    if (waitingJob.job.IsFailed)
                      waitingJob.plugin.CanRender = false;
                  }
                }
              }
              if ((bool) this._coreSettings.PerformanceSettings.CollectDebugInformation)
              {
                foreach (PluginWrapper plugin in this.pluginManager.Plugins)
                {
                  if (plugin.IsEnable && plugin.CanRender && (this.GameController.InGame || plugin.Force))
                    plugin.PerfomanceRender();
                }
              }
              else
              {
                foreach (PluginWrapper plugin in this.pluginManager.Plugins)
                {
                  if (plugin.IsEnable && (this.GameController.InGame || plugin.Force))
                    plugin.Render();
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
        interpolatedStringHandler.AppendLiteral("Core tick -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
    }

    private static (System.Diagnostics.Process process, Offsets offsets)? FindPoeProcess()
    {
      List<(System.Diagnostics.Process, Offsets)> list = ((IEnumerable<Offsets>) new Offsets[3]
      {
        Offsets.Regular,
        Offsets.Korean,
        Offsets.Steam
      }).SelectMany<Offsets, (System.Diagnostics.Process, Offsets)>((Func<Offsets, IEnumerable<(System.Diagnostics.Process, Offsets)>>) (o => ((IEnumerable<System.Diagnostics.Process>) System.Diagnostics.Process.GetProcessesByName(o.ExeName)).Select<System.Diagnostics.Process, (System.Diagnostics.Process, Offsets)>((Func<System.Diagnostics.Process, (System.Diagnostics.Process, Offsets)>) (p => (p, o))))).Where<(System.Diagnostics.Process, Offsets)>((Func<(System.Diagnostics.Process, Offsets), bool>) (x => !x.p.HasExited)).OrderBy<(System.Diagnostics.Process, Offsets), int>((Func<(System.Diagnostics.Process, Offsets), int>) (x => x.p.Id)).ToList<(System.Diagnostics.Process, Offsets)>();
      if (!list.Any<(System.Diagnostics.Process, Offsets)>())
        return new (System.Diagnostics.Process, Offsets)?();
      int? nullable = list.Count > 1 ? ProcessPicker.ShowDialogBox(list.Select<(System.Diagnostics.Process, Offsets), System.Diagnostics.Process>((Func<(System.Diagnostics.Process, Offsets), System.Diagnostics.Process>) (x => x.p))) : new int?(0);
      if (nullable.HasValue)
        return nullable.GetValueOrDefault() == -1 ? new (System.Diagnostics.Process, Offsets)?() : new (System.Diagnostics.Process, Offsets)?(list[nullable.Value]);
      Environment.Exit(0);
      return new (System.Diagnostics.Process, Offsets)?();
    }

    private void ParallelCoroutineManualThread()
    {
      try
      {
        while (true)
        {
          int millisecondsTimeout;
          do
          {
            this.MultiThreadManager?.Process((object) this);
            if (!this.CoroutineRunnerParallel.IsRunning)
            {
              System.Threading.Thread.Sleep(10);
            }
            else
            {
              DebugInformation.MeasureHolder measureHolder = Core.ParallelCoroutineTickDebugInformation.Measure();
              try
              {
                for (int index = 0; index < this.CoroutineRunnerParallel.IterationPerFrame; ++index)
                  this.CoroutineRunnerParallel.Update();
              }
              catch (Exception ex)
              {
                DebugWindow.LogMsg("Coroutine Parallel error: " + ex.Message, 6f, SharpDX.Color.White);
              }
              measureHolder.Dispose();
              millisecondsTimeout = (int) (this._targetParallelFpsTime - measureHolder.Elapsed.TotalMilliseconds);
            }
          }
          while (millisecondsTimeout <= 0);
          System.Threading.Thread.Sleep(millisecondsTimeout);
        }
      }
      catch (Exception ex)
      {
        DebugWindow.LogMsg("Coroutine Parallel error: " + ex.Message, 6f, SharpDX.Color.White);
        throw;
      }
    }

    public void TickCoroutines()
    {
      if (this.NextCoroutineTime > Time.TotalMilliseconds)
        return;
      this.NextCoroutineTime += this._targetParallelFpsTime;
      using (Core.CoroutineTickDebugInformation.Measure())
      {
        if (!this.CoroutineRunner.IsRunning)
          return;
        if ((bool) this._coreSettings.PerformanceSettings.CoroutineMultiThreading)
          this.CoroutineRunner.ParallelUpdate();
        else
          this.CoroutineRunner.Update();
      }
    }

    private void Render()
    {
      this._overlay.Position = this.lastClientBound.Location;
      this._overlay.Size = this.lastClientBound.Size;
      this.NextRender = Time.TotalMilliseconds + this.TargetPcFrameTime;
      this._dx11.ImGuiRender.BeginBackGroundWindow();
      this.Tick();
      ++this.frameCounter;
      WaitRender.Frame();
      if (Time.TotalMilliseconds - this.lastCounterTime <= 1000.0)
        return;
      Core.FpsCounterDebugInformation.Tick = (double) this.frameCounter;
      Core.DeltaTimeDebugInformation.Tick = 1000.0 / (double) this.frameCounter;
      this.lastCounterTime = Time.TotalMilliseconds;
      this.frameCounter = 0;
    }

    public void Run()
    {
      int num1 = (int) WinMm.timeBeginPeriod(1U);
      this._overlay.FocusLost += new EventHandler(this.OnFocusLoss);
      this._overlay.RenderAction = new Action(this.Render);
      this._overlay.PostFrameAction = (Action) (() =>
      {
        this._totalTick?.Dispose();
        using (Core.InterFrameInformation.Measure())
        {
          int num2 = 0;
          do
          {
            this.TickCoroutines();
            if (++num2 >= 4)
            {
              System.Threading.Thread.Sleep(1);
              num2 = 0;
            }
          }
          while (Time.TotalMilliseconds < this.NextRender);
        }
        this._totalTick = (IDisposable) Core.TotalDebugInformation.Measure();
      });
      this._readyToRun.SetResult();
      this._overlay.WaitForShutdown().Wait();
    }
  }
}
