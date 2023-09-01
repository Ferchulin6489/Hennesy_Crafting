// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.HideoutWrapper
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class HideoutWrapper : RemoteMemoryObject
  {
    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public WorldArea WorldArea1 => this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public WorldArea WorldArea2 => this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(this.Address + 48L));

    public WorldArea WorldArea3 => this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(this.Address + 64L));
  }
}
