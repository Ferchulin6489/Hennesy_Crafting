// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.NativeListNodeComponent
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct NativeListNodeComponent
  {
    [FieldOffset(0)]
    public long Next;
    [FieldOffset(8)]
    public long Prev;
    [FieldOffset(16)]
    public long String;
    [FieldOffset(24)]
    public int ComponentList;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 4);
      interpolatedStringHandler.AppendLiteral("Next: ");
      interpolatedStringHandler.AppendFormatted<long>(this.Next);
      interpolatedStringHandler.AppendLiteral(" Prev: ");
      interpolatedStringHandler.AppendFormatted<long>(this.Prev);
      interpolatedStringHandler.AppendLiteral(" String: ");
      interpolatedStringHandler.AppendFormatted<long>(this.String);
      interpolatedStringHandler.AppendLiteral(" ComponentList: ");
      interpolatedStringHandler.AppendFormatted<int>(this.ComponentList);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
