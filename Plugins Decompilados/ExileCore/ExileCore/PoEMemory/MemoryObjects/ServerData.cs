// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.ServerData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.FilesInMemory.Atlas;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class ServerData : RemoteMemoryObject
  {
    private static readonly int PlayerStashTabsOffset = Extensions.GetOffset<ServerDataOffsets>((Expression<Func<ServerDataOffsets, object>>) (x => (object) x.PlayerStashTabs)) + 32768;
    private static readonly int GuildStashTabsOffset = Extensions.GetOffset<ServerDataOffsets>((Expression<Func<ServerDataOffsets, object>>) (x => (object) x.GuildStashTabs)) + 32768;
    private readonly CachedValue<ServerDataOffsets> _cachedValue;
    private readonly CachedValue<ServerPlayerData> _PlayerData;
    private readonly List<Player> _nearestPlayers = new List<Player>();

    public ServerData()
    {
      this._cachedValue = (CachedValue<ServerDataOffsets>) new FrameCache<ServerDataOffsets>((Func<ServerDataOffsets>) (() => this.M.Read<ServerDataOffsets>(this.Address + 32768L)));
      this._PlayerData = (CachedValue<ServerPlayerData>) new FrameCache<ServerPlayerData>((Func<ServerPlayerData>) (() => this.GetObject<ServerPlayerData>(this._cachedValue.Value.PlayerRelatedData)));
    }

    public ServerPlayerData PlayerInformation => this._PlayerData.Value;

    public ServerDataOffsets ServerDataStruct => this._cachedValue.Value;

    public ushort TradeChatChannel => this.ServerDataStruct.TradeChatChannel;

    public ushort GlobalChatChannel => this.ServerDataStruct.GlobalChatChannel;

    public byte MonsterLevel => this.ServerDataStruct.MonsterLevel;

    public int DialogDepth => this.ServerDataStruct.DialogDepth;

    public byte MonstersRemaining => this.ServerDataStruct.MonstersRemaining;

    public ushort CurrentSulphiteAmount => this._cachedValue.Value.CurrentSulphiteAmount;

    public int CurrentAzuriteAmount => this._cachedValue.Value.CurrentAzuriteAmount;

    [Obsolete]
    public SharpDX.Vector2 WorldMousePosition => this._cachedValue.Value.WorldMousePosition.ToSharpDx();

    public System.Numerics.Vector2 WorldMousePositionNum => this._cachedValue.Value.WorldMousePosition;

    public IList<Player> NearestPlayers
    {
      get
      {
        if (this.Address == 0L)
          return (IList<Player>) new List<Player>();
        long first = this.ServerDataStruct.NearestPlayers.First;
        long last = this.ServerDataStruct.NearestPlayers.Last;
        long num = first + 16L;
        if (num < this.Address || (last - num) / 16L > 50L)
          return (IList<Player>) this._nearestPlayers;
        this._nearestPlayers.Clear();
        for (long addressPointer = num; addressPointer < last; addressPointer += 16L)
          this._nearestPlayers.Add(this.ReadObject<Player>(addressPointer));
        return (IList<Player>) this._nearestPlayers;
      }
    }

    public int GetBeastCapturedAmount(BestiaryCapturableMonster monster) => this.M.Read<int>(this.Address + 21056L + (long) (monster.Id * 4));

    public ushort LastActionId => this.ServerDataStruct.LastActionId;

    public int CharacterLevel => this.PlayerInformation.Level;

    public int PassiveRefundPointsLeft => this.PlayerInformation.PassiveRefundPointsLeft;

    public int FreePassiveSkillPointsLeft => this.PlayerInformation.FreePassiveSkillPointsLeft;

    public int QuestPassiveSkillPoints => this.PlayerInformation.QuestPassiveSkillPoints;

    public int TotalAscendencyPoints => this.PlayerInformation.TotalAscendencyPoints;

    public int SpentAscendencyPoints => this.PlayerInformation.SpentAscendencyPoints;

    public PartyAllocation PartyAllocationType => (PartyAllocation) this.ServerDataStruct.PartyAllocationType;

    public string League => this.ServerDataStruct.League.ToString(this.M);

    public PartyStatus PartyStatusType => (PartyStatus) this.ServerDataStruct.PartyStatusType;

    public bool IsInGame => this.NetworkState == NetworkStateE.Connected;

    public NetworkStateE NetworkState => (NetworkStateE) this.ServerDataStruct.NetworkState;

    public int Latency => this.ServerDataStruct.Latency;

    public string Guild => NativeStringReader.ReadString(this.ServerDataStruct.GuildName, this.M);

    public BetrayalData BetrayalData => this.GetObject<BetrayalData>(this.M.Read<long>(this.Address + 208L, 1968));

    public string GuildTag => NativeStringReader.ReadString(this.ServerDataStruct.GuildName + 32L, this.M);

    public BitArray WaypointsUnlockState => new BitArray(this.M.ReadBytes(this.Address + 43265L, 24));

    public IList<ushort> SkillBarIds
    {
      get
      {
        if (this.Address == 0L)
          return (IList<ushort>) new List<ushort>();
        SkillBarIdsStruct skillBarIds = this._cachedValue.Value.SkillBarIds;
        return (IList<ushort>) new List<ushort>()
        {
          skillBarIds.SkillBar1,
          skillBarIds.SkillBar2,
          skillBarIds.SkillBar3,
          skillBarIds.SkillBar4,
          skillBarIds.SkillBar5,
          skillBarIds.SkillBar6,
          skillBarIds.SkillBar7,
          skillBarIds.SkillBar8,
          skillBarIds.SkillBar9,
          skillBarIds.SkillBar10,
          skillBarIds.SkillBar11,
          skillBarIds.SkillBar12,
          skillBarIds.SkillBar13
        };
      }
    }

    public IList<ushort> PassiveSkillIds
    {
      get
      {
        if (this.Address == 0L)
          return (IList<ushort>) null;
        long num = this.PlayerInformation.AllocatedPassivesIds.ElementCount<ushort>();
        return num < 0L || num > 500L ? (IList<ushort>) new List<ushort>() : (IList<ushort>) this.M.ReadStdVector<ushort>(this.PlayerInformation.AllocatedPassivesIds);
      }
    }

    public IList<ushort> AtlasPassiveSkillIds
    {
      get
      {
        if (this.Address == 0L)
          return (IList<ushort>) null;
        NativePtrArray nativeContainer = this.M.Read<NativePtrArray>(this.ServerDataStruct.AtlasTreeContainerPtr + 352L);
        long num = nativeContainer.ElementCount<ushort>();
        return num < 0L || num > 500L ? (IList<ushort>) new List<ushort>() : (IList<ushort>) this.M.ReadStdVector<ushort>(nativeContainer);
      }
    }

    public IList<ServerStashTab> PlayerStashTabs => this.GetStashTabs(ServerData.PlayerStashTabsOffset, ServerData.PlayerStashTabsOffset + 8);

    public IList<ServerStashTab> GuildStashTabs => this.GetStashTabs(ServerData.GuildStashTabsOffset, ServerData.GuildStashTabsOffset + 8);

    private IList<ServerStashTab> GetStashTabs(int offsetBegin, int offsetEnd)
    {
      long startAddress = this.M.Read<long>(this.Address + (long) offsetBegin);
      long endAddress = this.M.Read<long>(this.Address + (long) offsetEnd);
      if (startAddress <= 0L || endAddress <= 0L)
        return (IList<ServerStashTab>) null;
      long num = endAddress - startAddress;
      return num <= 0L || num > (long) ushort.MaxValue || startAddress <= 0L || endAddress <= 0L ? (IList<ServerStashTab>) new List<ServerStashTab>() : (IList<ServerStashTab>) new List<ServerStashTab>((IEnumerable<ServerStashTab>) this.M.ReadStructsArray<ServerStashTab>(startAddress, endAddress, 104, (RemoteMemoryObject) this.TheGame));
    }

    private IList<InventoryHolder> ReadInventoryHolders(NativePtrArray array)
    {
      long first = array.First;
      long last = array.Last;
      return first == 0L || last <= first || (last - first) / 32L > 1024L ? (IList<InventoryHolder>) new List<InventoryHolder>() : (IList<InventoryHolder>) this.M.ReadStructsArray<InventoryHolder>(first, last, 32, (RemoteMemoryObject) this).ToList<InventoryHolder>();
    }

    public IList<InventoryHolder> PlayerInventories => this.ReadInventoryHolders(this.ServerDataStruct.PlayerInventories);

    public IList<InventoryHolder> NPCInventories => this.ReadInventoryHolders(this.ServerDataStruct.NPCInventories);

    public IList<InventoryHolder> GuildInventories => this.ReadInventoryHolders(this.ServerDataStruct.GuildInventories);

    public ServerInventory GetPlayerInventoryBySlot(InventorySlotE slot)
    {
      foreach (InventoryHolder playerInventory in (IEnumerable<InventoryHolder>) this.PlayerInventories)
      {
        if (playerInventory.Inventory.InventSlot == slot)
          return playerInventory.Inventory;
      }
      return (ServerInventory) null;
    }

    public ServerInventory GetPlayerInventoryByType(InventoryTypeE type)
    {
      foreach (InventoryHolder playerInventory in (IEnumerable<InventoryHolder>) this.PlayerInventories)
      {
        if (playerInventory.Inventory.InventType == type)
          return playerInventory.Inventory;
      }
      return (ServerInventory) null;
    }

    public ServerInventory GetPlayerInventoryBySlotAndType(InventoryTypeE type, InventorySlotE slot)
    {
      foreach (InventoryHolder playerInventory in (IEnumerable<InventoryHolder>) this.PlayerInventories)
      {
        if (playerInventory.Inventory.InventType == type && playerInventory.Inventory.InventSlot == slot)
          return playerInventory.Inventory;
      }
      return (ServerInventory) null;
    }

    public IList<WorldArea> CompletedAreas => this.GetAreas(this.ServerDataStruct.CompletedMaps);

    public IList<WorldArea> BonusCompletedAreas => this.GetAreas(this.ServerDataStruct.BonusCompletedAreas);

    [Obsolete]
    public IList<WorldArea> ShapedMaps => (IList<WorldArea>) new List<WorldArea>();

    [Obsolete]
    public IList<WorldArea> ElderGuardiansAreas => (IList<WorldArea>) new List<WorldArea>();

    [Obsolete]
    public IList<WorldArea> MasterAreas => (IList<WorldArea>) new List<WorldArea>();

    [Obsolete]
    public IList<WorldArea> ShaperElderAreas => (IList<WorldArea>) new List<WorldArea>();

    public IList<WorldArea> GetAreas(long address)
    {
      if (this.Address == 0L || address == 0L)
        return (IList<WorldArea>) new List<WorldArea>();
      List<WorldArea> areas = new List<WorldArea>();
      long num1 = this.M.Read<long>(address);
      int num2 = 0;
      if (num1 == 0L)
        return (IList<WorldArea>) areas;
      long addr = num1;
      while (addr != 0L)
      {
        long address1 = this.M.Read<long>(addr + 16L);
        if (address1 != 0L)
        {
          WorldArea byAddress = this.TheGame.Files.WorldAreas.GetByAddress(address1);
          if (byAddress != null)
            areas.Add(byAddress);
          ++num2;
          if (num2 > 1024)
          {
            areas = new List<WorldArea>();
          }
          else
          {
            addr = this.M.Read<long>(addr);
            if (addr != num1)
              continue;
          }
        }
        return (IList<WorldArea>) areas;
      }
      return (IList<WorldArea>) areas;
    }

    public List<byte> RegionIds_Debug
    {
      get
      {
        List<byte> regionIdsDebug = new List<byte>();
        for (int regionId = 0; regionId < 8; ++regionId)
          regionIdsDebug.Add(this.GetAtlasRegionUpgradesByRegion(regionId));
        return regionIdsDebug;
      }
    }

    public byte GetAtlasRegionUpgradesByRegion(int regionId) => this.M.Read<byte>(this.Address + 34930L + (long) regionId);

    public byte GetAtlasRegionUpgradesByRegion(AtlasRegion region) => this.M.Read<byte>(this.Address + 34930L + (long) region.Index);

    public ushort LesserBrokenCircleArtifacts => this.ServerDataStruct.Artifacts.LesserBrokenCircleArtifacts;

    public ushort GreaterBrokenCircleArtifacts => this.ServerDataStruct.Artifacts.GreaterBrokenCircleArtifacts;

    public ushort GrandBrokenCircleArtifacts => this.ServerDataStruct.Artifacts.GrandBrokenCircleArtifacts;

    public ushort ExceptionalBrokenCircleArtifacts => this.ServerDataStruct.Artifacts.ExceptionalBrokenCircleArtifacts;

    public ushort LesserBlackScytheArtifacts => this.ServerDataStruct.Artifacts.LesserBlackScytheArtifacts;

    public ushort GreaterBlackScytheArtifacts => this.ServerDataStruct.Artifacts.GreaterBlackScytheArtifacts;

    public ushort GrandBlackScytheArtifacts => this.ServerDataStruct.Artifacts.GrandBlackScytheArtifacts;

    public ushort ExceptionalBlackScytheArtifacts => this.ServerDataStruct.Artifacts.ExceptionalBlackScytheArtifacts;

    public ushort LesserOrderArtifacts => this.ServerDataStruct.Artifacts.LesserOrderArtifacts;

    public ushort GreaterOrderArtifacts => this.ServerDataStruct.Artifacts.GreaterOrderArtifacts;

    public ushort GrandOrderArtifacts => this.ServerDataStruct.Artifacts.GrandOrderArtifacts;

    public ushort ExceptionalOrderArtifacts => this.ServerDataStruct.Artifacts.ExceptionalOrderArtifacts;

    public ushort LesserSunArtifacts => this.ServerDataStruct.Artifacts.LesserSunArtifacts;

    public ushort GreaterSunArtifacts => this.ServerDataStruct.Artifacts.GreaterSunArtifacts;

    public ushort GrandSunArtifacts => this.ServerDataStruct.Artifacts.GrandSunArtifacts;

    public ushort ExceptionalSunArtifacts => this.ServerDataStruct.Artifacts.ExceptionalSunArtifacts;
  }
}
