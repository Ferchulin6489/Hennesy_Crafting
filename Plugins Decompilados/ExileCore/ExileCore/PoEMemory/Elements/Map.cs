// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.Map
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class Map : Element
  {
    private Element _largeMap;
    private Element _smallMap;

    public Element LargeMap => this._largeMap ?? (this._largeMap = this.ReadObjectAt<Element>(640));

    public float LargeMapShiftX => this.M.Read<float>(this.LargeMap.Address + 616L);

    public float LargeMapShiftY => this.M.Read<float>(this.LargeMap.Address + 620L);

    public float LargeMapZoom => this.M.Read<float>(this.LargeMap.Address + 684L);

    public Element SmallMiniMap => this._smallMap ?? (this._smallMap = this.ReadObjectAt<Element>(648));

    public float SmallMinMapX => this.M.Read<float>(this.SmallMiniMap.Address + 616L);

    public float SmallMinMapY => this.M.Read<float>(this.SmallMiniMap.Address + 620L);

    public float SmallMinMapZoom => this.M.Read<float>(this.SmallMiniMap.Address + 684L);

    public Element OrangeWords => this.ReadObjectAt<Element>(680);

    public Element BlueWords => this.ReadObjectAt<Element>(848);
  }
}
