// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.LabyrinthTrial
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class LabyrinthTrial : RemoteMemoryObject
  {
    public WorldArea area;
    private int id = -1;

    public int Id => this.id == -1 ? (this.id = this.M.Read<int>(this.Address + 16L)) : this.id;

    public WorldArea Area
    {
      get
      {
        if (this.area == null)
          this.area = this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(this.Address + 8L));
        return this.area;
      }
    }
  }
}
