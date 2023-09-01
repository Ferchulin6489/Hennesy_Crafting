// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ArchnemesisPanelElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class ArchnemesisPanelElement : Element
  {
    public IList<ArchnemesisInventorySlot> InventoryElements
    {
      get
      {
        Element childFromIndices = this.GetChildFromIndices(2, 0, 0);
        return childFromIndices == null ? (IList<ArchnemesisInventorySlot>) null : (IList<ArchnemesisInventorySlot>) childFromIndices.GetChildrenAs<ArchnemesisInventorySlot>().Where<ArchnemesisInventorySlot>((Func<ArchnemesisInventorySlot, bool>) (x => x.HasItem)).ToList<ArchnemesisInventorySlot>();
      }
    }
  }
}
