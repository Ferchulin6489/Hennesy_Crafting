// Decompiled with JetBrains decompiler
// Type: ExileCore.RenderQ.ThemeEditor
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ExileCore.RenderQ
{
  public class ThemeEditor
  {
    public const string ThemeExtension = ".hudtheme";
    public const string DefaultThemeName = "Default";
    private const string ThemesFolder = "config/themes";
    private readonly CoreSettings coreSettings;
    private ThemeConfig LoadedTheme;
    private string NewThemeName = "MyNewTheme";
    private int SelectedThemeId;
    private string SelectedThemeName;

    public ThemeEditor(CoreSettings coreSettings)
    {
      this.coreSettings = coreSettings;
      ThemeEditor.GenerateDefaultTheme();
      if (!Directory.Exists("config/themes"))
      {
        Directory.CreateDirectory("config/themes");
        ThemeEditor.SaveTheme(ThemeEditor.GenerateDefaultTheme(), "Default");
        coreSettings.Theme.Value = "Default";
      }
      this.LoadThemeFilesList();
      this.SelectedThemeName = coreSettings.Theme.Value ?? coreSettings.Theme.Values.FirstOrDefault<string>();
      ThemeEditor.ApplyTheme(this.SelectedThemeName);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      coreSettings.Theme.OnValueSelected += ThemeEditor.\u003C\u003EO.\u003C0\u003E__ApplyTheme ?? (ThemeEditor.\u003C\u003EO.\u003C0\u003E__ApplyTheme = new Action<string>(ThemeEditor.ApplyTheme));
    }

    private void LoadThemeFilesList() => this.coreSettings.Theme.Values = ((IEnumerable<FileInfo>) new DirectoryInfo("config/themes").GetFiles("*.hudtheme")).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (x => x.LastWriteTime)).Select<FileInfo, string>((Func<FileInfo, string>) (x => Path.GetFileNameWithoutExtension(x.Name))).ToList<string>();

    public unsafe void DrawSettingsMenu()
    {
      if (ImGui.Combo("Select Theme", ref this.SelectedThemeId, this.coreSettings.Theme.Values.ToArray(), this.coreSettings.Theme.Values.Count) && this.SelectedThemeName != this.coreSettings.Theme.Values[this.SelectedThemeId])
      {
        this.SelectedThemeName = this.coreSettings.Theme.Values[this.SelectedThemeId];
        this.LoadedTheme = ThemeEditor.LoadTheme(this.coreSettings.Theme.Values[this.SelectedThemeId], false);
        ThemeEditor.ApplyTheme(this.LoadedTheme);
      }
      if (ImGui.Button("Save current theme settings to selected"))
        ThemeEditor.SaveTheme(this.ReadThemeFromCurrent(), this.SelectedThemeName);
      ImGui.Text("");
      ImGui.InputText("New theme name", ref this.NewThemeName, 200U, ImGuiInputTextFlags.None);
      if (ImGui.Button("Create new theme from current") && !string.IsNullOrEmpty(this.NewThemeName))
      {
        this.NewThemeName = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars())) + "]").Replace(this.NewThemeName, "");
        ThemeEditor.SaveTheme(this.ReadThemeFromCurrent(), this.NewThemeName);
        this.SelectedThemeName = this.NewThemeName;
        this.LoadThemeFilesList();
      }
      ImGui.Text("");
      ImGuiStylePtr style = ImGui.GetStyle();
      if (ImGui.TreeNode("Theme settings"))
      {
        style.AntiAliasedFill = this.DrawBoolSetting("AntiAliasedFill", style.AntiAliasedFill);
        style.AntiAliasedLines = this.DrawBoolSetting("AntiAliasedLines", style.AntiAliasedLines);
        style.Alpha = this.DrawFloatSetting("Alpha", style.Alpha * 100f, 0.0f, 100f) / 100f;
        style.DisplayWindowPadding = this.DrawVectorSetting("DisplayWindowPadding", style.DisplayWindowPadding, 0.0f, 20f);
        style.TouchExtraPadding = this.DrawVectorSetting("TouchExtraPadding", style.TouchExtraPadding, 0.0f, 10f);
        style.WindowPadding = this.DrawVectorSetting("WindowPadding", style.WindowPadding, 0.0f, 20f);
        style.FramePadding = this.DrawVectorSetting("FramePadding", style.FramePadding, 0.0f, 20f);
        style.DisplaySafeAreaPadding = this.DrawVectorSetting("DisplaySafeAreaPadding", style.DisplaySafeAreaPadding, 0.0f, 20f);
        style.ItemInnerSpacing = this.DrawVectorSetting("ItemInnerSpacing", style.ItemInnerSpacing, 0.0f, 20f);
        style.ItemSpacing = this.DrawVectorSetting("ItemSpacing", style.ItemSpacing, 0.0f, 20f);
        style.GrabMinSize = this.DrawFloatSetting("GrabMinSize", style.GrabMinSize, 0.0f, 20f);
        style.GrabRounding = this.DrawFloatSetting("GrabRounding", style.GrabRounding, 0.0f, 12f);
        style.IndentSpacing = this.DrawFloatSetting("IndentSpacing", style.IndentSpacing, 0.0f, 30f);
        style.ScrollbarRounding = this.DrawFloatSetting("ScrollbarRounding", style.ScrollbarRounding, 0.0f, 19f);
        style.ScrollbarSize = this.DrawFloatSetting("ScrollbarSize", style.ScrollbarSize, 0.0f, 20f);
        style.WindowTitleAlign = this.DrawVectorSetting("WindowTitleAlign", style.WindowTitleAlign, 0.0f, 1f);
        style.WindowRounding = this.DrawFloatSetting("WindowRounding", style.WindowRounding, 0.0f, 14f);
        style.ChildRounding = this.DrawFloatSetting("ChildWindowRounding", style.ChildRounding, 0.0f, 16f);
        style.FrameRounding = this.DrawFloatSetting("FrameRounding", style.FrameRounding, 0.0f, 12f);
        style.ColumnsMinSpacing = this.DrawFloatSetting("ColumnsMinSpacing", style.ColumnsMinSpacing, 0.0f, 30f);
        style.CurveTessellationTol = this.DrawFloatSetting("CurveTessellationTolerance", style.CurveTessellationTol * 100f, 0.0f, 100f) / 100f;
      }
      ImGui.Text("");
      ImGui.Text("Colors:");
      ImGui.Columns(2, "Columns", true);
      ImGuiCol[] values = Enum.GetValues<ImGuiCol>();
      int num = values.Length / 2;
      foreach (ImGuiCol idx in values)
      {
        string label = Regex.Replace(idx.ToString(), "(\\B[A-Z])", " $1");
        Vector4* styleColorVec4 = ImGui.GetStyleColorVec4(idx);
        Vector4 col = new Vector4(styleColorVec4->X, styleColorVec4->Y, styleColorVec4->Z, styleColorVec4->W);
        ref Vector4 local = ref col;
        if (ImGui.ColorEdit4(label, ref local, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaBar | ImGuiColorEditFlags.AlphaPreviewHalf))
          ImGui.PushStyleColor(idx, col);
        if (num-- == -1)
          ImGui.NextColumn();
      }
    }

    private bool DrawBoolSetting(string name, bool result)
    {
      ImGui.Checkbox(name, ref result);
      return result;
    }

    private float DrawFloatSetting(string name, float result, float min, float max)
    {
      ImGui.SliderFloat(name, ref result, min, max, "%.0f");
      return result;
    }

    private Vector2 DrawVectorSetting(string name, Vector2 result, float min, float max)
    {
      ImGui.SliderFloat2(name, ref result, min, max, "%.0f");
      return result;
    }

    public static void ApplyTheme(string fileName)
    {
      ThemeConfig theme = ThemeEditor.LoadTheme(fileName, true);
      if (theme == null)
      {
        DebugWindow.LogMsg("Can't find theme file " + fileName + ", loading default.", 3f);
        theme = ThemeEditor.LoadTheme("Default", true);
        if (theme == null)
        {
          DebugWindow.LogMsg("Can't find default theme file Default, Generating default and saving...", 3f);
          theme = ThemeEditor.GenerateDefaultTheme();
          ThemeEditor.SaveTheme(theme, "Default");
        }
      }
      ThemeEditor.ApplyTheme(theme);
    }

    public static void ApplyTheme(ThemeConfig theme)
    {
      ImGuiStylePtr style = ImGui.GetStyle();
      style.AntiAliasedLines = theme.AntiAliasedLines;
      style.DisplaySafeAreaPadding = theme.DisplaySafeAreaPadding;
      style.DisplayWindowPadding = theme.DisplayWindowPadding;
      style.GrabRounding = theme.GrabRounding;
      style.GrabMinSize = theme.GrabMinSize;
      style.ScrollbarRounding = theme.ScrollbarRounding;
      style.ScrollbarSize = theme.ScrollbarSize;
      style.ColumnsMinSpacing = theme.ColumnsMinSpacing;
      style.IndentSpacing = theme.IndentSpacing;
      style.TouchExtraPadding = theme.TouchExtraPadding;
      style.ItemInnerSpacing = theme.ItemInnerSpacing;
      style.ItemSpacing = theme.ItemSpacing;
      style.FrameRounding = theme.FrameRounding;
      style.FramePadding = theme.FramePadding;
      style.ChildRounding = theme.ChildWindowRounding;
      style.WindowTitleAlign = theme.WindowTitleAlign;
      style.WindowRounding = theme.WindowRounding;
      style.WindowPadding = theme.WindowPadding;
      style.Alpha = theme.Alpha;
      style.AntiAliasedFill = theme.AntiAliasedFill;
      style.CurveTessellationTol = theme.CurveTessellationTolerance;
      foreach (KeyValuePair<ImGuiCol, Vector4> color in theme.Colors)
      {
        try
        {
          if (color.Key != ImGuiCol.COUNT)
            ImGui.PushStyleColor(color.Key, color.Value);
        }
        catch (Exception ex)
        {
          DebugWindow.LogError(ex.Message, 5f);
        }
      }
    }

    private unsafe ThemeConfig ReadThemeFromCurrent()
    {
      ImGuiStylePtr style = ImGui.GetStyle();
      ThemeConfig themeConfig = new ThemeConfig()
      {
        AntiAliasedLines = style.AntiAliasedLines,
        DisplaySafeAreaPadding = style.DisplaySafeAreaPadding,
        DisplayWindowPadding = style.DisplayWindowPadding,
        GrabRounding = style.GrabRounding,
        GrabMinSize = style.GrabMinSize,
        ScrollbarRounding = style.ScrollbarRounding,
        ScrollbarSize = style.ScrollbarSize,
        ColumnsMinSpacing = style.ColumnsMinSpacing,
        IndentSpacing = style.IndentSpacing,
        TouchExtraPadding = style.TouchExtraPadding,
        ItemInnerSpacing = style.ItemInnerSpacing,
        ItemSpacing = style.ItemSpacing,
        FrameRounding = style.FrameRounding,
        FramePadding = style.FramePadding,
        ChildWindowRounding = style.ChildRounding,
        WindowTitleAlign = style.WindowTitleAlign,
        WindowRounding = style.WindowRounding,
        WindowPadding = style.WindowPadding,
        Alpha = style.Alpha,
        AntiAliasedFill = style.AntiAliasedFill,
        CurveTessellationTolerance = style.CurveTessellationTol
      };
      foreach (ImGuiCol imGuiCol in Enum.GetValues<ImGuiCol>())
      {
        if (imGuiCol != ImGuiCol.COUNT)
        {
          Vector4* styleColorVec4 = ImGui.GetStyleColorVec4(imGuiCol);
          themeConfig.Colors.Add(imGuiCol, new Vector4(styleColorVec4->X, styleColorVec4->Y, styleColorVec4->Z, styleColorVec4->W));
        }
      }
      return themeConfig;
    }

    private static ThemeConfig GenerateDefaultTheme() => new ThemeConfig()
    {
      Colors = {
        {
          ImGuiCol.Text,
          new Vector4(0.9f, 0.9f, 0.9f, 1f)
        },
        {
          ImGuiCol.TextDisabled,
          new Vector4(0.6f, 0.6f, 0.6f, 1f)
        },
        {
          ImGuiCol.WindowBg,
          new Vector4(0.16f, 0.16f, 0.16f, 1f)
        },
        {
          ImGuiCol.ChildBg,
          new Vector4(0.12f, 0.12f, 0.12f, 1f)
        },
        {
          ImGuiCol.PopupBg,
          new Vector4(0.11f, 0.11f, 0.14f, 0.92f)
        },
        {
          ImGuiCol.Border,
          new Vector4(0.44f, 0.44f, 0.44f, 1f)
        },
        {
          ImGuiCol.BorderShadow,
          new Vector4(0.0f, 0.0f, 0.0f, 0.0f)
        },
        {
          ImGuiCol.FrameBg,
          new Vector4(0.2f, 0.2f, 0.2f, 1f)
        },
        {
          ImGuiCol.FrameBgHovered,
          new Vector4(0.98f, 0.61f, 0.26f, 1f)
        },
        {
          ImGuiCol.FrameBgActive,
          new Vector4(0.74f, 0.36f, 0.02f, 1f)
        },
        {
          ImGuiCol.TitleBg,
          new Vector4(0.4f, 0.19f, 0.0f, 1f)
        },
        {
          ImGuiCol.TitleBgActive,
          new Vector4(0.74f, 0.36f, 0.0f, 1f)
        },
        {
          ImGuiCol.TitleBgCollapsed,
          new Vector4(0.75f, 0.37f, 0.0f, 1f)
        },
        {
          ImGuiCol.MenuBarBg,
          new Vector4(0.29f, 0.29f, 0.3f, 1f)
        },
        {
          ImGuiCol.ScrollbarBg,
          new Vector4(0.28f, 0.28f, 0.28f, 1f)
        },
        {
          ImGuiCol.ScrollbarGrab,
          new Vector4(0.71f, 0.37f, 0.0f, 1f)
        },
        {
          ImGuiCol.ScrollbarGrabHovered,
          new Vector4(0.86f, 0.41f, 0.06f, 1f)
        },
        {
          ImGuiCol.ScrollbarGrabActive,
          new Vector4(0.64f, 0.29f, 0.0f, 1f)
        },
        {
          ImGuiCol.CheckMark,
          new Vector4(0.96f, 0.45f, 0.01f, 1f)
        },
        {
          ImGuiCol.SliderGrab,
          new Vector4(0.86f, 0.48f, 0.0f, 1f)
        },
        {
          ImGuiCol.SliderGrabActive,
          new Vector4(0.52f, 0.31f, 0.0f, 1f)
        },
        {
          ImGuiCol.Button,
          new Vector4(0.73f, 0.37f, 0.0f, 1f)
        },
        {
          ImGuiCol.ButtonHovered,
          new Vector4(0.97f, 0.57f, 0.0f, 1f)
        },
        {
          ImGuiCol.ButtonActive,
          new Vector4(0.62f, 0.29f, 0.01f, 1f)
        },
        {
          ImGuiCol.Header,
          new Vector4(0.59f, 0.28f, 0.0f, 1f)
        },
        {
          ImGuiCol.HeaderHovered,
          new Vector4(0.74f, 0.35f, 0.02f, 1f)
        },
        {
          ImGuiCol.HeaderActive,
          new Vector4(0.88f, 0.45f, 0.0f, 1f)
        },
        {
          ImGuiCol.Separator,
          new Vector4(0.5f, 0.5f, 0.5f, 1f)
        },
        {
          ImGuiCol.SeparatorHovered,
          new Vector4(0.6f, 0.6f, 0.7f, 1f)
        },
        {
          ImGuiCol.SeparatorActive,
          new Vector4(0.7f, 0.7f, 0.9f, 1f)
        },
        {
          ImGuiCol.ResizeGrip,
          new Vector4(1f, 1f, 1f, 0.16f)
        },
        {
          ImGuiCol.ResizeGripHovered,
          new Vector4(0.78f, 0.82f, 1f, 0.6f)
        },
        {
          ImGuiCol.ResizeGripActive,
          new Vector4(0.78f, 0.82f, 1f, 0.9f)
        },
        {
          ImGuiCol.PlotLines,
          new Vector4(1f, 1f, 1f, 1f)
        },
        {
          ImGuiCol.PlotLinesHovered,
          new Vector4(0.9f, 0.7f, 0.0f, 1f)
        },
        {
          ImGuiCol.PlotHistogram,
          new Vector4(0.9f, 0.7f, 0.0f, 1f)
        },
        {
          ImGuiCol.PlotHistogramHovered,
          new Vector4(1f, 0.6f, 0.0f, 1f)
        },
        {
          ImGuiCol.TextSelectedBg,
          new Vector4(1f, 0.03f, 0.03f, 0.35f)
        },
        {
          ImGuiCol.ModalWindowDimBg,
          new Vector4(0.2f, 0.2f, 0.2f, 0.35f)
        },
        {
          ImGuiCol.DragDropTarget,
          new Vector4(1f, 1f, 0.0f, 0.9f)
        }
      }
    };

    private static ThemeConfig LoadTheme(string fileName, bool nullIfNotFound)
    {
      try
      {
        string path = Path.Combine("config/themes", fileName + ".hudtheme");
        if (File.Exists(path))
          return JsonConvert.DeserializeObject<ThemeConfig>(File.ReadAllText(path), SettingsContainer.jsonSettings);
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
        interpolatedStringHandler.AppendLiteral("Error while loading theme ");
        interpolatedStringHandler.AppendFormatted(fileName);
        interpolatedStringHandler.AppendLiteral(": ");
        interpolatedStringHandler.AppendFormatted(ex.Message);
        interpolatedStringHandler.AppendLiteral(", Generating default one");
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear(), 3f);
      }
      return nullIfNotFound ? (ThemeConfig) null : ThemeEditor.GenerateDefaultTheme();
    }

    private static void SaveTheme(ThemeConfig theme, string fileName)
    {
      try
      {
        string path = Path.Combine("config/themes", fileName + ".hudtheme");
        string directoryName = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        using (StreamWriter streamWriter = new StreamWriter((Stream) File.Create(path)))
        {
          string str = JsonConvert.SerializeObject((object) theme, Formatting.Indented, SettingsContainer.jsonSettings);
          streamWriter.Write(str);
        }
      }
      catch (Exception ex)
      {
        DebugWindow.LogError("Error while loading theme: " + ex.Message, 3f);
      }
    }
  }
}
