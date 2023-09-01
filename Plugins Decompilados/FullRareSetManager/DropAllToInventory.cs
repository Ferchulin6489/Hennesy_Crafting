// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.DropAllToInventory
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Nodes;
using FullRareSetManager.Utilities;
using SharpDX;
using System;
using System.Threading;
using System.Windows.Forms;

namespace FullRareSetManager
{
  public class DropAllToInventory
  {
    private const int WHILE_DELAY = 5;
    private readonly FullRareSetManagerCore _plugin;

    public DropAllToInventory(FullRareSetManagerCore plugin) => this._plugin = plugin;

    private GameController GameController => this._plugin.GameController;

    public bool SwitchToTab(int tabIndex, FullRareSetManagerSettings Settings)
    {
      int latency = this.GameController.Game.IngameState.ServerData.Latency;
      Element openLeftPanel = this.GameController.Game.IngameState.IngameUi.OpenLeftPanel;
      try
      {
        if (this.GameController.Game.IngameState.IngameUi.StashElement.GetStashInventoryByIndex(tabIndex).InventoryUIElement.IsVisible)
          return true;
      }
      catch
      {
      }
      Vector2 topLeft = this.GameController.Window.GetWindowRectangle().TopLeft;
      int num1 = latency * 20 > 2000 ? latency * 20 / 5 : 400;
      if (tabIndex > 30)
        return this.SwitchToTabViaArrowKeys(tabIndex);
      StashElement stashElement = this.GameController.Game.IngameState.IngameUi.StashElement;
      try
      {
        Element viewAllStashButton = this.GameController.Game.IngameState.IngameUi.StashElement.ViewAllStashButton;
        if (((Element) stashElement).IsVisible && !viewAllStashButton.IsVisible)
          return this.SwitchToTabViaArrowKeys(tabIndex);
        Element viewAllStashPanel = this.GameController.Game.IngameState.IngameUi.StashElement.ViewAllStashPanel;
        if (!viewAllStashPanel.IsVisible)
        {
          Mouse.SetCursorPosAndLeftClick(viewAllStashButton.GetClientRect().Center + topLeft, RangeNode<int>.op_Implicit(Settings.ExtraDelay));
          Thread.Sleep(latency + RangeNode<int>.op_Implicit(Settings.ExtraDelay));
          if (this.GameController.Game.IngameState.IngameUi.StashElement.TotalStashes > 30L)
          {
            Mouse.VerticalScroll(true, 5);
            Thread.Sleep(latency + RangeNode<int>.op_Implicit(Settings.ExtraDelay));
          }
        }
        long totalStashes = this.GameController.Game.IngameState.IngameUi.StashElement.TotalStashes;
        bool flag = true;
        RectangleF clientRect;
        if (false)
        {
          clientRect = viewAllStashPanel.GetChildAtIndex(1).GetChildAtIndex(tabIndex).GetClientRect();
        }
        else
        {
          if (!flag)
            return false;
          clientRect = viewAllStashPanel.GetChildAtIndex(2).GetChildAtIndex(tabIndex).GetClientRect();
        }
        Mouse.SetCursorPosAndLeftClick(clientRect.Center + topLeft, RangeNode<int>.op_Implicit(Settings.ExtraDelay));
        Thread.Sleep(latency + RangeNode<int>.op_Implicit(Settings.ExtraDelay));
      }
      catch (Exception ex)
      {
        return false;
      }
      int num2 = 0;
      Inventory visibleStash;
      do
      {
        Thread.Sleep(5);
        visibleStash = stashElement.VisibleStash;
      }
      while (num2++ <= num1 && visibleStash?.VisibleInventoryItems == null);
      return true;
    }

    private bool SwitchToTabViaArrowKeys(int tabIndex)
    {
      int latency = this.GameController.Game.IngameState.ServerData.Latency;
      int indexVisibleStash = this.GameController.Game.IngameState.IngameUi.StashElement.IndexVisibleStash;
      int num = tabIndex - indexVisibleStash;
      bool flag = num < 0;
      for (int index = 0; index < Math.Abs(num); ++index)
      {
        Keyboard.KeyPress(flag ? Keys.Left : Keys.Right);
        Thread.Sleep(latency);
      }
      return true;
    }
  }
}
