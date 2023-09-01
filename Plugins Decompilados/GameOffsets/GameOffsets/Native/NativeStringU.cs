// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.NativeStringU
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System;
using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [Obsolete("This will be removed (Reason: bad name)")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct NativeStringU
  {
    [FieldOffset(0)]
    public long buf;
    [FieldOffset(8)]
    public long buf2;
    [FieldOffset(16)]
    public uint Size;
    [FieldOffset(24)]
    public uint Capacity;

    public static implicit operator NativeUtf16Text(NativeStringU s) => new NativeUtf16Text()
    {
      Buffer = s.buf,
      Reserved8Bytes = s.buf2,
      Length = (long) s.Size,
      LengthWithNullTerminator = (long) s.Capacity
    };
  }
}
