// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.AtlasPanel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Elements.AtlasElements;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Elements
{
  public class AtlasPanel : Element
  {
    public Element AtlasInventory => this.GetObject<Element>(this.M.Read<long>(this.Address + 584L, 944));

    public IList<Element> InventorySlots => this.AtlasInventory.Children;

    public AtlasMasterMissionPanelElement MasterMissionPanel => this[3].AsObject<AtlasMasterMissionPanelElement>();

    public VoidStoneFavouriteMapPanelElement VoidStoneAndFavouriteMapPanel => this[4].AsObject<VoidStoneFavouriteMapPanelElement>();

    public Element SearchbarPanel => this.GetChildAtIndex(6);

    public Element AtlasSkillsToggleButton => this.GetChildAtIndex(7);

    public Element KiracsVaultPassButton => this.GetChildAtIndex(8);

    public Element InnerAtlas => this.GetChildAtIndex(0);

    public Dictionary<Atlasbonus, int> AtlasBonus => new Dictionary<Atlasbonus, int>()
    {
      {
        Atlasbonus.Minimum,
        0
      },
      {
        Atlasbonus.Current,
        int.Parse(this.InnerAtlas.GetChildAtIndex(120).Text.Split('/')[0])
      },
      {
        Atlasbonus.Maximum,
        int.Parse(this.InnerAtlas.GetChildAtIndex(120).Text.Split('/')[1])
      }
    };

    public Element SearingExarchCounterElement => this.InnerAtlas.GetChildAtIndex(121);

    public Element MavenCounterElement => this.InnerAtlas.GetChildAtIndex(122);

    public Element EaterofWorldsCounterElement => this.InnerAtlas.GetChildAtIndex(123);

    public VoidStoneInventory SocketedVoidstones => this.InnerAtlas.GetChildFromIndices(124, 0).AsObject<VoidStoneInventory>();
  }
}
