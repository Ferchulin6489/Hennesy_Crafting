﻿// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Components.HeistRewardDisplay
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;

namespace ExileCore.PoEMemory.Components
{
  public class HeistRewardDisplay : Component
  {
    public Entity RewardItem => this.ReadObjectAt<Entity>(32);
  }
}
