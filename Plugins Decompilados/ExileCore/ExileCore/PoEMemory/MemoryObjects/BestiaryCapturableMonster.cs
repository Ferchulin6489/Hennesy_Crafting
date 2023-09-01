// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BestiaryCapturableMonster
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BestiaryCapturableMonster : RemoteMemoryObject
  {
    private BestiaryCapturableMonster bestiaryCapturableMonsterKey;
    private BestiaryGenus bestiaryGenus;
    private BestiaryGroup bestiaryGroup;
    private string monsterName;
    private MonsterVariety monsterVariety;

    public int Id { get; set; }

    public string MonsterName => this.monsterName == null ? (this.monsterName = this.M.ReadStringU(this.M.Read<long>(this.Address + 32L))) : this.monsterName;

    public MonsterVariety MonsterVariety => this.monsterVariety == null ? (this.monsterVariety = this.TheGame.Files.MonsterVarieties.GetByAddress(this.M.Read<long>(this.Address + 8L))) : this.monsterVariety;

    public BestiaryGroup BestiaryGroup => this.bestiaryGroup == null ? (this.bestiaryGroup = this.TheGame.Files.BestiaryGroups.GetByAddress(this.M.Read<long>(this.Address + 24L))) : this.bestiaryGroup;

    public long BestiaryEncountersPtr => this.M.Read<long>(this.Address + 48L);

    public BestiaryCapturableMonster BestiaryCapturableMonsterKey => this.bestiaryCapturableMonsterKey == null ? (this.bestiaryCapturableMonsterKey = this.TheGame.Files.BestiaryCapturableMonsters.GetByAddress(this.M.Read<long>(this.Address + 106L))) : this.bestiaryCapturableMonsterKey;

    public BestiaryGenus BestiaryGenus => this.bestiaryGenus == null ? (this.bestiaryGenus = this.TheGame.Files.BestiaryGenuses.GetByAddress(this.M.Read<long>(this.Address + 97L))) : this.bestiaryGenus;

    public int AmountCaptured => this.TheGame.IngameState.ServerData.GetBeastCapturedAmount(this);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 4);
      interpolatedStringHandler.AppendLiteral("Nane: ");
      interpolatedStringHandler.AppendFormatted(this.MonsterName);
      interpolatedStringHandler.AppendLiteral(", Group: ");
      interpolatedStringHandler.AppendFormatted(this.BestiaryGroup.Name);
      interpolatedStringHandler.AppendLiteral(", Family: ");
      interpolatedStringHandler.AppendFormatted(this.BestiaryGroup.Family.Name);
      interpolatedStringHandler.AppendLiteral(", Captured: ");
      interpolatedStringHandler.AppendFormatted<int>(this.AmountCaptured);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
