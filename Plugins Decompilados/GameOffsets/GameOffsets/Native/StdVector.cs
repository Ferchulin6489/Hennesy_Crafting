// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.StdVector
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct StdVector
  {
    public long First;
    public long Last;
    public long End;

    public long TotalElements(int elementSize) => (this.Last - this.First) / (long) elementSize;

    public long ElementCount<T>() where T : unmanaged => this.TotalElements(Unsafe.SizeOf<T>());

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
      interpolatedStringHandler.AppendLiteral("First: ");
      interpolatedStringHandler.AppendFormatted<long>(this.First, "X");
      interpolatedStringHandler.AppendLiteral(" - ");
      interpolatedStringHandler.AppendLiteral("Last: ");
      interpolatedStringHandler.AppendFormatted<long>(this.Last, "X");
      interpolatedStringHandler.AppendLiteral(" - ");
      interpolatedStringHandler.AppendLiteral("Size (bytes): ");
      interpolatedStringHandler.AppendFormatted<long>(this.TotalElements(1));
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
