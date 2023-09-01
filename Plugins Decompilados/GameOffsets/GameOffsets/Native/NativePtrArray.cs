// Decompiled with JetBrains decompiler
// Type: GameOffsets.Native.NativePtrArray
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets.Native
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct NativePtrArray : IEquatable<NativePtrArray>
  {
    public readonly long First;
    public readonly long Last;
    public readonly long End;

    public long Size => this.Last - this.First;

    public int Capacity => (int) (this.End - this.First);

    public long ElementCount(int elementSize) => (this.Last - this.First) / (long) elementSize;

    public long ElementCount<T>() where T : unmanaged => this.ElementCount(Unsafe.SizeOf<T>());

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 4);
      interpolatedStringHandler.AppendLiteral("First: 0x");
      interpolatedStringHandler.AppendFormatted<long>(this.First);
      interpolatedStringHandler.AppendLiteral(", Last: 0x");
      interpolatedStringHandler.AppendFormatted<long>(this.Last);
      interpolatedStringHandler.AppendLiteral(", End: 0x");
      interpolatedStringHandler.AppendFormatted<long>(this.End);
      interpolatedStringHandler.AppendLiteral(" Size:");
      interpolatedStringHandler.AppendFormatted<long>(this.Size);
      return interpolatedStringHandler.ToStringAndClear();
    }

    public bool Equals(NativePtrArray other) => this.First == other.First && this.Last == other.Last && this.End == other.End;

    public override bool Equals(object obj) => obj is NativePtrArray other && this.Equals(other);

    public override int GetHashCode() => (this.First.GetHashCode() * 397 ^ this.Last.GetHashCode()) * 397 ^ this.End.GetHashCode();
  }
}
