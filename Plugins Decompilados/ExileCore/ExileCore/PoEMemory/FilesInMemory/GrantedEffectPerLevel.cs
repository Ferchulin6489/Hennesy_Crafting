// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.GrantedEffectPerLevel
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class GrantedEffectPerLevel : RemoteMemoryObject
  {
    private GrantedEffect _grantedEffect;
    private int? _level;
    private int? _requiredLevel;
    private int? _costMultiplier;
    private int? _cooldown;

    public GrantedEffect GrantedEffect => this._grantedEffect ?? (this._grantedEffect = this.TheGame.Files.GrantedEffects.GetByAddress(this.M.Read<long>(this.Address)));

    public int Level
    {
      get
      {
        int valueOrDefault = this._level.GetValueOrDefault();
        if (this._level.HasValue)
          return valueOrDefault;
        int level = this.M.Read<int>(this.Address + 16L);
        this._level = new int?(level);
        return level;
      }
    }

    public int RequiredLevel
    {
      get
      {
        int valueOrDefault = this._requiredLevel.GetValueOrDefault();
        if (this._requiredLevel.HasValue)
          return valueOrDefault;
        int requiredLevel = this.M.Read<int>(this.Address + 20L);
        this._requiredLevel = new int?(requiredLevel);
        return requiredLevel;
      }
    }

    public int CostMultiplier
    {
      get
      {
        int valueOrDefault = this._costMultiplier.GetValueOrDefault();
        if (this._costMultiplier.HasValue)
          return valueOrDefault;
        int costMultiplier = this.M.Read<int>(this.Address + 24L);
        this._costMultiplier = new int?(costMultiplier);
        return costMultiplier;
      }
    }

    public int Cooldown
    {
      get
      {
        int valueOrDefault = this._cooldown.GetValueOrDefault();
        if (this._cooldown.HasValue)
          return valueOrDefault;
        int cooldown = this.M.Read<int>(this.Address + 32L);
        this._cooldown = new int?(cooldown);
        return cooldown;
      }
    }
  }
}
