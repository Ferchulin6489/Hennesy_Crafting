// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Ancestor.AncestorFightSelectionWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects.Ancestor
{
  public class AncestorFightSelectionWindow : Element
  {
    public Element TableContainer => this[2];

    public List<AncestorFightSelectionOpponentLine> Options
    {
      get
      {
        Element tableContainer = this.TableContainer;
        List<AncestorFightSelectionOpponentLine> selectionOpponentLineList;
        if (tableContainer == null)
          selectionOpponentLineList = (List<AncestorFightSelectionOpponentLine>) null;
        else
          selectionOpponentLineList = tableContainer.GetChildFromIndices(0, 0, 2)?.GetChildrenAs<AncestorFightSelectionOpponentLine>();
        return selectionOpponentLineList ?? new List<AncestorFightSelectionOpponentLine>();
      }
    }
  }
}
