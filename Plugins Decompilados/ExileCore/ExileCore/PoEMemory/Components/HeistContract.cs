// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HeistContract
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects.Heist;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Cache;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class HeistContract : Component
  {
    private readonly CachedValue<HeistContractComponentOffsets> _ContractData;
    private readonly CachedValue<HeistContractObjectiveOffsets> _ObjectivesData;
    private readonly CachedValue<HeistContractRequirementOffsets> _RequirementData;

    public HeistContract()
    {
      this._ContractData = (CachedValue<HeistContractComponentOffsets>) new FrameCache<HeistContractComponentOffsets>((Func<HeistContractComponentOffsets>) (() => this.M.Read<HeistContractComponentOffsets>(this.Address)));
      this._ObjectivesData = (CachedValue<HeistContractObjectiveOffsets>) new FrameCache<HeistContractObjectiveOffsets>((Func<HeistContractObjectiveOffsets>) (() => this.M.Read<HeistContractObjectiveOffsets>(this._ContractData.Value.ObjectiveKey)));
      this._RequirementData = (CachedValue<HeistContractRequirementOffsets>) new FrameCache<HeistContractRequirementOffsets>((Func<HeistContractRequirementOffsets>) (() => this.M.Read<HeistContractRequirementOffsets>(this._ContractData.Value.Requirements.First)));
    }

    private HeistContractObjectiveOffsets Objectives => this._ObjectivesData.Value;

    private HeistContractRequirementOffsets Requirements => this._RequirementData.Value;

    public BaseItemType TargetItem => this.TheGame.Files.BaseItemTypes.GetFromAddress(this.Objectives.TargetKey);

    public string Client => this.M.ReadStringU(this.Objectives.ClientKey);

    public HeistJobRecord RequiredJob => this.TheGame.Files.HeistJobs.GetByAddress(this.Requirements.JobKey);

    public byte RequiredJobLevel => this.Requirements.JobLevel;
  }
}
