// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.FullRareSetManagerCore
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Nodes;
using FullRareSetManager.SetParts;
using FullRareSetManager.Utilities;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;


#nullable enable
namespace FullRareSetManager
{
  public class FullRareSetManagerCore : BaseSettingsPlugin<
  #nullable disable
  FullRareSetManagerSettings>
  {
    private const int INPUT_DELAY = 15;
    private bool _bDropAllItems;
    private Inventory _currentOpenedStashTab;
    private string _currentOpenedStashTabName;
    private FullRareSetManagerCore.CurrentSetInfo _currentSetData;
    private string _drawInfoString = "";
    private DropAllToInventory _inventDrop;
    private BaseSetPart[] _itemSetTypes;
    private StashData _sData;
    public FullRareSetManagerCore.ItemDisplayData[] DisplayData;
    public FullRareSetManagerCore.FRSetManagerPublishInformation FrSetManagerPublishInformation;
    private bool _allowScanTabs = true;
    private Stopwatch _fixStopwatch = new Stopwatch();
    private readonly Dictionary<Entity, FullRareSetManagerCore.ItemDisplayData> _currentAlerts = new Dictionary<Entity, FullRareSetManagerCore.ItemDisplayData>();
    private Dictionary<long, LabelOnGround> _currentLabels = new Dictionary<long, LabelOnGround>();

    public virtual void ReceiveEvent(string eventId, object args)
    {
      if (!this.Settings.Enable.Value)
        return;
      switch (eventId)
      {
        case "stashie_start_drop_items":
          this._fixStopwatch.Restart();
          this._allowScanTabs = false;
          break;
        case "stashie_stop_drop_items":
          this._allowScanTabs = true;
          break;
        case "stashie_finish_drop_items_to_stash_tab":
          this._fixStopwatch.Restart();
          this.UpdateStashes();
          this.UpdatePlayerInventory();
          this.UpdateItemsSetsInfo();
          break;
      }
    }

    public virtual bool Initialise()
    {
      Input.RegisterKey(this.Settings.DropToInventoryKey.Value);
      this._sData = StashData.Load(this);
      if (this._sData == null)
      {
        this.LogMessage("RareSetManager: Can't load cached items from file StashData.json. Creating new config. Open stash tabs for updating info. Tell to developer if this happen often enough.", 10f);
        this._sData = new StashData();
      }
      this._inventDrop = new DropAllToInventory(this);
      this.DisplayData = new FullRareSetManagerCore.ItemDisplayData[8];
      for (int index = 0; index <= 7; ++index)
        this.DisplayData[index] = new FullRareSetManagerCore.ItemDisplayData();
      this.UpdateItemsSetsInfo();
      this.Settings.WeaponTypePriority.SetListValues(new List<string>()
      {
        "Two handed",
        "One handed"
      });
      this.Settings.CalcByFreeSpace.OnValueChanged += (EventHandler<bool>) ((_param1, _param2) => this.UpdateItemsSetsInfo());
      this.FrSetManagerPublishInformation = new FullRareSetManagerCore.FRSetManagerPublishInformation();
      return true;
    }

    public virtual void EntityAdded(Entity entity)
    {
      if (!this.Settings.EnableBorders.Value || entity.Type != 112 || !ToggleNode.op_Implicit(this.Settings.Enable) || this.GameController.Area.CurrentArea.IsTown || this._currentAlerts.ContainsKey(entity))
        return;
      StashItem stashItem = this.ProcessItem(entity.GetComponent<WorldItem>().ItemEntity);
      if (stashItem == null)
        return;
      if (ToggleNode.op_Implicit(this.Settings.IgnoreOneHanded) && stashItem.ItemType == StashItemType.OneHanded)
        stashItem = (StashItem) null;
      if (stashItem == null)
        return;
      int index = (int) stashItem.ItemType;
      if (index > 7)
        index = 0;
      FullRareSetManagerCore.ItemDisplayData itemDisplayData = this.DisplayData[index];
      this._currentAlerts.Add(entity, itemDisplayData);
    }

    public virtual void EntityRemoved(Entity entity)
    {
      if (!this.Settings.EnableBorders.Value || entity.Type != 112)
        return;
      if ((double) Vector2.Distance(entity.GridPos, this.GameController.Player.GridPos) < 10.0)
      {
        StashItem stashItem = this.ProcessItem(entity.GetComponent<WorldItem>().ItemEntity);
        if (stashItem == null)
          return;
        stashItem.BInPlayerInventory = true;
        this._sData.PlayerInventory.StashTabItems.Add(stashItem);
        this.UpdateItemsSetsInfo();
      }
      this._currentAlerts.Remove(entity);
      this._currentLabels.Remove(((RemoteMemoryObject) entity).Address);
    }

    public virtual void AreaChange(AreaInstance area)
    {
      this._currentLabels.Clear();
      this._currentAlerts.Clear();
    }

    public virtual void Render()
    {
      if (!this.GameController.Game.IngameState.InGame)
        return;
      this.FrSetManagerPublishInformation.WantedSets = this.Settings.MaxSets.Value;
      foreach (BaseSetPart itemSetType in this._itemSetTypes)
      {
        string partName = itemSetType.PartName;
        if (partName != null)
        {
          switch (partName.Length)
          {
            case 5:
              switch (partName[1])
              {
                case 'e':
                  if (partName == "Belts")
                  {
                    this.FrSetManagerPublishInformation.GatheredBelts = itemSetType.TotalSetsCount();
                    continue;
                  }
                  continue;
                case 'i':
                  if (partName == "Rings")
                  {
                    this.FrSetManagerPublishInformation.GatheredRings = itemSetType.TotalSetsCount();
                    continue;
                  }
                  continue;
                case 'o':
                  if (partName == "Boots")
                  {
                    this.FrSetManagerPublishInformation.GatheredBoots = itemSetType.TotalSetsCount();
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            case 6:
              if (partName == "Gloves")
              {
                this.FrSetManagerPublishInformation.GatheredGloves = itemSetType.TotalSetsCount();
                continue;
              }
              continue;
            case 7:
              switch (partName[0])
              {
                case 'A':
                  if (partName == "Amulets")
                  {
                    this.FrSetManagerPublishInformation.GatheredAmulets = itemSetType.TotalSetsCount();
                    continue;
                  }
                  continue;
                case 'H':
                  if (partName == "Helmets")
                  {
                    this.FrSetManagerPublishInformation.GatheredHelmets = itemSetType.TotalSetsCount();
                    continue;
                  }
                  continue;
                case 'W':
                  if (partName == "Weapons")
                  {
                    this.FrSetManagerPublishInformation.GatheredWeapons = itemSetType.TotalSetsCount();
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            case 11:
              if (partName == "Body Armors")
              {
                this.FrSetManagerPublishInformation.GatheredBodyArmors = itemSetType.TotalSetsCount();
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      this.PublishEvent("frsm_display_data", (object) this.FrSetManagerPublishInformation);
      if (!this._allowScanTabs)
      {
        if (this._fixStopwatch.ElapsedMilliseconds <= 3000L)
          return;
        this._allowScanTabs = true;
      }
      else
      {
        bool flag = this.UpdatePlayerInventory();
        IngameState ingameState = this.GameController.Game.IngameState;
        bool isVisible = ((Element) ingameState.IngameUi.StashElement).IsVisible;
        if (isVisible)
          flag = this.UpdateStashes() | flag;
        if (flag)
          this.UpdateItemsSetsInfo();
        if (this._bDropAllItems)
        {
          this._bDropAllItems = false;
          try
          {
            this.DropAllItems();
          }
          catch
          {
            this.LogError("There was an error while moving items.", 5f);
          }
          finally
          {
            this.UpdatePlayerInventory();
            this.UpdateItemsSetsInfo();
          }
        }
        if (!this._bDropAllItems)
          this.DrawSetsInfo();
        this.RenderLabels();
        if (!this.Settings.DropToInventoryKey.PressedOnce())
          return;
        if (isVisible && ((Element) ingameState.IngameUi.InventoryPanel).IsVisible && this._currentSetData.BSetIsReady)
          this._bDropAllItems = true;
        this.SellSetToVendor();
      }
    }

    public void SellSetToVendor(int callCount = 1)
    {
      try
      {
        Vector2 topLeft = this.GameController.Window.GetWindowRectangle().TopLeft;
        int latency = this.GameController.Game.IngameState.ServerData.Latency;
        SellWindow sellWindow = this.GameController.Game.IngameState.IngameUi.SellWindow;
        if (!((Element) sellWindow).IsVisible)
          this.LogMessage("Error: npcTradingWindow is not visible (opened)!", 5f);
        Element yourOffer = sellWindow.YourOffer;
        DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(41, 1);
        interpolatedStringHandler1.AppendLiteral("Player has put in ");
        interpolatedStringHandler1.AppendFormatted<long>(yourOffer.ChildCount - 2L);
        interpolatedStringHandler1.AppendLiteral(" in the trading window.");
        this.LogMessage(interpolatedStringHandler1.ToStringAndClear(), 3f);
        if (yourOffer.ChildCount < 11L)
        {
          for (int index = 0; index < 8; ++index)
          {
            StashItem[] preparedItems = this._itemSetTypes[index].GetPreparedItems();
            if (!((IEnumerable<StashItem>) preparedItems).Any<StashItem>((Func<StashItem, bool>) (item => !item.BInPlayerInventory)))
            {
              Keyboard.KeyDown(Keys.LControlKey);
              foreach (StashItem stashItem in preparedItems)
              {
                StashItem item = stashItem;
                NormalInventoryItem normalInventoryItem = ((IEnumerable<NormalInventoryItem>) this.GameController.Game.IngameState.IngameUi.InventoryPanel[(InventoryIndex) 13].VisibleInventoryItems).FirstOrDefault<NormalInventoryItem>((Func<NormalInventoryItem, bool>) (x => x.InventPosX == item.InventPosX && x.InventPosY == item.InventPosY));
                if (normalInventoryItem == null)
                {
                  this.LogError("FoundItem was null.", 3f);
                  return;
                }
                Thread.Sleep(15);
                Mouse.SetCursorPosAndLeftClick(((Element) normalInventoryItem).GetClientRect().Center + topLeft, RangeNode<int>.op_Implicit(this.Settings.ExtraDelay));
                Thread.Sleep(latency + RangeNode<int>.op_Implicit(this.Settings.ExtraDelay));
              }
            }
          }
          Keyboard.KeyUp(Keys.LControlKey);
        }
        Thread.Sleep(15 + this.Settings.ExtraDelay.Value);
        foreach (RemoteMemoryObject child in (IEnumerable<Element>) sellWindow.OtherOffer.Children)
        {
          Entity entity = child.AsObject<NormalInventoryItem>().Item;
          if (!string.IsNullOrEmpty(entity.Metadata))
          {
            string baseName = this.GameController.Files.BaseItemTypes.Translate(entity.Metadata).BaseName;
            if (!(baseName == "Chaos Orb") && !(baseName == "Regal Orb"))
            {
              this.LogMessage("Npc offered '" + baseName + "'", 3f);
              if (callCount >= 5)
                return;
              int millisecondsTimeout = 15 + this.Settings.ExtraDelay.Value;
              interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(32, 1);
              interpolatedStringHandler1.AppendLiteral("Trying to sell set again in ");
              interpolatedStringHandler1.AppendFormatted<int>(millisecondsTimeout);
              interpolatedStringHandler1.AppendLiteral(" ms.");
              this.LogMessage(interpolatedStringHandler1.ToStringAndClear(), 3f);
              Thread.Sleep(millisecondsTimeout);
              return;
            }
          }
        }
        Thread.Sleep(latency + RangeNode<int>.op_Implicit(this.Settings.ExtraDelay));
        Element acceptButton = sellWindow.AcceptButton;
        ++this.Settings.SetsAmountStatistics;
        FullRareSetManagerSettings settings = this.Settings;
        DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(27, 1);
        interpolatedStringHandler2.AppendLiteral("Total sets sold to vendor: ");
        interpolatedStringHandler2.AppendFormatted<int>(this.Settings.SetsAmountStatistics);
        TextNode textNode = TextNode.op_Implicit(interpolatedStringHandler2.ToStringAndClear());
        settings.SetsAmountStatisticsText = textNode;
        if (this.Settings.AutoSell.Value)
          Mouse.SetCursorPosAndLeftClick(acceptButton.GetClientRect().Center + topLeft, this.Settings.ExtraDelay.Value);
        else
          Mouse.SetCursorPos(acceptButton.GetClientRect().Center + topLeft);
      }
      catch
      {
        this.LogMessage("We hit catch!", 3f);
        Keyboard.KeyUp(Keys.LControlKey);
        Thread.Sleep(15);
      }
    }

    public void DropAllItems()
    {
      StashElement stashElement = this.GameController.IngameState.IngameUi.StashElement;
      IList<string> allStashNames = stashElement.AllStashNames;
      RectangleF windowRectangle = this.GameController.Window.GetWindowRectangle();
      int num = this.GameController.Game.IngameState.ServerData.Latency + RangeNode<int>.op_Implicit(this.Settings.ExtraDelay);
      POINT cursorPosition = Mouse.GetCursorPosition();
      for (int index = 0; index < 8; ++index)
      {
        StashItem[] preparedItems = this._itemSetTypes[index].GetPreparedItems();
        Keyboard.KeyDown(Keys.LControlKey);
        Thread.Sleep(15);
        try
        {
          foreach (StashItem stashItem in preparedItems)
          {
            if (!stashItem.BInPlayerInventory)
            {
              if (!this._inventDrop.SwitchToTab(allStashNames.IndexOf(stashItem.StashName), this.Settings))
              {
                Keyboard.KeyUp(Keys.LControlKey);
                return;
              }
              Thread.Sleep(num + RangeNode<int>.op_Implicit(this.Settings.ExtraDelay));
              this._currentOpenedStashTab = stashElement.VisibleStash;
              StashItem item = stashItem;
              NormalInventoryItem normalInventoryItem = ((IEnumerable<NormalInventoryItem>) this._currentOpenedStashTab.VisibleInventoryItems).FirstOrDefault<NormalInventoryItem>((Func<NormalInventoryItem, bool>) (x => x.InventPosX == item.InventPosX && x.InventPosY == item.InventPosY));
              int count = ((ICollection<NormalInventoryItem>) this._currentOpenedStashTab.VisibleInventoryItems).Count;
              if (normalInventoryItem != null)
              {
                Mouse.SetCursorPosAndLeftClick(((Element) normalInventoryItem).GetClientRect().Center + windowRectangle.TopLeft, RangeNode<int>.op_Implicit(this.Settings.ExtraDelay));
                item.BInPlayerInventory = true;
                Thread.Sleep(num + 100 + RangeNode<int>.op_Implicit(this.Settings.ExtraDelay));
                if (((ICollection<NormalInventoryItem>) this._currentOpenedStashTab.VisibleInventoryItems).Count == count)
                {
                  Thread.Sleep(200);
                  if (((ICollection<NormalInventoryItem>) this._currentOpenedStashTab.VisibleInventoryItems).Count == count)
                    this.LogError("Item was not dropped after additional delay: " + stashItem.ItemName, 5f);
                }
              }
              else
              {
                string itemName = item.ItemName;
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
                interpolatedStringHandler.AppendLiteral("Inventory Position: (");
                interpolatedStringHandler.AppendFormatted<int>(item.InventPosX);
                interpolatedStringHandler.AppendLiteral(",");
                interpolatedStringHandler.AppendFormatted<int>(item.InventPosY);
                interpolatedStringHandler.AppendLiteral(")");
                string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                this.LogError("We couldn't find the item we where looking for.\nItemName: " + itemName + ".\n" + stringAndClear, 5f);
              }
              if (!this.UpdateStashes())
                this.LogError("There was item drop but it don't want to update stash!", 10f);
            }
          }
        }
        catch (Exception ex)
        {
          this.LogError("Error move items: " + ex.Message, 4f);
        }
        Keyboard.KeyUp(Keys.LControlKey);
      }
      this.UpdatePlayerInventory();
      this.UpdateItemsSetsInfo();
      Mouse.SetCursorPos(cursorPosition);
    }

    private void DrawSetsInfo()
    {
      bool isVisible = ((Element) this.GameController.IngameState.IngameUi.StashElement).IsVisible;
      if (isVisible && this._currentSetData.BSetIsReady && this._currentOpenedStashTab != null)
      {
        IList<NormalInventoryItem> visibleInventoryItems = this._currentOpenedStashTab.VisibleInventoryItems;
        if (visibleInventoryItems != null)
        {
          RectangleF clientRect = this._currentOpenedStashTab.InventoryUIElement.GetClientRect();
          RectangleF rectangleF = new RectangleF(clientRect.Right, clientRect.Bottom, 270f, 240f);
          this.Graphics.DrawBox(rectangleF, new Color(0, 0, 0, 200));
          this.Graphics.DrawFrame(rectangleF, Color.White, 2);
          float x1 = rectangleF.X + 10f;
          float y1 = rectangleF.Y + 10f;
          this.Graphics.DrawText("Current " + (this._currentSetData.SetType == 1 ? "Chaos" : "Regal") + " set:", new Vector2(x1, y1), Color.White, 15);
          float y2 = y1 + 25f;
          for (int index = 0; index < 8; ++index)
          {
            foreach (StashItem preparedItem in this._itemSetTypes[index].GetPreparedItems())
            {
              int num = this._sData.PlayerInventory.StashTabItems.Contains(preparedItem) ? 1 : 0;
              bool flag = preparedItem.StashName == this._currentOpenedStashTabName;
              Color color = Color.Gray;
              if (num != 0)
                color = Color.Green;
              else if (flag)
                color = Color.Yellow;
              if (num == 0 & flag)
              {
                StashItem item = preparedItem;
                NormalInventoryItem normalInventoryItem = ((IEnumerable<NormalInventoryItem>) visibleInventoryItems).FirstOrDefault<NormalInventoryItem>((Func<NormalInventoryItem, bool>) (x => x.InventPosX == item.InventPosX && x.InventPosY == item.InventPosY));
                if (normalInventoryItem != null)
                  this.Graphics.DrawFrame(((Element) normalInventoryItem).GetClientRect(), Color.Yellow, 2);
              }
              this.Graphics.DrawText(preparedItem.StashName + " (" + preparedItem.ItemName + ") " + (preparedItem.LowLvl ? "L" : "H"), new Vector2(x1, y2), color, 15);
              y2 += 20f;
            }
          }
        }
      }
      if (ToggleNode.op_Implicit(this.Settings.ShowOnlyWithInventory) && !((Element) this.GameController.Game.IngameState.IngameUi.InventoryPanel).IsVisible || ToggleNode.op_Implicit(this.Settings.HideWhenLeftPanelOpened) && isVisible)
        return;
      float x2 = this.Settings.PositionX.Value;
      float y = this.Settings.PositionY.Value;
      RectangleF rectangleF1 = new RectangleF(x2, y, 230f, 200f);
      this.Graphics.DrawBox(rectangleF1, new Color(0, 0, 0, 200));
      this.Graphics.DrawFrame(rectangleF1, Color.White, 2);
      this.Graphics.DrawText(this._drawInfoString, new Vector2(x2 + 10f, y + 10f), Color.White, 15);
    }

    private void UpdateItemsSetsInfo()
    {
      this._currentSetData = new FullRareSetManagerCore.CurrentSetInfo();
      this._itemSetTypes = new BaseSetPart[8];
      BaseSetPart[] itemSetTypes1 = this._itemSetTypes;
      WeaponItemsSetPart weaponItemsSetPart = new WeaponItemsSetPart("Weapons");
      weaponItemsSetPart.ItemCellsSize = 8;
      itemSetTypes1[0] = (BaseSetPart) weaponItemsSetPart;
      BaseSetPart[] itemSetTypes2 = this._itemSetTypes;
      SingleItemSetPart singleItemSetPart1 = new SingleItemSetPart("Helmets");
      singleItemSetPart1.ItemCellsSize = 4;
      itemSetTypes2[1] = (BaseSetPart) singleItemSetPart1;
      BaseSetPart[] itemSetTypes3 = this._itemSetTypes;
      SingleItemSetPart singleItemSetPart2 = new SingleItemSetPart("Body Armors");
      singleItemSetPart2.ItemCellsSize = 6;
      itemSetTypes3[2] = (BaseSetPart) singleItemSetPart2;
      BaseSetPart[] itemSetTypes4 = this._itemSetTypes;
      SingleItemSetPart singleItemSetPart3 = new SingleItemSetPart("Gloves");
      singleItemSetPart3.ItemCellsSize = 4;
      itemSetTypes4[3] = (BaseSetPart) singleItemSetPart3;
      BaseSetPart[] itemSetTypes5 = this._itemSetTypes;
      SingleItemSetPart singleItemSetPart4 = new SingleItemSetPart("Boots");
      singleItemSetPart4.ItemCellsSize = 4;
      itemSetTypes5[4] = (BaseSetPart) singleItemSetPart4;
      BaseSetPart[] itemSetTypes6 = this._itemSetTypes;
      SingleItemSetPart singleItemSetPart5 = new SingleItemSetPart("Belts");
      singleItemSetPart5.ItemCellsSize = 2;
      itemSetTypes6[5] = (BaseSetPart) singleItemSetPart5;
      BaseSetPart[] itemSetTypes7 = this._itemSetTypes;
      SingleItemSetPart singleItemSetPart6 = new SingleItemSetPart("Amulets");
      singleItemSetPart6.ItemCellsSize = 1;
      itemSetTypes7[6] = (BaseSetPart) singleItemSetPart6;
      BaseSetPart[] itemSetTypes8 = this._itemSetTypes;
      RingItemsSetPart ringItemsSetPart = new RingItemsSetPart("Rings");
      ringItemsSetPart.ItemCellsSize = 1;
      itemSetTypes8[7] = (BaseSetPart) ringItemsSetPart;
      for (int index = 0; index <= 7; ++index)
        this.DisplayData[index].BaseData = this._itemSetTypes[index];
      foreach (StashItem stashTabItem in this._sData.PlayerInventory.StashTabItems)
      {
        int index = (int) stashTabItem.ItemType;
        if (index > 7)
          index = 0;
        BaseSetPart itemSetType = this._itemSetTypes[index];
        stashTabItem.BInPlayerInventory = true;
        StashItem stashItem = stashTabItem;
        itemSetType.AddItem(stashItem);
      }
      foreach (KeyValuePair<string, StashTabData> stashTab in this._sData.StashTabs)
      {
        List<StashItem> stashTabItems = stashTab.Value.StashTabItems;
        foreach (StashItem stashItem in stashTabItems)
        {
          int index = (int) stashItem.ItemType;
          if (index > 7)
            index = 0;
          BaseSetPart itemSetType = this._itemSetTypes[index];
          stashItem.BInPlayerInventory = false;
          itemSetType.AddItem(stashItem);
          itemSetType.StashTabItemsCount = stashTabItems.Count;
        }
      }
      this._drawInfoString = "";
      int val2 = 0;
      int num1 = int.MaxValue;
      int val1 = int.MaxValue;
      int num2 = 0;
      for (int index = 0; index <= 7; ++index)
      {
        BaseSetPart itemSetType = this._itemSetTypes[index];
        int num3 = itemSetType.LowSetsCount();
        int num4 = itemSetType.HighSetsCount();
        int num5 = itemSetType.TotalSetsCount();
        if (val1 > num5)
          val1 = num5;
        if (num2 < num5)
          num2 = num5;
        if (num1 > num4)
          num1 = num4;
        val2 += num3;
        this._drawInfoString = this._drawInfoString + itemSetType.GetInfoString() + "\r\n";
        FullRareSetManagerCore.ItemDisplayData itemDisplayData = this.DisplayData[index];
        itemDisplayData.TotalCount = num5;
        itemDisplayData.TotalLowCount = num3;
        itemDisplayData.TotalHighCount = num4;
        if (this.Settings.CalcByFreeSpace.Value)
        {
          int num6 = 144 / itemSetType.ItemCellsSize;
          itemDisplayData.FreeSpaceCount = num6 - (itemSetType.StashTabItemsCount + itemSetType.PlayerInventItemsCount());
          if (itemDisplayData.FreeSpaceCount < 0)
            itemDisplayData.FreeSpaceCount = 0;
          itemDisplayData.PriorityPercent = (float) itemDisplayData.FreeSpaceCount / (float) num6;
          if ((double) itemDisplayData.PriorityPercent > 1.0)
            itemDisplayData.PriorityPercent = 1f;
          itemDisplayData.PriorityPercent = 1f - itemDisplayData.PriorityPercent;
        }
      }
      if (!this.Settings.CalcByFreeSpace.Value)
      {
        int num7 = num2;
        if (this.Settings.MaxSets.Value > 0)
          num7 = this.Settings.MaxSets.Value;
        for (int index = 0; index <= 7; ++index)
        {
          FullRareSetManagerCore.ItemDisplayData itemDisplayData = this.DisplayData[index];
          if (itemDisplayData.TotalCount == 0)
          {
            itemDisplayData.PriorityPercent = 0.0f;
          }
          else
          {
            itemDisplayData.PriorityPercent = (float) itemDisplayData.TotalCount / (float) num7;
            if ((double) itemDisplayData.PriorityPercent > 1.0)
              itemDisplayData.PriorityPercent = 1f;
          }
        }
      }
      this._drawInfoString += "\r\n";
      int num8 = Math.Min(val1, val2);
      this._drawInfoString = this._drawInfoString + "Chaos sets ready: " + num8.ToString();
      if (this.Settings.ShowRegalSets.Value)
      {
        this._drawInfoString += "\r\n";
        this._drawInfoString = this._drawInfoString + "Regal sets ready: " + num1.ToString();
      }
      if (num8 <= 0 && num1 <= 0)
        return;
      int num9 = 0;
      int index1 = -1;
      bool flag = false;
      for (int index2 = 0; index2 < 8; ++index2)
      {
        PrepareItemResult prepareItemResult = this._itemSetTypes[index2].PrepareItemForSet(this.Settings);
        flag = flag || prepareItemResult.LowSet;
        if (num9 < prepareItemResult.AllowedReplacesCount && !prepareItemResult.BInPlayerInvent)
        {
          num9 = prepareItemResult.AllowedReplacesCount;
          index1 = index2;
        }
      }
      if (!flag)
      {
        if (ToggleNode.op_Implicit(this.Settings.ShowRegalSets))
        {
          this._currentSetData.BSetIsReady = true;
          this._currentSetData.SetType = 2;
        }
        else if (num9 == 0)
        {
          this._currentSetData.BSetIsReady = true;
          this._currentSetData.SetType = 2;
        }
        else if (index1 != -1)
        {
          this._itemSetTypes[index1].DoLowItemReplace();
          this._currentSetData.SetType = 1;
          this._currentSetData.BSetIsReady = true;
        }
        else
        {
          this._currentSetData.BSetIsReady = true;
          this._currentSetData.SetType = 1;
        }
      }
      else
      {
        this._currentSetData.BSetIsReady = true;
        this._currentSetData.SetType = 1;
      }
    }

    public bool UpdateStashes()
    {
      StashElement stashElement = this.GameController.IngameState.IngameUi.StashElement;
      if (stashElement == null)
      {
        this.LogMessage("ServerData.StashPanel is null", 3f);
        return false;
      }
      bool flag1 = false;
      this._currentOpenedStashTabName = "";
      this._currentOpenedStashTab = stashElement.VisibleStash;
      if (this._currentOpenedStashTab == null)
        return false;
      for (int index = 0; (long) index < stashElement.TotalStashes; ++index)
      {
        string stashName = stashElement.GetStashName(index);
        if (!this.Settings.OnlyAllowedStashTabs.Value || this.Settings.AllowedStashTabs.Contains(index))
        {
          Inventory inventoryByIndex = stashElement.GetStashInventoryByIndex(index);
          IList<NormalInventoryItem> visibleInventoryItems = inventoryByIndex?.VisibleInventoryItems;
          if (visibleInventoryItems != null)
          {
            if (((RemoteMemoryObject) this._currentOpenedStashTab).Address == ((RemoteMemoryObject) inventoryByIndex).Address)
              this._currentOpenedStashTabName = stashName;
            bool flag2 = false;
            StashTabData stashTabData;
            if (!this._sData.StashTabs.TryGetValue(stashName, out stashTabData))
            {
              stashTabData = new StashTabData();
              flag2 = true;
            }
            List<StashItem> stashItemList = new List<StashItem>();
            flag1 = true;
            foreach (NormalInventoryItem normalInventoryItem in (IEnumerable<NormalInventoryItem>) visibleInventoryItems)
            {
              StashItem stashItem = this.ProcessItem(normalInventoryItem.Item);
              if (stashItem == null)
              {
                if (ToggleNode.op_Implicit(this.Settings.ShowRedRectangleAroundIgnoredItems))
                  this.Graphics.DrawFrame(((Element) normalInventoryItem).GetClientRect(), Color.Red, 2);
              }
              else
              {
                stashItem.StashName = stashName;
                stashItem.InventPosX = normalInventoryItem.InventPosX;
                stashItem.InventPosY = normalInventoryItem.InventPosY;
                stashItem.BInPlayerInventory = false;
                stashItemList.Add(stashItem);
              }
            }
            if (((RemoteMemoryObject) this._currentOpenedStashTab).Address == ((RemoteMemoryObject) inventoryByIndex).Address)
            {
              stashTabData.StashTabItems = stashItemList;
              stashTabData.ItemsCount = (int) inventoryByIndex.ItemCount;
            }
            if (flag2 && stashTabData.ItemsCount > 0)
              this._sData.StashTabs.Add(stashName, stashTabData);
          }
        }
      }
      return flag1;
    }

    private bool UpdatePlayerInventory()
    {
      ServerInventory inventory = this.GameController.Game.IngameState.ServerData.PlayerInventories[0].Inventory;
      if (this._sData?.PlayerInventory == null)
        return true;
      this._sData.PlayerInventory = new StashTabData();
      ServerInventory serverInventory = inventory;
      if (serverInventory == null)
        return true;
      foreach (ServerInventory.InventSlotItem inventorySlotItem in (IEnumerable<ServerInventory.InventSlotItem>) serverInventory.InventorySlotItems)
      {
        StashItem stashItem = this.ProcessItem(inventorySlotItem.Item);
        if (stashItem != null)
        {
          stashItem.InventPosX = (int) inventorySlotItem.InventoryPosition.X;
          stashItem.InventPosY = (int) inventorySlotItem.InventoryPosition.Y;
          stashItem.BInPlayerInventory = true;
          this._sData.PlayerInventory.StashTabItems.Add(stashItem);
        }
      }
      this._sData.PlayerInventory.ItemsCount = inventory.TotalItemsCounts;
      return true;
    }

    private StashItem ProcessItem(Entity item)
    {
      try
      {
        if (item == null)
          return (StashItem) null;
        Mods component1 = item?.GetComponent<Mods>();
        if ((component1 != null ? (component1.ItemRarity != 2 ? 1 : 0) : 1) != 0)
          return (StashItem) null;
        bool identified = component1.Identified;
        if (identified && !ToggleNode.op_Implicit(this.Settings.AllowIdentified) || component1.ItemLevel < 60)
          return (StashItem) null;
        StashItem stashItem = new StashItem()
        {
          BIdentified = identified,
          LowLvl = component1.ItemLevel < 75
        };
        if (string.IsNullOrEmpty(item.Metadata))
        {
          this.LogError("Item metadata is empty. Can be fixed by restarting the game", 10f);
          return (StashItem) null;
        }
        if (this.Settings.IgnoreElderShaper.Value)
        {
          Base component2 = item.GetComponent<Base>();
          if (component2.isElder || component2.isShaper)
            return (StashItem) null;
        }
        BaseItemType baseItemType = this.GameController.Files.BaseItemTypes.Translate(item.Metadata);
        if (baseItemType == null)
          return (StashItem) null;
        stashItem.ItemClass = baseItemType.ClassName;
        stashItem.ItemName = baseItemType.BaseName;
        stashItem.ItemType = this.GetStashItemTypeByClassName(stashItem.ItemClass);
        if (stashItem.ItemType != StashItemType.Undefined)
          return stashItem;
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
        interpolatedStringHandler.AppendLiteral("Error in \"ProcessItem\": ");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        this.LogError(interpolatedStringHandler.ToStringAndClear(), 10f);
        return (StashItem) null;
      }
      return (StashItem) null;
    }

    private StashItemType GetStashItemTypeByClassName(string className)
    {
      if (className.StartsWith("Two Hand"))
        return StashItemType.TwoHanded;
      if (className.StartsWith("One Hand") || className.StartsWith("Thrusting One Hand"))
        return StashItemType.OneHanded;
      if (className != null)
      {
        switch (className.Length)
        {
          case 3:
            if (className == "Bow")
              return StashItemType.TwoHanded;
            break;
          case 4:
            switch (className[0])
            {
              case 'B':
                if (className == "Belt")
                  return StashItemType.Belt;
                break;
              case 'C':
                if (className == "Claw")
                  return StashItemType.OneHanded;
                break;
              case 'R':
                if (className == "Ring")
                  return StashItemType.Ring;
                break;
              case 'W':
                if (className == "Wand")
                  return StashItemType.OneHanded;
                break;
            }
            break;
          case 5:
            switch (className[0])
            {
              case 'B':
                if (className == "Boots")
                  return StashItemType.Boots;
                break;
              case 'S':
                if (className == "Staff")
                  return StashItemType.TwoHanded;
                break;
            }
            break;
          case 6:
            switch (className[0])
            {
              case 'A':
                if (className == "Amulet")
                  return StashItemType.Amulet;
                break;
              case 'D':
                if (className == "Dagger")
                  return StashItemType.OneHanded;
                break;
              case 'G':
                if (className == "Gloves")
                  return StashItemType.Gloves;
                break;
              case 'H':
                if (className == "Helmet")
                  return StashItemType.Helmet;
                break;
              case 'S':
                if (className == "Shield")
                  return StashItemType.OneHanded;
                break;
            }
            break;
          case 7:
            if (className == "Sceptre")
              return StashItemType.OneHanded;
            break;
          case 8:
            if (className == "Warstaff")
              return StashItemType.TwoHanded;
            break;
          case 11:
            switch (className[0])
            {
              case 'B':
                if (className == "Body Armour")
                  return StashItemType.Body;
                break;
              case 'R':
                if (className == "Rune Dagger")
                  return StashItemType.OneHanded;
                break;
            }
            break;
        }
      }
      return StashItemType.Undefined;
    }

    public virtual void DrawSettings()
    {
      base.DrawSettings();
      IList<string> allStashNames = this.GameController.Game.IngameState.IngameUi.StashElement.AllStashNames;
      int num1 = 0;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
      interpolatedStringHandler.AppendLiteral("Add##");
      ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
      int num2 = num1;
      int num3 = num2 + 1;
      local.AppendFormatted<int>(num2);
      if (ImGui.Button(interpolatedStringHandler.ToStringAndClear()))
        this.Settings.AllowedStashTabs.Add(-1);
      for (int index = 0; index < this.Settings.AllowedStashTabs.Count; ++index)
      {
        int allowedStashTab = this.Settings.AllowedStashTabs[index];
        if (ImGui.Combo(allowedStashTab >= allStashNames.Count || allowedStashTab < 0 ? "??" : allStashNames[allowedStashTab], ref allowedStashTab, allStashNames.ToArray<string>(), allStashNames.Count))
          this.Settings.AllowedStashTabs[index] = allowedStashTab;
        ImGui.SameLine();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
        interpolatedStringHandler.AppendLiteral("Remove##");
        interpolatedStringHandler.AppendFormatted<int>(num3++);
        if (ImGui.Button(interpolatedStringHandler.ToStringAndClear()))
        {
          this.Settings.AllowedStashTabs.RemoveAt(index);
          --index;
        }
      }
    }

    public virtual void OnClose()
    {
      if (this._sData == null)
        return;
      StashData.Save(this, this._sData);
    }

    private void RenderLabels()
    {
      if (!this.Settings.EnableBorders.Value)
        return;
      bool flag = false;
      foreach (KeyValuePair<Entity, FullRareSetManagerCore.ItemDisplayData> keyValuePair in ((IEnumerable<KeyValuePair<Entity, FullRareSetManagerCore.ItemDisplayData>>) new Dictionary<Entity, FullRareSetManagerCore.ItemDisplayData>((IDictionary<Entity, FullRareSetManagerCore.ItemDisplayData>) this._currentAlerts)).AsParallel<KeyValuePair<Entity, FullRareSetManagerCore.ItemDisplayData>>().Where<KeyValuePair<Entity, FullRareSetManagerCore.ItemDisplayData>>((Func<KeyValuePair<Entity, FullRareSetManagerCore.ItemDisplayData>, bool>) (x => x.Key != null && ((RemoteMemoryObject) x.Key).Address != 0L && x.Key.IsValid)).ToList<KeyValuePair<Entity, FullRareSetManagerCore.ItemDisplayData>>())
      {
        if (this.DrawBorder(((RemoteMemoryObject) keyValuePair.Key).Address, keyValuePair.Value) && !flag)
          flag = true;
      }
      if (flag)
        this._currentLabels = ((IEnumerable<LabelOnGround>) this.GameController.Game.IngameState.IngameUi.ItemsOnGroundLabels).Where<LabelOnGround>((Func<LabelOnGround, bool>) (y => y?.ItemOnGround != null)).GroupBy<LabelOnGround, long>((Func<LabelOnGround, long>) (y => ((RemoteMemoryObject) y.ItemOnGround).Address)).ToDictionary<IGrouping<long, LabelOnGround>, long, LabelOnGround>((Func<IGrouping<long, LabelOnGround>, long>) (y => y.Key), (Func<IGrouping<long, LabelOnGround>, LabelOnGround>) (y => ((IEnumerable<LabelOnGround>) y).First<LabelOnGround>()));
      if (!this.Settings.InventBorders.Value || !((Element) this.GameController.Game.IngameState.IngameUi.InventoryPanel).IsVisible)
        return;
      IList<NormalInventoryItem> visibleInventoryItems = this.GameController.Game.IngameState.IngameUi.InventoryPanel[(InventoryIndex) 13].VisibleInventoryItems;
      if (visibleInventoryItems == null)
        return;
      foreach (NormalInventoryItem normalInventoryItem in (IEnumerable<NormalInventoryItem>) visibleInventoryItems)
      {
        Entity entity = normalInventoryItem.Item;
        if (entity != null)
        {
          StashItem stashItem = this.ProcessItem(entity);
          if (stashItem != null)
          {
            int index = (int) stashItem.ItemType;
            if (index > 7)
              index = 0;
            FullRareSetManagerCore.ItemDisplayData itemDisplayData = this.DisplayData[index];
            RectangleF clientRect = ((Element) normalInventoryItem).GetClientRect();
            Color color = Color.Lerp(Color.Red, Color.Green, itemDisplayData.PriorityPercent);
            clientRect.X += 2f;
            clientRect.Y += 2f;
            clientRect.Width -= 4f;
            clientRect.Height -= 4f;
            RectangleF rectangleF = new RectangleF(clientRect.X + 3f, clientRect.Y + 3f, 40f, 20f);
            this.Graphics.DrawBox(rectangleF, new Color(10, 10, 10, 230));
            this.Graphics.DrawFrame(clientRect, color, 2);
            Graphics graphics = this.Graphics;
            DefaultInterpolatedStringHandler interpolatedStringHandler;
            string stringAndClear;
            if (!this.Settings.CalcByFreeSpace.Value)
            {
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler.AppendFormatted<float>(itemDisplayData.PriorityPercent, "p0");
              stringAndClear = interpolatedStringHandler.ToStringAndClear();
            }
            else
            {
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler.AppendFormatted<int>(itemDisplayData.FreeSpaceCount);
              stringAndClear = interpolatedStringHandler.ToStringAndClear();
            }
            Vector2 topLeft = rectangleF.TopLeft;
            Color white = Color.White;
            int num = this.Settings.TextSize.Value;
            graphics.DrawText(stringAndClear, topLeft, white, num);
          }
        }
      }
    }

    private bool DrawBorder(long entityAddress, FullRareSetManagerCore.ItemDisplayData data)
    {
      IngameUIElements ingameUi = this.GameController.Game.IngameState.IngameUi;
      bool flag = false;
      LabelOnGround labelOnGround;
      if (this._currentLabels.TryGetValue(entityAddress, out labelOnGround))
      {
        if (!labelOnGround.IsVisible)
          return flag;
        RectangleF clientRect = labelOnGround.Label.GetClientRect();
        if (ingameUi.OpenLeftPanel.IsVisible && ingameUi.OpenLeftPanel.GetClientRect().Intersects(clientRect) || ingameUi.OpenRightPanel.IsVisible && ingameUi.OpenRightPanel.GetClientRect().Intersects(clientRect))
          return false;
        int a1 = this.Settings.BorderOversize.Value;
        if (this.Settings.BorderAutoResize.Value)
          a1 = (int) this.Lerp((float) a1, 1f, data.PriorityPercent);
        clientRect.X -= (float) a1;
        clientRect.Y -= (float) a1;
        clientRect.Width += (float) (a1 * 2);
        clientRect.Height += (float) (a1 * 2);
        Color color = Color.Lerp(Color.Red, Color.Green, data.PriorityPercent);
        int a2 = this.Settings.BorderWidth.Value;
        if (this.Settings.BorderAutoResize.Value)
          a2 = (int) this.Lerp((float) a2, 1f, data.PriorityPercent);
        this.Graphics.DrawFrame(clientRect, color, a2);
        if (this.Settings.TextSize.Value == 0)
          return flag;
        if ((double) RangeNode<float>.op_Implicit(this.Settings.TextOffsetX) < 0.0)
          clientRect.X += RangeNode<float>.op_Implicit(this.Settings.TextOffsetX);
        else
          clientRect.X += clientRect.Width * (this.Settings.TextOffsetX.Value / 10f);
        if ((double) RangeNode<float>.op_Implicit(this.Settings.TextOffsetY) < 0.0)
          clientRect.Y += RangeNode<float>.op_Implicit(this.Settings.TextOffsetY);
        else
          clientRect.Y += clientRect.Height * (this.Settings.TextOffsetY.Value / 10f);
        Graphics graphics = this.Graphics;
        string stringAndClear;
        if (!this.Settings.CalcByFreeSpace.Value)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
          interpolatedStringHandler.AppendFormatted<float>(data.PriorityPercent, "p0");
          stringAndClear = interpolatedStringHandler.ToStringAndClear();
        }
        else
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
          interpolatedStringHandler.AppendFormatted<int>(data.FreeSpaceCount);
          stringAndClear = interpolatedStringHandler.ToStringAndClear();
        }
        Vector2 topLeft = clientRect.TopLeft;
        Color white = Color.White;
        int num = this.Settings.TextSize.Value;
        graphics.DrawText(stringAndClear, topLeft, white, num);
      }
      else
        flag = true;
      return flag;
    }

    private float Lerp(float a, float b, float f) => a + f * (b - a);

    public class ClassForPickit
    {
      public FullRareSetManagerCore.ItemDisplayData[] dataArray { get; set; }

      public int MaxItemSet { get; set; }
    }

    public class FRSetManagerPublishInformation
    {
      public int GatheredWeapons { get; set; }

      public int GatheredHelmets { get; set; }

      public int GatheredBodyArmors { get; set; }

      public int GatheredGloves { get; set; }

      public int GatheredBoots { get; set; }

      public int GatheredBelts { get; set; }

      public int GatheredAmulets { get; set; }

      public int GatheredRings { get; set; }

      public int WantedSets { get; set; }
    }

    private struct CurrentSetInfo
    {
      public bool BSetIsReady;
      public int SetType;
    }

    public class ItemDisplayData
    {
      public BaseSetPart BaseData;
      public int FreeSpaceCount;
      public float PriorityPercent;
      public int TotalCount;
      public int TotalHighCount;
      public int TotalLowCount;
    }
  }
}
