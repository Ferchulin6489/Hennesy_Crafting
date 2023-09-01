// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalData : RemoteMemoryObject
  {
    public BetrayalSyndicateLeadersData SyndicateLeadersData => this.GetObject<BetrayalSyndicateLeadersData>(this.M.Read<long>(this.Address + 720L));

    public List<BetrayalSyndicateState> SyndicateStates
    {
      get
      {
        long startAddress = this.M.Read<long>(this.Address + 800L);
        return this.M.ReadStructsArray<BetrayalSyndicateState>(startAddress, startAddress + (long) (BetrayalSyndicateState.STRUCT_SIZE * 14), BetrayalSyndicateState.STRUCT_SIZE, (RemoteMemoryObject) this).Where<BetrayalSyndicateState>((Func<BetrayalSyndicateState, bool>) (x => x.Target != null)).ToList<BetrayalSyndicateState>();
      }
    }

    public BetrayalEventData BetrayalEventData
    {
      get
      {
        long address = this.M.Read<long>(this.Address + 824L, 816);
        return address != 0L ? this.GetObject<BetrayalEventData>(address) : (BetrayalEventData) null;
      }
    }
  }
}
