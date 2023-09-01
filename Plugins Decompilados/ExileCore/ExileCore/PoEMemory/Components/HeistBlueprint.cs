// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HeistBlueprint
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.Shared.Cache;
using GameOffsets;
using GameOffsets.Native;
using System;
using System.Collections.Generic;

namespace ExileCore.PoEMemory.Components
{
  public class HeistBlueprint : Component
  {
    private readonly CachedValue<HeistBlueprintComponentOffsets> _CachedBlueprint;

    public HeistBlueprint() => this._CachedBlueprint = (CachedValue<HeistBlueprintComponentOffsets>) new FrameCache<HeistBlueprintComponentOffsets>((Func<HeistBlueprintComponentOffsets>) (() => this.M.Read<HeistBlueprintComponentOffsets>(this.Address)));

    public HeistBlueprintComponentOffsets BlueprintStruct => this._CachedBlueprint.Value;

    public byte AreaLevel => this.BlueprintStruct.AreaLevel;

    public bool IsConfirmed => this.BlueprintStruct.IsConfirmed == (byte) 1;

    public List<HeistBlueprint.Wing> Wings => this.GetWings(this.BlueprintStruct.Wings);

    private List<HeistBlueprint.Wing> GetWings(NativePtrArray source)
    {
      List<HeistBlueprint.Wing> wings = new List<HeistBlueprint.Wing>();
      int wingRecordSize = HeistBlueprintComponentOffsets.WingRecordSize;
      int num = 0;
      for (long first = source.First; first < source.Last && num < 50; ++num)
      {
        HeistBlueprint.Wing wing = this.GetObject<HeistBlueprint.Wing>(first);
        if (wing != null)
          wings.Add(wing);
        first += (long) wingRecordSize;
      }
      return wings;
    }

    public class Wing : RemoteMemoryObject
    {
      public List<(HeistJobRecord, int)> Jobs => this.GetJobs(this.Address);

      public List<HeistChestRewardTypeRecord> RewardRooms => this.GetRooms(this.Address + 32L);

      public List<HeistNpcRecord> Crew => this.GetCrew(this.Address + 56L);

      private List<(HeistJobRecord, int)> GetJobs(long source)
      {
        List<(HeistJobRecord, int)> jobs = new List<(HeistJobRecord, int)>();
        long num1 = this.M.Read<long>(source);
        long num2 = this.M.Read<long>(source + 8L);
        int jobRecordSize = HeistBlueprintComponentOffsets.JobRecordSize;
        int num3 = 0;
        for (long addr = num1; addr < num2 && num3 < 50; ++num3)
        {
          long address = this.M.Read<long>(addr);
          if (address != 0L)
          {
            byte num4 = this.M.Read<byte>(addr + 16L);
            HeistJobRecord byAddress = this.TheGame.Files.HeistJobs.GetByAddress(address);
            if (byAddress != null)
              jobs.Add((byAddress, (int) num4));
          }
          addr += (long) jobRecordSize;
        }
        return jobs;
      }

      private List<HeistChestRewardTypeRecord> GetRooms(long source)
      {
        List<HeistChestRewardTypeRecord> rooms = new List<HeistChestRewardTypeRecord>();
        long num1 = this.M.Read<long>(source);
        long num2 = this.M.Read<long>(source + 8L);
        int roomRecordSize = HeistBlueprintComponentOffsets.RoomRecordSize;
        int num3 = 0;
        for (long addr = num1; addr < num2 && num3 < 50; ++num3)
        {
          long address = this.M.Read<long>(addr);
          if (address != 0L)
          {
            HeistChestRewardTypeRecord byAddress = this.TheGame.Files.HeistChestRewardType.GetByAddress(address);
            if (byAddress != null)
              rooms.Add(byAddress);
          }
          addr += (long) roomRecordSize;
        }
        return rooms;
      }

      private List<HeistNpcRecord> GetCrew(long source)
      {
        List<HeistNpcRecord> crew = new List<HeistNpcRecord>();
        long num1 = this.M.Read<long>(source);
        long num2 = this.M.Read<long>(source + 8L);
        int npcRecordSize = HeistBlueprintComponentOffsets.NpcRecordSize;
        int num3 = 0;
        for (long index = num1; index < num2 && num3 < 50; ++num3)
        {
          long address = this.M.Read<long>(index + 8L);
          if (address != 0L)
          {
            HeistNpcRecord byAddress = this.TheGame.Files.HeistNpcs.GetByAddress(address);
            if (byAddress != null)
              crew.Add(byAddress);
          }
          index += (long) npcRecordSize;
        }
        return crew;
      }
    }
  }
}
