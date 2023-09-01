// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.StashTabContainer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.Elements
{
  public class StashTabContainer : Element
  {
    private readonly CachedValue<StashTabContainerOffsets> _cache;

    public StashTabContainer() => this._cache = (CachedValue<StashTabContainerOffsets>) this.CreateStructFrameCache<StashTabContainerOffsets>();

    public int VisibleStashIndex => this._cache.Value.VisibleStashIndex;

    public Element StashInventoryPanel => this.Address == 0L ? (Element) null : this[1];

    public Element ViewAllStashPanel => this.Address == 0L ? (Element) null : this[4];

    public long TotalStashes
    {
      get
      {
        Element stashInventoryPanel = this.StashInventoryPanel;
        return stashInventoryPanel == null ? 0L : stashInventoryPanel.ChildCount;
      }
    }

    public Element ViewAllStashesButton => this.GetObject<Element>(this._cache.Value.ViewAllStashesButtonPtr);

    public Element PinStashTabListButton => this.GetObject<Element>(this._cache.Value.PinStashTabListButtonPtr);

    public StashTopTabSwitcher TabSwitchBar => this.GetObject<StashTopTabSwitcher>(this._cache.Value.TabSwitchBarPtr);

    public Inventory VisibleStash
    {
      get
      {
        Inventory inventoryByIndex = this.GetStashInventoryByIndex(this.VisibleStashIndex);
        Inventory visibleStash;
        if (inventoryByIndex != null && inventoryByIndex.IsVisible)
        {
          visibleStash = inventoryByIndex;
        }
        else
        {
          Inventory inventory = inventoryByIndex;
          visibleStash = this.AllInventories.FirstOrDefault<Inventory>((Func<Inventory, bool>) (x => x != null && x.IsVisible)) ?? inventory;
        }
        return visibleStash;
      }
    }

    public IList<Inventory> AllInventories => this.GetAllInventories();

    public IList<string> AllStashNames => (IList<string>) this.GetAllStashNames();

    public IList<Element> ViewAllStashPanelChildren
    {
      get
      {
        Element viewAllStashPanel = this.ViewAllStashPanel;
        List<Element> elementList;
        if (viewAllStashPanel == null)
        {
          elementList = (List<Element>) null;
        }
        else
        {
          Element element = viewAllStashPanel.Children.LastOrDefault<Element>((Func<Element, bool>) (x => x.ChildCount == this.TotalStashes));
          elementList = element != null ? element.Children.Where<Element>((Func<Element, bool>) (x => x.ChildCount > 0L)).ToList<Element>() : (List<Element>) null;
        }
        return (IList<Element>) elementList ?? (IList<Element>) new List<Element>();
      }
    }

    public IList<Element> TabListButtons
    {
      get
      {
        IList<Element> stashPanelChildren = this.ViewAllStashPanelChildren;
        return (stashPanelChildren != null ? (IList<Element>) stashPanelChildren.Take<Element>((int) this.TotalStashes).ToList<Element>() : (IList<Element>) null) ?? (IList<Element>) new List<Element>();
      }
    }

    public string GetStashName(int index) => (long) index >= this.TotalStashes || index < 0 ? string.Empty : StashTabContainer.GetStashNameInternal(this.ViewAllStashPanelChildren, index);

    public virtual Inventory GetStashInventoryByIndex(int index)
    {
      if ((long) index >= this.TotalStashes)
        return (Inventory) null;
      try
      {
        return this.StashInventoryPanel?.GetChildAtIndex(index)?.GetChildAtIndex(0)?.AsObject<Inventory>();
      }
      catch
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
        interpolatedStringHandler.AppendLiteral("Not found inventory stash for index: ");
        interpolatedStringHandler.AppendFormatted<int>(index);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        return (Inventory) null;
      }
    }

    private IList<Inventory> GetAllInventories()
    {
      List<Inventory> allInventories = new List<Inventory>();
      for (int index = 0; (long) index < this.TotalStashes; ++index)
        allInventories.Add(this.GetStashInventoryByIndex(index));
      return (IList<Inventory>) allInventories;
    }

    private List<string> GetAllStashNames()
    {
      List<string> allStashNames = new List<string>();
      IList<Element> stashPanelChildren = this.ViewAllStashPanelChildren;
      for (int index = 0; (long) index < this.TotalStashes; ++index)
        allStashNames.Add(StashTabContainer.GetStashNameInternal(stashPanelChildren, index));
      return allStashNames;
    }

    private static string GetStashNameInternal(IList<Element> viewAllStashPanelChildren, int index)
    {
      string str;
      if (viewAllStashPanelChildren == null)
      {
        str = (string) null;
      }
      else
      {
        Element element = viewAllStashPanelChildren.ElementAt<Element>(index);
        if (element == null)
        {
          str = (string) null;
        }
        else
        {
          IList<Element> children = element.GetChildAtIndex(0).Children;
          str = children != null ? children.LastOrDefault<Element>()?.Text : (string) null;
        }
      }
      return str ?? "";
    }
  }
}
