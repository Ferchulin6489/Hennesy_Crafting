// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.MonsterTypesStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct MonsterTypesStruct
  {
    [FieldOffset(0)]
    public long IdStringPtr;
    [FieldOffset(12)]
    public bool IsSummoned;
    [FieldOffset(13)]
    public int Armour;
    [FieldOffset(17)]
    public int Evasion;
    [FieldOffset(21)]
    public int EnergyShield;
    [FieldOffset(25)]
    public int MovementSpeed;
    [FieldOffset(29)]
    public long TotalTagsKeys;
    [FieldOffset(37)]
    public long TagsKeys;
    [FieldOffset(53)]
    public long MonsterResistancesPtr;
  }
}
