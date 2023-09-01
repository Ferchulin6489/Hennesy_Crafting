// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Heist.HeistChestRewardTypeRecord
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects.Heist
{
  public class HeistChestRewardTypeRecord : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string Art => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public int MinimumDropLevel => this.M.Read<int>(this.Address + 40L);

    public int MaximumDropLevel => this.M.Read<int>(this.Address + 44L);

    public int Weight => this.M.Read<int>(this.Address + 48L);

    public string RoomName => this.M.ReadStringU(this.M.Read<long>(this.Address + 52L));

    public int RequiredJobLevel => this.M.Read<int>(this.Address + 60L);

    public HeistJobRecord RequiredJob => this.TheGame.Files.HeistJobs.GetByAddress(this.M.Read<long>(this.Address + 68L, 8));

    public override string ToString() => this.Name;
  }
}
