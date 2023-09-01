// Decompiled with JetBrains decompiler
// Type: GameOffsets.DiagnosticElementOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct DiagnosticElementOffsets
  {
    [FieldOffset(0)]
    public long DiagnosticArray;
    [FieldOffset(16)]
    public int X;
    [FieldOffset(20)]
    public int Y;
    [FieldOffset(24)]
    public int Width;
    [FieldOffset(28)]
    public int Height;
  }
}
