// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Base
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Cache;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using GameOffsets;
using System;

namespace ExileCore.PoEMemory.Components
{
  public class Base : Component
  {
    private readonly CachedValue<BaseComponentOffsets> _cachedValue;
    private readonly CachedValue<ItemInfoData> _ItemInfoData;
    private string _name;

    public Base()
    {
      this._cachedValue = (CachedValue<BaseComponentOffsets>) new FrameCache<BaseComponentOffsets>((Func<BaseComponentOffsets>) (() => this.M.Read<BaseComponentOffsets>(this.Address)));
      this._ItemInfoData = (CachedValue<ItemInfoData>) new FrameCache<ItemInfoData>((Func<ItemInfoData>) (() => this.GetObject<ItemInfoData>(this._cachedValue.Value.ItemInfo)));
    }

    public BaseComponentOffsets BaseStruct => this._cachedValue.Value;

    public string Name => this._name ?? (this._name = this._ItemInfoData.Value.Name);

    public byte ItemCellsSizeX => this._ItemInfoData.Value.ItemCellsSizeX;

    public byte ItemCellsSizeY => this._ItemInfoData.Value.ItemCellsSizeY;

    public Influence InfluenceFlag => (Influence) this._cachedValue.Value.Influence;

    public bool isShaper => (this.InfluenceFlag & Influence.Shaper) == Influence.Shaper;

    public bool isElder => (this.InfluenceFlag & Influence.Elder) == Influence.Elder;

    public bool isCrusader => (this.InfluenceFlag & Influence.Crusader) == Influence.Crusader;

    public bool isHunter => (this.InfluenceFlag & Influence.Hunter) == Influence.Hunter;

    public bool isRedeemer => (this.InfluenceFlag & Influence.Redeemer) == Influence.Redeemer;

    public bool isWarlord => (this.InfluenceFlag & Influence.Warlord) == Influence.Warlord;

    public bool isSynthesized => this.M.Read<byte>(this.Address + 222L) == (byte) 1;

    public bool isCorrupted => ((int) this._cachedValue.Value.Corrupted & 1) == 1;

    public int UnspentAbsorbedCorruption => this._cachedValue.Value.UnspentAbsorbedCorruption;

    public int ScourgedTier => this._cachedValue.Value.ScourgedTier;

    public string PublicPrice => this._cachedValue.Value.PublicPrice.ToString(this.M);
  }
}
