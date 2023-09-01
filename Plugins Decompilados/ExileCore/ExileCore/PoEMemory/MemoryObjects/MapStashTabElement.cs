// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.MapStashTabElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class MapStashTabElement : Element
  {
    private long mapListStartPtr => this.Address == 0L ? 0L : this.M.Read<long>(this.Address + 2520L);

    private long mapListEndPtr => this.Address == 0L ? 0L : this.M.Read<long>(this.Address + 2520L + 8L);

    public int TotalInventories => (int) ((this.mapListEndPtr - this.mapListStartPtr) / 16L);

    public Dictionary<MapSubInventoryKey, MapSubInventoryInfo> MapsCount => this.GetMapsCount();

    public Dictionary<string, string> MapsCountByName => this.GetMapsCount2();

    public Dictionary<string, string> MapsCountByTier => this.GetMapsCountFromUi();

    public Dictionary<string, string> CurrentCell => this.GetCurrentCell();

    public Dictionary<string, Element> CurrentCellElements => this.GetCurrentCellElements();

    public Dictionary<string, Element> TierElements => this.GetTierElements();

    private Dictionary<MapSubInventoryKey, MapSubInventoryInfo> GetMapsCount()
    {
      Dictionary<MapSubInventoryKey, MapSubInventoryInfo> mapsCount = new Dictionary<MapSubInventoryKey, MapSubInventoryInfo>();
      int totalInventories = this.TotalInventories;
      if (totalInventories > 1024)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 4);
        interpolatedStringHandler.AppendFormatted(nameof (MapStashTabElement));
        interpolatedStringHandler.AppendLiteral("-> ");
        interpolatedStringHandler.AppendFormatted(nameof (GetMapsCount));
        interpolatedStringHandler.AppendLiteral(" error ");
        interpolatedStringHandler.AppendFormatted("TotalInventories");
        interpolatedStringHandler.AppendLiteral(" = ");
        interpolatedStringHandler.AppendFormatted<int>(totalInventories);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        return (Dictionary<MapSubInventoryKey, MapSubInventoryInfo>) null;
      }
      for (int index = 0; index < totalInventories; ++index)
      {
        MapSubInventoryInfo subInventoryInfo = new MapSubInventoryInfo();
        MapSubInventoryKey key = new MapSubInventoryKey();
        subInventoryInfo.Tier = this.SubInventoryMapTier(index);
        subInventoryInfo.Count = this.SubInventoryMapCount(index);
        subInventoryInfo.MapName = this.SubInventoryMapName(index);
        key.Path = this.SubInventoryMapPath(index);
        key.Type = this.SubInventoryMapType(index);
        mapsCount.Add(key, subInventoryInfo);
      }
      return mapsCount;
    }

    private Dictionary<string, string> GetMapsCount2()
    {
      Dictionary<MapSubInventoryKey, MapSubInventoryInfo> mapsCount = this.GetMapsCount();
      Dictionary<string, string> mapsCount2 = new Dictionary<string, string>();
      foreach (KeyValuePair<MapSubInventoryKey, MapSubInventoryInfo> keyValuePair in (IEnumerable<KeyValuePair<MapSubInventoryKey, MapSubInventoryInfo>>) mapsCount.OrderBy<KeyValuePair<MapSubInventoryKey, MapSubInventoryInfo>, int>((Func<KeyValuePair<MapSubInventoryKey, MapSubInventoryInfo>, int>) (x => x.Value.Tier)))
      {
        string str = keyValuePair.Key.Type == MapType.Shaped ? "Shaped" : "";
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
        interpolatedStringHandler.AppendFormatted<int>(keyValuePair.Value.Tier);
        interpolatedStringHandler.AppendLiteral(": ");
        interpolatedStringHandler.AppendFormatted(str);
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted(keyValuePair.Value.MapName);
        string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
        interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<int>(keyValuePair.Value.Count);
        string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
        mapsCount2[stringAndClear1] = stringAndClear2;
      }
      return mapsCount2;
    }

    private int SubInventoryMapTier(int index) => this.M.Read<int>(this.mapListStartPtr + (long) (index * 16), new int[1]);

    private int SubInventoryMapCount(int index) => this.M.Read<int>(this.mapListStartPtr + (long) (index * 16), 8);

    private MapType SubInventoryMapType(int index) => (MapType) this.M.Read<int>(this.mapListStartPtr + (long) (index * 16), 16);

    private string SubInventoryMapPath(int index) => this.M.ReadStringU(this.M.Read<long>(this.mapListStartPtr + (long) (index * 16), 40, 0));

    private string SubInventoryMapName(int index) => this.M.ReadStringU(this.M.Read<long>(this.mapListStartPtr + (long) (index * 16), 40, 32));

    private Dictionary<string, Element> GetCurrentCellElements()
    {
      IList<Element> children = this.Children[2].Children[0].Children[0].Children;
      Dictionary<string, Element> currentCellElements = new Dictionary<string, Element>();
      foreach (Element element in (IEnumerable<Element>) children)
      {
        string key = element?.Tooltip?.Children?[0].Children[0].Children[3].Text;
        if (key == null)
        {
          string text = element.Tooltip?.Text;
          key = text == null ? "Error" : text.Substring(0, text.IndexOf('\n'));
        }
        string text1 = element.Children[4].Text;
        currentCellElements.Add(key, element);
      }
      return currentCellElements;
    }

    private Dictionary<string, string> GetCurrentCell()
    {
      IList<Element> children = this.Children[2].Children[0].Children[0].Children;
      Dictionary<string, string> currentCell = new Dictionary<string, string>();
      foreach (Element element in (IEnumerable<Element>) children)
      {
        string key = element?.Tooltip?.Children?[0].Children[0].Children[3].Text;
        if (key == null)
        {
          string text = element.Tooltip?.Text;
          key = text == null ? "Error" : text.Substring(0, text.IndexOf('\n'));
        }
        string text1 = element.Children[4].Text;
        currentCell.Add(key, text1);
      }
      return currentCell;
    }

    private Dictionary<string, string> GetMapsCountFromUi()
    {
      IEnumerable<Element> elements = this.Children[0].Children.Concat<Element>((IEnumerable<Element>) this.Children[1].Children);
      Dictionary<string, string> mapsCountFromUi = new Dictionary<string, string>();
      foreach (Element element in elements)
        mapsCountFromUi.Add(element.Children[0].Text, element.Children[1].Text);
      return mapsCountFromUi;
    }

    private Dictionary<string, Element> GetTierElements()
    {
      IEnumerable<Element> elements = this.Children[0].Children.Concat<Element>((IEnumerable<Element>) this.Children[1].Children);
      Dictionary<string, Element> tierElements = new Dictionary<string, Element>();
      int num = 1;
      foreach (Element element in elements)
      {
        string text = element.Children[0].Text;
        if (text != "U")
        {
          text = num.ToString();
          ++num;
        }
        tierElements[text] = element;
      }
      return tierElements;
    }
  }
}
