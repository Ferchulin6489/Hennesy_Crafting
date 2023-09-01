// Decompiled with JetBrains decompiler
// Type: ExileCore.GameController
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ExileCore
{
  public class GameController : IDisposable
  {
    private readonly CoreSettings _settings;
    private static readonly DebugInformation DebClearCache = new DebugInformation("Clear cache", false);
    private readonly DebugInformation debDeltaTime;
    private readonly TimeCache<Vector2> LeftCornerMap;
    private readonly TimeCache<Vector2> UnderCornerMap;
    private bool IsForeGroundLast;
    private bool WasInGame;
    public PluginBridge PluginBridge;

    public GameController(
      ExileCore.Memory memory,
      SoundController soundController,
      SettingsContainer settings,
      MultiThreadManager multiThreadManager)
    {
      this._settings = settings.CoreSettings;
      this.Memory = (IMemory) memory;
      this.SoundController = soundController;
      this.Settings = settings;
      this.MultiThreadManager = multiThreadManager;
      try
      {
        this.Cache = new ExileCore.Shared.Cache.Cache();
        this.Game = new TheGame((IMemory) memory, this.Cache, settings.CoreSettings);
        this.Area = new AreaController(this.Game);
        this.Window = new GameWindow(memory.Process);
        this.WasInGame = this.Game.InGame;
        this.EntityListWrapper = new EntityListWrapper(this, this._settings, multiThreadManager);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
      this.PluginBridge = new PluginBridge();
      this.IsForeGroundCache = WinApi.IsForegroundWindow(this.Window.Process.MainWindowHandle);
      this.LeftPanel = new PluginPanel(this.GetLeftCornerMap());
      this.UnderPanel = new PluginPanel(this.GetUnderCornerMap());
      this.debDeltaTime = Core.DebugInformations.FirstOrDefault<DebugInformation>((Func<DebugInformation, bool>) (x => x.Name == "Delta Time"));
      this.LeftCornerMap = new TimeCache<Vector2>(new Func<Vector2>(this.GetLeftCornerMap), 500L);
      this.UnderCornerMap = new TimeCache<Vector2>(new Func<Vector2>(this.GetUnderCornerMap), 500L);
      GameController.eIsForegroundChanged += (Action<bool>) (b =>
      {
        if (b)
        {
          Core.MainRunner.ResumeCoroutines(Core.MainRunner.Coroutines);
          Core.ParallelRunner.ResumeCoroutines(Core.ParallelRunner.Coroutines);
        }
        else
        {
          Core.MainRunner.PauseCoroutines(Core.MainRunner.Coroutines);
          Core.ParallelRunner.PauseCoroutines(Core.ParallelRunner.Coroutines);
        }
      });
      this._settings.RefreshArea.OnPressed += (Action) (() => this.Area.ForceRefreshArea(false));
      this._settings.ReloadFiles.OnPressed += new Action(this.Game.ReloadFiles);
      this.Area.RefreshState();
      this.EntityListWrapper.StartWork();
      this.Initialized = true;
    }

    private Stopwatch sw { get; } = Stopwatch.StartNew();

    public long ElapsedMs => this.sw.ElapsedMilliseconds;

    public TheGame Game { get; }

    public AreaController Area { get; }

    public GameWindow Window { get; }

    public IngameState IngameState => this.Game.IngameState;

    public FilesContainer Files => this.Game.Files;

    public Entity Player => this.EntityListWrapper.Player;

    public bool IsForeGroundCache { get; set; }

    public bool InGame { get; private set; }

    public bool IsLoading { get; private set; }

    public PluginPanel LeftPanel { get; }

    public PluginPanel UnderPanel { get; }

    public IMemory Memory { get; }

    public SoundController SoundController { get; }

    public SettingsContainer Settings { get; }

    public MultiThreadManager MultiThreadManager { get; }

    public EntityListWrapper EntityListWrapper { get; }

    public ExileCore.Shared.Cache.Cache Cache { get; set; }

    public double DeltaTime => this.debDeltaTime.Tick;

    public bool Initialized { get; }

    public ICollection<Entity> Entities => this.EntityListWrapper.Entities;

    public Dictionary<string, object> Debug { get; } = new Dictionary<string, object>();

    public void Dispose() => this.Memory?.Dispose();

    public static event Action<bool> eIsForegroundChanged = _param1 => { };

    public void Tick()
    {
      try
      {
        if (this.IsForeGroundLast != this.IsForeGroundCache)
        {
          this.IsForeGroundLast = this.IsForeGroundCache;
          GameController.eIsForegroundChanged(this.IsForeGroundCache);
        }
        AreaInstance.CurrentHash = this.Game.CurrentAreaHash;
        if (this.LeftPanel.Used)
          this.LeftPanel.StartDrawPoint = this.LeftCornerMap.Value;
        if (this.UnderPanel.Used)
          this.UnderPanel.StartDrawPoint = this.UnderCornerMap.Value;
        if (Core.FramesCount % 3U == 0U && this.Area.RefreshState())
        {
          double num = (double) GameController.DebClearCache.TickAction((Action) (() => RemoteMemoryObject.Cache.TryClearCache()));
        }
        this.InGame = this.Game.InGame;
        this.IsLoading = this.Game.IsLoading;
        if (!this.InGame)
          return;
        if (!this.WasInGame)
        {
          this.Game.ReloadFiles();
          this.Game.IngameState.UpdateData();
          this.WasInGame = true;
        }
        CachedValue.Latency = (float) this.Game.IngameState.ServerData.Latency;
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
    }

    public Vector2 GetLeftCornerMap()
    {
      if (!this.InGame)
        return Vector2.Zero;
      IngameState ingameState = this.Game.IngameState;
      RectangleF getClientRectCache = ingameState.IngameUi.Map.SmallMiniMap.GetClientRectCache;
      Element mapSideUi = ingameState.IngameUi.MapSideUI;
      switch (this.Game.DiagnosticInfoType)
      {
        case DiagnosticInfoType.Off:
          if (mapSideUi != null && mapSideUi.IsVisibleLocal)
          {
            getClientRectCache.X -= mapSideUi.GetClientRectCache.Width;
            break;
          }
          break;
        case DiagnosticInfoType.Full:
          if (mapSideUi != null && mapSideUi.IsVisibleLocal)
            getClientRectCache.X -= mapSideUi.GetClientRectCache.Width;
          getClientRectCache.Y += 175f;
          break;
        case DiagnosticInfoType.Short:
          getClientRectCache.X -= 265f;
          break;
      }
      return new Vector2(getClientRectCache.X, getClientRectCache.Y);
    }

    private Vector2 GetUnderCornerMap()
    {
      if (!this.InGame)
        return Vector2.Zero;
      RectangleF getClientRectCache = this.Game.IngameState.IngameUi.GemLvlUpPanel.Parent.GetClientRectCache;
      return new Vector2(getClientRectCache.X + getClientRectCache.Width, getClientRectCache.Y + getClientRectCache.Height);
    }
  }
}
