// Decompiled with JetBrains decompiler
// Type: GameOffsets.ObjectHeaderOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct ObjectHeaderOffsets
  {
    [FieldOffset(0)]
    public long MainObject;
    [FieldOffset(8)]
    public long Name;
    [FieldOffset(48)]
    public long ComponentLookUpPtr;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
      interpolatedStringHandler.AppendLiteral("MainObject: ");
      interpolatedStringHandler.AppendFormatted<long>(this.MainObject);
      interpolatedStringHandler.AppendLiteral(" ComponentList:");
      interpolatedStringHandler.AppendFormatted<long>(this.ComponentLookUpPtr);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
