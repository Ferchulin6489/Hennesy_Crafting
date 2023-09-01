// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Inventory
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Interfaces;
using GameOffsets;
using System;
using System.Collections.Generic;
using System.Linq;


#nullable enable
namespace ExileCore.PoEMemory.MemoryObjects
{
  public class Inventory : Element
  {
    private readonly 
    #nullable disable
    CachedValue<InventoryOffsets> _cachedValue;
    private readonly CachedValue<Inventory> _nestedInventory;
    private readonly CachedValue<bool> _isNestedInventory;
    private InventoryType _cacheInventoryType;

    public Inventory()
    {
      this._cachedValue = (CachedValue<InventoryOffsets>) new FrameCache<InventoryOffsets>((Func<InventoryOffsets>) (() =>
      {
        IMemory m = this.M;
        Element containerElement = this.OffsetContainerElement;
        long addr = containerElement != null ? containerElement.Address : 0L;
        return m.Read<InventoryOffsets>(addr);
      }));
      this._nestedInventory = (CachedValue<Inventory>) new FrameCache<Inventory>((Func<Inventory>) (() => !this.IsNestedInventory ? (Inventory) null : this.GetNestedVisibleInventory()));
      this._isNestedInventory = (CachedValue<bool>) new FrameCache<bool>((Func<bool>) (() => this.ChildCount == 2L && this[1].ChildCount == 5L));
    }

    protected virtual Element OffsetContainerElement => this.GetChildAtIndex(0);

    private InventoryOffsets InventoryStruct
    {
      get
      {
        Inventory visibleInventory = this.NestedVisibleInventory;
        return visibleInventory == null ? this._cachedValue.Value : visibleInventory.InventoryStruct;
      }
    }

    public ServerInventory ServerInventory => this.GetServerInventory();

    public long ItemCount => this.InventoryStruct.ItemCount;

    public long TotalBoxesInInventoryRow => (long) this.InventoryStruct.InventorySize.X;

    public NormalInventoryItem HoverItem => this.InventoryStruct.HoverItem != 0L ? this.GetObject<NormalInventoryItem>(this.InventoryStruct.HoverItem) : (NormalInventoryItem) null;

    public int X => this.InventoryStruct.RealPos.X;

    public int Y => this.InventoryStruct.RealPos.Y;

    public int XFake => this.InventoryStruct.FakePos.X;

    public int YFake => this.InventoryStruct.FakePos.Y;

    public bool CursorHoverInventory => this.InventoryStruct.CursorInInventory == 1;

    public bool IsNestedInventory => this._isNestedInventory.Value;

    public InventoryType InvType => this.GetInvType();

    public Element InventoryUIElement
    {
      get
      {
        if (!this.IsNestedInventory)
          return this.getInventoryElement();
        return this.NestedVisibleInventory?.InventoryUIElement;
      }
    }

    private Inventory NestedVisibleInventory => this._nestedInventory.Value;

    private StashTabContainer NestedStashContainer => this[1]?.AsObject<StashTabContainer>();

    public int? NestedVisibleInventoryIndex
    {
      get
      {
        if (!this.IsNestedInventory)
          return new int?();
        return this.NestedStashContainer?.VisibleStashIndex;
      }
    }

    public StashTopTabSwitcher NestedTabSwitchBar
    {
      get
      {
        if (!this.IsNestedInventory)
          return (StashTopTabSwitcher) null;
        return this.NestedStashContainer?.TabSwitchBar;
      }
    }

    public IList<NormalInventoryItem> VisibleInventoryItems
    {
      get
      {
        if (this.IsNestedInventory)
          return this.NestedVisibleInventory?.VisibleInventoryItems ?? (IList<NormalInventoryItem>) new List<NormalInventoryItem>();
        Element inventoryUiElement = this.InventoryUIElement;
        if (inventoryUiElement == null || inventoryUiElement.Address == 0L)
          return (IList<NormalInventoryItem>) null;
        List<NormalInventoryItem> visibleInventoryItems = new List<NormalInventoryItem>();
        switch (this.InvType)
        {
          case InventoryType.PlayerInventory:
          case InventoryType.NormalStash:
          case InventoryType.QuadStash:
          case InventoryType.VendorInventory:
            visibleInventoryItems.AddRange(inventoryUiElement.GetChildrenAs<NormalInventoryItem>().Skip<NormalInventoryItem>(1));
            break;
          case InventoryType.CurrencyStash:
            Element element1 = (Element) null;
            if (this.Children[1].IsVisible)
              element1 = this.Children[1];
            else if (this.Children[2].IsVisible)
              element1 = this.Children[2];
            if (element1 != null)
            {
              foreach (Element child in (IEnumerable<Element>) element1.Children)
              {
                if (child.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) child[1].AsObject<EssenceInventoryItem>());
              }
            }
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<EssenceInventoryItem>());
              }
              break;
            }
          case InventoryType.EssenceStash:
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<EssenceInventoryItem>());
              }
              break;
            }
          case InventoryType.DivinationStash:
            Element childAtIndex1 = inventoryUiElement.GetChildAtIndex(0);
            List<NormalInventoryItem> normalInventoryItemList;
            if (childAtIndex1 == null)
            {
              normalInventoryItemList = (List<NormalInventoryItem>) null;
            }
            else
            {
              Element childAtIndex2 = childAtIndex1.GetChildAtIndex(1);
              normalInventoryItemList = childAtIndex2 != null ? childAtIndex2.Children.Where<Element>((Func<Element, bool>) (x => x != null && x.IsVisibleLocal && x.ChildCount > 1L && x.GetChildAtIndex(1).ChildCount > 1L)).Select<Element, NormalInventoryItem>((Func<Element, NormalInventoryItem>) (x =>
              {
                Element childAtIndex3 = x.GetChildAtIndex(1);
                if (childAtIndex3 == null)
                  return (NormalInventoryItem) null;
                Element childAtIndex4 = childAtIndex3.GetChildAtIndex(1);
                return childAtIndex4 == null ? (NormalInventoryItem) null : (NormalInventoryItem) childAtIndex4.AsObject<DivinationInventoryItem>();
              })).Where<NormalInventoryItem>((Func<NormalInventoryItem, bool>) (x => x != null)).ToList<NormalInventoryItem>() : (List<NormalInventoryItem>) null;
            }
            return (IList<NormalInventoryItem>) normalInventoryItemList ?? (IList<NormalInventoryItem>) visibleInventoryItems;
          case InventoryType.MapStash:
            if (this.ChildCount > 3L)
            {
              foreach (Element child1 in (IEnumerable<Element>) this.Children[3].Children)
              {
                if (child1.IsVisible && child1.ChildCount > 1L)
                {
                  for (int index = 1; (long) index < child1.ChildCount; ++index)
                  {
                    Element child2 = child1.Children[index];
                    if (child2.ChildCount == 1L)
                      visibleInventoryItems.Add(child2.AsObject<NormalInventoryItem>());
                  }
                }
              }
              visibleInventoryItems.Sort((Comparison<NormalInventoryItem>) ((i1, i2) => (i1.PositionNum.X * 6f + i1.PositionNum.Y).CompareTo(i2.PositionNum.X * 6f + i2.PositionNum.Y)));
              break;
            }
            break;
          case InventoryType.FragmentStash:
            Element element2 = (Element) null;
            if (this.Children[0].IsVisible)
              element2 = this.Children[0];
            else if (this.Children[1].IsVisible)
              element2 = this.Children[1];
            else if (this.Children[2].IsVisible)
              element2 = this.Children[2];
            else if (this.Children[3].IsVisible)
              element2 = (Element) null;
            if (element2 != null)
            {
              using (IEnumerator<Element> enumerator = element2.Children.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Element current = enumerator.Current;
                  if (current.ChildCount > 1L)
                    visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<FragmentInventoryItem>());
                }
                break;
              }
            }
            else
              break;
          case InventoryType.DelveStash:
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<DelveInventoryItem>());
              }
              break;
            }
          case InventoryType.BlightStash:
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<BlightInventoryItem>());
              }
              break;
            }
          case InventoryType.DeliriumStash:
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<DeliriumInventoryItem>());
              }
              break;
            }
          case InventoryType.MetamorphStash:
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add((NormalInventoryItem) current[1].AsObject<MetamorphInventoryItem>());
              }
              break;
            }
          case InventoryType.UniqueStash:
            using (IEnumerator<Element> enumerator = inventoryUiElement.Children.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Element current = enumerator.Current;
                if (current.ChildCount > 1L)
                  visibleInventoryItems.Add(current[1].AsObject<NormalInventoryItem>());
              }
              break;
            }
          case InventoryType.FlaskStash:
            return (IList<NormalInventoryItem>) inventoryUiElement.Children.Where<Element>((Func<Element, bool>) (subInventory => subInventory.IsVisibleLocal)).SelectMany<Element, Element>((Func<Element, IEnumerable<Element>>) (x => (IEnumerable<Element>) x[1]?.Children ?? Enumerable.Empty<Element>())).Select<Element, NormalInventoryItem>((Func<Element, NormalInventoryItem>) (slot =>
            {
              Element element3 = slot[1];
              return element3 == null ? (NormalInventoryItem) null : (NormalInventoryItem) element3.AsObject<FlaskInventoryItem>();
            })).Where<NormalInventoryItem>((Func<NormalInventoryItem, bool>) (x => x != null)).ToList<NormalInventoryItem>();
          case InventoryType.GemStash:
            return (IList<NormalInventoryItem>) inventoryUiElement.Children.Where<Element>((Func<Element, bool>) (subInventory => subInventory.IsVisibleLocal)).SelectMany<Element, Element>((Func<Element, IEnumerable<Element>>) (x => (IEnumerable<Element>) x[1]?.Children ?? Enumerable.Empty<Element>())).Select<Element, NormalInventoryItem>((Func<Element, NormalInventoryItem>) (slot =>
            {
              Element element4 = slot[1];
              return element4 == null ? (NormalInventoryItem) null : (NormalInventoryItem) element4.AsObject<GemInventoryItem>();
            })).Where<NormalInventoryItem>((Func<NormalInventoryItem, bool>) (x => x != null)).ToList<NormalInventoryItem>();
        }
        return (IList<NormalInventoryItem>) visibleInventoryItems;
      }
    }

    public Entity this[int x, int y, int xLength]
    {
      get
      {
        long num = this.M.Read<long>(this.Address + 1040L, 1600, 56);
        y *= xLength;
        long addressPointer = this.M.Read<long>(num + (long) ((x + y) * 8));
        return addressPointer <= 0L ? (Entity) null : this.ReadObject<Entity>(addressPointer);
      }
    }

    protected virtual InventoryType GetInvType()
    {
      if (this.IsNestedInventory)
      {
        Inventory visibleInventory = this.NestedVisibleInventory;
        return visibleInventory == null ? InventoryType.InvalidInventory : visibleInventory.InvType;
      }
      if (this._cacheInventoryType != InventoryType.InvalidInventory)
        return this._cacheInventoryType;
      if (this.Address == 0L)
        return InventoryType.InvalidInventory;
      for (int k = 1; k < InventoryList.InventoryCount; ++k)
      {
        if (this.TheGame.IngameState.IngameUi.InventoryPanel[(InventoryIndex) k].Address == this.Address)
        {
          this._cacheInventoryType = InventoryType.PlayerInventory;
          return this._cacheInventoryType;
        }
      }
      long childCount1 = this.ChildCount;
      if (childCount1 <= 18L)
      {
        switch (childCount1 - 1L)
        {
          case 0:
            this._cacheInventoryType = this.TotalBoxesInInventoryRow != 24L ? InventoryType.NormalStash : InventoryType.QuadStash;
            goto label_38;
          case 1:
          case 2:
          case 5:
          case 7:
            break;
          case 3:
            long? childCount2 = this[0]?.ChildCount;
            InventoryType inventoryType;
            if (childCount2.HasValue)
            {
              switch (childCount2.GetValueOrDefault())
              {
                case 4:
                  inventoryType = InventoryType.GemStash;
                  goto label_36;
                case 5:
                  inventoryType = InventoryType.FlaskStash;
                  goto label_36;
              }
            }
            inventoryType = InventoryType.InvalidInventory;
label_36:
            this._cacheInventoryType = inventoryType;
            goto label_38;
          case 4:
            this._cacheInventoryType = this.Children[4].ChildCount != 4L ? InventoryType.DivinationStash : InventoryType.FragmentStash;
            goto label_38;
          case 6:
            this._cacheInventoryType = InventoryType.MapStash;
            goto label_38;
          case 8:
            this._cacheInventoryType = InventoryType.UniqueStash;
            goto label_38;
          default:
            switch (childCount1)
            {
              case 17:
                this._cacheInventoryType = InventoryType.MetamorphStash;
                goto label_38;
              case 18:
                this._cacheInventoryType = InventoryType.CurrencyStash;
                goto label_38;
            }
            break;
        }
      }
      else if (childCount1 <= 84L)
      {
        if (childCount1 != 35L)
        {
          if (childCount1 == 84L)
          {
            this._cacheInventoryType = InventoryType.BlightStash;
            goto label_38;
          }
        }
        else
        {
          this._cacheInventoryType = InventoryType.DelveStash;
          goto label_38;
        }
      }
      else if (childCount1 != 88L)
      {
        if (childCount1 == 111L)
        {
          this._cacheInventoryType = InventoryType.EssenceStash;
          goto label_38;
        }
      }
      else
      {
        this._cacheInventoryType = InventoryType.DeliriumStash;
        goto label_38;
      }
      this._cacheInventoryType = InventoryType.InvalidInventory;
label_38:
      return this._cacheInventoryType;
    }

    private Element getInventoryElement()
    {
      switch (this.InvType)
      {
        case InventoryType.PlayerInventory:
        case InventoryType.CurrencyStash:
        case InventoryType.EssenceStash:
        case InventoryType.DivinationStash:
        case InventoryType.FragmentStash:
        case InventoryType.DelveStash:
        case InventoryType.BlightStash:
        case InventoryType.DeliriumStash:
        case InventoryType.MetamorphStash:
        case InventoryType.VendorInventory:
          return (Element) this;
        case InventoryType.NormalStash:
        case InventoryType.QuadStash:
          return this.GetChildAtIndex(0);
        case InventoryType.MapStash:
          Element parent = this.AsObject<Element>().Parent;
          return parent == null ? (Element) null : (Element) parent.AsObject<MapStashTabElement>();
        case InventoryType.UniqueStash:
          return this.AsObject<Element>().Parent;
        case InventoryType.FlaskStash:
        case InventoryType.GemStash:
          return this.GetChildFromIndices(1, 0, 1);
        default:
          return (Element) null;
      }
    }

    private ServerInventory GetServerInventory()
    {
      switch (this.InvType)
      {
        case InventoryType.PlayerInventory:
        case InventoryType.NormalStash:
        case InventoryType.QuadStash:
        case InventoryType.VendorInventory:
          return this.InventoryUIElement?.ReadObjectAt<ServerInventory>(1248);
        case InventoryType.CurrencyStash:
        case InventoryType.EssenceStash:
        case InventoryType.MetamorphStash:
          return this.InventoryUIElement?.ReadObjectAt<Element>(96).ReadObjectAt<ServerInventory>(1248);
        case InventoryType.DivinationStash:
          return this.InventoryUIElement?.ReadObjectAt<ServerInventory>(872);
        case InventoryType.BlightStash:
          return this.InventoryUIElement?.ReadObjectAt<Element>(960).ReadObjectAt<ServerInventory>(1248);
        default:
          return (ServerInventory) null;
      }
    }

    private Inventory GetNestedVisibleInventory() => this.NestedStashContainer?.VisibleStash;
  }
}
