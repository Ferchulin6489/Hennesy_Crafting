// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Sanctum.SanctumRoom
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory.Sanctum
{
  public class SanctumRoom : RemoteMemoryObject
  {
    private SanctumRoomType _roomType;

    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string Arm => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string Teleports => this.M.ReadStringU(this.M.Read<long>(this.Address + 32L));

    public SanctumRoomType RoomType => this._roomType ?? (this._roomType = this.TheGame.Files.SanctumRoomTypes.GetByAddress(this.M.Read<long>(this.Address + 16L)));

    public override string ToString() => this.Id;
  }
}
