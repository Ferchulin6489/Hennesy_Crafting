// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.NativeUtf16Text
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct NativeUtf16Text
  {
    public long Buffer;
    public long Reserved8Bytes;
    public long Length;
    public long LengthWithNullTerminator;

    public long ByteLength => this.Length * 2L;

    public string CacheString
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
        interpolatedStringHandler.AppendFormatted<long>(this.Buffer);
        interpolatedStringHandler.AppendLiteral("_");
        interpolatedStringHandler.AppendFormatted<long>(this.Reserved8Bytes);
        interpolatedStringHandler.AppendLiteral("_");
        interpolatedStringHandler.AppendFormatted<long>(this.Length);
        return interpolatedStringHandler.ToStringAndClear();
      }
    }
  }
}
