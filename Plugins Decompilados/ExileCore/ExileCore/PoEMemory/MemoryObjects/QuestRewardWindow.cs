// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.QuestRewardWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;


#nullable enable
namespace ExileCore.PoEMemory.MemoryObjects
{
  public class QuestRewardWindow : Element
  {
    public 
    #nullable disable
    IList<(Entity, Element)> GetPossibleRewards()
    {
      Element childFromIndices = this.GetChildFromIndices(5, 0);
      List<Element> elementList = childFromIndices != null ? childFromIndices.Children.SelectMany<Element, Element>((Func<Element, IEnumerable<Element>>) (x => (IEnumerable<Element>) x.Children)).ToList<Element>() : (List<Element>) null;
      List<(Entity, Element)> possibleRewards = new List<(Entity, Element)>();
      foreach (Element element in elementList)
      {
        Entity entity = element.GetChildFromIndices(0, 1)?.Entity;
        possibleRewards.Add((entity, element));
      }
      return (IList<(Entity, Element)>) possibleRewards;
    }

    public Element CancelButton => this.GetChildAtIndex(3);

    public Element SelectOneRewardString => this.GetChildAtIndex(0);
  }
}
