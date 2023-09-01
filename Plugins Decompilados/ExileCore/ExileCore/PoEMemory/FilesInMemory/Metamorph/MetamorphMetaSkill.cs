// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.Metamorph.MetamorphMetaSkill
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory.Metamorph
{
  public class MetamorphMetaSkill : RemoteMemoryObject
  {
    public MonsterVariety MonsterVarietyMetadata => this.TheGame.Files.MonsterVarieties.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public MetamorphMetaSkillType MetaSkill => this.TheGame.Files.MetamorphMetaSkillTypes.GetByAddress(this.M.Read<long>(this.Address + 24L));

    public string SkillName => this.M.ReadStringU(this.M.Read<long>(this.Address + 232L), (int) byte.MaxValue);

    public string GrantedEffect1 => this.M.ReadStringU(this.M.Read<long>(this.Address + 40L, new int[1]), (int) byte.MaxValue);

    public string GrantedEffect2 => this.M.ReadStringU(this.M.Read<long>(this.Address + 88L, new int[1]), (int) byte.MaxValue);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
      interpolatedStringHandler.AppendFormatted<MetamorphMetaSkillType>(this.MetaSkill);
      interpolatedStringHandler.AppendLiteral(", ");
      interpolatedStringHandler.AppendFormatted(this.MonsterVarietyMetadata?.VarietyId);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
