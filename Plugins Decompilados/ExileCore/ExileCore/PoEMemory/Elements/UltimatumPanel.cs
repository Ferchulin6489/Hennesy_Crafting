// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.UltimatumPanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.FilesInMemory.Ultimatum;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class UltimatumPanel : Element
  {
    public Element ChoisesPanel => this.GetChildFromIndices(2, 4);

    public int SlectedChoise => this.M.Read<int>(this.ChoisesPanel.Address + 584L);

    public UltimatumModifier[] Modifiers
    {
      get
      {
        long addressPointer = this.M.Read<long>(this.ChoisesPanel.Address + 560L);
        return new UltimatumModifier[3]
        {
          this.ReadObject<UltimatumModifier>(addressPointer),
          this.ReadObject<UltimatumModifier>(addressPointer + 16L),
          this.ReadObject<UltimatumModifier>(addressPointer + 32L)
        };
      }
    }

    public IList<Element> ChoisesElements => this.ChoisesPanel.GetChildAtIndex(0).Children;

    public Element InventoryElement => this.GetObject<Element>(this.M.Read<long>(this.Address + 464L, 624));

    public NormalInventoryItem NextRewardItem => this.InventoryElement.GetChildAtIndex(1).AsObject<NormalInventoryItem>();
  }
}
