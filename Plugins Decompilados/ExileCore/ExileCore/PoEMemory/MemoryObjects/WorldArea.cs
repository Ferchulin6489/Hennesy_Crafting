// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.WorldArea
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class WorldArea : RemoteMemoryObject
  {
    private List<WorldArea> connections;
    private List<WorldArea> corruptedAreas;
    private string id;
    private string name;

    public string Id => this.id == null ? (this.id = this.M.ReadStringU(this.M.Read<long>(this.Address))) : this.id;

    public int Index { get; set; }

    public string Name => this.name == null ? (this.name = this.M.ReadStringU(this.M.Read<long>(this.Address + 8L), (int) byte.MaxValue)) : this.name;

    public int Act => this.M.Read<int>(this.Address + 16L);

    public bool IsTown => this.M.Read<byte>(this.Address + 20L) == (byte) 1;

    public bool IsHideout => this.Name.Contains("Hideout") && !this.Name.Contains("Syndicate Hideout");

    public bool HasWaypoint => this.M.Read<byte>(this.Address + 21L) == (byte) 1;

    public int AreaLevel => this.M.Read<int>(this.Address + 38L);

    public int WorldAreaId => this.M.Read<int>(this.Address + 42L);

    public bool IsAtlasMap => this.Id.StartsWith("MapAtlas");

    public bool IsMapWorlds => this.Id.StartsWith("MapWorlds");

    public bool IsCorruptedArea => this.Id.Contains("SideArea") || this.Id.Contains("Sidearea");

    public bool IsMissionArea => this.Id.Contains("Mission");

    public bool IsDailyArea => this.Id.Contains("Daily");

    public bool IsMapTrialArea => this.Id.StartsWith("EndGame_Labyrinth_trials");

    public bool IsLabyrinthArea => !this.IsMapTrialArea && this.Id.Contains("Labyrinth");

    public bool IsAbyssArea => this.Id.Equals("AbyssLeague") || this.Id.Equals("AbyssLeague2") || this.Id.Equals("AbyssLeagueBoss") || this.Id.Equals("AbyssLeagueBoss2") || this.Id.Equals("AbyssLeagueBoss3");

    public bool IsUnique => this.M.Read<bool>(this.Address + 492L);

    public IList<WorldArea> Connections
    {
      get
      {
        if (this.connections == null)
        {
          this.connections = new List<WorldArea>();
          int num = this.M.Read<int>(this.Address + 22L);
          long addr = this.M.Read<long>(this.Address + 30L);
          if (num > 30)
            return (IList<WorldArea>) this.connections;
          for (int index = 0; index < num; ++index)
          {
            this.connections.Add(this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(addr)));
            addr += 8L;
          }
        }
        return (IList<WorldArea>) this.connections;
      }
    }

    public IList<WorldArea> CorruptedAreas
    {
      get
      {
        if (this.corruptedAreas == null)
        {
          this.corruptedAreas = new List<WorldArea>();
          long addr = this.M.Read<long>(this.Address + 259L);
          int num = this.M.Read<int>(this.Address + 251L);
          if (num > 30)
            return (IList<WorldArea>) this.corruptedAreas;
          for (int index = 0; index < num; ++index)
          {
            this.corruptedAreas.Add(this.TheGame.Files.WorldAreas.GetByAddress(this.M.Read<long>(addr)));
            addr += 8L;
          }
        }
        return (IList<WorldArea>) this.corruptedAreas;
      }
    }

    public override string ToString() => this.Name ?? "";
  }
}
