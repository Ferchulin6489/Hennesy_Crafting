// Decompiled with JetBrains decompiler
// Type: GameOffsets.ChestComponentOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ChestComponentOffsets
  {
    [FieldOffset(352)]
    public long StrongboxData;
    [FieldOffset(360)]
    public bool IsOpened;
    [FieldOffset(361)]
    public bool IsLocked;
    [FieldOffset(364)]
    public readonly byte quality;
    [FieldOffset(424)]
    public bool IsStrongbox;
  }
}
