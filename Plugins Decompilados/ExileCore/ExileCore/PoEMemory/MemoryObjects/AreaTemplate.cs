// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.AreaTemplate
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class AreaTemplate : RemoteMemoryObject
  {
    public string RawName => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public int Act => this.M.Read<int>(this.Address + 16L);

    public bool IsTown => this.M.Read<byte>(this.Address + 20L) == (byte) 1;

    public bool HasWaypoint => this.M.Read<byte>(this.Address + 21L) == (byte) 1;

    public int NominalLevel => this.M.Read<int>(this.Address + 38L);

    public int MonsterLevel => this.M.Read<int>(this.Address + 38L);

    public int WorldAreaId => this.M.Read<int>(this.Address + 42L);

    public int CorruptedAreasVariety => this.M.Read<int>(this.Address + 251L);

    public List<WorldArea> PossibleCorruptedAreas => this._PossibleCorruptedAreas(this.Address + 259L, this.CorruptedAreasVariety);

    private List<WorldArea> _PossibleCorruptedAreas(long address, int count)
    {
      List<WorldArea> worldAreaList = new List<WorldArea>();
      for (int index = 0; index < count; ++index)
        worldAreaList.Add(this.GetObject<WorldArea>(address + (long) (index * 8)));
      return worldAreaList;
    }
  }
}
