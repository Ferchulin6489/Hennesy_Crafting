// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.QuestReward
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Models;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class QuestReward : RemoteMemoryObject
  {
    private QuestRewardOffer _offer;
    private Character _character;
    private BaseItemType _reward;

    public QuestRewardOffer Offer => this._offer ?? (this._offer = this.TheGame.Files.QuestRewardOffers.GetByAddress(this.M.Read<long>(this.Address)));

    public Character Character => this._character ?? (this._character = this.TheGame.Files.Characters.GetByAddress(this.M.Read<long>(this.Address + 20L)));

    public BaseItemType Reward => this._reward ?? (this._reward = this.TheGame.Files.BaseItemTypes.GetFromAddress(this.M.Read<long>(this.Address + 36L)));

    public int RewardLevel => this.M.Read<int>(this.Address + 52L);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 4);
      interpolatedStringHandler.AppendFormatted(this.Offer?.Id);
      interpolatedStringHandler.AppendLiteral(" for ");
      interpolatedStringHandler.AppendFormatted(this.Character?.Name ?? this.Character?.Id);
      interpolatedStringHandler.AppendLiteral(" -> ");
      interpolatedStringHandler.AppendFormatted(this.Reward?.BaseName);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
