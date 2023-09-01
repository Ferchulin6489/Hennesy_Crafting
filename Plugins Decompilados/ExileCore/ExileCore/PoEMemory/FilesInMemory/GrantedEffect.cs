// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.GrantedEffect
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class GrantedEffect : RemoteMemoryObject
  {
    private string _name;
    private bool? _isSupport;
    private string _supportGemLetter;
    private bool? _supportsGemsOnly;
    private bool? _cannotBeSupported;
    private int? _castTime;
    private List<GrantedEffectPerLevel> _perLevelEffects;

    public string Name => this._name ?? (this._name = this.M.ReadStringU(this.M.Read<long>(this.Address)));

    public bool IsSupport
    {
      get
      {
        bool valueOrDefault = this._isSupport.GetValueOrDefault();
        if (this._isSupport.HasValue)
          return valueOrDefault;
        bool isSupport = this.M.Read<bool>(this.Address + 8L);
        this._isSupport = new bool?(isSupport);
        return isSupport;
      }
    }

    public string SupportGemLetter => this._supportGemLetter ?? (this._supportGemLetter = this.M.ReadStringU(this.M.Read<long>(this.Address + 25L)));

    public bool SupportsGemsOnly
    {
      get
      {
        bool valueOrDefault = this._supportsGemsOnly.GetValueOrDefault();
        if (this._supportsGemsOnly.HasValue)
          return valueOrDefault;
        bool supportsGemsOnly = this.M.Read<bool>(this.Address + 69L);
        this._supportsGemsOnly = new bool?(supportsGemsOnly);
        return supportsGemsOnly;
      }
    }

    public bool CannotBeSupported
    {
      get
      {
        bool valueOrDefault = this._cannotBeSupported.GetValueOrDefault();
        if (this._cannotBeSupported.HasValue)
          return valueOrDefault;
        bool cannotBeSupported = this.M.Read<bool>(this.Address + 90L);
        this._cannotBeSupported = new bool?(cannotBeSupported);
        return cannotBeSupported;
      }
    }

    public int CastTime
    {
      get
      {
        int valueOrDefault = this._castTime.GetValueOrDefault();
        if (this._castTime.HasValue)
          return valueOrDefault;
        int castTime = this.M.Read<int>(this.Address + 95L);
        this._castTime = new int?(castTime);
        return castTime;
      }
    }

    public List<GrantedEffectPerLevel> PerLevelEffects => this._perLevelEffects ?? (this._perLevelEffects = this.TheGame.Files.GrantedEffectsPerLevel.EntriesList.Where<GrantedEffectPerLevel>((Func<GrantedEffectPerLevel, bool>) (x => this.Equals((object) x.GrantedEffect))).ToList<GrantedEffectPerLevel>());
  }
}
