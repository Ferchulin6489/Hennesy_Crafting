// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.MapStashTabElementQ
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class MapStashTabElementQ : Element
  {
    public Dictionary<string, string> MapsCount => this.GetMapsCount();

    public Dictionary<string, string> CurrentCell => this.GetCurrentCell();

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
          key = text != null ? text.Substring(0, text.IndexOf('\n')) : "Error";
        }
        string text1 = element.Children[4].Text;
        currentCell.Add(key, text1);
      }
      return currentCell;
    }

    private Dictionary<string, string> GetMapsCount()
    {
      IEnumerable<Element> elements = this.Children[0].Children.Concat<Element>((IEnumerable<Element>) this.Children[1].Children);
      Dictionary<string, string> mapsCount = new Dictionary<string, string>();
      foreach (Element element in elements)
        mapsCount.Add(element.Children[0].Text, element.Children[1].Text);
      return mapsCount;
    }
  }
}
