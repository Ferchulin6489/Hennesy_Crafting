// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.Enums.Influence
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;

namespace ExileCore.Shared.Enums
{
  [Flags]
  public enum Influence : byte
  {
    None = 0,
    Shaper = 1,
    Elder = 2,
    Crusader = 4,
    Redeemer = 8,
    Hunter = 16, // 0x10
    Warlord = 32, // 0x20
  }
}
