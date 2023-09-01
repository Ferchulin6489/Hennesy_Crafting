// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.SubterraneanChart
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Elements
{
  public class SubterraneanChart : Element
  {
    private DelveElement _grid;

    public DelveElement GridElement
    {
      get
      {
        if (this.Address == 0L)
          return (DelveElement) null;
        DelveElement grid = this._grid;
        if (grid != null)
          return grid;
        return this._grid = this.GetObject<DelveElement>(this.M.Read<long>(this.Address + 520L, 1744));
      }
    }
  }
}
