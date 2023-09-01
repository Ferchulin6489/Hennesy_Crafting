// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BestiaryRecipeComponent
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory;
using System.Text;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BestiaryRecipeComponent : RemoteMemoryObject
  {
    private BestiaryCapturableMonster bestiaryCapturableMonster;
    private BestiaryFamily bestiaryFamily;
    private BestiaryGenus bestiaryGenus;
    private BestiaryGroup bestiaryGroup;
    private int minLevel = -1;
    private ModsDat.ModRecord mod;
    private string recipeId;

    public int Id { get; internal set; }

    public string RecipeId => this.recipeId == null ? (this.recipeId = this.M.ReadStringU(this.M.Read<long>(this.Address))) : this.recipeId;

    public int MinLevel => this.minLevel == -1 ? (this.minLevel = this.M.Read<int>(this.Address + 8L)) : this.minLevel;

    public BestiaryFamily BestiaryFamily => this.bestiaryFamily == null ? (this.bestiaryFamily = this.TheGame.Files.BestiaryFamilies.GetByAddress(this.M.Read<long>(this.Address + 20L))) : this.bestiaryFamily;

    public BestiaryGroup BestiaryGroup => this.bestiaryGroup == null ? (this.bestiaryGroup = this.TheGame.Files.BestiaryGroups.GetByAddress(this.M.Read<long>(this.Address + 36L))) : this.bestiaryGroup;

    public BestiaryGenus BestiaryGenus => this.bestiaryGenus == null ? (this.bestiaryGenus = this.TheGame.Files.BestiaryGenuses.GetByAddress(this.M.Read<long>(this.Address + 88L))) : this.bestiaryGenus;

    public ModsDat.ModRecord Mod => this.mod == null ? (this.mod = this.TheGame.Files.Mods.GetModByAddress(this.M.Read<long>(this.Address + 52L))) : this.mod;

    public BestiaryCapturableMonster BestiaryCapturableMonster => this.bestiaryCapturableMonster ?? (this.bestiaryCapturableMonster = this.TheGame.Files.BestiaryCapturableMonsters.GetByAddress(this.M.Read<long>(this.Address + 68L)));

    public override string ToString()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
      interpolatedStringHandler.AppendLiteral("Id: ");
      interpolatedStringHandler.AppendFormatted<int>(this.Id);
      interpolatedStringHandler.AppendLiteral(", ");
      ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
      stringBuilder3.Append(ref local1);
      if (this.MinLevel > 0)
      {
        StringBuilder stringBuilder4 = stringBuilder1;
        StringBuilder stringBuilder5 = stringBuilder4;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder4);
        interpolatedStringHandler.AppendLiteral("MinLevel: ");
        interpolatedStringHandler.AppendFormatted<int>(this.MinLevel);
        interpolatedStringHandler.AppendLiteral(", ");
        ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
        stringBuilder5.Append(ref local2);
      }
      if (this.Mod != null)
      {
        StringBuilder stringBuilder6 = stringBuilder1;
        StringBuilder stringBuilder7 = stringBuilder6;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder6);
        interpolatedStringHandler.AppendLiteral("Mod: ");
        interpolatedStringHandler.AppendFormatted<ModsDat.ModRecord>(this.Mod);
        interpolatedStringHandler.AppendLiteral(", ");
        ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
        stringBuilder7.Append(ref local3);
      }
      if (this.BestiaryCapturableMonster != null)
      {
        StringBuilder stringBuilder8 = stringBuilder1;
        StringBuilder stringBuilder9 = stringBuilder8;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 1, stringBuilder8);
        interpolatedStringHandler.AppendLiteral("MonsterName: ");
        interpolatedStringHandler.AppendFormatted(this.BestiaryCapturableMonster.MonsterName);
        interpolatedStringHandler.AppendLiteral(", ");
        ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
        stringBuilder9.Append(ref local4);
      }
      if (this.BestiaryFamily != null)
      {
        StringBuilder stringBuilder10 = stringBuilder1;
        StringBuilder stringBuilder11 = stringBuilder10;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(18, 1, stringBuilder10);
        interpolatedStringHandler.AppendLiteral("BestiaryFamily: ");
        interpolatedStringHandler.AppendFormatted(this.BestiaryFamily.Name);
        interpolatedStringHandler.AppendLiteral(", ");
        ref StringBuilder.AppendInterpolatedStringHandler local5 = ref interpolatedStringHandler;
        stringBuilder11.Append(ref local5);
      }
      if (this.BestiaryGroup != null)
      {
        StringBuilder stringBuilder12 = stringBuilder1;
        StringBuilder stringBuilder13 = stringBuilder12;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder12);
        interpolatedStringHandler.AppendLiteral("BestiaryGroup: ");
        interpolatedStringHandler.AppendFormatted(this.BestiaryGroup.Name);
        interpolatedStringHandler.AppendLiteral(", ");
        ref StringBuilder.AppendInterpolatedStringHandler local6 = ref interpolatedStringHandler;
        stringBuilder13.Append(ref local6);
      }
      if (this.BestiaryGenus != null)
      {
        StringBuilder stringBuilder14 = stringBuilder1;
        StringBuilder stringBuilder15 = stringBuilder14;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder14);
        interpolatedStringHandler.AppendLiteral("BestiaryGenus: ");
        interpolatedStringHandler.AppendFormatted(this.BestiaryGenus.Name);
        interpolatedStringHandler.AppendLiteral(", ");
        ref StringBuilder.AppendInterpolatedStringHandler local7 = ref interpolatedStringHandler;
        stringBuilder15.Append(ref local7);
      }
      return stringBuilder1.ToString();
    }
  }
}
