// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.AtlasElements.VoidStoneFavouriteMapPanelElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements.AtlasElements
{
  public class VoidStoneFavouriteMapPanelElement : Element
  {
    public Element InfoHoverIcon => this.GetChildAtIndex(0);

    public Element FavouriteMapSlots => this.GetChildAtIndex(1);

    public VoidStoneInventory VoidStoneInventory => this[2].AsObject<VoidStoneInventory>();
  }
}
