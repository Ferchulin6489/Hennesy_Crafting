// Decompiled with JetBrains decompiler
// Type: GameOffsets.Components.WorldItem
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets.Components
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct WorldItem
  {
    [FieldOffset(0)]
    public ComponentHeader Header;
    [FieldOffset(32)]
    public long ItemPtr;
    [FieldOffset(48)]
    public int LootAllocationId;
    [FieldOffset(52)]
    public uint LootAllocationTime;
    [FieldOffset(56)]
    public int DroppedTime;
    [FieldOffset(56)]
    public float Unknown0;
    [FieldOffset(64)]
    public byte Unknown1;
    [FieldOffset(65)]
    public byte Unknown2;
  }
}
