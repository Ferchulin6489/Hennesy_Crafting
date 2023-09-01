// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.ArchnemesisAltarElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class ArchnemesisAltarElement : Element
  {
    public IList<ArchnemesisAltarInventorySlot> InventoryElements
    {
      get
      {
        Element childFromIndices = this.GetChildFromIndices(2, 0);
        return childFromIndices == null ? (IList<ArchnemesisAltarInventorySlot>) null : (IList<ArchnemesisAltarInventorySlot>) childFromIndices.GetChildrenAs<ArchnemesisAltarInventorySlot>().Where<ArchnemesisAltarInventorySlot>((Func<ArchnemesisAltarInventorySlot, bool>) (x => x.HasItem)).ToList<ArchnemesisAltarInventorySlot>();
      }
    }
  }
}
