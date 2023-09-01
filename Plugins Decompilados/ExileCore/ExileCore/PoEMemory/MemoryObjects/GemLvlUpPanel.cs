// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.GemLvlUpPanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class GemLvlUpPanel : Element
  {
    public IList<Element> GemsToLvlUp => this.GetChildAtIndex(0)?.Children;

    public List<(Entity, Element)> Gems => this.GemsToLvlUp.Select<Element, (Entity, Element)>((Func<Element, (Entity, Element)>) (gem => (gem?.ReadObject<Entity>(gem.Address + 496L), gem))).ToList<(Entity, Element)>();

    public Element LvlUpButtonForGem(Element gem) => gem?.GetChildAtIndex(1);

    public bool MeetRequirementForGem(Element gem)
    {
      string text = this.TextForGem(gem)?.Text;
      return text != null && text.ToLower().Trim() == "click to level up";
    }

    public Element TextForGem(Element gem) => gem?.GetChildAtIndex(3);
  }
}
