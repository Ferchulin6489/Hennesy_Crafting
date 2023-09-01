// Decompiled with JetBrains decompiler
// Type: ExileCore.BaseSettingsPlugin`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.AtlasHelper;
using ExileCore.Shared.Interfaces;
using Newtonsoft.Json;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ExileCore
{
  public abstract class BaseSettingsPlugin<TSettings> : IPlugin, IDisposable where TSettings : ISettings, new()
  {
    private const string TEXTURES_FOLDER = "textures";
    private AtlasTexturesProcessor _atlasTextures;
    private PluginManager _pluginManager;

    protected BaseSettingsPlugin()
    {
      this.InternalName = this.GetType().Namespace;
      if (string.IsNullOrWhiteSpace(this.Name))
        this.Name = this.InternalName;
      this.Drawers = new List<ISettingsHolder>();
    }

    public List<ISettingsHolder> Drawers { get; }

    public GameController GameController { get; private set; }

    public Graphics Graphics { get; private set; }

    public TSettings Settings => (TSettings) this._Settings;

    public ISettings _Settings { get; private set; }

    public bool CanUseMultiThreading { get; protected set; }

    public string Description { get; protected set; }

    public string DirectoryName { get; set; }

    public string DirectoryFullName { get; set; }

    public bool Force { get; protected set; }

    public bool Initialized { get; set; }

    public string InternalName { get; }

    public string Name { get; set; }

    public int Order { get; protected set; }

    public string ConfigDirectory => this.GameController.Settings.GetPluginSettingsDirectory((IPlugin) this);

    public void _LoadSettings()
    {
      try
      {
        string str = this.GameController.Settings.LoadSettings((IPlugin) this);
        if (str != null)
          this._Settings = (ISettings) JsonConvert.DeserializeObject<TSettings>(str, SettingsContainer.jsonSettings);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
      if (this._Settings == null)
      {
        ISettings settings;
        this._Settings = settings = (ISettings) new TSettings();
      }
      SettingsParser.Parse(this._Settings, this.Drawers);
    }

    public void _SaveSettings()
    {
      if (this._Settings == null)
        throw new NullReferenceException("Plugin settings is null");
      this.GameController.Settings.SaveSettings((IPlugin) this);
    }

    public virtual void AreaChange(AreaInstance area)
    {
    }

    public virtual void Dispose() => this.OnClose();

    public virtual void DrawSettings()
    {
      foreach (ISettingsHolder drawer in this.Drawers)
        drawer.Draw();
    }

    public virtual void EntityAdded(Entity entity)
    {
    }

    public virtual void EntityAddedAny(Entity entity)
    {
    }

    public virtual void EntityIgnored(Entity entity)
    {
    }

    public virtual void EntityRemoved(Entity entity)
    {
    }

    public virtual void OnLoad()
    {
    }

    public virtual void OnUnload()
    {
    }

    public virtual bool Initialise() => true;

    public void LogMsg(string msg) => DebugWindow.LogMsg(msg);

    public virtual void OnClose() => this._SaveSettings();

    public virtual void ReceiveEvent(string eventId, object args)
    {
    }

    public void PublishEvent(string eventId, object args) => this._pluginManager.ReceivePluginEvent(eventId, args, (IPlugin) this);

    public virtual void OnPluginSelectedInMenu()
    {
    }

    public virtual Job Tick() => (Job) null;

    public virtual void Render()
    {
    }

    public void LogError(string msg, float time = 1f) => DebugWindow.LogError(msg, time);

    public void LogMessage(string msg, float time, Color clr) => DebugWindow.LogMsg(msg, time, clr);

    public void LogMessage(string msg, float time = 1f) => DebugWindow.LogMsg(msg, time, Color.GreenYellow);

    public virtual void OnPluginDestroyForHotReload()
    {
    }

    public void SetApi(
      GameController gameController,
      Graphics graphics,
      PluginManager pluginManager)
    {
      this.GameController = gameController;
      this.Graphics = graphics;
      this._pluginManager = pluginManager;
    }

    public AtlasTexture GetAtlasTexture(string textureName)
    {
      if (this._atlasTextures == null)
      {
        string path = Path.Combine(this.DirectoryFullName, "textures");
        string[] files = Directory.GetFiles(path, "*.json");
        if (files.Length == 0)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
          interpolatedStringHandler.AppendLiteral("Plugin '");
          interpolatedStringHandler.AppendFormatted(this.Name);
          interpolatedStringHandler.AppendLiteral("': Can't find atlas json config file in '");
          interpolatedStringHandler.AppendFormatted(path);
          interpolatedStringHandler.AppendLiteral("' ");
          this.LogError(interpolatedStringHandler.ToStringAndClear() + "(expecting config 'from Free texture packer' program)", 20f);
          this._atlasTextures = new AtlasTexturesProcessor("%AtlasNotFound%");
          return (AtlasTexture) null;
        }
        string withoutExtension = Path.GetFileNameWithoutExtension(files[0]);
        DefaultInterpolatedStringHandler interpolatedStringHandler1;
        if (files.Length > 1)
        {
          interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(54, 2);
          interpolatedStringHandler1.AppendLiteral("Plugin '");
          interpolatedStringHandler1.AppendFormatted(this.Name);
          interpolatedStringHandler1.AppendLiteral("': Found multiple atlas configs in folder '");
          interpolatedStringHandler1.AppendFormatted(path);
          interpolatedStringHandler1.AppendLiteral("', ");
          this.LogError(interpolatedStringHandler1.ToStringAndClear() + "selecting the first one ''" + withoutExtension + "''", 20f);
        }
        string str = Path.Combine(this.DirectoryFullName, "textures\\" + withoutExtension + ".png");
        if (!File.Exists(str))
        {
          interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(51, 2);
          interpolatedStringHandler1.AppendLiteral("Plugin '");
          interpolatedStringHandler1.AppendFormatted(this.Name);
          interpolatedStringHandler1.AppendLiteral("': Can't find atlas png texture file in '");
          interpolatedStringHandler1.AppendFormatted(str);
          interpolatedStringHandler1.AppendLiteral("' ");
          this.LogError(interpolatedStringHandler1.ToStringAndClear(), 20f);
          this._atlasTextures = new AtlasTexturesProcessor(withoutExtension);
          return (AtlasTexture) null;
        }
        this._atlasTextures = new AtlasTexturesProcessor(files[0], str);
        this.Graphics.InitImage(str, false);
      }
      return this._atlasTextures.GetTextureByName(textureName);
    }

    public AtlasTexturesProcessor CreateAtlas(string configPath, string texturePath)
    {
      if (!File.Exists(configPath))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 2);
        interpolatedStringHandler.AppendLiteral("Plugin '");
        interpolatedStringHandler.AppendFormatted(this.Name);
        interpolatedStringHandler.AppendLiteral("': Can't find atlas json config file in '");
        interpolatedStringHandler.AppendFormatted(configPath);
        interpolatedStringHandler.AppendLiteral("'");
        this.LogError(interpolatedStringHandler.ToStringAndClear(), 20f);
        return new AtlasTexturesProcessor("%AtlasNotFound%");
      }
      if (!File.Exists(texturePath))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
        interpolatedStringHandler.AppendLiteral("Plugin '");
        interpolatedStringHandler.AppendFormatted(this.Name);
        interpolatedStringHandler.AppendLiteral("': Can't find atlas png texture file in '");
        interpolatedStringHandler.AppendFormatted(texturePath);
        interpolatedStringHandler.AppendLiteral("' ");
        this.LogError(interpolatedStringHandler.ToStringAndClear(), 20f);
        return new AtlasTexturesProcessor("%AtlasNotFound%");
      }
      this.Graphics.InitImage(texturePath, false);
      return new AtlasTexturesProcessor(configPath, texturePath);
    }
  }
}
