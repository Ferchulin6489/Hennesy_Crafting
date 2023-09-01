// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.QuestRewardOffer
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class QuestRewardOffer : RemoteMemoryObject
  {
    private string _id;
    private Quest _quest;

    public string Id => this._id ?? (this._id = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public Quest Quest => this._quest ?? (this._quest = this.TheGame.Files.Quests.GetByAddress(this.M.Read<long>(this.Address + 8L)));

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
      interpolatedStringHandler.AppendFormatted(this.Id);
      interpolatedStringHandler.AppendLiteral(" (");
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
