// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.FullRareSetManagerSettings
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FullRareSetManager
{
  public class FullRareSetManagerSettings : ISettings
  {
    public List<int> AllowedStashTabs = new List<int>();

    public FullRareSetManagerSettings()
    {
      this.Enable = new ToggleNode(false);
      this.AllowIdentified = new ToggleNode(false);
      this.ShowOnlyWithInventory = new ToggleNode(false);
      this.HideWhenLeftPanelOpened = new ToggleNode(false);
      this.ShowRegalSets = new ToggleNode(false);
      this.PositionX = new RangeNode<float>(0.0f, 0.0f, 2000f);
      this.PositionY = new RangeNode<float>(365f, 0.0f, 2000f);
      this.WeaponTypePriority = new ListNode()
      {
        Value = "Two handed"
      };
      this.DropToInventoryKey = HotkeyNode.op_Implicit(Keys.F5);
      this.ExtraDelay = new RangeNode<int>(50, 0, 2000);
      this.EnableBorders = new ToggleNode(false);
      this.InventBorders = new ToggleNode(false);
      this.BorderWidth = new RangeNode<int>(5, 1, 15);
      this.BorderAutoResize = new ToggleNode(false);
      this.BorderOversize = new RangeNode<int>(5, 0, 15);
      this.TextSize = new RangeNode<int>(20, 0, 30);
      this.TextOffsetX = new RangeNode<float>(0.0f, -50f, 12f);
      this.TextOffsetY = new RangeNode<float>(-20f, -50f, 12f);
      this.IgnoreOneHanded = new ToggleNode(false);
      this.MaxSets = new RangeNode<int>(0, 0, 30);
      this.CalcByFreeSpace = new ToggleNode(false);
      this.AutoSell = new ToggleNode(true);
    }

    [Menu("", "Registering after using DropToInventoryKey to NPC trade inventory")]
    public TextNode SetsAmountStatisticsText { get; set; } = TextNode.op_Implicit("Total sets sold to vendor: N/A");

    public int SetsAmountStatistics { get; set; }

    [Menu("Position X")]
    public RangeNode<float> PositionX { get; set; }

    [Menu("Position Y")]
    public RangeNode<float> PositionY { get; set; }

    [Menu("Allow Identified Items")]
    public ToggleNode AllowIdentified { get; set; }

    [Menu("Show only with inventory")]
    public ToggleNode ShowOnlyWithInventory { get; set; }

    [Menu("Hide when left panel opened")]
    public ToggleNode HideWhenLeftPanelOpened { get; set; }

    [Menu("Show Regal sets")]
    public ToggleNode ShowRegalSets { get; set; }

    [Menu("Priority", "Weapon prepare priority in list of set items. If you have 1-handed and 2-handed weapons- it will consider this option.")]
    public ListNode WeaponTypePriority { get; set; }

    [Menu("Max Collecting Sets (0 disable)", "Amount of sets you going to collect. It will display lower pick priority if amount of item are more than this value.")]
    public RangeNode<int> MaxSets { get; set; }

    [Menu("Drop To Invent Key", "It will also drop items to NPC trade window inventory")]
    public HotkeyNode DropToInventoryKey { get; set; }

    [Menu("Extra Click Delay")]
    public RangeNode<int> ExtraDelay { get; set; }

    [Menu("Items Lables Borders", 0)]
    public ToggleNode EnableBorders { get; set; }

    [Menu("Inventory Borders")]
    public ToggleNode InventBorders { get; set; }

    [Menu("Borders Width", 1, 0)]
    public RangeNode<int> BorderWidth { get; set; }

    [Menu("Borders Oversize", 2, 0)]
    public RangeNode<int> BorderOversize { get; set; }

    [Menu("Resize Borders accord. to Pick Priority", "That will change borders width, oversize depending on pick priority.", 3, 0)]
    public ToggleNode BorderAutoResize { get; set; }

    [Menu("Text Size", 4, 0)]
    public RangeNode<int> TextSize { get; set; }

    [Menu("Text Offset X", 5, 0)]
    public RangeNode<float> TextOffsetX { get; set; }

    [Menu("Text Offset Y", 6, 0)]
    public RangeNode<float> TextOffsetY { get; set; }

    [Menu("Don't Higlight One Handed", 7, 0)]
    public ToggleNode IgnoreOneHanded { get; set; }

    [Menu("Separate stash tabs for each item type", "Pick priority will be calculated by free space in stash tab. Free space will be calculated for each item stash tab.")]
    public ToggleNode CalcByFreeSpace { get; set; }

    [Menu("Ignore Elder/Shaper items")]
    public ToggleNode IgnoreElderShaper { get; set; } = new ToggleNode(true);

    [Menu("Show Red Rectangle Around Ignored Items")]
    public ToggleNode ShowRedRectangleAroundIgnoredItems { get; set; } = new ToggleNode(true);

    [Menu("Auto sell on keypress at vendor?")]
    public ToggleNode AutoSell { get; set; }

    public ToggleNode Enable { get; set; }

    [Menu("Only Allowed Stash Tabs", "Define stash tabs manually to ignore other tabs")]
    public ToggleNode OnlyAllowedStashTabs { get; set; } = new ToggleNode(false);
  }
}
