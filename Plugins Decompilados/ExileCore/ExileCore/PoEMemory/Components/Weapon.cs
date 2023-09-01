// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.Weapon
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

namespace ExileCore.PoEMemory.Components
{
  public class Weapon : Component
  {
    public int WeaponType => this.Address == 0L ? 0 : this.M.Read<int>(this.Address + 32L, new int[1]);

    public int DamageMin
    {
      get
      {
        if (this.Address == 0L)
          return 0;
        return this.M.Read<int>(this.Address + 32L, 4);
      }
    }

    public int DamageMax
    {
      get
      {
        if (this.Address == 0L)
          return 0;
        return this.M.Read<int>(this.Address + 32L, 8);
      }
    }

    public int AttackTime
    {
      get
      {
        if (this.Address == 0L)
          return 1;
        return this.M.Read<int>(this.Address + 32L, 12);
      }
    }

    public int CritChance
    {
      get
      {
        if (this.Address == 0L)
          return 0;
        return this.M.Read<int>(this.Address + 32L, 16);
      }
    }

    public int WeaponRange
    {
      get
      {
        if (this.Address == 0L)
          return 0;
        return this.M.Read<int>(this.Address + 32L, 24);
      }
    }
  }
}
