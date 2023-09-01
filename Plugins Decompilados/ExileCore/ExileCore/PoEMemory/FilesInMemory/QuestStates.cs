// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.QuestStates
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class QuestStates : UniversalFileWrapper<QuestState>
  {
    private Dictionary<(string, int), QuestState> _questStatesDictionary;

    public QuestStates(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    public QuestState GetQuestState(string questId, int stateId)
    {
      if (this._questStatesDictionary == null)
        this._questStatesDictionary = this.EntriesList.ToDictionary<QuestState, (string, int)>((Func<QuestState, (string, int)>) (x => (x.Quest.Id.ToLowerInvariant(), x.QuestStateId)));
      return this._questStatesDictionary.GetValueOrDefault<(string, int), QuestState>((questId.ToLowerInvariant(), stateId));
    }
  }
}
