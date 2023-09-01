// Decompiled with JetBrains decompiler
// Type: ExileCore.SettingsContainer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ExileCore
{
  public class SettingsContainer
  {
    private const string SETTINGS_FILE_NAME = "settings.json";
    private const string DEFAULT_PROFILE_NAME = "global";
    private const string CFG_DIR_NAME = "config";
    private static readonly string CfgDirectoryPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "config");
    private static readonly string SettingsFilePath = Path.Join(SettingsContainer.CfgDirectoryPath, "settings.json");
    public static readonly JsonSerializerSettings jsonSettings;
    private string _currentProfileName = "";
    public CoreSettings CoreSettings;

    static SettingsContainer() => SettingsContainer.jsonSettings = new JsonSerializerSettings()
    {
      ContractResolver = (IContractResolver) new SortContractResolver(),
      Converters = (IList<JsonConverter>) new JsonConverter[3]
      {
        (JsonConverter) new ColorNodeConverter(),
        (JsonConverter) new ToggleNodeConverter(),
        (JsonConverter) new FileNodeConverter()
      }
    };

    public SettingsContainer()
    {
      Directory.CreateDirectory(Path.Join(SettingsContainer.CfgDirectoryPath, "global"));
      this.LoadCoreSettings();
    }

    private static ReaderWriterLockSlim rwLock { get; } = new ReaderWriterLockSlim();

    private string CurrentProfileName
    {
      get => this._currentProfileName;
      set
      {
        EventHandler<string> onProfileChange = this.OnProfileChange;
        if (onProfileChange != null)
          onProfileChange((object) this, value);
        this._currentProfileName = value;
      }
    }

    public event EventHandler<string> OnProfileChange;

    public void LoadCoreSettings()
    {
      if (File.Exists(SettingsContainer.SettingsFilePath))
      {
        try
        {
          this.CoreSettings = JsonConvert.DeserializeObject<CoreSettings>(File.ReadAllText(SettingsContainer.SettingsFilePath));
          this.CurrentProfileName = this.CoreSettings.Profiles.Value;
          return;
        }
        catch (Exception ex)
        {
          DebugWindow.LogError(ex.ToString());
        }
      }
      CoreSettings coreSettings = new CoreSettings();
      File.WriteAllText(SettingsContainer.SettingsFilePath, JsonConvert.SerializeObject((object) coreSettings, Formatting.Indented));
      this.CoreSettings = coreSettings;
      this.CurrentProfileName = this.CoreSettings.Profiles.Value;
    }

    public void SaveCoreSettings()
    {
      SettingsContainer.rwLock.EnterWriteLock();
      try
      {
        string contents = JsonConvert.SerializeObject((object) this.CoreSettings, Formatting.Indented);
        if (new FileInfo(SettingsContainer.SettingsFilePath).Length > 1L)
          File.Copy(SettingsContainer.SettingsFilePath, Path.Join(SettingsContainer.CfgDirectoryPath, "dumpSettings.json"), true);
        File.WriteAllText(SettingsContainer.SettingsFilePath, contents);
      }
      catch (Exception ex)
      {
        DebugWindow.LogError(ex.ToString());
      }
      finally
      {
        SettingsContainer.rwLock.ExitWriteLock();
      }
    }

    public void SaveSettings(IPlugin plugin)
    {
      if (plugin == null)
        return;
      if (string.IsNullOrWhiteSpace(this.CurrentProfileName))
        this.CurrentProfileName = "global";
      SettingsContainer.rwLock.EnterWriteLock();
      try
      {
        Directory.CreateDirectory(Path.Join(SettingsContainer.CfgDirectoryPath, this.CurrentProfileName));
        File.WriteAllText(Path.Join(SettingsContainer.CfgDirectoryPath, this.CurrentProfileName, plugin.InternalName + "_settings.json"), JsonConvert.SerializeObject((object) plugin._Settings, Formatting.Indented, SettingsContainer.jsonSettings));
      }
      finally
      {
        SettingsContainer.rwLock.ExitWriteLock();
      }
    }

    public string LoadSettings(IPlugin plugin)
    {
      if (!Directory.Exists(Path.Join(SettingsContainer.CfgDirectoryPath, this.CurrentProfileName)))
        throw new DirectoryNotFoundException(this.CurrentProfileName + " not found in " + SettingsContainer.CfgDirectoryPath);
      string path = Path.Join(SettingsContainer.CfgDirectoryPath, this.CurrentProfileName, plugin.InternalName + "_settings.json");
      if (!File.Exists(path))
        return (string) null;
      string str = File.ReadAllText(path);
      return str.Length != 0 ? str : (string) null;
    }

    public string GetPluginSettingsDirectory(IPlugin plugin) => Directory.CreateDirectory(Path.Join(SettingsContainer.CfgDirectoryPath, plugin.InternalName)).FullName;

    public static TSettingType LoadSettingFile<TSettingType>(string fileName)
    {
      if (File.Exists(fileName))
        return JsonConvert.DeserializeObject<TSettingType>(File.ReadAllText(fileName));
      Logger.Log.Error("Cannot find file '" + fileName + "'.");
      return default (TSettingType);
    }

    public static void SaveSettingFile<TSettingType>(string fileName, TSettingType setting)
    {
      string contents = JsonConvert.SerializeObject((object) setting, Formatting.Indented);
      File.WriteAllText(fileName, contents);
    }
  }
}
