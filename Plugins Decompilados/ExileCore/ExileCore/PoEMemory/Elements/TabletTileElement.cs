// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.TabletTileElement
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.Elements
{
  public class TabletTileElement : Element
  {
    private LakeRoom _room;

    public int TileY => this.M.Read<int>(this.Address + 820L);

    public int TileX => this.M.Read<int>(this.Address + 816L);

    public int Difficulty => this.M.Read<int>(this.Address + 840L);

    public LakeRoom Room => this._room ?? (this._room = this.TheGame.Files.LakeRooms.GetByAddress(this.M.Read<long>(this.Address + 824L)));
  }
}
