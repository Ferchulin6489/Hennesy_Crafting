// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.MapReceptacleWindow
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements
{
  public class MapReceptacleWindow : Element
  {
    public Element CloseMapDialog => this.GetChildAtIndex(3);

    public Element ActivateButton => this.GetChildFromIndices(4);

    public Element MapPiecesPanel => this.GetChildAtIndex(7);

    public List<Element> MapsElements => this.MapPiecesPanel.Children.Where<Element>((Func<Element, bool>) (x =>
    {
      Entity entity = x.Entity;
      return entity != null && entity.IsValid;
    })).ToList<Element>();

    public List<Entity> InsertedMaps => this.MapsElements.Select<Element, Entity>((Func<Element, Entity>) (x => x.Entity)).ToList<Entity>();
  }
}
