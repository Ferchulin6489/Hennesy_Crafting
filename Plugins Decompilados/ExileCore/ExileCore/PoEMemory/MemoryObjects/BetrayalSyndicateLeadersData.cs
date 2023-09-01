// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.BetrayalSyndicateLeadersData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System.Collections.Generic;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class BetrayalSyndicateLeadersData : RemoteMemoryObject
  {
    public List<BetrayalSyndicateState> Leaders => new List<BetrayalSyndicateState>()
    {
      this.ReadObjectAt<BetrayalSyndicateState>(0),
      this.ReadObjectAt<BetrayalSyndicateState>(8),
      this.ReadObjectAt<BetrayalSyndicateState>(16),
      this.ReadObjectAt<BetrayalSyndicateState>(24)
    };
  }
}
