// Decompiled with JetBrains decompiler
// Type: GameOffsets.BaseComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct BaseComponentOffsets
  {
    [FieldOffset(16)]
    public long ItemInfo;
    [FieldOffset(198)]
    public byte Influence;
    [FieldOffset(199)]
    public byte Corrupted;
    [FieldOffset(96)]
    public NativeStringU PublicPrice;
    [FieldOffset(200)]
    public int UnspentAbsorbedCorruption;
    [FieldOffset(204)]
    public int ScourgedTier;
  }
}
