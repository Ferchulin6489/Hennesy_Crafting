// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.MonsterVarietiesStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct MonsterVarietiesStruct
  {
    [FieldOffset(0)]
    public long IdStringPtr;
    [FieldOffset(16)]
    public long MonsterTypePtr;
    [FieldOffset(32)]
    public MinMaxStruct AttackDistance;
    [FieldOffset(64)]
    public long TotalMods;
    [FieldOffset(72)]
    public long ModsPtr;
    [FieldOffset(244)]
    public long MonsterName;
  }
}
