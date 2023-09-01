// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HeistEquipment
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class HeistEquipment : Component
  {
    private readonly Lazy<HeistEquipmentOffsets> _HeistEquipmentItem;
    private readonly CachedValue<BaseItemType> _ItemBase;
    private readonly CachedValue<HeistJobRecord> _Job;

    public HeistEquipment()
    {
      Lazy<HeistEquipmentComponentOffsets> component = new Lazy<HeistEquipmentComponentOffsets>((Func<HeistEquipmentComponentOffsets>) (() => this.M.Read<HeistEquipmentComponentOffsets>(this.Address)));
      Lazy<HeistEquipmentComponentDataOffsets> componentData = new Lazy<HeistEquipmentComponentDataOffsets>((Func<HeistEquipmentComponentDataOffsets>) (() => this.M.Read<HeistEquipmentComponentDataOffsets>(component.Value.DataKey)));
      this._HeistEquipmentItem = new Lazy<HeistEquipmentOffsets>((Func<HeistEquipmentOffsets>) (() => this.M.Read<HeistEquipmentOffsets>(componentData.Value.HeistEquipmentKey)));
      this._ItemBase = (CachedValue<BaseItemType>) new StaticValueCache<BaseItemType>((Func<BaseItemType>) (() => this.TheGame.Files.BaseItemTypes.GetFromAddress(this._HeistEquipmentItem.Value.BaseItemKey)));
      this._Job = (CachedValue<HeistJobRecord>) new StaticValueCache<HeistJobRecord>((Func<HeistJobRecord>) (() => this.TheGame.Files.HeistJobs.GetByAddress(this._HeistEquipmentItem.Value.RequiredJobKey)));
    }

    public BaseItemType ItemBase => this._ItemBase.Value;

    public HeistJobRecord RequiredJob => this._Job.Value;

    public int JobMinimumLevel => this._HeistEquipmentItem.Value.RequiredJobMinimumLevel;

    public HeistJobE RequiredJobE => this._Job.Value != null ? (HeistJobE) this.TheGame.Files.HeistJobs.EntriesList.FindIndex((Predicate<HeistJobRecord>) (job => job.Address == this._HeistEquipmentItem.Value.RequiredJobKey)) : HeistJobE.Any;
  }
}
