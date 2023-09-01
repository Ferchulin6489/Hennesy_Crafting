// Decompiled with JetBrains decompiler
// Type: GameOffsets.SubActorSkillOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct SubActorSkillOffsets
  {
    [FieldOffset(0)]
    public ushort Id;
    [FieldOffset(8)]
    public long EffectsPerLevelPtr;
    [FieldOffset(112)]
    public byte CanBeUsedWithWeapon;
    [FieldOffset(113)]
    public byte CannotBeUsed;
    [FieldOffset(116)]
    public int TotalUses;
    [FieldOffset(128)]
    public int Cooldown;
    [FieldOffset(140)]
    public int SoulsPerUse;
    [FieldOffset(144)]
    public int TotalVaalUses;
    [FieldOffset(160)]
    public long StatsPtr;
  }
}
