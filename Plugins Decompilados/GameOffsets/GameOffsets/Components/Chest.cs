﻿// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.Chest
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct Chest
  {
    [FieldOffset(0)]
    public ComponentHeader Header;
    [FieldOffset(88)]
    public long ChestInternalPtr;
    [FieldOffset(376)]
    public bool IsOpened;
    [FieldOffset(377)]
    public bool IsLocked;
    [FieldOffset(380)]
    public byte Quality;
    [FieldOffset(176)]
    public byte Rarity;
  }
}
