// Decompiled with JetBrains decompiler
// Type: GameOffsets.CursorOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct CursorOffsets
  {
    public const int OffsetBuffers = 1772;
    [FieldOffset(0)]
    public int vTable;
    [FieldOffset(568)]
    public int Action;
    [FieldOffset(588)]
    public int Clicks;
    [FieldOffset(672)]
    public NativeStringU ActionString;
  }
}
