// Decompiled with JetBrains decompiler
// Type: ExileCore.MenuWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.RenderQ;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


#nullable enable
namespace ExileCore
{
  public class MenuWindow : IDisposable
  {
    private static readonly 
    #nullable disable
    Stopwatch swStartedProgram = Stopwatch.StartNew();
    private readonly SettingsContainer _settingsContainer;
    private readonly Core core;
    private int _index = -1;
    private DebugInformation AllPlugins;
    private readonly Action CoreSettings = (Action) (() => { });
    private static readonly DebugInformation DebugInformation = new DebugInformation("DebugWindow", false);
    private bool demo_window;
    private bool firstTime = true;
    private List<DebugInformation> MainDebugs = new List<DebugInformation>();
    private Action MoreInformation;
    private List<DebugInformation> NotMainDebugs = new List<DebugInformation>();
    private readonly Action OnWindowChange;
    private MenuWindow.Windows openWindow;
    private readonly int PluginNameWidth = 200;
    private List<DebugInformation> PluginsDebug = new List<DebugInformation>();
    private Action Selected = (Action) (() => { });
    private string selectedName = "";
    private readonly Stopwatch sw = Stopwatch.StartNew();
    private readonly ThemeEditor themeEditor;
    private readonly MenuWindow.Windows[] WindowsName;
    public static bool IsOpened;

    public MenuWindow(Core core, SettingsContainer settingsContainer)
    {
      MenuWindow menuWindow = this;
      this.core = core;
      this._settingsContainer = settingsContainer;
      this._CoreSettings = settingsContainer.CoreSettings;
      this.themeEditor = new ThemeEditor(this._CoreSettings);
      this.CoreSettingsDrawers = new List<ISettingsHolder>();
      SettingsParser.Parse((ISettings) this._CoreSettings, this.CoreSettingsDrawers);
      this.Selected = this.CoreSettings;
      this.CoreSettings = (Action) (() =>
      {
        foreach (ISettingsHolder coreSettingsDrawer in menuWindow.CoreSettingsDrawers)
          coreSettingsDrawer.Draw();
      });
      this._index = -1;
      this.Selected = this.CoreSettings;
      Core.DebugInformations.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnDebugInformationsOnCollectionChanged);
      this.OpenWindow = MenuWindow.Windows.MainDebugs;
      this.WindowsName = Enum.GetValues<MenuWindow.Windows>();
      this.OnWindowChange += (Action) (() =>
      {
        menuWindow.MoreInformation = (Action) null;
        menuWindow.selectedName = "";
      });
      Input.RegisterKey((Keys) this._CoreSettings.MainMenuKeyToggle);
      this._CoreSettings.MainMenuKeyToggle.OnValueChanged += (Action) (() => Input.RegisterKey((Keys) menuWindow._CoreSettings.MainMenuKeyToggle));
      this._CoreSettings.Enable.OnValueChanged += (EventHandler<bool>) ((sender, b) =>
      {
        if ((bool) menuWindow._CoreSettings.Enable)
          return;
        DefaultInterpolatedStringHandler interpolatedStringHandler;
        try
        {
          menuWindow._settingsContainer.SaveCoreSettings();
          foreach (PluginWrapper plugin in core.pluginManager.Plugins)
          {
            try
            {
              menuWindow._settingsContainer.SaveSettings(plugin.Plugin);
            }
            catch (Exception ex)
            {
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
              interpolatedStringHandler.AppendLiteral("SaveSettings for plugin error: ");
              interpolatedStringHandler.AppendFormatted<Exception>(ex);
              DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
            }
          }
        }
        catch (Exception ex)
        {
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
          interpolatedStringHandler.AppendLiteral("SaveSettings error: ");
          interpolatedStringHandler.AppendFormatted<Exception>(ex);
          DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        }
      });
    }

    public ExileCore.CoreSettings _CoreSettings { get; }

    public List<ISettingsHolder> CoreSettingsDrawers { get; }

    private MenuWindow.Windows OpenWindow
    {
      get => this.openWindow;
      set
      {
        if (this.openWindow == value)
          return;
        this.openWindow = value;
        Action onWindowChange = this.OnWindowChange;
        if (onWindowChange == null)
          return;
        onWindowChange();
      }
    }

    public void Dispose()
    {
      Core.DebugInformations.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnDebugInformationsOnCollectionChanged);
      this._settingsContainer.SaveCoreSettings();
    }

    private void OnDebugInformationsOnCollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs args)
    {
      if (args.Action != NotifyCollectionChangedAction.Add)
        return;
      if (this.firstTime)
      {
        this.MainDebugs = Core.DebugInformations.Where<DebugInformation>((Func<DebugInformation, bool>) (x => x.Main && !x.Name.EndsWith("[P]") && !x.Name.EndsWith("[R]"))).ToList<DebugInformation>();
        this.NotMainDebugs = Core.DebugInformations.Where<DebugInformation>((Func<DebugInformation, bool>) (x => !x.Main)).ToList<DebugInformation>();
        this.PluginsDebug = Core.DebugInformations.Where<DebugInformation>((Func<DebugInformation, bool>) (x => x.Name.EndsWith("[P]") || x.Name.EndsWith("[R]"))).ToList<DebugInformation>();
        this.firstTime = false;
      }
      else
      {
        foreach (DebugInformation newItem in (IEnumerable) args.NewItems)
        {
          if (newItem.Main && !newItem.Name.EndsWith("[P]") && !newItem.Name.EndsWith("[R]"))
            this.MainDebugs.Add(newItem);
          else if (newItem.Name.EndsWith("[P]") || newItem.Name.EndsWith("[R]"))
            this.PluginsDebug.Add(newItem);
          else
            this.NotMainDebugs.Add(newItem);
        }
      }
    }

    public void Render(GameController gameController)
    {
      PluginManager pluginManager = this.core.pluginManager;
      List<PluginWrapper> pluginWrapperList = (pluginManager != null ? pluginManager.Plugins.OrderBy<PluginWrapper, string>((Func<PluginWrapper, string>) (x => x.Name)).ToList<PluginWrapper>() : (List<PluginWrapper>) null) ?? new List<PluginWrapper>();
      if ((bool) this._CoreSettings.ShowDemoWindow)
      {
        this.demo_window = true;
        ImGui.ShowDemoWindow(ref this.demo_window);
        this._CoreSettings.ShowDemoWindow.Value = this.demo_window;
      }
      if (gameController != null)
        gameController.Memory.BackendMode = (bool) this._CoreSettings.UseNewMemoryBackend ? MemoryBackendMode.CacheAndPreload : MemoryBackendMode.AlwaysRead;
      if ((bool) this._CoreSettings.ShowDebugWindow)
      {
        double num1 = (double) MenuWindow.DebugInformation.TickAction(new Action(this.DebugWindowRender));
      }
      if (this._CoreSettings.MainMenuKeyToggle.PressedOnce())
      {
        this._CoreSettings.Enable.Value = !(bool) this._CoreSettings.Enable;
        if (!(bool) this._CoreSettings.Enable)
        {
          this._settingsContainer.SaveCoreSettings();
          if (gameController != null && this.core.pluginManager != null)
          {
            foreach (PluginWrapper plugin in this.core.pluginManager.Plugins)
              this._settingsContainer.SaveSettings(plugin.Plugin);
          }
        }
      }
      MenuWindow.IsOpened = (bool) this._CoreSettings.Enable;
      if (!(bool) this._CoreSettings.Enable)
        return;
      using (this.core.Graphics.UseCurrentFont())
      {
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(800f, 600f), ImGuiCond.FirstUseEver);
        bool p_open = this._CoreSettings.Enable.Value;
        ImGui.Begin("HUD S3ttings", ref p_open);
        this._CoreSettings.Enable.Value = p_open;
        ImGui.BeginChild("Left menu window", new System.Numerics.Vector2((float) this.PluginNameWidth, ImGui.GetContentRegionAvail().Y), true, ImGuiWindowFlags.None);
        if (ImGui.Selectable("Core", this._index == -1))
        {
          this._index = -1;
          this.Selected = this.CoreSettings;
        }
        ImGui.Separator();
        if (ImGui.Selectable("ThemeEditor", this._index == -2))
        {
          this._index = -2;
          this.Selected = new Action(this.themeEditor.DrawSettingsMenu);
        }
        if (gameController != null && this.core.pluginManager != null)
        {
          for (int index = 0; index < pluginWrapperList.Count; ++index)
          {
            PluginWrapper pluginWrapper = pluginWrapperList[index];
            bool isEnable = pluginWrapper.IsEnable;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
            interpolatedStringHandler.AppendLiteral("##");
            interpolatedStringHandler.AppendFormatted(pluginWrapper.Name);
            interpolatedStringHandler.AppendFormatted<int>(index);
            if (ImGui.Checkbox(interpolatedStringHandler.ToStringAndClear(), ref isEnable))
              pluginWrapper.TurnOnOffPlugin(isEnable);
            ImGui.SameLine();
            if (ImGui.Selectable(pluginWrapper.Name, this._index == index))
            {
              this._index = index;
              this.Selected = new Action(pluginWrapper.DrawSettings);
            }
          }
          int num2 = -2;
          foreach ((string str, string errText) in this.core.pluginManager.FailedSourcePlugins)
          {
            string pluginName = Path.GetFileName(str);
            ImGui.PushStyleColor(ImGuiCol.Text, Color.Red.ToImgui());
            bool selected = this._index == --num2;
            int num3 = ImGui.Selectable(pluginName, selected) ? 1 : 0;
            ImGui.PopStyleColor();
            int num4 = selected ? 1 : 0;
            if ((num3 | num4) != 0)
            {
              this._index = num2;
              this.Selected = (Action) (() => this.DrawFailedPluginMenu(str, pluginName, errText));
            }
          }
        }
        ImGui.EndChild();
        ImGui.SameLine();
        ImGui.BeginChild("Options", ImGui.GetContentRegionAvail(), true);
        Action selected1 = this.Selected;
        if (selected1 != null)
          selected1();
        ImGui.EndChild();
        ImGui.End();
      }
    }

    private void DrawFailedPluginMenu(string directory, string pluginName, string errText)
    {
      if (ImGui.Button("Try to rebuild"))
        this.core.pluginManager.LoadFailedSourcePlugin(directory);
      if (ImGui.Button("Open plugin folder"))
        Process.Start("explorer.exe", (IEnumerable<string>) new string[1]
        {
          directory
        });
      ImGui.Text("Plugin " + pluginName + " failed to compile with errors:");
      System.Numerics.Vector2 contentRegionAvail = ImGui.GetContentRegionAvail();
      float x = ImGui.CalcTextSize("A").X;
      int num = (int) ((double) contentRegionAvail.X / (double) x);
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
      interpolatedStringHandler.AppendLiteral("([^\\n]{1,");
      interpolatedStringHandler.AppendFormatted<int>(num);
      interpolatedStringHandler.AppendLiteral("}(?=\\s|$)|[^\\n]{");
      interpolatedStringHandler.AppendFormatted<int>(num);
      interpolatedStringHandler.AppendLiteral("})\\s*\\n?");
      string input = new Regex(interpolatedStringHandler.ToStringAndClear()).Replace(errText.ReplaceLineEndings("\n"), "$1\n");
      ImGui.InputTextMultiline("##err", ref input, 10000U, contentRegionAvail, ImGuiInputTextFlags.ReadOnly);
    }

    private void DebugWindowRender()
    {
      bool p_open = this._CoreSettings.ShowDebugWindow.Value;
      ImGui.Begin("Debug window", ref p_open);
      this._CoreSettings.ShowDebugWindow.Value = p_open;
      if (this.sw.ElapsedMilliseconds > 1000L)
        this.sw.Restart();
      ImGui.Text("Program work: ");
      ImGui.SameLine();
      ImGui.TextColored(Color.GreenYellow.ToImguiVec4(), MenuWindow.swStartedProgram.ElapsedMilliseconds.ToString());
      ImGui.BeginTabBar("Performance tabs");
      DefaultInterpolatedStringHandler interpolatedStringHandler1;
      foreach (MenuWindow.Windows windows in this.WindowsName)
      {
        interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(12, 1);
        interpolatedStringHandler1.AppendFormatted<MenuWindow.Windows>(windows);
        interpolatedStringHandler1.AppendLiteral("##WindowName");
        if (ImGui.BeginTabItem(interpolatedStringHandler1.ToStringAndClear()))
        {
          this.OpenWindow = windows;
          ImGui.EndTabItem();
        }
      }
      ImGui.EndTabBar();
      switch (this.OpenWindow)
      {
        case MenuWindow.Windows.MainDebugs:
          this.DrawMainDebugInfo(this.MainDebugs, Core.DebugInformations[0]);
          break;
        case MenuWindow.Windows.NotMainDebugs:
          if (ImGui.BeginTable("Deb", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.SizingFixedFit))
          {
            ImGui.TableSetupColumn("Name");
            ImGui.TableSetupColumn("Tick");
            ImGui.TableSetupColumn("Total");
            interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(14, 1);
            interpolatedStringHandler1.AppendLiteral("Data for ");
            interpolatedStringHandler1.AppendFormatted<int>(DebugInformation.SizeArray / (int) this._CoreSettings.PerformanceSettings.TargetFps);
            interpolatedStringHandler1.AppendLiteral(" sec.");
            ImGui.TableSetupColumn(interpolatedStringHandler1.ToStringAndClear());
            ImGui.TableHeadersRow();
            foreach (DebugInformation notMainDebug in this.NotMainDebugs)
            {
              ImGui.TableNextRow(ImGuiTableRowFlags.None, 24f);
              this.DrawInfoForNotMainDebugInformation(notMainDebug);
            }
            ImGui.EndTable();
            break;
          }
          break;
        case MenuWindow.Windows.Plugins:
          if (this.AllPlugins == null)
          {
            this.AllPlugins = Core.DebugInformations.FirstOrDefault<DebugInformation>((Func<DebugInformation, bool>) (x => x.Name == "All plugins"));
            break;
          }
          this.DrawMainDebugInfo(this.PluginsDebug, this.AllPlugins, new MenuWindow.MainDebugTableRecord(this.AllPlugins, Core.DebugInformations[0], this.MainDebugs.Count));
          break;
        case MenuWindow.Windows.Coroutines:
          this.DrawCoroutineRunnerInfo(this.core.CoroutineRunner);
          this.DrawCoroutineRunnerInfo(this.core.CoroutineRunnerParallel);
          if (ImGui.CollapsingHeader("Finished coroutines"))
          {
            foreach (CoroutineDetails finishedCoroutine in Core.MainRunner.FinishedCoroutines)
            {
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(12, 5);
              interpolatedStringHandler1.AppendFormatted(finishedCoroutine.Name);
              interpolatedStringHandler1.AppendLiteral(" - ");
              interpolatedStringHandler1.AppendFormatted<long>(finishedCoroutine.Ticks);
              interpolatedStringHandler1.AppendLiteral(" - ");
              interpolatedStringHandler1.AppendFormatted(finishedCoroutine.OwnerName);
              interpolatedStringHandler1.AppendLiteral(" - ");
              interpolatedStringHandler1.AppendFormatted<DateTime>(finishedCoroutine.Started);
              interpolatedStringHandler1.AppendLiteral(" - ");
              interpolatedStringHandler1.AppendFormatted<DateTime>(finishedCoroutine.Finished);
              ImGui.Text(interpolatedStringHandler1.ToStringAndClear());
            }
            using (List<CoroutineDetails>.Enumerator enumerator = Core.ParallelRunner.FinishedCoroutines.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                CoroutineDetails current = enumerator.Current;
                DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 5);
                interpolatedStringHandler2.AppendFormatted(current.Name);
                interpolatedStringHandler2.AppendLiteral(" - ");
                interpolatedStringHandler2.AppendFormatted<long>(current.Ticks);
                interpolatedStringHandler2.AppendLiteral(" - ");
                interpolatedStringHandler2.AppendFormatted(current.OwnerName);
                interpolatedStringHandler2.AppendLiteral(" - ");
                interpolatedStringHandler2.AppendFormatted<DateTime>(current.Started);
                interpolatedStringHandler2.AppendLiteral(" - ");
                interpolatedStringHandler2.AppendFormatted<DateTime>(current.Finished);
                ImGui.Text(interpolatedStringHandler2.ToStringAndClear());
              }
              break;
            }
          }
          else
            break;
        case MenuWindow.Windows.Caches:
          if (ImGui.BeginTable("Cache table", 6, ImGuiTableFlags.Borders))
          {
            ImGui.TableSetupColumn("Name");
            ImGui.TableSetupColumn("Count");
            ImGui.TableSetupColumn("Memory read");
            ImGui.TableSetupColumn("Cache read");
            ImGui.TableSetupColumn("Deleted");
            ImGui.TableSetupColumn("% Read from memory");
            ImGui.TableHeadersRow();
            ExileCore.Shared.Cache.Cache cache = this.core.GameController.Cache;
            (string, IStaticCache)[] valueTupleArray = new (string, IStaticCache)[6]
            {
              ("Elements", (IStaticCache) cache.StaticCacheElements),
              ("Components", (IStaticCache) cache.StaticCacheComponents),
              ("Entity", (IStaticCache) cache.StaticEntityCache),
              ("Entity list parse", (IStaticCache) cache.StaticEntityListCache),
              ("Server Entity list parse", (IStaticCache) cache.StaticServerEntityCache),
              ("String cache", (IStaticCache) cache.StringCache)
            };
            foreach ((string, IStaticCache) valueTuple in valueTupleArray)
            {
              ImGui.TableNextColumn();
              ImGui.Text(valueTuple.Item1);
              ImGui.TableNextColumn();
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler1.AppendFormatted<int>(valueTuple.Item2.Count);
              ImGui.Text(interpolatedStringHandler1.ToStringAndClear());
              ImGui.TableNextColumn();
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler1.AppendFormatted<int>(valueTuple.Item2.ReadMemory);
              ImGui.Text(interpolatedStringHandler1.ToStringAndClear());
              ImGui.TableNextColumn();
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler1.AppendFormatted<int>(valueTuple.Item2.ReadCache);
              ImGui.Text(interpolatedStringHandler1.ToStringAndClear());
              ImGui.TableNextColumn();
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler1.AppendFormatted<int>(valueTuple.Item2.DeletedCache);
              ImGui.Text(interpolatedStringHandler1.ToStringAndClear());
              ImGui.TableNextColumn();
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(3, 1);
              interpolatedStringHandler1.AppendFormatted<float>(valueTuple.Item2.Coeff);
              interpolatedStringHandler1.AppendLiteral(" %%");
              ImGui.Text(interpolatedStringHandler1.ToStringAndClear());
            }
            ImGui.EndTable();
            break;
          }
          break;
      }
      Action moreInformation = this.MoreInformation;
      if (moreInformation != null)
        moreInformation();
      ImGui.End();
    }

    private void DrawInfoForNotMainDebugInformation(DebugInformation deb)
    {
      ImGui.TableNextColumn();
      if (this.selectedName == deb.Name)
        ImGui.PushStyleColor(ImGuiCol.Text, Color.OrangeRed.ToImgui());
      ImGui.Text(deb.Name ?? "");
      if (ImGui.IsItemClicked() && deb.Index > 0)
        this.MoreInformation = (Action) (() => this.AddtionalInfo(deb));
      if (!string.IsNullOrEmpty(deb.Description))
      {
        ImGui.SameLine();
        ImGui.TextDisabled("(?)");
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.None))
          ImGui.SetTooltip(deb.Description);
      }
      ImGui.TableNextColumn();
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<double>(deb.Tick, "0.0000");
      ImGui.Text(interpolatedStringHandler.ToStringAndClear());
      ImGui.TableNextColumn();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<float>(deb.Total, "0.000");
      ImGui.TextUnformatted(interpolatedStringHandler.ToStringAndClear());
      ImGui.TableNextColumn();
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (deb.AtLeastOneFullTick)
      {
        num1 = deb.Ticks.AverageF();
        num2 = deb.Ticks.MinF();
      }
      else
      {
        float[] array = ((IEnumerable<float>) deb.Ticks).Take<float>(deb.Index).ToArray<float>();
        if (array.Length != 0)
        {
          num1 = ((IEnumerable<float>) array).Average();
          num2 = ((IEnumerable<float>) array).Min();
        }
      }
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 4);
      interpolatedStringHandler.AppendLiteral("Min: ");
      interpolatedStringHandler.AppendFormatted<float>(num2, "0.000");
      interpolatedStringHandler.AppendLiteral(" Max: ");
      interpolatedStringHandler.AppendFormatted<float>(deb.Ticks.MaxF(), "00.000");
      interpolatedStringHandler.AppendLiteral(" Avg: ");
      interpolatedStringHandler.AppendFormatted<float>(num1, "0.000");
      interpolatedStringHandler.AppendLiteral(" TAMax: ");
      interpolatedStringHandler.AppendFormatted<float>(deb.TotalMaxAverage, "00.000");
      ImGui.Text(interpolatedStringHandler.ToStringAndClear());
      if ((double) num1 >= (double) (float) this._CoreSettings.PerformanceSettings.LimitDrawPlot)
      {
        ImGui.SameLine();
        ImGui.PlotLines("##Plot" + deb.Name, ref deb.Ticks[0], DebugInformation.SizeArray);
      }
      if (!(this.selectedName == deb.Name))
        return;
      ImGui.PopStyleColor();
    }

    private void AddtionalInfo(DebugInformation deb)
    {
      this.selectedName = deb.Name;
      if (!deb.AtLeastOneFullTick)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 4);
        interpolatedStringHandler.AppendLiteral("Info ");
        interpolatedStringHandler.AppendFormatted(deb.Name);
        interpolatedStringHandler.AppendLiteral(" - ");
        interpolatedStringHandler.AppendFormatted<float>((float) (DebugInformation.SizeArray / (int) this._CoreSettings.PerformanceSettings.TargetFps) / 60f, "0.00");
        interpolatedStringHandler.AppendLiteral(" sec. Index: ");
        interpolatedStringHandler.AppendFormatted<int>(deb.Index);
        interpolatedStringHandler.AppendLiteral("/");
        interpolatedStringHandler.AppendFormatted<int>(DebugInformation.SizeArray);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        float num1 = ((IEnumerable<float>) deb.Ticks).Min();
        float num2 = ((IEnumerable<float>) deb.Ticks).Max();
        float windowWidth = ImGui.GetWindowWidth();
        string label = "##Plot" + deb.Name;
        ref float local = ref deb.Ticks[0];
        int sizeArray = DebugInformation.SizeArray;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
        interpolatedStringHandler.AppendLiteral("Avg: ");
        interpolatedStringHandler.AppendFormatted<float>(((IEnumerable<float>) deb.Ticks).Where<float>((Func<float, bool>) (x => (double) x > 0.0)).Average(), "0.0000");
        interpolatedStringHandler.AppendLiteral(" Max ");
        interpolatedStringHandler.AppendFormatted<float>(num2, "0.0000");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        double scale_min = (double) num1;
        double scale_max = (double) num2;
        System.Numerics.Vector2 graph_size = new System.Numerics.Vector2(windowWidth - 10f, 150f);
        ImGui.PlotHistogram(label, ref local, sizeArray, 0, stringAndClear, (float) scale_min, (float) scale_max, graph_size);
        if (!ImGui.Button("Close##" + deb.Name))
          return;
        this.MoreInformation = (Action) null;
      }
      else
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 4);
        interpolatedStringHandler.AppendLiteral("Info ");
        interpolatedStringHandler.AppendFormatted(deb.Name);
        interpolatedStringHandler.AppendLiteral(" - ");
        interpolatedStringHandler.AppendFormatted<float>((float) (DebugInformation.SizeArray * DebugInformation.SizeArray / (int) this._CoreSettings.PerformanceSettings.TargetFps) / 60f, "0.00");
        interpolatedStringHandler.AppendLiteral(" sec. Index: ");
        interpolatedStringHandler.AppendFormatted<int>(deb.Index);
        interpolatedStringHandler.AppendLiteral("/");
        interpolatedStringHandler.AppendFormatted<int>(DebugInformation.SizeArray);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        float num3 = deb.TicksAverage.MinF();
        float num4 = deb.TicksAverage.MaxF();
        float num5 = deb.Ticks.MaxF();
        float windowWidth = ImGui.GetWindowWidth();
        string label1 = "##Plot" + deb.Name;
        ref float local1 = ref deb.Ticks[0];
        int sizeArray1 = DebugInformation.SizeArray;
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<double>(deb.Tick, "0.000");
        string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
        double scale_max1 = (double) num5;
        System.Numerics.Vector2 graph_size1 = new System.Numerics.Vector2(windowWidth - 50f, 150f);
        ImGui.PlotHistogram(label1, ref local1, sizeArray1, 0, stringAndClear1, 0.0f, (float) scale_max1, graph_size1);
        float[] array = ((IEnumerable<float>) deb.TicksAverage).Where<float>((Func<float, bool>) (x => (double) x > 0.0)).ToArray<float>();
        if (array.Length != 0)
        {
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
          interpolatedStringHandler.AppendLiteral("Index: ");
          interpolatedStringHandler.AppendFormatted<int>(deb.IndexTickAverage);
          interpolatedStringHandler.AppendLiteral("/");
          interpolatedStringHandler.AppendFormatted<int>(DebugInformation.SizeArray);
          ImGui.Text(interpolatedStringHandler.ToStringAndClear());
          string label2 = "##Plot" + deb.Name;
          ref float local2 = ref deb.TicksAverage[0];
          int sizeArray2 = DebugInformation.SizeArray;
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
          interpolatedStringHandler.AppendLiteral("Avg: ");
          interpolatedStringHandler.AppendFormatted<float>(((IEnumerable<float>) array).Average(), "0.0000");
          interpolatedStringHandler.AppendLiteral(" Max ");
          interpolatedStringHandler.AppendFormatted<float>(num4, "0.0000");
          string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
          double scale_min = (double) num3;
          double scale_max2 = (double) num4;
          System.Numerics.Vector2 graph_size2 = new System.Numerics.Vector2(windowWidth - 50f, 150f);
          ImGui.PlotHistogram(label2, ref local2, sizeArray2, 0, stringAndClear2, (float) scale_min, (float) scale_max2, graph_size2);
        }
        else
          ImGui.Text("Dont have information");
        if (!ImGui.Button("Close##" + deb.Name))
          return;
        this.MoreInformation = (Action) null;
      }
    }

    private void DrawMainDebugInfo(
      List<DebugInformation> listSource,
      DebugInformation totalSource,
      MenuWindow.MainDebugTableRecord firstLine = null)
    {
      if (!ImGui.BeginTable("Deb", 6, ImGuiTableFlags.Borders | ImGuiTableFlags.Sortable | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.SortTristate))
        return;
      ImGui.TableSetupColumn("Name");
      ImGui.TableSetupColumn("%");
      ImGui.TableSetupColumn("Tick");
      ImGui.TableSetupColumn("Total");
      ImGui.TableSetupColumn("Total %");
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
      interpolatedStringHandler.AppendLiteral("Data for ");
      interpolatedStringHandler.AppendFormatted<int>(DebugInformation.SizeArray / (int) this._CoreSettings.PerformanceSettings.TargetFps);
      interpolatedStringHandler.AppendLiteral(" sec.");
      ImGui.TableSetupColumn(interpolatedStringHandler.ToStringAndClear());
      ImGui.TableHeadersRow();
      IEnumerable<MenuWindow.MainDebugTableRecord> source = listSource.Select<DebugInformation, MenuWindow.MainDebugTableRecord>((Func<DebugInformation, MenuWindow.MainDebugTableRecord>) (x => new MenuWindow.MainDebugTableRecord(x, totalSource, listSource.Count)));
      ImGuiTableSortSpecsPtr sortSpecs = ImGui.TableGetSortSpecs();
      if (sortSpecs.SpecsCount > 0)
      {
        ImGuiTableColumnSortSpecsPtr specs = sortSpecs.Specs;
        if (specs.SortDirection == ImGuiSortDirection.Ascending)
        {
          source = (IEnumerable<MenuWindow.MainDebugTableRecord>) source.OrderBy<MenuWindow.MainDebugTableRecord, object>(new Func<MenuWindow.MainDebugTableRecord, object>(SortSelector));
        }
        else
        {
          specs = sortSpecs.Specs;
          if (specs.SortDirection == ImGuiSortDirection.Descending)
            source = (IEnumerable<MenuWindow.MainDebugTableRecord>) source.OrderByDescending<MenuWindow.MainDebugTableRecord, object>(new Func<MenuWindow.MainDebugTableRecord, object>(SortSelector));
        }
      }
      if (firstLine != (MenuWindow.MainDebugTableRecord) null)
        source = source.Prepend<MenuWindow.MainDebugTableRecord>(firstLine);
      IEnumerable<MenuWindow.MainDebugTableRecord> list = (IEnumerable<MenuWindow.MainDebugTableRecord>) source.ToList<MenuWindow.MainDebugTableRecord>();
      float maxTotal = list.Max<MenuWindow.MainDebugTableRecord>((Func<MenuWindow.MainDebugTableRecord, float>) (x => x.Total));
      foreach (MenuWindow.MainDebugTableRecord debug in list)
      {
        ImGui.TableNextRow(ImGuiTableRowFlags.None, 24f);
        this.DrawMainDebugInfoLine(debug, maxTotal);
      }
      ImGui.EndTable();

      object SortSelector(MenuWindow.MainDebugTableRecord x)
      {
        short unmatchedValue = sortSpecs.Specs.ColumnIndex;
        object obj;
        switch (unmatchedValue)
        {
          case 0:
            obj = (object) x.Name;
            break;
          case 1:
            obj = (object) x.Percent;
            break;
          case 2:
            obj = (object) x.Tick;
            break;
          case 3:
            obj = (object) x.Total;
            break;
          case 4:
            obj = (object) x.TotalPercent;
            break;
          default:
            // ISSUE: reference to a compiler-generated method
            \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) unmatchedValue);
            break;
        }
        return obj;
      }
    }

    private void DrawMainDebugInfoLine(MenuWindow.MainDebugTableRecord debug, float maxTotal)
    {
      ImGui.TableNextColumn();
      ImGui.PushStyleColor(ImGuiCol.HeaderHovered, Color.Transparent.ToImgui());
      ImGui.PushStyleColor(ImGuiCol.HeaderActive, Color.Transparent.ToImgui());
      if (this.selectedName == debug.Name)
        ImGui.PushStyleColor(ImGuiCol.Text, Color.OrangeRed.ToImgui());
      ImGui.Selectable(debug.Name ?? "", false, ImGuiSelectableFlags.SpanAllColumns);
      if (ImGui.IsItemClicked())
        this.MoreInformation = (Action) (() => this.AddtionalInfo(debug.Current));
      if (!string.IsNullOrEmpty(debug.Current.Description))
      {
        ImGui.SameLine();
        ImGui.TextDisabled("(?)");
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.None))
          ImGui.SetTooltip(debug.Current.Description);
      }
      ImGui.TableNextColumn();
      float num1 = debug.Current.Ticks.AverageF();
      float num2 = num1 / debug.AllPluginAverage;
      System.Numerics.Vector4 imguiVec4 = ((double) num2 <= 0.5 ? Color.Green : ((double) num2 >= 4.0 ? Color.Red : ((double) num2 >= 1.5 ? Color.Orange : Color.Yellow))).ToImguiVec4();
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
      interpolatedStringHandler.AppendFormatted<float>(debug.Percent, "0.00");
      interpolatedStringHandler.AppendLiteral(" %%");
      ImGui.TextColored(imguiVec4, interpolatedStringHandler.ToStringAndClear().PadLeft(9));
      ImGui.TableNextColumn();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<double>(debug.Tick, "0.0000");
      ImGui.TextColored(imguiVec4, interpolatedStringHandler.ToStringAndClear().PadLeft(8));
      ImGui.TableNextColumn();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<float>(debug.Total, "0.000");
      ImGui.TextColored(imguiVec4, interpolatedStringHandler.ToStringAndClear().PadLeft((int) Math.Floor(Math.Log10((double) maxTotal)) + 5));
      ImGui.TableNextColumn();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
      interpolatedStringHandler.AppendFormatted<float>(debug.TotalPercent, "0.00");
      interpolatedStringHandler.AppendLiteral(" %%");
      ImGui.TextColored(imguiVec4, interpolatedStringHandler.ToStringAndClear().PadLeft(9));
      ImGui.TableNextColumn();
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 4);
      interpolatedStringHandler.AppendLiteral("Min: ");
      interpolatedStringHandler.AppendFormatted<float>(((IEnumerable<float>) debug.Current.Ticks).Min(), "0.000");
      interpolatedStringHandler.AppendLiteral(" Max: ");
      interpolatedStringHandler.AppendFormatted<float>(((IEnumerable<float>) debug.Current.Ticks).Max(), "00.000");
      interpolatedStringHandler.AppendLiteral(" Avg: ");
      interpolatedStringHandler.AppendFormatted<float>(num1, "0.000");
      interpolatedStringHandler.AppendLiteral(" TAMax: ");
      interpolatedStringHandler.AppendFormatted<float>(debug.Current.TotalMaxAverage, "00.000");
      ImGui.Text(interpolatedStringHandler.ToStringAndClear());
      if ((double) num1 >= (double) (float) this._CoreSettings.PerformanceSettings.LimitDrawPlot)
      {
        ImGui.SameLine();
        ImGui.PlotLines("##Plot" + debug.Name, ref debug.Current.Ticks[0], DebugInformation.SizeArray);
      }
      if (this.selectedName == debug.Name)
        ImGui.PopStyleColor();
      ImGui.PopStyleColor(2);
    }

    private void DrawCoroutineRunnerInfo(Runner runner)
    {
      ImGui.Text(runner.Name ?? "");
      if (!ImGui.BeginTable("CoroutineTable", 11, ImGuiTableFlags.Borders))
        return;
      ImGui.TableSetupColumn("Name");
      ImGui.TableSetupColumn("Owner");
      ImGui.TableSetupColumn("Ticks");
      ImGui.TableSetupColumn("Time ms");
      ImGui.TableSetupColumn("Started");
      ImGui.TableSetupColumn("Timeout");
      ImGui.TableSetupColumn("DoWork");
      ImGui.TableSetupColumn("AutoResume");
      ImGui.TableSetupColumn("Done");
      ImGui.TableSetupColumn("Priority");
      ImGui.TableSetupColumn("DO");
      ImGui.TableHeadersRow();
      foreach (Coroutine coroutine in runner.Coroutines.OrderByDescending<Coroutine, CoroutinePriority>((Func<Coroutine, CoroutinePriority>) (x => x.Priority)).ToList<Coroutine>())
      {
        ImGui.TableNextColumn();
        string str = "";
        if (coroutine.Condition != null)
          str = coroutine.Condition.GetType().Name;
        ImGui.Text(coroutine.Name ?? "");
        ImGui.TableNextColumn();
        ImGui.Text(coroutine.OwnerName ?? "");
        ImGui.TableNextColumn();
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<long>(coroutine.Ticks);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        ImGui.TableNextColumn();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<double>(Math.Round(runner.CoroutinePerformance.GetValueOrDefault<string, double>(coroutine.Name), 2));
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        ImGui.TableNextColumn();
        ImGui.Text(coroutine.Started.ToLongTimeString() ?? "");
        ImGui.TableNextColumn();
        ImGui.Text(str + ": " + coroutine.TimeoutForAction);
        ImGui.TableNextColumn();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<bool>(coroutine.Running);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        ImGui.TableNextColumn();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<bool>(coroutine.AutoResume);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        ImGui.TableNextColumn();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<bool>(coroutine.IsDone);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        ImGui.TableNextColumn();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<CoroutinePriority>(coroutine.Priority);
        ImGui.Text(interpolatedStringHandler.ToStringAndClear());
        ImGui.TableNextColumn();
        if (coroutine.Running)
        {
          if (ImGui.Button("Pause##" + coroutine.Name + "##" + runner.Name))
            coroutine.Pause();
        }
        else if (ImGui.Button("Start##" + coroutine.Name + "##" + runner.Name))
          coroutine.Resume();
        ImGui.SameLine();
        if (ImGui.Button("Done##" + coroutine.Name + "##" + runner.Name))
          coroutine.Done();
      }
      ImGui.EndTable();
    }

    private enum Windows
    {
      MainDebugs,
      NotMainDebugs,
      Plugins,
      Coroutines,
      Caches,
    }

    private record MainDebugTableRecord
    {
      public readonly string Name;
      public readonly float Percent;
      public readonly double Tick;
      public readonly float Total;
      public readonly float TotalPercent;
      public readonly float AllPluginAverage;

      public MainDebugTableRecord(
        DebugInformation Current,
        DebugInformation TotalDebug,
        int GroupCount)
      {
        // ISSUE: reference to a compiler-generated field
        this.\u003CCurrent\u003Ek__BackingField = Current;
        // ISSUE: reference to a compiler-generated field
        this.\u003CTotalDebug\u003Ek__BackingField = TotalDebug;
        // ISSUE: reference to a compiler-generated field
        this.\u003CGroupCount\u003Ek__BackingField = GroupCount;
        this.Name = Current.Name;
        this.Percent = (float) ((double) Current.Sum / (double) TotalDebug.Sum * 100.0);
        this.Tick = Current.Tick;
        this.Total = Current.Total;
        this.TotalPercent = (float) ((double) Current.Total / (double) TotalDebug.Total * 100.0);
        this.AllPluginAverage = TotalDebug.Average / (float) GroupCount;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      public DebugInformation Current { get; init; }

      public DebugInformation TotalDebug { get; init; }

      public int GroupCount { get; init; }

      [CompilerGenerated]
      protected virtual bool PrintMembers(
      #nullable enable
      StringBuilder builder)
      {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        builder.Append("Current = ");
        builder.Append((object) this.Current);
        builder.Append(", TotalDebug = ");
        builder.Append((object) this.TotalDebug);
        builder.Append(", GroupCount = ");
        builder.Append(this.GroupCount.ToString());
        builder.Append(", Name = ");
        builder.Append((object) this.Name);
        builder.Append(", Percent = ");
        builder.Append(this.Percent.ToString());
        builder.Append(", Tick = ");
        builder.Append(this.Tick.ToString());
        builder.Append(", Total = ");
        builder.Append(this.Total.ToString());
        builder.Append(", TotalPercent = ");
        builder.Append(this.TotalPercent.ToString());
        builder.Append(", AllPluginAverage = ");
        builder.Append(this.AllPluginAverage.ToString());
        return true;
      }
    }
  }
}
