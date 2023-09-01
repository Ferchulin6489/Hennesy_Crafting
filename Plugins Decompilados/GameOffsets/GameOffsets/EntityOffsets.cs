// Decompiled with JetBrains decompiler
// Type: GameOffsets.EntityOffsets
// Assembly: GameOffsets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 54DDBD06-CF7E-41A4-8DBF-0827C1971D02
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\GameOffsets.dll

using GameOffsets.Native;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GameOffsets
{
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct EntityOffsets
  {
    [FieldOffset(8)]
    public ObjectHeaderOffsets Head;
    [FieldOffset(8)]
    public long EntityDetailsPtr;
    [FieldOffset(16)]
    public StdVector ComponentList;
    [FieldOffset(96)]
    public uint Id;
    [FieldOffset(112)]
    public EntityFlags Flags;

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
      interpolatedStringHandler.AppendLiteral("Head: ");
      interpolatedStringHandler.AppendFormatted<ObjectHeaderOffsets>(this.Head);
      interpolatedStringHandler.AppendLiteral(" ComponentList:");
      interpolatedStringHandler.AppendFormatted<StdVector>(this.ComponentList);
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
