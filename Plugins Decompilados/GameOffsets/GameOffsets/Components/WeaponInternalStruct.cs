// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.WeaponInternalStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct WeaponInternalStruct
  {
    [FieldOffset(8)]
    public long Unknown0Ptr;
    [FieldOffset(16)]
    public int Unknown1;
    [FieldOffset(20)]
    public int minimum_damage;
    [FieldOffset(24)]
    public int maximum_damage;
    [FieldOffset(28)]
    public int weapon_speed;
    [FieldOffset(32)]
    public int critical_chance;
    [FieldOffset(36)]
    public int minimum_attack_distance;
    [FieldOffset(40)]
    public int maximum_attack_distance;
  }
}
