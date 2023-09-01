// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.StashElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class StashElement : Element
  {
    private readonly CachedValue<StashElementOffsets> _cache;

    public StashElement() => this._cache = (CachedValue<StashElementOffsets>) this.CreateStructFrameCache<StashElementOffsets>();

    public long TotalStashes
    {
      get
      {
        Element stashInventoryPanel = this.StashInventoryPanel;
        return stashInventoryPanel == null ? 0L : stashInventoryPanel.ChildCount;
      }
    }

    public Element ExitButton => this.Address == 0L ? (Element) null : this.GetObject<Element>(this._cache.Value.ExitButtonPtr);

    public StashTabContainer StashTabContainer
    {
      get
      {
        long address = this.M.Read<long>(this._cache.Value.StashTabContainerPtr1 + 632L);
        StashTabContainer stashTabContainer;
        if (address == 0L)
          stashTabContainer = this.GetChildFromIndices(2, 0, 0, 1)?.AsObject<StashTabContainer>();
        else
          stashTabContainer = this.GetObject<StashTabContainer>(address);
        return stashTabContainer;
      }
    }

    public Element StashTitlePanel => this.Address == 0L ? (Element) null : this.GetObject<Element>(this._cache.Value.StashTitlePanelPtr);

    public Element StashInventoryPanel
    {
      get
      {
        if (this.Address == 0L)
          return (Element) null;
        return this.StashTabContainer?.StashInventoryPanel;
      }
    }

    public Element ViewAllStashButton
    {
      get
      {
        if (this.Address == 0L)
          return (Element) null;
        return this.StashTabContainer?.ViewAllStashesButton;
      }
    }

    public Element ViewAllStashPanel
    {
      get
      {
        if (this.Address == 0L)
          return (Element) null;
        return this.StashTabContainer?.ViewAllStashPanel;
      }
    }

    [Obsolete("Duplicate with ViewAllStashButton")]
    public Element ButtonStashTabListPin
    {
      get
      {
        if (this.Address == 0L)
          return (Element) null;
        return this.StashTabContainer?.ReadObjectAt<Element>(2456);
      }
    }

    public Element PinStashTabListButton
    {
      get
      {
        if (this.Address == 0L)
          return (Element) null;
        return this.StashTabContainer?.PinStashTabListButton;
      }
    }

    public int IndexVisibleStash
    {
      get
      {
        StashTabContainer stashTabContainer = this.StashTabContainer;
        return stashTabContainer == null ? 0 : stashTabContainer.VisibleStashIndex;
      }
    }

    public Inventory VisibleStash
    {
      get
      {
        if (!this.IsVisible)
          return (Inventory) null;
        return this.StashTabContainer?.VisibleStash;
      }
    }

    public IList<string> AllStashNames => this.StashTabContainer?.AllStashNames ?? (IList<string>) new List<string>();

    public IList<Inventory> AllInventories => this.StashTabContainer?.AllInventories ?? (IList<Inventory>) new List<Inventory>();

    public IList<Element> TabListButtons => this.StashTabContainer?.TabListButtons;

    public IList<Element> ViewAllStashPanelChildren => this.StashTabContainer?.ViewAllStashPanelChildren;

    public Inventory GetStashInventoryByIndex(int index) => this.StashTabContainer?.GetStashInventoryByIndex(index);

    public IList<Element> GetTabListButtons() => this.TabListButtons;

    public string GetStashName(int index) => this.StashTabContainer?.GetStashName(index) ?? string.Empty;
  }
}
