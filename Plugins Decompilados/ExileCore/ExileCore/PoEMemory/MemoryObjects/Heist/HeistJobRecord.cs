// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.Heist.HeistJobRecord
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;

namespace ExileCore.PoEMemory.MemoryObjects.Heist
{
  public class HeistJobRecord : RemoteMemoryObject
  {
    public string Id => this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string Name => this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string RequiredSkillIcon => this.M.ReadStringU(this.M.Read<long>(this.Address + 16L));

    public string SkillIcon => this.M.ReadStringU(this.M.Read<long>(this.Address + 24L));

    public string MapIcon => this.M.ReadStringU(this.M.Read<long>(this.Address + 40L));

    public StatsDat.StatRecord LevelStat => this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(this.Address + 48L));

    public StatsDat.StatRecord AlertStat => this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(this.Address + 64L));

    public StatsDat.StatRecord AlarmStat => this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(this.Address + 80L));

    public StatsDat.StatRecord CostStat => this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(this.Address + 96L));

    public StatsDat.StatRecord ExperienceGainStat => this.TheGame.Files.Stats.GetStatByAddress(this.M.Read<long>(this.Address + 112L));

    public override string ToString() => this.Name;
  }
}
