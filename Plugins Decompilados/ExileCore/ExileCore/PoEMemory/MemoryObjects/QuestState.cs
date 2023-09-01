// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.QuestState
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using GameOffsets;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class QuestState : RemoteMemoryObject
  {
    public QuestStateOffsets QuestStateOffsets => this.M.Read<QuestStateOffsets>(this.Address);

    public long QuestPtr => this.QuestStateOffsets.QuestAddress;

    public Quest Quest => this.TheGame.Files.Quests.GetByAddress(this.QuestPtr);

    public int QuestStateId => (int) this.QuestStateOffsets.QuestStateId;

    public string QuestStateText => this.M.ReadStringU(this.QuestStateOffsets.QuestStateTextAddress);

    public string QuestProgressText => this.M.ReadStringU(this.QuestStateOffsets.QuestProgressTextAddress);

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 4);
      interpolatedStringHandler.AppendLiteral("Id: ");
      interpolatedStringHandler.AppendFormatted<int>(this.QuestStateId);
      interpolatedStringHandler.AppendLiteral(", Quest.Id: ");
      interpolatedStringHandler.AppendFormatted(this.Quest.Id);
      interpolatedStringHandler.AppendLiteral(", ProgressText ");
      interpolatedStringHandler.AppendFormatted(this.QuestProgressText);
      interpolatedStringHandler.AppendLiteral(", QuestName: ");
      interpolatedStringHandler.AppendFormatted(this.Quest.Name);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
