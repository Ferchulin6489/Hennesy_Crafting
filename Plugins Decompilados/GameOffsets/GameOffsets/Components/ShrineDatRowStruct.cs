// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.ShrineDatRowStruct
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ShrineDatRowStruct
  {
    [FieldOffset(0)]
    public long IdString;
    [FieldOffset(8)]
    public int TimeoutInSeconds;
    [FieldOffset(12)]
    public long DisplayName;
    [FieldOffset(20)]
    public bool IsRegain;
    [FieldOffset(29)]
    public bool PlayerShrineBuffDatRowPtr;
    [FieldOffset(37)]
    public int Unknown0;
    [FieldOffset(41)]
    public int Unknown1;
    [FieldOffset(45)]
    public long Description;
    [FieldOffset(53)]
    public long MonsterShrineBuffDatRowPtr;
  }
}
