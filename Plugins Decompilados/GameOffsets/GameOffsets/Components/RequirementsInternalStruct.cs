﻿// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.RequirementsInternalStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct RequirementsInternalStruct
  {
    [FieldOffset(16)]
    public int strength;
    [FieldOffset(20)]
    public int dexterity;
    [FieldOffset(24)]
    public int intelligence;
  }
}
