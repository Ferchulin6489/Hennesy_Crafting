﻿// Decompiled with JetBrains decompiler
// Type: GameOffsets.ModsComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ModsComponentOffsets
  {
    public static readonly int HumanStats = 32;
    public static readonly int ItemModRecordSize = 40;
    public static readonly int NameRecordSize = 16;
    public static readonly int NameOffset = 4;
    public static readonly int StatRecordSize = 32;
    [FieldOffset(56)]
    public NativePtrArray UniqueName;
    [FieldOffset(176)]
    public bool Identified;
    [FieldOffset(178)]
    public byte FracturedModsCount;
    [FieldOffset(180)]
    public int ItemRarity;
    [FieldOffset(192)]
    public NativePtrArray implicitMods;
    [FieldOffset(216)]
    public NativePtrArray explicitMods;
    [FieldOffset(240)]
    public NativePtrArray enchantMods;
    [FieldOffset(264)]
    public NativePtrArray ScourgeModsArray;
    [FieldOffset(288)]
    public NativePtrArray crucibleMods;
    [FieldOffset(528)]
    public long ModsComponentStatsPtr;
    [FieldOffset(608)]
    public NativePtrArray GetSynthesizedStats;
    [FieldOffset(576)]
    public int ItemLevel;
    [FieldOffset(580)]
    public int RequiredLevel;
    [FieldOffset(592)]
    public long IncubatorPtr;
    [FieldOffset(600)]
    public ushort IncubatorKills;
    [FieldOffset(605)]
    public byte IsMirrored;
    [FieldOffset(606)]
    public byte IsSplit;
    [FieldOffset(607)]
    public byte IsUsable;
  }
}
