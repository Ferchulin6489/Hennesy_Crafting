// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.PluginManager
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using MoreLinq.Extensions;
using SharpDX;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace ExileCore.Shared
{
  public class PluginManager
  {
    private const 
    #nullable disable
    string PluginsDirectory = "Plugins";
    private const string CompiledPluginsDirectory = "Compiled";
    private const string SourcePluginsDirectory = "Source";
    private const string TempPluginsDirectory = "Temp";
    private readonly GameController _gameController;
    private readonly Graphics _graphics;
    private readonly MultiThreadManager _multiThreadManager;
    private readonly bool _parallelLoading;
    private readonly Dictionary<string, string> _directories = new Dictionary<string, string>();
    private readonly ConcurrentDictionary<string, DateTime> _lastAssemblyLoadTime = new ConcurrentDictionary<string, DateTime>();
    private readonly object _locker = new object();

    public PluginManager(
      GameController gameController,
      Graphics graphics,
      MultiThreadManager multiThreadManager)
    {
      PluginManager pluginManager = this;
      this._gameController = gameController;
      this._graphics = graphics;
      this._multiThreadManager = multiThreadManager;
      this.RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
      this._directories["Temp"] = Path.Combine(this.RootDirectory, nameof (Plugins), "Temp");
      this._directories[nameof (Plugins)] = Path.Combine(this.RootDirectory, nameof (Plugins));
      this._directories["Compiled"] = Path.Combine(this._directories[nameof (Plugins)], "Compiled");
      this._directories["Source"] = Path.Combine(this._directories[nameof (Plugins)], "Source");
      this._gameController.EntityListWrapper.EntityAdded += new Action<Entity>(this.EntityListWrapperOnEntityAdded);
      this._gameController.EntityListWrapper.EntityRemoved += new Action<Entity>(this.EntityListWrapperOnEntityRemoved);
      this._gameController.EntityListWrapper.EntityAddedAny += new Action<Entity>(this.EntityListWrapperOnEntityAddedAny);
      this._gameController.EntityListWrapper.EntityIgnored += new Action<Entity>(this.EntityListWrapperOnEntityIgnored);
      this._gameController.Area.OnAreaChange += new Action<AreaInstance>(this.OnAreaChange);
      this._parallelLoading = (bool) this._gameController.Settings.CoreSettings.PluginSettings.MultiThreadLoadPlugins;
      foreach (KeyValuePair<string, string> directory in this._directories)
        Directory.CreateDirectory(directory.Value);
      (DirectoryInfo[] directoryInfoArray1, DirectoryInfo[] directoryInfoArray2) = this.SearchPlugins();
      Task task = (Task) null;
      if (directoryInfoArray2.Length != 0)
        task = Task.Run((Action) (() => closure_0.LoadPluginsFromSource((IEnumerable<DirectoryInfo>) directoryInfoArray2)));
      this.LoadCompiledPlugins(directoryInfoArray1, this._parallelLoading);
      task?.Wait();
      this.Plugins = this.Plugins.OrderBy<PluginWrapper, int>((Func<PluginWrapper, int>) (x => x.Order)).ThenByDescending<PluginWrapper, bool>((Func<PluginWrapper, bool>) (x => x.CanBeMultiThreading)).ThenBy<PluginWrapper, string>((Func<PluginWrapper, string>) (x => x.Name)).ToList<PluginWrapper>();
      PluginWrapper pluginWrapper = this.Plugins.FirstOrDefault<PluginWrapper>((Func<PluginWrapper, bool>) (x => x.Name.Equals("DevTree")));
      if (pluginWrapper != null)
      {
        try
        {
          pluginWrapper.Plugin.GetType().GetField(nameof (Plugins)).SetValue((object) pluginWrapper.Plugin, (object) new Func<List<PluginWrapper>>(devTreePlugins));
        }
        catch (Exception ex)
        {
          this.LogError(ex.ToString());
        }
      }
      if (this._parallelLoading)
      {
        IngameUIElements ingameUi = gameController.IngameState.IngameUi;
        IngameData data = gameController.IngameState.Data;
        ServerData serverData = gameController.IngameState.ServerData;
        Parallel.ForEach<PluginWrapper>((IEnumerable<PluginWrapper>) this.Plugins, (Action<PluginWrapper>) (wrapper => wrapper.Initialise(gameController)));
      }
      else
        this.Plugins.ForEach((Action<PluginWrapper>) (wrapper => wrapper.Initialise(gameController)));
      this.OnAreaChange(gameController.Area.CurrentArea);
      this.Plugins.DistinctBy<PluginWrapper, string>((Func<PluginWrapper, string>) (x => x.PathOnDisk)).ForEach<PluginWrapper>((Action<PluginWrapper>) (x => x.SubscrideOnFile(new Action<PluginWrapper, FileSystemEventArgs>(pluginManager.ReloadChangedDll))));
      this.AllPluginsLoaded = true;

      List<PluginWrapper> devTreePlugins() => pluginManager.Plugins;
    }

    public bool AllPluginsLoaded { get; }

    public string RootDirectory { get; }

    public List<PluginWrapper> Plugins { get; } = new List<PluginWrapper>();

    public ConcurrentDictionary<string, string> FailedSourcePlugins { get; } = new ConcurrentDictionary<string, string>();

    private void LoadCompiledPlugins(DirectoryInfo[] compiledPlugins, bool parallel)
    {
      if (parallel)
      {
        // ISSUE: method pointer
        Parallel.ForEach<DirectoryInfo>((IEnumerable<DirectoryInfo>) compiledPlugins, new Action<DirectoryInfo>((object) this, __methodptr(\u003CLoadCompiledPlugins\u003Eg__Load\u007C24_0)));
      }
      else
      {
        // ISSUE: method pointer
        ((IEnumerable<DirectoryInfo>) compiledPlugins).ForEach<DirectoryInfo>(new Action<DirectoryInfo>((object) this, __methodptr(\u003CLoadCompiledPlugins\u003Eg__Load\u007C24_0)));
      }
    }

    private PluginManager.LoadedAssembly LoadAssembly(
      DirectoryInfo dir,
      IEnumerable<string> suggestedDllNames)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler;
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(dir.FullName);
        if (!directoryInfo.Exists)
        {
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
          interpolatedStringHandler.AppendLiteral("Directory - ");
          interpolatedStringHandler.AppendFormatted<DirectoryInfo>(dir);
          interpolatedStringHandler.AppendLiteral(" not found.");
          this.LogError(interpolatedStringHandler.ToStringAndClear());
          return (PluginManager.LoadedAssembly) null;
        }
        FileInfo dll = ((IEnumerable<FileInfo>) directoryInfo.GetFiles(directoryInfo.Name + "*.dll", SearchOption.TopDirectoryOnly)).FirstOrDefault<FileInfo>();
        List<string> list1 = suggestedDllNames.Prepend<string>(directoryInfo.Name).ToList<string>();
        if (dll == null)
        {
          FileInfo[] files = directoryInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
          if (files.Length == 1)
          {
            dll = ((IEnumerable<FileInfo>) files).First<FileInfo>();
          }
          else
          {
            List<(FileInfo, string, string)> allDllsWithNames = ((IEnumerable<FileInfo>) files).Select<FileInfo, (FileInfo, string, string)>((Func<FileInfo, (FileInfo, string, string)>) (x => (x, Path.GetFileNameWithoutExtension(x.Name), Path.GetFileNameWithoutExtension(x.Name).Replace(" ", (string) null)))).ToList<(FileInfo, string, string)>();
            List<(FileInfo, string, string)> list2 = list1.Where<string>((Func<string, bool>) (x => !string.IsNullOrWhiteSpace(x))).SelectMany<string, (FileInfo, string, string)>((Func<string, IEnumerable<(FileInfo, string, string)>>) (targetName => allDllsWithNames.Where<(FileInfo, string, string)>((Func<(FileInfo, string, string), bool>) (x => x.fileName.Equals(targetName, StringComparison.InvariantCultureIgnoreCase) || x.noSpaceFileName.Equals(targetName.Replace(" ", (string) null), StringComparison.InvariantCultureIgnoreCase))))).Distinct<(FileInfo, string, string)>().ToList<(FileInfo, string, string)>();
            if (list2.Count == 1)
              dll = list2.First<(FileInfo, string, string)>().Item1;
          }
        }
        if (dll != null)
          return this.LoadAssembly(dll);
        this.LogError("Unable to find plugin dll in " + dir.FullName + ". Looked for names similar to " + string.Join(", ", (IEnumerable<string>) list1));
        return (PluginManager.LoadedAssembly) null;
      }
      catch (Exception ex)
      {
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
        interpolatedStringHandler.AppendFormatted(nameof (LoadAssembly));
        interpolatedStringHandler.AppendLiteral(" -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        this.LogError(interpolatedStringHandler.ToStringAndClear());
        return (PluginManager.LoadedAssembly) null;
      }
    }

    private PluginManager.LoadedAssembly LoadAssembly(FileInfo dll)
    {
      try
      {
        if ((dll != null ? (!dll.Exists ? 1 : 0) : 1) != 0)
          return (PluginManager.LoadedAssembly) null;
        PluginAssemblyLoadContext assemblyLoadContext = new PluginAssemblyLoadContext(dll.FullName, (bool) this._gameController.Settings.CoreSettings.PluginSettings.AvoidLockingDllFiles);
        if (!(bool) this._gameController.Settings.CoreSettings.PluginSettings.AvoidLockingDllFiles)
          return new PluginManager.LoadedAssembly(assemblyLoadContext.LoadFromAssemblyPath(dll.FullName), dll.FullName);
        using (FileStream assembly = File.OpenRead(dll.FullName))
        {
          string fullName = dll.FullName;
          int length = ".exe".Length;
          string path = fullName.Substring(0, fullName.Length - length) + ".pdb";
          using (FileStream assemblySymbols = File.Exists(path) ? File.OpenRead(path) : (FileStream) null)
            return new PluginManager.LoadedAssembly(assemblyLoadContext.LoadFromStream((Stream) assembly, (Stream) assemblySymbols), dll.FullName);
        }
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
        interpolatedStringHandler.AppendFormatted(nameof (LoadAssembly));
        interpolatedStringHandler.AppendLiteral(" -> ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        this.LogError(interpolatedStringHandler.ToStringAndClear());
        return (PluginManager.LoadedAssembly) null;
      }
    }

    private PluginManager.LoadedAssembly CompileAndLoadPluginAssembly(
      DirectoryInfo info,
      PluginCompiler compiler)
    {
      FileInfo csProj = ((IEnumerable<FileInfo>) info.GetFiles("*.csproj", SearchOption.AllDirectories)).FirstOrDefault<FileInfo>((Func<FileInfo, bool>) (f => !f.Name.Contains("test", StringComparison.OrdinalIgnoreCase) && !f.Name.Contains("_tmp", StringComparison.OrdinalIgnoreCase)));
      if (csProj == null)
      {
        DebugWindow.LogError("Plugin " + info.Name + " will not be compiled because there are no csproj files in the top-level directory");
        return (PluginManager.LoadedAssembly) null;
      }
      string str = Path.Join(this._directories["Temp"], info.Name);
      try
      {
        compiler.CompilePlugin(csProj, str);
      }
      catch (Exception ex)
      {
        File.WriteAllText(Path.Join(info.FullName, "Errors.txt"), ex.Message);
        Logger.Log.Error(ex, "Compilation of " + info.Name + " failed");
        this.FailedSourcePlugins[info.FullName] = ex.Message;
        return (PluginManager.LoadedAssembly) null;
      }
      return this.LoadAssembly(new DirectoryInfo(str), (IEnumerable<string>) new string[1]
      {
        Path.GetFileNameWithoutExtension(csProj.Name)
      });
    }

    private void LoadPluginsFromSource(IEnumerable<DirectoryInfo> sourcePlugins)
    {
      using (new PerformanceTimer("Compile and load source plugins"))
      {
        using (PluginCompiler compiler = PluginCompiler.Create())
        {
          if (compiler == null)
            this.LogError("Plugin compilation is disabled");
          else if (this._parallelLoading)
            Parallel.ForEach<DirectoryInfo>(sourcePlugins, new Action<DirectoryInfo>(CompileAndLoadPlugin));
          else
            sourcePlugins.ForEach<DirectoryInfo>(new Action<DirectoryInfo>(CompileAndLoadPlugin));

          void CompileAndLoadPlugin(DirectoryInfo directoryInfo)
          {
            using (new PerformanceTimer("Compile and load source plugin: " + directoryInfo.Name))
            {
              Stopwatch stopwatch = Stopwatch.StartNew();
              PluginManager.LoadedAssembly asm = this.CompileAndLoadPluginAssembly(directoryInfo, compiler);
              if (!(asm != (PluginManager.LoadedAssembly) null))
                return;
              List<PluginWrapper> collection = this.TryLoadPlugins(asm);
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
              interpolatedStringHandler.AppendLiteral("Plugins from directory ");
              interpolatedStringHandler.AppendFormatted(directoryInfo.Name);
              interpolatedStringHandler.AppendLiteral(" compiled and loaded in ");
              interpolatedStringHandler.AppendFormatted<long>(stopwatch.ElapsedMilliseconds);
              interpolatedStringHandler.AppendLiteral(" ms.");
              DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 1f, Color.Orange);
              lock (this._locker)
                this.Plugins.AddRange((IEnumerable<PluginWrapper>) collection);
            }
          }
        }
      }
    }

    public void LoadFailedSourcePlugin(string path)
    {
      this.FailedSourcePlugins.Remove<string, string>(path, out string _);
      this.LoadPluginsFromSource((IEnumerable<DirectoryInfo>) new DirectoryInfo[1]
      {
        new DirectoryInfo(path)
      });
    }

    private List<PluginWrapper> TryLoadPlugins(PluginManager.LoadedAssembly asm)
    {
      List<PluginWrapper> pluginWrapperList = new List<PluginWrapper>();
      try
      {
        DirectoryInfo directory = new FileInfo(asm.PathOnDisk).Directory;
        string fullName = directory.FullName;
        Type[] types = asm.Assembly.GetTypes();
        if (types.Length == 0)
        {
          this.LogError("Not found any types in plugin " + asm.PathOnDisk);
          return pluginWrapperList;
        }
        Type[] typeArray = types.WhereF<Type>((Func<Type, bool>) (type => typeof (IPlugin).IsAssignableFrom(type) && !type.IsAbstract));
        if (types.FirstOrDefaultF<Type>((Func<Type, bool>) (type => typeof (ISettings).IsAssignableFrom(type))) == (Type) null)
        {
          this.LogError("Not found setting class");
          return pluginWrapperList;
        }
        foreach (Type type in typeArray)
        {
          if (Activator.CreateInstance(type) is IPlugin instance)
          {
            instance.DirectoryName = directory.Name;
            instance.DirectoryFullName = fullName;
            PluginWrapper pluginWrapper = new PluginWrapper(instance, asm.PathOnDisk);
            pluginWrapper.SetApi(this._gameController, this._graphics, this);
            pluginWrapper.LoadSettings();
            pluginWrapper.Onload();
            pluginWrapperList.Add(pluginWrapper);
          }
        }
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
        interpolatedStringHandler.AppendLiteral("Error when load plugin (");
        interpolatedStringHandler.AppendFormatted(asm.Assembly.ManifestModule.ScopeName);
        interpolatedStringHandler.AppendLiteral("): ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        interpolatedStringHandler.AppendLiteral(")");
        this.LogError(interpolatedStringHandler.ToStringAndClear());
      }
      return pluginWrapperList;
    }

    private void ReloadChangedDll(PluginWrapper wrapper, FileSystemEventArgs args)
    {
      try
      {
        if ((args.ChangeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes) 0)
          return;
        string fullPath = args.FullPath;
        if (fullPath != wrapper.PathOnDisk || DateTime.UtcNow - this._lastAssemblyLoadTime.GetValueOrDefault<string, DateTime>(fullPath, DateTime.MinValue) < TimeSpan.FromSeconds(2.0))
          return;
        this._lastAssemblyLoadTime[fullPath] = DateTime.UtcNow;
        this.ReloadPluginDll(fullPath);
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
        interpolatedStringHandler.AppendLiteral("HotReload error: ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      }
    }

    private void ReloadPluginDll(string fullPath)
    {
      string pluginName = Path.GetFileNameWithoutExtension(fullPath);
      Core.MainRunner.Run(new Coroutine((Action) (() =>
      {
        PluginManager.LoadedAssembly asm = this.LoadAssembly(new FileInfo(fullPath));
        if (asm == (PluginManager.LoadedAssembly) null)
        {
          this.LogError(pluginName + " cant load assembly for reloading.");
        }
        else
        {
          List<PluginWrapper> pluginWrapperList = this.TryLoadPlugins(asm);
          if (!pluginWrapperList.Any<PluginWrapper>())
            return;
          foreach (PluginWrapper pluginWrapper in this.Plugins.Where<PluginWrapper>((Func<PluginWrapper, bool>) (x => x.PathOnDisk == fullPath)))
            pluginWrapper.Close();
          lock (this._locker)
            this.Plugins.RemoveAll((Predicate<PluginWrapper>) (x => x.PathOnDisk == fullPath));
          foreach (PluginWrapper pluginWrapper in pluginWrapperList)
          {
            pluginWrapper.Initialise(this._gameController);
            if (pluginWrapper.IsEnable)
              pluginWrapper.AreaChange(this._gameController.Area.CurrentArea);
            foreach (Entity entity in (IEnumerable<Entity>) this._gameController.Entities)
              pluginWrapper.EntityAdded(entity);
          }
          pluginWrapperList.First<PluginWrapper>().SubscrideOnFile(new Action<PluginWrapper, FileSystemEventArgs>(this.ReloadChangedDll));
          lock (this._locker)
            this.Plugins.AddRange((IEnumerable<PluginWrapper>) pluginWrapperList);
        }
      }), (IYieldBase) new WaitTime(1000), (IPlugin) null, "Reload: " + pluginName, false)
      {
        SyncModWork = true
      });
    }

    private (DirectoryInfo[] CompiledDirectories, DirectoryInfo[] SourceDirectories) SearchPlugins()
    {
      DirectoryInfo[] array1 = ((IEnumerable<DirectoryInfo>) new DirectoryInfo(this._directories["Compiled"]).GetDirectories()).Where<DirectoryInfo>((Func<DirectoryInfo, bool>) (x => x.EnumerateFiles("*.dll", SearchOption.AllDirectories).Any<FileInfo>())).ToArray<DirectoryInfo>();
      DirectoryInfo[] array2 = ((IEnumerable<DirectoryInfo>) new DirectoryInfo(this._directories["Source"]).GetDirectories()).Where<DirectoryInfo>((Func<DirectoryInfo, bool>) (x => (x.Attributes & FileAttributes.Hidden) == (FileAttributes) 0)).ToArray<DirectoryInfo>();
      if ((bool) this._gameController.Settings.CoreSettings.PluginSettings.PreferSourcePlugins)
        array1 = ((IEnumerable<DirectoryInfo>) array1).ExceptBy<DirectoryInfo, string>(((IEnumerable<DirectoryInfo>) array2).Select<DirectoryInfo, string>((Func<DirectoryInfo, string>) (x => x.Name)), (Func<DirectoryInfo, string>) (x => x.Name)).ToArray<DirectoryInfo>();
      else
        array2 = ((IEnumerable<DirectoryInfo>) array2).ExceptBy<DirectoryInfo, string>(((IEnumerable<DirectoryInfo>) array1).Select<DirectoryInfo, string>((Func<DirectoryInfo, string>) (x => x.Name)), (Func<DirectoryInfo, string>) (x => x.Name)).ToArray<DirectoryInfo>();
      return (array1, array2);
    }

    public void CloseAllPlugins()
    {
      foreach (PluginWrapper plugin in this.Plugins)
        plugin.Close();
    }

    private void OnAreaChange(AreaInstance area)
    {
      foreach (PluginWrapper plugin in this.Plugins)
      {
        if (plugin.IsEnable)
          plugin.AreaChange(area);
      }
    }

    private void EntityListWrapperOnEntityIgnored(Entity entity)
    {
      foreach (PluginWrapper plugin in this.Plugins)
      {
        if (plugin.IsEnable)
          plugin.EntityIgnored(entity);
      }
    }

    private void EntityListWrapperOnEntityAddedAny(Entity entity)
    {
      foreach (PluginWrapper plugin in this.Plugins)
      {
        if (plugin.IsEnable)
          plugin.EntityAddedAny(entity);
      }
    }

    private void EntityListWrapperOnEntityAdded(Entity entity)
    {
      if ((bool) this._gameController.Settings.CoreSettings.PerformanceSettings.AddedMultiThread && this._multiThreadManager.ThreadsCount > 0)
      {
        List<Job> listJob = new List<Job>();
        this.Plugins.WhereF<PluginWrapper>((Predicate<PluginWrapper>) (x => x.IsEnable)).Batch<PluginWrapper>(this._multiThreadManager.ThreadsCount).ForEach<IEnumerable<PluginWrapper>>((Action<IEnumerable<PluginWrapper>>) (wrappers => listJob.Add(this._multiThreadManager.AddJob((Action) (() => wrappers.ForEach<PluginWrapper>(closure_0 ?? (closure_0 = (Action<PluginWrapper>) (x => x.EntityAdded(entity))))), "Entity added"))));
        this._multiThreadManager.Process((object) this);
        SpinWait.SpinUntil((Func<bool>) (() => listJob.AllF<Job>((Predicate<Job>) (x => x.IsCompleted))), 500);
      }
      else
      {
        foreach (PluginWrapper plugin in this.Plugins)
        {
          if (plugin.IsEnable)
            plugin.EntityAdded(entity);
        }
      }
    }

    private void EntityListWrapperOnEntityRemoved(Entity entity)
    {
      foreach (PluginWrapper plugin in this.Plugins)
      {
        if (plugin.IsEnable)
          plugin.EntityRemoved(entity);
      }
    }

    private void LogError(string msg) => DebugWindow.LogError(msg, 5f);

    public void ReceivePluginEvent(string eventId, object args, IPlugin owner)
    {
      foreach (PluginWrapper plugin in this.Plugins)
      {
        if (plugin.IsEnable && plugin.Plugin != owner)
          plugin.ReceiveEvent(eventId, args);
      }
    }

    private record LoadedAssembly(Assembly Assembly, string PathOnDisk);
  }
}
