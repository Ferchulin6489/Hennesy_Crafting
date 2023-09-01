// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.HarvestWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class HarvestWindow : Element
  {
    public List<HarvestCraftElement> Crafts => this.GetChildFromIndices(8, 0, 1).Children.Select<Element, HarvestCraftElement>((Func<Element, HarvestCraftElement>) (x => x.GetChildAtIndex(3).AsObject<HarvestCraftElement>())).ToList<HarvestCraftElement>();
  }
}
