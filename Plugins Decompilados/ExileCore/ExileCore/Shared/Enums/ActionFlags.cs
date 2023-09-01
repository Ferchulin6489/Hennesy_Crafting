// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.ActionFlags
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Enums
{
  [Flags]
  public enum ActionFlags
  {
    None = 0,
    UsingAbility = 2,
    AbilityCooldownActive = 16, // 0x00000010
    UsingAbilityAbilityCooldown = AbilityCooldownActive | UsingAbility, // 0x00000012
    Dead = 64, // 0x00000040
    Moving = 128, // 0x00000080
    WashedUpState = 256, // 0x00000100
    HasMines = 2048, // 0x00000800
  }
}
