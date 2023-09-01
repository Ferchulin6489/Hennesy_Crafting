// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.Sanctum.SanctumRoomData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.FilesInMemory.Sanctum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.Elements.Sanctum
{
  public class SanctumRoomData : RemoteMemoryObject
  {
    public SanctumRoom FightRoom => this.TheGame.Files.SanctumRooms.GetByAddress(this.M.Read<long>(this.Address + 8L));

    public SanctumRoom RewardRoom => this.TheGame.Files.SanctumRooms.GetByAddress(this.M.Read<long>(this.Address + 24L));

    public SanctumPersistentEffect RoomEffect => this.TheGame.Files.SanctumPersistentEffects.GetByAddress(this.M.Read<long>(this.Address + 40L));

    public SanctumDeferredRewardCategory Reward1 => this.TheGame.Files.SanctumDeferredRewardCategories.GetByAddress(this.M.Read<long>(this.Address + 64L));

    public SanctumDeferredRewardCategory Reward2 => this.TheGame.Files.SanctumDeferredRewardCategories.GetByAddress(this.M.Read<long>(this.Address + 80L));

    public SanctumDeferredRewardCategory Reward3 => this.TheGame.Files.SanctumDeferredRewardCategories.GetByAddress(this.M.Read<long>(this.Address + 96L));

    public List<SanctumDeferredRewardCategory> Rewards => ((IEnumerable<SanctumDeferredRewardCategory>) new SanctumDeferredRewardCategory[3]
    {
      this.Reward1,
      this.Reward2,
      this.Reward3
    }).Where<SanctumDeferredRewardCategory>((Func<SanctumDeferredRewardCategory, bool>) (x => x != null && x.Address != 0L)).ToList<SanctumDeferredRewardCategory>();
  }
}
