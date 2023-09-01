// Decompiled with JetBrains decompiler
// Type: ExileCore.CoreSettings
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ClickableTransparentOverlay;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExileCore
{
  public class CoreSettings : ISettings
  {
    [Menu("Refresh area")]
    [JsonIgnore]
    public ButtonNode RefreshArea { get; set; } = new ButtonNode();

    [JsonIgnore]
    public ButtonNode ReloadFiles { get; set; } = new ButtonNode();

    [Menu(null, "Uses more memory, should be faster")]
    public ToggleNode UseNewMemoryBackend { get; set; } = new ToggleNode(true);

    [Menu("List profiles", "Currently not works. Soon.")]
    public ListNode Profiles { get; set; } = new ListNode()
    {
      Values = new List<string>() { "global" },
      Value = "global"
    };

    [Menu("Current Menu Theme")]
    public ListNode Theme { get; set; } = new ListNode()
    {
      Value = "Default"
    };

    public HotkeyNode MainMenuKeyToggle { get; set; } = (HotkeyNode) Keys.F12;

    public CorePluginSettings PluginSettings { get; set; } = new CorePluginSettings();

    public CorePerformanceSettings PerformanceSettings { get; set; } = new CorePerformanceSettings();

    [Menu("Enable VSync")]
    public ToggleNode EnableVSync { get; set; } = new ToggleNode(false);

    public ListNode Font { get; set; } = new ListNode()
    {
      Values = new List<string>() { "Not found" }
    };

    public ListNode FontGlyphRange { get; set; } = new ListNode()
    {
      Values = ((IEnumerable<FontGlyphRangeType>) Enum.GetValues<FontGlyphRangeType>()).Select<FontGlyphRangeType, string>((Func<FontGlyphRangeType, string>) (x => x.ToString())).ToList<string>(),
      Value = FontGlyphRangeType.Cyrillic.ToString()
    };

    [Menu("Font size", "Currently not works. Because this option broke calculate how much pixels needs for render.")]
    [IgnoreMenu]
    public RangeNode<int> FontSize { get; set; } = new RangeNode<int>(13, 7, 36);

    [Menu(null, "If unchecked, some plugin may ignore the selected font")]
    public ToggleNode ApplySelectedFontGlobally { get; set; } = new ToggleNode(true);

    [Menu(null, "If you use a large custom cursor, this can help see tooltips better")]
    public RangeNode<float> MouseCursorScale { get; set; } = new RangeNode<float>(1f, 0.1f, 10f);

    public RangeNode<int> Volume { get; set; } = new RangeNode<int>(100, 0, 100);

    public ToggleNode ShowDebugWindow { get; set; } = new ToggleNode(false);

    public ToggleNode ShowLogWindow { get; set; } = new ToggleNode(false);

    public ToggleNode ShowDemoWindow { get; set; } = new ToggleNode(false);

    public ToggleNode Enable { get; set; } = new ToggleNode(true);

    public ToggleNode ForceForeground { get; set; } = new ToggleNode(false);

    public ToggleNode HideAllDebugging { get; set; } = new ToggleNode(false);

    [Menu(null, "If you have a widescreen display and use some sort of a hack to force poe to render without black bars, you might need this")]
    public ToggleNode DisableBlackBarAdjustment { get; set; } = new ToggleNode(false);
  }
}
